using System.Diagnostics;
using System.Net;
using System.Windows.Automation.Peers;
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
            jsonObj = PopulateLatitudeLongitude(jsonObj);
            Debug.WriteLine($"{jsonObj.IpAddress} : {jsonObj.City}, {jsonObj.StateProv}, {jsonObj.CountryCode} | {jsonObj.Latitude}, {jsonObj.Longitude}");
            return jsonObj;
        }

        private static IpLocation PopulateLatitudeLongitude(IpLocation location)
        {
            var address = $"{location.City}, {location.StateProv}, {location.CountryCode}";

            var locationService = new GoogleLocationService();
            var point = locationService.GetLatLongFromAddress(address);

            point = point ?? new MapPoint()
            {
                Longitude = 0,
                Latitude = 0
            };

            return new IpLocation()
            {
                IpAddress = location.IpAddress ?? "0.0.0.0",
                CountryCode = location.CountryCode ?? "ZZ",
                StateProv = location.StateProv ?? "ZZ",
                City = location.City ?? "ZZ",
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
