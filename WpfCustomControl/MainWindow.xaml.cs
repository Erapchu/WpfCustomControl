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
using WpfCustomControl.Controls;

namespace WpfCustomControl
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private LowLevelKeyboardListener _listener;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }

        private void Flyout_IsOpenChanged(object sender, RoutedEventArgs e)
        {
            if (!(sender is Flyout flyout))
                return;

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //_listener = new LowLevelKeyboardListener();
            //_listener.OnKeyPressed += _listener_OnKeyPressed;

            //_listener.HookKeyboard();
        }

        void _listener_OnKeyPressed(object sender, KeyPressedArgs e)
        {
            //gasd.Text += e.KeyPressed.ToString();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //_listener.UnHookKeyboard();
        }
    }
}
