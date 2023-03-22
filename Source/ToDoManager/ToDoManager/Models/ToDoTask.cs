using System.Collections.Generic;

namespace ToDoManager.Models
{
    public class ToDoTask
    {
        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string Time { get; set; } = null!;

        public string Date { get; set; } = null!;

        public string Reapet { get; set; } = null!;

        public List<ToDoTask> Subtasks { get; set; } = new();

        public bool Auto_fail { get; set; } = false!;

        public bool Finish { get; set; } = false!;
    }
}
