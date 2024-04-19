using Microsoft.EntityFrameworkCore;
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

namespace ToDo;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly ToDoDbContext _context = new ToDoDbContext();
    public MainWindow()
    {
        InitializeComponent();
    }


    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        _context.Database.EnsureCreated();
        _context.Users.Load();
        _context.Projects.Load();
        _context.Tasks.Load();
        _context.SubTasks.Load();

    }
}