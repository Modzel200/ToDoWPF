using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ToDo.Components;
using ToDo.Models;
using ToDo.Services;

namespace ToDo.Pages
{
    public partial class TasksPage : Page
    {
        private DbService _dbService;
        private UserService _userService;
        private TaskService _taskService;
        private readonly ProjectService _projectService;
        private int _projectId;
        private Action reloadProjects;

        public TasksPage(int projectId, Action reloadProjects)
        {
            InitializeComponent();
            _dbService = new DbService();
            var dbContext = _dbService.Context();
            _userService = new UserService(dbContext);
            _projectService = new ProjectService(dbContext, _userService);
            _taskService = new TaskService(dbContext, _userService, _projectService);
            _projectId = projectId;
            this.reloadProjects = reloadProjects;
            LoadTasks();
        }

        private void LoadTasks()
        {
            var tasks = _taskService.GetAllTasks(_projectId);
            // Since adding children manually, have to clear before rendering again
            TaskStackPanel.Children.Clear();
            foreach (var task in tasks)
            {
                AddTaskToPanel(task);
            }
        }

        private void AddTaskToPanel(TaskDto task)
        {
            var taskBorder = (DataTemplate)FindResource("TaskItemTemplate");
            var taskContent = taskBorder.LoadContent() as FrameworkElement;
            var contentControl = new ContentControl { Content = taskContent };
            contentControl.DataContext = task;
            TaskStackPanel.Children.Add(contentControl);
        }


        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            AddTaskWindow addTaskWindow = new AddTaskWindow(_projectId);
            addTaskWindow.Closed += AddTaskWindow_Closed;
            addTaskWindow.Show();
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
                        _taskService.DeleteTask(Id);
                        LoadTasks();
                    }   
                }   
        }

        private void CheckBox_ToggleDone(object sender, RoutedEventArgs e)
        {
            var source = e.OriginalSource as DependencyObject;
            var checkbox = FindAncestor<CheckBox>(source);

            if (checkbox != null && checkbox.Tag is int Id)
            {
                _taskService.ToggleDone(Id);
                LoadTasks();
                reloadProjects.Invoke();
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

        private void AddTaskWindow_Closed(object sender, EventArgs e)
        {
            LoadTasks();
        }

        private void SummaryButton_Click(object sender, RoutedEventArgs e)
        {
            var source = e.OriginalSource as DependencyObject;

            var button = FindAncestor<Components.IconButton>(source);


            if (button != null && button.Tag is int Id)
            {

                NavigationService.Navigate(new SubtasksPage(Id, this));
            }
        }
    }
}
