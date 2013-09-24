using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ENode.Commanding;
using ENode.Infrastructure;
using Grandsys.Wfm.Backend.Outsource.Commands;
using Grandsys.Wfm.Backend.Outsource.Domain;
using Grandsys.Wfm.Backend.Outsource.Events;

namespace Grandsys.Wfm.Backend.Outsource.CommandHandlers
{
    [Component]
    public class EvaluationItemCommandHandler :
        ICommandHandler<CreatePieceEvaluationItem>,
        ICommandHandler<CreateRatioEvaluationItem>,
        ICommandHandler<SetLinearFormula>,
        ICommandHandler<DeleteEvaluationItem>, ICommandHandler<EnableEvaluationItem>, ICommandHandler<SetSlideFormula>
    {
        void ICommandHandler<CreatePieceEvaluationItem>.Handle(ICommandContext context, CreatePieceEvaluationItem command)
        {
            context.Add(new EvaluationItem(command.EvaluationItemId, command.Name, "piece"));
        }

        void ICommandHandler<CreateRatioEvaluationItem>.Handle(ICommandContext context, CreateRatioEvaluationItem command)
        {
            context.Add(new EvaluationItem(command.EvaluationItemId, command.Name, "ratio"));
        }


        public void Handle(ICommandContext context, SetLinearFormula command)
        {
            var obj = context.Get<EvaluationItem>(command.EvaluationItemId);

            var paramsInfo = new ParametersInfo
            {
                BaseIndicator = command.BaseIndicator,
                BaseScore = command.BaseScore,
                Scale = command.Scale,
                IncreaseStepScore = command.IncreaseStepScore,
                DecreaseStepScore = command.DecreaseStepScore
            };

            //var des = string.Format(@"達作業指標{0}得{1}分, 每增加{2},則{3},每减少{2},則{4}.", command.BaseIndicator, command.BaseScore, command.Scale, command.IncreaseStepScore, command.DecreaseStepScore);
            obj.ChangeGradeSteps(paramsInfo, new Linear(paramsInfo));
        }

        void ICommandHandler<DeleteEvaluationItem>.Handle(ICommandContext context, DeleteEvaluationItem command)
        {
            var obj = context.Get<EvaluationItem>(command.ItemId);
            obj.Disable();
        }

        void ICommandHandler<EnableEvaluationItem>.Handle(ICommandContext context, EnableEvaluationItem command)
        {
            var obj = context.Get<EvaluationItem>(command.ItemId);
            obj.Enable();
        }

        void ICommandHandler<SetSlideFormula>.Handle(ICommandContext context, SetSlideFormula command)
        {
            var obj = context.Get<EvaluationItem>(command.EvaluationItemId);

            var paramsInfo = new ParametersInfo
            {
                BaseIndicator = command.BaseIndicator,
                BaseScore = command.BaseScore,
                Scale = command.Scale,
                StepScore = command.StepScore,
                StartIndicator = command.StartIndicator,
                FinalIndicator = command.FinalIndicator
            };

            obj.ChangeGradeSteps(paramsInfo, new Slide(paramsInfo));
        }
    }

}
