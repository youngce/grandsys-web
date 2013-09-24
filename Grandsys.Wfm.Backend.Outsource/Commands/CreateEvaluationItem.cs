using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ENode.Commanding;

namespace Grandsys.Wfm.Backend.Outsource.Commands
{
    [Serializable]
    public class CreateRatioEvaluationItem : Command
    {
        public Guid EvaluationItemId { get; set; }
        public string Name { get; set; }
        //public double BaseIndicator { get; set; }
        //public double Scale { get; set; }
    }

    [Serializable]
    public class CreatePieceEvaluationItem : Command
    {
        public Guid EvaluationItemId { get; set; }
        public string Name { get; set; }
        //public int BaseIndicator { get; set; }
        //public double Scale { get; set; }
    }

    [Serializable]
    public class SetLinearFormula :Command
    {
        public Guid EvaluationItemId { get; set; }

        public double BaseIndicator { get; set; }
        public double BaseScore { get; set; }
        public double Scale { get; set; }

        public double IncreaseStepScore { get; set; }
        public double DecreaseStepScore { get; set; }
    }

    [Serializable]
    public class SetSlideFormula : Command
    {
        public Guid EvaluationItemId { get; set; }

        public double BaseIndicator { get; set; }
        public double BaseScore { get; set; }
        public double Scale { get; set; }

        public double StepScore { get; set; }
        public double StartIndicator { get; set; }
        public double FinalIndicator { get; set; }
    }
}
