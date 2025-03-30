using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using Dominio;
using FluentValidation;
using MediatR;
using Persistencia;

namespace Aplicacion.Cursos
{
    public class Editar
    {
        public class Ejecuta: IRequest{
            public Guid CursoId { get; set; }
            public string Titulo { get; set; }
            public string Descripcion { get; set; } 
            public DateTime? FechaPublicacion { get; set; }
            public List<Guid> ListaInstructor { get; set; }
            public decimal? Precio { get; set; }
            public decimal? Promocion { get; set; }
        }

        public class EjecutaValidacion : AbstractValidator<Ejecuta>{
            public EjecutaValidacion(){
                RuleFor(x => x.Titulo).NotEmpty();
                RuleFor(x => x.Descripcion).NotEmpty();
                RuleFor(x => x.FechaPublicacion).NotEmpty();
            }
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
                var curso = await _context.Curso.FindAsync(request.CursoId);
                if (curso == null)
                {
                    {
                    //throw new Exception("No se pudo eliminar el curso");
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new {mensaje="No se encontro el curso"});
                }
                }

                curso.Titulo = request.Titulo ?? curso.Titulo;
                curso.Descripcion = request.Descripcion ?? curso.Descripcion;
                curso.FechaPublicacion = request.FechaPublicacion ?? curso.FechaPublicacion;

                //Actualizar precio del curso
                var precioEntidad = _context.Precio.Where(x=>x.CursoId == curso.CursoId).FirstOrDefault();
                if (precioEntidad != null){
                    precioEntidad.PrecioActual = request.Precio ?? precioEntidad.PrecioActual;
                    precioEntidad.Promocion = request.Promocion ?? precioEntidad.Promocion;
                }else{
                    precioEntidad = new Precio {
                        PrecioId = Guid.NewGuid(),
                        PrecioActual = request.Precio ?? 0,
                        Promocion = request.Promocion ?? 0,
                        CursoId = curso.CursoId,
                    };
                    await _context.Precio.AddAsync(precioEntidad);
                }

                if (request.ListaInstructor != null)
                {
                    if (request.ListaInstructor.Count > 0)
                    {
                        //Eliminar los instructore actuales del curso en la base de datos
                        var instructoresBD = _context.CursoInstructor.Where(x => x.CursoId == request.CursoId);
                        foreach (var instructorEliminar in instructoresBD)
                        {
                            _context.CursoInstructor.Remove(instructorEliminar);
                        }

                        //Procedimiento para agregar instructores que provienen del cliente
                        foreach (var id in request.ListaInstructor)
                        {
                            var nuevoInstructor = new CursoInstructor {
                                CursoId = request.CursoId,
                                InstructorId = id
                            };

                            _context.CursoInstructor.Add(nuevoInstructor);
                        }
                    }
                }

                var resultado = await _context.SaveChangesAsync();
                if (resultado > 0)
                {
                    return Unit.Value;
                }else{
                    throw new Exception("No se guardaron los cambios del curso");
                }

            }
        }
    }
}