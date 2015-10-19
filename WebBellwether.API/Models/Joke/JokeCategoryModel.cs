using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBellwether.API.Models.Joke
{
    public class JokeCategoryModel
    {
        public int Id { get; set; }//global id
        public int JokeCategoryId { get; set; } //translate id 
        public string JokeCategoryTemplateName { get; set; } //jako templete zawsze angielski na twardo
        public string JokeCategoryName { get; set; }
        public int LanguageId { get; set; }
    }
}
