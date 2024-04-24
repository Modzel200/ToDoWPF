using System.Windows;
using System.Windows.Controls;

namespace ToDo.Pages;

public partial class AccountCreated : UserControl
{
    public AccountCreated()
    {
        InitializeComponent();
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        Window.GetWindow(this).Close();
    }
}