using System;
using ClassLibrary;
using Microsoft.Extensions.DependencyInjection;

namespace ClassLibrary
{
    class Program
    {
        static void Main()
        {
            Administrador admin = new Administrador("Luciano", "Rodriguez",
                "luciano.rodriguez@gmail.com", "0938414342");

            AdministrarClientes.Instancia.CrearCliente(admin, "Tom", "Rodriguez",
                "tom.rodr@gmail.com", "92847143", "Hombre",
                new DateTime(2006, 02, 27));

            if (AdministrarUsuarios.Instancia.Usuarios().Count == 0)
            {
                AdministrarUsuarios.Instancia.AgregarAdministrador(
                    "admin", "1", "admin@admin.com", "0000", "admin123");
            }

            // Arrancar el bot
            BotLoader.LoadAsync().GetAwaiter().GetResult();
        }
    }
}