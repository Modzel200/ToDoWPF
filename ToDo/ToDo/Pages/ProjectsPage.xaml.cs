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
using ToDo.Components;
using ToDo.Models;
using ToDo.Services;

namespace ToDo.Pages
{
    /// <summary>
    /// Interaction logic for ProjectsPage.xaml
    /// </summary>
    public partial class ProjectsPage : Page
    {
        private DbService _dbService;
        private UserService _userService;
        private ProjectService _projectService;
        public ProjectsPage()
        {
            InitializeComponent();
            _dbService = new DbService();
            var dbContext = _dbService.Context();
            _userService = new UserService(dbContext);
            _projectService = new ProjectService(dbContext, _userService);
            LoadProjects();
        }

        private void LoadProjects()
        {
            var projects = _projectService.GetAllProjects(new ProjectFilterSortDto());
            ProjectListBox.ItemsSource = projects;
            ProjectListBox.SelectedIndex = 0;
        }

        private void ProjectListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedProject = ProjectListBox.SelectedItem as ProjectDto;
            if (selectedProject != null)
            {
                EnableTaskComponent(selectedProject.Id);
            }
        }

        private void EnableTaskComponent(int projectId)
        {
            taskFrame.Content = new TasksPage(projectId);
        }

        private void AddProjectButton_Click(object sender, RoutedEventArgs e)
        {
            AddProjectWindow addProjectWindow = new AddProjectWindow();
            addProjectWindow.Closed += AddProjectWindow_Closed;
            addProjectWindow.Show();
        }

        private void DeleteProjectButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedProject = ProjectListBox.SelectedItem as ProjectDto;
            if (selectedProject == null)
            {
                return;
            }

            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this project?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                ProjectListBox.SelectedIndex = 0;
                _projectService.DeleteProject(selectedProject.Id);
                LoadProjects();
            }
        }

        private void AddProjectWindow_Closed(object sender, EventArgs e)
        {
            LoadProjects();
        }
    }
}
