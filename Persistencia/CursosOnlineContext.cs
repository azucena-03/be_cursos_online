using System;
using Dominio;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Persistencia
{
    public class CursosOnlineContext : IdentityDbContext<Usuario>
    {
       public CursosOnlineContext(DbContextOptions options) : base(options){

       }

       protected override void OnModelCreating(ModelBuilder builder){
        base.OnModelCreating(builder);
        builder.Entity<CursoInstructor>().HasKey(ci => new { ci.CursoId, ci.InstructorId });
       }

       public DbSet<Curso> Curso { get; set; }
       public DbSet<Precio> Precio { get; set; }
       public DbSet<Instructor> Instructor { get; set; }
       public DbSet<Comentario> Comentario { get; set; }
       public DbSet<CursoInstructor> CursoInstructor { get; set; }
       
       
    }
}