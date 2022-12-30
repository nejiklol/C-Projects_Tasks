using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Projects.Models;
using Projects;
using MySQLApp;

// In this controller, the functions are implemented similar to those of the project controller.
// That's why I think there is no need to describe them

namespace Projects.Controllers
{
    [Route("/api/[controller]")]
    public class TasksController : Controller
    {
        
        ApplicationContext db = new ApplicationContext();

        [HttpGet]
        public IActionResult Get()
        {
            var tasks = db.tasks.ToList();

            if (tasks == null)
            {
                return NotFound();
            }
            return Ok(tasks);
        }


        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {

            var taskss = db.tasks.ToList();

            List<Tasks> ans = new List<Tasks>();
            if (taskss == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(taskss.SingleOrDefault(p => p.Id == id));
            }
            return NotFound();

        }


        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            db.tasks.Remove(db.tasks.SingleOrDefault(p => p.Id == id));
            return Ok();
        }
        [HttpPut]
        public IActionResult Put(Tasks Task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var storedTask = db.tasks.SingleOrDefault(p => p.Id == Task.Id);
            if (storedTask == null) return NotFound();
            storedTask.Name = Task.Name;
            storedTask.Status = Task.Status;
            if (storedTask.Status < 1 || storedTask.Status > 2)
            {
                return BadRequest($"Status can only be 1,2,3");
            }
            storedTask.description = Task.description;
            storedTask.Priority = Task.Priority;
            storedTask.Priority = Task.Project_ID;
            db.SaveChanges();
            return Ok(storedTask);
        }
        private int GetNextTaskId => db.tasks.Count() == 0 ? 1 : db.tasks.Max(x => x.Id) + 1;

        
        public int GetGetNextTaskId()
        {
            return GetNextTaskId;
        }

        [HttpPost]
        public IActionResult Post(Tasks task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            task.Id = GetNextTaskId;
            if (task.Status < 1 || task.Status > 2)
            {
                return BadRequest($"Status can only be 1,2,3")ж
                // Status can only be 1,2,3 (ToDo / InProgress / Done)
            }
            try
            {
                db.tasks.Add(task);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            db.SaveChanges();
            return Ok();
        }

        [HttpPost("AddTask")]
        public IActionResult PostBody([FromBody] Tasks task) =>
            Post(task);
    }
}
