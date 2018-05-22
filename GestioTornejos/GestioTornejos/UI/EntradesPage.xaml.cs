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
    public sealed partial class EntradesPage : Page
    {
        private Shared mainPageShared = new Shared(0, new ObservableCollection<Torneig>(), new ListView());
        private Torneig Torneig;
        private Grup grup;
        private Partida partida;

        public EntradesPage()
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
            lvGrups.ItemsSource = Torneig.Grups;

            FormEnabled(false);
        }
        
        private void FormEnabled(bool status)
        {
            tbCarambolesA.IsEnabled
                = tbCarambolesB.IsEnabled
                = tbNumEntrades.IsEnabled
                = cbEstatPartida.IsEnabled
                = cbGuanyador.IsEnabled
                = cbModeVictoria.IsEnabled
                = status; 
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (checkForm())
            {
                partida.CarambolesA = Int32.Parse(tbCarambolesA.Text);
                partida.CarambolesB = Int32.Parse(tbCarambolesA.Text);
                partida.NumEntrades = Int32.Parse(tbNumEntrades.Text);
                partida.EstatPartida = (EstatPartida) cbEstatPartida.SelectedIndex;
                partida.Guanyador = (Guanyador) cbGuanyador.SelectedIndex;
                partida.ModeVictoria = (ModeVictoria) cbModeVictoria.SelectedIndex;

                if (PartidaDB.Update(partida))
                {
                    populateForm();
                }
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            populateForm();
        }

        private void populateForm()
        {
            tbCarambolesA.Text = partida.CarambolesA.ToString();
            tbCarambolesB.Text = partida.CarambolesB.ToString();
            tbNumEntrades.Text = partida.NumEntrades.ToString();
            cbEstatPartida.SelectedIndex = (int)partida.EstatPartida;
            cbModeVictoria.SelectedIndex = (int)partida.ModeVictoria;
            cbGuanyador.Items.Clear();
            cbGuanyador.Items.Add(partida.SociA);
            cbGuanyador.Items.Add(partida.SociB);
            cbGuanyador.SelectedIndex = (int)partida.Guanyador;
        }

        private void tbCarambolesA_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (!e.Key.ToString().Contains("Number"))
            {
                e.Handled = true;
            }
            else
            {
                try
                {
                    Int32.Parse(tbCarambolesA.Text + e.Key.ToString().Replace("Number", ""));
                }
                catch (OverflowException) { e.Handled = true; }
            }
        }

        private void tbCarambolesB_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (!e.Key.ToString().Contains("Number"))
            {
                e.Handled = true;
            }
            else
            {
                try
                {
                    Int32.Parse(tbCarambolesB.Text + e.Key.ToString().Replace("Number", ""));
                }
                catch (OverflowException) { e.Handled = true; }
            }
        }

        private void tbNumEntrades_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (!e.Key.ToString().Contains("Number"))
            {
                e.Handled = true;
            }
            else
            {
                try
                {
                    Int32.Parse(tbNumEntrades.Text + e.Key.ToString().Replace("Number", ""));
                }
                catch (OverflowException) { e.Handled = true; }
            }
        }

        private bool checkForm()
        {
            if (tbCarambolesA.Text.Equals("") || Int32.Parse(tbCarambolesA.Text) < 0)
            {
                tbCarambolesA.Background = new SolidColorBrush(Colors.Red);
                return false;
            }
            else
            {
                tbCarambolesA.Background = new SolidColorBrush(Colors.Transparent);
            }

            if (tbCarambolesB.Text.Equals("") || Int32.Parse(tbCarambolesB.Text) < 0)
            {
                tbCarambolesB.Background = new SolidColorBrush(Colors.Red);
                return false;
            }
            else
            {
                tbCarambolesB.Background = new SolidColorBrush(Colors.Transparent);
            }

            if (tbNumEntrades.Text.Equals("") || Int32.Parse(tbNumEntrades.Text) < 0)
            {
                tbNumEntrades.Background = new SolidColorBrush(Colors.Red);
                return false;
            }
            else
            {
                tbNumEntrades.Background = new SolidColorBrush(Colors.Transparent);
            }

            return true;
        }

        private void lvGrups_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            grup = Torneig.Grups[lvGrups.SelectedIndex];
            lvPartides.ItemsSource = grup.Partides;
        }

        private void lvPartides_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            partida = grup.Partides[lvPartides.SelectedIndex];
            populateForm();
            FormEnabled(true);
        }
    }
}
