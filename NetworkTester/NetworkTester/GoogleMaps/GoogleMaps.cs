using Google.Maps;
using Google.Maps.StaticMaps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;
using System.Windows;

namespace NetworkTester
{
    class GoogleMaps
    {
        public static void FindImage(string latitude, string longitude)
        {
            var map = new StaticMapRequest();
            map.Center = new Location($"{latitude}, {longitude}");
            map.Size = new System.Drawing.Size(600, 600);
            map.Zoom = 5;
            map.Sensor = false;
            map.Markers.Add($"{latitude}, {longitude}");
            map.Markers.Add("New York City");

            var imageSource = map.ToUri();

            Clipboard.SetText(imageSource.ToString());
            Debug.WriteLine(imageSource.ToString());
        }
    }
}
