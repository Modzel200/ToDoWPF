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
        private ProjectFilterSortDto _filterDto;
        public ProjectsPage()
        {
            InitializeComponent();
            _dbService = new DbService();
            var dbContext = _dbService.Context();
            _userService = new UserService(dbContext);
            _projectService = new ProjectService(dbContext, _userService);
            _filterDto = new ProjectFilterSortDto();
            LoadProjects();
        }

        private void LoadProjects()
        {
            var projects = _projectService.GetAllProjects(_filterDto);
            ProjectListBox.ItemsSource = projects;
            ProjectListBox.SelectedIndex = 0;
            if (projects.Count() == 0) taskFrame.Content = null;
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
            taskFrame.Content = new TasksPage(projectId, LoadProjects);
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

        private void ApplyFilter_Click(object sender, RoutedEventArgs e)
        {
            var sortDir = SortDirection.ASC;
            var sortBy = "Id";
            List<Entities.Color> selectedColors = new List<Entities.Color>();
            foreach (var item in ColorListBox.SelectedItems)
            {
                if (Enum.TryParse(item.ToString(), out Entities.Color color))
                {
                    selectedColors.Add(color);
                }
            }
            if (DirectionComboBox.SelectedItem is ComboBoxItem selectedItem && selectedItem.Tag is Models.SortDirection selectedDir)
            {
                sortDir = selectedDir;
            }
            if (SortByCombobox.SelectedItem is ComboBoxItem selectedBy && selectedBy.Tag is string selectedSortBy)
            {
                sortBy = selectedSortBy;
            }
            _filterDto = new ProjectFilterSortDto()
            {
                Search = SearchBox.Text,
                SortDirection = sortDir,
                Colors = selectedColors,
                SortBy = sortBy,
                IsDone = isDoneFilter.IsChecked
            };
            LoadProjects();
        }

        private void ClearFilter_Click(object sender, RoutedEventArgs e)
        {
            _filterDto = new ProjectFilterSortDto();
            SearchBox.Text = "";
            DirectionComboBox.SelectedIndex = 0;
            ColorListBox.SelectedItems.Clear();
            SortByCombobox.SelectedIndex = 0;
            isDoneFilter.IsChecked = false;
            LoadProjects();
        }


    }
}
