using Microsoft.EntityFrameworkCore;
using ToDoList.Domain.Entity;

namespace ToDoList.Infrastructure.Data
{
    public class AppDBContext: DbContext // Наслед. от DbContext EF
    {
        public AppDBContext(DbContextOptions options): base(options) // Конструктор
        {
            Database.EnsureCreated(); // При обращ. к дб созадаться опред. дб
        }

       public DbSet<TaskEntity> TaskTable { get; set; } // Созд. сущности

    }
}
    