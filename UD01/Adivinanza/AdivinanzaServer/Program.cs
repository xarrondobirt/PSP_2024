using System.IO.Pipes;

namespace AdivinanzaServer
{
    class Program
    {
        private static List<Adivinanza> adivinanzas = new List<Adivinanza>();
        static void Main(string[] args)
        {
            try
            {
                // Leer el XML con las adivinanzas
                string pathXML = @"..\..\..\..\AdivinanzaServer\adivinanzas_short.xml";
                // string pathXML = @"..\..\..\..\AdivinanzaServer\adivinanzas.xml";
                adivinanzas = Adivinanza.LeerXML(pathXML);

                // Realizar conexión
                var server = new NamedPipeServerStream("PSP_Pipes");
                server.WaitForConnection();
                Console.WriteLine("Conexión a servidor establecida.");
                Console.WriteLine("Pipe Servidor esperando datos.");
                StreamReader reader = new StreamReader(server);
                StreamWriter writer = new StreamWriter(server);
                Random rnd = new Random();

                // Índice aleatorio
                int indice = rnd.Next(0, adivinanzas.Count);
                while (true)
                {
                    // Enviar enunciado y leer respuesta del cliente
                    Adivinanza adivinanzaActual = adivinanzas[indice];
                    string enunciadoActual = adivinanzaActual.enunciado;
                    string respuestaActual = adivinanzaActual.respuesta;
                    Console.WriteLine(adivinanzas.Count);
                    Console.WriteLine("Enviado:P " + enunciadoActual);
                    writer.WriteLine(enunciadoActual);
                    writer.Flush();
                    string respuesta = reader.ReadLine();
                    Console.WriteLine("Recibido: " + respuesta);
                    Console.WriteLine("Respuesta real: " + respuestaActual);

                    // Si ha acertado
                    if (respuesta.ToLower() == respuestaActual.ToLower())
                    {
                        Console.WriteLine("Enviado: R OK");
                        Console.WriteLine("Borrando adivinanza de la lista: " + enunciadoActual);

                        // Borrar la adivinanza de la lista
                        adivinanzas.RemoveAt(indice);

                        // Mientras haya adivinanzas en la lista
                        if (adivinanzas.Count > 0)
                        {
                            Console.WriteLine("Número de adivinanzas disponibles: " + adivinanzas.Count);
                            writer.WriteLine("OK");
                            writer.Flush();

                            // Nuevo índice aleatorio
                            indice = rnd.Next(0, adivinanzas.Count);
                        }

                        // Si ha acertado y no hay más adivinanzas el juego termina
                        else
                        {
                            Console.WriteLine("Enviado: R FIN");
                            writer.WriteLine("FIN");
                            writer.Flush();
                        }

                    }

                    // Si no ha acertado
                    else
                    {
                        Console.WriteLine("Enviado: R KO");
                        writer.WriteLine("KO");
                        writer.Flush();

                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}  Apangado servidor por error", e.Message);
            }

        }

    }
}