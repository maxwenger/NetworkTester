using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace NetworkTester
{
    public static class PingExtensions
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
                        DestinationAddress = address,
                        SourceAddress = e.Reply?.Address.ToString(),
                        Status = e.Reply?.Status.ToString(),
                        RoundtripTime = (long) e.Reply?.RoundtripTime,
                        TimeToLive = e.Reply.Options?.Ttl ?? -1
                    });
                }
                else
                {
                    tcs.SetResult(new PingResult()
                    {
                        DestinationAddress = address,
                        SourceAddress = "0.0.0.0",
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
            /// <summary>
            /// The address attempted to reach
            /// </summary>
            public string DestinationAddress { get; set; }

            /// <summary>
            /// The address that responded to the ICMP packet
            /// </summary>
            public string SourceAddress { get; set; }

            /// <summary>
            /// See Ping.Status
            /// </summary>
            public string Status { get; set; }

            /// <summary>
            /// See Ping.RoundtripTime
            /// </summary>
            public long RoundtripTime { get; set; }
            
            public int TimeToLive { get; set; }
        }
    }
}