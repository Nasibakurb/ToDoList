using ToDoList.Domain.Entity;
using ToDoList.Domain.Filters.Task;
using ToDoList.Domain.Response;
using ToDoList.Domain.ViewModel.Task;

namespace ToDoList.Domain.Interfaces
{
    public interface ITaskService
    {
        Task<IBaseResponse<TaskEntity>> Create(CreateTaskModel model);
        Task<DataTableResult> GetTasks(TaskFilter filter); // Возвращает данные и пагинацию
        Task<IBaseResponse<bool>> EndTask(long id);
        Task<IBaseResponse<IEnumerable<TaskCompletedViewModel>>> GetComplatedTask();
        Task<IBaseResponse<IEnumerable<TaskViewModel>>>CalculateCompletedTask();
        Task<IBaseResponse<TaskEntity>> Delete(long id);
        Task<IBaseResponse<TaskEntity>> Edit(CreateTaskModel model);

    }
}
