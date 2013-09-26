using System.Collections.Generic;
using Grandsys.Wfm.Backend.Outsource.Events;
using Grandsys.Wfm.Services.Outsource.ServiceModel;
using EvaluationItem = Grandsys.Wfm.Backend.Outsource.Domain.EvaluationItem;

namespace Grandsys.Wfm.Services.Outsource.ServiceInterface
{
    public partial class EvaluationItemService
    {
        private IEnumerable<Link> GetFormulaOptions(EvaluationItem obj)
        {
            string id = obj.Id.ToString();

            ParametersInfo value = obj.AllFormulaParams ?? new ParametersInfo
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
                    Request = new LinearFormula
                    {
                        EvaluationItemId = id,
                        BaseIndicator = value.BaseIndicator,
                        BaseScore = value.BaseScore,
                        Scale = value.Scale,
                        DecreaseStepScore = value.DecreaseStepScore,
                        IncreaseStepScore = value.IncreaseStepScore
                    }
                },
                new Link
                {
                    Name = "Slide",
                    Method = "PUT",
                    Request = new SlideFormula
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
    }
}