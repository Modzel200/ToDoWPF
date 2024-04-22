using System;
using System.Collections.Generic;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ToDo.Services;

namespace ToDo.Pages
{
    /// <summary>
    /// Interaction logic for UserPage.xaml
    /// </summary>
    public partial class UserPage : Page
    {
        private DbService _dbService;
        private UserService _userService;
        public UserPage()
        {
            InitializeComponent();
            _dbService = new DbService();
            var dbContext = _dbService.Context();
            _userService = new UserService(dbContext);
            RenderInputs();
        }

        private void ButtonEdit(object sender, RoutedEventArgs e)
        {
            string newUsername = userEdit.Text;
            int newPin;
            if (!int.TryParse(pinEdit.Text, out newPin))
            {
                MessageBox.Show("Please enter a valid PIN.");
                return;
            }

            bool editSuccess = _userService.editUser(newUsername, newPin);

            if (editSuccess)
            {
                usernameInUse.Visibility = Visibility.Collapsed;
                MessageBox.Show("User details updated successfully.");
                RenderInputs();
            }
            else
            {
                usernameInUse.Visibility = Visibility.Visible;
            }
        }

        private void ButtonRefresh(object sender, RoutedEventArgs e)
        {
            RenderInputs();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void RenderInputs()
        {
            var userInfo = _userService.GetUser();
            userEdit.Text = userInfo.Username;
            pinEdit.Text = userInfo.Pin.ToString();
        }
    }
}
