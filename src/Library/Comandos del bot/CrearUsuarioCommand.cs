using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary;
using Discord.Commands;
namespace Library.Comandos_del_bot;

public class CrearUsuarioCommand: ModuleBase<SocketCommandContext>
{
    private readonly SessionService sessions;

    public CrearUsuarioCommand(SessionService session)
    {
        this.sessions = session;
    }

    [Command("crearusuario")]
    public async Task CrearUsuarioCommandAsync(string nombre, string apellido, string email, string telefono, string contraseña, string rol)
    {
        Usuario vendedor = sessions.GetUsuario(Context.User.Id);
        if (vendedor == null)
        {
            await ReplyAsync("Debes iniciar sesión primero con `!login`.");
            return;
        }

        // Solo administradores
        if (vendedor is not Administrador admin)
        {
            await ReplyAsync("Solo los administradores pueden crear usuarios.");
            return;
        }

        // Validación simple
        if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(apellido) ||
            string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(telefono) ||
            string.IsNullOrWhiteSpace(contraseña) || string.IsNullOrWhiteSpace(rol))
        {
            await ReplyAsync("Formato incorrecto. Uso: '!crearusuario nombre apellido email telefono contraseña rol'");
            return;
        }

        Usuario nuevoUsuario = Fachada.FachadaSistema.DelegarCrearUsuario(
            admin, nombre, apellido, email, telefono, contraseña, rol);

        if (nuevoUsuario == null)
        {
            await ReplyAsync("Error: rol inválido. Debe ser usuario, vendedor o administrador.");
            return;
        }

        await ReplyAsync($"El nuevo {rol} ha sido creado exitosamente.");
    }

}