using System.Threading;
using static System.Math;

namespace Aufgabe1
{
    /// <summary>
    /// Konfiguration des Teleprompters
    /// </summary>
    class Config
    {
        readonly object configMonitor = new object();
        
        public int DelayInMilliseconds { get; private set; }

        public bool Done { get; private set; }

        public Config()
        {
            DelayInMilliseconds = 200; //Anfangswert in 200ms
        }

        /// <summary>
        /// Der Parameter increment kann positiv oder negativ sein, je nachdem ob man die Pause nach
        /// einem Wort verkürzen oder verlängern möchte. Die neue Dauer darf jedoch nicht kleiner als
        /// 100ms und nicht größer als 1000ms werden. Außerdem muss das Ändern des Wertes
        /// THREADSICHER!!! sein.
        /// </summary>
        /// <param name="increment"></param>
        public void UpdateDelay(int increment)
        {
            lock (configMonitor)
            {
                int updatedDelay = Min(DelayInMilliseconds + increment, 1000);
                updatedDelay = Max(updatedDelay, 100);
                DelayInMilliseconds = updatedDelay;
            }            
        }

        public void SetDone()
        {
            Done = true;
        }
    }
}
