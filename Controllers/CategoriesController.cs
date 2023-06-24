using Microsoft.AspNetCore.Mvc;
using OnlineBookStoreAPI.DataTransferObjects;
using OnlineBookStoreAPI.Models;
using OnlineBookStoreAPI.Services;

namespace OnlineBookStoreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly IGenericRepository<Category, string> _categoriesRepository;
        private readonly IGenericRepository<Book, int> _booksRepository;

        public CategoriesController(
            IGenericRepository<Category, string> categoriesRepository,
            IGenericRepository<Book, int> booksRepository)
        {
            _categoriesRepository = categoriesRepository;
            _booksRepository = booksRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CategoryDTO>))]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _categoriesRepository.GetAllAsync();
            var categoryDTOs = categories.Select(category =>
                new CategoryDTO
                {
                    Name = category.Name,
                    
                    BookIds = category.Books.Select(book => book.Id).ToList()
                });
            return Ok(categoryDTOs);
        }

        [HttpGet("{categoryName}")]
        [ProducesResponseType(200, Type = typeof(CategoryDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCategoryByName(string categoryName)
        {
            var category = await _categoriesRepository.GetByIdAsync(categoryName);
            if (category == null)
            {
                return NotFound();
            }

            var categoryDTO = new CategoryDTO
            {
                Name = category.Name,
                
                BookIds = category.Books.Select(book => book.Id).ToList()
            };

            return Ok(categoryDTO);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CategoryDTO))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> AddCategory(string name, ICollection<int> bookIds)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("The name field is required.");
            }

            List<Book>? books = null;
            if (bookIds != null && bookIds.Any())
            {
                books = new List<Book>();
                foreach (var bookId in bookIds)
                {
                    var book = await _booksRepository.GetByIdAsync(bookId);
                    if (book == null)
                    {
                        return BadRequest("Specified book with provided ID wasn't found.");
                    }
                }
                  
            }

            var category = new Category
            {
                Name = name,
                Books = books
            };

            category = await _categoriesRepository.AddAsync(category);

            var categoryDTO = new CategoryDTO
            {
                Name = category.Name,
                
                BookIds = category.Books.Select(book => book.Id).ToList()
            };

            return CreatedAtAction(nameof(GetCategoryByName), new { CategoryName = category.Name }, categoryDTO);
        }

        [HttpPut("{categoryName}")]
        [ProducesResponseType(200, Type = typeof(CategoryDTO))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpdateCategory(string categoryName, [FromQuery] List<int>? bookIds)
        {
            var existingCategory = await _categoriesRepository.GetByIdAsync(categoryName);
            if (existingCategory == null)
            {
                return NotFound();
            }

            if (bookIds != null && bookIds.Any())
            {
                existingCategory.Books = new List<Book>();
                
                foreach (var bookId in bookIds)
                {
                    var book = await _booksRepository.GetByIdAsync(bookId);
                    if (book == null)
                    {
                        return BadRequest("One or more of the provided book IDs wasn't found.");
                    }
                    existingCategory.Books.Add(book);
                }
                

               
            }

            existingCategory = await _categoriesRepository.UpdateAsync(existingCategory);

            var categoryDTO = new CategoryDTO
            {
                Name = existingCategory.Name,
                
                BookIds = existingCategory.Books.Select(book => book.Id).ToList()
            };

            return CreatedAtAction(nameof(GetCategoryByName), new { CategoryName = existingCategory.Name }, categoryDTO);
        }

        [HttpDelete("{categoryName}")]
        [ProducesResponseType(200, Type = typeof(CategoryDTO))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> DeleteCategory(string categoryName)
        {
            var category = await _categoriesRepository.DeleteAsync(categoryName);
            if (category == null)
            {
                return NotFound();
            }

            var categoryDTO = new CategoryDTO
            {
                Name = category.Name,
                
                BookIds = category.Books.Select(book => book.Id).ToList()
            };

            return Ok(categoryDTO);
        }
    }
}