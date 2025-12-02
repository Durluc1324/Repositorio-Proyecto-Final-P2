using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary;
using ClassLibrary.Excepciones;
using Discord.Commands;
namespace Library.Comandos_del_bot;

public class RehabilitarUsuarioCommand: ModuleBase<SocketCommandContext>
{
    private readonly SessionService sessions;

    public RehabilitarUsuarioCommand(SessionService session)
    {
        this.sessions = session;
    }

    [Command("rehabilitarusuario")]
    public async Task RehabilitarUsuarioCommandAsync(string emailOTelefono)
    {
        Usuario vendedor = sessions.GetUsuario(Context.User.Id);
        if (vendedor == null)
        {
            await ReplyAsync("Debes iniciar sesión primero con `!login`.");
            return;
        }

        if (vendedor is not Administrador admin)
        {
            await ReplyAsync("Solo los administradores pueden rehabilitar usuarios.");
            return;
        }

        if (string.IsNullOrEmpty(emailOTelefono))
        {
            await ReplyAsync("Formato incorrecto: use '!rehabilitarusuario EmailOTelefonoUsuario'");
            return;
        }
        
        Usuario usuarioARehabilitar = Fachada.FachadaSistema.DelegarBuscarUsuario(emailOTelefono);
        if (usuarioARehabilitar == null)
        {
            await ReplyAsync($"No se encontró ningún usuario que coincida con '{emailOTelefono}'.");
            return;
        }

        if (usuarioARehabilitar is Administrador)
        {
            await ReplyAsync("No se pueden rehabilitar administradores.");
            return;
        }

        if (usuarioARehabilitar == admin)
        {
            await ReplyAsync("No puedes rehabilitar tu propio usuario.");
            return;
        }

        if (!usuarioARehabilitar.Suspendido)
        {
            await ReplyAsync("No puedes rehabilitar usuarios no suspendidos.");
            return;
        }

        try
        {
            Fachada.FachadaSistema.DelegarRehabilitarUsuario(admin, usuarioARehabilitar);

            await ReplyAsync($"El usuario **{usuarioARehabilitar.Nombre} {usuarioARehabilitar.Apellido}** ha sido rehabilitado.");
        }
        catch (Exception ex)
        {
            await ReplyAsync($"No se pudo suspender al usuario: {ex.Message}");
        }
    }
}