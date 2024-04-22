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
        public ContentPage(Action RenderPage)
        {
            InitializeComponent();
            _dbService = new DbService();
            var dbContext = _dbService.Context();
            _userService = new UserService(dbContext);
            this.revalidateRoute = RenderPage;
        }

        private void Logout_MouseDown(object sender, MouseButtonEventArgs e)
        {
            LogoutUser();
        }

        private void Projects_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Projects Clicked");
        }

        private void User_MouseDown(object sender, MouseButtonEventArgs e)
        {
            contentFrame.Navigate(new UserPage());
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
    }
}
