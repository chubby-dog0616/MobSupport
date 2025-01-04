using MobSupport.Models;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Reactive.Bindings.TinyLinq;
using System.Reactive.Disposables;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using MobSupport.Class;

namespace MobSupport.ViewModel
{
    internal class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private readonly MainModel _model = new();

        public ReactivePropertySlim<string> TimerDisplay { get; }
        public ReactivePropertySlim <bool> IsTimerEidtable { get; }
        public ReadOnlyReactivePropertySlim<bool> IsMemberEditable { get; }
        public ReactivePropertySlim<string> StartOrStopButtonText { get; }
        public ReactivePropertySlim<string> StartOrStopButtonIcon { get; }

        public ReactivePropertySlim<string> MemberName { get; } = new();
        public ReactiveCollection<Member> Members { get; }

        public ReactivePropertySlim<RoleRotator> Rotator { get; }

        public ReactiveCommandSlim StartTimer { get; private set; } = new();
        public ReactiveCommandSlim<string> SetTimer { get; private set; } = new();
        public ReactiveCommandSlim ResetTimer {  get; private set; } = new();

        public ReactiveCommandSlim AddMember { get; } = new();

        public ReactiveCommandSlim ResetMember { get; } = new();

        public ReactiveCommandSlim ShuffleRole { get; } = new();

        public ReactiveCommandSlim<int> RemoveMember { get; } = new();

        private readonly CompositeDisposable disposables = [];

        public MainViewModel()
        {
            TimerDisplay = _model.TimerDisplay.AddTo(disposables);
            IsTimerEidtable = _model.IsTimerRunning.AddTo(disposables);
            IsMemberEditable = IsTimerEidtable.Select(x => !x).ToReadOnlyReactivePropertySlim().AddTo(disposables);

            StartOrStopButtonText = _model.StartOrStopButtonText.AddTo(disposables);
            StartOrStopButtonIcon = _model.StartOrStopButtonIcon.AddTo(disposables);
            Rotator = _model.Rotator.AddTo(disposables);

            Members = _model.Members.AddTo(disposables);

            StartTimer.Subscribe(_ => _model.StartOrStopTimer());
            SetTimer.Subscribe(value => _model.SetTimer(value));
            ResetTimer.Subscribe(_ => _model.ResetTimer());
            AddMember.Subscribe(_ => {
                _model.AddMember(MemberName.Value);
                MemberName.Value = string.Empty;
            });
            ResetMember.Subscribe(_ => { _model.ResetMembers(); });
            ShuffleRole.Subscribe(_ => { _model.ShuffleRole(); });
            RemoveMember.Subscribe(id => { _model.RemoveMember(id); });
            
        }
    }

    
}
