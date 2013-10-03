using System;
using ENode.Commanding;
using ENode.Infrastructure;
using Grandsys.Wfm.Backend.Outsource.Commands;
using Grandsys.Wfm.Backend.Outsource.Domain;
using Grandsys.Wfm.Backend.Outsource.Events;
using Grandsys.Wfm.Backend.Outsource.Interface;

namespace Grandsys.Wfm.Backend.Outsource.CommandHandlers
{
    [Component]
    public class EvaluationItemCommandHandler :
        ICommandHandler<CreatePieceEvaluationItem>,
        ICommandHandler<CreateRatioEvaluationItem>,
        ICommandHandler<SetLinearFormula>,
        ICommandHandler<DeleteEvaluationItem>, ICommandHandler<EnableEvaluationItem>, ICommandHandler<SetSlideFormula>,
        ICommandHandler<ChangeEvaluationItemName>, ICommandHandler<SetEvaluationItemDescription>
    {
        public void Handle(ICommandContext context, ChangeEvaluationItemName command)
        {
            var obj = context.Get<EvaluationItem>(command.ItemId);
            obj.ChangeName(command.NewName);
        }

        void ICommandHandler<CreatePieceEvaluationItem>.Handle(ICommandContext context,
            CreatePieceEvaluationItem command)
        {
            context.Add(new EvaluationItem(command.EvaluationItemId, command.Name, "piece"));
        }

        void ICommandHandler<CreateRatioEvaluationItem>.Handle(ICommandContext context,
            CreateRatioEvaluationItem command)
        {
            context.Add(new EvaluationItem(command.EvaluationItemId, command.Name, "ratio"));
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

        public void Handle(ICommandContext context, SetEvaluationItemDescription command)
        {
            var obj = context.Get<EvaluationItem>(command.ItemId);

            obj.SetDescription(command);

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
                DecreaseStepScore = command.DecreaseStepScore,
                StepScore = -5,
                StartIndicator = 3,
                FinalIndicator = 14
            };

            //var des = string.Format(@"達作業指標{0}得{1}分, 每增加{2},則{3},每减少{2},則{4}.", command.BaseIndicator, command.BaseScore, command.Scale, command.IncreaseStepScore, command.DecreaseStepScore);
            obj.ChangeGradeSteps(new Linear(paramsInfo));
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
                FinalIndicator = command.FinalIndicator,
                DecreaseStepScore = 0,
                IncreaseStepScore = 0,
            };

            obj.ChangeGradeSteps(new Slide(paramsInfo));
        }
    }
}