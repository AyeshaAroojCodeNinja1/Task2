using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Task2.Data;
using Task2.Models;
using Task2.Policies;

namespace Task2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoController : Controller
    {
        private readonly IToDoRepository _repository;
        public ToDoController(ToDoRepository repository)
        {
            _repository = repository;
        }
        [Authorize(Policy = CustomPolicies.SameUserOrAdmin)]
        [HttpGet]        
        public ActionResult<IEnumerable<ToDo>> GetAll()
        {
            var toDo = _repository.GetAll();
            return Ok(toDo);
        }

        [Authorize(Policy = CustomPolicies.SameUserOrAdmin)]
        [HttpGet("{id}")]
        public ActionResult<ToDo> GetById(int id)
        {
            var toDo = _repository.GetById(id);
            return toDo == null ? NotFound() : Ok(toDo);
        }

        [Authorize(Policy = CustomPolicies.SameUserOrAdmin)]
        [HttpPost]       
        public ActionResult<ToDo> Post([FromBody] ToDo item)
        {
            if (item == null)
            {
                return BadRequest("Invalid data");
            }

            try
            {
                item.CreatedBy = GetLoggedInUserId();
                var addedItem = _repository.Create(item);

                return CreatedAtAction(nameof(GetById), new { id = addedItem.Id }, item);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing the request. Please try again later." + ex.Message);
            }
        }
        private string GetLoggedInUserId()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                return identity.Name;
            }
            return null;
        }

        [Authorize(Policy = CustomPolicies.SameUserOrAdmin)]
        [HttpPut("{id}")]        
        public IActionResult Put(int id, [FromBody] ToDo item)
        {
            var existingItem = _repository.GetById(id);

            if (existingItem == null)
            {
                return NotFound("Item not found");
            }

            try
            {
                item.Id = id;
                _repository.Update(item);
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound("Item not found");
            }
        }

        [Authorize(Policy = CustomPolicies.SameUserOrAdmin)]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var existingItem = _repository.GetById(id);

                if (existingItem == null)
                {
                    return NotFound("Todo not found");
                }

                _repository.Delete(id);
                return Ok();

            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing the request. Please try again later." + ex.Message);
            }
        }
    }
}
