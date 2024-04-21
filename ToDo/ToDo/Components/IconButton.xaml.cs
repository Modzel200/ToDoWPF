using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ToDo.Components
{
    public partial class IconButton : UserControl
    {
        public event RoutedEventHandler Click;

        public IconButton()
        {
            InitializeComponent();

        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }


        public static readonly DependencyProperty TextProperty =
          DependencyProperty.Register("Text", typeof(string), typeof(IconButton), new UIPropertyMetadata(""));

        public ImageSource Image
        {
            get { return (ImageSource)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        public static readonly DependencyProperty ImageProperty =
           DependencyProperty.Register("Image", typeof(ImageSource), typeof(IconButton), new UIPropertyMetadata(null));


        private void button_Click(object sender, RoutedEventArgs e)
        {

            if (null != Click)

                Click(sender, e);

        }

    }
}
