using System;
using System.Collections.Generic;
using System.Text;

namespace eazy.rest.data.Models
{
    public class TaskDto
    { 
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsDone { get; set; }
        public decimal AmountCharged { get; set; } 
    }
}
