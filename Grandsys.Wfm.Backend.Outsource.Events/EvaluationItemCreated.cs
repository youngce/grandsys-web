using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ENode.Eventing;

namespace Grandsys.Wfm.Backend.Outsource.Events
{
    [Serializable]
    public class EvaluationItemCreated : Event
    {
        public EvaluationItemCreated(Guid evaluationItemId, string name, string statisticalWay)
        {
            StatisticalWay = statisticalWay;
            Name = name;
            EvaluationItemId = evaluationItemId;
        }

        public Guid EvaluationItemId { get; private set; }

        public string StatisticalWay { get; private set; }

        public string Name { get; private set; }
    }

    [Serializable]
    public class EvaluationItemDeleted : Event
    {
        public EvaluationItemDeleted(Guid id)
        {
            EvaluationItemId = id;
        }

        public Guid EvaluationItemId { get; private set; }
    }
}
