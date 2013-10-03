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
    public class EvaluationFormCommandHandler : ICommandHandler<CreateEvaluationForm>
    {
        public void Handle(ICommandContext context, CreateEvaluationForm command)
        {
            context.Add(new EvaluationForm(Guid.NewGuid(), command.Items));
        }
    }
}
