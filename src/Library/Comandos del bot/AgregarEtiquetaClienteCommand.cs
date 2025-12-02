using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassLibrary;
using Discord.Commands;
namespace Library.Comandos_del_bot;

public class AgregarEtiquetaClienteCommand: ModuleBase<SocketCommandContext>
{
    private readonly SessionService sessions;

    public AgregarEtiquetaClienteCommand(SessionService sessions)
    {
        this.sessions = sessions;
    }

    [Command("agregaretiquetacliente")]
    public async Task AgregarEtiquetaClienteCommandAsync(string email, [Remainder] string etiqueta)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(etiqueta))
        {
            await ReplyAsync("Formato inválido. Uso: `!agregaretiquetacliente EmailCLiente etiqueta`");
            return;
        }

        // Verificar login
        Usuario vendedor = sessions.GetUsuario(Context.User.Id);
        if (vendedor == null)
        {
            await ReplyAsync("Debes iniciar sesión primero con `!login`.");
            return;
        }

        // Buscar cliente
        List<Cliente> clientes = Fachada.FachadaSistema.DelegarBuscarClientes(email);

        if (clientes.Count == 0)
        {
            await ReplyAsync("No se encontró un cliente con ese correo.");
            return;
        }

        if (clientes.Count > 1)
        {
            await ReplyAsync("Se encontraron múltiples clientes con ese dato. Usa un dato único como correo.");
            return;
        }

        Cliente cliente = clientes[0];

        Fachada.FachadaSistema.DelegarAgregarEtiquetaCliente(vendedor, cliente, etiqueta);

        await ReplyAsync($"La etiqueta **{etiqueta}** fue añadida correctamente a **{cliente.Nombre} {cliente.Apellido}**.");
    }

}