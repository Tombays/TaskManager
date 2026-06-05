using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace TaskManager
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<TaskItem> _tasks = new ObservableCollection<TaskItem>();

        public MainWindow()
        {
            InitializeComponent();
            TaskListView.ItemsSource = _tasks;
            _tasks.CollectionChanged += (s, e) => UpdateStats();
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            AddTask();
        }

        private void TaskInputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                AddTask();
        }

        private void AddTask()
        {
            var text = TaskInputBox.Text?.Trim();
            if (string.IsNullOrEmpty(text)) return;

            var task = new TaskItem(text);
            task.PropertyChanged += (s, e) => UpdateStats();
            _tasks.Add(task);

            TaskInputBox.Clear();
            TaskInputBox.Focus();

            EmptyState.Visibility = Visibility.Collapsed;
            UpdateStats();

            // Scroll to new item
            TaskListView.ScrollIntoView(task);
        }

        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.Button btn && btn.Tag is TaskItem task)
            {
                _tasks.Remove(task);
                EmptyState.Visibility = _tasks.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
                UpdateStats();
            }
        }

        private void UpdateStats()
        {
            int total = _tasks.Count;
            int done = _tasks.Count(t => t.IsCompleted);
            int active = total - done;

            TotalCount.Text = total.ToString();
            ActiveCount.Text = active.ToString();
            DoneCount.Text = done.ToString();

            // Update progress bar
            double progressWidth = total > 0
                ? (done / (double)total) * (ActualWidth - 56)
                : 0;

            // Clamp to container width
            var containerWidth = ActualWidth - 56;
            if (containerWidth > 0 && total > 0)
            {
                ProgressBar.Width = Math.Max(0, (done / (double)total) * containerWidth);
                ProgressText.Text = $"{done} из {total} выполнено";
            }
            else
            {
                ProgressBar.Width = 0;
                ProgressText.Text = "";
            }
        }
    }
}
