using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ReceptApp
{
    public class Ingrediens : INotifyPropertyChanged, INotifyCollectionChanged
    {
        #region InotifyPropertyChanged
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public event NotifyCollectionChangedEventHandler? CollectionChanged;
        #endregion

        #region Properties
        private string _namn;

        private int _kalori;
        private int _protein;
        private int _kolhydrat;
        private int _socker;
        private int _fett;
        private int _gramperdl;
        private int _liten;
        private int _medel;
        private int _stor;
        private string _bild;
        private bool _harGramPerDL;
        private bool _harAntalLiten;
        private bool _harAntalMedel;
        private bool _harAntalStor;

        //public int ID { get; set; } för DB

        public bool HarGramPerDL
        {
            get => _harGramPerDL;
            set
            {
                if (_harGramPerDL != value)
                {
                    _harGramPerDL = value;
                    OnPropertyChanged(nameof(HarGramPerDL));
                }
            }
        }
        public bool HarAntalLiten
        {
            get => _harAntalLiten;
            set
            {
                if (_harAntalLiten != value)
                {
                    _harAntalLiten = value;
                    OnPropertyChanged(nameof(HarAntalLiten));
                }
            }
        }

        public bool HarAntalMedel
        {
            get => _harAntalMedel;
            set
            {
                if (_harAntalMedel != value)
                {
                    _harAntalMedel = value;
                    OnPropertyChanged(nameof(HarAntalMedel));
                }
            }
        }

        public bool HarAntalStor
        {
            get => _harAntalStor;
            set
            {
                if (_harAntalStor != value)
                {
                    _harAntalStor = value;
                    OnPropertyChanged(nameof(HarAntalStor));
                }
            }
        }

        public ObservableCollection<string> ViktMått { get; set; }

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
        public int Kalori
        {
            get { return _kalori; }
            set
            {
                if (_kalori != value)
                {
                    _kalori = value;
                    OnPropertyChanged(nameof(Kalori));
                }
            }
        }
        public int Protein
        {
            get { return _protein; }
            set
            {
                if (_protein != value)
                {
                    _protein = value;
                    OnPropertyChanged(nameof(Protein));
                }
            }
        }
        public int Kolhydrat
        {
            get { return _kolhydrat; }
            set
            {
                if (_kolhydrat != value)
                {
                    _kolhydrat = value;
                    OnPropertyChanged(nameof(Kolhydrat));
                }
            }
        }
        public int Socker
        {
            get { return _socker; }
            set
            {
                if (_socker != value)
                {
                    _socker = value;
                    OnPropertyChanged(nameof(Socker));
                }
            }
        }
        public int Fett
        {
            get { return _fett; }
            set
            {
                if (_fett != value)
                {
                    _fett = value;
                    OnPropertyChanged(nameof(Fett));
                }
            }
        }

        public int GramPerDl
        {
            get { return _gramperdl; }
            set
            {
                if (_gramperdl != value)
                {
                    _gramperdl = value;
                    LäggTillGramPerDL();
                    OnPropertyChanged(nameof(GramPerDl));
                }
            }
        }
        public int Liten
        {
            get { return _liten; }
            set
            {
                if (_liten != value)
                {
                    _liten = value;
                    LäggTillAntalLiten();
                    OnPropertyChanged(nameof(Liten));
                }
            }
        }
        public int Medel
        {
            get { return _medel; }
            set
            {
                if (_medel != value)
                {
                    _medel = value;
                    LäggTillAntalMedel();
                    OnPropertyChanged(nameof(Medel));
                }

            }
        }
        public int Stor
        {
            get { return _stor; }
            set
            {
                if (_stor != value)
                {
                    _stor = value;
                    LäggTillAntalStor();
                    OnPropertyChanged(nameof(Stor));
                }
            }
        }
        public string Bild
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
        #endregion


        public Ingrediens()
        {
            ViktMått = new ObservableCollection<string>();
            //if (!ViktMått.Contains("Gram")) ViktMått.Add("Gram");
            ViktMått.CollectionChanged += ViktMått_CollectionChanged;
        }

        private void ViktMått_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            TaBortExtraGram();
        }

        private void TaBortExtraGram()
        {
            if (ViktMått.Count(x => x == "Gram") > 1)
            {
                ViktMått.Remove("Gram");
            }
        }

        private void LäggTillGramPerDL()
        {
            if (GramPerDl > 0)
            {
                HarGramPerDL = true;
                if (!ViktMått.Contains("Deciliter"))
                {
                    ViktMått.Add("Deciliter");
                    ViktMått.Add("Matsked");
                    ViktMått.Add("Tesked");
                    ViktMått.Add("Kryddmått");
                }
            }
            else
            {
                HarGramPerDL = false;
                if (ViktMått.Contains("Deciliter"))
                {
                    ViktMått.Remove("Deciliter");
                    ViktMått.Remove("Matsked");
                    ViktMått.Remove("Tesked");
                    ViktMått.Remove("Kryddmått");
                }
            }
        }

        private void LäggTillAntalLiten()
        {
            if (Liten > 0)
            {
                HarAntalLiten = true;
                if (!ViktMått.Contains("Antal liten")) ViktMått.Add("Antal liten");
            }
            else 
            {
                HarAntalLiten = false;
                if (ViktMått.Contains("Antal liten")) ViktMått.Remove("Antal liten"); 
            }

        }

        private void LäggTillAntalMedel()
        {
            if (Medel > 0)
            {
                HarAntalMedel = true;
                if (!ViktMått.Contains("Antal medel")) ViktMått.Add("Antal medel");
            }
            else
            {
                HarAntalMedel = false;
                if (ViktMått.Contains("Antal medel")) ViktMått.Remove("Antal medel");
            }
        }

        private void LäggTillAntalStor()
        {
            if (Stor > 0)
            {
                HarAntalStor = true;
                if (!ViktMått.Contains("Antal stor")) ViktMått.Add("Antal stor");
            }
            else
            {
                HarAntalStor = false;
                if (ViktMått.Contains("Antal stor")) ViktMått.Remove("Antal stor");
            }
        }

    }
}
