using BookCommand.Service.Services;
using BookCommand.Service.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BookCommand.Service.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _service;
        public BookController(IBookService service)
        {
            _service = service;
        }

        // create a new book
        [HttpPost]
        public async Task<ActionResult> CreateAsync(BookCreateViewModel book)
        {
            await _service.CreateAsync(book);
            return Ok();
        }

        // update an existing book
        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateAsync(int id, BookUpdateViewModel book)
        {
            await _service.UpdateAsync(id, book);
            return Ok();
        }

        // delete an existing book
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await _service.DeleteAsync(id);
            return Ok();
        }
    }
}
