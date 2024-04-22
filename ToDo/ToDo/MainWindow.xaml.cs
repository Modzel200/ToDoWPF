using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
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
using ToDo.Entities;
using ToDo.Pages;
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
    private LoginPage loginPage;

    public MainWindow()
    {
        InitializeComponent();
        Closing += OnWindowClosing;
    }
    private void OnWindowClosing(object sender, CancelEventArgs e)
    {
        var user = _userService.loggedUser();
        if(user != null && loginPage != null && !loginPage.IsRemembered)
        {
            _userService.logoutUser(user);
        }
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        _dbService = new DbService();
        var dbContext = _dbService.Context();
        _userService = new UserService(dbContext);
        RenderPage();
    }

    private void RenderPage()
    {
        if (_userService.loggedUser() != null)
        {
            NavigateToContentPage();
        }
        else
        {
            NavigateToLoginPage();
        }
    }

    private void NavigateToLoginPage()
    {
        this.loginPage = new LoginPage(RenderPage);
        this.Content = this.loginPage;
    }

    private void NavigateToContentPage()
    {
        var contentPage = new ContentPage(RenderPage);
        this.Content = contentPage;
    }

}