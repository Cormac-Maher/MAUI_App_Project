using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Json;

namespace MovieExplorer
{
    public class MovieService
    {
        private readonly HttpClient _httpClient = new HttpClient();

        public async Task<List<Movies>> GetMoviesAsync()
        {
            var url = "https://raw.githubusercontent.com/DonH-ITS/jsonfiles/refs/heads/main/moviesemoji.json";
            return await _httpClient.GetFromJsonAsync<List<Movies>>(url);
        }
    }
}
