using Grandsys.Wfm.Services.Outsource.ServiceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Grandsys.Wfm.Services.Outsource.ServiceInterface
{
    public partial class EvaluationItemService
    {
        public object Get(EvaluationItemsCreationWays request)
        {
            return new EvaluationItemsCreationWays
            {
                Links = new[]
                {
                    new Link { Name = "By Piece", Method = "POST", Request = new PieceEvaluationItem() },
                    new Link { Name = "By Ratio", Method = "POST", Request = new RatioEvaluationItem() }
                }
            };
        }

        public object Post(RatioEvaluationItem request)
        {
            var evaluationItemId = Guid.NewGuid();
            var result =_commandService.Execute(new Grandsys.Wfm.Backend.Outsource.Commands.CreateRatioEvaluationItem { EvaluationItemId = evaluationItemId, Name = request.Name });

            var id = evaluationItemId.ToString();

            return new EvaluationItem
            {
                Id = id,
                Name = request.Name,
                StatisticalWay = "ratio",
                Links = new[] { new Link { Name = "NEXT", Method = "GET", Request = new GetEvaluationItem() { Id = id, Mode = "edit" } } }
            };
        }

        public object Post(PieceEvaluationItem request)
        {
            var evaluationItemId = Guid.NewGuid();
           var result =  _commandService.Execute(new Grandsys.Wfm.Backend.Outsource.Commands.CreatePieceEvaluationItem { EvaluationItemId = evaluationItemId, Name = request.Name });

            var id = evaluationItemId.ToString();

            return new EvaluationItem
            {
                Id = id,
                Name = request.Name,
                StatisticalWay = "piece",
                Links = new[] { new Link { Name = "NEXT", Method = "GET", Request = new GetEvaluationItem() { Id = id, Mode = "edit" } } }
            };
        }
    }
}