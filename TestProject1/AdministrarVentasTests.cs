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
            Assert.AreEqual(1, adminVentas.ObtenerVentas().Count);
            Assert.AreEqual(cliente, venta.ClienteComprador);
        }

        // -------------------------------
        // BUSCAR POR CLIENTE
        // -------------------------------
        [TestMethod]
        public void BuscarTodasPorCliente_DeberiaRetornarCorrectas()
        {
            var v1 = adminVentas.CrearVenta(vendedor, cliente, DateTime.Now);
            var v2 = adminVentas.CrearVenta(vendedor, cliente, DateTime.Now);
            var otroCliente = new Cliente("Ana", "Lopez", "alopez@mail.com", "099222222", "Mujer", new DateTime(1987,4,16), vendedor);
            adminVentas.CrearVenta(vendedor, otroCliente, DateTime.Now);

            var resultados = adminVentas.BuscarTodasPorCliente(cliente);

            Assert.AreEqual(2, resultados.Count);
            Assert.IsTrue(resultados.Contains(v1));
            Assert.IsTrue(resultados.Contains(v2));
        }

        // -------------------------------
        // BUSCAR POR USUARIO
        // -------------------------------
        [TestMethod]
        public void BuscarTodasPorUsuario_DeberiaRetornarCorrectas()
        {
            var vendedor2 = new Vendedor("Mario", "Suarez", "m@mail.com", "094444444");

            var v1 = adminVentas.CrearVenta(vendedor, cliente, DateTime.Now);
            var v2 = adminVentas.CrearVenta(vendedor2, cliente, DateTime.Now);

            var resultados = adminVentas.BuscarTodasPorUsuario(vendedor);

            Assert.AreEqual(1, resultados.Count);
            Assert.AreEqual(v1, resultados[0]);
        }

        // -------------------------------
        // BUSCAR POR FECHA
        // -------------------------------
        [TestMethod]
        public void BuscarTodasPorFecha_DeberiaFiltrarCorrectamente()
        {
            DateTime hoy = DateTime.Today;
            DateTime ayer = hoy.AddDays(-1);

            var v1 = adminVentas.CrearVenta(vendedor, cliente, hoy);
            adminVentas.CrearVenta(vendedor, cliente, ayer);

            var resultados = adminVentas.BuscarTodasPorFecha(hoy);

            Assert.AreEqual(1, resultados.Count);
            Assert.AreEqual(v1, resultados[0]);
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
        // CERRAR VENTA
        // -------------------------------
        [TestMethod]
        public void CerrarVenta_DeberiaCambiarEstado()
        {
            var venta = adminVentas.CrearVenta(vendedor, cliente, DateTime.Now);

            adminVentas.CerrarVenta(venta);

            Assert.IsTrue(venta.VentaCerrada);
        }

        // -------------------------------
        // TOTAL POR PERÍODO
        // -------------------------------
        [TestMethod]
        public void TotalVentasPeriodo_DeberiaSumarCorrectamente()
        {
            DateTime hoy = DateTime.Today;

            var v1 = adminVentas.CrearVenta(vendedor, cliente, hoy);
            adminVentas.AgregarProducto(v1, "Mouse", 500, 1);

            var v2 = adminVentas.CrearVenta(vendedor, cliente, hoy.AddHours(1));
            adminVentas.AgregarProducto(v2, "Teclado", 1500, 1);

            double total = adminVentas.ObtenerTotalVentasPeriodo(
                vendedor,
                hoy.AddHours(-1),
                hoy.AddHours(2)
            );

            Assert.AreEqual(2000, total);
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