using Microsoft.EntityFrameworkCore;
using NuGetCrackChecker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCrackChecker.Data
{
    public class ProyectoContext : DbContext
    {
        public ProyectoContext(DbContextOptions options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Juego> Juegos { get; set; }

    }
}
