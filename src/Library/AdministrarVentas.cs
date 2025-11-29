using System;
using System.Collections.Generic;

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
        if (usuario == null) throw new ArgumentNullException(nameof(usuario));
        if (cliente == null) throw new ArgumentNullException(nameof(cliente));

        Venta nueva = new Venta(cliente, usuario, fecha);
        _ventas.Add(nueva);
        return nueva;
    }

    public List<Venta> ObtenerVentas()
    {
        return new List<Venta>(_ventas);
    }
    
    public List<Venta> BuscarTodasPorCliente(Cliente cliente)
    {
        List<Venta> resultados = new List<Venta>();

        foreach (Venta v in _ventas)
        {
            if (v.ClienteComprador == cliente)
            {
                resultados.Add(v);
            }
        }

        return resultados;
    }
    
    public List<Venta> BuscarTodasPorUsuario(Usuario vendedor)
    {
        List<Venta> resultados = new List<Venta>();

        foreach (Venta v in _ventas)
        {
            if (v.UsuarioVendedor == vendedor)
            {
                resultados.Add(v);
            }
        }

        return resultados;
    }
    
    public List<Venta> BuscarTodasPorFecha(DateTime fecha)
    {
        List<Venta> resultados = new List<Venta>();

        foreach (Venta v in _ventas)
        {
            if (v.Fecha.Date == fecha.Date)
            {
                resultados.Add(v);
            }
        }

        return resultados;
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
                    venta.ClienteComprador.Nombre.ToLower().Contains(c) ||
                    venta.ClienteComprador.Apellido.ToLower().Contains(c) ||
                    venta.UsuarioVendedor.Nombre.ToLower().Contains(c);
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
                venta.ClienteComprador.Nombre.ToLower().Contains(criterio) ||
                venta.ClienteComprador.Apellido.ToLower().Contains(criterio) ||
                venta.ClienteComprador.Genero.ToLower().Contains(criterio) ||
                venta.ClienteComprador.Email.ToLower().Contains(criterio) ||
                venta.ClienteComprador.Telefono.ToLower().Contains(criterio);

            coincide &= coincideTexto;
        }

        if (fecha.HasValue)
            coincide &= venta.Fecha.Date == fecha.Value.Date;

        return coincide;
    }

    
}
}