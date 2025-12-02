using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using MAIN_IMPORTANT_PRACTICE.Models;
using System.Linq;

namespace MAIN_IMPORTANT_PRACTICE;

public partial class LogIn : Window
{
    public LogIn()
    {
        InitializeComponent();
    }

    private void LoginButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        // валидация полей
        if (string.IsNullOrEmpty(LoginText.Text) || string.IsNullOrEmpty(PasswordText.Text))
        {
            return;
        }

        // проверка логина
        var login_exist = App.DbContext.Сотрудникs.FirstOrDefault(l => l.Логин == LoginText.Text && l.Пароль == PasswordText.Text);
        if (login_exist == null) return;

        LoginVariableData.selectedUserInMainWindow = login_exist;

        // проверка на роль и переход по страницам
        var parent = this.VisualRoot as Window;

        if (login_exist.КодРоли == 1)
        {
            var admin_page = new AdminPage();
            admin_page.ShowDialog(parent);
        }
        else if (login_exist.КодРоли == 2)
        {
            var department_page = new DepartmentPage();
            department_page.ShowDialog(parent);
        }
        else if (login_exist.КодРоли == 3)
        {
            var teacher_page = new TeacherPage();
            teacher_page.ShowDialog(parent);
        }
        else
        {
            var student_page = new StudentPage();
            student_page.ShowDialog(parent);
        }
    }
}