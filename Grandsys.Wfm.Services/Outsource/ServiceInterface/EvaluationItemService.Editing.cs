using Grandsys.Wfm.Services.Outsource.ServiceModel;
using System;
using System.Linq;

namespace Grandsys.Wfm.Services.Outsource.ServiceInterface
{
    public partial class EvaluationItemService
    {
        public object Patch(UpdateEvaluationItem request)
        {
            if (request.Formula != null)
            {
                var formulaInfo = request.Formula;
                var formulaType = formulaInfo.Type.ToLower();

                if (formulaType == "linear")
                {
                    _commandService.Execute(new Backend.Outsource.Commands.SetLinearFormula()
                        {
                            EvaluationItemId = new Guid(request.Id),
                            BaseIndicator = formulaInfo.BaseIndicator,
                            Scale = formulaInfo.Scale,
                            BaseScore = formulaInfo.BaseScore,
                            IncreaseStepScore = formulaInfo.IncreaseStepScore,
                            DecreaseStepScore = formulaInfo.DecreaseStepScore
                        });
                }
                else if (formulaType == "slide")
                {
                    _commandService.Execute(new Backend.Outsource.Commands.SetSlideFormula()
                    {
                        EvaluationItemId = new Guid(request.Id),
                        BaseIndicator = formulaInfo.BaseIndicator,
                        Scale = formulaInfo.Scale,
                        BaseScore = formulaInfo.BaseScore,
                        StepScore = formulaInfo.StepScore,
                        StartIndicator = formulaInfo.StartIndicator,
                        FinalIndicator = formulaInfo.FinalIndicator
                    });
                }
            }
            return Get(new GetEvaluationItem() { Id = request.Id, Mode = "read" });
        }

        public object Put(UpdateEvaluationItem request)
        {
            _commandService.Execute(new Grandsys.Wfm.Backend.Outsource.Commands.EnableEvaluationItem { ItemId = new Guid(request.Id) });
            return Get(new GetEvaluationItem() { Id = request.Id, Mode = "read" });
        }

        public object Delete(ServiceModel.DeleteEvaluationItem request)
        {
            var result = _commandService.Execute(new Grandsys.Wfm.Backend.Outsource.Commands.DeleteEvaluationItem { ItemId = new Guid(request.Id) });
            if (result.IsCompleted)
            {
                return new EvaluationItem
                {
                    Id = request.Id,
                    Name = request.Name,
                    Status = "deleted",
                    Links = new[]
                    {
                        new Link { Name = "Undo", Method = "PUT", Request = new UpdateEvaluationItem { Id = request.Id} }
                    }
                };
            }
            else
                throw result.ErrorInfo.Exception;
        }
    }
}