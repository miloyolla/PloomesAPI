using Microsoft.AspNetCore.Mvc;
using Ploomes.Models;
using System.Data.SqlClient;

namespace Ploomes.Controllers
{
    [Route("api/Clientes")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly SqlConnection _sqlConnection;

        public ClientesController(SqlConnection sqlConnection)
        {
            _sqlConnection = sqlConnection;
        }

        [HttpPost]
        [Route(nameof(ClientesController.Cadastrar))]
        public async Task<IActionResult> Cadastrar([FromBody] ClientesModel cliente)
        {
            try
            {
                await _sqlConnection.OpenAsync();
                var sql = "INSERT INTO CLIENTES (Id, Nome, Email, ContaId) VALUES (@Id, @Nome, @Email, @ContaId)";
                using (var cmd = new SqlCommand(sql, _sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@Id", cliente.Id);
                    cmd.Parameters.AddWithValue("@Nome", cliente.Nome);
                    cmd.Parameters.AddWithValue("@Email", cliente.Email);
                    cmd.Parameters.AddWithValue("@ContaId", cliente.ContaId);

                    await cmd.ExecuteScalarAsync();

                }

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            finally
            {
                _sqlConnection.Close();
            }
        }

        [HttpPut]
        [Route(nameof(ClientesController.Atualizar))]
        public async Task<IActionResult> Atualizar(int contaId, int clienteId, [FromBody] ClientesModel cliente)
        {
            try
            {
                await _sqlConnection.OpenAsync();
                var sql = "UPDATE CLIENTES SET Nome = @Nome, Email = @Email WHERE Id = @clienteId, contaid = @contaId";
                using (var cmd = new SqlCommand(sql, _sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@Nome", cliente.Nome);
                    cmd.Parameters.AddWithValue("@Email", cliente.Email);

                    await cmd.ExecuteScalarAsync();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            finally
            {
                _sqlConnection.Close();
            }

        }
    }
}
