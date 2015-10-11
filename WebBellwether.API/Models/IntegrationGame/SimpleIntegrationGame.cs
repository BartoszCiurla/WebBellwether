using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebBellwether.API.Entities.Translations;

namespace WebBellwether.API.Models.IntegrationGame
{
    public class SimpleIntegrationGame
    {
        public int Id { get; set; }
        public Language Language { get; set; }
    }
}