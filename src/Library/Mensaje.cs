using System;

namespace ClassLibrary;

public class Mensaje: Interaccion, IRespondible
{
    public string NumeroEmisor { get; private set; }
    public string NumeroReceptor { get; private set; }
    public bool Respondido { get; private set; }
    
    public Mensaje(Persona emisor, Persona receptor, DateTime fecha, string tema) : base(emisor, receptor, fecha, tema)
    {
        NumeroEmisor = emisor.Telefono;
        NumeroReceptor = receptor.Telefono;
    }
    public void MarcarComoRespondido()
    {
        Respondido = true;
    }

    public void MarcarComoNoRespondido()
    {
        Respondido = false;
    }

    public bool EsRespuestaDe(IRespondible otra)
    {
        if (otra is Mensaje otroMensaje)
        {
            return otroMensaje.Emisor == this.Receptor &&
                   otroMensaje.Receptor == this.Emisor &&
                   otroMensaje.Fecha > this.Fecha;
        }

        return false;
    }
}