using System.ComponentModel.DataAnnotations;

namespace ReversiRestApi.Models.DTO
{
    public class SpelPutDTO
    {
        [Required]
        public string Token { get; set; }

        public string Speler1Token { get; set; }

        public string Speler2Token { get; set; }

        public int? Y { get; set; }

        public int? X { get; set; }
    }
}
