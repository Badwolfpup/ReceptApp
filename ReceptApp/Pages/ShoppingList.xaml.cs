using ReceptApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ReceptApp.Pages
{
    /// <summary>
    /// Interaction logic for ShoppingList.xaml
    /// </summary>
    public partial class ShoppingList : Page
    {
        App app = (App)Application.Current;

        private Priser _valtpris = null;


        public ShoppingList()
        {
            InitializeComponent();
            DataContext = app;
            Loaded += ShoppingList_Loaded;
        }

        private void ShoppingList_Loaded(object sender, RoutedEventArgs e)
        {
            AggregeraRceptIngredienser();
        }

        private void AggregeraRceptIngredienser()
        {

            //Slår ihop alla ingredienser i shoppinglistan så att varje ingrediens bara förekommer en gång
            app.ReceptIngrediensShoppingList = new ObservableCollection<ReceptIngrediens>(app.ShoppingIngredienser
                .SelectMany(r => r.ReceptIngredienser)
                .GroupBy(item => item.Ingrediens.Namn)
                .Select(group =>
                {
                    string störstmått = group.OrderByDescending(ing => ing.AntalGram).First().Mått;
                    double totalmängd = group.Sum(ing =>
                    {
                        return KonverteraMått(ing.Mått, ing.Mängd, ing.AntalGram, ing.Ingrediens);
                    });


                    return new ReceptIngrediens
                    {

                        Mått = störstmått,
                        Ingrediens = group.First().Ingrediens,
                        Mängd = totalmängd,
                        AntalGram = group.First().AntalGram,
                    };
                })
                .ToList()
            );

            //Konverterar mängden tillbaka till det mått som är hade mest mängd
            foreach (var item in app.ReceptIngrediensShoppingList)
            {
                if (item.Mått == "st")
                {
                    item.Mängd = (double)(item.Mängd / item.Ingrediens.Styck);
                }
                else
                {
                    if (item.Mått == "dl")
                    {
                        item.Mängd = (double)(item.Mängd / 100 / (item.Ingrediens.GramPerDl / 100));
                    }
                    else if (item.Mått == "msk")
                    {
                        item.Mängd = (double)(item.Mängd / 100 / (item.Ingrediens.GramPerDl / 100) / 0.15);
                    }
                    else if (item.Mått == "tsk")
                    {
                        item.Mängd = (double)(item.Mängd / 100 / (item.Ingrediens.GramPerDl / 100) / 0.05);
                    }
                    else if (item.Mått == "krm")
                    {
                        item.Mängd = (double)(item.Mängd / 100 / (item.Ingrediens.GramPerDl / 100) / 0.01);
                    }
                }
            }
            app.PriserIShoppingList.Clear();
            if (RadioBilligast.IsChecked == true)
            {
                foreach (var item in app.ReceptIngrediensShoppingList)
                {
                    app.PriserIShoppingList.Add(item.Ingrediens.PrisLista.OrderBy(pris =>
                    {
                        if (item.Mått == "g") return pris.PrisPerKg;
                        if (item.Mått == "st") return pris.PrisPerSt;
                        return pris.PrisPerLiter;
                    }).First());
                    if (app.PriserIShoppingList.Count > 0)
                    {
                        if (app.PriserIShoppingList[app.PriserIShoppingList.Count - 1].Antal > 0)
                        {
                            var receptmängd = item.AntalGram * 100;
                            var prismängd = app.PriserIShoppingList[app.PriserIShoppingList.Count - 1].Mängd;
                            prismängd = KonverteraPrismängd(app.PriserIShoppingList[app.PriserIShoppingList.Count - 1].Mått, prismängd, app.PriserIShoppingList.Count - 1, item);

                            //if (app.PriserIShoppingList[app.PriserIShoppingList.Count - 1].Mått == "kg") { prismängd *= 1000; }
                            //else if (app.PriserIShoppingList[app.PriserIShoppingList.Count - 1].Mått == "dl") { prismängd *= item.Ingrediens.GramPerDl / 100; }
                            //else if (app.PriserIShoppingList[app.PriserIShoppingList.Count - 1].Mått == "l") { prismängd *= item.Ingrediens.GramPerDl / 100 * 10; }
                            app.PriserIShoppingList[app.PriserIShoppingList.Count - 1].AntalProdukter = (int)Math.Ceiling((decimal)(receptmängd / prismängd));
                            app.PriserIShoppingList[app.PriserIShoppingList.Count - 1].Summa = app.PriserIShoppingList[app.PriserIShoppingList.Count - 1].AntalProdukter > 1 ? app.PriserIShoppingList[app.PriserIShoppingList.Count - 1].AntalProdukter * app.PriserIShoppingList[app.PriserIShoppingList.Count - 1].Pris : app.PriserIShoppingList[app.PriserIShoppingList.Count - 1].Pris;
                        }
                    }
                }
            }
            else
            {
                foreach (var item in app.ReceptIngrediensShoppingList)
                {
                    app.PriserIShoppingList.Add(item.Ingrediens.PrisLista.OrderBy(vikt =>
                    {
                        double? prisvikt = 0;
                        if (vikt.Mått == "g") prisvikt = vikt.Mängd;
                        else if (vikt.Mått == "kg") prisvikt = vikt.Mängd * 1000;
                        else if (vikt.Mått == "dl") prisvikt = vikt.Mängd * item.Ingrediens.GramPerDl / 100;
                        else if (vikt.Mått == "l") prisvikt = vikt.Mängd * item.Ingrediens.GramPerDl / 100 * 10;
                        return item.AntalGram * 100 / prisvikt;
                    }).First());
                    if (app.PriserIShoppingList.Count > 0)
                    {
                        if (app.PriserIShoppingList[app.PriserIShoppingList.Count - 1].Antal > 0)
                        {
                            var receptmängd = item.AntalGram * 100;
                            var prismängd = app.PriserIShoppingList[app.PriserIShoppingList.Count - 1].Mängd;
                            prismängd = KonverteraPrismängd(app.PriserIShoppingList[app.PriserIShoppingList.Count - 1].Mått, prismängd, app.PriserIShoppingList.Count - 1, item);

                            //if (app.PriserIShoppingList[app.PriserIShoppingList.Count - 1].Mått == "kg") { prismängd *= 1000; }
                            //else if (app.PriserIShoppingList[app.PriserIShoppingList.Count - 1].Mått == "dl") { prismängd *= item.Ingrediens.GramPerDl / 100; }
                            //else if (app.PriserIShoppingList[app.PriserIShoppingList.Count - 1].Mått == "l") { prismängd *= item.Ingrediens.GramPerDl / 100 * 10; }
                            app.PriserIShoppingList[app.PriserIShoppingList.Count - 1].AntalProdukter = (int)Math.Ceiling((decimal)(receptmängd / prismängd));
                            app.PriserIShoppingList[app.PriserIShoppingList.Count - 1].Summa = app.PriserIShoppingList[app.PriserIShoppingList.Count - 1].AntalProdukter > 1 ? app.PriserIShoppingList[app.PriserIShoppingList.Count - 1].AntalProdukter * app.PriserIShoppingList[app.PriserIShoppingList.Count - 1].Pris : app.PriserIShoppingList[app.PriserIShoppingList.Count - 1].Pris;
                        }
                    }
                }

            }
            if (app.PriserIShoppingList.Count > 0)
            {
                app.TotalSumma = (double)app.PriserIShoppingList.Sum(x => x.Summa);
            }
        }


        private double KonverteraMått(string mått, double mängd, double? antalgram, Ingrediens ingrediens)
        {

            if (mått == "g") 
            {
                return (double)(antalgram * 100);
            } else if (mått == "st")
            {
                return (double)(mängd * ingrediens.Styck);
            } else
            {
                return (double)(antalgram * 100 * (ingrediens.GramPerDl/100));
            }
        }

        private void DeleteIngredient_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                if (button.DataContext is Priser i)
                {
                    var hittaRecept = app.ShoppingIngredienser.FirstOrDefault(r => r.ReceptIngredienser.Any(x => x.Ingrediens.PrisLista.Contains(i)));
                    if (hittaRecept != null)
                    {
                        var prislista = hittaRecept.ReceptIngredienser.FirstOrDefault(x => x.Ingrediens.PrisLista.Contains(i));
                        if (prislista != null)
                        {
                            hittaRecept.ReceptIngredienser.Remove(prislista);
                            AggregeraRceptIngredienser();
                        }
                        
                    }

                }
            }
        }

        private void AddToShoppingCart_Click(object sender, RoutedEventArgs e)
        {
            string clipboard = "";
            foreach (var item in app.PriserIShoppingList)
            {
                clipboard += $"{item.Mängd}{item.Mått} {item.Namn}\n";
            }
            Clipboard.SetText(clipboard);
        }

        private void DeleteRecipe_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                if (button.DataContext is Recept r)
                {
                    app.ShoppingIngredienser.Remove(r);
                    AggregeraRceptIngredienser();
                }
            }
        }

        private void EditVara_Click(object sender, RoutedEventArgs e)
        {
            prispopup.IsOpen = true;
            ObservableCollection<Priser> priser = null;
            if (sender is Button button)
            {
                if (button.DataContext is Priser i)
                {

                    var hittaRecept = app.ShoppingIngredienser.FirstOrDefault(r => r.ReceptIngredienser.Any(x => x.Ingrediens.PrisLista.Contains(i)));
                    if (hittaRecept != null)
                    {
                        var prislista = hittaRecept.ReceptIngredienser.FirstOrDefault(x => x.Ingrediens.PrisLista.Contains(i));
                        if (prislista != null)
                        {
                            priser = prislista.Ingrediens.PrisLista;
                            _valtpris = i;
                        }
                    }
                }
            }
            if (priser == null) return;
            prislista.ItemsSource = priser;

        }


        private void SorteringsRadio_Checked(object sender, RoutedEventArgs e)
        {
            AggregeraRceptIngredienser();
        }

        private void StängPopup_Click(object sender, RoutedEventArgs e)
        {
            prispopup.IsOpen = false;
        }

        private void ÄndraInköpsLista_Click(object sender, RoutedEventArgs e)
        {
            //PriserIShoppingList
            if (sender is Button button) 
            {
                if (app.PriserIShoppingList.Count >= 0) {
                    var olditem = app.PriserIShoppingList.First(x => x.Namn == _valtpris.Namn);
                    var index = app.PriserIShoppingList.IndexOf(olditem);
                    app.PriserIShoppingList[index] = _valtpris;
                    var hittaRecept = app.ShoppingIngredienser.FirstOrDefault(r => r.ReceptIngredienser.Any(x => x.Ingrediens.PrisLista.Contains(_valtpris)));
                    if (hittaRecept != null)
                    {
                        var prislista = hittaRecept.ReceptIngredienser.FirstOrDefault(x => x.Ingrediens.PrisLista.Contains(_valtpris));
                        if (prislista != null)
                        {
                            var receptmängd = prislista.AntalGram * 100;
                            var prismängd = app.PriserIShoppingList[index].Mängd;
                            prismängd = KonverteraPrismängd(app.PriserIShoppingList[index].Mått, prismängd, index, prislista);
                            //if (app.PriserIShoppingList[index].Mått == "kg") { prismängd *= 1000; }
                            //else if (app.PriserIShoppingList[index].Mått == "dl") { prismängd *= prislista.Ingrediens.GramPerDl / 100; }
                            //else if (app.PriserIShoppingList[index].Mått == "l") { prismängd *= prislista.Ingrediens.GramPerDl / 100 * 10; }
                            app.PriserIShoppingList[index].AntalProdukter = (int)Math.Ceiling((decimal)(receptmängd / prismängd));
                            app.PriserIShoppingList[index].Summa = app.PriserIShoppingList[index].AntalProdukter > 1 ? app.PriserIShoppingList[index].AntalProdukter * app.PriserIShoppingList[index].Pris : app.PriserIShoppingList[index].Pris;
                        }
                    }


                    prispopup.IsOpen = false;
                 }
            }
        }

        private double? KonverteraPrismängd(string mått, double? prismängd, int index, ReceptIngrediens prislista)
        {
            if (app.PriserIShoppingList[index].Mått == "kg") { return prismängd *= 1000; }
            else if (app.PriserIShoppingList[index].Mått == "dl") { return prismängd *= prislista.Ingrediens.GramPerDl / 100; }
            else if (app.PriserIShoppingList[index].Mått == "l") { return prismängd *= prislista.Ingrediens.GramPerDl / 100 * 10; }
            return prismängd;
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton radio)
            {
                if (radio.DataContext is Priser pris)
                {
                    _valtpris = pris;

                }
            }
        }
    }
}
