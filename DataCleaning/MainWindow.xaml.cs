using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;

namespace DataCleaning_Preprocessing
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ViewModel _viewModel;
        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new ViewModel();
            DataContext = _viewModel;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _viewModel.IsBusy = true;
            Task.Run(async () =>
            {
                await _viewModel.LoadCleanedDataAsync();
            });
        }
    }
}