using Newtonsoft.Json;
using System.Net;

namespace NetworkTester.LocationAPI
{
    public class LocationAPI
    {
        /// <summary>
        /// Returns the latitude, longitude, and datetime for the specified host.
        /// </summary>
        /// <param name="_host">Can be an IP address, or website address. Ex: 8.8.8.8 or google.com</param>
        /// <returns></returns>
        public static LocationData GetLocation(string _host)
        {
            string json;
            var host = _host; 
            var apiUrl = "https://tools.keycdn.com/geo.json?host=" + host;

            using (var webClient = new WebClient())
            {
                json = webClient.DownloadString(apiUrl);
            }
    
            var jsonObject = JsonConvert.DeserializeObject<RootObject>(json);

            var latitude = jsonObject.Data.Geo.Latitude;
            var longitude = jsonObject.Data.Geo.Longitude;
            var ip = jsonObject.Data.Geo.Ip;

            var apiData = new LocationData(latitude, longitude, ip);
            

            return apiData;
        }
    }
}
