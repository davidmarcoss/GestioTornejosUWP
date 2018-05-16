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
        Shared mainPageShared = new Shared(0, new ObservableCollection<Torneig>(), new ListView());
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

            if (!Torneig.PreinscripcioOberta)
            {
                tbNom.IsEnabled = dpDataInici.IsEnabled = cbModalitats.IsEnabled = false;
            }
            else
            {
                tbNom.IsEnabled = dpDataInici.IsEnabled = cbModalitats.IsEnabled = true;
            }

            cbModalitats.ItemsSource = ModalitatDB.Get();

            populateForm();
        }

        private void btnCrear_Click(object sender, RoutedEventArgs e)
        {
            isNou = true;
            resetForm();
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (checkForm())
            {
                if (isNou)
                {
                    Torneig nouTorneig = new Torneig(-1, (Modalitat)cbModalitats.SelectedItem, tbNom.Text, dpDataInici.Date.DateTime, true);
                    TorneigDB.InsertOrUdate(nouTorneig);
                    Torneig = nouTorneig;
                }
                else
                {
                    Torneig.Nom = tbNom.Text;
                    Torneig.DataInici = dpDataInici.Date.DateTime;
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
            if (TorneigDB.Delete(mainPageShared.OcTornejos[mainPageShared.IdxSelected].Id))
            {
                mainPageShared.OcTornejos.Remove(mainPageShared.OcTornejos[mainPageShared.IdxSelected]);
                if (mainPageShared.LvTornejos.Items.Count > 0)
                {
                    mainPageShared.LvTornejos.SelectedIndex = 0;
                }
            }
        }

        private void populateForm()
        {
            if (Torneig != null)
            {
                tbNom.Text = Torneig.Nom;
                dpDataInici.Date = Torneig.DataInici;
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
            bool status = true;

            if (tbNom.Text.Length <= 0 || tbNom.Text.Equals(""))
            {
                tbNom.Background = new SolidColorBrush(Colors.Red);
                status = false;
            }
            else
            {
                tbNom.Background = new SolidColorBrush(Colors.Transparent);
            }

            if (cbModalitats.SelectedIndex < 0)
            {
                cbModalitats.Background = new SolidColorBrush(Colors.Red);
                status = false;
            }
            else
            {
                tbNom.Background = new SolidColorBrush(Colors.Transparent);
            }

            if (dpDataInici.Date.DateTime < DateTime.Now)
            {
                dpDataInici.Background = new SolidColorBrush(Colors.Red);
                status = false;
            }
            else
            {
                tbNom.Background = new SolidColorBrush(Colors.Transparent);
            }

            return status;
        }
    }
}
