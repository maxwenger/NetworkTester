using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Documents;
using NetworkTester.GoogleMaps;
using NetworkTester.LocationAPI;

namespace NetworkTester
{
    public partial class MainWindow
    {
        private PingSender pingSender;
        private bool isSendingPings;

        public MainWindow()
        {
            isSendingPings = false;
            pingSender = new PingSender();

            InitializeComponent();

            pingSender.AllPingsReceived += OnAllPingsReceived;
        }

        private void btn_togglePings_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            isSendingPings = !isSendingPings;
            
            if (isSendingPings)
            {
                pingSender.Start();
            }
            else
            {
                pingSender.Stop();
            }

            btn_togglePings.Content = isSendingPings ? "Stop Pings" : "Start Pings";
        }

        private void btn_addIp_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var ip = tb_ip.Text;
            pingSender.AddAddress(ip);

            tb_ip.Text = "";

            dg_ipList.DataContext = typeof(List);
            dg_ipList.DataContext = pingSender.GetAddressList();
        }

        private void btn_showAddFlyout_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            flo_addIp.IsOpen = true;
        }

        public void OnAllPingsReceived(object source, PingEventArgs e)
        {
            // TODO: Figure out how to do this without calling Dispatcher.Invoke
            Dispatcher.Invoke(() =>
            {
                dg_pingResponces.DataContext = typeof(PingEventArgs);
                dg_pingResponces.DataContext = e.PingResults;
            });
        }

        private void btn_removeIp_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var cells = dg_ipList.SelectedCells;
            var ips = cells.Select(cell => cell.ToString()).ToList();
            pingSender.RemoveAddress(ips);

            dg_ipList.DataContext = typeof(List);
            dg_ipList.DataContext = pingSender.GetAddressList();
        }

        private void GetTracerouteMap(string address)
        {
            var ips = new Traceroute(address).GetRoute();
            var locations = new List<LocationData>();

            foreach (var pingResult in ips)
            {
                locations.Add(LocationAPI.LocationAPI.GetLocation(pingResult.SourceAddress));
            }

            var coordinates  = new List<Coordinate>();

            foreach (var locationData in locations)
            {
                if(locationData.Longitude != null && locationData.Latitude != null) { 
                    coordinates.Add(new Coordinate()
                    {
                        Longitude = double.Parse(locationData.Longitude),
                        Latitude = double.Parse(locationData.Latitude)
                    });

                    // Debug.WriteLine($"{locationData.Longitude}, {locationData.Latitude}");
                    Debug.WriteLine(coordinates.Count);


                }
            }

            var maps = new GoogleMaps.GoogleMaps(coordinates);

            var uri = maps.StaticMapBuilder();

            var b = new Browser(uri);
            b.Show();
        }

        private void brn_getTraceroute_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            GetTracerouteMap("50.111.100.18");
        }
    }
}
