using System.Collections.Generic;

namespace ClassLibrary
{
    public class Usuario : Persona
    {
        public bool Suspendido { get; set; }
        public List<Cliente> ClientesAsignados { get; set; }
        
        public string Contraseña { get; private set; }


        public TipoRol Rol { get; set; }

        //Primer constructor: creoado antes de definir que necesitaba una contraseña. 
        //Permite que los tests unitarios no se rompan y evita que tenga que modificar todos los usuarios
        public Usuario(string nombre, string apellido, string email, string telefono, TipoRol rol) :
            base(nombre, apellido, email, telefono)
        {
            Rol = rol;
            ClientesAsignados = new List<Cliente>();
            Contraseña = "tempo123";
        }
        
        //Segundo constructos: permite que los usuarios puedan ingresar contraseñas propias.
        public Usuario(string nombre, string apellido, string email, string telefono,string contraseña, TipoRol rol )
            : base(nombre, apellido, email, telefono)
        {
            Rol = rol;
            ClientesAsignados = new List<Cliente>();
            Contraseña = contraseña;
        }

        
    
    }
}