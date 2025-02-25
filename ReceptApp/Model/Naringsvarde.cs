using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceptApp.Model
{
    public class Naringsvarde: INotifyPropertyChanged
    {
        #region InotifyPropertyChanged
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        //public event NotifyCollectionChangedEventHandler? CollectionChanged;
        #endregion

        public Naringsvarde()
        {
            ViktMått = new ObservableCollection<string>();
            if (!ViktMått.Any(x => x == "Gram")) ViktMått.Add("Gram");
        }

        private ObservableCollection<string> _viktMått; //Lista med viktmått
        public ObservableCollection<string> ViktMått
        {
            get { return _viktMått; }
            set
            {
                if (_viktMått != value)
                {

                    if (_viktMått != null)
                    {
                        // Detach the event from the old collection
                        _viktMått.CollectionChanged -= ViktMått_CollectionChanged;
                    }

                    _viktMått = value;

                    if (_viktMått != null)
                    {
                        // Attach the event to the new collection
                        _viktMått.CollectionChanged += ViktMått_CollectionChanged;
                    }
                    OnPropertyChanged(nameof(ViktMått));
                }
            }
        }

        private void ViktMått_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            TaBortExtraGram();
        }


        //Tar bort gram från viktmått om det finns fler än ett
        private void TaBortExtraGram()
        {
            if (ViktMått.Count(x => x == "Gram") > 1)
            {
                var item = ViktMått.FirstOrDefault(x => x == "Gram");
                if (item != default) ViktMått.Remove(item);
            }
        }

        //Lägger till Deciliter, Matsked, Tesked och Kryddmått i viktmått om GramPerDl har ett värde
        private void LäggTillGramPerDL()
        {
            if (GramPerDl > 0)
            {
                HarGramPerDL = true;
                if (!ViktMått.Any(x => x == "Deciliter"))
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
                if (ViktMått.Any(x => x == "Deciliter"))
                {
                    ViktMått.Remove("Deciliter");
                    ViktMått.Remove("Matsked");
                    ViktMått.Remove("Tesked");
                    ViktMått.Remove("Kryddmått");
                }
            }
        }

        private void LäggTillStyck()
        {
            if (Styck > 0)
            {
                HarStyck = true;
                if (!ViktMått.Any(x => x == "Stycken"))
                {
                    ViktMått.Add("Stycken");
                }
            }
            else
            {
                HarStyck = false;
                if (ViktMått.Any(x => x == "Stycken"))
                {
                    ViktMått.Remove("Stycken");
                }
            }
        }

        private bool _harStyck; //Om styck har ett värde och att checkoboxen ska vara ibockad
        public bool HarStyck
        {
            get => _harStyck;
            set
            {
                if (_harStyck != value)
                {
                    _harStyck = value;
                    OnPropertyChanged(nameof(HarStyck));
                }
            }
        }

        private bool _harGramPerDL; //Om gram per deciliter har ett värde och att checkoboxen ska vara ibockad
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

        private int _kalori; //Kalorier per 100g
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

        private int _protein; //Protein per 100g
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

        private int _kolhydrat; //Kolhydrater per 100g
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

        private int _fett; //Fett per 100g
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

        private int? _gramperdl; //Gram per deciliter
        public int? GramPerDl
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

        private int? _styck; //Vikt per styck
        public int? Styck
        {
            get { return _styck; }
            set
            {
                if (_styck != value)
                {
                    _styck = value;
                    LäggTillStyck();
                    OnPropertyChanged(nameof(Styck));
                }
            }
        }


    }
}
