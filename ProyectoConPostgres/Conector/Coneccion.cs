using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace ProyectoConPostgres.Conector
{
    public class Coneccion
    {
        public static string conexionConPostgres()
        {
            Console.WriteLine("Creando la conexion con Postgres");
            var builder = new NpgsqlConnectionStringBuilder();
            builder.Host= "localhost";
            builder.Username= "postgres";
            builder.Password= "12345678";
            builder.Database= "bd_universidad";
            Console.WriteLine(builder.ConnectionString);
            return builder.ConnectionString;
            
        }
    }
}
