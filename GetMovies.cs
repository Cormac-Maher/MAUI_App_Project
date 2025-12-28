using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MovieExplorer
{
    public class GetMovies
    {
        private readonly string fileName = "moviesemoji.json";
        private readonly string fileUrl = "https://github.com/Cormac-Maher/MAUI_App_Project/blob/master/movies.JSON";

        public async Task<List<Movies>> LoadMoviesAsync()
        {
            string path = Path.Combine(FileSystem.AppDataDirectory, fileName);
            string jsonContent;

            if (File.Exists(path))
            {
                jsonContent = await File.ReadAllTextAsync(path);
            }
            else
            {
                using var httpClient = new HttpClient();
                var response = await httpClient.GetAsync(fileUrl);
                if (response == null || !response.IsSuccessStatusCode)
                    return new List<Movies>();

                jsonContent = await response.Content.ReadAsStringAsync();
                await File.WriteAllTextAsync(path, jsonContent);
            }

            var movies = JsonSerializer.Deserialize<List<Movies>>(jsonContent);
            return movies ?? new List<Movies>();
        }
    }
}
