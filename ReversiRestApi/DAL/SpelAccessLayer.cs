using ReversiRestApi.DAL.Interfaces;
using ReversiRestApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ReversiRestApi.DAL
{
    public class SpelAccessLayer : ISpelRepository
    {
        private readonly ReversiContext _context;

        public SpelAccessLayer(ReversiContext context)
        {
            _context = context;
        }

        public void Add(Spel spel)
        { 
            _context.Spel.Add(spel);
            _context.SaveChanges();
        }

        public Spel Get(string spelToken)
        {
            return _context.Spel.FirstOrDefault(x=>x.Token.Equals(spelToken));
        }

        public List<Spel> GetSpellen()
        {
            return _context.Spel.ToList();
        }

        public bool Update(Spel spel)
        {
            try
            {
                var index = _context.Spel.Update(spel);
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
