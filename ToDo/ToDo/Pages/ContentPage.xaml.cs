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
using ToDo.Entities;
using ToDo.Models;
using ToDo.Services;

namespace ToDo.Pages
{
    /// <summary>
    /// Interaction logic for ContentPage.xaml
    /// </summary>
    public partial class ContentPage : Page
    {
        private DbService _dbService;
        private UserService _userService;
        private ProjectService _projectService;
        private TaskService _taskService;
        private SubtaskService _subtaskService;
        private NotificationService _notificationService;
        private Action revalidateRoute;
        private Page currentPage;
        public ContentPage(Action RenderPage)
        {
            InitializeComponent();
            _dbService = new DbService();
            var dbContext = _dbService.Context();
            _userService = new UserService(dbContext);
            _notificationService = new NotificationService(dbContext, _userService);
            this.revalidateRoute = RenderPage;
            contentFrame.Navigate(new ProjectsPage());
            currentPage = new ProjectsPage();
            SetActiveLink(currentPage.GetType());
        }

        private void LoadNotifications()
        {
            var notifications = _notificationService.GetAllNotifications();
            NotifcationStackPanel.Children.Clear();
            foreach (var notification in notifications)
            {
                AddTaskToPanel(notification);

            }
        }

        private void AddTaskToPanel(NotificationDto subtask)
        {
            var notificationBorder = (DataTemplate)FindResource("NotificationItemTemplate");
            var notificationContent = notificationBorder.LoadContent() as FrameworkElement;
            var contentControl = new ContentControl { Content = notificationContent };
            contentControl.DataContext = subtask;
            NotifcationStackPanel.Children.Add(contentControl);
        }

        private void Logout_MouseDown(object sender, MouseButtonEventArgs e)
        {
            LogoutUser();
        }

        private void Projects_MouseDown(object sender, MouseButtonEventArgs e)
        {
            contentFrame.Navigate(new ProjectsPage());
            currentPage = new ProjectsPage();
            SetActiveLink(currentPage.GetType());
            contentFrame.Navigate(currentPage);
        }

        private void User_MouseDown(object sender, MouseButtonEventArgs e)
        {
            contentFrame.Navigate(new UserPage(revalidateRoute));
            currentPage = new UserPage(revalidateRoute);
            SetActiveLink(currentPage.GetType());
            contentFrame.Navigate(currentPage);
        }

        private void Notifications_Clicked(object sender, RoutedEventArgs e)
        {
            NotificationPopup.IsOpen = !NotificationPopup.IsOpen;
            if(NotificationPopup.IsOpen) LoadNotifications();    
        }

        private void LogoutUser()
        {
            var user = _userService.loggedUser();
            if (user != null)
            {
                _userService.logoutUser(user);
            }
            revalidateRoute?.Invoke();
        }

        private void SetActiveLink(Type pageType)
        {
            ProjectsTextBlock.Foreground = Brushes.White;
            UserTextBlock.Foreground = Brushes.White;

            if (pageType == typeof(ProjectsPage))
                ProjectsTextBlock.Foreground = Brushes.Black;
            else if (pageType == typeof(UserPage))
                UserTextBlock.Foreground = Brushes.Black;

        }
    }
}
