using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Documents;

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

        private static void ShowTracerouteMap(string address)
        {
            var ips = new Traceroute(address).GetRoute();
            var loc = ips.Select(pingResult => IpInformation.GetIpLocation(pingResult.SourceAddress)).ToList();
            var m = new Map(loc);
            m.Show();

        }

        private void brn_getTraceroute_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ShowTracerouteMap("8.8.8.8");
        }
    }
}
