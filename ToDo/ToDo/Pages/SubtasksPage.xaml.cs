using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ToDo.Components;
using ToDo.Models;
using ToDo.Services;

namespace ToDo.Pages
{
    /// <summary>
    /// Interaction logic for SubtasksPage.xaml
    /// </summary>
    public partial class SubtasksPage : Page
    {
        private DbService _dbService;
        private UserService _userService;
        private SubtaskService _SubtaskService;
        private readonly Page previousPage;
        private int _taskId;
        public SubtasksPage(int taskId, Page previousPage)
        {
            InitializeComponent();
            this.previousPage = previousPage;
            _dbService = new DbService();
            var dbContext = _dbService.Context();
            _userService = new UserService(dbContext);
            _SubtaskService = new SubtaskService(dbContext, _userService);
            _taskId = taskId;
            LoadSubtasks();
        }

        private void LoadSubtasks()
        {
            var subtasks = _SubtaskService.GetAllSubTasks(_taskId);
            // Since adding children manually, have to clear before rendering again
            SubtaskStackPanel.Children.Clear();
            foreach (var subtask in subtasks)
            {
                AddTaskToPanel(subtask);
            }
        }

        private void AddTaskToPanel(SubTaskDto subtask)
        {
            var taskBorder = (DataTemplate)FindResource("SubtaskItemTemplate");
            var subtaskContent = taskBorder.LoadContent() as FrameworkElement;
            var contentControl = new ContentControl { Content = subtaskContent };
            contentControl.DataContext = subtask;
            SubtaskStackPanel.Children.Add(contentControl);
        }

        private void CheckBox_ToggleDone(object sender, RoutedEventArgs e)
        {
            var source = e.OriginalSource as DependencyObject;
            var checkbox = FindAncestor<CheckBox>(source);

            if (checkbox != null && checkbox.Tag is int Id)
            {
                _SubtaskService.ToggleDone(Id);
                LoadSubtasks();
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (previousPage != null)
            {
                NavigationService.Navigate(previousPage);
            }
        }

        private void AddSubtaskButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            var source = e.OriginalSource as DependencyObject;

            var button = FindAncestor<Components.IconButton>(source);


            if (button != null && button.Tag is int Id)
            {

                MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this task?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    _SubtaskService.DeleteSubTask(Id);
                    LoadSubtasks();
                }
            }

        }

        private T FindAncestor<T>(DependencyObject current)
            where T : DependencyObject
        {
            while (current != null)
            {
                if (current is T)
                {
                    return (T)current;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            return null;
        }
    }
}
