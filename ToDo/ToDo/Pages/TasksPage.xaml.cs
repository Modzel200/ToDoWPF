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
    /// Logika interakcji dla klasy TasksPage.xaml
    /// </summary>
    public partial class TasksPage : Page
    {
        private DbService _dbService;
        private UserService _userService;
        private TaskService _taskService;
        private int _projectId;
        public TasksPage(int projectId)
        {
            InitializeComponent();
            _dbService = new DbService();
            var dbContext = _dbService.Context();
            _userService = new UserService(dbContext);
            _taskService = new TaskService(dbContext, _userService);
            _projectId = projectId;
            LoadTasks();
        }

        private void LoadTasks()
        {
            var tasks = _taskService.GetAllTasks(_projectId);
            TaskListBox.ItemsSource = tasks;
        }

        private void TaskListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedTask = TaskListBox.SelectedItem as TaskDto;
            if (selectedTask != null)
            {
                DisplayTaskDetails(selectedTask);
            }
        }

        private void DisplayTaskDetails(TaskDto task)
        {
            TaskDetailsTextBlock.Text = $"Name: {task.Name}\n" +
                                           $"Description: {task.Description}\n" +
                                           $"Deadline: {task.DeadLine}\n" +
                                           $"Color: {task.Category}\n" +
                                           $"Is Done: {task.PriorityLevel}\n" +
                                           $"Done Ratio: {task.DoneRatio}";
        }

        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            AddTaskWindow addTaskWindow = new AddTaskWindow(_projectId);
            addTaskWindow.Closed += AddTaskWindow_Closed;
            addTaskWindow.Show();
        }

        private void DeleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedTask = TaskListBox.SelectedItem as TaskDto;
            if (selectedTask == null)
            {
                return;
            }

            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this task?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                TaskListBox.SelectedIndex = 0;
                _taskService.DeleteTask(selectedTask.Id);
                LoadTasks();
            }
        }

        private void AddTaskWindow_Closed(object sender, EventArgs e)
        {
            LoadTasks();
        }
    }
}
