using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestioTornejos.Models
{
    public class Grup : INotifyPropertyChanged
    {
        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private String descripcio;

        public String Descripcio
        {
            get { return descripcio; }
            set { descripcio = value; }
        }

        private int carambolesVictoria;

        public int CarambolesVictoria
        {
            get { return carambolesVictoria; }
            set { carambolesVictoria = value; }
        }

        private int limitEntrades;

        public int LimitEntrades
        {
            get { return limitEntrades; }
            set { limitEntrades = value; }
        }

        private ObservableCollection<Inscripcio> inscripcions;

        public ObservableCollection<Inscripcio> Inscripcions
        {
            get { return inscripcions; }
            set { inscripcions = value; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Grup(int id, string descripcio, int carambolesVictoria, int limitEntrades)
        {
            Id = id;
            Descripcio = descripcio;
            CarambolesVictoria = carambolesVictoria;
            LimitEntrades = limitEntrades;
            inscripcions = new ObservableCollection<Inscripcio>();
        }

        public override bool Equals(object obj)
        {
            var grup = obj as Grup;
            return grup != null &&
                   Id == grup.Id;
        }

        public override int GetHashCode()
        {
            return 2108858624 + Id.GetHashCode();
        }
    }
}
