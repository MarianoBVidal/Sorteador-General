namespace SorteadorGeneral;
using System;
using System.IO.Compression;
using System.IO;

public class Program
{
    public static void Main(string[] args)
    {
        //  Inicialización del programa

        int anchoVentana = Console.WindowWidth;
        String barraLateral = new string('=', anchoVentana);

        Console.Clear();
        ConsoleColor colorPrevio = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(barraLateral + "\n");

        String tituloPrograma = "SORTEADOR GENERAL (v0.1)";
        String autorPrograma = "Programa escrito por Mariano B. Vidal";
        String descPrograma = @"
        Su principal función es ordenar nombres (o números, lo que sea) en
        parejas de a dos, de manera aleatoria, y guardarlos en archivos
        de texto con el nombre de cada uno, para facilitar el sorteo en el Amigo Secreto.";

        Console.WriteLine(tituloPrograma.PadLeft(anchoVentana / 2));
        Console.WriteLine(autorPrograma.PadLeft(anchoVentana / 2 + anchoVentana / 24));
        Console.WriteLine(descPrograma);

        //  Lógica del programa

        try
        {
            String[,] nombresEmparejados = EmparejarNombres(args);

            string oldPath = System.AppDomain.CurrentDomain.BaseDirectory;
            string newPath = oldPath + "sorteado\\";
            string filePath;

            Directory.CreateDirectory(newPath);
            Directory.SetCurrentDirectory(newPath);

            //Crear un archivo de texto para cada nombre
            for (int i = 0; i < nombresEmparejados.GetLength(0); i++)
            {
                filePath = newPath + $"para_{nombresEmparejados[i, 0]}.txt";
                CrearArchivo("¡Buenos días! ", "En esta ocasión, tu amigo secreto es: ", 
                            nombresEmparejados[i, 0],
                            nombresEmparejados[i, 1],
                            filePath);
            }

            //Crear un zip con la carpeta 'sorteado'
            Directory.SetCurrentDirectory(oldPath);
            ZipFile.CreateFromDirectory(newPath, oldPath + "sorteado.zip");

            Directory.Delete(newPath, true);

            string msjExito = "La operación se realizó con éxito";
            Console.WriteLine("\n" + msjExito.PadLeft(anchoVentana / 2));
        }
        catch (Exception e)
        {
            String msjError = "ERROR: " + e.Message;
            Console.WriteLine("\n" + msjError.PadLeft(anchoVentana / 2));
        }

        String msjTermino = "\nTerminando programa...";

        Console.WriteLine(msjTermino.PadLeft(anchoVentana / 3));
        Console.WriteLine(barraLateral);
        Console.ForegroundColor = colorPrevio;
    }

    // MÉTODOS

    public static string[,] EmparejarNombres(string[] elementos)
    {
        if (elementos.Length < 2)
            throw new FormatException("Se pasaron menos de dos argumentos");

        //Crear matriz
        string[,] matrizElementos = new string[elementos.Length, 2];

        //Randomizar el orden de los elementos y copiar el primero
        Shuffle<string>(elementos);
        string primerElemento = elementos[0];

        //Enlazar cada elemento con el que le sigue
        for (int i = 0; i < (elementos.Length - 1); i++)
        {
            matrizElementos[i, 0] = elementos[i];
            matrizElementos[i, 1] = elementos[i + 1];
        }

        //Paso final - enlazar el último elemento con el primero
        matrizElementos[elementos.Length - 1, 0] = elementos[elementos.Length - 1];
        matrizElementos[elementos.Length - 1, 1] = primerElemento;

        return matrizElementos;
    }

    public static void Shuffle<T>(T[] array)
    {
        Random rnd = new Random();
        for (int i = array.Length - 1; i >= 0; i--)
        {
            int index = rnd.Next(i);
            T temp = array[index];
            array[index] = array[i];
            array[i] = temp;
        }
    }

    public static void CrearArchivo(string msj1, string msj2, string destinatario, string receptor, string filePath)
    {
        using (StreamWriter sw = new StreamWriter(filePath, false))
        {
            sw.WriteLine($"{msj1} {destinatario}");
            sw.WriteLine($"{msj2} {receptor}");
        }
    }
}