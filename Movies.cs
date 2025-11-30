using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieExplorer
{
    public class Movies
    {
        public string title { get; set; }
        public int year { get; set; }
        public List<string> genre { get; set; }
        public string director { get; set; }
        public double rating { get; set; }
        public string emoji { get; set; }
    }
}
