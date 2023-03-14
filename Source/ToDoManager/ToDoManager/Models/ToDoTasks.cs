using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ToDoManager.Models
{
    public class ToDoTasks
    {
        public string? Id { get; set; }

        public string Email { get; set; } = null!;


        public List<ToDoTask> Tasks { get; set; } = null!;
    }


    public class ToDoTask
    {
        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string Time { get; set; } = null!;

        public string Date { get; set; } = null!;

        public string Reapet { get; set; } = null!;

        public List<ToDoTask> Subtasks { get; set; } = null!;

        public bool Auto_fail { get; set; } = false!;

        public List<string> Group { get; set; } = null!;

        public bool Finish { get; set; } = false!;
    }

}
