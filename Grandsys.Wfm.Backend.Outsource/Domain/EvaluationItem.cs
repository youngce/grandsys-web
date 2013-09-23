using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ENode.Domain;
using ENode.Eventing;
using Grandsys.Wfm.Backend.Outsource.Events;

namespace Grandsys.Wfm.Backend.Outsource.Domain
{
    public class LessThanOrEqualToZeroException : Exception { }

    [Serializable]
    public class Formula
    {
        private readonly Func<IEnumerable<GradeStep>, IEnumerable<GradeStep>> _gradeStepsCreation;

        public Formula(Func<IEnumerable<GradeStep>, IEnumerable<GradeStep>> gradeStepsCreation)
        {
            _gradeStepsCreation = gradeStepsCreation;
        }

        public IEnumerable<GradeStep> CreateGradeSteps(IEnumerable<GradeStep> gradeSteps)
        {
            return _gradeStepsCreation(gradeSteps);
        }
    }

    public static class StatisticalWay
    {
        private static Formula ByRatio()
        {
            return new Formula(gradeSteps => new NonOverlappingGradeSteps(gradeSteps));
        }

        private static void IsNotIntergerThrow(double number)
        {
            if (number != Math.Truncate(number))
                throw new InvalidConstraintException();
        }

        private static Formula ByPiece()
        {
            return new Formula(gradeSteps =>
                {
                    foreach (var gradeStep in gradeSteps)
                        IsNotIntergerThrow(gradeStep.Scale);

                    return new NonOverlappingGradeSteps(gradeSteps);
                });
        }

        public static Formula By(string way)
        {
            if (way == "ratio")
                return ByRatio();
            if (way == "piece")
                return ByPiece();

            throw new NotImplementedException();
        }
    }

    public class DefaultValue
    {
        public double BaseIndicator { get; private set; }
        public double Scale { get; private set; }
        public DefaultValue(double baseIndicator, double scale)
        {
            BaseIndicator = baseIndicator;
            Scale = scale;
        }

        public IList<GradeStep> Create()
        {
            var decreaseGradeStep = new GradeStep(new Range<double>(double.MinValue, BaseIndicator), Scale);
            var increaseGradeStep = new GradeStep(new Range<double>(BaseIndicator, double.MaxValue), Scale);
            return new[] { decreaseGradeStep, increaseGradeStep };
        }
    }

    //public class BaseIndicator
    //{
    //    public double Score { get; set; }
    //    public double Value { get; set; }
    //}

    //public class GradeStepDot
    //{

    //}

    //public class Line
    //{
    //    private SortedList<double, GradeStepDot> _dots;
    //    public Line()
    //    {
    //        _dots = new SortedList<double, GradeStepDot>
    //            {
    //                {double.MinValue, new GradeStepDot()},
    //                {double.MaxValue, new GradeStepDot()},
    //            };
    //    }
    //}

    [Serializable]
    public class NonOverlappingGradeSteps : IEnumerable<GradeStep>
    {
        private IList<GradeStep> _gradeSteps;

        public NonOverlappingGradeSteps(IEnumerable<GradeStep> gradeSteps)
        {
            _gradeSteps = gradeSteps.ToArray();
        }

        public IEnumerator<GradeStep> GetEnumerator()
        {
            return _gradeSteps.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    [Serializable]
    public class GradeStep
    {
        private readonly Range<double> _range;
        private double _scale;

        public GradeStep(Range<double> range, double scale)
        {
            _range = range;
            Scale = scale;
        }

        public double Score { get; set; }

        // public Range<double> Range { get { return _range; } }

        public double Scale
        {
            get { return _scale; }
            private set
            {
                _scale = value;
                if (_scale <= 0)
                    throw new LessThanOrEqualToZeroException();
            }
        }
    }

    [Serializable]
    public class Range<T> where T : IComparable
    {
        private readonly T _min;
        private readonly T _max;

        public T Max { get { return _max; } }
        public T Min { get { return _min; } }

        public Range(T min, T max)
        {
            if (max.CompareTo(min) <= 0)
                throw new ArgumentException();
            _min = min;
            _max = max;
        }
    }

    [Serializable]
    public class EvaluationItem : AggregateRoot<Guid>
        , IEventHandler<EvaluationItemCreated>, IEventHandler<EvaluationItemAvailability>
    {
        private Formula _formula;
        private string _statisticalWay;
        private string _name;
        private bool _inuse;

        public EvaluationItem() { }

        public EvaluationItem(Guid evaluationItemId, string name, string statisticalWay)
            : base(evaluationItemId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("name can not be null or whiteSpace");

            RaiseEvent(new EvaluationItemCreated(evaluationItemId, name, statisticalWay));
        }

        void IEventHandler<EvaluationItemCreated>.Handle(EvaluationItemCreated evnt)
        {
            _inuse = true;
            _name = evnt.Name;
            _statisticalWay = evnt.StatisticalWay;
            
            _formula = Domain.StatisticalWay.By(evnt.StatisticalWay);
        }

        public void ChangeGradeSteps(IEnumerable<GradeStep> gradeSteps)
        {
            //_formula.SetGradeSteps(gradeSteps);
        }

        public void Disable()
        {
            if (!_inuse)
                throw new Exception(string.Format("The item '{0}' is already disabled.", _name));

            RaiseEvent(new EvaluationItemAvailability(Id, false));
        }

        public void Enable()
        {
            if(_inuse)
                throw new Exception(string.Format("The item '{0}' is already enabled.", _name));

            RaiseEvent(new EvaluationItemAvailability(Id, _name, _statisticalWay));
        }

        void IEventHandler<EvaluationItemAvailability>.Handle(EvaluationItemAvailability evnt)
        {
            _inuse = evnt.Inuse;
        }

        public string Name { get { return _name; } }
        public string StatisticalWay { get { return _statisticalWay; } }
    }
}
