using System;
using System.Collections.Generic;

namespace ClassLibrary;

public class Venta
{
    public Dictionary<Producto, int> Productos { get; private set; }
    public DateTime Fecha { get; private set; }
    public double Total => CalcularTotal();
    public Cliente ClienteComprador { get; private set; }
    public Usuario UsuarioVendedor { get; private set; }

    public Venta(Cliente cliente, Usuario vendedor, DateTime fecha)
    {
        ClienteComprador = cliente;
        UsuarioVendedor = vendedor;
        Fecha = fecha;
        Productos = new Dictionary<Producto, int>();

        cliente.AgregarVenta(this);
        vendedor.AgregarVenta(this);
    }

    public void AgregarProducto(Producto producto, int cantidad)
    {
        if (Productos.ContainsKey(producto))
            Productos[producto] += cantidad;
        else
            Productos.Add(producto, cantidad);
    }

    private double CalcularTotal()
    {
        double total = 0;
        foreach (KeyValuePair<Producto, int> par in Productos)
        {
            total += par.Key.Precio * par.Value;
        }
        return total;
    }
}