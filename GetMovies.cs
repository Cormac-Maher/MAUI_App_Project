using System.Net.Http.Json;
using System.Text.Json;

namespace MovieExplorer
{
    public class GetMovies
    {
        private readonly string fileName = "moviesemoji.json";
        private readonly string fileUrl = "https://raw.githubusercontent.com/Cormac-Maher/MAUI_App_Project/master/MovieExplorer/moviesemoji.json";            // JSON URL 
        public async Task<List<Movies>> LoadMoviesAsync()
        {
            string localPath = Path.Combine(FileSystem.AppDataDirectory, fileName);
            List<Movies> movies = new();

            if (File.Exists(localPath))
            {
                string jsonContent = await File.ReadAllTextAsync(localPath);            // json is read from local file 
                if (!IsHtml(jsonContent))
                {
                    movies = DeserializeMethod(jsonContent);
                }
            }

            if (movies == null || movies.Count == 0)
            {
                movies = await DownloadFile(localPath);        // if local file is not there, json is downloaded from the internet link
            }

            if (movies == null || movies.Count == 0)
            {      
                movies = GetFallbackJson();                                                         // If everything else fails, use hardcoded JSON, this means the app can be used without internet
            }

            return movies;
        }

        private async Task<List<Movies>> DownloadFile(string localPath)
        {
            try
            {
                using var http = new HttpClient();
                http.DefaultRequestHeaders.Add("User-Agent", "MauiApp-MovieExplorer");                      // github sometimes needed a User-Agent header to allow the download
                var response = await http.GetAsync(fileUrl);

                if (response == null || !response.IsSuccessStatusCode)
                {
                    return new List<Movies>();
                }
                string json = await response.Content.ReadAsStringAsync();
             
                if (IsHtml(json))
                    return new List<Movies>();                // in case download returned HTML 
                
                await File.WriteAllTextAsync(localPath, json);      // JSON is saved locally for future use
                return DeserializeMethod(json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"DOWNLOAD ERROR: {ex.Message}");       // Bug fixing
                return new List<Movies>();
            }
        }


        private bool IsHtml(string content)
        {
            return content.TrimStart().StartsWith("<");             // see if content is HTML
        }

        private List<Movies> DeserializeMethod(string json)
        {
            try
            {
                return JsonSerializer.Deserialize<List<Movies>>(json)             // Deserialize JSON string to List
                       ?? new List<Movies>();
            }
            catch
            {
                return new List<Movies>();
            }
        }

        private List<Movies> GetFallbackJson()    // HardcodedJSON data
        {
            string fallbackJson = @"[
                  {""title"": ""The Shawshank Redemption"", ""year"": 1994, ""genre"": [""Drama""], ""director"": ""Frank Darabont"", ""rating"": 9.3, ""emoji"": ""🎭"", ""image"": ""https://upload.wikimedia.org/wikipedia/en/8/81/ShawshankRedemptionMoviePoster.jpg""},
                  {""title"": ""The Godfather"", ""year"": 1972, ""genre"": [""Crime"", ""Drama""], ""director"": ""Francis Ford Coppola"", ""rating"": 9.2, ""emoji"": ""🕴️"", ""image"": ""https://image.tmdb.org/t/p/original/9cqNxx0GxF0bflZmeSMuL5tnGzr.jpg""},
                  {""title"": ""The Dark Knight"", ""year"": 2008, ""genre"": [""Action"", ""Crime"", ""Drama""], ""director"": ""Christopher Nolan"", ""rating"": 9.0, ""emoji"": ""🦇"", ""image"": ""https://commons.wikimedia.org/wiki/File:The_Dark_Knight_movie_poster_-_censored_copyright.jpg""},
                  {""title"": ""Pulp Fiction"", ""year"": 1994, ""genre"": [""Crime"", ""Drama""], ""director"": ""Quentin Tarantino"", ""rating"": 8.9, ""emoji"": ""💼"", ""image"": ""https://en.wikipedia.org/wiki/File:Pulp_Fiction_Theatrical_Poster.jpg""},
                  {""title"": ""Schindler's List"", ""year"": 1993, ""genre"": [""Biography"", ""Drama"", ""History""], ""director"": ""Steven Spielberg"", ""rating"": 8.9, ""emoji"": ""📜"", ""image"": ""https://en.wikipedia.org/wiki/File:Schindler%27s_List_movie_poster.jpg""},
                  {""title"": ""The Lord of the Rings: The Return of the King"", ""year"": 2003, ""genre"": [""Action"", ""Adventure"", ""Drama""], ""director"": ""Peter Jackson"", ""rating"": 8.9, ""emoji"": ""🗡️"", ""image"": ""https://en.wikipedia.org/wiki/File:The_Lord_of_the_Rings_The_Return_of_the_King_poster.jpg""},
                  {""title"": ""Fight Club"", ""year"": 1999, ""genre"": [""Drama""], ""director"": ""David Fincher"", ""rating"": 8.8, ""emoji"": ""🥊"", ""image"": ""https://en.wikipedia.org/wiki/File:Fight_Club_poster.jpg""},
                  {""title"": ""Forrest Gump"", ""year"": 1994, ""genre"": [""Drama"", ""Romance""], ""director"": ""Robert Zemeckis"", ""rating"": 8.8, ""emoji"": ""🏃‍♂️"", ""image"": ""https://en.wikipedia.org/wiki/File:Forrest_Gump_poster.jpg""},
                  {""title"": ""Inception"", ""year"": 2010, ""genre"": [""Action"", ""Adventure"", ""Sci-Fi""], ""director"": ""Christopher Nolan"", ""rating"": 8.8, ""emoji"": ""🚀"", ""image"": ""https://en.wikipedia.org/wiki/File:Inception_(2010)_theatrical_poster.jpg""},
                  {""title"": ""The Matrix"", ""year"": 1999, ""genre"": [""Action"", ""Sci-Fi""], ""director"": ""Lana Wachowski, Lilly Wachowski"", ""rating"": 8.7, ""emoji"": ""🕶️"", ""image"": ""https://en.wikipedia.org/wiki/File:The_Matrix_Poster.jpg""},
                  {""title"": ""Goodfellas"", ""year"": 1990, ""genre"": [""Biography"", ""Crime"", ""Drama""], ""director"": ""Martin Scorsese"", ""rating"": 8.7, ""emoji"": ""💰"", ""image"": ""https://en.wikipedia.org/wiki/File:Goodfellas_poster.jpg""},
                  {""title"": ""The Empire Strikes Back"", ""year"": 1980, ""genre"": [""Action"", ""Adventure"", ""Fantasy""], ""director"": ""Irvin Kershner"", ""rating"": 8.7, ""emoji"": ""🌌"", ""image"": ""https://en.wikipedia.org/wiki/File:Star_Wars_The_Empire_Strikes_Back_1980_theatrical_poster.jpg""},
                  {""title"": ""Interstellar"", ""year"": 2014, ""genre"": [""Adventure"", ""Drama"", ""Sci-Fi""], ""director"": ""Christopher Nolan"", ""rating"": 8.6, ""emoji"": ""🌌"", ""image"": ""https://en.wikipedia.org/wiki/File:Interstellar_film_poster.jpg""},
                  {""title"": ""City of God"", ""year"": 2002, ""genre"": [""Crime"", ""Drama""], ""director"": ""Fernando Meirelles, Kátia Lund"", ""rating"": 8.6, ""emoji"": ""🏙️"", ""image"": ""https://en.wikipedia.org/wiki/File:City_of_God_poster.jpg""},
                  {""title"": ""Se7en"", ""year"": 1995, ""genre"": [""Crime"", ""Drama"", ""Mystery""], ""director"": ""David Fincher"", ""rating"": 8.6, ""emoji"": ""🔎"", ""image"": ""https://en.wikipedia.org/wiki/File:Se7en_poster.jpg""},
                  {""title"": ""The Silence of the Lambs"", ""year"": 1991, ""genre"": [""Crime"", ""Drama"", ""Thriller""], ""director"": ""Jonathan Demme"", ""rating"": 8.6, ""emoji"": ""🐑"", ""image"": ""https://en.wikipedia.org/wiki/File:The_Silence_of_the_Lambs_movie_poster.jpg""},
                  {""title"": ""Saving Private Ryan"", ""year"": 1998, ""genre"": [""Drama"", ""War""], ""director"": ""Steven Spielberg"", ""rating"": 8.6, ""emoji"": ""🎖️"", ""image"": ""https://en.wikipedia.org/wiki/File:Saving_Private_Ryan_poster.jpg""},
                  {""title"": ""The Green Mile"", ""year"": 1999, ""genre"": [""Crime"", ""Drama"", ""Fantasy""], ""director"": ""Frank Darabont"", ""rating"": 8.6, ""emoji"": ""🟩"", ""image"": ""https://en.wikipedia.org/wiki/File:The_Green_Mile_film_poster.jpg""},
                  {""title"": ""Gladiator"", ""year"": 2000, ""genre"": [""Action"", ""Adventure"", ""Drama""], ""director"": ""Ridley Scott"", ""rating"": 8.5, ""emoji"": ""⚔️"", ""image"": ""https://en.wikipedia.org/wiki/File:Gladiator_film_poster.jpg""},
                  {""title"": ""The Lion King"", ""year"": 1994, ""genre"": [""Animation"", ""Adventure"", ""Drama""], ""director"": ""Roger Allers, Rob Minkoff"", ""rating"": 8.5, ""emoji"": ""🦁"", ""image"": ""https://en.wikipedia.org/wiki/File:The_Lion_King_theatrical_poster.jpg""},
                  {""title"": ""Terminator 2: Judgment Day"", ""year"": 1991, ""genre"": [""Action"", ""Sci-Fi""], ""director"": ""James Cameron"", ""rating"": 8.5, ""emoji"": ""🤖"", ""image"": ""https://en.wikipedia.org/wiki/File:Terminator_2_poster.jpg""},
                  {""title"": ""The Dark Knight Rises"", ""year"": 2012, ""genre"": [""Action"", ""Adventure"", ""Thriller""], ""director"": ""Christopher Nolan"", ""rating"": 8.4, ""emoji"": ""🦇"", ""image"": ""https://en.wikipedia.org/wiki/File:The_Dark_Knight_Rises_poster.jpg""},
                  {""title"": ""Batman Begins"", ""year"": 2005, ""genre"": [""Action"", ""Adventure""], ""director"": ""Christopher Nolan"", ""rating"": 8.2, ""emoji"": ""🦇"", ""image"": ""https://en.wikipedia.org/wiki/File:Batman_Begins.jpg""},
                  {""title"": ""Inglourious Basterds"", ""year"": 2009, ""genre"": [""Adventure"", ""Drama"", ""War""], ""director"": ""Quentin Tarantino"", ""rating"": 8.3, ""emoji"": ""🎖️"", ""image"": ""https://en.wikipedia.org/wiki/File:Inglourious_Basterds_poster.jpg""},
                  {""title"": ""Kill Bill: Vol. 1"", ""year"": 2003, ""genre"": [""Action"", ""Crime""], ""director"": ""Quentin Tarantino"", ""rating"": 8.1, ""emoji"": ""⚔️"", ""image"": ""https://en.wikipedia.org/wiki/File:Kill_Bill_Vol._1_poster.jpg""},
                  {""title"": ""Kill Bill: Vol. 2"", ""year"": 2004, ""genre"": [""Action"", ""Crime""], ""director"": ""Quentin Tarantino"", ""rating"": 8.0, ""emoji"": ""🗡️"", ""image"": ""https://en.wikipedia.org/wiki/File:Kill_Bill_Vol._2_poster.jpg""},
                  {""title"": ""The Hateful Eight"", ""year"": 2015, ""genre"": [""Crime"", ""Drama"", ""Mystery""], ""director"": ""Quentin Tarantino"", ""rating"": 7.8, ""emoji"": ""❄️"", ""imagek"": ""https://en.wikipedia.org/wiki/File:The_Hateful_Eight_(film)_poster.jpg""},
                  {""title"": ""Insomnia"", ""year"": 2002, ""genre"": [""Drama"", ""Mystery"", ""Thriller""], ""director"": ""Christopher Nolan"", ""rating"": 7.2, ""emoji"": ""🌙"", ""image"": ""https://en.wikipedia.org/wiki/File:Insomnia_2002_film_poster.jpg""},
                  {""title"": ""Dunkirk"", ""year"": 2017, ""genre"": [""Action"", ""Drama"", ""History""], ""director"": ""Christopher Nolan"", ""rating"": 7.9, ""emoji"": ""⛵"", ""image"": ""https://en.wikipedia.org/wiki/File:Dunkirk_2017_film_poster.jpg""},
                  {""title"": ""The Theory of Everything"", ""year"": 2014, ""genre"": [""Biography"", ""Drama"", ""Romance""], ""director"": ""James Marsh"", ""rating"": 7.7, ""emoji"": ""🌌"", ""image"": ""https://en.wikipedia.org/wiki/File:The_Theory_of_Everything_(film)_poster.jpg""},
                  {""title"": ""Spotlight"", ""year"": 2015, ""genre"": [""Biography"", ""Crime"", ""Drama""], ""director"": ""Tom McCarthy"", ""rating"": 8.2, ""emoji"": ""📰"", ""image"": ""https://en.wikipedia.org/wiki/File:Spotlight_(film)_poster.jpg""},
                  {""title"": ""Ford v Ferrari"", ""year"": 2019, ""genre"": [""Action"", ""Biography"", ""Drama""], ""director"": ""James Mangold"", ""rating"": 8.1, ""emoji"": ""🏎️"", ""image"": ""https://en.wikipedia.org/wiki/File:Ford_v_Ferrari_(film)_poster.jpg""},
                  {""title"": ""Jojo Rabbit"", ""year"": 2019, ""genre"": [""Comedy"", ""Drama"", ""War""], ""director"": ""Taika Waititi"", ""rating"": 7.9, ""emoji"": ""🎭"", ""image"": ""https://en.wikipedia.org/wiki/File:Jojo_Rabbit_(2019_film)_poster.jpg""},
                  {""title"": ""1917"", ""year"": 2019, ""genre"": [""Drama"", ""War""], ""director"": ""Sam Mendes"", ""rating"": 8.3, ""emoji"": ""🎖️"", ""image"": ""https://en.wikipedia.org/wiki/File:1917_2019_film_poster.jpg""},
                  {""title"": ""Blade Runner 2049"", ""year"": 2017, ""genre"": [""Action"", ""Drama"", ""Sci-Fi""], ""director"": ""Denis Villeneuve"", ""rating"": 8.0, ""emoji"": ""🤖"", ""image"": ""https://en.wikipedia.org/wiki/File:Blade_Runner_2049_poster.jpg""},
                  {""title"": ""No Country for Old Men"", ""year"": 2007, ""genre"": [""Crime"", ""Drama"", ""Thriller""], ""director"": ""Joel Coen, Ethan Coen"", ""rating"": 8.1, ""emoji"": ""🔫"", ""image"": ""https://en.wikipedia.org/wiki/File:No_Country_for_Old_Men_poster.jpg""},
                  {""title"": ""The Revenant"", ""year"": 2015, ""genre"": [""Action"", ""Adventure"", ""Drama""], ""director"": ""Alejandro G. Iñárritu"", ""rating"": 8.0, ""emoji"": ""❄️"", ""image"": ""https://en.wikipedia.org/wiki/File:The_Revenant_2015_film_poster.jpg""},
                  {""title"": ""Slumdog Millionaire"", ""year"": 2008, ""genre"": [""Drama"", ""Romance""], ""director"": ""Danny Boyle"", ""rating"": 8.0, ""emoji"": ""🎲"", ""image"": ""https://en.wikipedia.org/wiki/File:Slumdog_Millionaire_poster.jpg""},
                  {""title"": ""The Imitation Game"", ""year"": 2014, ""genre"": [""Biography"", ""Drama"", ""Thriller""], ""director"": ""Morten Tyldum"", ""rating"": 8.0, ""emoji"": ""💻"", ""image"": ""https://en.wikipedia.org/wiki/File:The_Imitation_Game_poster.jpg""},
                  {""title"": ""The Pianist"", ""year"": 2002, ""genre"": [""Biography"", ""Drama"", ""Music""], ""director"": ""Roman Polanski"", ""rating"": 8.5, ""emoji"": ""🎹"", ""image"": ""https://en.wikipedia.org/wiki/File:The_Pianist_film_poster.jpg""},
                  {""title"": ""Amélie"", ""year"": 2001, ""genre"": [""Comedy"", ""Romance""], ""director"": ""Jean-Pierre Jeunet"", ""rating"": 8.3, ""emoji"": ""🌷"", ""image"": ""https://en.wikipedia.org/wiki/File:Am%c3%a9lie_poster.jpg""},
                  {""title"": ""Pan's Labyrinth"", ""year"": 2006, ""genre"": [""Drama"", ""Fantasy"", ""War""], ""director"": ""Guillermo del Toro"", ""rating"": 8.2, ""emoji"": ""🧚"", ""image"": ""https://en.wikipedia.org/wiki/File:Pan%27s_Labyrinth_poster.jpg""},
                  {""title"": ""The Sixth Sense"", ""year"": 1999, ""genre"": [""Drama"", ""Mystery"", ""Thriller""], ""director"": ""M. Night Shyamalan"", ""rating"": 8.1, ""emoji"": ""👻"", ""image"": ""https://en.wikipedia.org/wiki/File:The_Sixth_Sense_poster.jpg""},
                  {""title"": ""The Hunt"", ""year"": 2012, ""genre"": [""Drama""], ""director"": ""Thomas Vinterberg"", ""rating"": 8.3, ""emoji"": ""🦌"", ""image"": ""https://en.wikipedia.org/wiki/File:Jagten_poster.jpg""},
                  {""title"": ""Life Is Beautiful"", ""year"": 1997, ""genre"": [""Comedy"", ""Drama"", ""Romance""], ""director"": ""Roberto Benigni"", ""rating"": 8.6, ""emoji"": ""🎪"", ""image"": ""https://en.wikipedia.org/wiki/File:Life_Is_Beautiful_poster.jpg""},
                  {""title"": ""Up"", ""year"": 2009, ""genre"": [""Animation"", ""Adventure"", ""Comedy""], ""director"": ""Pete Docter, Bob Peterson"", ""rating"": 8.2, ""emoji"": ""🎈"", ""image"": ""https://en.wikipedia.org/wiki/File:Up_(2009_film).jpg""},
                  {""title"": ""Inside Out"", ""year"": 2015, ""genre"": [""Animation"", ""Adventure"", ""Comedy""], ""director"": ""Pete Docter, Ronnie del Carmen"", ""rating"": 8.1, ""emoji"": ""😊"", ""image"": ""https://en.wikipedia.org/wiki/File:Inside_Out_(2015_film)_poster.jpg""},
                  {""title"": ""Coco"", ""year"": 2017, ""genre"": [""Animation"", ""Adventure"", ""Family""], ""director"": ""Lee Unkrich, Adrian Molina"", ""rating"": 8.4, ""emoji"": ""💀"", ""image"": ""https://en.wikipedia.org/wiki/File:Coco_(2017_film)_poster.jpg""},
                  {""title"": ""The Incredibles"", ""year"": 2004, ""genre"": [""Animation"", ""Action"", ""Adventure""], ""director"": ""Brad Bird"", ""rating"": 8.0, ""emoji"": ""🦸‍♂️"", ""image"": ""https://en.wikipedia.org/wiki/File:The_Incredibles_(2004_animated_feature_film).jpg""},
                  {""title"": ""Monsters, Inc."", ""year"": 2001, ""genre"": [""Animation"", ""Adventure"", ""Comedy""], ""director"": ""Pete Docter, David Silverman"", ""rating"": 8.1, ""emoji"": ""👹"", ""image"": ""https://en.wikipedia.org/wiki/File:Monsters_Inc.JPG""},
                  {""title"": ""Shrek"", ""year"": 2001, ""genre"": [""Animation"", ""Adventure"", ""Comedy""], ""director"": ""Andrew Adamson, Vicky Jenson"", ""rating"": 7.9, ""emoji"": ""🧅"", ""image"": ""https://en.wikipedia.org/wiki/File:DreamWorks_Shrek_1_Poster_HQ.png""}  
            ]";
            return JsonSerializer.Deserialize<List<Movies>>(fallbackJson) ?? new List<Movies>();
        }


    }
}
