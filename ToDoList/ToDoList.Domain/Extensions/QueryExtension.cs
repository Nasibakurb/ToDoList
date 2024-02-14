using System.Linq.Expressions;
 

namespace ToDoList.Domain.Extensions
{
    public static class QueryExtension
    {
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, 
            bool condition, Expression<Func<T, bool>> predicate)
        { // Метод для удобной фильтрации
            if (condition)
            {
                return source.Where(predicate);
            }   
            return source;
        }
    } // Пример. Если строка не пустая (!string.IsNullOrWhiteSpace(filter.Name), 
    // то выполняется поиск x => x.Name == filter.Name
}
