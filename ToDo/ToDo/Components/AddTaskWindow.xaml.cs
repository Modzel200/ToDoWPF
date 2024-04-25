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

namespace ToDo.Components
{
    public partial class AddTaskWindow : Window
    {
        private readonly DbService _dbService;
        private readonly UserService _userService;
        private readonly TaskService _taskService;
        private readonly ProjectService _projectService;
        private readonly int _projectId;

        public AddTaskWindow(int projectId)
        {
            InitializeComponent();
            _dbService = new DbService();
            var dbContext = _dbService.Context();
            _userService = new UserService(dbContext);
            _taskService = new TaskService(dbContext, _userService, _projectService);
            _projectId = projectId;
        }

        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (CategoryComboBox.SelectedItem is ComboBoxItem selectedItem && selectedItem.Tag is Entities.Category selectedCategory && PriorityComboBox.SelectedItem is ComboBoxItem selectedItemSecond && selectedItemSecond.Tag is Entities.PriorityLevel selectedPriority)
            {
                var dto = new CreateTaskDto
                {
                    Name = NameTextBox.Text,
                    Description = DescriptionTextBox.Text,
                    Category = selectedCategory,
                    PriorityLevel = selectedPriority,
                    DeadLine = DeadLineDatePicker.SelectedDate
                };

                _taskService.AddTask(dto, _projectId);
                Close();
            }
            else
            {
                MessageBox.Show("Please select a category and priority level.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
