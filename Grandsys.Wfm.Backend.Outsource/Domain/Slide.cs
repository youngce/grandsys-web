using System;
using System.Collections.Generic;
using Grandsys.Wfm.Backend.Outsource.Events;

namespace Grandsys.Wfm.Backend.Outsource.Domain
{
    [Serializable]
    public class Slide : IFormula
    {
        private  double _baseIndicator;
        private  double _startIndicator;
        private  double _finalIndicator;
        private  double _stepScore;
        private  double _scale;
        private ParametersInfo _parameters;

        public Slide() { }

        public Slide(ParametersInfo values)
        {
            Parameters = values;
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
                _parameters.BaseIndicator,
                _parameters.BaseScore,
                _parameters.StartIndicator,
                _parameters.FinalIndicator ,
                _parameters.StepScore,
                _parameters.Scale
            };
        }

        public ParametersInfo Parameters
        {
            get
            {
                return _parameters;
            }
            private set
            {
                _baseIndicator = value.BaseIndicator;
                _startIndicator = value.StartIndicator + _scale;
                _finalIndicator = value.FinalIndicator;
                _stepScore = value.StepScore;
                _scale = value.Scale;

                if (!IsValid())
                    throw new ArgumentException();
                _parameters = value;
            }
        }
    }
}