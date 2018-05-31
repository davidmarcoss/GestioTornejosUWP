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
                = tbNumEntradesA.IsEnabled
                = tbNumEntradesB.IsEnabled
                = cbEstatPartida.IsEnabled
                = cbGuanyador.IsEnabled
                = cbModeVictoria.IsEnabled
                = btnGuardar.IsEnabled
                = btnCancelar.IsEnabled
                = status; 
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (checkForm())
            {
                partida.CarambolesA = Int32.Parse(tbCarambolesA.Text);
                partida.CarambolesB = Int32.Parse(tbCarambolesB.Text);
                partida.NumEntradesA = Int32.Parse(tbNumEntradesA.Text);
                partida.NumEntradesB = Int32.Parse(tbNumEntradesB.Text);
                partida.EstatPartida = (EstatPartida) cbEstatPartida.SelectedIndex;
                partida.Guanyador = (Guanyador) cbGuanyador.SelectedIndex;
                partida.ModeVictoria = (ModeVictoria) cbModeVictoria.SelectedIndex;

                if (PartidaDB.Update(partida))
                {
                    populateForm();
                    lvPartides.ItemsSource = grup.Partides;

                    if (partida.EstatPartida == EstatPartida.JUGAT)
                    {
                        Modalitat modalitat = Torneig.Modalitat;
                        EstadisticaModalitat estadisticaModalitat = null;

                        estadisticaModalitat = partida.SociA.Estadistiques.SingleOrDefault(em => em.Modalitat.Equals(modalitat));
                        if (estadisticaModalitat == null)
                        {
                            estadisticaModalitat = new EstadisticaModalitat();
                            estadisticaModalitat.Soci = partida.SociA;
                            estadisticaModalitat.Modalitat = Torneig.Modalitat;
                            estadisticaModalitat.CarambolesTemporadaActual += partida.CarambolesA;
                            estadisticaModalitat.EntradesTemporadaActual += partida.NumEntradesA;
                            double coeficient = (double)estadisticaModalitat.CarambolesTemporadaActual / (double)estadisticaModalitat.EntradesTemporadaActual;
                            estadisticaModalitat.CoeficientBase = coeficient;
                            if (SociDB.InsertEstadistiques(partida.SociA, estadisticaModalitat))
                            {
                                partida.SociA.Estadistiques.Add(estadisticaModalitat);
                            }
                        }
                        else
                        {
                            estadisticaModalitat.CarambolesTemporadaActual += partida.CarambolesA;
                            estadisticaModalitat.EntradesTemporadaActual += partida.NumEntradesA;
                            double coeficient = (double)estadisticaModalitat.CarambolesTemporadaActual / (double)estadisticaModalitat.EntradesTemporadaActual;
                            estadisticaModalitat.CoeficientBase = coeficient;
                            SociDB.UpdateEstadistiques(partida.SociA, estadisticaModalitat);
                        }

                        estadisticaModalitat = null;

                        estadisticaModalitat = partida.SociB.Estadistiques.SingleOrDefault(em => em.Modalitat.Equals(modalitat));
                        if (estadisticaModalitat == null)
                        {
                            estadisticaModalitat = new EstadisticaModalitat();
                            estadisticaModalitat.Soci = partida.SociB;
                            estadisticaModalitat.Modalitat = Torneig.Modalitat;
                            estadisticaModalitat.CarambolesTemporadaActual += partida.CarambolesB;
                            estadisticaModalitat.EntradesTemporadaActual += partida.NumEntradesB;
                            double coeficient = (double)estadisticaModalitat.CarambolesTemporadaActual / (double)estadisticaModalitat.EntradesTemporadaActual;
                            estadisticaModalitat.CoeficientBase = coeficient;
                            if (SociDB.UpdateEstadistiques(partida.SociB, estadisticaModalitat))
                            {
                                partida.SociB.Estadistiques.Add(estadisticaModalitat);
                            }
                        }
                        else
                        {
                            estadisticaModalitat.CarambolesTemporadaActual += partida.CarambolesB;
                            estadisticaModalitat.EntradesTemporadaActual += partida.NumEntradesB;
                            double coeficient = (double)estadisticaModalitat.CarambolesTemporadaActual / (double)estadisticaModalitat.EntradesTemporadaActual;
                            estadisticaModalitat.CoeficientBase = coeficient;
                            SociDB.UpdateEstadistiques(partida.SociB, estadisticaModalitat);
                        }
                    }
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
            tbNumEntradesA.Text = partida.NumEntradesA.ToString();
            tbNumEntradesB.Text = partida.NumEntradesB.ToString();
            cbEstatPartida.SelectedIndex = (int) partida.EstatPartida;
            cbModeVictoria.SelectedIndex = (int) partida.ModeVictoria;
            cbGuanyador.Items.Clear();
            cbGuanyador.Items.Add(partida.SociA);
            cbGuanyador.Items.Add(partida.SociB);
            cbGuanyador.SelectedIndex = (int) partida.Guanyador;
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

        private void tbNumEntradesA_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (!e.Key.ToString().Contains("Number"))
            {
                e.Handled = true;
            }
            else
            {
                try
                {
                    Int32.Parse(tbNumEntradesA.Text + e.Key.ToString().Replace("Number", ""));
                }
                catch (OverflowException) { e.Handled = true; }
            }
        }

        private void tbNumEntradesB_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (!e.Key.ToString().Contains("Number"))
            {
                e.Handled = true;
            }
            else
            {
                try
                {
                    Int32.Parse(tbNumEntradesB.Text + e.Key.ToString().Replace("Number", ""));
                }
                catch (OverflowException) { e.Handled = true; }
            }
        }

        private bool checkForm()
        {
            int carambolesA = Int32.Parse(tbCarambolesA.Text);
            if (tbCarambolesA.Text.Equals("") || carambolesA < 0)
            {
                tbCarambolesA.Background = new SolidColorBrush(Colors.LightPink);
                return false;
            }
            else
            {
                tbCarambolesA.Background = new SolidColorBrush(Colors.Transparent);
            }

            int carambolesB = Int32.Parse(tbCarambolesB.Text);
            if (tbCarambolesB.Text.Equals("") || carambolesB < 0)
            {
                tbCarambolesB.Background = new SolidColorBrush(Colors.LightPink);
                return false;
            }
            else
            {
                tbCarambolesB.Background = new SolidColorBrush(Colors.Transparent);
            }

            int numEntradesA = Int32.Parse(tbNumEntradesA.Text);
            if (tbNumEntradesA.Text.Equals("") || numEntradesA < 0)
            {
                tbNumEntradesA.Background = new SolidColorBrush(Colors.LightPink);
                return false;
            }
            else
            {
                tbNumEntradesA.Background = new SolidColorBrush(Colors.Transparent);
            }

            int numEntradesB = Int32.Parse(tbNumEntradesB.Text);
            if (tbNumEntradesB.Text.Equals("") || numEntradesB < 0)
            {
                tbNumEntradesB.Background = new SolidColorBrush(Colors.LightPink);
                return false;
            }
            else
            {
                tbNumEntradesB.Background = new SolidColorBrush(Colors.Transparent);
            }

            if (carambolesA > grup.CarambolesVictoria)
            {
                tbCarambolesA.Background = new SolidColorBrush(Colors.LightPink);
                return false;
            }
            if (carambolesB > grup.CarambolesVictoria)
            {
                tbCarambolesB.Background = new SolidColorBrush(Colors.LightPink);
                return false;
            }

            if (numEntradesA > grup.LimitEntrades)
            {
                tbCarambolesA.Background = new SolidColorBrush(Colors.LightPink);
                return false;
            }
            if (numEntradesB > grup.LimitEntrades)
            {
                tbCarambolesB.Background = new SolidColorBrush(Colors.LightPink);
                return false;
            }

            if (carambolesA > numEntradesA)
            {
                tbCarambolesA.Background = new SolidColorBrush(Colors.LightPink);
                return false;
            }

            if (carambolesB > numEntradesB)
            {
                tbCarambolesB.Background = new SolidColorBrush(Colors.LightPink);
                return false;
            }

            return true;
        }

        private void lvGrups_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lvPartides.SelectedIndex = -1;
            grup = Torneig.Grups[lvGrups.SelectedIndex];
            lvPartides.ItemsSource = grup.Partides;
            if (grup.Partides.Count > 0)
            {
                lvPartides.SelectedIndex = 0;
            }
        }

        private void lvPartides_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvPartides.SelectedIndex >= 0)
            {
                partida = grup.Partides[lvPartides.SelectedIndex];
                populateForm();
                FormEnabled(true);

                if (partida.EstatPartida == EstatPartida.JUGAT)
                {
                    FormEnabled(false);
                }
            }
        }
    }
}
