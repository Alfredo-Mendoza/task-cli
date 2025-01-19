using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task_cli.Models
{
    public class Tasks
    {
        public int id { get; set; }
        public string description { get; set; }
        public string status { get; set; } = EnumTaskStatus.In_progress.ToString();
        public DateTime createdAt { get; set; } = DateTime.Now;
        public DateTime updatedAt { get; set; } = DateTime.Now;
    }
}
