using System;
using System.Collections;
using System.Collections.Generic;
using ENode.Eventing;

namespace Grandsys.Wfm.Backend.Outsource.Events
{
    public interface IFormula
    {
        object ToValue();
        ParametersInfo Parameters { get; }
        //IEnumerable<GradeStep> GenGradeSteps();
    }

    [Serializable]
    public class EvaluationItemFormulaChanged : Event
    {
        public IFormula Formula { get; set; }
        public string Description { get; set; }
    }

    [Serializable]
    public class ParametersInfo
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