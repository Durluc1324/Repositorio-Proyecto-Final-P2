namespace ClassLibrary;

public class Producto
{
    public string Nombre { get; private set; }
    public double Precio { get; private set; }

    public Producto(string nombre, double precio)
    {
        Nombre = nombre;
        Precio = precio;
    }
}