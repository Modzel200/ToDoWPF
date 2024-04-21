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

namespace ToDo.Components
{
    /// <summary>
    /// Interaction logic for CustomButton.xaml
    /// </summary>
    public partial class CustomButton : UserControl
    {
        public event RoutedEventHandler Click;

        public CustomButton()
        {
            InitializeComponent();

        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }


        public static readonly DependencyProperty TextProperty =
          DependencyProperty.Register("Text", typeof(string), typeof(CustomButton), new UIPropertyMetadata(""));

        public ImageSource Image
        {
            get { return (ImageSource)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        public static readonly DependencyProperty ImageProperty =
           DependencyProperty.Register("Image", typeof(ImageSource), typeof(CustomButton), new UIPropertyMetadata(null));


        private void button_Click(object sender, RoutedEventArgs e)
        {

            if (null != Click)

                Click(sender, e);

        }

    }
}
