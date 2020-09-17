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

namespace WpfCustomControl
{
    /// <summary>
    /// Interaction logic for TextBoxHintWithClear.xaml
    /// </summary>
    public partial class TextBoxHintWithClear : TextBox
    {
        public TextBoxHintWithClear()
        {
            InitializeComponent();
        }

        public string Hint
        {
            get { return (string)GetValue(HintProperty); }
            set { SetValue(HintProperty, value); }
        }

        public static readonly DependencyProperty HintProperty =
            DependencyProperty.Register("Hint", typeof(string), typeof(TextBoxHintWithClear), new PropertyMetadata());

        public bool HasClearButton
        {
            get { return (bool)GetValue(HasClearButtonProperty); }
            set { SetValue(HasClearButtonProperty, value); }
        }

        public static readonly DependencyProperty HasClearButtonProperty =
            DependencyProperty.Register("HasClearButton", typeof(bool), typeof(TextBoxHintWithClear), new PropertyMetadata(default(bool)));
    }
}
