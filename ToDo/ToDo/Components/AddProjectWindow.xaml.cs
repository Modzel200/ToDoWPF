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
using System.Windows.Shapes;
using ToDo.Models;
using ToDo.Services;
using ToDo.Entities;

namespace ToDo.Components
{
    /// <summary>
    /// Interaction logic for AddProjectWindow.xaml
    /// </summary>
    public partial class AddProjectWindow : Window
    {
        private readonly DbService _dbService;
        private readonly UserService _userService;
        private readonly ProjectService _projectService;

        public AddProjectWindow()
        {
            InitializeComponent();
            _dbService = new DbService();
            var dbContext = _dbService.Context();
            _userService = new UserService(dbContext);
            _projectService = new ProjectService(dbContext, _userService);
        }

        private void AddProjectButton_Click(object sender, RoutedEventArgs e)
        {
            if (ColorComboBox.SelectedItem is ComboBoxItem selectedItem && selectedItem.Tag is Entities.Color selectedColor)
            {
                var dto = new CreateProjectDto
                {
                    Name = NameTextBox.Text,
                    Description = DescriptionTextBox.Text,
                    Color = selectedColor,
                    DeadLine = DeadLineDatePicker.SelectedDate
                };

                _projectService.AddProject(dto);
                Close();
            }
            else
            {
                MessageBox.Show("Please select a color.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
