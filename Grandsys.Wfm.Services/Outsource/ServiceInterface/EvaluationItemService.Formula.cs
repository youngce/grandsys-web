using System.Collections.Generic;
using Grandsys.Wfm.Backend.Outsource.Events;
using Grandsys.Wfm.Backend.Outsource.Interface;
using Grandsys.Wfm.Services.Outsource.ServiceModel;
using ServiceStack.Common;
using ServiceStack.Text;
using EvaluationItem = Grandsys.Wfm.Backend.Outsource.Domain.EvaluationItem;

namespace Grandsys.Wfm.Services.Outsource.ServiceInterface
{
    public partial class EvaluationItemService
    {
        private IEnumerable<object> GetFormulaOptions(IFormula formula)
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

            var formulas = new object[]
            {
                new Backend.Outsource.Domain.Linear(defaultValue),
                new Backend.Outsource.Domain.Slide(defaultValue)
            };

            if (formula != null)
                for (var i = 0; i < formulas.Length; i++)
                {
                    if (formulas[i].GetType() == formula.GetType())
                        formulas[i] = formula;
                }

            return formulas;
        }
    }
}