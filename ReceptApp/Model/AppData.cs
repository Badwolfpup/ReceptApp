using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.IO;

namespace ReceptApp.Model
{
    public class AppData
    {
        public ObservableCollection<Ingrediens> Ingredienslista { get; set; } = new ObservableCollection<Ingrediens>();
        public ObservableCollection<Ingrediens> Ovrigavaraorlista { get; set; } = new ObservableCollection<Ingrediens>();
        public ObservableCollection<Recept> ReceptLista { get; set; } = new ObservableCollection<Recept>();


        public void SaveAll()
        {
            string _folderpath = AppDomain.CurrentDomain.BaseDirectory;
            string _filepath = _folderpath + "Appdata.json";

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                PreserveReferencesHandling = PreserveReferencesHandling.All,
                Formatting = Formatting.Indented

            };

            string file = JsonConvert.SerializeObject(this, settings);
            File.WriteAllText(_filepath, file);
        }

        public static AppData Load()
        {

            string _folderpath = AppDomain.CurrentDomain.BaseDirectory;
            string _filepath = _folderpath + "Appdata.json";

            if (!File.Exists(_filepath))
            {
                return new AppData();
            }

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                PreserveReferencesHandling = PreserveReferencesHandling.All
            };

            string file = File.ReadAllText(_filepath);
            return !string.IsNullOrWhiteSpace(file) ? JsonConvert.DeserializeObject<AppData>(file, settings) : new AppData();
        }
    }
}
