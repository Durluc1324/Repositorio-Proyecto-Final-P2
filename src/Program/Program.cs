using System;
using ClassLibrary;
using Microsoft.Extensions.DependencyInjection;

namespace ClassLibrary
{
    class Program
    {
        static void Main()
        {

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