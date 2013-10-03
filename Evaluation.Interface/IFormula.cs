using System;
using System.Collections.Generic;

namespace Grandsys.Wfm.Backend.Outsource.Interface
{
    public interface IFormula
    {
        ParametersInfo Parameters { get; }
        IEnumerable<GradeStep> GenGradeSteps();
    }

    [Serializable]
    public struct ParametersInfo
    {

        public double BaseIndicator { get; set; }
        public double BaseScore { get; set; }
        public double Scale { get; set; }

        //linear
        public double IncreaseStepScore { get; set; }
        public double DecreaseStepScore { get; set; }

        //slide
        public double StepScore { get; set; }
        public double StartIndicator { get; set; }
        public double FinalIndicator { get; set; }
    }
}
