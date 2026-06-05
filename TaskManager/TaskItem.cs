using System;
using System.ComponentModel;

namespace TaskManager
{
    public class TaskItem : INotifyPropertyChanged
    {
        private bool _isCompleted;

        public string Title { get; set; }
        public DateTime CreatedAt { get; set; }

        public bool IsCompleted
        {
            get => _isCompleted;
            set
            {
                if (_isCompleted != value)
                {
                    _isCompleted = value;
                    OnPropertyChanged(nameof(IsCompleted));
                }
            }
        }

        public TaskItem(string title)
        {
            Title = title;
            CreatedAt = DateTime.Now;
            IsCompleted = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
