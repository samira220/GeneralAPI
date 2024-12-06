using GeneralAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.PortableExecutable;
using System.Text.Json;

namespace GeneralAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReadersController : Controller
    {
        private readonly HttpClient _httpClient;
        public ReadersController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }


        [HttpGet]
        [Route("getAllReaders")]
        public async Task<IActionResult> GetAllReaders()
        {
            try
            {
                var response = await _httpClient.GetAsync("http://localhost:1111/api/Reader/getAllReaders");
                response.EnsureSuccessStatusCode();
                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponseReader>();
                if (apiResponse == null || apiResponse.Readers == null)
                {
                    return StatusCode(500, "Ответ от сервиса книг не содержит данных.");
                }

                return Ok(apiResponse.Readers);
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
        [Route("DeleteReader/{Id}")]

        public async Task<IActionResult> DeleteReader(int Id) 
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"http://localhost:1111/api/Reader/DeleteReader/{Id}");
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(result))
                {
                    return StatusCode(500, "Ответ пустой.");
                }

                return Ok($"Читатель с ID {Id} успешно удален.");
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
        [Route("Readers/Pagination")]
        public async Task<IActionResult> GetReaderPagination([FromQuery] string? Name, [FromQuery] string? FName, [FromQuery] string? Contact, [FromQuery] DateTime? Birth_Day, [FromQuery] int page = 1, [FromQuery] int pageSize = 2)
        {

                var response = await _httpClient.GetAsync($"http://localhost:1111/api/Reader/Readers/Pagination?Name={Name}");
                response.EnsureSuccessStatusCode();

                // Десериализация в объект ApiResponse
                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponseReader>();


                return Ok(apiResponse.Readers);

        }

        [HttpGet]
        [Route("getReader/{Id}")]
        public async Task<IActionResult> GetReaderId(int Id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"http://localhost:1111/api/Reader/getReader/{Id}");
                response.EnsureSuccessStatusCode();

                // Десериализация в объект ApiResponse
                var ReaderResponse = await response.Content.ReadFromJsonAsync<ReaderResponce>();

                // Проверяем, получили ли данные
                if (ReaderResponse?.Reader == null)
                {
                    return NotFound("Читатель в базе не найден.");
                }
             
                return Ok(ReaderResponse.Reader);
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
        [Route("ADDReader")]
        public async Task<IActionResult> PostReader([FromBody] Readers newReader)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("http://localhost:1111/api/Reader/ADDReader", newReader);
                response.EnsureSuccessStatusCode();
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
        [Route("UpdateReader")]
        public async Task<IActionResult> PutReader([FromBody] Readers UpdateReader)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync("http://localhost:1111/api/Readers/UpdateReader", UpdateReader);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsStringAsync();
                return Ok($"Читатель с ID {UpdateReader.Id_Reader} успешно обновлен.");
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
        [Route("/Readers/Filter")]
        public async Task<IActionResult> GetReadersFilter([FromQuery] DateTime? DateRegist)
        {
            try
            {
                var query = string.Empty;
                if (DateRegist.HasValue)
                {
                    query = $"?DateRegist={DateRegist.Value.ToString("yyyy-MM-dd")}";
                }

                var response = await _httpClient.GetAsync($"http://localhost:1111/api/Readers/Readers/Filter");
                response.EnsureSuccessStatusCode();
                var readers = await response.Content.ReadFromJsonAsync<List<Readers>>();

                return Ok(readers);
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
