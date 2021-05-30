using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Aufgabe3
{
    class Program
    {
        private static BlockingCollection<Robot> robotsWaitinglist;
        const int NumberOfChargingStations = 4;
        const int NumberOfRobots = 10;

        static void Main(string[] args)
        {
            //waiting list of Robots, that can be charged at one moment
            robotsWaitinglist = new BlockingCollection<Robot>(boundedCapacity: NumberOfChargingStations);

            Task produceTask = Task.Run(() => ProduceRobots());
            Task[] consumer = new Task[NumberOfChargingStations];
            for (int i = 0; i < consumer.Length; i++)
            {
                consumer[i] = Task.Run(() => ChargeRobots());
            }
            Task.WaitAll(produceTask, consumer[0], consumer[1], consumer[2], consumer[3]);

            //Alternative Realisierung mithilfe der Parallel-Klasse:
            //Parallel.For(0, NumberOfChargingStations, job =>
            //{
            //    ChargeRobots();
            //});
            //Task.WaitAll(produceTask);
        }


        /// <summary>
        /// Erzeugerprozess
        /// </summary>
        private static void ProduceRobots()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            //Roboter erzeugen
            Random random = new Random(DateTime.Now.Millisecond);
            Robot[] robots = new Robot[NumberOfRobots];
            for (int i = 0; i < NumberOfRobots; i++)
            {
                robots[i] = new Robot
                {
                    Number = i + 1,
                    ChargeTime = random.Next(10, 20),
                    BatteryLifeTime = random.Next(5, 10)
                };
            }

            int counter = 0;

            while (true)
            {
                Thread.Sleep(500);
                // mit .Add wird Thread blockiert, wenn Collection seine Kapazität erreicht hat
                if (stopWatch.ElapsedMilliseconds <= robots[counter].BatteryLifeTime * 1000)
                {
                    robotsWaitinglist.Add(robots[counter]);
                    Console.WriteLine($"Roboter {robots[counter].Number} steht in der Warteschlange.\n");
                }
                else
                {
                    Console.WriteLine($"Roboter {robots[counter].Number} hat keine Energie mehr.\n");
                }
                counter += 1;
                // here wird das Erzeugen von den Roboter beendet
                if (counter >= NumberOfRobots)
                {
                    robotsWaitinglist.CompleteAdding();
                    Console.WriteLine("Der Erzeugerprozess wird beendet.\n");
                    stopWatch.Stop();
                    return;
                }
            }
        }


        /// <summary>
        /// Verbraucherprozesse
        /// </summary>
        private static void ChargeRobots()
        {
            Robot robotOnCharging;
            Thread.Sleep(750);
            while (robotsWaitinglist.IsCompleted != true)
            {
                
                robotOnCharging = robotsWaitinglist.Take();                
                Console.WriteLine("Roboter {0} wird an Ladestation {1} aufgeladen. Ladezeit: {2} Sekunden\n",
                    robotOnCharging.Number,
                    Thread.CurrentThread.ManagedThreadId,
                    robotOnCharging.ChargeTime);
                Thread.Sleep(robotOnCharging.ChargeTime * 1000); // simulate charging time in seconds
                Console.WriteLine($"Roboter {robotOnCharging.Number} ist voll aufgeladen.\n");
            }
            Console.WriteLine($"Ladestation {Thread.CurrentThread.ManagedThreadId} ist fertig.\n");
        }

    }
}
