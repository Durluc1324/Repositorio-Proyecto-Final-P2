using ClassLibrary;
namespace TestProject1;

[TestClass]
public class AdministrarInteraccionesTests
{
    private Administrador admin;
    private Vendedor vendedor;
    private Cliente cliente;

    [TestInitialize]
    public void Setup()
    {
        AdministrarInteracciones.Instancia.LimpiarParaTest();
        AdministrarClientes.Instancia.LimpiarParaTest();
        AdministrarUsuarios.Instancia.LimpiarParaTest();

        admin = new Administrador("Admin", "Test", "admin@test.com", "099111222");
        vendedor = new Vendedor("V", "Uno", "v1@test.com", "091000001");

        cliente = AdministrarClientes.Instancia.CrearCliente(admin, "Cli", "Uno", "cli1@test.com", "0912", "X", DateTime.Now);
    }

    [TestCleanup]
    public void Cleanup()
    {
        AdministrarInteracciones.Instancia.LimpiarParaTest();
        AdministrarClientes.Instancia.LimpiarParaTest();
        AdministrarUsuarios.Instancia.LimpiarParaTest();
    }

    [TestMethod]
    public void CrearLlamada_DeberiaAgregarATodosLosListados()
    {
        var fecha = DateTime.Now;
        var llamada = AdministrarInteracciones.Instancia.CrearLlamada(vendedor, cliente, fecha, "Tema");

        Assert.IsNotNull(llamada);
        Assert.AreEqual(1, AdministrarInteracciones.Instancia.Interacciones().Count);
        Assert.IsTrue(vendedor.ListaInteracciones.Contains(llamada));
        Assert.IsTrue(cliente.ListaInteracciones.Contains(llamada));
    }

    [TestMethod]
    public void CrearMensaje_CrearEmail_CrearReunion_DeberianAgregar()
    {
        var m = AdministrarInteracciones.Instancia.CrearMensaje(vendedor, cliente, DateTime.Now, "M");
        var e = AdministrarInteracciones.Instancia.CrearEmail(vendedor, cliente, DateTime.Now, "E", "Contenido");
        var r = AdministrarInteracciones.Instancia.CrearReunion(vendedor, cliente, DateTime.Now, "R", "Lugar");

        Assert.AreEqual(3, AdministrarInteracciones.Instancia.Interacciones().Count);
        Assert.IsTrue(vendedor.ListaInteracciones.Contains(m));
        Assert.IsTrue(cliente.ListaInteracciones.Contains(e));
        Assert.IsTrue(cliente.ListaInteracciones.Contains(r));
    }

    [TestMethod]
    public void VerInteraccionesCliente_DeberiaFiltrarPorTipoYFecha()
    {
        var fechaHoy = DateTime.Today;
        var m1 = AdministrarInteracciones.Instancia.CrearMensaje(vendedor, cliente, fechaHoy, "uno");
        var m2 = AdministrarInteracciones.Instancia.CrearMensaje(vendedor, cliente, fechaHoy.AddDays(1), "dos");
        var l = AdministrarInteracciones.Instancia.CrearLlamada(vendedor, cliente, fechaHoy, "ll");

        var resTipo = AdministrarInteracciones.Instancia.VerInteraccionesCliente(cliente, typeof(Mensaje), null);
        Assert.IsTrue(resTipo.Contains(m1));
        Assert.IsTrue(resTipo.Contains(m2));
        Assert.IsFalse(resTipo.Contains(l));

        var resFecha = AdministrarInteracciones.Instancia.VerInteraccionesCliente(cliente, null, fechaHoy);
        Assert.IsTrue(resFecha.Contains(m1));
        Assert.IsTrue(resFecha.Contains(l));
        Assert.IsFalse(resFecha.Contains(m2));
    }

    [TestMethod]
    public void AgregarNota_DeberiaFijarNotaEnInteraccion()
    {
        var m = AdministrarInteracciones.Instancia.CrearMensaje(vendedor, cliente, DateTime.Now, "X");
        AdministrarInteracciones.Instancia.AgregarNota(m, "nota de prueba");

        Assert.AreEqual("nota de prueba", m.Nota);
    }

    
    
}