using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetNotes()
        {
            // Sample data for demonstration purposes
            var notes = new List<string>
            {
                "Note 1: Meeting at 10 AM",
                "Note 2: Buy groceries",
                "Note 3: Call John"
            };
            return Ok(notes);
        }
    }
}
