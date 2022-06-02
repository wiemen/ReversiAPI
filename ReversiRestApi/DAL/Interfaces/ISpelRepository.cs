using ReversiRestApi.Models;
using System.Collections.Generic;

namespace ReversiRestApi.DAL.Interfaces
{
    public interface ISpelRepository
    {
        void Add(Spel spel);

        public bool Update(Spel spel);

        public List<Spel> GetSpellen();

        Spel Get(string spelToken);
    }
}
