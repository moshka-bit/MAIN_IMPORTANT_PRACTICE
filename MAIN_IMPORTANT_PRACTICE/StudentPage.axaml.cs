using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MAIN_IMPORTANT_PRACTICE;

public partial class StudentPage : Window
{
    public StudentPage()
    {
        InitializeComponent();
    }

    private void SwitchDisciplines_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        MainControl.Content = new SeeDisciplines();
    }
    private void SwitchExams_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        MainControl.Content = new SeeExams();
    }
}