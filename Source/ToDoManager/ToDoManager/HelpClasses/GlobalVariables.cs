using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoManager.Models;

namespace ToDoManager.HelpClasses
{
    public static class GlobalVariables
    {
        //public static Uri _baseAddres =  new Uri("https://localhost:7078/");
        public static String _baseAddres ="https://localhost:7078/";
        public static List<ToDoTask> ToDoTask { get; set; }
    }
}
