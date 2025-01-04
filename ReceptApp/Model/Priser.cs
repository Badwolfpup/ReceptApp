using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ReceptApp.Model
{

    public class Priser : INotifyPropertyChanged
    {
        #region InotifyPropertyChanged
        protected virtual void OnPropertyChanged(string propertyName)
        {
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

        private double? _prisPerKg = 0;
        public double? PrisPerKg
        {
            get { return _prisPerKg; }
            set
            {
                if (_prisPerKg != value)
                {
                    _prisPerKg = value;
                    OnPropertyChanged(nameof(PrisPerKg));
                }
            }
        }

        private double? _prisPerLiter = 0;
        public double? PrisPerLiter
        {
            get { return _prisPerLiter; }
            set
            {
                if (_prisPerLiter != value)
                {
                    _prisPerLiter = value;
                    OnPropertyChanged(nameof(PrisPerLiter));
                }
            }
        }

        private double? _prisPerSt = 0;
        public double? PrisPerSt
        {
            get { return _prisPerSt; }
            set
            {
                if (_prisPerSt != value)
                {
                    _prisPerSt = value;
                    OnPropertyChanged(nameof(PrisPerSt));
                }
            }
        }

        private double? _pris = 0;
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

        private double? _mängd = 0;
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

        private int _antalprodukter;
        public int AntalProdukter
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

        private int? _antal;
        public int? Antal
        {
            get { return _antal; }
            set
            {
                if (_antal != value)
                {
                    _antal = value;
                    OnPropertyChanged(nameof(Antal));
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

        private void JämförelsePriser()
        {
            if (Mått == "g")
            {
                JämförelsePris = Pris / Mängd * 1000;
            }
            else if (Mått == "kg")
            {
                JämförelsePris = Pris / Mängd;
            }
            else if (Mått == "l")
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
    }
}
