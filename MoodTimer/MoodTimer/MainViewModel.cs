using System;
using System.ComponentModel;
using System.Timers;
using System.Windows.Input;
using Xamarin.Forms;

namespace MoodTimer
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private bool _isRunning;
        private int _minutes;
        private int _seconds;
        private Timer _timer;
        private int _mood;

        public MainViewModel()
        {
            StartCmd = new Command(() => OnStart(), () => CanExecuteStart);
            StopCmd = new Command(() => OnStop(), () => CanExecuteStop);
            _minutes = UserInputtedMinutes = 30;
            _seconds = 60;
            _mood = 60;
            _timer = new Timer();
            _timer.Elapsed += OnIntervalElapsed;
            _timer.Interval = 1000;
            _isRunning = false;
        }

        public int Mood
        {
            get => _mood;
            set
            {
                _mood = value;
                RaisePropertyChanged(nameof(Mood));
            }
        }

        public ICommand StartCmd { get; }

        public ICommand StopCmd { get; }

        public bool CanExecuteStart => IsMinutesEnabled && Minutes > 0;

        public bool CanExecuteStop => _isRunning;

        public bool IsMinutesEnabled => !_isRunning;

        public int Minutes
        {
            get => _minutes;
            set
            {
                _minutes = value > 60 ? 60 : value;
                if (!_isRunning)
                {
                    UserInputtedMinutes = _minutes;
                }
                RaisePropertyChanged(nameof(Minutes));
            }
        }

        private int UserInputtedMinutes
        {
            get => (int)App.Current.Properties["minutes"];
            set => App.Current.Properties["minutes"] = value;
        }

        private void OnStart()
        {
            _timer.Start();
            SwitchCanExecute(true);
        }

        private void OnStop()
        {
            Stop();
        }

        private void OnIntervalElapsed(object sender, ElapsedEventArgs e)
        {
            _seconds--;
            if (_seconds == 0)
            {
                OnMinuteElapsed();
            }            
            else
            {
                double sum = _seconds + ((Minutes - 1) * 60);
                double percentageOfMaxTime = (sum / 3600) * 100;
                double colourFromPercentage = 255 * percentageOfMaxTime / 100;
                double remainderOfColour = 255 - colourFromPercentage;
                double adjusment = remainderOfColour / 2;
                Mood = (int)(colourFromPercentage + adjusment);
            }
        }

        private void OnMinuteElapsed()
        {
            _seconds = 60;
            Minutes--;
            if (Minutes == 0)
            {
                Stop();
            }
        }

        private void Stop()
        {
            _timer.Stop();
            SwitchCanExecute(false);
            Minutes = UserInputtedMinutes;
            Mood = 0;            
        }

        private void SwitchCanExecute(bool state)
        {
            _isRunning = state;
            RaisePropertyChanged(nameof(IsMinutesEnabled));
            Device.BeginInvokeOnMainThread(() =>
            {
                ((Command)StartCmd).ChangeCanExecute();
                ((Command)StopCmd).ChangeCanExecute();
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged(string propertyName)
        {
            var changed = PropertyChanged;
            if (changed != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
