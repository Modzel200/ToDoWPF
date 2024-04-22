using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ToDo.Services;

namespace ToDo.Pages
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        private DbService _dbService;
        private UserService _userService;
        private ProjectService _projectService;
        private TaskService _taskService;
        private SubtaskService _subtaskService;
        private Action revalidateRoute;
        public bool IsRemembered { get; private set; }
        public LoginPage(Action RenderPage)
        {
            InitializeComponent();
            _dbService = new DbService();
            var dbContext = _dbService.Context();
            _userService = new UserService(dbContext);
            addUserForm.Width = 1000;
            this.revalidateRoute = RenderPage;
        }

        private void StayLogged_Checked(object sender, RoutedEventArgs e)
        {
           this.IsRemembered = true;
        }

        private void StayLogged_Unchecked(object sender, RoutedEventArgs e)
        {
            this.IsRemembered = false;
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DoubleAnimation startAnimation = new DoubleAnimation(300, new Duration(TimeSpan.FromSeconds(0.5)));
            DoubleAnimation endAnimation = new DoubleAnimation(1000, new Duration(TimeSpan.FromSeconds(0.5)));

            if (addUserForm.Width == 1000)
            {
                addUserForm.BeginAnimation(WidthProperty, startAnimation);
                addUserForm.Visibility = System.Windows.Visibility.Visible;
                buttonImg.Image = new BitmapImage(new Uri("pack://application:,,,/Icons/chevrons_right_icon.png"));
            }
            else
            {
                addUserForm.BeginAnimation(WidthProperty, endAnimation);
                //addUserForm.Visibility = System.Windows.Visibility.Collapsed;
                buttonImg.Image = new BitmapImage(new Uri("pack://application:,,,/Icons/chevrons_left_icon.png"));
            }
        }

        private void ButtonAddUser(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(userRegister.Text) || string.IsNullOrWhiteSpace(userRegisterPin.Text))
            {
                MessageBox.Show("Username / PIN can't be blank.");
                return;
            }

            if (!_userService.addUser(userRegister.Text, int.Parse(userRegisterPin.Text)))
            {
                loginInUse.Visibility = Visibility.Visible;
            }
            else
            {
                loginInUse.Visibility = Visibility.Collapsed;
                MessageBox.Show("Account created");
                userRegister.Text = "";
                userRegisterPin.Text = "";
            }
        }

        private void ButtonLogin(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(userLogin.Text) || string.IsNullOrWhiteSpace(userPin.Text))
            {
                MessageBox.Show("Username / PIN can't be blank.");
                return;
            }
            if (_userService.loginUser(userLogin.Text, int.Parse(userPin.Text)))
            {
                _projectService = new ProjectService(_dbService.Context(), _userService);
                _taskService = new TaskService(_dbService.Context(), _userService);
                _subtaskService = new SubtaskService(_dbService.Context(), _userService);
                revalidateRoute?.Invoke();
            }
        }

        private void NumberButton_Click(object sender, RoutedEventArgs e)
        {

            var source = e.OriginalSource as DependencyObject;

            var button = FindAncestor<Components.IconButton>(source);

            if (button != null)
            {
                string value = button.Tag?.ToString();

                if (!string.IsNullOrEmpty(value))
                {
                    userPin.Text += value;
                }
            }
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
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
