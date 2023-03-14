using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ToDoManager.Models
{
    public class TimeTables 
    {
        public string? Id { get; set; }

        public string Email { get; set; } = null!;
        public List<Time> Timetable { get; set; } = null!;
    }

    public class Time
    {
        public string Date { get; set; } = null!;

        public string NameTask { get; set; } = null!;

        public string Of { get; set; } = null!;

        public string To { get; set; } = null!;

    }

}
