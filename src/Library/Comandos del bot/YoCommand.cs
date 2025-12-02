using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary;
using Discord.Commands;
namespace Library.Comandos_del_bot;

public class YoCommand : ModuleBase<SocketCommandContext>
{
    private readonly SessionService sessions;

    public YoCommand(SessionService sessions)
    {
        this.sessions = sessions;
    }

    [Command("yo")]
    public async Task YoAsync()
    {
        // Obtenemos al usuario autenticado dentro de tu SessionService
        var u = sessions.GetUsuario(Context.User.Id);

        if (u == null)
        {
            await ReplyAsync("No estás autenticado. Usa el comando `!login` para iniciar sesión.");
            return;
        }

       
        string mensaje = ConstruirPanelUsuario(u);

        await ReplyAsync(mensaje);
    }

    // Dejé este método separado SOLO para que vos edites acá
    private string ConstruirPanelUsuario(Usuario u)
    {
        var panel = new StringBuilder();
        panel.AppendLine("---------------------------");
        panel.AppendLine("--Información del Usuario--");
        panel.AppendLine("---------------------------");
        panel.AppendLine($"Nombre: {u.Nombre}");
        panel.AppendLine($"Apellido: {u.Apellido}");
        panel.AppendLine($"Teléfono: {u.Telefono}");
        panel.AppendLine($"Correo Electrónico: {u.Email}");
        
        
        if (u.Rol == TipoRol.USUARIO)
        {
            panel.AppendLine("Rol: Usuario");
        }
        else if (u.Rol == TipoRol.VENDEDOR)
        {
            panel.AppendLine("Rol: Vendedor");
        }
        else panel.AppendLine("Rol: Administrador");
        
        panel.AppendLine("---------------------------");

        panel.AppendLine();
        // ==============================
        // CLIENTES
        // ==============================
        panel.AppendLine("--Clientes Totales--");
        panel.AppendLine("----------------------");
        panel.AppendLine();
        panel.AppendLine($"Cantidad: {u.ClientesAsignados.Count}");
        panel.AppendLine();
        foreach (var c in u.ClientesAsignados)
        {
            panel.AppendLine("----------");
            panel.AppendLine($"• {c.Nombre} {c.Apellido}");
            panel.AppendLine($"   Tel: {c.Telefono}");
            panel.AppendLine($"   Email: {c.Email}");
            panel.AppendLine($"   Genero: {c.Genero}");

        }
        panel.AppendLine("----------------------");

        panel.AppendLine();

        // ==============================
        // INTERACCIONES RECIENTES
        // ==============================

        panel.AppendLine("--Interacciones Recientes--");

        var ultimasInteracciones = u.ListaInteracciones
            .OrderByDescending(i => i.Fecha)   // más recientes primero
            .Take(5);                           // solamente 5

        foreach (var inter in ultimasInteracciones)
        {
            panel.AppendLine($"• {inter.Fecha:dd/MM/yyyy HH:mm} - {inter.Tema}");
        }
        panel.AppendLine();
        panel.AppendLine();

        // ==============================
        // PRÓXIMAS REUNIONES
        // ==============================

        panel.AppendLine("--Próximas Reuniones--");

        List<Reuniones> reuniones = new List<Reuniones>();
        foreach (Interaccion interaccion in u.ListaInteracciones)
        {
            if (interaccion is Reuniones reunion)
                reuniones.Add(reunion);
        }
        
      
        List<Reuniones> reunionesFuturas = new List<Reuniones>();

        foreach (var reunion in reuniones)
        {
            if (reunion.Fecha > DateTime.Now)
            {
                reunionesFuturas.Add(reunion);
            }
        }
        
        reunionesFuturas.Sort((a, b) => a.Fecha.CompareTo(b.Fecha));


        if (reunionesFuturas.Count == 0)
        {
            panel.AppendLine("No hay reuniones próximas.");
        }
        else
        {
            foreach (Reuniones reunionFutura in reunionesFuturas)
            {
                panel.AppendLine(
                    $"Reunion con {reunionFutura.Receptor.Nombre} {reunionFutura.Receptor.Apellido} en {reunionFutura.Lugar} a las {reunionFutura.Fecha:dd/MM/yyyy HH:mm}");
            }
        }
        
        
        
        return panel.ToString();
    }
}