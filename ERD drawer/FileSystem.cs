using System.Diagnostics;
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

        public static bool LoadFile(string fileName, ref Data? data)
        {
            string jsonString = File.ReadAllText(fileName);
            try
            {
                data = JsonSerializer.Deserialize<Data>(jsonString, options);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                MessageBox.Show("Couldn't parse JSON!:\n" + ex.Message);
            }
            return false;
        }

        public static void SaveFile(string fileName, Data data)
        {
            string jsonString = JsonSerializer.Serialize(data, options);
            File.WriteAllText(fileName, jsonString);
        }
    }
}
