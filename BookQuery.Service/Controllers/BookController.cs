using BookQuery.Service.ViewModels;
using BookQuery.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookQuery.Service.Controllers
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

        // get all books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookViewModel>>> GetAsync()
        {
            return Ok(await _service.GetAllAsync());
        }

        // get book by id
        [HttpGet("{id:int}")]
        public async Task<ActionResult<BookViewModel>> GetByIdAsync(int id)
        {
            return Ok(await _service.GetByIdAsync(id));
        }
    }
}
