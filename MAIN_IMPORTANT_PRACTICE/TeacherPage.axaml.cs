using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MAIN_IMPORTANT_PRACTICE;

public partial class TeacherPage : Window
{
    public TeacherPage()
    {
        InitializeComponent();
    }

    private void SwitchDisciplines_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        MainControl.Content = new SeeDisciplines();
    }

    private void SwitchExams_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        MainControl.Content = new ExamsControl_CRUD_();
    }

    private void SwitchStudents_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        MainControl.Content = new SeeStudents();
    }
}