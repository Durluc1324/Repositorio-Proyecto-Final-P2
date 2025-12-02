using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary;
using Discord.Commands;
namespace Library.Comandos_del_bot;

public class EliminarUsuarioCommand: ModuleBase<SocketCommandContext>
{
    private readonly SessionService sessions;

    public EliminarUsuarioCommand(SessionService session)
    {
        this.sessions = session;
    }

    [Command("eliminarusuario")]
    public async Task EliminarUsuarioCommandAsync(string criterio)
    {
        Usuario vendedor = sessions.GetUsuario(Context.User.Id);
        if (vendedor == null)
        {
            await ReplyAsync("Debes iniciar sesión primero con `!login`.");
            return;
        }

        if (vendedor is not Administrador admin)
        {
            await ReplyAsync("Solo los administradores pueden eliminar usuarios.");
            return;
        }

        Usuario usuarioAEliminar = Fachada.FachadaSistema.DelegarBuscarUsuario(criterio);

        if (usuarioAEliminar == null)
        {
            await ReplyAsync($"No se encontró ningún usuario que coincida con '{criterio}'.");
            return;
        }

        if (usuarioAEliminar== admin)
        {
            await ReplyAsync("No puedes eliminar tu propio usuario.");
            return;
        }

        try
        {
            Fachada.FachadaSistema.DelegarEliminarUsuario(admin, usuarioAEliminar);

            await ReplyAsync($"El usuario **{usuarioAEliminar.Nombre} {usuarioAEliminar.Apellido}** ha sido eliminado exitosamente.");
        }
        catch (Exception ex)
        {
            // Manejo básico de errores de dominio
            await ReplyAsync($"No se pudo eliminar el usuario: {ex.Message}");
        }
    }

}