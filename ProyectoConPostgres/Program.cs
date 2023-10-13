using System;
using System.Collections.Generic;
using ProyectoConPostgres.Controlador;
using ProyectoConPostgres.Modelo;

namespace ProyectoConPostgres
{
    public class Program
    {
        public static void Main()
        {
            EstudianteController estudianteController = new EstudianteController();
            UniversidadController universidadController = new UniversidadController();

            // Crear una universidad
            Universidad universidad = new Universidad
            {
                Nombre = "Universidad Ejemplo"
            };
            universidadController.Create(universidad);

            // Crear un estudiante
            Estudiantes estudiante = new Estudiantes
            {
                idEstudiante = 4,
                Nombre = "Samuel",
                Edad = 19,
                Ubicacion = "Bolivia",
                idUniversidad = universidad.iduniversidad 
            };
            estudianteController.Create(estudiante);


            estudianteController.Delete(1);


            List<Estudiante> estudianteList = estudianteController.Get();

            foreach (var est in estudianteList)
            {
                Console.WriteLine("El código es: " + est.idEstudiante);
                Console.WriteLine("El nombre es: " + est.Nombre);
                Console.WriteLine("La edad es: " + est.Edad);
                Console.WriteLine("La ubicación es: " + est.Ubicacion);
                Console.WriteLine("ID de la universidad: " + est.idUniversidad);
            }
        }
    }
}
