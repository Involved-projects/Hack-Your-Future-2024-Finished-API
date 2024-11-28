using API.Database;
using API.DTOs;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly TodoDbContext _todoDbContext;

        public TodoController(TodoDbContext todoDbContext) {
            _todoDbContext = todoDbContext;
            _todoDbContext.Database.EnsureCreated();
        }

        [HttpPost("[action]")]
        public IActionResult Add([FromBody] TodoDto todoDto)
        {
            var todo = new Todo() {
                Title = todoDto.Title,
                Description = todoDto.Description,
                Assignee = todoDto.Assignee
            };

            _todoDbContext.Todos.Add(todo);
            _todoDbContext.SaveChanges();

            todoDto.Id = todo.Id;

            return Ok(todo);
        }


        [HttpGet("[action]")]
        public IActionResult GetAll()
        {
            var todoDtos = _todoDbContext.Todos
                .Select(todo => new TodoDto() {
                    Id = todo.Id,
                    Title = todo.Title,
                    Description = todo.Description,
                    Assignee = todo.Assignee
                })
                .ToList();

            return Ok(todoDtos);
        }


        [HttpGet("[action]/{id}")]
        public IActionResult Get([FromRoute] int id)
        {
            var todo = _todoDbContext.Todos.Find(id);

            if(todo == null) {
                return NotFound();
            }

            var todoDto = new TodoDto() {
                    Id = todo.Id,
                    Title = todo.Title,
                    Description = todo.Description,
                    Assignee = todo.Assignee
                };

            return Ok(todoDto);
        }


        [HttpGet("[action]")]
        public IActionResult Search([FromQuery] string? title, [FromQuery] string? assignee)
        {
            var todos = _todoDbContext.Todos.AsQueryable();

            if (!string.IsNullOrWhiteSpace(title))
                todos = todos.Where(todo => todo.Title.Contains(title));

            if (!string.IsNullOrWhiteSpace(assignee))
                todos = todos.Where(todo => todo.Assignee.Contains(assignee));

            var todoDtos = todos
                .Select(todo => new TodoDto
                {
                    Id = todo.Id,
                    Title = todo.Title,
                    Assignee = todo.Assignee,
                    Description = todo.Description
                })
                .ToList();

            return Ok(todoDtos);
        }


        [HttpPut("[action]")]
        public IActionResult Update([FromBody] TodoDto todoDto)
        {
            var todo = _todoDbContext.Todos.Find(todoDto.Id);

            if(todo == null) {
                return NotFound();
            }

            todo.Title = todoDto.Title;
            todo.Assignee = todoDto.Assignee;
            todo.Description = todoDto.Description;

            _todoDbContext.Todos.Update(todo);
            _todoDbContext.SaveChanges();

            return Ok();
        }

        [HttpDelete("[action]/{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var todo = _todoDbContext.Todos.Find(id);

            if(todo == null) {
                return NotFound();
            }

            _todoDbContext.Todos.Remove(todo);
            _todoDbContext.SaveChanges();

            return Ok();
        }
    }
}