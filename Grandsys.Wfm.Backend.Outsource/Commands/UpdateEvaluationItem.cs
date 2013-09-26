using ENode.Commanding;
using System;

namespace Grandsys.Wfm.Backend.Outsource.Commands
{
    [Serializable]
    public class DeleteEvaluationItem : Command
    {
        public Guid ItemId { get; set; }
    }

    [Serializable]
    public class EnableEvaluationItem : Command
    {
        public Guid ItemId { get; set; }
    }

    [Serializable]
    public class SetEvaluationItemDescription : Command
    {
        public Guid ItemId { get; set; }

        public string Title { get; set; }
        public string Unit { get; set; }
        public string Denominator { get; set; }
        public string Numerator { get; set; }
        
    }
}