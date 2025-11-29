namespace ClassLibrary;

public class Vendedor : Usuario
{
    public Vendedor(string nombre, string apellido, string email, string telefono)
        : base(nombre, apellido, email, telefono, TipoRol.VENDEDOR)
    {
    }

    // Método específico de vendedor
    public void CambiarVendedorAsignado(Cliente cliente, Vendedor vendedorNuevo)
    {
        // eliminar del vendedor actual
        this.ClientesAsignados.Remove(cliente);
        // agregar al nuevo vendedor
        vendedorNuevo.ClientesAsignados.Add(cliente);
        // actualizar referencia del cliente
        cliente.UsuarioAsignado = vendedorNuevo;
    }
}