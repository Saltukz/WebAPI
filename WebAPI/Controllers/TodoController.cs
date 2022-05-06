using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebAPI.Entities;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TodoController : Controller
    {
        private DatabaseContext _db;

        public TodoController(DatabaseContext context)
        {
            _db = context;
        }
        [HttpGet("generate-fakedata")]
        public IActionResult GenerateFakeData()
        {
            if (_db.Todos.Any())
            {
                return Ok("Tablon dolu");
            }
            for(int i = 0; i < 50; i++)
            {
                _db.Todos.Add(new Todo
                {
                    Text = MFramework.Services.FakeData.TextData.GetSentence(),
                    IsCompleted = MFramework.Services.FakeData.BooleanData.GetBoolean(),
                    Description = MFramework.Services.FakeData.TextData.GetSentences(2),

                });
             
            }
            _db.SaveChanges();

            return Ok("tablo dolduruldu.");

        }
        [HttpGet("list")]
        [ProducesResponseType(200,Type=typeof(List<TodoResponse>))]
        public IActionResult List()
        {
            List<TodoResponse> todoResult = _db.Todos.Select(x => new TodoResponse
            {
                Text = x.Text,
                IsCompleted = x.IsCompleted,
                Description = x.Description

            }).ToList();
            return Ok(todoResult);
        }
        [HttpPost("create")]
        [ProducesResponseType(200, Type = typeof(TodoResponse))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(List<TodoResponse>))]
        public IActionResult Create([FromBody]TodoCreateModel model)
        {
            if (ModelState.IsValid)
            {
                Todo todo = new Todo
                {
                    Text = model.Text,
                    Description = model.Description,
                    IsCompleted = false
                };

                try
                {
                    _db.Todos.Add(todo);
                    int affacted = _db.SaveChanges();

                    if (affacted > 0)
                    {
                        TodoResponse result = new TodoResponse
                        {
                            Id = todo.Id,
                            Text = todo.Text,
                            Description = todo.Description,
                            IsCompleted = todo.IsCompleted,
                        };
                        return Created(String.Empty, result);
                    }
                    else
                    {
                        return BadRequest("Kayıt yapılamadı.");
                    }
                }
                catch (Exception e)
                {

                    return BadRequest(e.Message);
                }
               
            }
                
            return BadRequest(ModelState);
            
        }

        [HttpPut("edit/{id}")]
        [ProducesResponseType(200, Type = typeof(TodoResponse))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(string))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(string))]
        public IActionResult Update([FromRoute]int id,[FromBody]TodoUpdateModel model)
        {
            Todo todo = _db.Todos.Find(id);

            if (todo == null)
            {
                return NotFound("Kayıt Bulunamadı.");
            }
            todo.Text = model.Text;
            todo.Description = model.Description;
            todo.IsCompleted = model.IsCompleted;
            int affactedRow = _db.SaveChanges();

            if(affactedRow > 0)
            {
                TodoResponse Response = new TodoResponse
                {
                    Id = todo.Id,
                    Text=todo.Text,
                    Description=todo.Description,
                    IsCompleted=todo.IsCompleted,
                };
                return Ok(Response);
            }
            else
            {
                return BadRequest("Güncelleme Yapılamadı.");
            }
         
        }



        [HttpPatch("changestate/{id}/{isCompleted}")]
        [ProducesResponseType(200, Type = typeof(TodoResponse))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(string))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(string))]
        public IActionResult ChangeState([FromRoute] int id, [FromRoute] bool isCompleted)
        {
            Todo todo = _db.Todos.Find(id);

            if (todo == null)
            {
                return NotFound("Kayıt Bulunamadı.");
            }

            todo.IsCompleted = isCompleted;
            int affactedRow = _db.SaveChanges();

            if (affactedRow > 0)
            {
                TodoResponse Response = new TodoResponse
                {
                    Id = todo.Id,
                    Text = todo.Text,
                    Description = todo.Description,
                    IsCompleted = todo.IsCompleted,
                };
                return Ok(Response);
            }
            else
            {
                return BadRequest("Durum Değiştirilemedi.");
            }

        }

        [HttpDelete("remove/{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(string))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(string))]
        public IActionResult Delete([FromRoute]int id)
        {
            Todo todo = _db.Todos.Find(id);

            if(todo == null)
            {
                return NotFound("Kayıt bulunamadı.");
            }

            _db.Todos.Remove(todo);
            int affected = _db.SaveChanges();

            if(affected > 0)
            {
                return Ok();
            }
            else
            {
                return BadRequest("Kayıt silinemedi");
            }
        }


        [HttpDelete("remove-all")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(string))]
        public IActionResult DeleteAll()
        {
          

            List<Todo> todos = _db.Todos.ToList();


            foreach(Todo todo in todos)
            {
                _db.Todos.Remove(todo);
            }
            int affected = _db.SaveChanges();

            if (affected > 0)
            {
                return Ok();
            }
            else
            {
                return BadRequest("Kayıt yok yada kayıt silinemedi.");
            }
        }
    }
}
