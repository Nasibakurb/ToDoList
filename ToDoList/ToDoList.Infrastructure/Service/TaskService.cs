using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net.Security;
using ToDoList.Domain.Entity;
using ToDoList.Domain.Enum;
using ToDoList.Domain.Extensions;
using ToDoList.Domain.Filters.Task;
using ToDoList.Domain.Interfaces;
using ToDoList.Domain.Response;
using ToDoList.Domain.ViewModel.Task;

namespace ToDoList.Infrastructure.Service
{
    public class TaskService : ITaskService // Наследуемся от интерфейса
    {
        private readonly IBaseRepository<TaskEntity> _repository; 
        private ILogger<TaskService> _logger; // Журнал событий и состояния 

        public TaskService(IBaseRepository<TaskEntity> ibaseRepository, 
            ILogger<TaskService> logger)
        {
            _repository = ibaseRepository;
            _logger = logger;
        }

        public async Task<IBaseResponse<IEnumerable<TaskViewModel>>> CalculateCompletedTask()
        {
            try
            {
                var task = await _repository.GetAll()
                    .Where(x => x.Created.Date == DateTime.Today)
                    .Select(x => new TaskViewModel()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                        IsDone = x.IsDone == true ? "Готова" : "Не готова",
                        Priority = x.Priority.GetDisplayName(),
                        Created = x.Created.ToLongDateString(),
                    })
                    .ToListAsync();
                return new BaseResponse<IEnumerable<TaskViewModel>>()
                {
                    StatusCode = StatusCode.Ok,
                    Data = task,
                };

            }
            catch (Exception ex)
            {
                _logger.LogInformation($"[TaskService.Created]:{ex.Message}");
                return new BaseResponse<IEnumerable<TaskViewModel>>()
                {
                    Description = $"Ошибка: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<TaskEntity>>Create(CreateTaskModel model)
        {  // Вернет IBaseResponse<TaskEntity> (ответ с сущностью)
            try
            {
                model.Validation();
                _logger.LogInformation($"Запрос на создание задачи: {model.Name}");
                var task = await _repository.GetAll() // 1. Получить все элементы
                 .Where(x => x.Created.Date == DateTime.Today) // 2. Найти элемент с определен. датой
                 .FirstOrDefaultAsync(x => x.Name == model.Name); //3. Выбирается первый элемент с определ. именем

                if (task != null) // Если такая задача по дате и имене существует 
                {
                    return new BaseResponse<TaskEntity>() // Создается новый объект
                    {
                        Description = "Задача с таким названием уже есть", // Описание ошибки
                        StatusCode = StatusCode.TaskIsHasAlready // Код состоян. ошибки 
                    };
                }
                // Если такая задача по дате и имене не существует 
                task = new TaskEntity()
                { // Создается новый объект и заполняется свойствами 
                    Name = model.Name,
                    Description = model.Description,
                    IsDone = false,
                    Priority = model.Priority,
                    Created = DateTime.Now

                };
                await _repository.Create(task); // Создание и сохранение
                _logger.LogInformation($"Данная задача создалась: {model.Name}");

                return new BaseResponse<TaskEntity>()
                { // Возвр. объект с информацией 
                    Description = "Данная задача создалась",
                    StatusCode = StatusCode.Ok
                };
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"[TaskService.Created]:{ex.Message}");
                return new BaseResponse<TaskEntity>()
                {
                    Description= $"Ошибка: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<TaskEntity>> Delete(long id)
        {
            try
            {
               var task = await _repository.GetAll()
                    .FirstOrDefaultAsync (x => x.Id == id);
                if (task == null) 
                {
                    return new BaseResponse<TaskEntity>()
                    {
                        Description = $"Задача не найдена",
                        StatusCode = StatusCode.TaskNotFound
                    };
                }
                await _repository.Delete(task);

                return new BaseResponse<TaskEntity>()
                {
                    Description = $"Задача удалена",
                    StatusCode = StatusCode.Ok
                };

            }
            catch (Exception ex)
            {
                _logger.LogInformation($"[TaskService.Delete]:{ex.Message}");
                return new BaseResponse<TaskEntity>()
                {
                    Description = $"Ошибка: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
                
            }
        }

        public async Task<IBaseResponse<TaskEntity>> Edit(CreateTaskModel model)
        {
            try
            {
                model.Validation();
                _logger.LogInformation($"Запрос на редактирование задачи: {model.Id}");

                var task = await _repository.GetAll()
                    .FirstOrDefaultAsync(x => x.Id == model.Id);
                if (task == null) // Если задача не найдена
                {
                    return new BaseResponse<TaskEntity>()
                    {
                        Description = "Задача не найдена",
                        StatusCode = StatusCode.TaskNotFound
                    };
                }
                task.Name = model.Name;
                task.Description = model.Description;
                task.Priority = model.Priority;

                await _repository.Update(task); // Обновление задачи в базе данных
                _logger.LogInformation($"Задача успешно обновлена: {model.Id}");

                return new BaseResponse<TaskEntity>()
                {
                    Description = "Задача успешно обновлена",
                    StatusCode = StatusCode.Ok
                };
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"[TaskService.Edit]:{ex.Message}");
                return new BaseResponse<TaskEntity>()
                {
                    Description = $"Ошибка: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<bool>> EndTask(long id)
        { // Заверщение задачи по id
            try
            {
                var task = await _repository.GetAll()
                    .FirstOrDefaultAsync(x => x.Id == id);
                if (task == null) 
                {
                    return new BaseResponse<bool>()
                    {
                        Description = $"Задача не найдена",
                        StatusCode = StatusCode.TaskNotFound
                    };
                }
                task.IsDone = true;
                await _repository.Update(task);

                return new BaseResponse<bool>()
                {
                    Description = $"Задача завершенна",
                    StatusCode = StatusCode.Ok
                };
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"[TaskService.EndTask]:{ex.Message}");
                return new BaseResponse<bool>()
                {
                    Description = $"Ошибка: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<IEnumerable<TaskCompletedViewModel>>> GetComplatedTask()
        { // Вывод завершенных задач
            try
            {
                var task = await _repository.GetAll()
                    .Where(x => x.IsDone) // isDone = true
                    .Where(x => x.Created.Date == DateTime.Today)
                    .Select(x => new TaskCompletedViewModel()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                      
                    })
                    .ToListAsync();

                return new BaseResponse<IEnumerable<TaskCompletedViewModel>>()
                {
                    Data = task,
                    StatusCode = StatusCode.Ok
                };
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"[TaskService.GetComplatedTask]:{ex.Message}");
                return new BaseResponse<IEnumerable<TaskCompletedViewModel>>()
                {
                    Description = $"Ошибка: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<DataTableResult> GetTasks(TaskFilter filter)
        {
            try
            {
                var task = await _repository.GetAll()
                    .Where(x => x.IsDone == false) // Хранить только Не завершенные задачи

                    // Фильтрация происходит есть строка не пустая
                    .WhereIf(!string.IsNullOrWhiteSpace(filter.Name), x => x.Name == filter.Name)
                    .WhereIf(filter.Priority.HasValue, x => x.Priority == filter.Priority)
                                        
                    .Select(x => new TaskViewModel()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                        IsDone = x.IsDone == true ? "Готова" : "Не готова",
                        Priority = x.Priority.GetDisplayName(),
                        Created = x.Created.ToLongDateString(),
                       
                    })
                    .Skip(filter.Skip) // Сколько элементов пропустить (пагинация)
                    .Take(filter.PageSize) // Сколько элементов взять (пагинация)
                    .ToListAsync();

                var count = _repository.GetAll().Count(x => x.IsDone == false); // Колич. всех записей (пагинация)


                return new DataTableResult()
                {
                    Data = task,
                    Total = count,
                };
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"[TaskService.Created]:{ex.Message}");
                return new DataTableResult()
                {
                    Data = null,
                    Total = 0,
                };
            }
        }


    }
}
