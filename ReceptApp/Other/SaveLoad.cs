using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Data.SqlClient;
//using Dapper; Used för mapping to DB
using System.Windows.Media.Imaging;

namespace ReceptApp
{
    public static class SaveLoad
    {
        private static string _folderpath = AppDomain.CurrentDomain.BaseDirectory;
        private static string _filename;
        //private static string connectionString = "Server=(local);Database=master;Integrated Security=True;";


        public static void SaveIngrediens(string filename, ObservableCollection<Ingrediens> ingrediens)
        {
            _filename = _folderpath + filename + ".json";
            string file = JsonConvert.SerializeObject(ingrediens);
            File.WriteAllText(_filename, file);
        }

        public static void SaveRecept(string filename, ObservableCollection<Recept> recept)
        {
            _filename = _folderpath + filename + ".json";
            string file = JsonConvert.SerializeObject(recept);
            File.WriteAllText(_filename, file);
        }

        public static ObservableCollection<Ingrediens> LoadIngrediens(string filename)
        {
            _filename = _folderpath + filename + ".json";
            if (File.Exists(_filename))
            {
                string file = File.ReadAllText(_filename);
                if (!string.IsNullOrEmpty(file))
                {
                    ObservableCollection<Ingrediens> ingrediens = JsonConvert.DeserializeObject<ObservableCollection<Ingrediens>>(file);
                    return ingrediens;
                }
            }
            return new ObservableCollection<Ingrediens>();
        }

        public static ObservableCollection<Recept> LoadRecept(string filename)
        {
            _filename = _folderpath + filename + ".json";
            if (File.Exists(_filename))
            {
                string file = File.ReadAllText(_filename);
                if (!string.IsNullOrEmpty(file))
                {
                    ObservableCollection<Recept> recept = JsonConvert.DeserializeObject<ObservableCollection<Recept>>(file);
                    return recept;
                }
            }
            return new ObservableCollection<Recept>();
        }





        public static void KopieraBild(BitmapImage img, string filnamn, string fileextension, bool hasExtension)
        {
            if (!hasExtension) fileextension = ".png";
            filnamn += fileextension;
            if (img != null && !string.IsNullOrEmpty(filnamn))
            {
                // Create a new BitmapEncoder
                BitmapEncoder encoder = new PngBitmapEncoder(); // Choose the appropriate encoder based on your requirements

                // Create a new MemoryStream to hold the encoded image data
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    // Encode the BitmapImage and write the encoded data to the MemoryStream
                    encoder.Frames.Add(BitmapFrame.Create(img));
                    encoder.Save(memoryStream);
                    string bildfolder = _folderpath + @"\Bilder\";
                    if (!Directory.Exists(bildfolder)) Directory.CreateDirectory(bildfolder);
                    string filePath = Path.Combine(bildfolder, filnamn);
                    if (!Directory.Exists(_folderpath)) { Directory.CreateDirectory(_folderpath); }
                    // Write the encoded data from the MemoryStream to the file
                    File.WriteAllBytes(filePath, memoryStream.ToArray());
                }
            }
        }
        #region Databas
        //public static void AddIngrediensToDB(Ingrediens i)
        //{
        //    string sql = @"INSERT INTO Ingredienser(IngrediensID, Namn, Kalori, Protein, Kolhydrat, Socker, Fett, GramPerDl, Liten, Medel, Stor, Bild)
        //                VALUES(@ID, @Namn, @Kalori, @Kolhydrat, @Socker, @Protein, @Fett, @GramPerDl, @Liten, @Medel, @Stor, @Bild)";

        //    string sqlantalrader = @"SELECT COUNT(*) FROM Ingredienser";
        //    int antalrader = 0;
        //    using (var connection = new SqlConnection(connectionString))
        //    {
        //        connection.Open();
        //        if (i != null)
        //        {
        //            connection.ExecuteScalar<int>(sqlantalrader);
        //            i.ID = antalrader + 1;
        //            connection.Execute(sql, i);
        //        }
        //        connection.Close();
        //    }

        //}

        //public static ObservableCollection<Ingrediens> LoadIngrediensFromDB()
        //{
        //    string query = @"SELECT * FROM Ingredienser";
        //    ObservableCollection<Ingrediens> ingred = new ObservableCollection<Ingrediens>();
        //    using (var connection = new SqlConnection(connectionString))
        //    {
        //        connection.Open();
        //        List<Ingrediens> lista = (connection.Query<Ingrediens>(query)).AsList();
        //        ingred = new ObservableCollection<Ingrediens>(lista);
        //    }
        //    return ingred;
        //}

        //public static ObservableCollection<Recept> LoadReceptFromDB()
        //{
        //    string query = @"SELECT * FROM Recept";
        //    ObservableCollection<Recept> recept = new ObservableCollection<Recept>();
        //    using (var connection = new SqlConnection(connectionString))
        //    {
        //        connection.Open();
        //        List<Recept> lista = (connection.Query<Recept>(query)).AsList();
        //        recept = new ObservableCollection<Recept>(lista);
        //    }
        //    return recept;
        //}
        #endregion
    }
}
