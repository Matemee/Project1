using Microsoft.AspNetCore.Mvc;
using OnlineBookStoreAPI.DataTransferObjects;
using OnlineBookStoreAPI.Models;
using OnlineBookStoreAPI.Services;

namespace OnlineBookStoreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IGenericRepository<Book, int> _booksRepository;
        private readonly IGenericRepository<Author, int> _authorsRepository;
        private readonly IGenericRepository<Category, string> _categoriesRepository;
        private readonly IBooksRepository _iBooksRepository;

        public BooksController(IGenericRepository<Book, int> booksRepository,
                               IGenericRepository<Author, int> authorsRepository,
                               IGenericRepository<Category, string> categoriesRepository,
                               IBooksRepository iBooksRepository)
        {
            _booksRepository = booksRepository;
            _authorsRepository = authorsRepository;
            _categoriesRepository = categoriesRepository;
            _iBooksRepository = iBooksRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<BookDTO>))]
        public async Task<IActionResult> GetAllBooks()
        {
            var books = await _booksRepository.GetAllAsync();

            var bookDTOs = books.Select(book => new BookDTO
            {
                Id = book.Id,
                Title = book.Title,
                Description = book.Description,
                Price = Convert.ToDecimal(book.Price),
                PublicationDate = book.PublicationDate,
                AuthorNames = book.Authors.Select(author => author.Name),
                CategoryNames = book.Categories.Select(category => category.Name)
            });

            return Ok(bookDTOs);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(BookDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBookById(int id)
        {
            var book = await _booksRepository.GetByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            var bookDTO = new BookDTO
            {
                Id = book.Id,
                Title = book.Title,
                Description = book.Description,
                Price = Convert.ToDecimal(book.Price),
                PublicationDate = book.PublicationDate,
                AuthorNames = book.Authors.Select(author => author.Name),
                CategoryNames = book.Categories.Select(category => category.Name)
            };

            return Ok(bookDTO);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Book>> AddBook(CreateBookDTO bookDTO)
        {
            // Get categories by name and add them to the book
            var categories = new List<Category>();
            foreach (var categoryName in bookDTO.CategoryNames)
            {
                var category = await _categoriesRepository.GetByIdAsync(categoryName);
                if (category == null)
                {
                    var newCategory = new Category
                    {
                        Name = categoryName
                    };
                    await _categoriesRepository.AddAsync(newCategory);
                    categories.Add(newCategory);
                }
                else
                {
                    categories.Add(category);
                }
            }

            // Get authors by id and add them to the book
            var authors = new List<Author>();
            foreach (var authorId in bookDTO.AuthorIds)
            {
                var author = await _authorsRepository.GetByIdAsync(authorId);
                if (author == null)
                {
                    return BadRequest($"Author with id '{authorId}' not found.");
                }
                authors.Add(author);
            }

            // Create the book entity and add it to the database
            var newBook = new Book
            {
                Title = bookDTO.Title,
                Description = bookDTO.Description,
                Price = bookDTO.Price,
                PublicationDate = bookDTO.PublicationDate,
                Categories = categories,
                Authors = authors
            };
            await _booksRepository.AddAsync(newBook);

            return CreatedAtAction(nameof(GetBookById), new { id = newBook.Id }, newBook);
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateBook(int id, CreateBookDTO bookDTO)
        {
            // Get the book to update
            var book = await _booksRepository.GetByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            // Get categories by name and update them for the book
            var categories = new List<Category>();
            foreach (var categoryName in bookDTO.CategoryNames)
            {
                var category = await _categoriesRepository.GetByIdAsync(categoryName);
                if (category == null)
                {
                    var newCategory = new Category
                    {
                        Name = categoryName
                    };
                    await _categoriesRepository.AddAsync(newCategory);
                    categories.Add(newCategory);
                }
                else
                {
                    categories.Add(category);
                }
            }
            book.Categories = categories;

            // Get authors by id and update them for the book
            var authors = new List<Author>();
            foreach (var authorId in bookDTO.AuthorIds)
            {
                var author = await _authorsRepository.GetByIdAsync(authorId);
                if (author == null)
                {
                    return BadRequest($"Author with id '{authorId}' not found.");
                }
                authors.Add(author);
            }
            book.Authors = authors;

            // Update the rest of the book properties
            book.Title = bookDTO.Title;
            book.Description = bookDTO.Description;
            book.Price = bookDTO.Price;
            book.PublicationDate = bookDTO.PublicationDate;

            await _booksRepository.UpdateAsync(book);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _booksRepository.GetByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            await _booksRepository.DeleteAsync(id);

            return NoContent();
        }


        [HttpGet("search/title/{searchQuery}")]
        public async Task<ActionResult<IEnumerable<Book>>> SearchByTitle(string searchQuery)
        {
            var books = await _iBooksRepository.SelectWithTitle(searchQuery);

            return Ok(books);
        }

        [HttpGet("search/author/{searchQuery}")]
        public async Task<ActionResult<IEnumerable<Book>>> SearchByAuthor(string searchQuery)
        {
            var books = await _iBooksRepository.SelectWithAuthor(searchQuery);

            return Ok(books);
        }


    }
}