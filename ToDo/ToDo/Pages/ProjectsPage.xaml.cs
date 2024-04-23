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
            var projects = _projectService.GetAllProjects();
            ProjectListBox.ItemsSource = projects;
        }

        private void ProjectListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedProject = ProjectListBox.SelectedItem as ProjectDto;
            if (selectedProject != null)
            {
                DisplayProjectDetails(selectedProject);
            }
        }

        private void DisplayProjectDetails(ProjectDto project)
        {
            ProjectDetailsTextBlock.Text = $"Name: {project.Name}\n" +
                                           $"Description: {project.Description}\n" +
                                           $"Deadline: {project.DeadLine}\n" +
                                           $"Color: {project.Color}\n" +
                                           $"Is Done: {project.IsDone}\n" +
                                           $"Done Ratio: {project.DoneRatio}";
        }
    }
}
