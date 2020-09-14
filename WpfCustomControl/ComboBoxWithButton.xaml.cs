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
    /// Interaction logic for ComboBoxWithButton.xaml
    /// </summary>
    public partial class ComboBoxWithButton : ComboBox
    {
        public ComboBoxWithButton()
        {
            InitializeComponent();
        }

        public object AdditionalControl
        {
            get { return GetValue(AdditionalControlProperty); }
            set { SetValue(AdditionalControlProperty, value); }
        }

        public static readonly DependencyProperty AdditionalControlProperty =
            DependencyProperty.Register("AdditionalControl", typeof(object), typeof(ComboBoxWithButton), new UIPropertyMetadata());

        /*public ICommand AdditionalButtonCommand
        {
            get { return (ICommand)GetValue(AdditionalButtonCommandProperty); }
            set { SetValue(AdditionalButtonCommandProperty, value); }
        }

        public static readonly DependencyProperty AdditionalButtonCommandProperty =
            DependencyProperty.Register("AdditionalButtonCommand", typeof(ICommand), typeof(ComboBoxWithButton), new UIPropertyMetadata());

        public object AdditionalButtonContent
        {
            get { return GetValue(AdditionalButtonContentProperty); }
            set { SetValue(AdditionalButtonContentProperty, value); }
        }

        public static readonly DependencyProperty AdditionalButtonContentProperty =
            DependencyProperty.Register("AdditionalButtonContent", typeof(object), typeof(ComboBoxWithButton), new UIPropertyMetadata());*/
    }
}
