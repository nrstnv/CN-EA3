using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Aufgabe1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("[Press Esc to stop the teleprompter, '<' or '-' to slow down and '>' or '+' to speed up.]\n");
            StartTeleprompter().Wait();
            Console.WriteLine("\n\n[Press Return to exit.]");
            Console.ReadLine();
        }



        /// <summary>
        /// Enumeratormethode zum Einlesen der Textdatei.
        /// 
        /// Diese Methode liest die Datei zeilenweise ein und gibt die einzelnen Wörter mit »yield return«
        /// zurück.Falls eine Zeile mehr als 70 Zeichen enthält, dann soll ein zusätzlicher Zeilenumbruch
        /// mit »yield return Environment.NewLine« hinzugefügt werden.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        static IEnumerable<string> ReadFrom(string file)
        {
            string lineOfText;            
            using (var reader = File.OpenText(file))
            {
                while ((lineOfText = reader.ReadLine()) != null)
                {
                    IEnumerable<string> words = lineOfText.Split(" ");
                    int symbols = 0;
                    bool skipNewLine = false;
                    foreach (string word in words)
                    {
                        yield return word + " ";
                        symbols += word.Length + 1;
                        if (symbols > 70)
                        {
                            yield return Environment.NewLine;
                            symbols = 0;
                            skipNewLine = true;
                        }
                    }
                    if (!skipNewLine)
                        yield return Environment.NewLine;
                }
            }
        }


        /// <summary>
        /// Methode zum Ändern der Geschwindigkeit und zum Beenden der Wiedergabe.
        /// 
        /// Solange die Eigenschaft Done des übergebenen Config-Objekts false ist, soll diese taskbasierte
        /// asynchrone Methode die Tastatureingaben des Nutzers entgegennehmen und verarbeiten.
        /// Falls der Nutzer eine der Tasten „>“ oder „+“ drückt, dann soll der Wert DelayInMilliseconds
        /// um 100 Millisekunden erhöht werden und falls der Nutzer eine der Tasten „<“ oder „-“ drückt,
        /// dann soll der Wert DelayInMilliseconds um 100 Millisekunden verringert werden.
        /// Falls der Nutzer die Escape-Taste drückt, dann soll die Eigenschaft Done auf »true« gesetzt und
        /// dieser Task beendet werden.
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        private static async Task GetInputAsync(Config config)
        {
            Action getInput = () =>
            {
                do
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    if (key.KeyChar == '>' || key.KeyChar == '-')
                    {
                        config.UpdateDelay(-100);
                        Console.Write("[" + config.DelayInMilliseconds + "ms] ");
                    }
                    else if (key.KeyChar == '<' || key.KeyChar == '+')
                    {
                        config.UpdateDelay(100);
                        Console.Write("[" + config.DelayInMilliseconds + "ms] ");
                    }
                    else if (key.Key == ConsoleKey.Escape)
                    {
                        config.SetDone();
                    }
                } while (!config.Done);
                
            };
            await Task.Run(getInput);
            Console.WriteLine("\n\nTask GetInputAsync completed.");
        }


        /// <summary>
        /// Methode zur Textausgabe in die Konsole.
        /// 
        /// Diese Methode soll den Text aus der Datei einlesen und dann Wort für Wort in der Konsole
        /// ausgeben.Nach jedem Wort soll dabei eine Pause von DelayInMilliseconds gemacht werden.
        /// Falls die gesamte Textdatei ausgegeben wurde, wird die Eigenschaft Done auf »true« gesetzt und
        /// die Methode beendet.
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        private static async Task ShowTextAsync(Config config)
        {
            string filePath = Path.Combine(Directory
                .GetParent(Directory.GetCurrentDirectory())
                .Parent
                .Parent
                .FullName, @"Data\Quotes.txt");

            var lines = ReadFrom(filePath);            
            foreach (var line in lines)
            {
                Console.Write(line);
                if (!string.IsNullOrWhiteSpace(line))
                {
                    await Task.Delay(config.DelayInMilliseconds);
                }
                if (config.Done)
                {
                    break;
                }
            }
            if (!config.Done)
                config.SetDone();
            Console.WriteLine("\n\nTask ShowTextAsync completed.");
        }


        /// <summary>
        /// Methode zum Starten des Teleprompters.
        /// 
        /// Innerhalb dieser Methode wird ein neues Config-Objekt erzeugt und an die beiden Methoden
        /// GetInputAsync und ShowTextAsync übergeben. Der Rückgabewert dieser Methode ist ein
        /// neuer Task, der auf die anderen beiden Tasks wartet.
        /// </summary>
        /// <returns></returns>
        private static async Task StartTeleprompter()
        {
            Config config = new Config();
            Task displayTask = ShowTextAsync(config);
            Task speedTask = GetInputAsync(config);

            await Task.WhenAll(displayTask, speedTask);
        }

    }
}
