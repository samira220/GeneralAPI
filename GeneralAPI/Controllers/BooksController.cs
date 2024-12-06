
using Microsoft.AspNetCore.Mvc;
using GeneralAPI.Model;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace GeneralAPI.Controllers
{
    public class BooksController : Controller
    {
        private readonly HttpClient _httpClient;

        public BooksController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        [HttpGet]
        [Route("getAllBooks")]
        public async Task<IActionResult> GetAllBooks() //РАБОТАЕТ
        {
            try
            {
                var response = await _httpClient.GetAsync("http://localhost:9999/api/Books/getAllBooks");
                response.EnsureSuccessStatusCode();

                // Десериализация в объект ApiResponse
                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();

                if (apiResponse == null || apiResponse.Books == null)
                {
                    return StatusCode(500, "Ответ от сервиса книг не содержит данных.");
                }

                return Ok(apiResponse.Books);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, $"Ошибка при выполнении запроса: {ex.Message}");
            }
            catch (JsonException ex)
            {
                return StatusCode(500, $"Ошибка десериализации JSON: {ex.Message}");
            }

        }

        

        [HttpGet]
        [Route("getBookAuthor_Name/{Author}/{Name}")] //РАБОТАЕТ
        public async Task<IActionResult> GetBookAuhtor_Name(string Author, string Name)
        {

                // Выполняем запрос с передачей параметров Author и Name через маршрут
                var response = await _httpClient.GetAsync($"http://localhost:9999/api/Books/getBookAuthor_Name/{Author}/{Name}");
                response.EnsureSuccessStatusCode();

                // Десериализация ответа в объект с одним свойством books
                var bookResponse = await response.Content.ReadFromJsonAsync<BookResponse>();

                // Проверяем, получили ли данные
                if (bookResponse?.Books == null)
                {
                    return NotFound("Книга с указанными автором и названием не найдена.");
                }

                return Ok(bookResponse.Books);

        }

        [HttpGet]
        [Route("getBook/{Id}")]
        public async Task<IActionResult> GetBookId(int Id) //РАБОТАЕТ
        {
            try
            {
                var response = await _httpClient.GetAsync($"http://localhost:9999/api/Books/getBook/{Id}");
                response.EnsureSuccessStatusCode();

                // Десериализация в объект ApiResponse
                var bookResponse = await response.Content.ReadFromJsonAsync<BookResponse>();

                // Проверяем, получили ли данные
                if (bookResponse?.Books == null)
                {
                    return NotFound("Книга с указанными автором и названием не найдена.");
                }

                return Ok(bookResponse.Books);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, $"Ошибка при выполнении запроса: {ex.Message}");
            }
            catch (JsonException ex)
            {
                return StatusCode(500, $"Ошибка десериализации JSON: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("getBookZhanr/{Id_Zhanr}")]
        public async Task<IActionResult> GetBookId_Zhanr(int Id_Zhanr) //РАБОТАЕТ
        {
            try
            {
                var response = await _httpClient.GetAsync($"http://localhost:9999/api/Books/getBook/{Id_Zhanr}");
                response.EnsureSuccessStatusCode();

                // Десериализация в объект ApiResponse
                var bookResponse = await response.Content.ReadFromJsonAsync<BookResponse>();

                // Проверяем, получили ли данные
                if (bookResponse?.Books == null)
                {
                    return NotFound("Книга с указанными автором и названием не найдена.");
                }

                    return Ok(bookResponse.Books);
                }
                catch (HttpRequestException ex)
                {
                    return StatusCode(500, $"Ошибка при выполнении запроса: {ex.Message}");
                }
                catch (JsonException ex)
                {
                    return StatusCode(500, $"Ошибка десериализации JSON: {ex.Message}");
                }
        }

        [HttpPost]
        [Route("ADDBook")]
        public async Task<IActionResult> PostBook([FromBody] Books newBook) //РАБОТАЕТ
        {
            try
            {
                // Отправка POST-запроса с JSON-данными
                var response = await _httpClient.PostAsJsonAsync("http://localhost:9999/api/Books/ADDBook", newBook);
                response.EnsureSuccessStatusCode();

                // Проверка успешного добавления
                var result = await response.Content.ReadAsStringAsync();
                return Ok(result);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, $"Ошибка при выполнении запроса: {ex.Message}");
            }
            catch (JsonException ex)
            {
                return StatusCode(500, $"Ошибка десериализации JSON: {ex.Message}");
            }
        }


        [HttpPut]
        [Route("UpdateBook")]
        public async Task<IActionResult> PutBook([FromBody] Books UpdateBook) //РАБОТАЕТ
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync("http://localhost:9999/api/Books/UpdateBook", UpdateBook);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsStringAsync();
                return Ok($"Книга с ID {UpdateBook.Id_Book} успешно обновлена.");
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, $"Ошибка при выполнении запроса: {ex.Message}");
            }
            catch (JsonException ex)
            {
                return StatusCode(500, $"Ошибка десериализации JSON: {ex.Message}");
            }

        }

      

        [HttpGet]
        [Route("api/books")]
        public async Task<IActionResult> GetBooks([FromQuery] string? Name, [FromQuery] string? Author, [FromQuery] int? zhanr, [FromQuery] DateTime? year)
        {
            try
            {
                var response = await _httpClient.GetAsync("http://localhost:9999/api/Books/api/books");
                response.EnsureSuccessStatusCode();
                var bookResponse = await response.Content.ReadFromJsonAsync<BookResponse>();
                if (bookResponse?.Books == null)
                {
                    return NotFound("Книга с указанными автором и названием не найдена.");
                }
                    return Ok(bookResponse.Books);
                }
                catch (HttpRequestException ex)
                {
                    return StatusCode(500, $"Ошибка при выполнении запроса: {ex.Message}");
                }
                catch (JsonException ex)
                {
                    return StatusCode(500, $"Ошибка десериализации JSON: {ex.Message}");
                }
        }

        [HttpGet]
        [Route("api/books/pagination")]
        public async Task<IActionResult> GetBooksPagination(
            [FromQuery] string? Name,
            [FromQuery] string? author,
            [FromQuery] int? Zhanr,
            [FromQuery] DateTime? year,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 2)
        {
            try
            {
                var response = await _httpClient.GetAsync("http://localhost:9999/api/Books/api/books/pagination");
                response.EnsureSuccessStatusCode();

                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();

                if (apiResponse == null || apiResponse.Books == null)
                {
                    return StatusCode(500, "Ответ от сервиса книг не содержит данных.");
                }

                return Ok(apiResponse.Books);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, $"Ошибка при выполнении запроса: {ex.Message}");
            }
            catch (JsonException ex)
            {
                return StatusCode(500, $"Ошибка десериализации JSON: {ex.Message}");
            }
            

        }

        [HttpDelete]
        [Route("DeleteBook/{Id}")] //РАБОТАЕТ
        public async Task<IActionResult> DeleteBook(int Id)
        {
            try
            {
                // Отправка DELETE-запроса
                var response = await _httpClient.DeleteAsync($"http://localhost:9999/api/Books/DeleteBook/{Id}");
                response.EnsureSuccessStatusCode();

                // Проверка успешного удаления
                var result = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(result))
                {
                    return StatusCode(500, "Ответ от сервиса книг пустой.");
                }

                return Ok($"Книга с ID {Id} успешно удалена.");
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, $"Ошибка при выполнении запроса: {ex.Message}");
            }
            catch (JsonException ex)
            {
                return StatusCode(500, $"Ошибка десериализации JSON: {ex.Message}");
            }

        }
    }
}
