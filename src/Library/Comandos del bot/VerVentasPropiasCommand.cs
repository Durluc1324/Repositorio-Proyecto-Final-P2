using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary;
using Discord.Commands;
namespace Library.Comandos_del_bot;

public class VerVentasPropiasCommand: ModuleBase<SocketCommandContext>
{
    private readonly SessionService sessions;

    public VerVentasPropiasCommand(SessionService sessions)
    {
        this.sessions = sessions;
    }

    [Command("verventaspropias")]
    public async Task VerVentasPropiasCommandAsync()
    {
        Usuario vendedor = sessions.GetUsuario(Context.User.Id);
        if (vendedor == null)
        {
            await ReplyAsync("Debes iniciar sesión primero con `!login`.");
            return;
        }

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("--------------------------------------------------");
        sb.AppendLine($"Ventas realizadas por {vendedor.Nombre} {vendedor.Apellido}");
        sb.AppendLine("--------------------------------------------------");

        foreach (Venta venta in vendedor.ListaVentas)
        {
            sb.AppendLine("----------------------------");
            sb.AppendLine($"Comprador {venta.ClienteComprador.Nombre} {venta.ClienteComprador.Apellido}");
            foreach (var producto in venta.Productos)
            {
                sb.AppendLine($"Producto:{producto.Key.Nombre}");
                sb.AppendLine($"Precio: ${producto.Key.Precio}");
                sb.AppendLine($"Cantidad: {producto.Value}");
                sb.AppendLine($"Subtotal: {producto.Key.Precio * producto.Value}");
            }
        }
        sb.AppendLine("----------------------------");

        await ReplyAsync(sb.ToString());
    }
}
//a