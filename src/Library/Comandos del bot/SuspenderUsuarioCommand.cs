using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary;
using ClassLibrary.Excepciones;
using Discord.Commands;
namespace Library.Comandos_del_bot;

public class SuspenderUsuarioCommand: ModuleBase<SocketCommandContext>
{
    private readonly SessionService sessions;

    public SuspenderUsuarioCommand(SessionService session)
    {
        this.sessions = session;
    }

    [Command("suspenderusuario")]
    public async Task SuspenderUsuarioCommandAsync(string emailOTelefono)
    {
        Usuario vendedor = sessions.GetUsuario(Context.User.Id);
        if (vendedor == null)
        {
            await ReplyAsync("Debes iniciar sesión primero con `!login`.");
            return;
        }

        if (vendedor is not Administrador admin)
        {
            await ReplyAsync("Solo los administradores pueden suspender usuarios.");
            return;
        }
        
        if (string.IsNullOrEmpty(emailOTelefono))
        {
            await ReplyAsync("Formato incorrecto: use '!suspenderusuario EmailOTelefonoUsuario'");
            return;
        }

        Usuario usuarioASuspender = Fachada.FachadaSistema.DelegarBuscarUsuario(emailOTelefono);
        if (usuarioASuspender == null)
        {
            await ReplyAsync($"No se encontró ningún usuario que coincida con '{emailOTelefono}'.");
            return;
        }

        if (usuarioASuspender is Administrador)
        {
            await ReplyAsync("No se pueden suspender administradores.");
            return;
        }

        if (usuarioASuspender == admin)
        {
            await ReplyAsync("No puedes suspender tu propio usuario.");
            return;
        }

        try
        {
            Fachada.FachadaSistema.DelegarSuspenderUsuario(admin, usuarioASuspender);

            await ReplyAsync($"El usuario **{usuarioASuspender.Nombre} {usuarioASuspender.Apellido}** ha sido suspendido.");
        }
        catch (Exception ex)
        {
            await ReplyAsync($"No se pudo suspender al usuario: {ex.Message}");
        }
    }

}