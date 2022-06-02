using ReversiRestApi.Models;
using System;
using System.Collections.Generic;

namespace ReversiRestApi.DAL.Interfaces
{
    public interface IUitslagRepository
    {
        void Add(Uitslag spel);

        public bool Update(Uitslag spel);

        public List<Uitslag> GetAll();

        Uitslag Get(Guid ID);
    }
}
