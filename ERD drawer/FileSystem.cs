using System.Text.Json;

namespace ERD_drawer
{
    public  class FileSystem
    {
        private static readonly JsonSerializerOptions options = new()
        {
            IncludeFields = true,
            AllowTrailingCommas = true,
            WriteIndented = true,
            PropertyNameCaseInsensitive = true,
            //UnmappedMemberHandling = System.Text.Json.Serialization.JsonUnmappedMemberHandling.Skip,
            //IgnoreReadOnlyFields = true,
            //IgnoreReadOnlyProperties = true,
        };

        public static Data LoadFile(string fileName)
        {
            string jsonString = File.ReadAllText(fileName);

            try
            {
                return JsonSerializer.Deserialize<Data>(jsonString, options);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Couldn't parse JSON!:\n" + ex.Message);
            }
            return null;
        }

        public static void SaveFile(string fileName, Data data)
        {
            string jsonString = JsonSerializer.Serialize(data, options);
            File.WriteAllText(fileName, jsonString);
        }
    }
}
