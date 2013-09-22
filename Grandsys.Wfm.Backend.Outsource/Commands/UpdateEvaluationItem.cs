using ENode.Commanding;
using System;

namespace Grandsys.Wfm.Backend.Outsource.Commands
{
    [Serializable]
    public class UpdateBaseIndicatorEvaluationItem : Command
    {
        public double Value { get; set; }
        public double Scale { get; set; }
    }

    [Serializable]
    public class DeleteEvaluationItem : Command
    {
        public Guid ItemId { get; set; }
    }
}