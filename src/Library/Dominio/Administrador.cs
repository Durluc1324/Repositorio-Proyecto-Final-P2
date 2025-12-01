namespace ClassLibrary
{
    public class Administrador : Usuario
    {
        public Administrador(string nombre, string apellido, string email, string telefono)
            : base(nombre, apellido, email, telefono, TipoRol.ADMINISTRADOR)
        {
        }

        public Administrador(string nombre, string apellido, string email, string telefono, string contraseña) :
            base(nombre, apellido, email, telefono, contraseña,TipoRol.ADMINISTRADOR)
        {
            
        }
    
    }
}