using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.Filters.Task;
using ToDoList.Domain.Interfaces;
using ToDoList.Domain.ViewModel.Task;

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
        public async Task<IActionResult>TaskHeandlet(TaskFilter filter)
        {
            var response = await _service.GetTasks(filter);

            return Json(new { data = response.Data });
        }

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
        public async Task<IActionResult>EndTask(long id)
        {
            var response = await _service.EndTask(id);
            if (response.StatusCode == Domain.Enum.StatusCode.Ok)
            {
                return Ok(new { description = response.Description });
            }
            return BadRequest(new { description = response.Description });
        }

        [HttpGet]
        public async Task<IActionResult> GetComplatedTask()
        {
            var result = await _service.GetComplatedTask();
            return Json(new { data = result.Data });
          

        }

    }
}