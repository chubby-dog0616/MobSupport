using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace MobSupport.Class
{
    internal class TimerCounter
    {
        private readonly DispatcherTimer _timer;

        private double totalSeconds;
        private double startedSeconds;
        public ReactivePropertySlim<string> TimerDisplay { get; }
        public ReactivePropertySlim<bool> IsRunning { get; }

        public event Action? TimerFinished; 
        public TimerCounter(double seconds)
        {
            _timer = new(DispatcherPriority.Normal)
            {
                Interval = new TimeSpan(0, 0, 0, 1)
            };
            _timer.Tick += Timer_Tick;
            totalSeconds = seconds;
            TimerDisplay = new(Display());
            IsRunning = new(_timer.IsEnabled);
        }

        public void SetTimer(double soconds)
        {
            if (_timer.IsEnabled)
            {
                MessageBox.Show("タイマー作動中に時間を設定することは出来ません");
                return;
            }
            
            if (soconds < 0)
            {
                MessageBox.Show("タイマーの設定が不正です");
                return;
            }
            totalSeconds = soconds;
        }

        public void ResetTimer()
        {
            _timer.Stop();
            totalSeconds = startedSeconds;
            Update();
        } 

        public void Start()
        {
            startedSeconds = totalSeconds;
            _timer.Start();
            Update();
        }

        public void Stop()
        {
            _timer.Stop();
            Update();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            totalSeconds--;

            if ( totalSeconds <= 0)
            {
                PlayAlert();
                ResetTimer();
                TimerFinished?.Invoke();
            }
            Update();
        }

        private string Display()
        {
            return TimeSpan.FromSeconds(totalSeconds).ToString(@"mm\:ss");
        }

        private void Update()
        {
            TimerDisplay.Value = Display();
            IsRunning.Value = _timer.IsEnabled;
        }

        private void PlayAlert()
        {
            const string FILE_PATH = @"pack://application:,,,/Sounds/timeout.mp3";
            SoundPlayer.Play(new Uri(FILE_PATH));
        }
    }
}
