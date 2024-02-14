using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Domain.Interfaces
{
    public interface IBaseRepository<T>
    { // Первый этап создание методов (по умолчанию)
        Task Create(T entity); // Принимает объект
        IQueryable<T> GetAll(); // Возвращает IQueryable<T> (работает на стороне сервера)
        Task Delete(T entity); // Принимает объект
        Task<T> Update(T entity); // Возвращает Task<T>
    }
}
