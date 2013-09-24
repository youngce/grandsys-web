using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ENode.Commanding;
using ENode.Domain;
using Grandsys.Wfm.Services.Outsource.ServiceModel;

using NHibernate;
using DeleteEvaluationItem = Grandsys.Wfm.Services.Outsource.ServiceModel.DeleteEvaluationItem;

namespace Grandsys.Wfm.Services.Outsource.ServiceInterface
{
    public partial class EvaluationItemService : ServiceStack.ServiceInterface.Service
    {
        private readonly ICommandService _commandService;
        private readonly IMemoryCache _memoryCache;
        private readonly ISessionFactory _sessionFactory;

        public EvaluationItemService(ICommandService commandService, IMemoryCache memoryCache, ISessionFactory sessionFactory)
        {
            _commandService = commandService;
            _memoryCache = memoryCache;
            _sessionFactory = sessionFactory;
        }

        public object Get(EvaluationItems request)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                return session.QueryOver<ReadSide.Outsource.EvaluationItem>()
                       .Future()
                       .Select(o => new EvaluationItem
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
            var id = request.Id;
            var obj = _memoryCache.Get<Backend.Outsource.Domain.EvaluationItem>(new Guid(id));
            var response = new EvaluationItem
            {
                Id = request.Id,
                Name = obj.Name,
                StatisticalWay = obj.StatisticalWay,
                Formula = obj.Formula,
                Status = "inuse"
            };

            if (request.Mode == "read")
            {
                response.Links = new[]
                    {
                        new Link { Name = "DELETE", Method = "DELETE", Request = new DeleteEvaluationItem {Id = id, Name = obj.Name } },
                        new Link {Name = "Edit", Method = "GET", Request = new GetEvaluationItem {Id = id, Mode = "edit"}}
                    };
            }
            else if (request.Mode == "edit")
            {
                response.SetFormulaOptions = GetFormulaOptions(obj);
                response.Links = new[]
                    {
                        new Link { Name = "Update", Method = "PUT" },
                         new Link { Name ="Discard",  Method = "GET", Request = new GetEvaluationItem { Id = id, Mode = "read"} }
                    };
            }
            return response;
        }
    }
}