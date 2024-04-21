using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
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
    private DbService _dbService;
    private UserService _userService;
    private ProjectService _projectService;
    private TaskService _taskService;
    private SubtaskService _subtaskService;

    public MainWindow()
    {
        InitializeComponent();
        Closing += OnWindowClosing;
    }
    private void OnWindowClosing(object sender, CancelEventArgs e)
    {
        var user = _userService.loggedUser();
        if(user != null)
        {
            _userService.logoutUser(user);
        }
    }
    private void LogoutAllUsersDebug()
    {
        var dbContext = _dbService.Context();
        var users = dbContext.Users;
        foreach (var elem in users)
        {
            elem.isLogged = false;
            dbContext.Update(elem);
        }
        dbContext.SaveChanges();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        _dbService = new DbService();
        var dbContext = _dbService.Context();
        _userService = new UserService(dbContext);
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        if(addUserForm.Visibility == System.Windows.Visibility.Collapsed)
        {
            addUserForm.Visibility = System.Windows.Visibility.Visible;
            buttonImg.Image = new BitmapImage(new Uri("pack://application:,,,/Icons/chevrons_right_icon.png"));
        }
        else
        {   
            addUserForm.Visibility = System.Windows.Visibility.Collapsed;
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
        _userService.addUser(userRegister.Text, int.Parse(userRegisterPin.Text));
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
            isLogged.Text = "Udało się zalogować";
        }
        else
        {
            isLogged.Text = "Nie powodzenie";
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