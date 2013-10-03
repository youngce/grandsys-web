using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ENode.Commanding;
using Grandsys.Wfm.Backend.Outsource.Events;

namespace Grandsys.Wfm.Backend.Outsource.Commands
{
    public class CreateEvaluationForm : Command
    {
        public EvaluationItemValue[] Items { get; set; }
    }
}
