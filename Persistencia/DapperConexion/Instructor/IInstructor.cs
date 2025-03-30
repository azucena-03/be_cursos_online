using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Persistencia.DapperConexion.Instructor
{
    public interface IInstructor
    {
        Task<IEnumerable<InstructorModel>> ObtenerLista();
        Task<InstructorModel> ObtenerPorId();
        Task<int> Nuevo(InstructorModel parametros);
        Task<int> Actualiza(InstructorModel parametros);
        Task<int> ELimina(Guid id);
    }
}