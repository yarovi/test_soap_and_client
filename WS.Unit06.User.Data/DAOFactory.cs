using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WS.Unit06.User.Data;
using WS.Unit06.User.Data.DAO;
using WS.Unit06.User.Data.DAO.Impl;


namespace WS.Unit06.user.Data
{
    public class DAOFactory : IDisposable
    {
        private DataContext _context;

        public DAOFactory()
        {
            _context = new DataContext();
        }

        public IUserDAO UserDAO
        {
            get { return new UserDAO(_context); }
        }

        
        public void Dispose() { _context.Dispose(); }
    }
}