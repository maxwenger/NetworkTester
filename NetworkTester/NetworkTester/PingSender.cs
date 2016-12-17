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
        private List<string> addressList;

        private Timer timer;
        private readonly int timeout;

        public PingSender(int interval = 2000, int timeout = 500)
        {
            addressList = new List<string>();
            this.timeout = timeout;

            timer = new Timer();
            timer.Elapsed += SendPings;
            timer.Interval = interval;
        }

        public void AddAddress(string ipString) =>
            addressList?.Add(ipString);

        public void RemoveAddress(List<string> ipStrings) => 
            addressList?.RemoveAll(ip => ipStrings.Any(s => s.Equals(ip)));
        

        public List<string> GetAddressList()
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
                    return new Ping().SendTaskAsync(ip);
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
                if (e.Reply != null)
                {
                    tcs.SetResult(new PingResult()
                    {
                        Address = address,
                        Status = e.Reply?.Status.ToString() ?? "INVALID",
                        RoundtripTime = e.Reply?.RoundtripTime ?? -1,
                        TimeToLive = e.Reply.Options?.Ttl ?? -1
                    });
                }
                else
                {
                    tcs.SetResult(new PingResult()
                    {
                        Address = address,
                        Status = "INVALID",
                        RoundtripTime = -1,
                        TimeToLive = -1
                    });
                }
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
