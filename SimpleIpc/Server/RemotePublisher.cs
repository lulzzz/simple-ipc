using System;
using System.Reactive.Linq;
using log4net;
using SimpleIpc.Shared;

namespace SimpleIpc.Server
{
    internal class RemotePublisher : IPublisher
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ServerConnection _connection;

        public IObservable<string> MessageReceived {get;}
        public IObservable<string> PublishReceived {get;}
        public IObservable<string> UnpublishReceived {get;}

        public RemotePublisher(ServerConnection connection)
        {
            _connection = connection;
            _connection.DataReceived.Subscribe((s) => Log.Debug("Received: " + s));

            MessageReceived = _connection.DataReceived;
            PublishReceived = _connection.ControlReceived.Where((cc) => cc.Control == ControlBytes.Publish).Select((cc) => cc.Data);
            UnpublishReceived = _connection.ControlReceived.Where((cc) => cc.Control == ControlBytes.Unpublish).Select((cc) => cc.Data);
        }
    }
}