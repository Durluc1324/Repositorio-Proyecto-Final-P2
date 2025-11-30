using System.Collections.Generic;

namespace ClassLibrary
{
    public class Usuario : Persona
    {
        public bool Suspendido { get; set; }
        public List<Cliente> ClientesAsignados { get; set; }


        public TipoRol Rol { get; set; }

        public Usuario(string nombre, string apellido, string email, string telefono, TipoRol rol) :
            base(nombre, apellido, email, telefono)
        {
            Rol = rol;
            ClientesAsignados = new List<Cliente>();
        }

        
    
    }
}