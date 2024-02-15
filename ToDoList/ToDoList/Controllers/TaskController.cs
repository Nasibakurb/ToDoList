using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.Filters.Task;
using ToDoList.Domain.Interfaces;
using ToDoList.Domain.ViewModel.Task;
using ToDoList.Infrastructure.Service;

namespace ToDoList.Controllers
{
    public class TaskController : Controller
    {
        private readonly ITaskService _service;
        public TaskController(ITaskService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> TaskHeandlet(TaskFilter filter)
        {
            var start = Request.Form["start"].FirstOrDefault(); // с какой страницы начинается пагинация
            var length = Request.Form["length"].FirstOrDefault(); // сколько элементов отображать
            
            var pageSize = length != null ? int.Parse(length) : 0;  // счет страницы
            var skip = start != null ? int.Parse(start) : 0;

            filter.Skip = skip; // фильтр для пагинации
            filter.PageSize = pageSize;

            var response = await _service.GetTasks(filter); // фильтр для поиска
            return Json(new { recordsFiltered = response.Total, recordsTotal = response.Total, data = response.Data });
        } // recordsTotal - сколько элементов есть всего для высчитывания страниц

        [HttpPost]
        public async Task<IActionResult> Create(CreateTaskModel model)
        {
            var response = await _service.Create(model);

            if (response.StatusCode == Domain.Enum.StatusCode.Ok) // Если объект создался
            { // Метод "ок" с объектом json c содерж. описание 
                return Ok(new {description = response.Description});
            }
            return BadRequest(new { description = response.Description});
        }

        [HttpPost]
        public async Task<IActionResult>EndTask(long id) // Завершение задачи
        {
            var response = await _service.EndTask(id);
            if (response.StatusCode == Domain.Enum.StatusCode.Ok)
            {
                return Ok(new { description = response.Description });
            }
            return BadRequest(new { description = response.Description });
        }

        [HttpGet]
        public async Task<IActionResult> GetComplatedTask() // Вывод завершенных задач
        {
            var result = await _service.GetComplatedTask();
            return Json(new { data = result.Data });
          

        }

        [HttpPost]
        public async Task<IActionResult> CalculateCompletedTask()
        { // Завершение для (статистика в Excel)
            var result = await _service.CalculateCompletedTask();
            if(result.StatusCode == Domain.Enum.StatusCode.Ok)
            { // Создаем сервис 
                var csvService = new CsvBaseService<IEnumerable<TaskViewModel>>();
                var upLoadFile = csvService.UpLoadFile(result.Data); // Массив байтов
                return File(upLoadFile, "txt/csv", $"Статистика" +
                    $" за {DateTime.Now.ToLongDateString}.csv");
            }
            return BadRequest(new { description = result.Description });
        }

    }
}