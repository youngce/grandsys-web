using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ENode.Infrastructure;
using Grandsys.Wfm.Backend.Outsource.Events;
using ENode.Eventing;
using NHibernate;

namespace Grandsys.Wfm.ReadSide.Outsource
{
    public class EvaluationItem
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string StatisticalWay { get; set; }
    }

    [Component]
    public class EvaluationItemEventHandler : IEventHandler<EvaluationItemCreated>, IEventHandler<EvaluationItemDeleted>
    {
        private readonly ISessionFactory _sessionFactory;

        public EvaluationItemEventHandler(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        void IEventHandler<EvaluationItemCreated>.Handle(EvaluationItemCreated evnt)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                
                session.Save(new EvaluationItem
                {
                    Id = evnt.EvaluationItemId,
                    Name = evnt.Name,
                    StatisticalWay = evnt.StatisticalWay
                });
                session.Flush();
            }
        }

        void IEventHandler<EvaluationItemDeleted>.Handle(EvaluationItemDeleted evnt)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                session.Delete(evnt.EvaluationItemId);
                session.Flush();
            }
        }
    }
}
