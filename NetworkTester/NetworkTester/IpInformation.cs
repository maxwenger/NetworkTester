using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using GoogleMaps.LocationServices;
using NetworkTester.Properties;
using Newtonsoft.Json;

namespace NetworkTester
{
    public class IpInformation
    {
        public static IpLocation GetIpLocation(string address)
        {
            // TODO: add dns lookup
            var apiKey = Resources.DBIPKey;
            var apiUrl = $"http://api.db-ip.com/v2/{apiKey}/{address}";

            string json;
            using (var web = new WebClient())
            {
                json = web.DownloadString(apiUrl);
            }

            var jsonObj = JsonConvert.DeserializeObject<IpLocation>(json);

            // TODO: Safety!!

            Debug.WriteLine($"{jsonObj.IpAddress} : {jsonObj.City}, {jsonObj.StateProv}, {jsonObj.CountryCode}");
            jsonObj = PopulateLatitudeLongitude(jsonObj);
            return jsonObj;
        }

        private static IpLocation PopulateLatitudeLongitude(IpLocation location)
        {
            var address = $"{location.City}, {location.StateProv}, {location.CountryCode}";

            var locationService = new GoogleLocationService();
            var point = locationService.GetLatLongFromAddress(address);

            return new IpLocation()
            {
                IpAddress = location.IpAddress,
                CountryCode = location.CountryCode,
                StateProv = location.StateProv,
                City = location.City,
                Latitude = point.Latitude,
                Longitude = point.Longitude
            };
        }
    }

    public class IpLocation
    {
        public string IpAddress { get; set; }
        public string CountryCode { get; set; }
        public string StateProv { get; set; }
        public string City { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
