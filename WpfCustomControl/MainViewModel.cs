using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfCustomControl
{
    class MainViewModel : ViewModelBase
    {
        public ObservableCollection<string> Collection { get; } = new ObservableCollection<string>();

        private RelayCommand _someCommand;
        public RelayCommand SomeCommand => _someCommand
            ?? (_someCommand = new RelayCommand(() => 
            {
                //Catch me
                MessageBox.Show("It's working");
            }));

        public MainViewModel()
        {
            for (int i = 0; i < 100; i++)
                Collection.Add($"string {i}");
        }
    }
}
