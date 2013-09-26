using System;
using System.Collections.Generic;
using Grandsys.Wfm.Backend.Outsource.Events;

namespace Grandsys.Wfm.Backend.Outsource.Domain
{
    [Serializable]
    public class Slide : IFormula
    {
        private readonly double _baseIndicator;
        private readonly double _startIndicator;
        private readonly double _finalIndicator;
        private readonly double _stepScore;
        private readonly double _scale;
        private readonly double _baseScore;


        //private readonly object _valueObject;

        public Slide() { }

        public Slide(ParametersInfo values)
        {
            _baseIndicator = values.BaseIndicator;
            _startIndicator = values.StartIndicator + _scale;
            _finalIndicator = values.FinalIndicator;
            _stepScore = values.StepScore;
            _scale = values.Scale;
            _baseScore = values.BaseScore;

            if (!IsValid())
                throw new ArgumentException();
        }

        private bool IsValid()
        {
            if (Math.Abs(_startIndicator) >= Math.Abs(_finalIndicator))
                return false;

            if (_startIndicator <= _baseIndicator && _baseIndicator < _finalIndicator)
                return false;

            return true;
        }

        private double[] DecideStepScores()
        {
            var stepScores = new double[4];
            if (_baseIndicator < _startIndicator)
            {
                stepScores[0] = stepScores[1] = 0;
                stepScores[2] = _stepScore;
                stepScores[3] = double.MinValue;
            }
            if (_baseIndicator > _startIndicator)
            {
                stepScores[0] = double.MinValue;
                stepScores[1] = _stepScore;
                stepScores[2] = stepScores[3] = 0;
            }
            return stepScores;
        }

        public IEnumerable<GradeStep> GenGradeSteps()
        {
            var stepScores = DecideStepScores();

            var indicator = new double[] { double.MinValue, double.MaxValue, _baseIndicator, _startIndicator, _finalIndicator };
            Array.Sort(indicator);

            var result = new List<GradeStep>();
            var sortIndicator = new LinkedList<double>(indicator);

            var current = sortIndicator.First;
            var index = 0;
            while (current != null)
            {
                if (current.Next == null)
                    break;
                result.Add(new GradeStep(new Range<double>(current.Value, current.Next.Value), _scale) { Score = stepScores[index] });

                current = current.Next;
                index += 1;
            }
            return result;
        }

        public object ToValue()
        {
            return new
            {
                BaseIndicator = _baseIndicator,
                BaseScore = _baseScore,
                StartIndicator = (_startIndicator - _scale),
                FinalIndicator = _finalIndicator,
                StepScore = _stepScore,
                Scale = _scale
            };
        }
    }
}