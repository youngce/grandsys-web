using System.Collections.Generic;
using Grandsys.Wfm.Backend.Outsource.Events;
using Grandsys.Wfm.Services.Outsource.ServiceModel;
using ServiceStack.Common;
using ServiceStack.Text;
using EvaluationItem = Grandsys.Wfm.Backend.Outsource.Domain.EvaluationItem;

namespace Grandsys.Wfm.Services.Outsource.ServiceInterface
{
    public partial class EvaluationItemService
    {
        private IEnumerable<string> GetFormulaOptions(EvaluationItem obj)
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
            if (obj.FormulaParams != null)
            {
                defaultValue.PopulateWith(obj.FormulaParams);
            }


            return new []
            {
                new
                {
                    defaultValue.BaseIndicator,
                    defaultValue.BaseScore,
                    defaultValue.Scale,
                    defaultValue.IncreaseStepScore,
                    defaultValue.DecreaseStepScore
                }.ToJson(),
                new
                {
                    defaultValue.BaseIndicator,
                    defaultValue.BaseScore,
                    defaultValue.Scale,
                    defaultValue.StepScore,
                    defaultValue.StartIndicator,
                    defaultValue.FinalIndicator
                }.ToJson()
            };
        }
    }
}