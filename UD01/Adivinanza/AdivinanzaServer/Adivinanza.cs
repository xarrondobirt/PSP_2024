using System.Xml;

namespace AdivinanzaServer
{
    public class Adivinanza
    {

        public string enunciado { get; set; }
        public string respuesta { get; set; }

        public Adivinanza(string enunciado, string respuesta)
        {
            this.enunciado = enunciado;
            this.respuesta = respuesta;
        }

        public static List<Adivinanza> LeerXML(string filePath)
        {
            try
            {
                // Crea una lista para almacenar las adivinanzas
                var adivinanzas = new List<Adivinanza>();

                // Crea un XmlDocument y carga el archivo XML
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(filePath);

                // Itera a través de los elementos <adivinanza> en el archivo XML
                foreach (XmlNode node in xmlDoc.DocumentElement.ChildNodes)
                {
                    // Lee el enunciado y la respuesta de cada adivinanza
                    string enunciado = node["enunciado"].InnerText;
                    string respuesta = node["respuesta"].InnerText;

                    // Crea un objeto Adivinanza y agregar a la lista
                    var adivinanza = new Adivinanza(enunciado, respuesta);
                    adivinanzas.Add(adivinanza);
                }

                // Convierte la lista a un array y devuélvelo
                return adivinanzas;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al leer el archivo XML: " + ex.Message);
                return new List<Adivinanza>();
            }
        }
    }
}
