using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBellwether.API.Models.Translation.Yandex
{
    public class YandexResponse
    {
        public int Code { get; set; }
        public string Lang { get; set; }
        public IEnumerable<string> Text { get; set; }
    }
}
