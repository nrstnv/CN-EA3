using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.Runtime.Serialization.Json;

namespace Aufgabe2
{
    /// <summary>
    /// Implementieren Sie die drei Methoden, um die Datenstruktur in Form einer Binärdatei, einer
    /// XML-Datei und einer JSON-Datei zu serialisieren.Innerhalb dieser Methoden soll die generierte
    /// Datei direkt wieder ausgelesen und die Datenstruktur deserialisiert werden.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Warrior[] warriors = new Warrior[] {
                new Warrior() {
                    Name = "Gimli",
                    Weapons = new Weapon[] {
                        new Weapon() { Type = "battleaxe", Range = WeaponRange.Middle },
                        new Weapon() { Type = "knife", Range = WeaponRange.Short }
                    }
                },
                new Warrior() { Name = "Legolas",
                    Weapons = new Weapon[] {
                        new Weapon() { Type = "longbow", Range = WeaponRange.Long },
                        new Weapon() { Type = "knife", Range = WeaponRange.Short }
                    }
                }
            };
            CreateBinaryFile(@"C:\Temp\warriors.dat", warriors);
            CreateXmlFile(@"C:\Temp\warriors.xml", warriors);
            CreateJsonFile(@"C:\Temp\warriors.json", warriors);
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        

        /// <summary>
        /// Methode zum Serialisieren und Deserialisieren einer Datenstruktur
        /// in Form einer Binärdatei
        /// </summary>
        /// <param name="file"></param>
        /// <param name="warriors"></param>        
        private static void CreateBinaryFile(string file, Warrior[] warriors)
        {
            BinaryFormatter binF = new BinaryFormatter();            
            
            //Serialisierung
            using (FileStream fs = new FileStream(file, FileMode.Create))
            {
                binF.Serialize(fs, warriors);
                Console.WriteLine($"Serializing data to file: {file}");
                fs.Close();
            }

            // Deserialisierung
            if (File.Exists(file))
            {
                // Zuerst braucht man einen neuen Stream, der uns die Datei
                // mit den Daten, die wir deserialisieren wollen, aufmacht.
                using (FileStream fs = new FileStream(file, FileMode.Open))
                {
                    Warrior[] binaryObj = (Warrior[])binF.Deserialize(fs);
                    fs.Close();

                    // Hier überprüfen wir, ob die Daten korrekt deserialisiert wurden
                    if (binaryObj != null)
                    {
                        Console.Write($"File {file} contains {binaryObj.Length} warriors: ");
                        for (int index = 0; index < binaryObj.Length; index++)
                        {
                            Console.Write($"{binaryObj[index].Name}" + (index == (binaryObj.Length - 1) ? "" : " and "));
                        }
                    }
                    Console.Write("\n\n");
                }
            }
        }



        /// <summary>
        /// Methode zum Serialisieren und Deserialisieren einer Datenstruktur
        /// in Form einer XML-Datei
        /// </summary>
        /// <param name="file"></param>
        /// <param name="warriors"></param>
        private static void CreateXmlFile(string file, Warrior[] warriors)
        {
            XmlSerializer xmlF = new XmlSerializer(typeof(Warrior[]));

            //Serialisierung
            //Ein Stream, der die Datenstruktur annimmt und in die Datei schreibt
            using (TextWriter writer = new StreamWriter(file))
            {
                xmlF.Serialize(writer, warriors);
                Console.WriteLine($"Serializing data to file: {file}");
                writer.Close();
            }

            // Deserialisierung
            if (File.Exists(file))
            {
                //Ein anderer Stream, der die Datei liest
                using (TextReader reader = new StreamReader(file))
                {
                    Warrior[] xmlObj = (Warrior[])xmlF.Deserialize(reader);
                    reader.Close();

                    if (xmlObj != null)
                    {
                        Console.Write($"File {file} contains {xmlObj.Length} warriors: ");
                        for (int index = 0; index < xmlObj.Length; index++)
                        {
                            Console.Write($"{xmlObj[index].Name}" + (index == (xmlObj.Length - 1) ? "" : " and "));
                        }
                    }
                    Console.Write("\n\n");
                }                
            }      
        }



        /// <summary>
        /// Methode zum Serialisieren und Deserialisieren einer Datenstruktur
        /// in Form einer JSON-Datei
        /// </summary>
        /// <param name="file"></param>
        /// <param name="warriors"></param>
        private static void CreateJsonFile(string file, Warrior[] warriors)
        {
            DataContractJsonSerializer jsonF = new DataContractJsonSerializer(typeof(Warrior[]));

            //Serialisierung
            using (FileStream fs = new FileStream(file, FileMode.OpenOrCreate))
            {
                // Indem wir diese Methode aufrufen, serialisieren wir den Objekt
                // und übermitteln das Ergebnis dem Stream, welcher in seiner Folge
                // sein Inhalt in die Datei schreibt.
                jsonF.WriteObject(fs, warriors);
                Console.WriteLine($"Serializing data to file: {file}");
                fs.Close();
            }

            // Deserialisierung
            using (FileStream fs = new FileStream(file, FileMode.Open))
            {
                // Indem wir diese Methode aufrufen, deserialisieren wir den Inhalt
                // des Stream in neuen Objekt.
                Warrior[] jsonObj = (Warrior[])jsonF.ReadObject(fs);
                fs.Close();
                if (jsonObj != null)
                {
                    Console.Write($"File {file} contains {jsonObj.Length} warriors: ");
                    for (int index = 0; index < jsonObj.Length; index++)
                    {
                        Console.Write($"{jsonObj[index].Name}" + (index == (jsonObj.Length - 1) ? "" : " and "));
                    }
                    Console.Write("\n\n");
                }
            }
        }

    }
}
