using System.Diagnostics;
using System.IO.Pipes;

namespace AdivinanzaCliente
{
    class Program
    {

        static void Main(string[] args)
        {
            Process p;
            StartServer(out p);
            Task.Delay(1000).Wait();

            //Preparar conexion del cliente
            var client = new NamedPipeClientStream("PSP_Pipes");
            client.Connect();
            StreamReader reader = new StreamReader(client);
            StreamWriter writer = new StreamWriter(client);

            Console.WriteLine("********************************");
            Console.WriteLine("Comienza el juego");
            Console.WriteLine("********************************");
            Console.WriteLine();

            while (true)
            {
                // Recibir acertijo y enviar respuesta
                Console.WriteLine("Resuelve el siguiente acertijo:");
                Console.WriteLine();
                Console.WriteLine("************Acertijo************");
                string line = reader.ReadLine();
                Console.WriteLine(line);
                Console.WriteLine("********************************");
                Console.WriteLine("Indica la respuesta");
                writer.WriteLine(Console.ReadLine());
                writer.Flush();
                string validar = reader.ReadLine();

                // Si se ha acertado el acertijo
                if (validar == "OK")
                {
                    Console.WriteLine("¡Enhorabuena! Has acertado.");
                    Console.WriteLine("-----***-----");
                    Console.WriteLine();
                    Console.WriteLine("********************************");
                    Console.WriteLine("Juega otra vez");
                    Console.WriteLine("********************************");
                    Console.WriteLine();
                }

                // Si no se ha acertado
                else if (validar == "KO")
                {
                    Console.WriteLine("No has acertado. Introduce otra respuesta:");
                }

                // Si no hay más acertijos
                else if (validar == "FIN")
                {
                    Console.WriteLine("No quedan más adivinanzas.");
                    Console.WriteLine("********************************");
                    Console.WriteLine("FIN DEL JUEGO");
                    Console.WriteLine("********************************");
                    break;

                }
            }
        }

        static Process StartServer(out Process p1)
        {
            // iniciar un proceso con el servidor y devolver
            ProcessStartInfo info = new ProcessStartInfo(@"..\..\..\..\AdivinanzaServer\bin\Release\net6.0\publish\win-x64\AdivinanzaServer.exe");

            // su valor por defecto el false, si se establece a true no se "crea" ventana
            info.CreateNoWindow = false;
            info.WindowStyle = ProcessWindowStyle.Normal;
            // indica si se utiliza el cmd para lanzar el proceso
            info.UseShellExecute = true;
            p1 = Process.Start(info);
            return p1;
        }
    }
}