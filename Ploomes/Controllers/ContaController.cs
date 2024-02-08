using Microsoft.AspNetCore.Mvc;
using Ploomes.Models;
using System.Data.SqlClient;

namespace Ploomes.Controllers
{
    [Route("api/Contas")]
    [ApiController]
    public class ContaController : ControllerBase {

        private readonly SqlConnection _sqlConnection;

        public ContaController(SqlConnection sqlConnection)
        {
            _sqlConnection = sqlConnection;
        }

        [HttpPost]
        [Route(nameof(ClientesController.Cadastrar))]
        public async Task<IActionResult> Cadastrar([FromBody] ContaModel conta)
        {
            try
            {
                await _sqlConnection.OpenAsync();
                var sql = "INSERT INTO CONTAS (Id, Nome, Data_Criacao) VALUES (@Id, @Nome, @DataCriacao)";
                using (var cmd = new SqlCommand(sql, _sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@Id", conta.Id);
                    cmd.Parameters.AddWithValue("@Nome", conta.Nome);
                    cmd.Parameters.AddWithValue("@DataCriacao", DateTime.Now);

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

        [HttpGet("{contaId}")]
        public async Task<IActionResult> ListarClientes(int contaId)
        {
            try
            {
                await _sqlConnection.OpenAsync();

                var clientes = new List<ClientesModel>();

                var sql = "SELECT FROM CLIENTES (Id, Nome, Data_Criacao) WHERE ContaId = @contaId";
                using (var cmd = new SqlCommand(sql, _sqlConnection))
                {
                    using (var render = await cmd.ExecuteReaderAsync())
                    {
                        while (await render.ReadAsync())
                        {
                            clientes.Add(new ClientesModel
                            {
                                Id = render.GetInt32(0),
                                Nome = render.GetString(1),
                                Email = render.GetString(2)
                            });
                        }
                    }
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
