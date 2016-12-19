using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using System.Diagnostics;
using Newtonsoft.Json.Linq;

namespace NetworkTester
{
    class LocationAPI
    {
        /// <summary>
        /// Returns the latitude, longitude, and datetime for the specified host.
        /// </summary>
        /// <param name="_host">Can be an IP address, or website address. Ex: 8.8.8.8 or google.com</param>
        /// <returns></returns>
        public static APIData GetLocation(string _host)
        {
            string json;
            string host = _host; 
            string apiURL = "https://tools.keycdn.com/geo.json?host=" + host;
            string latitude;
            string longitude;
            string datetime;

            using (var webClient = new WebClient())
            {
                json = webClient.DownloadString(apiURL);
            }
    
            RootObject jsonObject = JsonConvert.DeserializeObject<RootObject>(json);

            latitude = jsonObject.data.geo.latitude;
            longitude = jsonObject.data.geo.longitude;
            datetime = jsonObject.data.geo.datetime;

            APIData apiData = new APIData(latitude, longitude, datetime);

            Debug.WriteLine($"Latitude: {apiData.latitude}, Longitude: {apiData.longitude}, Datetime: {apiData.datetime}");

            return apiData;
        }
    }
}
