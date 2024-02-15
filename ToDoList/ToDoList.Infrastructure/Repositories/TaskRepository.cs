using ToDoList.Domain.Entity;
using ToDoList.Domain.Interfaces;
using ToDoList.Infrastructure.Data;

namespace ToDoList.Infrastructure.Response
{
    public class TaskRepository : IBaseRepository<TaskEntity> // Указываем точную модель 
    {
        private readonly AppDBContext _appDbContext; 
        
        public TaskRepository(AppDBContext appDbContext)
        { // Запросы к Дб через AppDbContext (DbContext)
            _appDbContext = appDbContext;
        }

        public async Task Create(TaskEntity entity)
        {
            await _appDbContext.TaskTable.AddAsync(entity);  // TaskTable - созданная сущность в AppDbContext
            await _appDbContext.SaveChangesAsync();
        }

        public async Task Delete(TaskEntity entity)
        {
            _appDbContext.TaskTable.Remove(entity);
            await _appDbContext.SaveChangesAsync();
        }

        public IQueryable<TaskEntity> GetAll()
        {
            return _appDbContext.TaskTable;
        }

        public async Task<TaskEntity> Update(TaskEntity entity)
        {
            _appDbContext.Update(entity);
            await _appDbContext.SaveChangesAsync();
            return entity;
        }
    }
}
