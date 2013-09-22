using System;
using ServiceStack.ServiceHost;

namespace Grandsys.Wfm.Services.SayHello.ServiceModel
{
    [Api("This is Sample.")]
    public class Note
    {
        public string Name { get; set; }
        public Guid Id { get; set; }
    }
}