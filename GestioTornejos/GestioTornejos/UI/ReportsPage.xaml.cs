using GestioTornejos.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace GestioTornejos.UI
{
    public sealed partial class ReportsPage : Page
    {
        private Shared mainPageShared = new Shared(0, new ObservableCollection<Torneig>(), new ListView());
        private Torneig Torneig;

        public ReportsPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            mainPageShared = (Shared)e.Parameter;

            Torneig = mainPageShared.OcTornejos.ElementAt(mainPageShared.IdxSelected);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void btnReportClassificacio_Click(object sender, RoutedEventArgs e)
        {
            webView.Navigate(new Uri(string.Format("http://92.222.27.83:8080/jasperserver/rest_v2/reports/m2-dmarcos/PROJECTE/partides.html?j_username=m2-dmarcos&j_password=23844512K&torneigId={0}", Torneig.Id)));
        }

        private void btnReportSocis_Click(object sender, RoutedEventArgs e)
        {
            webView.Navigate(new Uri("http://92.222.27.83:8080/jasperserver/rest_v2/reports/m2-dmarcos/PROJECTE/socis.html?j_username=m2-dmarcos&j_password=23844512K"));
        }
    }
}
