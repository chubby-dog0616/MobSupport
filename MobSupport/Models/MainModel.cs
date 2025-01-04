using MobSupport.Class;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace MobSupport.Models
{
    internal class MainModel 
    {
        private const string ICON_START = "Play";
        private const string ICON_STOP = "Stop";
        private const string TEXT_START = "スタート";
        private const string TEXT_STOP = "ストップ";
        /// <summary>
        /// 初期タイマー：15分(900秒)
        /// </summary>
        private static double InitialTime = 900;

        private static Queue<string> Icons = new(NumberStringsGenerator.GenerateShuffledZeroPaddedNumbers(12));

        private readonly TimerCounter _timerCounter;

        public ReactivePropertySlim<string> TimerDisplay { get; }

        public ReactivePropertySlim<bool> IsTimerRunning { get; }

        public ReactivePropertySlim<string> StartOrStopButtonIcon { get; } = new(ICON_START);

        public ReactivePropertySlim<string> StartOrStopButtonText { get; } = new(TEXT_START);

        public ReactiveCollection<Member> Members { get; }
        public ReactivePropertySlim<RoleRotator> Rotator { get; private set; }

        public MainModel()
        {
            _timerCounter = new TimerCounter(InitialTime);
            _timerCounter.TimerFinished += RoleRotate;
            _timerCounter.TimerFinished += MainWindowActivator.ActivateMainWindow;
            TimerDisplay = _timerCounter.TimerDisplay;
            IsTimerRunning = _timerCounter.IsRunning;
            IsTimerRunning.Subscribe(_ => ToggleStartOrStopButtonContent());
            Members = new();
            Rotator = new(new RoleRotator());

        }

        public void StartOrStopTimer()
        {
            if (IsTimerRunning.Value)
            {
                StopTimer();
            }
            else
            {
                StartTimer();
            }
        }

        public void ResetTimer()
        {
            _timerCounter.ResetTimer();
        }

        public void SetTimer(string timer)
        {
            if (IsTimerRunning.Value) { return; }

            if (TimeSpan.TryParseExact(timer, @"mm\:ss", null, out TimeSpan result))
            {
                _timerCounter.SetTimer(result.TotalSeconds);

            }
            else
            {
                MessageBox.Show("タイマーの入力は [mm:ss] 形式で入力してください");
            }
        }

        public void AddMember(string name)
        {
            string iconNum = Icons.Dequeue();
            BitmapImage img = RetriveIconPath(iconNum);
            try
            {
                var member = new Member(Members.Count, name, iconNum, img);
                Members.AddOnScheduler(member);

                Rotator?.Value.AddMember(member);
            }
            catch (ArgumentNullException ex)
            {
                MessageBox.Show("名前を空白にすることはできません");
            }
        }

        public void RemoveMember(int id)
        {
            var target = Members.First(x => x.Id == id);
            Icons.Enqueue(target.IconNumber);
            Members.Remove(target);
            Rotator.Value.RemoveMember(target);
        }

        public void ResetMembers()
        {
            Members.Clear();
            Rotator.Value.Clear();
        }

        public void ShuffleRole()
        {
            Rotator.Value.Shuffle();
        }

        public void Activate()
        {
            
        }

        private void StartTimer()
        {
            _timerCounter.Start();
        }

        private void StopTimer()
        {
            _timerCounter.Stop();
        }

        private void ToggleStartOrStopButtonContent()
        {
            if (IsTimerRunning.Value)
            {
                StartOrStopButtonIcon.Value = ICON_STOP;
                StartOrStopButtonText.Value = TEXT_STOP;
            }
            else
            {
                StartOrStopButtonIcon.Value = ICON_START;
                StartOrStopButtonText.Value = TEXT_START;
                
            }
        }

        private void RoleRotate()
        {
            if (Members.Count > 0)
            {
                Rotator.Value.Rotate();
            }
        }



        private BitmapImage RetriveIconPath(string iconNum)
        {
            string path = $@"pack://application:,,,/Images/icon{iconNum}.png";
            return new BitmapImage(new Uri(path));
        }
    }
}
