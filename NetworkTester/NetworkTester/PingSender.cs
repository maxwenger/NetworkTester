using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Timers;

namespace NetworkTester
{

    public class PingEventArgs : EventArgs
    {
        public List<MyExtensions.PingResult> PingResults { get; set; }
    }

    public class PingSender
    {
        private List<IPAddress> addressList;

        private Timer timer;
        private readonly int timeout;

        public PingSender(int interval = 2000, int timeout = 500)
        {
            addressList = new List<IPAddress>();
            this.timeout = timeout;

            timer = new Timer();
            timer.Elapsed += SendPings;
            timer.Interval = interval;
        }

        public bool AddAddress(string ipString)
        {
            IPAddress address;
            var isValidIp = IPAddress.TryParse(ipString, out address);

            if (isValidIp)
            {
                addressList.Add(address);
            }

            return isValidIp;
        }
        public List<IPAddress> GetAddressList()
        {
            return addressList;
        }
        
        public void Start() => timer.Enabled = true;
        public void Stop() => timer.Enabled = false;

        public event EventHandler<PingEventArgs> AllPingsReceived;

        private async void SendPings(object source, ElapsedEventArgs e)
        {
            var pingTasks = addressList.Select(ip =>
            {
                using (var ping = new Ping())
                {
                    return new Ping().SendTaskAsync(ip.ToString());
                }
            }).ToList();

            await Task.WhenAll(pingTasks);

            var pingReplies = pingTasks.Select(reply => reply.Result).ToList();

            OnAllPingsReceived(pingReplies);

        }

        protected virtual void OnAllPingsReceived(List<MyExtensions.PingResult> pingResults)
        {
            AllPingsReceived?.Invoke(this, new PingEventArgs() { PingResults = pingResults });
        }

    }


     
    public static class MyExtensions
    {
        /**
         * Extension method Curtosy of SO user L.B.
         * http://stackoverflow.com/a/25534776
         **/
        public static Task<PingResult> SendTaskAsync(this Ping ping, string address)
        {
            var tcs = new TaskCompletionSource<PingResult>();
            PingCompletedEventHandler response = null;
            response = (s, e) =>
            {
                ping.PingCompleted -= response;
                tcs.SetResult(new PingResult()
                {
                    Address = address,
                    Status = e.Reply.Status.ToString(),
                    RoundtripTime = e.Reply.RoundtripTime,
                    TimeToLive = e.Reply.Options?.Ttl ?? -1
                });
            };
            ping.PingCompleted += response;
            ping.SendAsync(address, address);
            return tcs.Task;
        }

        public class PingResult
        {
            public string Address { get; set; }
            public string Status { get; set; }
            public long RoundtripTime { get; set; }
            public int TimeToLive { get; set; }
        }
    }

}
