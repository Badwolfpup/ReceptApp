using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ReceptApp.Model
{
    public class Vara: INotifyPropertyChanged
    {
        #region InotifyPropertyChanged
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        //public event NotifyCollectionChangedEventHandler? CollectionChanged;
        #endregion
        private string? _bild; //Genväg till bild
        public string? Bild
        {
            get { return _bild; }
            set
            {
                if (_bild != value)
                {
                    _bild = value;

                    OnPropertyChanged(nameof(Bild));
                }
            }
        }

        public bool SkaÄndraJmfrPris { get; set; } = true;

        private string _namn = ""; //Ingrediensens namn
        public string Namn
        {
            get { return _namn; }
            set
            {
                if (_namn != value)
                {
                    _namn = value;
                    OnPropertyChanged(nameof(Namn));
                }
            }
        }

        private double? _jämförelsepris;
        public double? JämförelsePris
        {
            get { return _jämförelsepris; }
            set
            {
                if (_jämförelsepris != value)
                {
                    _jämförelsepris = value;
                    OnPropertyChanged(nameof(JämförelsePris));
                }
            }
        }

        private double? _pris;
        public double? Pris
        {
            get { return _pris; }
            set
            {
                if (_pris != value)
                {
                    _pris = value;
                    OnPropertyChanged(nameof(Pris));
                }
            }
        }

        private double? _mängd;
        public double? Mängd
        {
            get { return _mängd; }
            set
            {
                if (_mängd != value)
                {
                    _mängd = value;
                    OnPropertyChanged(nameof(Mängd));
                }
            }
        }

        private string? _info;
        public string? Info
        {
            get { return _info; }
            set
            {
                if (_info != value)
                {
                    _info = value;
                    OnPropertyChanged(nameof(Info));
                }
            }
        }

        private string? _typ;
        public string? Typ
        {
            get { return _typ; }
            set
            {
                if (_typ != value)
                {
                    _typ = value;
                    OnPropertyChanged(nameof(Typ));
                }
            }
        }

        private bool _ärintelösvikt = true;
        public bool ÄrInteLösvikt
        {
            get { return _ärintelösvikt; }
            set
            {
                if (_ärintelösvikt != value)
                {
                    _ärintelösvikt = value;
                    OnPropertyChanged(nameof(ÄrInteLösvikt));
                }
            }
        }

        private bool _ärovrigvara = true;
        public bool ÄrInteÖvrigVara
        {
            get { return _ärovrigvara; }
            set
            {
                if (_ärovrigvara != value)
                {
                    _ärovrigvara = value;
                    OnPropertyChanged(nameof(ÄrInteÖvrigVara));
                }
            }
        }

        private string? _mått;
        public string? Mått
        {
            get { return _mått; }
            set
            {
                if (_mått != value)
                {
                    _mått = value;
                    if (SkaÄndraJmfrPris) JämförelsePriser();
                    OnPropertyChanged(nameof(Mått));
                }
            }
        }

        private string? _förpackningstyp;
        public string? Förpackningstyp
        {
            get { return _förpackningstyp; }
            set
            {
                if (_förpackningstyp != value)
                {
                    _förpackningstyp = value;
                    OnPropertyChanged(nameof(Förpackningstyp));
                }
            }
        }

        private bool _ärTillagdIRecept = false; //Om ingrediensen är tillagd i ett recept (medans man håller på att skapa recept)
        [JsonIgnore]
        public bool ÄrTillagdIRecept
        {
            get => _ärTillagdIRecept;
            set
            {
                if (_ärTillagdIRecept != value)
                {
                    _ärTillagdIRecept = value;
                    OnPropertyChanged(nameof(ÄrTillagdIRecept));
                }
            }
        }

        public Naringsvarde Naring { get; set; }

        public Vara()
        {
            Naring = new Naringsvarde();
        }

        public void JämförelsePriser()
        {
            if (Mått == "g")
            {
                JämförelsePris = Pris / Mängd * 1000;
            }
            else if (Mått == "kg")
            {
                JämförelsePris = Pris / Mängd;
            }
            else if (Mått == "L")
            {
                JämförelsePris = Pris / Mängd;
            }
            else if (Mått == "dl")
            {
                JämförelsePris = Pris / Mängd * 10;
            }
            else if (Mått == "st")
            {
                JämförelsePris = Pris / Mängd;
            }
        }

        public void PrisSomJmfrPris()
        {
            if (Mått == "g")
            {
                Pris = Mängd / 1000 * JämförelsePris;
            }
            else if (Mått == "dl")
            {
                Pris = Mängd / 10 * JämförelsePris;
            }
            else
            {
                Pris = Mängd * JämförelsePris;
            }
        }

        public Vara Copy()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            };
            var json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<Vara>(json, settings);
        }
    }
}
