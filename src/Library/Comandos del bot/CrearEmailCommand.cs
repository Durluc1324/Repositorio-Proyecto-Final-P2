using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary;
using ClassLibrary.Excepciones;
using Discord.Commands;
namespace Library.Comandos_del_bot;

public class CrearEmailCommand : ModuleBase<SocketCommandContext>
{
    private readonly SessionService sessions;

    public CrearEmailCommand(SessionService sessions)
    {
        this.sessions = sessions;
    }

    [Command("crearemail")]
    public async Task CrearEmailCommandAsync(string correoEmisor, string correoReceptor, string fecha, [Remainder]string temaYContenido)
    {
        if (string.IsNullOrEmpty(correoEmisor) || string.IsNullOrEmpty(correoReceptor) || string.IsNullOrEmpty(fecha) || string.IsNullOrEmpty(temaYContenido))
        {
            await ReplyAsync("Formato incorrecto.\nUso: `!crearemail correoemisor correoreceptor dd:mm:yyyy tema | contenido`");
            return;
        }

        Usuario vendedor = sessions.GetUsuario(Context.User.Id);
        if (vendedor == null)
        {
            await ReplyAsync("Debes iniciar sesión primero con `!login`.");
            return;
        }

        var partes = temaYContenido.Split("|");
        if (partes.Length != 2)
        {
            await ReplyAsync("Debes separar el tema del contenido con `|`.");
            return;
        }

        string datosPrincipales = partes[0].Trim();
        
        
        string[] tema_Contenido = temaYContenido.Split("|");
        string contenido = tema_Contenido[1].Trim();
        string tema = tema_Contenido[0].Trim();

        

        Persona emisor = null;
        Persona receptor = null;

        if (vendedor.Email.Equals(correoEmisor))
        {
            emisor = vendedor;
            receptor = Fachada.FachadaSistema.BuscarPersona(correoReceptor);
        }
        else if (vendedor.Email.Equals(correoReceptor))
        {
            emisor = Fachada.FachadaSistema.BuscarPersona(correoEmisor);
            receptor = vendedor;
        }

        if (emisor == null)
        {
            await ReplyAsync($"No se encontró al emisor con el correo {correoEmisor}");
            return;
        }

        if (receptor == null)
        {
            await ReplyAsync($"No se encontró al receptor con el correo {correoReceptor}");
            return;
        }

        // Parseo de fecha
        string[] partesFecha = fecha.Split(":");
        if (partesFecha.Length != 3 ||
            !int.TryParse(partesFecha[0], out int dia) ||
            !int.TryParse(partesFecha[1], out int mes) ||
            !int.TryParse(partesFecha[2], out int año))
        {
            await ReplyAsync("Fecha inválida. Usa el formato dd:mm:yyyy");
            return;
        }

        DateTime fechaEmail;
        try
        {
            fechaEmail = new DateTime(año, mes, dia);
        }
        catch
        {
            await ReplyAsync("La fecha ingresada no es válida.");
            return;
        }
        

        Email email = Fachada.FachadaSistema.DelegarCrearEmail(
            emisor, receptor, fechaEmail, tema, contenido
        );

        await ReplyAsync($"El email de {email.DireccionEmisor} a {email.DireccionReceptor} ha sido creado correctamente.");
    }
}

