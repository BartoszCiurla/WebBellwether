using System.Collections.Generic;

namespace WebBellwether.Models.Models.Translation.Yandex
{
    public class YandexResponse
    {
        public int Code { get; set; }
        public string Lang { get; set; }
        public IEnumerable<string> Text { get; set; }
    }
}
