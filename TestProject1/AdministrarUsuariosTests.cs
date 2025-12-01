using ClassLibrary;
namespace TestProject1;

[TestClass]
public class AdministrarUsuariosTests
{
    private Administrador admin;
    private Vendedor vendedor;

    [TestInitialize]
    public void Setup()
    {
        AdministrarUsuarios.Instancia.LimpiarParaTest();
        admin = new Administrador("Admin", "Test", "admin@test.com", "099111222");
        vendedor = new Vendedor("V", "Uno", "v1@test.com", "091000001");
    }

    [TestCleanup]
    public void Cleanup()
    {
        AdministrarUsuarios.Instancia.LimpiarParaTest();
    }

    [TestMethod]
    public void CrearUsuario_Admin_DeberiaCrearVendedorYAdmin()
    {
        AdministrarUsuarios.Instancia.CrearUsuario(admin, "Nombre", "Apellido", "nuevo@test.com", "091234","5678" ,TipoRol.VENDEDOR);
        Assert.AreEqual(1, AdministrarUsuarios.Instancia.Usuarios().Count);
        Assert.IsInstanceOfType(AdministrarUsuarios.Instancia.Usuarios()[0], typeof(Vendedor));

        AdministrarUsuarios.Instancia.CrearUsuario(admin, "A2", "B2", "a2@test.com", "099999", "2345",TipoRol.ADMINISTRADOR);
        Assert.AreEqual(2, AdministrarUsuarios.Instancia.Usuarios().Count);
        Assert.IsInstanceOfType(AdministrarUsuarios.Instancia.Usuarios()[1], typeof(Administrador));
    }

    [TestMethod]
    [ExpectedException(typeof(UnauthorizedAccessException))]
    public void CrearUsuario_NoAdmin_DeberiaLanzar()
    {
        AdministrarUsuarios.Instancia.CrearUsuario(vendedor, "X", "Y", "x@y.com", "091", "6789",TipoRol.VENDEDOR);
    }

    [TestMethod]
    public void EliminarUsuario_Admin_DeberiaEliminar()
    {
        // Crear 2 usuarios
        AdministrarUsuarios.Instancia.CrearUsuario(admin, "U1", "U1", "u1@test.com", "01", "1642",TipoRol.VENDEDOR);
        var u = AdministrarUsuarios.Instancia.Usuarios()[0];
        Assert.AreEqual(1, AdministrarUsuarios.Instancia.Usuarios().Count);

        AdministrarUsuarios.Instancia.EliminarUsuario(admin, u);
        Assert.AreEqual(0, AdministrarUsuarios.Instancia.Usuarios().Count);
    }

    [TestMethod]
    [ExpectedException(typeof(UnauthorizedAccessException))]
    public void EliminarUsuario_NoAdmin_DeberiaLanzar()
    {
        AdministrarUsuarios.Instancia.CrearUsuario(admin, "U1", "U1", "u1@test.com", "u1", "2542",TipoRol.VENDEDOR);
        var u = AdministrarUsuarios.Instancia.Usuarios()[0];
        AdministrarUsuarios.Instancia.EliminarUsuario(vendedor, u);
    }

    [TestMethod]
    public void SuspenderYRehabilitarUsuario_Admin_DeberiaCambiarEstado()
    {
        AdministrarUsuarios.Instancia.CrearUsuario(admin, "U2", "U2", "u2@test.com", "02", "2531",TipoRol.VENDEDOR);
        var u = AdministrarUsuarios.Instancia.Usuarios()[0];
        AdministrarUsuarios.Instancia.SuspenderUsuario(admin, u);
        Assert.IsTrue(u.Suspendido);

        AdministrarUsuarios.Instancia.RehabilitarUsuario(admin, u);
        Assert.IsFalse(u.Suspendido);
    }

    [TestMethod]
    [ExpectedException(typeof(UnauthorizedAccessException))]
    public void SuspenderUsuario_NoPuedeSuspenderAdmin()
    {
        // Crear otro admin
        AdministrarUsuarios.Instancia.CrearUsuario(admin, "Super", "Admin", "s@a.com", "03", "63q23",TipoRol.ADMINISTRADOR);
        var otroAdmin = AdministrarUsuarios.Instancia.Usuarios()[0];
        AdministrarUsuarios.Instancia.SuspenderUsuario(admin, otroAdmin); // debe lanzar
    }

    [TestMethod]
    [ExpectedException(typeof(UnauthorizedAccessException))]
    public void VerTodos_NoAdmin_DeberiaLanzar()
    {
        AdministrarUsuarios.Instancia.VerTodos(vendedor);
    }

    [TestMethod]
    public void VerTodos_Admin_DeberiaRetornarLista()
    {
        AdministrarUsuarios.Instancia.CrearUsuario(admin, "U1", "U1", "u1@test.com", "01", "3563",TipoRol.VENDEDOR);
        var list = AdministrarUsuarios.Instancia.VerTodos(admin);
        Assert.IsNotNull(list);
        Assert.AreEqual(1, list.Count);
    }
}