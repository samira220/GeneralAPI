using GeneralAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace GeneralAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ZhanrController : Controller
    {
        private readonly HttpClient _httpClient;

        public ZhanrController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }
        [HttpGet]
        [Route("getAllZhanr")]
        public async Task<IActionResult> GetAllZhanr() //РАБОТАЕТ
        {
            try
            {
                var response = await _httpClient.GetAsync("http://localhost:9999/api/Zhanr/getAllZhanr");
                response.EnsureSuccessStatusCode();

                // Десериализация в объект ApiResponse
                var apiResponse = await response.Content.ReadFromJsonAsync<APIResponce>();

                if (apiResponse == null || apiResponse.Zhanr == null)
                {
                    return StatusCode(500, "Ответ от сервиса книг не содержит данных.");
                }

                return Ok(apiResponse.Zhanr);
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
        [Route("ADDZhanr")] //РАБОТАЕТ
        public async Task<IActionResult> PostZhanr([FromBody] Zhanr newZhanr)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("http://localhost:9999/api/Zhanr/ADDZhanr", newZhanr);
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
        [Route("UpdateZhanr")] //РАБОТАЕТ
        public async Task<IActionResult> PutZhanr([FromBody] Zhanr UpdateZhanr)
        {


            try
            {
                var response = await _httpClient.PutAsJsonAsync("http://localhost:9999/api/Zhanr/UpdateZhanr", UpdateZhanr);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsStringAsync();
                return Ok($"Жанр с ID {UpdateZhanr.Id_Zhanr} успешно обновлен.");
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
        [Route("DeleteZhanr/{Id}")] //РАБОТАЕТ
        public async Task<IActionResult> DeleteZhanr(int Id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"http://localhost:9999/api/Zhanr/DeleteZhanr/{Id}");
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(result))
                {
                    return StatusCode(500, "Ответ от сервиса книг пустой.");
                }
                return Ok($"Жанр с ID {Id} успешно удален.");
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
