// See https://aka.ms/new-console-template for more information
using DocuTrackBST.Controllers;

namespace DocuTrackBST
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ArbolController controlador = new ArbolController();
            controlador.IniciarSimulacion();
        }
    }
}