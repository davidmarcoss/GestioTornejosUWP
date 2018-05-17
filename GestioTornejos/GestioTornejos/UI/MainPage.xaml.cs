using GestioTornejos.DB;
using GestioTornejos.Models;
using GestioTornejos.UI;
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

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0xc0a

namespace GestioTornejos
{
    /// <summary>
    /// Página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private ObservableCollection<Torneig> ocTornejos;
        private Shared mainPageShared = null;

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ocTornejos = TorneigDB.Get();

            lvTornejos.ItemsSource = ocTornejos;

            mainPageShared = new Shared(0, new ObservableCollection<Torneig>(), lvTornejos);
            mainPageShared.OcTornejos = ocTornejos;
            mainPageShared.LvTornejos = lvTornejos;
            if (mainPageShared.LvTornejos.Items.Count > 0)
            { 
                mainPageShared.LvTornejos.SelectedIndex = 0;
            }

            frameDades.Navigate(typeof(DadesPage), mainPageShared);
        }

        private void actionPivots_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            navigateToFrame();
        }

        private void TorneigItemRow_Tapped(object sender, TappedRoutedEventArgs e)
        {
            mainPageShared.IdxSelected = lvTornejos.SelectedIndex;

            TorneigItemRow torneigItemRow = (TorneigItemRow)sender;

            navigateToFrame();
        }

        private void navigateToFrame()
        {
            if (mainPageShared != null)
            {
                if (actionPivots.SelectedIndex == 0)
                {
                    frameDades.Navigate(typeof(DadesPage), mainPageShared);
                }
                else if (actionPivots.SelectedIndex == 1)
                {
                    frameGrups.Navigate(typeof(GrupsPage), mainPageShared);
                }
            }
        }

        private void btnAplicarFiltres_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
