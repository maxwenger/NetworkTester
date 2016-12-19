using Google.Maps;
using Google.Maps.StaticMaps;
using System.Collections.Generic;
using System.Windows;

namespace NetworkTester.GoogleMaps
{
    public class GoogleMaps
    {
        public static void FindImage(string latitude, string longitude)
        {
            var map = new StaticMapRequest
            {
                Center = new Location($"{latitude}, {longitude}"),
                Size = new System.Drawing.Size(600, 600),
                Zoom = 5,
                Sensor = false
            };
            map.Markers.Add($"{latitude}, {longitude}");
            map.Markers.Add("New York City");

            var imageSource = map.ToUri();

            Clipboard.SetText(imageSource.ToString());
        }

        public void GenerateMap(List<Location> locations)
        {
                      
        }
    }
}
