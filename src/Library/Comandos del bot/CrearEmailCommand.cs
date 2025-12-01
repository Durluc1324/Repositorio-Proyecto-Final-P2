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
    public async Task CrearEmailCommandAsync([Remainder] string mensaje)
    {
        if (string.IsNullOrEmpty(mensaje))
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

        var partes = mensaje.Split("|");
        if (partes.Length != 2)
        {
            await ReplyAsync("Debes separar el tema del contenido con `|`.");
            return;
        }

        string datosPrincipales = partes[0].Trim();
        string contenido = partes[1].Trim();

        string[] datos = datosPrincipales.Split(" ");

        if (datos.Length < 4)
        {
            await ReplyAsync(
                "Faltan datos. Uso: `!crearemail correoemisor correoreceptor dd:mm:yyyy tema | contenido`");
            return;
        }

        Persona emisor = null;
        Persona receptor = null;

        if (vendedor.Email.Equals(datos[0]))
        {
            emisor = vendedor;
            receptor = Fachada.FachadaSistema.BuscarPersona(datos[1]);
        }
        else if (vendedor.Email.Equals(datos[1]))
        {
            emisor = Fachada.FachadaSistema.BuscarPersona(datos[0]);
            receptor = vendedor;
        }

        if (emisor == null)
        {
            await ReplyAsync($"No se encontró al emisor con el correo {datos[0]}");
            return;
        }

        if (receptor == null)
        {
            await ReplyAsync($"No se encontró al receptor con el correo {datos[1]}");
            return;
        }

        // Parseo de fecha
        string[] partesFecha = datos[2].Split(":");
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

        string tema = string.Join(" ", datos.Skip(3));

        Email email = Fachada.FachadaSistema.DelegarCrearEmail(
            emisor, receptor, fechaEmail, tema, contenido
        );

        await ReplyAsync($"El email de {email.DireccionEmisor} a {email.DireccionReceptor} ha sido creado correctamente.");
    }
}

