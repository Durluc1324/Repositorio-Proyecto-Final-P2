namespace ClassLibrary
{
    public interface IRespondible
    {
        bool Respondido { get; }
        void MarcarComoRespondido();
        void MarcarComoNoRespondido();
        bool EsRespuestaDe(IRespondible otraInteraccion);


    }
}