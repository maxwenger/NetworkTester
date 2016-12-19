using System;
using System.Windows;

namespace NetworkTester
{
    /// <summary>
    /// Interaction logic for Browser.xaml
    /// </summary>
    public partial class Browser : Window
    {
        public Browser(string url)
        {
            InitializeComponent();

            brw_browser.Loaded += delegate
            {
                brw_browser.Navigate(new Uri(url));
            };
        }
    }
}
