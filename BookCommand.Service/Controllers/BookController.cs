using BookCommand.Service.Services;
using BookCommand.Service.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BookCommand.Service.Controllers
{
    /* The BookController class is a controller class that handles all the HTTP requests for the Book
    resource */
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        /* A constructor that takes in a service and assigns it to a private variable. */
        private readonly IBookService _service;
        public BookController(IBookService service)
        {
            _service = service;
        }

        /// <summary>
        /// It takes a book object, and then calls the CreateAsync function in the service layer, which
        /// then calls the CreateAsync function in the repository layer, which then calls the
        /// CreateAsync function in the data layer, which then calls the CreateAsync function in the
        /// database.
        /// </summary>
        /// <param name="BookCreateViewModel">This is the model that is used to create a new
        /// book.</param>
        /// <returns>
        /// The result of the action.
        /// </returns>
        [HttpPost]
        public async Task<ActionResult> CreateAsync(BookCreateViewModel book)
        {
            await _service.CreateAsync(book);
            return Ok();
        }

        /// <summary>
        /// The function takes an id and a book object, and updates the book with the given id with the
        /// given book object
        /// </summary>
        /// <param name="id">The id of the book to update</param>
        /// <param name="BookUpdateViewModel">This is the model that will be used to update the
        /// book.</param>
        /// <returns>
        /// The Ok() method returns a 200 OK response.
        /// </returns>
        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateAsync(int id, BookUpdateViewModel book)
        {
            await _service.UpdateAsync(id, book);
            return Ok();
        }

        /// <summary>
        /// This function deletes a record from the database
        /// </summary>
        /// <param name="id">int - This is the id of the item you want to delete.</param>
        /// <returns>
        /// The result of the DeleteAsync method.
        /// </returns>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await _service.DeleteAsync(id);
            return Ok();
        }
    }
}
