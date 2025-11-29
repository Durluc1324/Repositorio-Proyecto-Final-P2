using System;
using System.Collections.Generic;

namespace ClassLibrary
{
    public class Cliente : Persona
    {
        public DateTime? FechaNacimiento { get; set; }
        public string Genero { get; set; }
        public List<string> Etiquetas { get; set; }
        public Usuario UsuarioAsignado { get; set; }
    
        public Cliente(string nombre, string apellido, string email, string telefono,string genero, DateTime fechanacimiento, Usuario usuarioAsignado):base(nombre, apellido,email,telefono)
        {
            Genero = genero;
            FechaNacimiento = fechanacimiento;
            UsuarioAsignado = usuarioAsignado;
            Etiquetas = new List<string>();
        }

    }
}