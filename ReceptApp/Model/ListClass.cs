using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ReceptApp
{
    public class ListClass: INotifyPropertyChanged
    {
        #region InotifyPropertyChanged
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        #endregion
        private MainWindow _main;
        private ObservableCollection<Ingrediens>? _ingredienslista;
        private Ingrediens _valdingrediens;
        private Recept _nyttrecept;
        private ObservableCollection<Recept>? _receptlista;
        private Recept _valtrecept;
        public Recept ValtRecept
        {
            get { return _valtrecept; } 
            set 
            { 
                if (_valtrecept != value)
                {
                    _valtrecept = value;
                    OnPropertyChanged(nameof(ValtRecept));
                }
            }
        }

        public ObservableCollection<Ingrediens> Ingredienslista
        {
            get { return _ingredienslista; }
            set
            {
                if (value != _ingredienslista)
                {
                    _ingredienslista = value;
                    OnPropertyChanged(nameof(Ingredienslista));
                }
            }
        }
        public Ingrediens ValdIngrediens
        {
            get { return _valdingrediens; }
            set
            {
                if (_valdingrediens != value)
                {
                    _valdingrediens = value;
                    OnPropertyChanged(nameof(ValdIngrediens));
                }
            }
        }

        private int _antalportioner;

        public int Antalportioner
        {
            get { return _antalportioner; }
            set
            {
                if (_antalportioner != value)
                {
                    _antalportioner = value;
                    OnPropertyChanged(nameof(Antalportioner));
                }
            }
        }
        public Recept Nyttrecept
        {
            get { return _nyttrecept; }
            set
            {
                if (_nyttrecept != value)
                {
                    _nyttrecept = value;
                    OnPropertyChanged(nameof(Nyttrecept));
                }
            }
        }
        public ObservableCollection<Recept> ReceptLista
        {
            get { return _receptlista; }
            set
            {
                if (value != _receptlista)
                {
                    _receptlista = value;
                    OnPropertyChanged(nameof(ReceptLista));
                }
            }
        }
        public bool HasAddedImage { get; set; }
        public bool HasExtension { get; set; } 
        public BitmapImage TempBild { get; set; } = new BitmapImage();
        public ListClass()
        {
           
            Ingredienslista = SaveLoad.LoadIngrediens("Ingrediens");
            //ReceptLista = SaveLoad.LoadReceptFromDB();
            
            Antalportioner = 4;
            if (Ingredienslista.Count != 0) ValdIngrediens = Ingredienslista[0]; else { ValdIngrediens = new Ingrediens(); AddImageSource(); }
            
            //if (ReceptLista.Count > 0) ValtRecept = ReceptLista[0];
        }

        private void AddImageSource()
        {
            ValdIngrediens.Bild = "pack://application:,,,/ReceptApp;component/Bilder/dummybild.png";
        }
    }
}
