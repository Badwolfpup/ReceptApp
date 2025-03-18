using Newtonsoft.Json;
using ReceptApp.Model;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;

namespace ReceptApp
{
    public class Ingrediens : INotifyPropertyChanged
    {


        #region InotifyPropertyChanged
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        //public event NotifyCollectionChangedEventHandler? CollectionChanged;
        #endregion

        #region Properties
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

        private ObservableCollection<Vara> _varor; //Lista med varor
        public ObservableCollection<Vara> Varor
        {
            get { return _varor; }
            set
            {
                if (_varor != value)
                {
                    if (_varor != null)
                    {
                        // Detach the event from the old collection
                        _varor.CollectionChanged -= Varor_CollectionChanged;
                        foreach (var item in _varor)
                        {
                            item.PropertyChanged -= Varor_PropertyChanged;
                        }
                    }
                    _varor = value;
                    if (_varor != null)
                    {
                        // Attach the event to the new collection
                        _varor.CollectionChanged += Varor_CollectionChanged;
                        foreach (var item in _varor)
                        {
                            item.PropertyChanged += Varor_PropertyChanged;
                        }
                    }
                    OnPropertyChanged(nameof(Varor));
                }
            }
        }
        public bool HarVaror => Varor.Count < 1;
        private void Varor_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            AppData.Instance.SaveAll();
        }

        private void Varor_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            AppData.Instance.SaveAll();
        }

        #endregion


        public Ingrediens(string namn)
        {
            Varor = new ObservableCollection<Vara>();
            Namn = namn;
        }


        App app = (App)Application.Current;


        public ObservableCollection<Vara> CopyVaror()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            };
            var json = JsonConvert.SerializeObject(Varor);
            return JsonConvert.DeserializeObject<ObservableCollection<Vara>>(json, settings);
        }




    }

}
