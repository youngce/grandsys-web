using System;
using ENode.Commanding;
using ENode.Domain;
using Grandsys.Wfm.Services.SayHello.ServiceModel;
using NoteSample.Commands;
using ServiceStack.ServiceInterface;

namespace Grandsys.Wfm.Services.SayHello.ServiceInterface
{
    public class NoteService : Service
    {
        private readonly ICommandService _commandService;
        private readonly IMemoryCache _memoryCache;

        public NoteService(ICommandService commandService, IMemoryCache memoryCache)
        {
            _commandService = commandService;
            _memoryCache = memoryCache;
        }

        public object Post(Note request)
        {
            var createCommand = new CreateNote { NoteId = Guid.NewGuid(), Title = request.Name };
            var result = _commandService.Execute(createCommand);
            return new HelloResponse { Result = result.AggregateRootId };
        }

        public object Get(Note request)
        {
            return new HelloResponse { Result = _memoryCache.Get<NoteSample.Domain.Note>(request.Id).Title };
        }
    }
}