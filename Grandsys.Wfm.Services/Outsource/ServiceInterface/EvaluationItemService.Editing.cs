using Grandsys.Wfm.Services.Outsource.ServiceModel;
using System;
using System.Linq;

namespace Grandsys.Wfm.Services.Outsource.ServiceInterface
{
    public partial class EvaluationItemService
    {
        public object Put(EnableEvaluationItem request)
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
                        new Link { Name = "Undo", Method = "PUT", Request = new EnableEvaluationItem { Id = request.Id } }
                    }
                };
            }
            else
                throw result.ErrorInfo.Exception;
        }
    }
}