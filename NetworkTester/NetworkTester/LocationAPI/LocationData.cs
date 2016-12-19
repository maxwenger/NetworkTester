namespace NetworkTester.LocationAPI
{
    public class LocationData
    {
        public string Address { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }

        public LocationData(string latitude, string longitude, string address)
        {
            Latitude = latitude;
            Longitude = longitude;
            Address = address;
        }
    }

    public class Geo
    {
        public string Host { get; set; }
        public string Ip { get; set; }
        public string Rdns { get; set; }
        public string Asn { get; set; }
        public string Isp { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string ContinentCode { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string DmaCode { get; set; }
        public string AreaCode { get; set; }
        public string Timezone { get; set; }
        public string Datetime { get; set; }
    }
    
    // Used for JSON deserialisation.
    public class Data
    {
        public Geo Geo { get; set; }
    }

    public class RootObject
    {
        public string Status { get; set; }
        public string Description { get; set; }
        public Data Data { get; set; }
    }
}
