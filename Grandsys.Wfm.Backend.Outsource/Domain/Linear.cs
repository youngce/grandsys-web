using System;
using System.Collections.Generic;
using Grandsys.Wfm.Backend.Outsource.Events;

namespace Grandsys.Wfm.Backend.Outsource.Domain
{
    [Serializable]
    public class Linear : IFormula
    {
        private double _baseIndicator;
        private double _scale;
        private double _decreaseStepScore;
        private double _increaseStepScore;
        private ParametersInfo _parameters;


        public Linear() { }

        public Linear(ParametersInfo values)
        {
            Parameters = values;
        }

        public IEnumerable<GradeStep> GenGradeSteps()
        {
            var decreaseGradeStep = new GradeStep(new Range<double>(double.MinValue, _baseIndicator), _scale) { Score = _decreaseStepScore };
            var increaseGradeStep = new GradeStep(new Range<double>(_baseIndicator, double.MaxValue), _scale) { Score = _increaseStepScore };

            return new[] { decreaseGradeStep, increaseGradeStep };
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
                _scale = value.Scale;
                _decreaseStepScore = value.DecreaseStepScore;
                _increaseStepScore = value.IncreaseStepScore;
                _parameters = value;
            }
        }

        public double BaseIndicator { get { return _parameters.BaseIndicator; } }
        public double BaseScore { get { return _parameters.BaseScore; } }
        public double Scale { get { return _parameters.Scale; } }

        //linear
        public double IncreaseStepScore { get { return _parameters.IncreaseStepScore; } }
        public double DecreaseStepScore { get { return _parameters.DecreaseStepScore; } }

        public string Type { get { return "linear"; } }
    }
}