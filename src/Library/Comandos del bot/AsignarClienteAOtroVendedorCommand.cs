using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary;
using ClassLibrary.Excepciones;
using Discord.Commands;
namespace Library.Comandos_del_bot;

public class AsignarClienteAOtroVendedorCommand: ModuleBase<SocketCommandContext>
{
    private readonly SessionService sessions;

    public AsignarClienteAOtroVendedorCommand(SessionService session)
    {
        this.sessions = session;
    }

    [Command("asignarclienteaotrovendedor")]
    public async Task AsignarClienteAOtroVendedorCommandAsync(string emailOTelefonoCliente, string emailOTelefonoVendedor)
    {
        Usuario usuario = sessions.GetUsuario(Context.User.Id);
        if (usuario == null)
        {
            await ReplyAsync("Debes iniciar sesión primero con `!login`.");
            return;
        }

        if (string.IsNullOrEmpty(emailOTelefonoCliente) || string.IsNullOrEmpty(emailOTelefonoVendedor))
        {
            await ReplyAsync(
                "No se ha introducido datos suficientes. Usa '!asignarclienteaotrovendedor EmailOTelefonoCliente EmailOTelefonoVendedorNuevo");
            return;
        }

        if (usuario is not Vendedor vendedor)
        {
            await ReplyAsync("Solo los vendedores pueden intercambiar clientes.");
            return;
        }

        // Buscar el cliente correcto
        List<Cliente> clientes = Fachada.FachadaSistema.DelegarBuscarClientes(vendedor, emailOTelefonoCliente);

        if (clientes.Count == 0)
        {
            await ReplyAsync("No se encontró un cliente con ese email o teléfono.");
            return;
        }

        Cliente clienteAIntercambiar = clientes[0];

        // Buscar el vendedor nuevo
        Usuario vendedorNuevo = Fachada.FachadaSistema.DelegarBuscarUsuario(emailOTelefonoVendedor);

        if (vendedorNuevo is not Vendedor nuevoVendedor)
        {
            await ReplyAsync("El usuario destino debe ser un vendedor.");
            return;
        }

        try
        {
            Fachada.FachadaSistema.DelegarAsignarClienteAOtroVendedor(
                vendedor, clienteAIntercambiar, nuevoVendedor
            );

            await ReplyAsync($"El cliente **{clienteAIntercambiar.Nombre} {clienteAIntercambiar.Apellido}** " + 
                             $"ha sido asignado al vendedor **{nuevoVendedor.Nombre} {nuevoVendedor.Apellido}**.");
        }
        catch (Exception ex)
        {
            await ReplyAsync($"No se pudo intercambiar el cliente: {ex.Message}");
        }
    }

}