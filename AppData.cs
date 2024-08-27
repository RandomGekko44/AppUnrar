using System.Collections.ObjectModel;
using System.Reflection;
using System.Text.Json;

namespace AppUnrar
{
    public static class AppData
    {
        public static ObservableCollection<Model.File>? Files_List { get; set; }
        public static ObservableCollection<string>? Passwords { get; set; }
        public static ObservableCollection<string>? FileExtensionsToExtract { get; set; }
    }

    public class MapAppData
    {
        public ObservableCollection<string>? Passwords { get; set; }
        public ObservableCollection<string>? FileExtensionsToExtract { get; set; }

        public void MapToStaticClass(MapAppData source)
        {
            var sourceProperties = source.GetType().GetProperties();

            //Key thing here is to specify we want the static properties only
            var destinationProperties = typeof(AppData)
                .GetProperties(BindingFlags.Public | BindingFlags.Static);

            foreach (var prop in sourceProperties)
            {
                //Find matching property by name
                var destinationProp = destinationProperties
                    .Single(p => p.Name == prop.Name);

                //Set the static property value
                destinationProp.SetValue(null, prop.GetValue(source));
            }
        }
    }
}
