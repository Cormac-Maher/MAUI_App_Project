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
        private readonly string fileUrl = "https://github.com/Cormac-Maher/MAUI_App_Project/blob/master/movies.JSON";      // online json file movies are from

        public async Task<List<Movies>> LoadMoviesAsync()
        {
            string path = Path.Combine(FileSystem.AppDataDirectory, fileName);                     // path to store json file
            string jsonContent;

            if (File.Exists(path))
            {
                jsonContent = await File.ReadAllTextAsync(path);                // reads local json file
            }
            else
            {
                using var httpClient = new HttpClient();                              // http client to download online file
                var response = await httpClient.GetAsync(fileUrl);
                if (response == null || !response.IsSuccessStatusCode)
                    return new List<Movies>();

                jsonContent = await response.Content.ReadAsStringAsync();           // reads online json file
                await File.WriteAllTextAsync(path, jsonContent);                     // saves online json file locally
            }

            var movies = JsonSerializer.Deserialize<List<Movies>>(jsonContent);       // deserializes json content to list of Movies objects
            return movies ?? new List<Movies>();                                    // returns list of Movies objects
        }
    }
}
