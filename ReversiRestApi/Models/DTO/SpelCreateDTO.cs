using System.ComponentModel.DataAnnotations;

namespace ReversiRestApi.Models.DTO
{
    public class SpelCreateDTO
    {
        [Required]
        public string Speler1Token { get; set; }

        [Required]
        public string Omschrijving { get; set; }
    }
}
