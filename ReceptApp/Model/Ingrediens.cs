using Newtonsoft.Json;
using ReceptApp.Model;
using ReceptApp.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
        private ObservableCollection<string> _viktMått; //Lista med viktmått
        public ObservableCollection<string> ViktMått
        {
            get { return _viktMått; }
            set
            {
                if (_viktMått != value)
                {
                    _viktMått = value;
                    OnPropertyChanged(nameof(ViktMått));
                }
            }
        }

        private ObservableCollection<Priser> _prislista; //Lista med priser
        public ObservableCollection<Priser> PrisLista
        {
            get { return _prislista; }
            set
            {
                if (_prislista != value)
                {
                    _prislista = value;
                    OnPropertyChanged(nameof(PrisLista));
                }
            }
        }

        private string _namn; //Ingrediensens namn
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

        private string? _bild; //Gneväg till bild
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

        //private int _liten; //Vikt för liten storlek av ingreiensen
        //public int Liten
        //{
        //    get { return _liten; }
        //    set
        //    {
        //        if (_liten != value)
        //        {
        //            _liten = value;
        //            LäggTillAntalLiten();
        //            OnPropertyChanged(nameof(Liten));
        //        }
        //    }
        //}

        //private int _medel; //Vikt för medel storlek av ingrediensen
        //public int Medel
        //{
        //    get { return _medel; }
        //    set
        //    {
        //        if (_medel != value)
        //        {
        //            _medel = value;
        //            LäggTillAntalMedel();
        //            OnPropertyChanged(nameof(Medel));
        //        }

        //    }
        //}

        //private int _stor; //Vikt för stor storlek av ingrediensen
        //public int Stor
        //{
        //    get { return _stor; }
        //    set
        //    {
        //        if (_stor != value)
        //        {
        //            _stor = value;
        //            LäggTillAntalStor();
        //            OnPropertyChanged(nameof(Stor));
        //        }
        //    }
        //}

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

        //private double _pris; //Pris per vald enhet
        //public double Pris
        //{
        //    get { return _pris; }
        //    set
        //    {
        //        if (_pris != value)
        //        {
        //            _pris = value;
        //            OnPropertyChanged(nameof(Pris));
        //        }
        //    }
        //}

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

        private bool _harPris; //Om pris har ett värde och att checkoboxen ska vara ibockad
        public bool HarPris
        {
            get => _harPris;
            set
            {
                if (_harPris != value)
                {
                    _harPris = value;
                    OnPropertyChanged(nameof(HarPris));
                }
            }
        }

        #region GamlaProperties
        //private bool _harAntalLiten; //Om liten har ett värde och att checkoboxen ska vara ibockad
        //public bool HarAntalLiten
        //{
        //    get => _harAntalLiten;
        //    set
        //    {
        //        if (_harAntalLiten != value)
        //        {
        //            _harAntalLiten = value;
        //            OnPropertyChanged(nameof(HarAntalLiten));
        //        }
        //    }
        //}

        //private bool _harAntalMedel; //Om medel har ett värde och att checkoboxen ska vara ibockad
        //public bool HarAntalMedel
        //{
        //    get => _harAntalMedel;
        //    set
        //    {
        //        if (_harAntalMedel != value)
        //        {
        //            _harAntalMedel = value;
        //            OnPropertyChanged(nameof(HarAntalMedel));
        //        }
        //    }
        //}

        //private bool _harAntalStor; //Om stor har ett värde och att checkoboxen ska vara ibockad
        //public bool HarAntalStor
        //{
        //    get => _harAntalStor;
        //    set
        //    {
        //        if (_harAntalStor != value)
        //        {
        //            _harAntalStor = value;
        //            OnPropertyChanged(nameof(HarAntalStor));
        //        }
        //    }
        //}
        #endregion

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
                    if (app.FilteredIngredientList != null) app.FilteredIngredientList.Refresh();
                }
            }
        }

        public double AntalGram { get; set; } //Det relativa av ingrediensens tillagda vikt i förhållande till 100g. Ex 125g = 1,25



        #endregion


        public Ingrediens()
        {
            ViktMått = new ObservableCollection<string>();
            if (!ViktMått.Any(x => x == "Gram")) ViktMått.Add("Gram");
            ViktMått.CollectionChanged += ViktMått_CollectionChanged;
            PrisLista = new ObservableCollection<Priser>();
        }

        App app = (App)Application.Current;

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
                if (ViktMått.Any(x => x == "Styck"))
                {
                    ViktMått.Remove("Stycken");
                }
            }
        }
        #region GamlaMetoder
        ////Lägger till Antal liten i viktmått om Liten har ett värde
        //private void LäggTillAntalLiten()
        //{
        //    if (Liten > 0)
        //    {
        //        HarAntalLiten = true;
        //        if (!ViktMått.Any(x => x == "Antal liten")) ViktMått.Add("Antal liten");
        //    }
        //    else 
        //    {
        //        HarAntalLiten = false;
        //        if (ViktMått.Any(x => x == "Antal liten")) ViktMått.Remove("Antal liten"); 
        //    }

        //}

        ////Lägger till Antal medel i viktmått om Medel har ett värde
        //private void LäggTillAntalMedel()   
        //{
        //    if (Medel > 0)
        //    {
        //        HarAntalMedel = true;
        //        if (!ViktMått.Any(x => x == "Antal medel")) ViktMått.Add("Antal medel");
        //    }
        //    else
        //    {
        //        HarAntalMedel = false;
        //        if (ViktMått.Any(x => x == "Antal medel")) ViktMått.Remove("Antal medel");
        //    }
        //}

        ////Lägger till Antal stor i viktmått om Stor har ett värde
        //private void LäggTillAntalStor()
        //{
        //    if (Stor > 0)
        //    {
        //        HarAntalStor = true;
        //        if (!ViktMått.Any(x => x == "Antal stor")) ViktMått.Add("Antal stor");
        //    }
        //    else
        //    {
        //        HarAntalStor = false;
        //        if (ViktMått.Any(x => x == "Antal stor")) ViktMått.Remove("Antal stor");
        //    }
        //}
        #endregion


    }

}
