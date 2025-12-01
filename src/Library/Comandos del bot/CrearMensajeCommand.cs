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
    public async Task CrearMensajeCommandAsync(string mensaje)
    {
        if (string.IsNullOrEmpty(mensaje))
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
        
        string[] datos = mensaje.Split(" ");
        Persona emisor = null;
        Persona receptor = null;

        if (vendedor.Telefono.Equals(datos[0]))
        {
            emisor = vendedor;
            receptor = Fachada.FachadaSistema.BuscarPersona(datos[1]);
        }
        else if (vendedor.Telefono.Equals(datos[1]))

        {
            emisor = Fachada.FachadaSistema.BuscarPersona(datos[0]);
            receptor = vendedor;
        }
        
        if (emisor == null)
        {
            await ReplyAsync($"No se encontró al emisor con teléfono {datos[0]}");
            return;
        }

        if (receptor == null)
        {
            await ReplyAsync($"No se encontró al receptor con teléfono {datos[1]}");
            return;
        }


        string tema = string.Join(" ", datos.Skip(3));

        string fecha = datos[2];
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

        Mensaje mensajeCreado = Fachada.FachadaSistema.DelegarCrearMensaje(emisor, receptor, fechadeMensaje, tema);

        await ReplyAsync($"El mensaje de {emisor.Nombre} {emisor.Apellido} a {receptor.Nombre} {receptor.Apellido} ha sido creado con exito");
    }
}