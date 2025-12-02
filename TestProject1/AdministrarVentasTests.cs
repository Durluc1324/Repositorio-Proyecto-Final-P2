using ClassLibrary;
using ClassLibrary.Excepciones;

namespace TestProject1
{
    [TestClass]
    public class AdministrarVentasTests
    {
        private Usuario vendedor;
        private Cliente cliente;
        private AdministrarVentas adminVentas;

        [TestInitialize]
        public void Init()
        {
            adminVentas = AdministrarVentas.Instancia;
            adminVentas.LimpiarParaTest();

            vendedor = new Vendedor("Juan", "Pérez", "jp@mail.com", "099123123");
            cliente = new Cliente("Carlos", "Gómez", "cg@mail.com",  "099999999", "Hombre", new DateTime(2000,6,21), vendedor);
        }

        // -------------------------------
        // CREAR VENTA
        // -------------------------------
        [TestMethod]
        public void CrearVenta_DeberiaAgregarVentaALaLista()
        {
            DateTime fecha = DateTime.Now;

            Venta venta = adminVentas.CrearVenta(vendedor, cliente, fecha);

            Assert.IsNotNull(venta);
            Assert.AreEqual(1, adminVentas.Ventas().Count);
            Assert.AreEqual(cliente, venta.ClienteComprador);
        }

      

        // -------------------------------
        // AGREGAR PRODUCTO
        // -------------------------------
        [TestMethod]
        public void AgregarProducto_DeberiaAgregarProductoALaVenta()
        {
            var venta = adminVentas.CrearVenta(vendedor, cliente, DateTime.Now);

            adminVentas.AgregarProducto(venta, "Laptop", 1500, 1);

            Assert.AreEqual(1, venta.Productos.Count);

            var prod = venta.Productos.Keys.First(); // obtenemos el producto agregado

            Assert.AreEqual("Laptop", prod.Nombre);
            Assert.AreEqual(1500, prod.Precio);

            Assert.AreEqual(1, venta.Productos[prod]); // cantidad
        }

        // -------------------------------
        // BUSQUEDA ADMIN
        // -------------------------------
        [TestMethod]
        public void BuscarVentasComoAdmin_FiltraPorNombreCliente()
        {
            var venta = adminVentas.CrearVenta(vendedor, cliente, DateTime.Now);

            var resultados = adminVentas.BuscarVentasComoAdmin("carl", null, null, null);

            Assert.AreEqual(1, resultados.Count);
            Assert.AreEqual(venta, resultados[0]);
        }
    }
}