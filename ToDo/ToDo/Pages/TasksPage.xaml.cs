using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
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
        private TaskFilterSortDto _filterDto;

        public TasksPage(int projectId, Action reloadProjects)
        {
            InitializeComponent();
            _dbService = new DbService();
            var dbContext = _dbService.Context();
            _userService = new UserService(dbContext);
            _projectService = new ProjectService(dbContext, _userService);
            _taskService = new TaskService(dbContext, _userService, _projectService);
            _projectId = projectId;
            _filterDto = new TaskFilterSortDto();
            this.reloadProjects = reloadProjects;
            LoadTasks();
        }

        private void LoadTasks()
        {
            var tasks = _taskService.GetAllTasks(_projectId, _filterDto);
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

        private void ChangeFilterButton_Click(object sender, RoutedEventArgs e)
        {
            switch (_filterDto.CategoryFilter)
            {
                case Entities.Category.Work:
                    FilterButton.Image = new BitmapImage(new Uri("pack://application:,,,/Icons/filter_icon.png"));
                    _filterDto.CategoryFilter = null;
                    LoadTasks();
                    return;
                case Entities.Category.Home:
                    FilterButton.Image = new BitmapImage(new Uri("pack://application:,,,/Icons/suitcase_icon.png"));
                    _filterDto.CategoryFilter = Entities.Category.Work;
                    LoadTasks();
                    return;
                default:
                    FilterButton.Image = new BitmapImage(new Uri("pack://application:,,,/Icons/home_icon.png"));
                    _filterDto.CategoryFilter = Entities.Category.Home;
                    LoadTasks();
                    return;
            };
        }

        private void ChangeSortButton_Click(object sender, RoutedEventArgs e)
        {
            switch (_filterDto.Sort)
            {
                case Entities.PriorityLevel.Not_Important:
                    SortButton.Image = new BitmapImage(new Uri("pack://application:,,,/Icons/sort_filter_icon.png"));
                    _filterDto.Sort = null;
                    LoadTasks();
                    return;
                case Entities.PriorityLevel.Very_Important:
                    SortButton.Image = new BitmapImage(new Uri("pack://application:,,,/Icons/sort_down_icon.png"));
                    _filterDto.Sort = Entities.PriorityLevel.Not_Important;
                    LoadTasks();
                    return;
                default:
                    SortButton.Image = new BitmapImage(new Uri("pack://application:,,,/Icons/sort_up_icon.png"));
                    _filterDto.Sort = Entities.PriorityLevel.Very_Important;
                    LoadTasks();
                    return;
            };
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
