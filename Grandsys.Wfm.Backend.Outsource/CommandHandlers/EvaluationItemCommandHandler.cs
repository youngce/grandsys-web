using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ENode.Commanding;
using ENode.Infrastructure;
using Grandsys.Wfm.Backend.Outsource.Commands;
using Grandsys.Wfm.Backend.Outsource.Domain;

namespace Grandsys.Wfm.Backend.Outsource.CommandHandlers
{
    [Component]
    public class EvaluationItemCommandHandler :
        ICommandHandler<CreatePieceEvaluationItem>,
        ICommandHandler<CreateRatioEvaluationItem>,
        ICommandHandler<SetLinearFormula>,
        ICommandHandler<DeleteEvaluationItem>, ICommandHandler<EnableEvaluationItem>
    {
        void ICommandHandler<CreatePieceEvaluationItem>.Handle(ICommandContext context, CreatePieceEvaluationItem command)
        {   
            context.Add(new EvaluationItem(command.EvaluationItemId ,command.Name, "piece"));
        }

        void ICommandHandler<CreateRatioEvaluationItem>.Handle(ICommandContext context, CreateRatioEvaluationItem command)
        {
            context.Add(new EvaluationItem(command.EvaluationItemId ,command.Name, "ratio"));
        }


        public void Handle(ICommandContext context, SetLinearFormula command)
        {
            var obj = context.Get<EvaluationItem>(command.EvaluationItemId);
            
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
    }

}
