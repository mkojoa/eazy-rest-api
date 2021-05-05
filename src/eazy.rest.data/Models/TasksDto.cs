using System;
using System.Collections.Generic;
using System.Text;

namespace eazy.rest.data.Models
{
    public class TasksDto
    {
        public Guid Id { get; set; } 
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsDone { get; set; }
        public decimal AmountCharged { get; set; }
    }
}
