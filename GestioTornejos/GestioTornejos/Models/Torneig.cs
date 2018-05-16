using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestioTornejos.Models
{
    public class Torneig : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private Modalitat modalitat;

        public Modalitat Modalitat
        {
            get { return modalitat; }
            set { modalitat = value; }
        }

        private String nom;

        public String Nom
        {
            get { return nom; }
            set { nom = value; }
        }

        private DateTime dataInici;

        public DateTime DataInici
        {
            get { return dataInici; }
            set { dataInici = value; }
        }

        private DateTime dataFi;

        public DateTime DataFi
        {
            get { return dataFi; }
            set { dataFi = value; }
        }

        private bool preinscripcioOberta;

        public bool PreinscripcioOberta
        {
            get { return preinscripcioOberta; }
            set { preinscripcioOberta = value; }
        }

        private ObservableCollection<Grup> grups;

        public ObservableCollection<Grup> Grups
        {
            get { return grups; }
            set { grups = value; }
        }

        private ObservableCollection<Inscripcio> inscripcions;

        public ObservableCollection<Inscripcio> Inscripcions
        {
            get { return inscripcions; }
            set { inscripcions = value; }
        }

        public Torneig(int id, Modalitat modalitat, string nom, DateTime dataInici, bool preinscripcioOberta)
        {
            Id = id;
            Modalitat = modalitat;
            Nom = nom;
            DataInici = dataInici;
            PreinscripcioOberta = preinscripcioOberta;
        }
    }
}
