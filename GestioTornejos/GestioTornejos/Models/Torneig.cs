using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestioTornejos.Models
{
    public class Torneig
    {
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

        private bool preinscripcioOberta;

        public bool PreinscripcioOberta
        {
            get { return preinscripcioOberta; }
            set { preinscripcioOberta = value; }
        }

        private List<Grup> grups;

        public List<Grup> Grups
        {
            get { return grups; }
            set { grups = value; }
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
