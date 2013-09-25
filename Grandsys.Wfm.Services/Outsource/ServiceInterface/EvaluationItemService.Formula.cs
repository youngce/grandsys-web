using Grandsys.Wfm.Services.Outsource.ServiceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Grandsys.Wfm.Services.Outsource.ServiceInterface
{
    public partial class EvaluationItemService
    {
        public object Any(SlideFormula request)
        {
            _commandService.Execute(new Backend.Outsource.Commands.SetSlideFormula()
            {
                EvaluationItemId = new Guid(request.EvaluationItemId),
                BaseIndicator = request.BaseIndicator,
                Scale = request.Scale,
                BaseScore = request.BaseScore,
                StepScore = request.StepScore,
                StartIndicator = request.StartIndicator,
                FinalIndicator = request.FinalIndicator
            });

            return Get(new GetEvaluationItem() { Id = request.EvaluationItemId, Mode = "read" });
        }

        public object Any(LinearFormula request)
        {
            _commandService.Execute(new Backend.Outsource.Commands.SetLinearFormula()
            {
                EvaluationItemId = new Guid(request.EvaluationItemId),
                BaseIndicator = request.BaseIndicator,
                Scale = request.Scale,
                BaseScore = request.BaseScore,
                IncreaseStepScore = request.IncreaseStepScore,
                DecreaseStepScore = request.DecreaseStepScore
            });

            return Get(new GetEvaluationItem() { Id = request.EvaluationItemId, Mode = "read" });
        }

        private IEnumerable<Link> GetFormulaOptions(Backend.Outsource.Domain.EvaluationItem obj)
        {
            var id = obj.Id.ToString();
            
            var value = obj.FormulaParams ?? new Grandsys.Wfm.Backend.Outsource.Events.ParametersInfo()
            {
                BaseIndicator = 0,
                BaseScore = 100,
                Scale = 1,
                DecreaseStepScore = 0,
                IncreaseStepScore = 0,
                StepScore = -5,
                StartIndicator = 3,
                FinalIndicator = 14
            };

            var links = new[]
            {
                new Link
                {
                    Name = "Linear",
                    Method = "PUT",
                    Request = new LinearFormula()
                    {
                        EvaluationItemId = id,
                        BaseIndicator = value.BaseIndicator,
                        BaseScore = value.BaseScore,
                        Scale = value.Scale,
                        DecreaseStepScore = value.DecreaseStepScore,
                        IncreaseStepScore = value.IncreaseStepScore
                    }
                }, new Link
                   {
                       Name = "Slide",
                       Method = "PUT",
                       Request = new SlideFormula()
                       {
                           EvaluationItemId = id,
                           BaseIndicator = value.BaseIndicator,
                           BaseScore = value.BaseScore,
                           Scale = value.Scale,
                           StepScore = value.StepScore,
                           StartIndicator = value.StartIndicator,
                           FinalIndicator = value.FinalIndicator
                       }
                   }
            };

            return links;
        }

        //private Link GetUpdateLink(Backend.Outsource.Domain.EvaluationItem obj)
        //{
        //    if (string.IsNullOrEmpty(obj.Formula))
        //        return new Link { Name = "Update", Method = "PUT" };

        //    var link = GetFormulaOptions(obj).FirstOrDefault(o => obj.Formula.ToLower().Contains(o.Name.ToLower()));

        //    if (link == null)
        //        return new Link { Name = "Update", Method = "PUT" };
        //    link.Name = "Update";
        //    return link;
        //}
    }
}