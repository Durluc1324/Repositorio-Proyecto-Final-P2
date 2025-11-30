using System;

namespace ClassLibrary;

public class Reuniones : Interaccion
{
    public string Lugar { get; set; }
    
    public Reuniones(Persona emisor, Persona receptor, DateTime fecha, string tema, string lugar) : base(emisor, receptor,
        fecha, tema)
    {
        Lugar = lugar;
    }
    
}