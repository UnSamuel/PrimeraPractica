using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using ProyectoConPostgres.Conector;
using ProyectoConPostgres.Modelo;

namespace ProyectoConPostgres.Controlador
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocenteController : ControllerBase
    {
        private readonly string db = Coneccion.conexionConPostgres();

        [HttpGet]
        public List<Docente> Get()
        {
            List<Docente> docentes = new List<Docente>();
            string select = "SELECT * FROM public.\"Docente\"";

            using NpgsqlConnection connection = new NpgsqlConnection(db);
            try
            {
                connection.Open();
                Console.WriteLine("Conexión obtenida");
                NpgsqlCommand command = new NpgsqlCommand(select, connection);
                NpgsqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Docente docente = new Docente
                        {
                            idDocente = int.Parse(reader["idDocente"].ToString()),
                            Nombre = reader["Nombre"].ToString(),
                            Apellido = reader["Apellido"].ToString(),
                            Ubicacion = reader["Ubicacion"].ToString(),
                            Sexo = bool.Parse(reader["Sexo"].ToString()),
                            CI = reader["CI"].ToString(),
                            idUniversidad = int.Parse(reader["idUniversidad"].ToString())
                        };
                        docentes.Add(docente);
                    }
                }
                reader.Close();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
            return docentes;
        }

        [HttpPost]
        public Docente Create([FromBody] Docente docente)
        {
            string insert = "INSERT INTO public.\"Docente\"(\"Nombre\", \"Apellido\", \"Ubicacion\", \"Sexo\", \"CI\", \"idUniversidad\")" +
                            "VALUES (@Nombre, @Apellido, @Ubicacion, @Sexo, @CI, @idUniversidad) RETURNING \"idDocente\";";

            using NpgsqlConnection connection = new NpgsqlConnection(db);
            try
            {
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand(insert, connection);
                command.Parameters.AddWithValue("@Nombre", docente.Nombre);
                command.Parameters.AddWithValue("@Apellido", docente.Apellido);
                command.Parameters.AddWithValue("@Ubicacion", docente.Ubicacion);
                command.Parameters.AddWithValue("@Sexo", docente.Sexo);
                command.Parameters.AddWithValue("@CI", docente.CI);
                command.Parameters.AddWithValue("@idUniversidad", docente.idUniversidad);

                // Obtener el ID generado
                docente.idDocente = Convert.ToInt32(command.ExecuteScalar());

                connection.Close();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
            return docente;
        }

        [HttpDelete("{idDocente}")]
        public IActionResult Delete(int idDocente)
        {
            string delete = "DELETE FROM public.\"Docente\" WHERE \"idDocente\" = @idDocente;";

            using NpgsqlConnection connection = new NpgsqlConnection(db);
            try
            {
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand(delete, connection);
                command.Parameters.AddWithValue("@idDocente", idDocente);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    return Ok("Docente eliminado");
                }
                else
                {
                    return NotFound("Docente no encontrado");
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return BadRequest("Error al eliminar el docente");
            }
        }
    }
}
