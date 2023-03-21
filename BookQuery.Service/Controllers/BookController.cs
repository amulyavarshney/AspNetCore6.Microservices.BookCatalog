using BookQuery.Service.ViewModels;
using BookQuery.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookQuery.Service.Controllers
{
    /* The BookController class is a controller class that handles HTTP requests and responses */
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        /* This is a constructor that takes in a service and assigns it to a private variable. */
        private readonly IBookService _service;
        public BookController(IBookService service)
        {
            _service = service;
        }

        /// <summary>
        /// It returns a list of books from the database
        /// </summary>
        /// <returns>
        /// A list of BookViewModel objects.
        /// </returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookViewModel>>> GetAsync()
        {
            return Ok(await _service.GetAllAsync());
        }

        /// <summary>
        /// This function is a GET request that takes an integer as a parameter and returns a
        /// BookViewModel object
        /// </summary>
        /// <param name="id">int - This is the route parameter. It's the id of the book we want to
        /// get.</param>
        /// <returns>
        /// The GetByIdAsync method is returning a BookViewModel object.
        /// </returns>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<BookViewModel>> GetByIdAsync(int id)
        {
            return Ok(await _service.GetByIdAsync(id));
        }
    }
}
