namespace ClassLibrary;

public class Vendedor : Usuario
{
    public Vendedor(string nombre, string apellido, string email, string telefono)
        : base(nombre, apellido, email, telefono, TipoRol.VENDEDOR)
    {
    }

    public Vendedor(string nombre, string apellido, string email, string telefono, string contraseña)
        : base(nombre, apellido, email, telefono, contraseña, TipoRol.VENDEDOR)
    {
        
    }
    
}