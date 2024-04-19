using Microsoft.EntityFrameworkCore;
using System.Security.Principal;
using System.Text;
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
using ToDo.Services;

namespace ToDo;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private UserService _userService;

    public MainWindow()
    {
        InitializeComponent();
    }


    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        _userService = new UserService();
        _userService.loadDatabase();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        if(addUserForm.Visibility == System.Windows.Visibility.Collapsed)
        {
            addUserForm.Visibility = System.Windows.Visibility.Visible;
        }
        else
        {
            addUserForm.Visibility = System.Windows.Visibility.Collapsed;
        }
    }

    private void ButtonAddUser(object sender, RoutedEventArgs e)
    {
        _userService.addUser(userRegister.Text, int.Parse(userRegisterPin.Text));
    }

    private void ButtonLogin(object sender, RoutedEventArgs e)
    {
        if(_userService.loginUser(userLogin.Text, int.Parse(userPin.Text)))
        {
            isLogged.Text = "Udało się zalogować";
        }
        else
        {
            isLogged.Text = "Nie powodzenie";
        }
    }
}