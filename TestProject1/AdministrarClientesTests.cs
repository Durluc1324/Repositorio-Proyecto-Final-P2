using ClassLibrary;

namespace TestProject1;

[TestClass]
public class AdministrarClientesTests
{
    private Administrador admin;
    private Vendedor vendedor;
    private Vendedor otroVendedor;

    [TestInitialize]
    public void Setup()
    {
        AdministrarClientes.Instancia.LimpiarParaTest();
        AdministrarUsuarios.Instancia.LimpiarParaTest();

        admin = new Administrador("Admin", "Test", "admin@test.com", "099111222");
        vendedor = new Vendedor("V", "Uno", "v1@test.com", "091000001");
        otroVendedor = new Vendedor("V", "Dos", "v2@test.com", "091000002");

        // Registrar usuarios en el admin (si lo necesitás)
        AdministrarUsuarios.Instancia.CrearUsuario(admin, vendedor.Nombre, vendedor.Apellido, vendedor.Email, vendedor.Telefono, TipoRol.VENDEDOR);
        AdministrarUsuarios.Instancia.CrearUsuario(admin, otroVendedor.Nombre, otroVendedor.Apellido, otroVendedor.Email, otroVendedor.Telefono, TipoRol.VENDEDOR);
    }

    [TestCleanup]
    public void Cleanup()
    {
        AdministrarClientes.Instancia.LimpiarParaTest();
        AdministrarUsuarios.Instancia.LimpiarParaTest();
    }

    private Cliente CrearClienteEjemplo(Usuario solicitante)
    {
        return AdministrarClientes.Instancia.CrearCliente(
            solicitante, "Tom", "Rodriguez", "tom@test.com", "091234567", "Hombre", new DateTime(2000, 2, 10)
        );
    }

    [TestMethod]
    public void CrearCliente_DeberiaCrearClienteYAgregarALasListasCorrespondientes()
    {
        // Act
        Cliente creado = CrearClienteEjemplo(admin);

        // Assert
        Assert.IsNotNull(creado);
        Assert.AreEqual(1, AdministrarClientes.Instancia.Clientes().Count);
        Assert.AreSame(creado, AdministrarClientes.Instancia.Clientes()[0]);
        Assert.AreEqual(1, admin.ClientesAsignados.Count);
        Assert.AreSame(creado, admin.ClientesAsignados[0]);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void CrearCliente_UsuarioSuspendido_DeberiaLanzar()
    {
        vendedor.Suspendido = true;
        AdministrarClientes.Instancia.CrearCliente(vendedor, "A", "B", "a@b.com", "091", "X", DateTime.Now);
    }

    [TestMethod]
    public void ModificarCliente_UsuarioAsignado_DeberiaModificarCampos()
    {
        var cliente = CrearClienteEjemplo(vendedor);

        AdministrarClientes.Instancia.ModificarCliente(vendedor, cliente, nombre: "Nuevo", telefono: "000");

        Assert.AreEqual("Nuevo", cliente.Nombre);
        Assert.AreEqual("000", cliente.Telefono);
    }

    [TestMethod]
    [ExpectedException(typeof(UnauthorizedAccessException))]
    public void ModificarCliente_OtroVendedorNoAdmin_DeberiaLanzar()
    {
        var cliente = CrearClienteEjemplo(vendedor);
        // otroVendedor intenta modificar cliente que no le pertenece
        AdministrarClientes.Instancia.ModificarCliente(otroVendedor, cliente, nombre: "Hacker");
    }

    [TestMethod]
    public void EliminarCliente_Admin_DeberiaEliminar()
    {
        var cliente = CrearClienteEjemplo(vendedor);
        Assert.AreEqual(1, AdministrarClientes.Instancia.Clientes().Count);

        AdministrarClientes.Instancia.EliminarCliente(admin, cliente);

        Assert.AreEqual(0, AdministrarClientes.Instancia.Clientes().Count);
        Assert.IsFalse(vendedor.ClientesAsignados.Contains(cliente));
    }

    [TestMethod]
    [ExpectedException(typeof(UnauthorizedAccessException))]
    public void EliminarCliente_OtroVendedorNoAdmin_DeberiaLanzar()
    {
        var cliente = CrearClienteEjemplo(vendedor);
        AdministrarClientes.Instancia.EliminarCliente(otroVendedor, cliente);
    }

    [TestMethod]
    public void BuscarClientes_DeberiaEncontrarPorNombreYApeEmailTelefono()
    {
        var c1 = AdministrarClientes.Instancia.CrearCliente(admin, "Carlos", "Perez", "carlos@a.com", "100", "M", DateTime.Now);
        var c2 = AdministrarClientes.Instancia.CrearCliente(admin, "Ana", "Lopez", "ana@b.com", "200", "F", DateTime.Now);

        var res1 = AdministrarClientes.Instancia.BuscarClientes("Carlos");
        Assert.IsTrue(res1.Contains(c1));
        Assert.IsFalse(res1.Contains(c2));

        var res2 = AdministrarClientes.Instancia.BuscarClientes("ana@b.com");
        Assert.IsTrue(res2.Contains(c2));
    }

    [TestMethod]
    public void BuscarClientes_DeberiaBuscarPorEtiqueta()
    {
        var c = AdministrarClientes.Instancia.CrearCliente(admin, "P", "Q", "p@q.com", "300", "X", DateTime.Now);
        AdministrarClientes.Instancia.AgregarEtiquetaCliente(admin, c, "VIP");

        List<Cliente> res = AdministrarClientes.Instancia.BuscarClientes("VIP");
        Assert.IsTrue(res.Contains(c));
    }

    [TestMethod]
    public void AgregarEtiquetaCliente_NoDuplicaYValidaPermisos()
    {
        var c = AdministrarClientes.Instancia.CrearCliente(vendedor, "Cl", "Ux", "x@x.com", "400", "X", DateTime.Now);

        AdministrarClientes.Instancia.AgregarEtiquetaCliente(vendedor, c, "nuevo");
        Assert.IsTrue(c.Etiquetas.Contains("nuevo"));

        // Intentar agregar igual no duplica
        AdministrarClientes.Instancia.AgregarEtiquetaCliente(vendedor, c, "nuevo");
        Assert.AreEqual(1, c.Etiquetas.Count);
    }

    [TestMethod]
    [ExpectedException(typeof(UnauthorizedAccessException))]
    public void AgregarEtiquetaCliente_OtroVendedorNoAdmin_DeberiaLanzar()
    {
        var c = AdministrarClientes.Instancia.CrearCliente(vendedor, "Cl2", "Ux2", "x2@x.com", "401", "X", DateTime.Now);
        AdministrarClientes.Instancia.AgregarEtiquetaCliente(otroVendedor, c, "bad");
    }

    [TestMethod]
    public void AsignarClienteAOtroVendedor_DeberiaMoverYActualizarUsuarioAsignado()
    {
        var cliente = AdministrarClientes.Instancia.CrearCliente(vendedor, "Asig", "Cli", "as@c.com", "500", "X", DateTime.Now);
        Assert.IsTrue(vendedor.ClientesAsignados.Contains(cliente));

        AdministrarClientes.Instancia.AsignarClienteAOtroVendedor(vendedor, cliente, otroVendedor);

        Assert.IsFalse(vendedor.ClientesAsignados.Contains(cliente));
        Assert.IsTrue(otroVendedor.ClientesAsignados.Contains(cliente));
        Assert.AreSame(otroVendedor, cliente.UsuarioAsignado);
    }
}