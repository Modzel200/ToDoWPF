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
        private Action revalidateRoute;
        private Page currentPage;
        public ContentPage(Action RenderPage)
        {
            InitializeComponent();
            _dbService = new DbService();
            var dbContext = _dbService.Context();
            _userService = new UserService(dbContext);
            this.revalidateRoute = RenderPage;
            contentFrame.Navigate(new ProjectsPage());
            currentPage = new ProjectsPage();
            SetActiveLink(currentPage.GetType());
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

        private void Settings_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Settings Clicked");
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
