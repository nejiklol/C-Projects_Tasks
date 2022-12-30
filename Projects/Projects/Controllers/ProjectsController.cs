using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Projects.Models;
using Projects;
using MySQLApp;


// This file is used to work with the table of projects and related tasks


namespace Projects.Controllers
{
    [Route("/api/[controller]")]    
    public class ProjectsController : Controller
    {
        ApplicationContext db = new ApplicationContext();   // The 'db' variable queries the database

        [HttpGet]
        public IActionResult Get()                          // View all projects in the database
        {
            var projects = db.projects.ToList();
                
            if (projects == null)
            {
                return NotFound();                          // If there is no data, this query will return a bump NotFound()
            }
            return Ok(projects);                            // If not, it will return all existing projects
        }


        [HttpGet("{id}")]           
        public IActionResult Get(int id)                    // Here we return the project with the entered id 
                                                            // Also, the 'Get' function is overloaded because the project id is added
        {

            var projects = db.projects.ToList();


            if (projects == null)                           // If the project with the requested id is not in the database, an error will be returned,
                                                            // Otherwise, an error will be returned
            {
                return NotFound();
            }
            else
            {

                //var a = new List<Tasks>();                // Here I tried to output all the tasks belonging to the project
                //foreach (var i in db.tasks.ToList())      // But I had a conflict with the type of variable 
                //{                                         // in a project class, because of an error when creating the database
                //    if (i.Project_ID == id)               // Since there was no data in the database in the form of an array I could not add to the database 
                //    {                                     // arrays of tasks
                //        a.Add(i);                         // I had an idea to load strings at project request after Get operation
                //        //mem.Tasks_OF_Project.Add(i);    // but a conflict of data types occurred, so I abandoned the idea
                //    }                                     // And made a separate query for the tasks of a particular project
                //}
                return Ok(projects.SingleOrDefault(p => p.Id == id));
            }
            return NotFound();
            
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)                 // Project deletion function by id
        {                                                   // This function also deletes all tasks belonging to the project
            var projects = db.projects.ToList();
            var a = new List<Tasks> { };
            db.projects.Remove(db.projects.SingleOrDefault(p => p.Id == id));
            foreach (var i in db.tasks.ToList())            // This loop goes through all requests for belonging to
            {                                               // the project to be deleted, i understand that this is
                                                            // poorly implemented, because all tasks are loaded into RAM
                if (i.Project_ID == id)                     // And only after that is the sampling of all the tasks
                {                                           // But queries to the database via this library are not possible
                    db.tasks.Remove(i);                     // That is why the implementation looks like this
                    a.Add(i);
                }
            }
            db.SaveChanges();
            return Ok();
        }

        [HttpGet("/api/TasksOfProject/{Id}")]
        public IActionResult Getid(int Id)                  // Query tasks by project id
        {
            
            var projects = db.projects.ToList();
            var a = new List<Tasks> { };
            
            foreach (var i in db.tasks.ToList())           // The implementation is similar to the one used 
            {                                              // when deleting project tasks
                if (i.Project_ID == Id)
                {
                    a.Add(i);
                }
            }
            if (a.Count() == 0)                           // when deleting project tasks
            {
                return NotFound();
            }
            else
            {
                return Ok(a);
            }
        }
        private int NextProjectId =>  db.projects.Count() == 0 ? 1 : db.projects.Max(x => x.Id) + 1;
        // NextProjectId variable is used to determine the id of the next project,
        // In this case, if from the projects with the numbers
        // 1 2 3 remove 2, then the next project will have the id 4
        // This is not correct, but more understandable in implementation and writing.

        public int GetNextProjectId()                   // Function for determining the next project id
        {
            return NextProjectId;
        }

        [HttpPost]
        public IActionResult Post(Project project)      // Function for adding a new element
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            project.Id = NextProjectId;
            if (project.Status < 1 || project.Status > 2)
            {
                return BadRequest($"Status can only be 1,2,3");
                                                        // Status can only be 1,2,3 (NotStarted, Active, Completed)
            }
            try
            {
                db.projects.Add(project);               
            }
            catch(Exception ex)
            {
                return BadRequest(ex);                  // If the data are not suitable for addition, an error will be thrown
            }
            db.SaveChanges();
            return Ok();
        }

        [HttpPost("AddProject")]                        // Call the add function
        public IActionResult PostBody([FromBody] Project project) =>
            Post(project);

        [HttpPut]
        public IActionResult Put(Project project)       // Function for changing data in the selected project
        {
            if (!ModelState.IsValid)                    // If there is no project with this id, an error will be thrown
            {
                return BadRequest(ModelState);
            }
            var storedProject = db.projects.SingleOrDefault(p => p.Id == project.Id);
            if (storedProject == null) return NotFound();
            storedProject.Name = project.Name;                          
            storedProject.Project_Start_date = project.Project_Start_date;
            storedProject.Project_Completion_date = project.Project_Completion_date;
            storedProject.Status = project.Status; 
            if (storedProject.Status < 1 || storedProject.Status > 2)
            {
                return BadRequest($"Status can only be 1,2,3");
            }
            storedProject.Priority = project.Priority;  // Line-by-line addition of the project data to
                                                        // which you want to change the parameters
            db.SaveChanges();
            return Ok(storedProject);
        }

    }
}
