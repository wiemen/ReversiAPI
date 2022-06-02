using ReversiRestApi.DAL.Interfaces;
using ReversiRestApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ReversiRestApi.DAL
{
    public class UitslagAccessLayer : IUitslagRepository
    {
        private readonly ReversiContext _context;

        public UitslagAccessLayer(ReversiContext context)
        {
            _context = context;
        }

        public void Add(Uitslag uitslag)
        {
            _context.Uitslag.Add(uitslag);
            _context.SaveChanges();
        }

        public Uitslag Get(Guid id)
        {
            return _context.Uitslag.FirstOrDefault(x => x.ID == id);
        }

        public List<Uitslag> GetAll()
        {
            return _context.Uitslag.ToList();
        }

        public bool Update(Uitslag uitslag)
        {
            try
            {
                var index = _context.Uitslag.Update(uitslag);
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
