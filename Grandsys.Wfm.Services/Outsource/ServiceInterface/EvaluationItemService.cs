using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ENode.Commanding;
using ENode.Domain;
using Grandsys.Wfm.Services.Outsource.ServiceModel;
using Grandsys.Wfm.Backend.Outsource.Commands;
using NHibernate;
using DeleteEvaluationItem = Grandsys.Wfm.Services.Outsource.ServiceModel.DeleteEvaluationItem;

namespace Grandsys.Wfm.Services.Outsource.ServiceInterface
{
    public class EvaluationItemService : ServiceStack.ServiceInterface.Service
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

        public object Get(EvaluationItemsCreationWays request)
        {
            return new EvaluationItemsCreationWays
            {
                Links = new[]
                        {
                            new Link { Name = "By Piece", Method = "POST", Request = new PieceEvaluationItem()},
                            new Link { Name = "By Ratio", Method = "POST", Request = new RatioEvaluationItem()}
                        }
            };
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

            using (var session = _sessionFactory.OpenSession())
            {
                var obj = session.Get<ReadSide.Outsource.EvaluationItem>(new Guid(id));
                var response = new EvaluationItem
                {
                    Id = request.Id,
                    Name = obj.Name,
                    StatisticalWay = obj.StatisticalWay,
                    Status = "inuse"
                };

                if (request.Mode == "read")
                {
                    response.Links = new[]
                    {
                        new Link { Name = "DELETE", Method = "DELETE", Request = new DeleteEvaluationItem {Id = id} },
                        new Link {Name = "Edit", Method = "GET", Request = new GetEvaluationItem {Id = id, Mode = "edit"}}
                    };
                }
                else if (request.Mode == "edit")
                {
                    response.SetFormulaOptions = new[] { LinearFormula(request.Id), SlideFormula(request.Id) };
                    response.Links = new[]
                    {
                         new Link { Name ="Update",  Method = "PUT", Request = new UpdateEvaluationItem { Id = id, Name = obj.Name, Status = "inuse" } },
                         new Link { Name ="Discard",  Method = "GET", Request = new GetEvaluationItem { Id = id, Mode = "read"} }
                    };
                }
                return response;
            }
        }

        public object Post(RatioEvaluationItem request)
        {
            var evaluationItemId = Guid.NewGuid();
            _commandService.Send(new CreateRatioEvaluationItem { EvaluationItemId = evaluationItemId, Name = request.Name });

            var response = CreateEvaluationItem(evaluationItemId.ToString());
            response.Name = request.Name;
            response.StatisticalWay = "ratio";
            return response;
        }

        public object Post(PieceEvaluationItem request)
        {
            var evaluationItemId = Guid.NewGuid();
            _commandService.Send(new CreatePieceEvaluationItem { EvaluationItemId = evaluationItemId, Name = request.Name });

            var response = CreateEvaluationItem(evaluationItemId.ToString());
            response.Name = request.Name;
            response.StatisticalWay = "piece";
            return response;
        }

        static EvaluationItem CreateEvaluationItem(string evaluationItemId)
        {
            return new EvaluationItem
            {
                Id = evaluationItemId,
                SetFormulaOptions = new[] { LinearFormula(evaluationItemId), SlideFormula(evaluationItemId) },
                Links = new[] { new Link{ Name ="NEXT" , Method = "GET", Request = new GetEvaluationItem(){ Id = evaluationItemId, Mode ="edit" } }}
            };
        }

        static Link LinearFormula(string evaluationItemId)
        {
            return new Link
                {
                    Name = "Linear",
                    Method = "POST",
                    Request = new LinearFormula() { EvaluationItemId = evaluationItemId }
                };
        }

        static Link SlideFormula(string evaluationItemId)
        {
            return new Link
            {
                Name = "Slide",
                Method = "POST",
                Request = new SlideFormula() { EvaluationItemId = evaluationItemId }
            };
        }

        public object Put(UpdateEvaluationItem rquest)
        {
            if (rquest.Status == "inuse")
            {
                return Get(new GetEvaluationItem { Id = rquest.Id, Mode = "read" });
            }

            return new EvaluationItem { Name = rquest.Name, Status = "inuse" };
        }

        public object Any(SlideFormula request)
        {
            throw new NotImplementedException();
        }

        public object Any(LinearFormula request)
        {
            throw new NotImplementedException();
        }


        public object Delete(ServiceModel.DeleteEvaluationItem request)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                var obj = session.Get<ReadSide.Outsource.EvaluationItem>(new Guid(request.Id));

                return new EvaluationItem
                {
                    Id = request.Id,
                    Name = obj.Name,
                    Status = "deleted",
                    Links = new[]{ 
                    new Link{ Name ="Undo", Method = "PUT", Request = new UpdateEvaluationItem{ Id = request.Id, Status = "inuse" }}}
                };

            }
        }
    }
}