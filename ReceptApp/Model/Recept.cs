using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceptApp
{
    public class Recept: INotifyPropertyChanged
    {
        #region InotifyPropertyChanged
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        #endregion

        private ObservableCollection<ReceptIngrediens>? _receptingredienser;

        public ObservableCollection<ReceptIngrediens> ReceptIngredienser
        {
            get {  return _receptingredienser;}
            set
            {
                if (_receptingredienser != value)
                {
                    _receptingredienser = value;
                    OnPropertyChanged(nameof(ReceptIngredienser));
                }
            }
        }

        private string _namn;

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

        private int _antalportioner;

        private int _tidigareantalportioner;

        public int Antalportioner
        {
            get { return _antalportioner; }
            set
            {
                if (_antalportioner != value)
                {
                    _tidigareantalportioner = _antalportioner;
                    _antalportioner = value;
                    BeräknaIngrediensMängd();
                    BeräknaVärden();
                    OnPropertyChanged(nameof(Antalportioner));
                }
            }
        }

        private double _portionkalori;
        private double _portionkolhydrat;
        private double _portionsocker; 
        private double _portionfett;
        private double _portionprotein;
        public double PortionKalori 
        {
            get { return _portionkalori; }
            set
            {
                if (_portionkalori != value)
                {
                    _portionkalori = value;
                    OnPropertyChanged(nameof(PortionKalori));
                }

            }
        }

        public double PortionKolhydrat 
        {
            get { return _portionkolhydrat; }
            set
            {
                if (_portionkolhydrat != value)
                {
                    _portionkolhydrat = value;
                    OnPropertyChanged(nameof(PortionKolhydrat));
                }

            }
        }
        public double PortionSocker 
        {
            get { return _portionsocker; }
            set
            {
                if (_portionkolhydrat != value)
                {
                    _portionkolhydrat = value;
                    OnPropertyChanged(nameof(PortionSocker));
                }

            }
        }
        public double PortionFett 
        {
            get { return _portionfett; }
            set
            {
                if (_portionfett != value)
                {
                    _portionfett = value;
                    OnPropertyChanged(nameof(PortionFett));
                }

            }
        }
        public double PortionProtein 
        {
            get { return _portionprotein; }
            set
            {
                if (_portionprotein != value)
                {
                    _portionprotein = value;
                    OnPropertyChanged(nameof(PortionProtein));
                }

            }
        }



        public Recept(int antalportioner)
        {            
            ReceptIngredienser = new ObservableCollection<ReceptIngrediens>();
            Antalportioner = antalportioner;
            _tidigareantalportioner = antalportioner;
            ReceptIngredienser.CollectionChanged += KaloriPortion_CollectionChanged;
            
        }

        private void KaloriPortion_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            BeräknaVärden();
        }

        public void BeräknaVärden()
        {
            PortionKalori = 0; PortionProtein = 0; PortionFett = 0; PortionKolhydrat = 0; PortionSocker = 0;
            foreach (var item in ReceptIngredienser)
            {
                PortionKalori += item.Ingrediens.Kalori * item.AntalGram / Antalportioner;
                PortionKolhydrat += item.Ingrediens.Kolhydrat * item.AntalGram / Antalportioner;
                PortionSocker += item.Ingrediens.Socker * item.AntalGram / Antalportioner;
                PortionFett += item.Ingrediens.Fett * item.AntalGram / Antalportioner;
                PortionProtein += item.Ingrediens.Protein * item.AntalGram / Antalportioner;
            }
        }

        private void BeräknaIngrediensMängd()
        {
            foreach (var item in ReceptIngredienser)
            {
                item.Mängd = item.Mängd / _tidigareantalportioner * _antalportioner;
            }
        }


    }
}
