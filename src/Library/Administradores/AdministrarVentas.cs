using System;
using System.Collections.Generic;
using ClassLibrary.Excepciones;

namespace ClassLibrary
{
    public class AdministrarVentas
{
    // Singleton (nombre consistente con el resto)
    private static readonly AdministrarVentas _instancia = new AdministrarVentas();
    public static AdministrarVentas Instancia => _instancia;
    private readonly List<Venta> _ventas = new List<Venta>();

    // Constructor privado
    private AdministrarVentas()
    {
    }
    

    public Venta CrearVenta(Usuario usuario, Cliente cliente, DateTime fecha)
    {
            ArgumentNullException.ThrowIfNull(usuario);
            ArgumentNullException.ThrowIfNull(cliente);

            Venta nueva = new Venta(cliente, usuario, fecha);
        _ventas.Add(nueva);
        return nueva;
    }
    
    public List<Venta> BuscarTodasPorRango(Usuario usuario, DateTime inicio, DateTime fin)
    {
        List<Venta> resultados = new List<Venta>();

        foreach (Venta venta in usuario.ListaVentas)
        {
            if (venta.Fecha >= inicio && venta.Fecha <= fin)
            {
                resultados.Add(venta);
            }
        }

        return resultados;
    }
    
    public List<Venta> BuscarVentasComoAdmin(
        string criterio,
        DateTime? fecha,
        Cliente? cliente,
        Usuario? vendedor)
    {
        if (string.IsNullOrWhiteSpace(criterio) &&
            !fecha.HasValue &&
            cliente == null &&
            vendedor == null)
        {
            throw new InvalidOperationException("No se introdujo ningún criterio de búsqueda.");
        }
        
        List<Venta> resultados = new List<Venta>();
        criterio = criterio?.ToLower();

        
        foreach (var venta in _ventas)
        {
            bool coincide = true;

            if (!string.IsNullOrWhiteSpace(criterio))
            {
                string c = criterio.ToLower();
                coincide &= 
                    venta.ClienteComprador.Nombre.Contains(c, StringComparison.CurrentCultureIgnoreCase) ||
                    venta.ClienteComprador.Apellido.Contains(c, StringComparison.CurrentCultureIgnoreCase) ||
                    venta.UsuarioVendedor.Nombre.Contains(c, StringComparison.CurrentCultureIgnoreCase);
            }

            if (fecha.HasValue)
                coincide &= venta.Fecha.Date == fecha.Value.Date;

            if (cliente != null)
                coincide &= venta.ClienteComprador == cliente;

            if (vendedor != null)
                coincide &= venta.UsuarioVendedor == vendedor;

            if (coincide)
                resultados.Add(venta);
        }
        
        return resultados;
    }

    public List<Venta> BuscarVentaComoUsuario(Usuario usuario, string criterio, DateTime? fecha)
    {
        List<Venta> resultados = new List<Venta>();
        string c = criterio?.ToLower();

        // 1. Ventas propias
        foreach (var venta in usuario.ListaVentas)
        {
            if (CoincideUsuario(venta, c, fecha))
                resultados.Add(venta);
        }

        // 2. Ventas de clientes asignados
        foreach (Cliente cliente in usuario.ClientesAsignados)
        {
            foreach (Venta venta in cliente.ListaVentas)
            {
                if (CoincideUsuario(venta, c, fecha))
                    resultados.Add(venta);
            }
        }

        return resultados;
    }

    private bool CoincideUsuario(Venta venta, string criterio, DateTime? fecha)
    {
        bool coincide = true;

        if (!string.IsNullOrWhiteSpace(criterio))
        {
            criterio = criterio.ToLower();

            bool coincideTexto =
                venta.ClienteComprador.Nombre.Contains(criterio, StringComparison.CurrentCultureIgnoreCase) ||
                venta.ClienteComprador.Apellido.Contains(criterio, StringComparison.CurrentCultureIgnoreCase) ||
                venta.ClienteComprador.Genero.Contains(criterio, StringComparison.CurrentCultureIgnoreCase) ||
                venta.ClienteComprador.Email.Contains(criterio, StringComparison.CurrentCultureIgnoreCase) ||
                venta.ClienteComprador.Telefono.Contains(criterio, StringComparison.CurrentCultureIgnoreCase);

            coincide &= coincideTexto;
        }

        if (fecha.HasValue)
            coincide &= venta.Fecha.Date == fecha.Value.Date;

        return coincide;
    }

    public void AgregarProducto(Venta venta, string nombre, double precio, int cantidad)
    {
            ArgumentNullException.ThrowIfNull(venta);
            

            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("Nombre del producto inválido.");

            if (precio <= 0)
                throw new ArgumentException("El precio debe ser mayor que cero.");

            if (cantidad <= 0)
                throw new ArgumentException("La cantidad debe ser mayor que cero.");

            Producto producto = new Producto(nombre, precio);
            venta.AgregarProducto(producto, cantidad);
    }
    
   
    
    public List<Venta> ObtenerVentasPeriodo(Usuario usuario, DateTime inicio, DateTime fin)
    {
        if (usuario == null)
            throw new UsuarioNuloException();

        List<Venta> ventasEnPeriodo = new List<Venta>();

        foreach (var venta in usuario.ListaVentas)
        {
            if (venta.Fecha >= inicio && venta.Fecha <= fin)
            {
                ventasEnPeriodo.Add(venta);
            }
        }

        return ventasEnPeriodo;
    }


    
    public void LimpiarParaTest()
    {
        _ventas.Clear();
    }

    public List<Venta> Ventas()
    {
        return _ventas;
    }
    
}
}