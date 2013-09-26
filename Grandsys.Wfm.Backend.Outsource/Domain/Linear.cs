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
        private double _baseScore;
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


        public object ToValue()
        {
            return new
            {
                BaseIndicator = _baseIndicator,
                BaseScore = _baseScore,
                Scale = _scale,
                DecreaseStepScore = _decreaseStepScore,
                IncreaseStepScore = _increaseStepScore
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
                _scale = value.Scale;
                _decreaseStepScore = value.DecreaseStepScore;
                _increaseStepScore = value.IncreaseStepScore;
                _baseScore = value.BaseScore;
                _parameters = value;
            }
        }
    }
}