using System.Collections.Generic;
using Grandsys.Wfm.Backend.Outsource.Events;
using Grandsys.Wfm.Services.Outsource.ServiceModel;
using NHibernate.Dialect.Function;
using ServiceStack.Common;
using EvaluationItem = Grandsys.Wfm.Backend.Outsource.Domain.EvaluationItem;

namespace Grandsys.Wfm.Services.Outsource.ServiceInterface
{
    public partial class EvaluationItemService
    {
        private IEnumerable<Link> GetFormulaOptions(EvaluationItem obj)
        {
            var defaultValue = new ParametersInfo
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

            var f1 = new LinearFormula().PopulateWith(defaultValue);
            var f2 = new SlideFormula().PopulateWith(defaultValue);

            if (obj.FormulaParams != null)
            {
                f1.PopulateWith(obj.FormulaParams);
                f2.PopulateWith(obj.FormulaParams);
            }

            var links = new[]
            {
                new Link
                {
                    Name = "Linear",
                    Method = "PUT",
                    Request = f1
                },
                new Link
                {
                    Name = "Slide",
                    Method = "PUT",
                    Request = f2
                }
            };

            return links;
        }
    }
}