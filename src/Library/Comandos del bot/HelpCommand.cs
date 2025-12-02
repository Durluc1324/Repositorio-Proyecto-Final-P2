using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary;
using ClassLibrary.Excepciones;
using Discord.Commands;
namespace Library.Comandos_del_bot;

public class HelpCommand: ModuleBase<SocketCommandContext>
{
    private readonly SessionService sessions;

    public HelpCommand(SessionService session)
    {
        this.sessions = session;
    }

    [Command("help")]

    public async Task HelpCommandAsync()
    {
        StringBuilder panelDeAyuda = new StringBuilder();
        panelDeAyuda.AppendLine("--------------");
        panelDeAyuda.AppendLine("   COMANDOS   ");
        panelDeAyuda.AppendLine("--------------");
        panelDeAyuda.AppendLine();
        panelDeAyuda.AppendLine("Detalles a tener en cuenta:");
        panelDeAyuda.AppendLine(" * Las fechas deben siempre escribirse con : para diferenciar los días");
        panelDeAyuda.AppendLine(" * Los comandos que incluyan | deben de tenerlo sí o sí para diferenciar cada parte del contenido");
        panelDeAyuda.AppendLine();
        panelDeAyuda.AppendLine("---------------------------------------------");
        panelDeAyuda.AppendLine("!agregaretiquetacliente EmailCliente etiqueta");
        panelDeAyuda.AppendLine("!agregarnota indice nota");
        panelDeAyuda.AppendLine("!asignarclienteaotrovendedor EmailOTelefonoCliente EmailOTelefonoVendedorNuevo");
        panelDeAyuda.AppendLine("!buscarcliente criterio");
        panelDeAyuda.AppendLine("!buscarclientesininteracciondesde dd:mm:yyyy");
        panelDeAyuda.AppendLine("!buscarventaporrangofecha dd:mm:yyyy dd:mm:yyyy");
        panelDeAyuda.AppendLine("!crearcliente nombre apellido telefono genero dd:mm:yyyy");
        panelDeAyuda.AppendLine("!crearcliente nombre apellido telefono genero dd:mm:yyyy");
        panelDeAyuda.AppendLine("!crearcotizacion EmailOTelefonoCliente  dd:mm:yyyy descrición");
        panelDeAyuda.AppendLine("!crearcotizacion EmailOTelefonoCliente  dd:mm:yyyy descrición");
        panelDeAyuda.AppendLine("!crearemail CorreoEmisor CorreoReceptor dd:mm:yyyy Tema | contenido");
        panelDeAyuda.AppendLine("!crearemail CorreoEmisor CorreoReceptor dd:mm:yyyy tema | contenido (la barra sí o sí es necesaria)");
        panelDeAyuda.AppendLine("!crearllamada NumeroDelCliente dd:mm:yyyy Tema");
        panelDeAyuda.AppendLine("!crearmensaje NumeroEmisor NumeroReceptor dd:mm:yyyy Contenido");
        panelDeAyuda.AppendLine("!crearmensaje NumeroEmisor NumeroReceptor dd:mm:yyyy Contenido");
        panelDeAyuda.AppendLine("!crearreunion CorreoReceptor dd:mm:yyyy Tema | Lugar");
        panelDeAyuda.AppendLine("!crearusuario Nombre Apellido Email Telefono Contraseña Rol");
        panelDeAyuda.AppendLine("!crearventa correo producto precio cantidad ...");
        panelDeAyuda.AppendLine("!eliminarcliente EmailOTelefono");
        panelDeAyuda.AppendLine("!login EmailOTelefono Contraseña");
        panelDeAyuda.AppendLine("!logout");
        panelDeAyuda.AppendLine("!misinteracciones");
        panelDeAyuda.AppendLine("!modificarcliente email parametro:valor ...");
        panelDeAyuda.AppendLine("!rehabilitarusuario EmailOTelefonoUsuario");
        panelDeAyuda.AppendLine("!suspenderusuario EmailOTelefonoUsuario");
        panelDeAyuda.AppendLine("!verclientespropios");
        panelDeAyuda.AppendLine("!verinteraccionconcliente EmailCLiente");
        panelDeAyuda.AppendLine("!yo");
        panelDeAyuda.AppendLine("---------------------------------------------");

        await ReplyAsync(panelDeAyuda.ToString());
    }
}