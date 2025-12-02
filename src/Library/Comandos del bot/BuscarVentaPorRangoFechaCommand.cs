using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary;
using ClassLibrary.Excepciones;
using Discord.Commands;
namespace Library.Comandos_del_bot;

public class BuscarVentaPorRangoFechaCommand: ModuleBase<SocketCommandContext>
{
    private readonly SessionService sessions;

    public BuscarVentaPorRangoFechaCommand(SessionService session)
    {
        this.sessions = session;
    }

    [Command("buscarventaporrangofecha")]
    public async Task BuscarVentaPorRangoFechaCommandAsync(string inicioVenta, string finVenta)
    {
        Usuario vendedor = sessions.GetUsuario(Context.User.Id);
        if (vendedor == null)
        {
            await ReplyAsync("Debes iniciar sesión primero con `!login`.");
            return;
        }

        if (string.IsNullOrEmpty(inicioVenta) || string.IsNullOrEmpty(finVenta))
        {
            await ReplyAsync("Formato incorrecto: debe usar '!buscarventaporrangofecha dd:mm:yyyy dd:mm:yyyy' siendo la primera fecha el inicio y la segunda el final del periodo");
            return;
        }
        
        string[] inicioPeriodoVenta = inicioVenta.Split(":");
        
        if (inicioPeriodoVenta.Length != 3 ||
            !int.TryParse(inicioPeriodoVenta[0], out int dia) ||
            !int.TryParse(inicioPeriodoVenta[1], out int mes) ||
            !int.TryParse(inicioPeriodoVenta[2], out int año))
        {
            await ReplyAsync("La fecha debe tener formato dd:mm:yyyy");
            return;
        }

        DateTime fechaInicio;

        try
        {
            fechaInicio = new DateTime(año, mes, dia);
        }
        catch
        {
            await ReplyAsync("La fecha ingresada no es válida.");
            return;
        }
        
        string[] finPeriodoVenta = finVenta.Split(":");

        if (finPeriodoVenta.Length != 3 ||
            !int.TryParse(finPeriodoVenta[0], out int dia1) ||
            !int.TryParse(finPeriodoVenta[1], out int mes1) ||
            !int.TryParse(finPeriodoVenta[2], out int año1))
        {
            await ReplyAsync("La fecha debe tener formato dd:mm:yyyy");
            return;
        }


        DateTime fechaFin;

        try
        {
            fechaFin = new DateTime(año1, mes1, dia1);
        }
        catch
        {
            await ReplyAsync("La fecha ingresada no es válida.");
            return;
        }

        var ventas = Fachada.FachadaSistema.DelegarObtenerVentasPeriodo(vendedor, fechaInicio, fechaFin);

        if (ventas.Count == 0)
        {
            await ReplyAsync("No tienes ventas registradas en ese período.");
            return;
        }
        
       

        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"Ventas entre **{fechaInicio:dd/MM/yyyy}** y **{fechaFin:dd/MM/yyyy}**:");
        sb.AppendLine("--------------------------------------------------");

        double total = 0;

        foreach (var venta in ventas)
        {
            sb.AppendLine($"{venta.Fecha:dd/MM/yyyy} | Cliente: {venta.ClienteComprador.Nombre} | Total: ${venta.Total}");
            total += venta.Total;
        }

        sb.AppendLine("--------------------------------------------------");
        sb.AppendLine($"**Total generado:** ${total}");

        await ReplyAsync(sb.ToString());

    }
}