using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazored.LocalStorage;

namespace ClientLibrary.Helpers
{
    public class LocalStorageService(ILocalStorageService localStorageService)
    {
        private const string Key = "authentication-Token";
        public async Task<string> GetToken() => await localStorageService.GetItemAsStringAsync(Key);
        public async Task SetToken(string item) => await localStorageService.SetItemAsStringAsync(Key, item);
        public async Task RemoveToken() => await localStorageService.RemoveItemAsync(Key);
    }
}