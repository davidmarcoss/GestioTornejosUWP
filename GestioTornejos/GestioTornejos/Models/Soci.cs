using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestioTornejos.Models
{
    public class Soci
    {
        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private String nom;

        public String Nom
        {
            get { return nom; }
            set { nom = value; }
        }

        private String cognom1;

        public String Cognom1
        {
            get { return cognom1; }
            set { cognom1 = value; }
        }

        private String cognom2;

        public String Cognom2
        {
            get { return cognom2; }
            set { cognom2 = value; }
        }

        public Soci(int id, string nom, string cognom1, string cognom2)
        {
            Id = id;
            Nom = nom;
            Cognom1 = cognom1;
            Cognom2 = cognom2;
        }

        public override string ToString()
        {
            return Nom + " " + Cognom1 + " " + Cognom2;
        }

        public override bool Equals(object obj)
        {
            var soci = obj as Soci;
            return soci != null &&
                   Id == soci.Id;
        }

        public override int GetHashCode()
        {
            return 2108858624 + Id.GetHashCode();
        }
    }
}
