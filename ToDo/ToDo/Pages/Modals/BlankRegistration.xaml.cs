using System.Windows;
using System.Windows.Controls;

namespace ToDo.Pages.Modals;

public partial class BlankRegistration : UserControl
{
    public BlankRegistration()
    {
        InitializeComponent();
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        Window.GetWindow(this).Close();
    }
}