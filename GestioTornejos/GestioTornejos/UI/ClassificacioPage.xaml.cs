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
    public sealed partial class ClassificacioPage : Page
    {
        private Shared mainPageShared = new Shared(0, new ObservableCollection<Torneig>(), new ListView());
        private Torneig Torneig;
        private Grup grup;
        private ObservableCollection<Soci> socis = new ObservableCollection<Soci>();
        private ObservableCollection<SociRanking> socisRanking = new ObservableCollection<SociRanking>();

        public ClassificacioPage()
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
        }

        private void lvGrups_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            socis.Clear();

            grup = Torneig.Grups[lvGrups.SelectedIndex];
            foreach(Inscripcio inscripcio in grup.Inscripcions)
            {
                socis.Add(inscripcio.Soci);
            }

            socisRanking = new ObservableCollection<SociRanking>();

            foreach(Soci soci in socis)
            {
                SociRanking sociRanking = new SociRanking();
            
                string nom = soci.Nom + " " + soci.Cognom1 + " " + soci.Cognom2;
                int partidesJugades = GetCountPartidesJugades(soci);
                int partidesGuanyades = GetCountPartidesGuanyades(soci);
                int partidesPerdudes = GetCountPartidesPerdudes(soci);
                double coeficient = GetCoeficient(soci);

                sociRanking.Nom = nom;
                sociRanking.PartidesJugades = partidesJugades;
                sociRanking.PartidesGuanyades = partidesGuanyades;
                sociRanking.PartidesPerdudes = partidesPerdudes;
                sociRanking.Coeficient = coeficient;

                socisRanking.Add(sociRanking);
            }

            var socisRankingOrdered = socisRanking.OrderByDescending(sr => sr.Coeficient);

            lvSocis.ItemsSource = socisRankingOrdered;
        }

        private int GetCountPartidesJugades(Soci soci)
        {
            int count = 0;

            foreach(Partida partida in grup.Partides)
            {
                if ((partida.SociA.Equals(soci) || partida.SociB.Equals(soci)) && partida.EstatPartida == EstatPartida.JUGAT)
                {
                    count++;
                }
            }

            return count;
        }

        private int GetCountPartidesGuanyades(Soci soci)
        {
            int count = 0;

            foreach (Partida partida in grup.Partides)
            {
                if (partida.EstatPartida == EstatPartida.JUGAT)
                {
                    if ((partida.Guanyador == Guanyador.A && partida.SociA.Equals(soci)) 
                        || (partida.Guanyador == Guanyador.B && partida.SociB.Equals(soci)))
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private int GetCountPartidesPerdudes(Soci soci)
        {
            int count = 0;

            foreach (Partida partida in grup.Partides.Where(p => p.SociA.Equals(soci) || p.SociB.Equals(soci)))
            {

                if ((partida.EstatPartida == EstatPartida.JUGAT && (partida.Guanyador == Guanyador.A && !partida.SociA.Equals(soci))
                    || (partida.Guanyador == Guanyador.B && !partida.SociB.Equals(soci))))
                {
                    count++;
                }
                
            }

            return count;
        }

        private double GetCoeficient(Soci soci)
        {
            double coeficient = 0;

            foreach(EstadisticaModalitat estadistica in soci.Estadistiques)
            {
                if (estadistica.Modalitat.Equals(Torneig.Modalitat))
                {
                    coeficient = estadistica.CoeficientBase;
                    break;
                }
            }

            return coeficient;
        }
    }
}
