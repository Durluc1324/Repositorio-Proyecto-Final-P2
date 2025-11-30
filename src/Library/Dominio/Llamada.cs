using System;

namespace ClassLibrary;

public class Llamada: Interaccion, IRespondible
{
    public string NumeroEmisor { get; private set; }
    public string NumeroReceptor { get; private set; }
    public bool Respondido { get; private set; }

    public Llamada(Persona emisor, Persona receptor, DateTime fecha, string tema) : base(emisor, receptor, fecha, tema)
    {
        NumeroEmisor = emisor.Telefono;
        NumeroReceptor = receptor.Telefono;
      
        MarcarComoRespondido();
    }
    
    public void MarcarComoRespondido()
    {
        Respondido = true;
    }

    public void MarcarComoNoRespondido()
    {
        //Está vacío porque viola ISP, crear una interfaz extra para este método me atrasa respecto
        //al trabajo principal que es terminar el proyecto . _.
    }

    public bool EsRespuestaDe(IRespondible otra)
    {
        if (otra is Llamada otraLlamada)
        {
            return otraLlamada.Emisor == this.Receptor &&
                   otraLlamada.Receptor == this.Emisor &&
                   otraLlamada.Fecha > this.Fecha;
        }

        return false;
    }
}