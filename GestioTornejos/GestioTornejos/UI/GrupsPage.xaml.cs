using GestioTornejos.DB;
using GestioTornejos.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace GestioTornejos.UI
{
    public sealed partial class GrupsPage : Page
    {
        private Shared mainPageShared = new Shared(0, new ObservableCollection<Torneig>(), new ListView());
        private Torneig Torneig;
        private Grup grup;
        private bool isNou;
        private ObservableCollection<Inscripcio> inscripcionsCopia;
        private ObservableCollection<Inscripcio> inscripcionsGrupCopia;

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

            inscripcionsCopia = new ObservableCollection<Inscripcio>(Torneig.Inscripcions);
            lvInscrits.ItemsSource = inscripcionsCopia;

            isNou = true;

            if (Torneig.PreinscripcioOberta)
            {
                DisableForm();
            }
            else
            {
                EnableForm();
            }
        }

        private void DisableForm()
        {
            tbDescripcio.IsEnabled = tbCarambolesVictoria.IsEnabled = tbLimitEntrades.IsEnabled = btnGuardar.IsEnabled 
                = btnCancelar.IsEnabled = btnCrear.IsEnabled = btnEliminar.IsEnabled = false;
        }

        private void EnableForm()
        {
            tbDescripcio.IsEnabled = tbCarambolesVictoria.IsEnabled = tbLimitEntrades.IsEnabled = btnGuardar.IsEnabled
                = btnCancelar.IsEnabled = btnCrear.IsEnabled = btnEliminar.IsEnabled = true;
        }

        private void GrupItemRow_Tapped(object sender, TappedRoutedEventArgs e)
        {
            grup = Torneig.Grups[lvGrups.SelectedIndex];
            populateForm();
            EnableForm();
            isNou = false;
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
                inscripcionsGrupCopia = new ObservableCollection<Inscripcio>(grup.Inscripcions);
                lvInscritsGrup.ItemsSource = inscripcionsGrupCopia;
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
            if (checkForm())
            {
                if (isNou)
                {
                    Grup nouGrup = new Grup(-1, tbDescripcio.Text, Int32.Parse(tbCarambolesVictoria.Text), Int32.Parse(tbLimitEntrades.Text));
                    nouGrup.Inscripcions = new ObservableCollection<Inscripcio>(inscripcionsGrupCopia);
                    GrupDB.Insert(Torneig, nouGrup);

                    grup = nouGrup;
                    Torneig.Grups.Add(nouGrup);
                }
                else
                {
                    grup.Descripcio = tbDescripcio.Text;
                    grup.CarambolesVictoria = Int32.Parse(tbCarambolesVictoria.Text);
                    grup.LimitEntrades = Int32.Parse(tbLimitEntrades.Text);
                    grup.Inscripcions = new ObservableCollection<Inscripcio>(inscripcionsGrupCopia);
                    GrupDB.Update(Torneig, grup);
                }

                populateForm();
            }

            Torneig.Inscripcions = new ObservableCollection<Inscripcio>(inscripcionsCopia);
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            resetForm();
            inscripcionsCopia = new ObservableCollection<Inscripcio>(Torneig.Inscripcions);
            lvInscrits.ItemsSource = inscripcionsCopia;
            inscripcionsGrupCopia = null;
            lvInscritsGrup.ItemsSource = inscripcionsGrupCopia;
            populateForm();
            isNou = false;
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
                if (GrupDB.Delete(grup))
                {
                    foreach(Inscripcio inscripcio in grup.Inscripcions)
                    {
                        inscripcionsCopia.Add(inscripcio);
                    }

                    grup.Inscripcions.Clear();
                    inscripcionsGrupCopia.Clear();

                    Torneig.Grups.Remove(grup);
                    lvInscrits.ItemsSource = inscripcionsCopia;
                    lvGrups.ItemsSource = Torneig.Grups;

                    if (lvGrups.Items.Count > 0)
                    {
                        lvGrups.SelectedIndex = 0;
                        mainPageShared.IdxSelected = 0;
                    }

                    grup = null;
                }
            }
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

        private void lvInscrits_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (lvGrups.SelectedIndex >= 0)
            {
                Inscripcio inscripcio = (Inscripcio)lvInscrits.SelectedItem;
                if (inscripcio != null)
                {
                    inscripcionsGrupCopia.Add(inscripcio);
                    inscripcionsCopia.Remove(inscripcio);
                }
            }
        }

        private void lvInscritsGrup_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (lvGrups.SelectedIndex >= 0)
            {
                Inscripcio inscripcio = (Inscripcio)lvInscritsGrup.SelectedItem;
                if (inscripcio != null)
                {
                    inscripcionsCopia.Add(inscripcio);
                    inscripcionsGrupCopia.Remove(inscripcio);
                }
            }
        }

        private void btnEmparellaments_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Fer emparellaments
        }
    }
}
