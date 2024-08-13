using System;
using System.Collections.Generic;
using System.Media;
using System.Threading;

namespace CaYaChat
{
    public class TimerService
    {
        private static readonly Dictionary<string, System.Threading.Timer> _timers = new Dictionary<string, System.Threading.Timer>();
        private static readonly object _lock = new object();

        // Alarm ses dosyası
        private static readonly SoundPlayer _soundPlayer = new SoundPlayer(Properties.Resources.Alarm);

        public static void StartTimer(string id, int minutes, Action callback)
        {
            lock (_lock)
            {
                // Timer varsa, öncekini iptal et
                if (_timers.ContainsKey(id))
                {
                    _timers[id].Dispose();
                    _timers.Remove(id);
                }

                // Yeni bir timer oluştur ve kaydet
                var timer = new System.Threading.Timer(state =>
                {         
                    // Geri çağırmayı çalıştır
                    callback();

                    // Ses çal
                    PlayAlarmSound();

                    lock (_lock)
                    {
                        _timers.Remove(id);
                    }
                }, null, TimeSpan.FromMinutes(minutes), Timeout.InfiniteTimeSpan);

                _timers[id] = timer;
            }
        }

        public static void StopTimer(string id)
        {
            lock (_lock)
            {
                if (_timers.ContainsKey(id))
                {
                    _timers[id].Dispose();
                    _timers.Remove(id);
                }
            }
        }

        public static bool TimerExists(string id)
        {
            lock (_lock)
            {
                return _timers.ContainsKey(id);
            }
        }

        private static void PlayAlarmSound()
        {
            try
            {
                _soundPlayer.Play();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Alarm sesini çalarken bir hata oluştu: " + ex.Message);
            }
        }
    }
}
