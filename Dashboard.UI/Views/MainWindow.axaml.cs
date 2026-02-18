using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Diagnostics;

namespace Dashboard.UI.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        //CalculateButton.Click += Button_OnClick; // attach once
    }

    //private async void Button_OnClick(object? sender, RoutedEventArgs e)
    //{
    //    var dialog = new Window
    //    {
    //        Title = "Alert",
    //        Width = 300,
    //        Height = 150,
    //        Content = new TextBlock { Text = $"Celsius value: {Celsius.Text}" }
    //    };
    //    await dialog.ShowDialog(this);
    //}

}