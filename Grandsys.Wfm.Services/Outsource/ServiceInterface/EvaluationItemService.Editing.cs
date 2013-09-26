using System;
using ENode.Commanding;
using Grandsys.Wfm.Backend.Outsource.Commands;
using Grandsys.Wfm.Services.Outsource.ServiceModel;
using DeleteEvaluationItem = Grandsys.Wfm.Services.Outsource.ServiceModel.DeleteEvaluationItem;

namespace Grandsys.Wfm.Services.Outsource.ServiceInterface
{
    public partial class EvaluationItemService
    {
        public object Patch(UpdateEvaluationItem request)
        {
            if (request.Formula != null)
            {
                FormulaInfo formulaInfo = request.Formula;
                string formulaType = formulaInfo.Type.ToLower();

                if (formulaType == "linear")
                {
                    _commandService.Execute(new SetLinearFormula
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
                    _commandService.Execute(new SetSlideFormula
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
            string newName = request.Name;
            if (!string.IsNullOrEmpty(newName))
            {
                _commandService.Execute(new ChangeEvaluationItemName
                {
                    ItemId = new Guid(request.Id),
                    NewName = request.Name
                });
            }

            if (request.Description != null)
            {
                _commandService.Execute(new SetEvaluationItemDescription
                {
                    ItemId = new Guid(request.Id),
                    Denominator = request.Description.Denominator,
                    Numerator = request.Description.Numerator,
                    Title = request.Description.Title,
                    Unit = request.Description.Unit
                });
            }

            return Get(new GetEvaluationItem {Id = request.Id, Mode = "read"});
        }

        public object Put(UpdateEvaluationItem request)
        {
            _commandService.Execute(new EnableEvaluationItem {ItemId = new Guid(request.Id)});
            return Get(new GetEvaluationItem {Id = request.Id, Mode = "read"});
        }

        public object Delete(DeleteEvaluationItem request)
        {
            CommandAsyncResult result =
                _commandService.Execute(new Backend.Outsource.Commands.DeleteEvaluationItem
                {
                    ItemId = new Guid(request.Id)
                });
            if (result.IsCompleted)
            {
                return new EvaluationItem
                {
                    Id = request.Id,
                    Name = request.Name,
                    Status = "deleted",
                    Links = new[]
                    {
                        new Link {Name = "Undo", Method = "PUT", Request = new UpdateEvaluationItem {Id = request.Id}}
                    }
                };
            }
            throw result.ErrorInfo.Exception;
        }
    }
}