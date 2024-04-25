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
using ToDo.Entities;
using ToDo.Models;
using ToDo.Services;

namespace ToDo.Components
{
    /// <summary>
    /// Interaction logic for AddSubtaskWindow.xaml
    /// </summary>
    public partial class AddSubtaskWindow : Window
    {
        private readonly DbService _dbService;
        private readonly UserService _userService;
        private readonly SubtaskService _subtaskService;
        private readonly ProjectService _projectService;
        private readonly int _taskId;
        public AddSubtaskWindow(int taskId)
        {
            InitializeComponent();
            _dbService = new DbService();
            var dbContext = _dbService.Context();
            _userService = new UserService(dbContext);
            _subtaskService = new SubtaskService(dbContext, _userService);
            _taskId = taskId;
        }

        private void AddSubtaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (DescriptionTextBox.Text != null && DescriptionTextBox.Text != "")
            {
                var dto = new CreateSubTaskDto
                {
                    Description = DescriptionTextBox.Text,
                };

                _subtaskService.AddSubTask(dto, _taskId);
                Close();
            }
            else
            {
                MessageBox.Show("Please select a category and priority level.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
