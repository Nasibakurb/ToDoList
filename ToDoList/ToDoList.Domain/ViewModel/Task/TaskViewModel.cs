using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Domain.Enum;

namespace ToDoList.Domain.ViewModel.Task
{
    public class TaskViewModel
    {
        public int Id { get; set; }
 
        [Display(Name = "Название")]
        public string Name { get; set; }
        
        [Display(Name = "Готовность")]
        public string IsDone { get; set; }

        [Display(Name = "Приоритет")]
        public string Priority { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Display(Name = "Дата создания")]
        public string Created { get; set; }
        

    }
}
