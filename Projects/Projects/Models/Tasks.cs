using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Projects.Models
{
    public class Tasks
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int Status { get; set; }


        public string description { get; set; }

        public int Priority { get; set; }
        public int Project_ID { get; set; }

    }
}
