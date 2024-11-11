using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLibrary.Responses;

namespace ClientLibrary.Services.Interfaces
{
    public interface IGenericService<T>
    {
        Task<List<T>> GetAll(string baseUrl);
        Task<T> Get(string baseUrl, int id);
        Task<GeneralResponse> Insert(string baseUrl, T model);
        Task<GeneralResponse> Update(string baseUrl, T model);
        Task<GeneralResponse> Delete(string baseUrl, int id);
    }
}