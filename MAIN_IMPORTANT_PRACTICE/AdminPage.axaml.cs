using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MAIN_IMPORTANT_PRACTICE;

public partial class AdminPage : Window
{
    public AdminPage()
    {
        InitializeComponent();
    }

    private void SwitchDisciplines_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        MainControl.Content = new DisciplinesControl_AdminAndDepartment();
    }

    private void SwitchTeachers_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        MainControl.Content = new TeacherControl_AdminAndDepartment();
    }

    private void SwitchExams_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        MainControl.Content = new ExamsControl_CRUD_();
    }

    private void SwitchStudents_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        MainControl.Content = new StudentControl_CRUD_();
    }
}