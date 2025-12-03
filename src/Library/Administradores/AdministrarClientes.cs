using System;
using System.Collections.Generic;
using System.Text;

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
    
    /// <summary>
    /// Permite buscar a los clientes en la lista global que tiene esta clase
    /// </summary>
    /// <param name="criterio"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
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

    /// <summary>
    /// Permite agregar una etiqueta al cliente
    /// </summary>
    /// <param name="solicitante"></param>
    /// <param name="cliente"></param>
    /// <param name="etiqueta"></param>
    /// <exception cref="UnauthorizedAccessException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public void AgregarEtiquetaCliente(Usuario solicitante, Cliente cliente, string etiqueta)
    {
        if (cliente.UsuarioAsignado != solicitante && solicitante.Rol != TipoRol.ADMINISTRADOR)
            throw new UnauthorizedAccessException("No puede agregar etiquetas a este cliente.");
        if (solicitante.Suspendido)
            throw new InvalidOperationException("Usuario suspendido.");
        if (!cliente.Etiquetas.Contains(etiqueta))
            cliente.Etiquetas.Add(etiqueta);
    }

    /// <summary>
    /// Devuelve la lista de los clientes que tiene un usuario
    /// </summary>
    /// <param name="usuario"></param>
    /// <returns></returns>
    public List<Cliente> VerClientesPropios(Usuario usuario)
    {
        return usuario.ClientesAsignados;
    }

    /// <summary>
    /// Como dice su nombre, asigna un cliente a otro vendedor
    /// </summary>
    /// <param name="vendedor"></param>
    /// <param name="cliente"></param>
    /// <param name="vendedorNuevo"></param>
    public void AsignarClienteAOtroVendedor(Vendedor vendedor, Cliente cliente, Vendedor vendedorNuevo)
    {
        vendedor.ClientesAsignados.Remove(cliente);
        vendedorNuevo.ClientesAsignados.Add(cliente);
        cliente.UsuarioAsignado = vendedorNuevo;
    }

    /// <summary>
    /// Aquí busca los clientes que no se han tenido interacciones desde cierta fecha. Los clientes con los que no se haya
    /// interacctado después de esa fecha son los que el método devuelve
    /// </summary>
    /// <param name="usuario"></param>
    /// <param name="fecha"></param>
    /// <returns></returns>
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
    
    //-----------------------------
    //1) Comando que retorne los clientes con ventas mayores o menores a cierto monto o dentro de un cierto rango de montos
    //-----------------------------
    
    public List<Cliente> ClientesConVentasMayoresOMenoresAMonto(Usuario usuario, string monto, string sign)
    {
        List<Cliente> clientesEncontrados = new List<Cliente>();

        //transforma el monto dado a double (el comando del bot verifica que sea efectivamente un número que 
        //pueda ser pasado a double o le informa al usuario, evitando qeu se ejecute el método innecesariamente
        double montoAComparar = double.Parse(monto);

        //Añade al resultado los clientes del usuario que en su lista de ventas tengan una venta con total
        //mayor al monto dado
        if (sign == ">")
        {
            foreach (Cliente cliente in usuario.ClientesAsignados)
            {
                foreach (Venta venta in cliente.ListaVentas)
                {
                    if (venta.Total > montoAComparar)
                    {
                        clientesEncontrados.Add(cliente);
                        break;
                    }
                       
                }
            }
        }
        
        //Añade al resultado los clientes del usuario que en su lista de ventas tengan una venta con total
        //menor al monto dado
        if (sign == "<")
        {
            foreach (Cliente cliente in usuario.ClientesAsignados)
            {
                foreach (Venta venta in cliente.ListaVentas)
                {
                    if (venta.Total < montoAComparar)
                    {
                        clientesEncontrados.Add(cliente);
                        break;
                    }
                        
                }
            }
        }
        
        return clientesEncontrados;
    }

    public Dictionary<Cliente, string> ClientesConMontoEntreRango(Usuario usuario, string monto1, string monto2)
    {
        //Crea la lista de clientes a enviar
        List<Cliente> clientesEncontrados = new List<Cliente>();
        
        //Se almacenan las ventas de los clientes que estén dentro del rango dado
        List<Venta> ventasDentroDelRango = new List<Venta>();
        
        /*Pasa los montos a double, porque el total de las ventas fue definida en double y
         no se generan errores
        */
        
        double monto1double = double.Parse(monto1);

        double monto2dluble = double.Parse(monto2);
        
        //En cada venta que tiene almacenado el usuario que ha realizado él, revisa si tiene ventas mayores
        //al primer monto dado o menores al segundo monto dado
        foreach (Venta venta in usuario.ListaVentas)
        {
            if (venta.Total >= monto1double && venta.Total <= monto2dluble)
            {
                //Si el cliente no está en la lista de clientes encontrados, se le añad
                if (!clientesEncontrados.Contains(venta.ClienteComprador))
                {
                    clientesEncontrados.Add(venta.ClienteComprador);
                }
                
                ventasDentroDelRango.Add(venta);
            }
        }
        //Stringbuilder para construir el mensaje a envíar
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("Clientes encontrados:");
        
        //Lo que devolverá el método
        Dictionary<Cliente, string> resultado = new Dictionary<Cliente, string>();

        
        //Revisa en cada cliente encontrado 
        foreach (Cliente cliente in clientesEncontrados)
        {
            sb.AppendLine($"Cliente {cliente.Nombre} {cliente.Apellido}");
            sb.AppendLine($"-- VENTAS DEL CLIENTE --");
            if (!resultado.Keys.Contains(cliente))
            {
                //Revisa entre las ventas encontradas
                foreach (Venta venta in ventasDentroDelRango)
                {
                    //Si el cliente que tiene la venta coincide con el cliente actual que se está evaluando
                    //enconces registrará la venta como parte de la venta del cliente.
                    if (venta.ClienteComprador == cliente)
                    {
                        foreach (var producto in venta.Productos)
                        {
                            sb.AppendLine("----------------------------");

                            sb.AppendLine($"Producto:{producto.Key.Nombre}");
                            sb.AppendLine($"Precio: ${producto.Key.Precio}");
                            sb.AppendLine($"Cantidad: {producto.Value}");
                            sb.AppendLine($"Subtotal: {producto.Key.Precio * producto.Value}");
                        }

                        sb.AppendLine($"Total: {venta.Total}");

                    }

                    sb.AppendLine("----------------------------");

                }

                string datosCliente = sb.ToString();

                //Añade el cliente y los datos al diccionario
                resultado.Add(cliente, datosCliente);
            }
        }
        
        //Devuelve el diciconario con todos los datos.
        return resultado;


    }
    //-----------------------------
    //2) Comando que retorne los clientes con ventas de cierto producto o servicio
    //-----------------------------

    public List<Cliente> ClientesConVentaOServicio(Usuario usuario, string productoOServicio)
    {
        //Lista que devolverá el método
        List<Cliente> resultado = new List<Cliente>();

        //Se crea una lista auxiliar para encontrar todas las ventas que tengan ese producto.
        List<Venta> ventasConElProducto = new List<Venta>();

        foreach (Venta venta in usuario.ListaVentas)
        {
            foreach (var producto in venta.Productos)
            {
                //Si el nombre del producto coincide con el nombre dado, entonces se almacena la venta en las ventas encontradas
                if (productoOServicio.Equals(producto.Key.Nombre, StringComparison.OrdinalIgnoreCase))
                {
                    ventasConElProducto.Add(venta);
                    break;
                }
            }
        }
        //Revisamos cada venta, si el cliente no está, se agrega a la lista de resultados
        foreach (Venta venta in ventasConElProducto)
        {
            if (!resultado.Contains(venta.ClienteComprador))
            {
                resultado.Add(venta.ClienteComprador);
            }
        }
        
        //retorna la lista con los clientes encontrados
        return resultado;
    }
    
    

}
}