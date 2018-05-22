using GestioTornejos.DB;
using GestioTornejos.Models;
using GestioTornejos.UI;
using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
namespace GestioTornejos
{
    public sealed partial class MainPage : Page
    {
        private ObservableCollection<Torneig> ocTornejos;
        private Shared mainPageShared = null;

        public MainPage()
        {
            this.InitializeComponent();
        }


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ocTornejos = TorneigDB.Get(-1, "", "");

            lvTornejos.ItemsSource = ocTornejos;

            mainPageShared = new Shared(0, new ObservableCollection<Torneig>(), lvTornejos);
            mainPageShared.OcTornejos = ocTornejos;
            mainPageShared.LvTornejos = lvTornejos;
            if (mainPageShared.LvTornejos.Items.Count > 0)
            { 
                mainPageShared.LvTornejos.SelectedIndex = 0;
            }

            frameDades.Navigate(typeof(DadesPage), mainPageShared);

            dpDataFrom.IsEnabled = dpDataTo.IsEnabled = false;
        }

        private void actionPivots_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
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
                else if (actionPivots.SelectedIndex == 2)
                {
                    frameEntrades.Navigate(typeof(EntradesPage), mainPageShared);
                }
                else if (actionPivots.SelectedIndex == 3)
                {
                    frameClassificacio.Navigate(typeof(ClassificacioPage), mainPageShared);
                }
                else if (actionPivots.SelectedIndex == 4)
                {
                    frameReports.Navigate(typeof(ReportsPage), mainPageShared);
                }
            }
        }

        private void btnAplicarFiltres_Click(object sender, RoutedEventArgs e)
        {
            int estat = cbEstats.SelectedIndex - 1;
            String dataFrom = "";
            String dataTo = "";

            if (cbFiltreDates.IsChecked == true)
            {
                dataFrom = dpDataFrom.Date.DateTime.ToString("yyyy-MM-dd");
                dataTo = dpDataTo.Date.DateTime.ToString("yyyy-MM-dd");
            }

            ocTornejos = TorneigDB.Get(estat, dataFrom, dataTo);
            lvTornejos.ItemsSource = ocTornejos;
        }

        private void dpDataFrom_DateChanged(object sender, DatePickerValueChangedEventArgs e)
        {
            
        }

        private void dpDataTo_DateChanged(object sender, DatePickerValueChangedEventArgs e)
        {
            if(dpDataTo.Date.DateTime < dpDataFrom.Date.DateTime)
            {
                DialogBox.Show("Error", "La data dos ha de ser més gran que la primera");
            }
        }

        private void cbFiltreDates_Checked(object sender, RoutedEventArgs e)
        {  
            dpDataFrom.IsEnabled = dpDataTo.IsEnabled = true;
        }

        private void cbFiltreDates_Unchecked(object sender, RoutedEventArgs e)
        {
            dpDataFrom.IsEnabled = dpDataTo.IsEnabled = false;
        }

        private void lvTornejos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvTornejos.SelectedIndex >= 0)
            {
                mainPageShared.IdxSelected = lvTornejos.SelectedIndex;

                navigateToFrame();
            }
        }
    }
}
