using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Projects.Models
{
    public class Project 
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        
        public DateTime Project_Start_date { get; set; }

        
        public DateTime Project_Completion_date { get; set; }

       
        public int Status { get; set; }

        
        public int Priority { get; set; }

        //public List<Tasks> Tasks_OF_Project { get; set; }

    }

}
