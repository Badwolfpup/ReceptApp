using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;

namespace ReceptApp.Model
{
    //Gammal klass som inte används
    public class Priser : INotifyPropertyChanged
    {
        #region InotifyPropertyChanged
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (app.appdata != null) app.appdata.SaveAll();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public event NotifyCollectionChangedEventHandler? CollectionChanged;
        #endregion

        App app = (App)Application.Current;

        public bool SkaÄndraJmfrPris { get; set; } = true;


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

        private double? _summa;
        public double? Summa
        {
            get { return _summa; }
            set
            {
                if (_summa != value)
                {
                    _summa = value;
                    OnPropertyChanged(nameof(Summa));
                }
            }
        }

        private string _namn = "";
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

        private string? _bild;
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

        private int? _antalprodukter;
        public int? AntalProdukter
        {
            get { return _antalprodukter; }
            set
            {
                if (_antalprodukter != value)
                {
                    _antalprodukter = value;
                    OnPropertyChanged(nameof(AntalProdukter));
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


        public Priser(string namn)
        {
            Namn = namn;
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
    }
}
