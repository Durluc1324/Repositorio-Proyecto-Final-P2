using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary;
using ClassLibrary.Excepciones;
using Discord.Commands;
namespace Library.Comandos_del_bot;

public class CrearMensajeCommand:ModuleBase<SocketCommandContext>
{
    private readonly SessionService sessions;
    
    public CrearMensajeCommand(SessionService sessions)
    {
        this.sessions = sessions;
    }

    [Command("crearmensaje")]
    public async Task CrearMensajeCommandAsync(string numeroEmisor, string numeroReceptor, string fechaCreacion, [Remainder]string contenido)
    {
        if (string.IsNullOrEmpty(numeroEmisor) || string.IsNullOrEmpty(numeroReceptor) || string.IsNullOrEmpty(fechaCreacion) || string.IsNullOrEmpty(contenido))
        {
            await ReplyAsync("No se han introducido datos suficientes. Use '!crearmensaje numeroemisor numeroreceptor dd:mm:yyyy contenido");
            return;
        }
        
        
        Usuario vendedor = sessions.GetUsuario(Context.User.Id);
        if (vendedor == null)
        {
            await ReplyAsync("Debes iniciar sesión primero con `!login`.");
            return;
        }
        
        Persona emisor = null;
        Persona receptor = null;

        if (vendedor.Telefono.Equals(numeroEmisor))
        {
            emisor = vendedor;
            receptor = Fachada.FachadaSistema.BuscarPersona(numeroReceptor);
        }
        else if (vendedor.Telefono.Equals(numeroReceptor))

        {
            emisor = Fachada.FachadaSistema.BuscarPersona(numeroEmisor);
            receptor = vendedor;
        }
        
        if (emisor == null)
        {
            await ReplyAsync($"No se encontró al emisor con teléfono {numeroEmisor}");
            return;
        }

        if (receptor == null)
        {
            await ReplyAsync($"No se encontró al receptor con teléfono {numeroReceptor}");
            return;
        }

        string fecha = fechaCreacion;
        string[] partesFecha = fecha.Split(":");
        
        if (partesFecha.Length != 3 ||
            !int.TryParse(partesFecha[0], out int dia) ||
            !int.TryParse(partesFecha[1], out int mes) ||
            !int.TryParse(partesFecha[2], out int año))
        {
            await ReplyAsync("Fecha inválida. Usa el formato dd:mm:yyyy");
            return;
        }
        DateTime fechadeMensaje;

        try
        {
            fechadeMensaje = new DateTime(año, mes, dia);
        }
        catch
        {
            await ReplyAsync("La fecha ingresada no es válida.");
            return;
        }

        Mensaje mensajeCreado = Fachada.FachadaSistema.DelegarCrearMensaje(emisor, receptor, fechadeMensaje, contenido);

        await ReplyAsync($"El mensaje de {emisor.Nombre} {emisor.Apellido} a {receptor.Nombre} {receptor.Apellido} ha sido creado con exito");
    }
}