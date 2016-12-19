using System.Diagnostics;
using Newtonsoft.Json;
using System.Net;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

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
    
            var jsonObject = JsonConvert.DeserializeObject<RootObject>(
                json,
                new JsonSerializerSettings()
                {
                    Error = delegate(object sender, ErrorEventArgs args)
                    {
                        args.ErrorContext.Handled = true;
                    }
                }
                );

            var latitude = jsonObject.Data.Geo.Latitude;
            var longitude = jsonObject.Data.Geo.Longitude;
            var ip = jsonObject.Data.Geo.Ip;

            Debug.WriteLine($"{ip} : {latitude}, {longitude}");

            var apiData = new LocationData(latitude, longitude, ip);
            

            return apiData;
        }
    }
}
