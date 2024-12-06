using Azure.Messaging;
using Microsoft.AspNetCore.Mvc;

namespace GeneralAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImageController : Controller
    {
        private readonly HttpClient _httpClient;
        public ImageController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }
        [HttpGet]
        [Route("GetImage")]
        public async Task<string> Download(string fileName)
        {
            var requestUrl = $"http://localhost:8888/api/Image/GetImage";

            var response = await _httpClient.GetAsync($"http://localhost:8888/api/Image/GetImage?fileName={fileName}");

            var fileBytes = await response.Content.ReadAsByteArrayAsync();

            var currentDirectory = Directory.GetCurrentDirectory();
            var uploadsPath = Path.Combine(currentDirectory, "uploads");
            if (!Directory.Exists(uploadsPath))
            {
                Directory.CreateDirectory(uploadsPath);
            }

            var localFilePath = Path.Combine(uploadsPath, fileName);

            await System.IO.File.WriteAllBytesAsync(localFilePath, fileBytes);

            var fileUrl = $"localhost:8888/uploads/{fileName}";

            return fileUrl;


        }

        [HttpPost]
        [Route("AddImage")]
        public async Task<string> UploadAsync(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    throw new ArgumentException("Файл не выбран или пуст.");

                using (var content = new MultipartFormDataContent())
                {
                    var fileStreamContent = new StreamContent(file.OpenReadStream())
                    {
                        Headers =
                {
                    ContentLength = file.Length,
                    ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType)
                }
                    };

                    content.Add(fileStreamContent, "file", file.FileName);

                    var response = await _httpClient.PostAsync("http://localhost:8888/api/Image/AddImage", content);

                    if (!response.IsSuccessStatusCode)
                    {
                        var errorMessage = await response.Content.ReadAsStringAsync();
                        throw new HttpRequestException($"Ошибка от микросервиса: {errorMessage}");
                    }

                    return await response.Content.ReadAsStringAsync();
                }
            }
            catch (HttpRequestException ex)
            {
                return $"Ошибка HTTP-запроса: {ex.Message}";
            }
            catch (Exception ex)
            {
                return $"Неожиданная ошибка: {ex.Message}";
            }
        }

        [HttpDelete]
        [Route("DeleteImage")]
        public async Task<bool> FileExists(string fileName)
        {

            var response = await _httpClient.GetAsync($"http://localhost:8888/api/Image/DeleteImage?fileName={fileName}");
            response.EnsureSuccessStatusCode();

            var exists = await response.Content.ReadAsStringAsync();
            return bool.Parse(exists);



        }
    }
}
