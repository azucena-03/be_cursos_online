using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace Persistencia.DapperConexion.Instructor
{
    public class InstructorRepository : IInstructor
    {
        private readonly IFactoryConnection _connection;
        public InstructorRepository(IFactoryConnection connection)
        {
            _connection = connection;
        }

        public Task<int> Actualiza(InstructorModel parametros)
        {
            throw new NotImplementedException();
        }

        public Task<int> ELimina(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<int> Nuevo(InstructorModel parametros)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<InstructorModel>> ObtenerLista()
        {
            IEnumerable<InstructorModel> instructorList = null;
            var storeProcedure = "usp_Obtener_Instructores";
            try
            {
                var connection = _connection.GetConnection();
                instructorList = await connection.QueryAsync<InstructorModel>(storeProcedure, null, commandType: CommandType.StoredProcedure);
            }catch(Exception ex){
                throw new Exception("Error en la consulta de datos", ex);
            }
            finally
            {
                _connection.CloseConnection();
            }
            return instructorList;
        }

        public Task<InstructorModel> ObtenerPorId()
        {
            throw new NotImplementedException();
        }
    }
}