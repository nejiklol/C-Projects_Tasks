using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Projects.Models;
using Projects;
using MySQLApp;

namespace Projects.Controllers
{
    [Route("/api/[controller]")]    
    public class ProjectsController : Controller
    {
        ApplicationContext db = new ApplicationContext();
        
        [HttpGet]
        public IActionResult Get()
        {
            var projects = db.projects.ToList();
                
            if (projects == null)
            {
                return NotFound();
            }
            return Ok(projects);
        }


        [HttpGet("{id}")]           
        public IActionResult Get(int id)
        {

            var projects = db.projects.ToList();


            if (projects == null)
            {
                return NotFound();
            }
            else
            {
                var mem = projects.SingleOrDefault(p => p.Id == id);
                var a = new List<Tasks>();

                foreach (var i in db.tasks.ToList())
                {
                    if (i.Project_ID == id)
                    {
                        a.Add(i);
                        //mem.Tasks_OF_Project.Add(i);
                    }
                }
                return Ok(mem);
            }
            return NotFound();
            
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var projects = db.projects.ToList();
            var a = new List<Tasks> { };
            db.projects.Remove(db.projects.SingleOrDefault(p => p.Id == id));
            foreach (var i in db.tasks.ToList())
            {
                if (i.Project_ID == id)
                {
                    db.tasks.Remove(i);
                    a.Add(i);
                }
            }
            db.SaveChanges();
            return Ok();
        }

        [HttpGet("/api/TasksOfProject/{Id}")]
        public IActionResult Getid(int Id)
        {
            
            var projects = db.projects.ToList();
            var a = new List<Tasks> { };
            //db.projects.Remove(db.projects.SingleOrDefault(p => p.Id == id));
            foreach (var i in db.tasks.ToList())
            {
                if (i.Project_ID == Id)
                {
                    a.Add(i);
                }
            }
            if (a.Count() == 0)
            {
                return NotFound();
            }
            else
            {
                return Ok(a);
            }
        }
        private int NextProjectId =>  db.projects.Count() == 0 ? 1 : db.projects.Max(x => x.Id) + 1;
        
        [HttpGet("GetNextProjectId")]    //  проверка: /api/GetNextProjectId/
        public int GetNextProjectId()
        {
            return NextProjectId;
        }

        [HttpPost]
        public IActionResult Post(Project project)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            project.Id = NextProjectId;
            try
            {
                db.projects.Add(project);
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
            db.SaveChanges();
            return Ok();
        }

        [HttpPost("AddProject")]
        public IActionResult PostBody([FromBody] Project project) =>
            Post(project);

        [HttpPut]
        public IActionResult Put(Project project)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var storedProject = db.projects.SingleOrDefault(p => p.Id == project.Id);
            if (storedProject == null) return NotFound();
            storedProject.Name = project.Name;
            storedProject.Project_Start_date = project.Project_Start_date;
            storedProject.Project_Completion_date = project.Project_Completion_date;
            storedProject.Status = project.Status;
            storedProject.Priority = project.Priority;
            db.SaveChanges();
            return Ok(storedProject);
        }

    }
}
