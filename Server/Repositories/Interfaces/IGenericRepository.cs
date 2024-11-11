using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLibrary.Responses;

namespace Server.Repositories.Interfaces
{
    public interface IGenericRepository<T>
    {
        Task<List<T>> GetAll();
        Task<T> GetById(int id);
        Task<GeneralResponse> Insert(T entity);
        Task<GeneralResponse> Update(T entity);
        Task<GeneralResponse> DeleteById(int id);
    }
}