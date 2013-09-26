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
    public class EvaluationItemEventHandler : IEventHandler<EvaluationItemCreated>, IEventHandler<EvaluationItemAvailability>, IEventHandler<EvaluationItemNameChanged>
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

        void IEventHandler<EvaluationItemAvailability>.Handle(EvaluationItemAvailability evnt)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                if (evnt.Inuse)
                {
                    session.Save(new EvaluationItem
                    {
                        Id = evnt.EvaluationItemId,
                        Name = evnt.Name,
                        StatisticalWay = evnt.StatisticalWay
                    });
                }
                else
                    session.Delete(session.Load<EvaluationItem>(evnt.EvaluationItemId));

                session.Flush();
            }
        }

        public void Handle(EvaluationItemNameChanged evnt)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                var obj = session.Get<EvaluationItem>(evnt.ItemId);
                if (obj == null)
                    return;

                obj.Name = evnt.NewName;
                session.Update(obj);
                session.Flush();
            }
        }
    }
}
