using System;
using System.Linq;
using ENode.Commanding;
using ENode.Domain;
using Grandsys.Wfm.ReadSide.Outsource;
using Grandsys.Wfm.Services.Outsource.ServiceModel;
using NHibernate;
using ServiceStack.ServiceInterface;

namespace Grandsys.Wfm.Services.Outsource.ServiceInterface
{
    public partial class EvaluationItemService : Service
    {
        private readonly ICommandService _commandService;
        private readonly IMemoryCache _memoryCache;
        private readonly ISessionFactory _sessionFactory;

        public EvaluationItemService(ICommandService commandService, IMemoryCache memoryCache,
            ISessionFactory sessionFactory)
        {
            _commandService = commandService;
            _memoryCache = memoryCache;
            _sessionFactory = sessionFactory;
        }

        public object Get(EvaluationItems request)
        {
            using (ISession session = _sessionFactory.OpenSession())
            {
                return session.QueryOver<EvaluationItem>()
                    .Future()
                    .Select(o => new ResponseEvaluationItem
                    {
                        Id = o.Id.ToString(),
                        Name = o.Name,
                        StatisticalWay = o.StatisticalWay,
                    })
                    .ToArray();
            }
        }

        public object Get(GetEvaluationItem request)
        {
            string id = request.Id;
            var obj = _memoryCache.Get<Backend.Outsource.Domain.EvaluationItem>(new Guid(id));
            var response = new ResponseEvaluationItem
            {
                Id = request.Id,
                Name = obj.Name,
                StatisticalWay = obj.StatisticalWay,
                Description = obj.Description,
                Status = "inuse"
            };

            if (obj.Formula != null)
            {
                response.FormulaParams = obj.Formula;
            }

            if (request.Mode == "read")
            {
                response.Links = new[]
                {
                    new Link
                    {
                        Name = "DELETE",
                        Method = "DELETE",
                        Request = new DeleteEvaluationItem {Id = id, Name = obj.Name}
                    },
                    new Link {Name = "Edit", Method = "GET", Request = new GetEvaluationItem {Id = id, Mode = "edit"}}
                };
            }
            else if (request.Mode == "edit")
            {
                response.FormulaOptions = GetFormulaOptions(obj);
                response.Links = new[]
                {
                    new Link {Name = "Update", Method = "PATCH", Request = new UpdateEvaluationItem {Id = id}},
                    new Link
                    {
                        Name = "Discard",
                        Method = "GET",
                        Request = new GetEvaluationItem {Id = id, Mode = "read"}
                    }
                };
            }
            return response;
        }
    }
}