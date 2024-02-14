using ToDoList.Domain.Entity;
using ToDoList.Domain.Filters.Task;
using ToDoList.Domain.Response;
using ToDoList.Domain.ViewModel.Task;

namespace ToDoList.Domain.Interfaces
{
    public interface ITaskService
    {
        Task<IBaseResponse<TaskEntity>> Create(CreateTaskModel model);
        Task<IBaseResponse<IEnumerable<TaskViewModel>>> GetTasks(TaskFilter filter);
        Task<IBaseResponse<bool>> EndTask(long id);
        Task<IBaseResponse<IEnumerable<TaskCompletedViewModel>>> GetComplatedTask();

    }
}
