using System;
using System.Collections.Generic;
using Grandsys.Wfm.Backend.Outsource.Events;

namespace Grandsys.Wfm.Backend.Outsource.Domain
{
    [Serializable]
    public class Linear : IFormula
    {
        private readonly double _baseIndicator;
        private readonly double _scale;
        private readonly double _decreaseStepScore;
        private readonly double _increaseStepScore;

        public Linear() { }

        public Linear(ParametersInfo values)
        {
            _baseIndicator = values.BaseIndicator;
            _scale = values.Scale;
            _decreaseStepScore = values.DecreaseStepScore;
            _increaseStepScore = values.IncreaseStepScore;
        }

        public IEnumerable<GradeStep> GenGradeSteps()
        {
            var decreaseGradeStep = new GradeStep(new Range<double>(double.MinValue, _baseIndicator), _scale) { Score = _decreaseStepScore };
            var increaseGradeStep = new GradeStep(new Range<double>(_baseIndicator, double.MaxValue), _scale) { Score = _increaseStepScore };

            return new[] { decreaseGradeStep, increaseGradeStep };
        }

        
    }
}