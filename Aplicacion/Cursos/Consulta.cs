using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Dominio;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Cursos
{
    public class Consulta
    {
        public class ListaCursos: IRequest<List<CursoDto>> {}

        public class Manejador: IRequestHandler<ListaCursos, List<CursoDto>> {
            
            private readonly CursosOnlineContext _context;
            private readonly IMapper _mapper;

            public Manejador(CursosOnlineContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<CursoDto>> Handle(ListaCursos request, CancellationToken cancellationToken) {
                var cursos = await _context.Curso.Include(c=>c.ComentarioLista)
                                                 .Include(p=>p.PrecioPromocion)
                                                 .Include(c => c.InstructorLink)
                                                 .ThenInclude(i => i.Instructor).ToListAsync();

                var cursosDto = _mapper.Map<List<Curso>, List<CursoDto>>(cursos);
                
                return cursosDto;
            }
        }
    }
}