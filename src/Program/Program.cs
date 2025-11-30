//--------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Universidad Católica del Uruguay">
//     Copyright (c) Programación II. Derechos reservados.
// </copyright>
//--------------------------------------------------------------------------------

using System;
using ClassLibrary;

namespace ConsoleApplication
{
    class Program
    {
        static void Main()
        {
            Administrador admin = new Administrador("Luciano", "Rodriguez", "luciano.rodriguez@gmail.com",
                "0938414342");
            
            AdministrarClientes.Instancia.CrearCliente(admin, "Tom", "Rodriguez", "tom.rodr@gmail.com",
                "92847143", "Hombre", new DateTime(2006, 02, 27));
        }
    }
}