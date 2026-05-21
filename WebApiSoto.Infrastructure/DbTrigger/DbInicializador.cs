using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiSoto.API.Middleware.Exceptions;
using WebApiSoto.Infrastructure.Context;

namespace WebApiSoto.Infrastructure.DbTrigger
{
    public class DbInicializador : IDbInicializador
    {
        private readonly AppDbContext _context;
        public DbInicializador(AppDbContext con)
        {
            _context = con;
        }

        public void Inicializar()
        {
            try
            {
                if (_context.Database.GetPendingMigrations().Count() > 0)
                    _context.Database.Migrate();
            }
            catch(Exception )
            {
                throw new MigrationErrorException(500);
            }
        }
    }
}
