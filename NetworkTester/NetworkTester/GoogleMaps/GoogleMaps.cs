using System;
using System.Collections.Generic;
using System.Linq;
using Google.Maps;
using Google.Maps.StaticMaps;

namespace NetworkTester.GoogleMaps
{
    public class Coordinate
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class GoogleMaps
    {
        private List<Coordinate> locations;

        public GoogleMaps(List<Coordinate> locations)
        {
            this.locations = locations;
        }
        public string StaticMapBuilder()
        {
            var center = FindCenter(locations[0], locations[locations.Count - 1]);
            var map = new StaticMapRequest
            {
                Center = new Location($"{center.Latitude}, {center.Longitude}"),
                Size = new System.Drawing.Size(600, 600),
                // Size = new System.Drawing.Size((int)FindXSize() + 10, (int)FindYSize() + 10),
                Zoom = 5,
                Sensor = false
            };


            foreach (var coordinate in locations)
            {
                map.Markers.Add($"{coordinate.Latitude}, {coordinate.Longitude}");
            }

            var imageSource = map.ToUri();

            return imageSource.ToString();
        }

        private static Coordinate FindCenter(Coordinate first, Coordinate last) => new Coordinate()
            {
                Latitude = first.Latitude + (last.Latitude - first.Latitude) / 2,
                Longitude = first.Longitude + (last.Longitude - first.Longitude) / 2
            };

        private double FindXSize()
        {
            var s = locations[0].Latitude;
            var b = locations[0].Latitude;
            foreach (var coordinate in locations)
            {
                if (s > coordinate.Latitude)
                {
                    s = coordinate.Latitude;
                }

                if (b < coordinate.Latitude)
                {
                    b = coordinate.Latitude;
                }
            }

            return b - s;
        }

        private double FindYSize()
        {
            var s = locations[0].Longitude;
            var b = locations[0].Longitude;
            foreach (var coordinate in locations)
            {
                if (s > coordinate.Longitude)
                {
                    s = coordinate.Longitude;
                }

                if (b < coordinate.Longitude)
                {
                    b = coordinate.Longitude;
                }
            }

            return b - s;
        }
    }
}
