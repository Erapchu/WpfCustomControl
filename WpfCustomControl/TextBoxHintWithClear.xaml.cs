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

        public static readonly DependencyProperty HasClearButtonProperty =
            DependencyProperty.Register("HasClearButton", typeof(bool), typeof(TextBoxHintWithClear), new PropertyMetadata(default(bool), HasClearButtonProperty_Changed));

        public static bool GetHasClearButton(DependencyObject d)
        {
            return (bool)d.GetValue(HasClearButtonProperty);
        }

        public static void SetHasClearButton(DependencyObject d, bool value)
        {
            d.SetValue(HasClearButtonProperty, value);
        }

        private static void HasClearButtonProperty_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is Control box))
                return;

            if (box.IsLoaded)
                SetHasClearButtonHandler(box);
            else
                box.Loaded += (sender, args) =>
                    SetHasClearButtonHandler(box);
        }

        public static void SetHasClearButtonHandler(Control box)
        {
            var bValue = GetHasClearButton(box);
            var clearButton = box.Template.FindName("PART_ClearButton", box) as Button;
            if (clearButton != null)
            {
                void handler(object sender, RoutedEventArgs args)
                {
                    (box as TextBox)?.SetCurrentValue(TextProperty, null);
                }
                if (bValue)
                    clearButton.Click += handler;
                else
                    clearButton.Click -= handler;
            }
        }
    }
}
