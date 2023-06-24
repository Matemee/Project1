using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using OnlineBookStoreAPI.DataTransferObjects;
using OnlineBookStoreAPI.Models;
using OnlineBookStoreAPI.Services;

namespace OnlineBookStoreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorsController : ControllerBase
    {
        private readonly IGenericRepository<Author, int> _authorsRepository;
        private readonly IGenericRepository<Book, int> _booksRepository;

        public AuthorsController(IGenericRepository<Author, int> authorsRepository, IGenericRepository<Book, int> booksRepository)
        {
            _authorsRepository = authorsRepository;
            _booksRepository = booksRepository;
        }


        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<AuthorDTO>))]
        public async Task<IActionResult> GetAllAuthors()
        {
            var authors = await _authorsRepository.GetAllAsync();

            var authorDTOs = authors.Select(author => new AuthorDTO()
            {
                Id = author.Id,
                Name = author.Name,
                DateOfBirth = author.DateOfBirth,
                BookIds = author.Books.Select(book => book.Id).ToList()
            });

            return Ok(authorDTOs);
        }

        [HttpGet("{Id}")]
        [ProducesResponseType(200, Type = typeof(AuthorDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAuthorById(int Id)
        {
            var author = await _authorsRepository.GetByIdAsync(Id);
            if (author == null)
            {
                return NotFound();
            }

            var authorDTO = new AuthorDTO()
            {
                Id = author.Id,
                Name = author.Name,
                DateOfBirth = author.DateOfBirth,
                BookIds = author.Books.Select(book => book.Id).ToList()
            };

            return Ok(authorDTO);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(AuthorDTO))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> AddAuthor([FromBody] AuthorDTO createAuthorDTO)
        {
            List<Book>? Books = null;
            if (createAuthorDTO.BookIds != null && createAuthorDTO.BookIds.Count() > 0)
            {
                Books = new List<Book>();
                foreach (var bookId in createAuthorDTO.BookIds)
                {
                    var book = await _booksRepository.GetByIdAsync(bookId);
                    if (book == null)
                    {
                        return BadRequest($"Book with the specified Id is not found. \nId: {bookId}\nThis field is optional, there might be authors which don't have any book associated");
                    }
                    Books.Add(book);
                }
            }

            var author = new Author
            {
                Name = createAuthorDTO.Name,
                DateOfBirth = createAuthorDTO.DateOfBirth,
                Books = Books
            };

            author = await _authorsRepository.AddAsync(author);

            var authorDTO = new AuthorDTO()
            {
                Id = author.Id,
                Name = author.Name,
                DateOfBirth = author.DateOfBirth,
                BookIds = author.Books.Select(book => book.Id).ToList()
            };

            return CreatedAtAction(nameof(GetAuthorById), new { Id = author.Id }, authorDTO);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(200, Type = typeof(AuthorDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpdateAuthor(int id, [FromBody] AuthorDTO authorDTO)
        {
            var author = await _authorsRepository.GetByIdAsync(id); if (author == null) { return NotFound(); }

            List<Book>? Books = null;
            if (authorDTO.BookIds != null && authorDTO.BookIds.Count() > 0)
            {
                Books = new List<Book>();
                foreach (var bookId in authorDTO.BookIds)
                {
                    var book = await _booksRepository.GetByIdAsync(bookId);
                    if (book == null)
                    {
                        return BadRequest($"Book with the specified Id is not found. \nId: {bookId}\nThis field is optional, there might be authors which don't have any book associated");
                    }
                    Books.Add(book);
                }
            }

            author.Name = authorDTO.Name;
            author.DateOfBirth = authorDTO.DateOfBirth;
            author.Books = Books;

            await _authorsRepository.UpdateAsync(author);

            return Ok(authorDTO);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var author = await _authorsRepository.GetByIdAsync(id);
            if (author == null)
            {
                return NotFound();
            }

            await _authorsRepository.DeleteAsync(id);

            return NoContent();
        }
    }
}