using System;
using System.Collections.Generic;

namespace ClassLibrary
{
    public class AdministrarClientes
{
    // Singleton
    private static readonly AdministrarClientes _instancia = new AdministrarClientes();
    public static AdministrarClientes Instancia => _instancia;

    // Lista interna
    private readonly List<Cliente> _clientes = new List<Cliente>();

    // Constructor privado → evita que se creen instancias fuera
    private AdministrarClientes() 
    {
    }

    /// <summary>
    /// Este método se encarga de generar los comandos
    /// </summary>
    /// <param name="solicitante"></param>
    /// <param name="nombre"></param>
    /// <param name="apellido"></param>
    /// <param name="email"></param>
    /// <param name="telefono"></param>
    /// <param name="genero"></param>
    /// <param name="fechaNacimiento"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public Cliente CrearCliente(Usuario solicitante, string nombre, string apellido, string email, string telefono,
        string genero, DateTime fechaNacimiento)
    {
        if (solicitante.Suspendido)
            throw new InvalidOperationException("Usuario suspendido.");

        Cliente cliente = new Cliente(nombre, apellido, email, telefono, genero, fechaNacimiento, solicitante);
        
        _clientes.Add(cliente);
        solicitante.ClientesAsignados.Add(cliente);
        return cliente;
    }

    public void ModificarCliente(Usuario solicitante, Cliente cliente,
        string? nombre = null, string? apellido = null, string? email = null, string? telefono = null,
        string? genero = null, DateTime? fechaNacimiento = null)
    {
        if (solicitante.Suspendido)
            throw new InvalidOperationException("Usuario suspendido.");

        if (cliente.UsuarioAsignado != solicitante && solicitante.Rol != TipoRol.ADMINISTRADOR)
            throw new UnauthorizedAccessException("No puede modificar este cliente.");

        if (nombre != null) cliente.Nombre = nombre;
        if (apellido != null) cliente.Apellido = apellido;
        if (email != null) cliente.Email = email;
        if (telefono != null) cliente.Telefono = telefono;
        if (genero != null) cliente.Genero = genero;
        if (fechaNacimiento.HasValue) cliente.FechaNacimiento = fechaNacimiento;
    }

    public void EliminarCliente(Usuario solicitante, Cliente cliente)
    {
        if (solicitante.Suspendido)
            throw new InvalidOperationException("Usuario suspendido.");

        if (cliente.UsuarioAsignado != solicitante && solicitante.Rol != TipoRol.ADMINISTRADOR)
            throw new UnauthorizedAccessException("No puede eliminar este cliente.");

        _clientes.Remove(cliente);
        cliente.UsuarioAsignado?.ClientesAsignados.Remove(cliente);
    }

    public List<Cliente> BuscarClientes(Usuario solicitante, string criterio)
    {
        if (string.IsNullOrWhiteSpace(criterio))
            throw new ArgumentException("Debe indicar un criterio de búsqueda.");

        List<Cliente> clientesEncontrados = new List<Cliente>();

        foreach (Cliente c in solicitante.ClientesAsignados)
        {
            bool coincide = (c.Nombre.IndexOf(criterio, StringComparison.OrdinalIgnoreCase) >= 0) ||
                            (c.Apellido.IndexOf(criterio, StringComparison.OrdinalIgnoreCase) >= 0) ||
                            (c.Email.IndexOf(criterio, StringComparison.OrdinalIgnoreCase) >= 0) ||
                            (c.Telefono.IndexOf(criterio, StringComparison.OrdinalIgnoreCase) >= 0);

            // Revisar nombre, apellido, email y teléfono

            // Revisar etiquetas
            if (!coincide && c.Etiquetas.Count>0)
            {
                foreach (string etiqueta in c.Etiquetas)
                {
                    if (!string.IsNullOrEmpty(etiqueta) &&
                        etiqueta.IndexOf(criterio, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        coincide = true;
                        break; // No hace falta seguir revisando etiquetas
                    }
                }
            }

            if (coincide)
                clientesEncontrados.Add(c);
        }

        return clientesEncontrados;
    }
    
    public List<Cliente> BuscarClientesEnGlobal( string criterio)
    {
        if (string.IsNullOrWhiteSpace(criterio))
            throw new ArgumentException("Debe indicar un criterio de búsqueda.");

        List<Cliente> clientesEncontrados = new List<Cliente>();

        foreach (Cliente c in _clientes)
        {
            bool coincide = (c.Nombre.IndexOf(criterio, StringComparison.OrdinalIgnoreCase) >= 0) ||
                            (c.Apellido.IndexOf(criterio, StringComparison.OrdinalIgnoreCase) >= 0) ||
                            (c.Email.IndexOf(criterio, StringComparison.OrdinalIgnoreCase) >= 0) ||
                            (c.Telefono.IndexOf(criterio, StringComparison.OrdinalIgnoreCase) >= 0);

            // Revisar nombre, apellido, email y teléfono

            // Revisar etiquetas
            if (!coincide && c.Etiquetas.Count>0)
            {
                foreach (string etiqueta in c.Etiquetas)
                {
                    if (!string.IsNullOrEmpty(etiqueta) &&
                        etiqueta.IndexOf(criterio, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        coincide = true;
                        break; // No hace falta seguir revisando etiquetas
                    }
                }
            }

            if (coincide)
                clientesEncontrados.Add(c);
        }

        return clientesEncontrados;
    }

    public void AgregarEtiquetaCliente(Usuario solicitante, Cliente cliente, string etiqueta)
    {
        if (cliente.UsuarioAsignado != solicitante && solicitante.Rol != TipoRol.ADMINISTRADOR)
            throw new UnauthorizedAccessException("No puede agregar etiquetas a este cliente.");
        if (solicitante.Suspendido)
            throw new InvalidOperationException("Usuario suspendido.");
        if (!cliente.Etiquetas.Contains(etiqueta))
            cliente.Etiquetas.Add(etiqueta);
    }

    public List<Cliente> VerClientesPropios(Usuario usuario)
    {
        return usuario.ClientesAsignados;
    }

    public void AsignarClienteAOtroVendedor(Vendedor vendedor, Cliente cliente, Vendedor vendedorNuevo)
    {
        vendedor.ClientesAsignados.Remove(cliente);
        vendedorNuevo.ClientesAsignados.Add(cliente);
        cliente.UsuarioAsignado = vendedorNuevo;
    }

    public List<Cliente> BuscarClientesSinInteraccionDesde(Usuario usuario, DateTime fecha)
    {
        List<Cliente> clientesInactivos = new List<Cliente>();

        foreach (Cliente cliente in usuario.ClientesAsignados)
        {
            // Caso 1: nunca tuvo interacciones
            if (cliente.ListaInteracciones.Count == 0)
            {
                clientesInactivos.Add(cliente);
                continue;
            }

            // Caso 2: obtener la última interacción con el usuario
            DateTime ultimaFecha = DateTime.MinValue;

            foreach (Interaccion interaccion in cliente.ListaInteracciones)
            {
                // Solo contar interacciones entre usuario <-> cliente
                bool participa = (interaccion.Emisor == usuario || interaccion.Receptor == usuario);

                if (participa && interaccion.Fecha > ultimaFecha)
                {
                    ultimaFecha = interaccion.Fecha;
                }
            }

            // Si no hubo interacciones reales del usuario → se considera inactivo
            if (ultimaFecha == DateTime.MinValue)
            {
                clientesInactivos.Add(cliente);
            }
            else if (ultimaFecha < fecha)
            {
                clientesInactivos.Add(cliente);
            }
        }

        return clientesInactivos;
    }


    public void LimpiarParaTest()
    {
        _clientes.Clear();
    }

    public List<Cliente> Clientes()
    {
        return _clientes;
    }
}
}