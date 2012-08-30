using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SignalR.Hubs;

namespace MvcApplication1.Hubs
{
    public class ConnectionCounterHub : Hub, IDisconnect, IConnected
    {
        private static int connectionCount;

        public Task Connect()
        {
            Interlocked.Increment(ref connectionCount);
            return Clients.updateConnectionCount(connectionCount);
        }

        public Task Reconnect(IEnumerable<string> groups)
        {
            Interlocked.Increment(ref connectionCount);
            return Clients.updateConnectionCount(connectionCount);
        }

        public Task Disconnect()
        {
            Interlocked.Decrement(ref connectionCount);
            return Clients.updateConnectionCount(connectionCount);
        }
    }
}