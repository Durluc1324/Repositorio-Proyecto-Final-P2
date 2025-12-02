using ClassLibrary;
namespace TestProject1;

[TestClass]
public class AdministrarCotizacionesTests
{
    private Administrador admin;
    private Vendedor vendedor;
    private Cliente cliente;

    [TestInitialize]
    public void Setup()
    {
        AdministrarCotizaciones.Instancia.LimpiarParaTest();
        AdministrarUsuarios.Instancia.LimpiarParaTest();
        AdministrarClientes.Instancia.LimpiarParaTest();

        admin = new Administrador("Admin", "Test", "admin@test.com", "099111222");
        vendedor = new Vendedor("V", "Uno", "v1@test.com", "091000001");

        // Crear cliente para las cotizaciones
        cliente = AdministrarClientes.Instancia.CrearCliente(admin, "Cote", "Uno", "c1@test.com", "0911", "X", DateTime.Now);
    }

    [TestCleanup]
    public void Cleanup()
    {
        AdministrarCotizaciones.Instancia.LimpiarParaTest();
        AdministrarClientes.Instancia.LimpiarParaTest();
        AdministrarUsuarios.Instancia.LimpiarParaTest();
    }

    [TestMethod]
    public void CrearCotizacion_DeberiaCrearYAgregar()
    {
        var c = AdministrarCotizaciones.Instancia.CrearCotizacion(vendedor, cliente, DateTime.Now, DateTime.Now.AddDays(7), "Desc");
        Assert.IsNotNull(c);
        Assert.AreEqual(1, AdministrarCotizaciones.Instancia.Cotizaciones().Count);
        Assert.AreSame(c, AdministrarCotizaciones.Instancia.Cotizaciones()[0]);
        // También debiera haberse agregado a la lista del creador y del cliente por el constructor
        Assert.IsTrue(vendedor.ListaCotizaciones.Contains(c));
        Assert.IsTrue(cliente.ListaCotizaciones.Contains(c));
    }
    
}