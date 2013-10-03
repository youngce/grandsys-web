using System;
using System.Collections;
using System.Collections.Generic;
using ENode.Eventing;
using Grandsys.Wfm.Backend.Outsource.Interface;

namespace Grandsys.Wfm.Backend.Outsource.Events
{
    [Serializable]
    public class EvaluationItemFormulaChanged : Event
    {
        public IFormula Formula { get; set; }
        public string Description { get; set; }
    }
}