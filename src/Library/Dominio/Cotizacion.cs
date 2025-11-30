using System;
using System.Collections.Generic;
using System.Linq;

namespace ClassLibrary
{
    public class Cotizacion
    {
        public Usuario UsuarioCreador { get; private set; }
        public Cliente ClienteDestino { get; private set; }
        public DateTime Fecha { get; private set; }
        public DateTime FechaLimite { get; private set; }
        public string Descripcion { get; private set; }

        private List<Producto> _productos;
        public IReadOnlyList<Producto> Productos => _productos.AsReadOnly();

        public double Total => CalcularTotal();

        public Cotizacion(Usuario creador, Cliente destino,
            DateTime fecha, DateTime fechaLimite, string descripcion)
        {
            UsuarioCreador = creador;
            ClienteDestino = destino;
            Fecha = fecha;
            FechaLimite = fechaLimite;
            Descripcion = descripcion;

            _productos = new List<Producto>();

            destino.AgregarCotizacion(this);
            creador.AgregarCotizacion(this);
        }

        public void AgregarProducto(Producto producto)
        {
            if (producto == null) throw new ArgumentNullException(nameof(producto));
            _productos.Add(producto);
        }

        public void RemoverProducto(Producto producto)
        {
            _productos.Remove(producto);
        }

        private double CalcularTotal()
        {
            return _productos.Sum(p => p.Precio);
        }
    }
}