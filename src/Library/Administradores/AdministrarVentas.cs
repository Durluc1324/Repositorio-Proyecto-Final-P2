using System;
using System.Collections.Generic;
using ClassLibrary.Excepciones;

namespace ClassLibrary
{
    public class AdministrarVentas
{
    // Singleton 
    private static readonly AdministrarVentas _instancia = new AdministrarVentas();
    public static AdministrarVentas Instancia => _instancia;
    private readonly List<Venta> _ventas = new List<Venta>();

    // Constructor privado
    private AdministrarVentas()
    {
    }
    

    /// <summary>
    /// Crea una instancia de una venta
    /// </summary>
    /// <param name="usuario"></param>
    /// <param name="cliente"></param>
    /// <param name="fecha"></param>
    /// <returns></returns>
    public Venta CrearVenta(Usuario usuario, Cliente cliente, DateTime fecha)
    {
            ArgumentNullException.ThrowIfNull(usuario);
            ArgumentNullException.ThrowIfNull(cliente);

            Venta nueva = new Venta(cliente, usuario, fecha);
        _ventas.Add(nueva);
        return nueva;
    }
    
    /// <summary>
    /// Crea un objeto producto y lo agrega a la venta dada.
    /// </summary>
    /// <param name="venta"></param>
    /// <param name="nombre"></param>
    /// <param name="precio"></param>
    /// <param name="cantidad"></param>
    /// <exception cref="ArgumentException"></exception>
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
    
   
    
    /// <summary>
    /// Se encarga de obtener las ventas de un usuario que estén en medio entre la fecha de inicio y final dadas
    /// </summary>
    /// <param name="usuario"></param>
    /// <param name="inicio"></param>
    /// <param name="fin"></param>
    /// <returns></returns>
    /// <exception cref="UsuarioNuloException"></exception>
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