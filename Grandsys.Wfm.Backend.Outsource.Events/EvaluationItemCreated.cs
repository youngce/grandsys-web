using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
    public class EvaluationItemAvailability : Event
    {
        public EvaluationItemAvailability() { }

        public EvaluationItemAvailability(Guid id, bool inuse)
        {
            EvaluationItemId = id;
            Inuse = inuse;
        }

        public EvaluationItemAvailability(Guid id, string name, string statisticalWay) :this(id, true)
        {
            Name = name;
            StatisticalWay = statisticalWay;
        }

        public string StatisticalWay { get; private set; }

        public string Name { get; private set; }

        public bool Inuse { get; private set; }

        public Guid EvaluationItemId { get; private set; }
    }

    [Serializable]
    public class EvaluationItemNameChanged : Event
    {
        public Guid ItemId { get; set; }
        public string NewName { get; set; }
            
    }

    [Serializable]
    public class EvaluationItemDescriptionChanged : Event
    {
        public Guid ItemId { get; set; }
        public IItemDescription Description { get; set; }

    }

    public interface IItemDescription
    {
        
    }
}
