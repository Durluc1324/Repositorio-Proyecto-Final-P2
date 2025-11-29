using System.Collections.Generic;

namespace ClassLibrary
{
    public abstract class Persona
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public List<Interaccion> ListaInteracciones { get; set; }
        public List<Cotizacion> ListaCotizaciones { get; set; }
        public List<Venta> ListaVentas { get; set; }

        public Persona(string nombre, string apellido, string email, string telefono)
        {
            this.Nombre = nombre;
            this.Apellido = apellido;
            this.Email = email;
            this.Telefono = telefono;
            this.ListaInteracciones = new List<Interaccion>();
            this.ListaCotizaciones = new List<Cotizacion>();
            this.ListaVentas = new List<Venta>();
        }
        public void AgregarCotizacion(Cotizacion cotizacion)
        {
            ListaCotizaciones.Add(cotizacion);
        }

        public void AgregarVenta(Venta venta)
        {
            ListaVentas.Add(venta);
        }

        public void AgregarInteraccion(Interaccion interaccion)
        {
            ListaInteracciones.Add(interaccion);
        }

        public void EliminarCotizacion(Cotizacion cotizacion)
        {
            ListaCotizaciones.Remove(cotizacion);
        }

        public void EliminarVenta(Venta venta)
        {
            ListaVentas.Remove(venta);
        }

        public void EliminarInteraccion(Interaccion interaccion)
        {
            ListaInteracciones.Remove(interaccion);
        }
    }
}