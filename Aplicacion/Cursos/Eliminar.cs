using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using MediatR;
using Persistencia;

namespace Aplicacion.Cursos
{
    public class Eliminar
    {
        public class Ejecuta: IRequest{
            public Guid Id { get; set; }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly CursosOnlineContext _context;
            public Manejador(CursosOnlineContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                //Eliminar referencias en otras tablas
                var instructoresBD = _context.CursoInstructor.Where(x=>x.CursoId == request.Id);
                foreach (var instructor in instructoresBD)
                {
                    _context.CursoInstructor.Remove(instructor);
                }

                //Eliminar referencias de comentarios
                var comentariosBD = _context.Comentario.Where(x=>x.CursoId == request.Id);
                foreach (var cmt in comentariosBD)
                {
                    _context.Comentario.Remove(cmt);
                }

                //Eliminar referencia a precio
                var precioDB = _context.Precio.Where(x=>x.CursoId == request.Id).FirstOrDefault();
                if (precioDB != null)
                {
                    _context.Precio.Remove(precioDB);
                }
                
                //Eliminar curso
                var curso = await _context.Curso.FindAsync(request.Id);
                if (curso == null)
                {
                    //throw new Exception("No se pudo eliminar el curso");
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new {mensaje="No se encontro el curso"});
                }

                _context.Remove(curso);
                var resultado = await _context.SaveChangesAsync();

                if (resultado > 0)
                {
                    return Unit.Value;
                }else{
                    throw new Exception("No se pudieron guardar los cambios");
                }
                
            }
        }
    }
}