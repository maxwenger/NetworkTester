using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;

namespace NetworkTester
{
    public class Traceroute
    {
        private string address;
        private const int Timeout = 2500;
        private int maxHops;

        public Traceroute(string address, int maxHops = 30)
        {
            maxHops = maxHops < 1 || maxHops > 100 ? maxHops : 30;
            this.maxHops = maxHops;

            this.address = Dns.GetHostAddresses(address)[0].ToString();
        }

        public List<PingExtensions.PingResult> GetRoute(int nextHop = 1)
        {
            PingExtensions.PingResult result;

            using (var ping = new Ping())
            {
                var options = PingOptionsFactory(nextHop);
                PingReply p;
                try
                {
                    p = ping.Send(address, Timeout, new byte[32], options);
                }
                catch (PingException e)
                {
                    return new List<PingExtensions.PingResult>()
                    {
                        new PingExtensions.PingResult()
                        {
                            SourceAddress = "0.0.0.0",
                            RoundtripTime = 0,
                            Status = e.InnerException?.ToString(),
                            DestinationAddress = address,
                            TimeToLive = 0
                        }
                    };
                }

                result = PingResultFactory(nextHop, p);
            }

            List<PingExtensions.PingResult> results;
            if (result.SourceAddress.Equals(result.DestinationAddress))
            {
                results = new List<PingExtensions.PingResult>()
                {
                    result
                };
            }
            else if (nextHop >= maxHops)
            {
                results = null;
            }
            else
            {
                results = GetRoute(++nextHop);
                results.Add(result);
            }

            return results;
        }

        private PingExtensions.PingResult PingResultFactory(int nextHop, PingReply p)
        {
            var result = new PingExtensions.PingResult();

            if (p != null)
            {
                result.SourceAddress = p.Address?.ToString();
                result.DestinationAddress = address;
                result.Status = p.Status.ToString();
                result.RoundtripTime = p.RoundtripTime;
                result.TimeToLive = nextHop;
            }
            return result;
        }

        private static PingOptions PingOptionsFactory(int nextHop)
        {
            var options = new PingOptions
            {
                DontFragment = true,
                Ttl = nextHop
            };
            return options;
        }
    }
}
