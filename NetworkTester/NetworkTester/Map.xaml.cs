using System.Collections.Generic;
using System.Windows;
using Microsoft.Maps.MapControl.WPF;

namespace NetworkTester
{
    /// <summary>
    /// Interaction logic for Map.xaml
    /// </summary>
    public partial class Map : Window
    {
        public Map(List<IpLocation> locations)
        {
            InitializeComponent();

            mp_map.CredentialsProvider = new ApplicationIdCredentialsProvider()
            {
                ApplicationId = GetBingMapsApiKey()
            };

            RemoveInvalidLocations(locations);

            var line = GetLocations(locations);

            SetPolyline(line);

            SetPushPins(locations);
        }

        private static void RemoveInvalidLocations(List<IpLocation> locations)
        {
            for (var i = locations.Count - 1; i >= 0; i--)
            {
                var ipLocation = locations[i];
                if (ipLocation.CountryCode.Equals("ZZ"))
                {
                    locations.Remove(ipLocation);
                }
            }
        }

        private static LocationCollection GetLocations(List<IpLocation> locations)
        {
            var line = new LocationCollection();
            foreach (var ipLocation in locations)
            {
                line.Add(new Location(ipLocation.Latitude, ipLocation.Longitude));
            }
            return line;
        }

        private void SetPushPins(List<IpLocation> locations)
        {
            foreach (var ipLocation in locations)
            {
                var pin = new Pushpin
                {
                    Location = new Location(ipLocation.Latitude, ipLocation.Longitude)
                };
                mp_map.Children.Add(pin);
            }
        }

        private void SetPolyline(LocationCollection line)
        {
            var polyline = new MapPolyline()
            {
                Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Blue),
                StrokeThickness = 5,
                Opacity = 0.7,
                Locations = line
            };

            mp_map.Children.Add(polyline);
        }

        public string GetBingMapsApiKey() => Properties.Resources.BingMapsKey;
    }

}
