using GestioTornejos.DB;
using GestioTornejos.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace GestioTornejos.UI
{
    public sealed partial class DadesPage : Page
    {
        private Shared mainPageShared = new Shared(0, new ObservableCollection<Torneig>(), new ListView());
        private bool isNou = false;
        private Torneig Torneig;

        public DadesPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            mainPageShared = (Shared)e.Parameter;

            Torneig = mainPageShared.OcTornejos.ElementAt(mainPageShared.IdxSelected);


            cbModalitats.ItemsSource = ModalitatDB.Get();

            populateForm();

            FormEnabled(Torneig.PreinscripcioOberta);
        }

        private void FormEnabled(bool status)
        {
            tbNom.IsEnabled 
                = dpDataInici.IsEnabled
                = dpDataFi.IsEnabled
                = cbModalitats.IsEnabled 
                = btnTancarPreinscripcions.IsEnabled 
                = btnGuardar.IsEnabled 
                = btnEliminar.IsEnabled 
                = btnCancelar.IsEnabled 
                = status;
        }

        private void btnCrear_Click(object sender, RoutedEventArgs e)
        {
            isNou = true;
            resetForm();
            FormEnabled(true);
            tbNom.IsEnabled = dpDataInici.IsEnabled = cbModalitats.IsEnabled = true;
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (checkForm())
            {
                if (isNou)
                {
                    Torneig nouTorneig = new Torneig(-1, (Modalitat)cbModalitats.SelectedItem, tbNom.Text, dpDataInici.Date.DateTime, dpDataFi.Date.DateTime, true);
                    TorneigDB.InsertOrUdate(nouTorneig);
                    Torneig = nouTorneig;
                    mainPageShared.OcTornejos.Add(Torneig);
                }
                else
                {
                    Torneig.Nom = tbNom.Text;
                    Torneig.DataInici = dpDataInici.Date.DateTime;
                    Torneig.DataFi = dpDataFi.Date.DateTime;
                    Torneig.Modalitat = (Modalitat) cbModalitats.SelectedItem;
                    TorneigDB.InsertOrUdate(Torneig);
                }

                populateForm();
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            populateForm();
        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            EliminarConfirmDialog();
        }

        private async void EliminarConfirmDialog()
        {
            ContentDialog locationPromptDialog = new ContentDialog
            {
                Title = "Eliminar torneig",
                Content = "Estàs segur que vols eliminar aquest torneig?",
                CloseButtonText = "Cancel·la",
                PrimaryButtonText = "Acceptar"
            };

            ContentDialogResult result = await locationPromptDialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                if (TorneigDB.Delete(mainPageShared.OcTornejos[mainPageShared.IdxSelected].Id))
                {
                    mainPageShared.OcTornejos.Remove(mainPageShared.OcTornejos[mainPageShared.IdxSelected]);
                    if (mainPageShared.LvTornejos.Items.Count > 0)
                    {
                        mainPageShared.LvTornejos.SelectedIndex = 0;
                        mainPageShared.IdxSelected = 0;
                        Torneig = mainPageShared.OcTornejos.ElementAt(mainPageShared.IdxSelected);
                        populateForm();
                    }
                }
            }
        }

        private void populateForm()
        {
            if (Torneig != null)
            {
                tbNom.Text = Torneig.Nom;
                dpDataInici.Date = Torneig.DataInici;
                dpDataFi.Date = Torneig.DataFi;
                cbModalitats.SelectedIndex = Torneig.Modalitat.Id - 1;
            }
        }

        private void resetForm()
        {
            tbNom.Text = "";
            dpDataInici.Date = DateTime.Now;
            cbModalitats.SelectedIndex = 0;
        }

        private bool checkForm()
        {
            if (tbNom.Text.Length <= 0 || tbNom.Text.Equals(""))
            {
                tbNom.Background = new SolidColorBrush(Colors.Red);
                return false;
            }
            else
            {
                tbNom.Background = new SolidColorBrush(Colors.Transparent);
            }

            if (cbModalitats.SelectedIndex < 0)
            {
                cbModalitats.Background = new SolidColorBrush(Colors.Red);
                return false;
            }
            else
            {
                tbNom.Background = new SolidColorBrush(Colors.Transparent);
            }

            if (dpDataInici.Date.DateTime < DateTime.Now)
            {
                DialogBox.Show("Error", "La data de inici ha de ser major que avui");
                dpDataInici.Background = new SolidColorBrush(Colors.Red);
                return false;
            }
            else
            {
                dpDataInici.Background = new SolidColorBrush(Colors.Transparent);
            }

            if (dpDataFi.Date.DateTime < DateTime.Now)
            {
                DialogBox.Show("Error", "La data de fi ha de ser major que avui");
                dpDataFi.Background = new SolidColorBrush(Colors.Red);
                return false;
            }
            else
            {
                dpDataFi.Background = new SolidColorBrush(Colors.Transparent);
            }

            if (dpDataInici.Date.DateTime > dpDataFi.Date.DateTime)
            {
                DialogBox.Show("Error", "La data de inici ha de ser menor a la data de fi");
                return false;
            }
            else
            {
                dpDataInici.Background = new SolidColorBrush(Colors.Transparent);
            }

            return true;
        }

        private void btnTancarPreinscripcions_Click(object sender, RoutedEventArgs e)
        {
            if (TorneigDB.TancarPreinscripcions(Torneig))
            {
                Torneig.PreinscripcioOberta = false;
                FormEnabled(false);
            }
        }
    }
}
