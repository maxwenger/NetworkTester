using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkTester
{
    public class APIData
    {
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string datetime { get; set; }

        public APIData(string latitude, string longitude, string datetime)
        {
            this.latitude = latitude;
            this.longitude = longitude;
            this.datetime = datetime;
        }
    }

    public class Geo
    {
        public string host { get; set; }
        public string ip { get; set; }
        public string rdns { get; set; }
        public string asn { get; set; }
        public string isp { get; set; }
        public string country_name { get; set; }
        public string country_code { get; set; }
        public string region { get; set; }
        public string city { get; set; }
        public string postal_code { get; set; }
        public string continent_code { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string dma_code { get; set; }
        public string area_code { get; set; }
        public string timezone { get; set; }
        public string datetime { get; set; }
    }

    public class Data
    {
        public Geo geo { get; set; }
    }

    public class RootObject
    {
        public string status { get; set; }
        public string description { get; set; }
        public Data data { get; set; }
    }
}
