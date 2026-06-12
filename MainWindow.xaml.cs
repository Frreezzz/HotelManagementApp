using System.Windows;
using HotelManagementApp.ViewModels;

namespace HotelManagementApp;

public partial class MainWindow : Window
{
    public MainWindow(MainViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
