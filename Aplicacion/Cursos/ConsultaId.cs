using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using AutoMapper;
using Dominio;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Cursos
{
    public class ConsultaId
    {
        public class CursoUnico : IRequest<CursoDto>{
            public Guid Id { get; set; }
        };

        public class Manejador : IRequestHandler<CursoUnico, CursoDto>
        {
            private readonly CursosOnlineContext _context;
            private readonly IMapper _mapper;
            public Manejador(CursosOnlineContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<CursoDto> Handle(CursoUnico request, CancellationToken cancellationToken)
            {
                var curso = await _context.Curso.Include(c=>c.ComentarioLista)
                                                .Include(p=>p.PrecioPromocion)
                                                .Include(c => c.InstructorLink)
                                                .ThenInclude(i=> i.Instructor)
                                                .FirstOrDefaultAsync(x=> x.CursoId == request.Id);
                if (curso == null)
                {
                    //throw new Exception("No se encontro el curso");
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new {mensaje="No se encontro el curso"});
                }
                var cursoDto = _mapper.Map<Curso, CursoDto>(curso);
                return cursoDto;
            }
        }
    }
}