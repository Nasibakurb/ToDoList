using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Domain.Enum
{
    public enum Priority
    {
        [Display(Name = "Легкая")]
        Easy = 1,
        [Display(Name = "Средняя")]
        Medium = 2,
        [Display(Name = "Тяжелая")]
        Hard = 3
    }

}
