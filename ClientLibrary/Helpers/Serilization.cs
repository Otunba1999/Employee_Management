using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text.Json;
using System.Threading.Tasks;

namespace ClientLibrary.Helpers
{
    public static class Serilization
    {
        public static string? SerializeObj<T>(T model) => JsonSerializer.Serialize(model);
        public static T? DeserializeObj<T>(string json) => JsonSerializer.Deserialize<T>(json);
        public static IList<T>? DeserializeList<T>(string json) => JsonSerializer.Deserialize<  IList<T>>(json);
    }
}