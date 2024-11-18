namespace Servaind.Intranet.Core
{
    public class Filtro
    {
        // Propiedades.
        public int Tipo { get; private set; }
        public object Valor { get; private set; }


        public Filtro(int tipo, object valor)
        {
            Tipo = tipo;
            Valor = valor;
        }
    }
}