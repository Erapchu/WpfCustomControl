using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

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

        private string _filter;
        private string _filterLower = string.Empty;
        public string Filter
        {
            get
            {
                if (string.IsNullOrEmpty(_filter)) _filter = string.Empty;
                return _filter;
            }
            set
            {
                _filter = value;
                _filterLower = _filter.ToLowerInvariant();
                RaisePropertyChanged();
                HeroesCollection.Refresh();
            }
        }

        public ICollectionView HeroesCollection { get; }

        private int _positionX;
        public int PositionX
        {
            get => _positionX;
            set => Set(ref _positionX, value);
        }

        private int _positionY;
        public int PositionY
        {
            get => _positionY;
            set => Set(ref _positionY, value);
        }

        public MainViewModel()
        {
            for (int i = 0; i < 100; i++)
                Collection.Add($"string {i}");

            var heroes = new string[] 
            { 
                "Wonder woman",
                "Iron man",
                "Program",
                "Ms Z",
                "Black cat"
            };

            HeroesCollection = new ListCollectionView(heroes) 
            {
                Filter = FilterCollection
            };
        }

        private bool FilterCollection(object obj)
        {
            return (obj as string).ToLowerInvariant().Contains(_filterLower);
        }
    }
}
