using GestioTornejos.DB;
using GestioTornejos.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
    public sealed partial class GrupsPage : Page
    {
        Shared mainPageShared = new Shared(0, new ObservableCollection<Torneig>(), new ListView());
        private Torneig Torneig;
        private Grup grup;
        private bool isNou;

        public GrupsPage()
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

            lvInscrits.ItemsSource = Torneig.Inscripcions;

            isNou = true;
        }

        private void GrupItemRow_Tapped(object sender, TappedRoutedEventArgs e)
        {
            grup = Torneig.Grups[lvGrups.SelectedIndex];
            populateForm();
        }

        private void resetForm()
        {
            tbDescripcio.Text = "";
            tbCarambolesVictoria.Text = "";
            tbLimitEntrades.Text = "";
            lvInscrits.SelectedItem = null;
        }

        private bool checkForm()
        {
            bool status = true;

            if (tbDescripcio.Text.Length <= 0 || tbDescripcio.Text.Equals(""))
            {
                tbDescripcio.Background = new SolidColorBrush(Colors.Red);
                status = false;
            }
            else
            {
                tbDescripcio.Background = new SolidColorBrush(Colors.Transparent);
            }

            return status;
        }

        private void populateForm()
        {
            if (grup != null)
            {
                tbDescripcio.Text = grup.Descripcio;
                tbCarambolesVictoria.Text = grup.CarambolesVictoria.ToString();
                tbLimitEntrades.Text = grup.LimitEntrades.ToString();
                lvInscritsGrup.ItemsSource = grup.Inscripcions;
            }
        }

        private void btnCrear_Click(object sender, RoutedEventArgs e)
        {
            isNou = true;
            resetForm();
            lvGrups.SelectedItem = null;
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            ObservableCollection<Soci> socisSeleccionats = new ObservableCollection<Soci>();
            foreach (Object item in lvInscrits.SelectedItems)
            {
                socisSeleccionats.Add((Soci)item);
            }

            if (checkForm())
            {
                if (isNou)
                {
                    Grup nouGrup = new Grup(-1, tbDescripcio.Text, Int32.Parse(tbCarambolesVictoria.Text), Int32.Parse(tbLimitEntrades.Text));
                    foreach(Soci soci in socisSeleccionats)
                    {
                        Inscripcio inscripcio = Torneig.Inscripcions.Single(Ins => Ins.Soci.Equals(soci));
                        if (inscripcio != null)
                        {
                            nouGrup.Inscripcions.Add(inscripcio);
                        }
                    }
                    GrupDB.Insert(Torneig, nouGrup);

                    grup = nouGrup;
                    Torneig.Grups.Add(nouGrup);
                }
                else
                {
                    grup.Descripcio = tbDescripcio.Text;
                    grup.CarambolesVictoria = Int32.Parse(tbCarambolesVictoria.Text);
                    grup.LimitEntrades = Int32.Parse(tbLimitEntrades.Text);
                    GrupDB.Update(Torneig, grup);
                }

                populateForm();
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            resetForm();
        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void tbCarambolesVictoria_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(e.Key.ToString(), "[0-9]"))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void tbLimitEntrades_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(e.Key.ToString(), "[0-9]"))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private bool isNumeric(String text)
        {
            try
            {
                Int32.Parse(text);
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }
    }
}
