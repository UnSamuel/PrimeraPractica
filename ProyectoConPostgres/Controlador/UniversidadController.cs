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
    public class UniversidadController : ControllerBase
    {
        private readonly string db = Coneccion.conexionConPostgres();

        [HttpGet]
        public List<Universidad> Get()
        {
            List<Universidad> universidades = new List<Universidad>();
            string select = "SELECT * FROM public.\"Universidad\"";

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
                        Universidad universidad = new Universidad
                        {
                            iduniversidad = int.Parse(reader["iduniversidad"].ToString()),
                            Nombre = reader["Nombre"].ToString()
                        };
                        universidades.Add(universidad);
                    }
                }
                reader.Close();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
            return universidades;
        }

        [HttpPost]
        public Universidad Create([FromBody] Universidad universidad)
        {
            string insert = "INSERT INTO public.\"Universidad\"(\"Nombre\") VALUES (@Nombre) RETURNING \"iduniversidad\";";

            using NpgsqlConnection connection = new NpgsqlConnection(db);
            try
            {
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand(insert, connection);
                command.Parameters.AddWithValue("@Nombre", universidad.Nombre);

                // Obtener el ID generado
                universidad.iduniversidad = Convert.ToInt32(command.ExecuteScalar());

                connection.Close();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
            return universidad;
        }

        [HttpDelete("{iduniversidad}")]
        public IActionResult Delete(int iduniversidad)
        {
            string delete = "DELETE FROM public.\"Universidad\" WHERE \"iduniversidad\" = @iduniversidad;";

            using NpgsqlConnection connection = new NpgsqlConnection(db);
            try
            {
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand(delete, connection);
                command.Parameters.AddWithValue("@iduniversidad", iduniversidad);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    return Ok("Universidad eliminada");
                }
                else
                {
                    return NotFound("Universidad no encontrada");
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return BadRequest("Error al eliminar la universidad");
            }
        }
    }
}
