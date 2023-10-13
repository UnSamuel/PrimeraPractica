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
    public class EstudianteController : ControllerBase
    {
        private readonly string db = Coneccion.conexionConPostgres();

        [HttpGet]
        public List<Estudiante> Get()
        {
            List<Estudiante> estudiantes = new List<Estudiante>();
            string select = "SELECT * FROM public.\"Estudiante\"";

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
                        Estudiante estudiante = new Estudiante
                        {
                            idEstudiante = int.Parse(reader["idEstudiante"].ToString()),
                            Nombre = reader["Nombre"].ToString(),
                            Apellido = reader["Apellido"].ToString(),
                            Edad = int.Parse(reader["Edad"].ToString()),
                            Sexo = bool.Parse(reader["Sexo"].ToString()),
                            idUniversidad = int.Parse(reader["idUniversidad"].ToString())
                        };
                        estudiantes.Add(estudiante);
                    }
                }
                reader.Close();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
            return estudiantes;
        }

        [HttpPost]
        public Estudiante Create([FromBody] Estudiante estudiante)
        {
            string insert = "INSERT INTO public.\"Estudiante\"(\"Nombre\", \"Apellido\", \"Edad\", \"Sexo\", \"idUniversidad\")" +
                            "VALUES (@Nombre, @Apellido, @Edad, @Sexo, @idUniversidad) RETURNING \"idEstudiante\";";

            using NpgsqlConnection connection = new NpgsqlConnection(db);
            try
            {
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand(insert, connection);
                command.Parameters.AddWithValue("@Nombre", estudiante.Nombre);
                command.Parameters.AddWithValue("@Apellido", estudiante.Apellido);
                command.Parameters.AddWithValue("@Edad", estudiante.Edad);
                command.Parameters.AddWithValue("@Sexo", estudiante.Sexo);
                command.Parameters.AddWithValue("@idUniversidad", estudiante.idUniversidad);

                // Obtener el ID generado
                estudiante.idEstudiante = Convert.ToInt32(command.ExecuteScalar());

                connection.Close();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
            return estudiante;
        }

        [HttpDelete("{idEstudiante}")]
        public IActionResult Delete(int idEstudiante)
        {
            string delete = "DELETE FROM public.\"Estudiante\" WHERE \"idEstudiante\" = @idEstudiante;";

            using NpgsqlConnection connection = new NpgsqlConnection(db);
            try
            {
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand(delete, connection);
                command.Parameters.AddWithValue("@idEstudiante", idEstudiante);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    return Ok("Estudiante eliminado");
                }
                else
                {
                    return NotFound("Estudiante no encontrado");
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return BadRequest("Error al eliminar el estudiante");
            }
        }
    }
}
