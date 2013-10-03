using ENode.Eventing;
using System;

namespace Grandsys.Wfm.Backend.Outsource.Events
{
    [Serializable]
    public class EvaluationFormCreated : Event
    {
        public EvaluationItemValue[] EvaluationItems { get; set; }
    }
}
