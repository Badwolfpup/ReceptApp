using ReceptApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ReceptApp.ViewModel
{
    public class VMNewRecipe: INotifyPropertyChanged
    {

        #region InotifyPropertyChanged
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        #endregion

        public ICommand ScrollIngrediensNyttRecept { get; set; }
        public ICommand AddIngredient { get; set; }
        public ICommand GotFocus { get; set; }
        public ICommand ScrollTillagdaIngredienser { get; set; }
        public ICommand MenuItem { get; set; }

        private Recept _nyttrecept;
        public Recept NyttRecept
        {
            get { return _nyttrecept; }
            set
            {
                if (_nyttrecept != value)
                {
                    _nyttrecept = value;
                    OnPropertyChanged(nameof(NyttRecept));
                }
            }
        }

        public ObservableCollection<Ingrediens> IngrediensLista => AppData.Instance.IngrediensLista;

        private string _knapptext;
        public string KnappText
        {
            get { return _knapptext; }
            set
            {
                _knapptext = value;
                OnPropertyChanged(nameof(KnappText));
            }
        }


        private bool _valtreceptingrediens = true;
        private ListView _lastSelectedListView;


        private Vara _valdvara;
        public Vara ValdVara
        {
            get { return _valdvara; }
            set
            {
                _valdvara = value;
                OnPropertyChanged(nameof(ValdVara));
            }
        }

        private ReceptIngrediens _valdreceptingrediens;
        public ReceptIngrediens ValdReceptIngrediens
        {
            get { return _valdreceptingrediens; }
            set
            {
                _valdreceptingrediens = value;
                OnPropertyChanged(nameof(ValdReceptIngrediens));
            }
        }

        public VMNewRecipe(bool nyttrecept, Recept recept)
        {
            NyttRecept = recept;

            KnappText = nyttrecept ? "Lägg till recept" : "Spara ändringar";

            //ScrollIngrediensNyttRecept = new RelayCommand(ScrollIngrediensNyttRecept_SelectionChanged);
            //AddIngredient = new RelayCommand(AddIngredient_Click);
            //GotFocus = new RelayCommand(ListView_GotFocus);
            //ScrollTillagdaIngredienser = new RelayCommand(ScrollTillagdaIngredienser_SelectionChanged);
            //MenuItem = new RelayCommand(MenuItem_Click);
        }

        //private void ScrollIngrediensNyttRecept_SelectionChanged(object sender, object parameter)
        //{

        //    if (sender is ListView listview && listview.SelectedItem is Vara i)
        //    {
        //        if (_valtreceptingrediens) ValdReceptIngrediens = new ReceptIngrediens();
        //        _valtreceptingrediens = false;
        //        ValdVara = i;
        //        if (parameter is ComboBox ComboBoxMått)
        //        {
        //            ComboBoxMått.SelectedIndex = 0;
        //        }
        //    }
        //}

        //private void AddIngredient_Click(object sender)
        //{
        //    if (sender is not Tuple<object, object, object> item) return;

        //    var textbox = item.Item1 as TextBox;
        //    var combobox = item.Item2 as ComboBox;
        //    var listview = item.Item3 as ListView;

        //    if (textbox == null || combobox == null || listview == null) return;

        //    int.TryParse(textbox.Text, out int mängd);
        //    if (mängd > 0)
        //    {
        //        if (_lastSelectedListView.SelectedItem is not Vara i || NyttRecept.ReceptIngredienser.Any(x => x.Vara.Namn == ValdVara.Namn))
        //        {
        //            MessageBox.Show("Du har inte valt en vara eller försöker du lägga till en dublett"); return;
        //        }
        //        ValdReceptIngrediens.Mått = ValdReceptIngrediens.KonverteraMåttTillText(combobox.Text);
        //        ValdReceptIngrediens.Vara = ValdVara;
        //        ValdReceptIngrediens.BeräknaAntalGram();
        //        NyttRecept.ReceptIngredienser.Add(ValdReceptIngrediens);
        //        listview.SelectedItem = ValdReceptIngrediens;
        //        _valtreceptingrediens = true;
        //    }
        //    else MessageBox.Show("Du behöver ange hur mycket");
        //}

        //private void ListView_GotFocus(object sender)
        //{
        //    if (_lastSelectedListView != null && _lastSelectedListView != sender)
        //    {
        //        _lastSelectedListView.SelectedItem = null;
        //    }
        //    _lastSelectedListView = sender as ListView;
        //}

        //private void ScrollTillagdaIngredienser_SelectionChanged(object sender, object parameter)
        //{
        //    if (sender is ListView listview && listview.SelectedItem is ReceptIngrediens i)
        //    {
        //        _valtreceptingrediens = true;
        //        ValdReceptIngrediens = i;
        //        ValdVara = i.Vara;
        //        if (parameter is ComboBox ComboBoxMått)
        //        {
        //            switch (i.Mått)
        //            {
        //                case "g": ComboBoxMått.SelectedItem = "Gram"; break;
        //                case "dl": ComboBoxMått.SelectedItem = "Deciliter"; break;
        //                case "msk": ComboBoxMått.SelectedItem = "Matsked"; break;
        //                case "tsk": ComboBoxMått.SelectedItem = "Tesked"; break;
        //                case "krm": ComboBoxMått.SelectedItem = "Kryddmått"; break;
        //                case "st": ComboBoxMått.SelectedItem = "Stycken"; break;
        //                case "stora": ComboBoxMått.SelectedItem = "Antal stor"; break;
        //                case "stor": ComboBoxMått.SelectedItem = "Antal stor"; break;
        //                case "medelstora": ComboBoxMått.SelectedItem = "Antal medel"; break;
        //                case "medelstor": ComboBoxMått.SelectedItem = "Antal medel"; break;
        //                case "små": ComboBoxMått.SelectedItem = "Antal liten"; break;
        //                case "liten": ComboBoxMått.SelectedItem = "Antal liten"; break;
        //            }
        //        }

        //    }
        //}

        //private void MenuItem_Click(object sender)
        //{
        //    if (sender is ListView listview && listview.SelectedItem is ReceptIngrediens i)
        //    {
        //        if (i == null) return;
        //        i.Vara.ÄrTillagdIRecept = false;
        //        int index = NyttRecept.ReceptIngredienser.IndexOf(i);
        //        if (index >= 0) NyttRecept.ReceptIngredienser.Remove(i);
        //        ValdReceptIngrediens = NyttRecept.ReceptIngredienser.Count == 0 ? ValdReceptIngrediens = new ReceptIngrediens() : (index >= NyttRecept.ReceptIngredienser.Count ? NyttRecept.ReceptIngredienser[NyttRecept.ReceptIngredienser.Count - 1] : NyttRecept.ReceptIngredienser[index]);
        //    }
        //    //ReceptIngrediens i = ScrollTillagdaIngredienser.SelectedItem as ReceptIngrediens;

        //}
    }
}
