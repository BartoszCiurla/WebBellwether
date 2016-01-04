using System.ComponentModel.DataAnnotations.Schema;

namespace WebBellwether.API.Entities.Translations
{
    [Table("Language")]
    public class LanguageDao
    {
        public int Id { get; set; }
        public string LanguageName { get; set; }
        public string LanguageShortName { get; set; }
        public bool IsPublic { get; set; }
    }
}