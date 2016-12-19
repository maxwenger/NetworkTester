using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Timers;

namespace NetworkTester
{

    public class PingEventArgs : EventArgs
    {
        public List<PingExtensions.PingResult> PingResults { get; set; }
    }

    public class PingSender
    {
        private List<string> addressList;

        private Timer timer;

        public PingSender(int interval = 2000)
        {
            addressList = new List<string>();

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
                    return ping.SendTaskAsync(ip);
                }
            }).ToList();

            await Task.WhenAll(pingTasks);

            var pingReplies = pingTasks.Select(reply => reply.Result).ToList();

            OnAllPingsReceived(pingReplies);

        }

        protected virtual void OnAllPingsReceived(List<PingExtensions.PingResult> pingResults)
        {
            AllPingsReceived?.Invoke(this, new PingEventArgs() {PingResults = pingResults});
        }

    }
}
