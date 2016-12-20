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
        private List<IpLocation> locations;
        public Map(List<IpLocation> locations)
        {
            this.locations = new List<IpLocation>(locations);
            InitializeComponent();

            var line = new LocationCollection();
            foreach (var ipLocation in locations)
            {
                line.Add(new Location(ipLocation.Latitude, ipLocation.Longitude));
            }

            var polyline = new MapPolyline()
            {
                Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Blue),
                StrokeThickness = 5,
                Opacity = 0.7,
                Locations = line
            };

            mp_map.Children.Add(polyline);

            foreach (var ipLocation in this.locations)
            {
                var pin = new Pushpin
                {
                    Location = new Location(ipLocation.Latitude, ipLocation.Longitude)
                };
                mp_map.Children.Add(pin);
            }
        }
        
        public string GetBingMapsApiKey() => Properties.Resources.BingMapsKey;
    }

}
