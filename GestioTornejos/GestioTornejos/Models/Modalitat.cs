using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestioTornejos.Models
{
    public class Modalitat : INotifyPropertyChanged
    {
        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private String descripcio;

        public event PropertyChangedEventHandler PropertyChanged;

        public String Descripcio
        {
            get { return descripcio; }
            set { descripcio = value; }
        }

        public Modalitat(int id, string descripcio)
        {
            Id = id;
            Descripcio = descripcio;
        }

        public override string ToString()
        {
            return Descripcio;
        }

        public override bool Equals(object obj)
        {
            var modalitat = obj as Modalitat;
            return modalitat != null &&
                   Id == modalitat.Id;
        }

        public override int GetHashCode()
        {
            return 2108858624 + Id.GetHashCode();
        }
    }
}
