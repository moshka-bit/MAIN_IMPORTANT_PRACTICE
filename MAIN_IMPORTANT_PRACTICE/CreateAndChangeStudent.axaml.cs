using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using MAIN_IMPORTANT_PRACTICE.Data;
using MAIN_IMPORTANT_PRACTICE.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace MAIN_IMPORTANT_PRACTICE;

public partial class CreateAndChangeStudent : Window
{
    public CreateAndChangeStudent()
    {
        InitializeComponent();

        SpecialityComboBox.ItemsSource = App.DbContext.Специальностьs
            .Include(s => s.ШифрNavigation)
            .ToList();

        if (LoginVariableData.selectedStudentInMainWindow != null)
        {
            var loadedStudent = App.DbContext.Студентs
                .Include(s => s.НомерNavigation)
                .Include(s => s.Сотрудник)
                .FirstOrDefault(s => s.РегНомер == LoginVariableData.selectedStudentInMainWindow.РегНомер);

            DataContext = loadedStudent ?? LoginVariableData.selectedStudentInMainWindow;
            RegNumberTextBox.IsReadOnly = true;
            DeleteButton.IsVisible = true;

            var currentStudent = DataContext as Студент;
            if (currentStudent != null && !string.IsNullOrEmpty(currentStudent.Номер))
            {
                var speciality = App.DbContext.Специальностьs
                    .Include(s => s.ШифрNavigation)
                    .FirstOrDefault(s => s.Номер == currentStudent.Номер);

                if (speciality != null)
                    SpecialityComboBox.SelectedItem = speciality;
            }
        }
        else
        {
            DataContext = new Студент();
            RegNumberTextBox.IsReadOnly = false;
            DeleteButton.IsVisible = false;
        }
    }

    private void SaveButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(RegNumberTextBox.Text)) return;

        if (SpecialityComboBox.SelectedItem == null) return;

        if (string.IsNullOrWhiteSpace(LastNameTextBox.Text)) return;

        if (string.IsNullOrWhiteSpace(LoginTextBox.Text)) return;

        if (string.IsNullOrWhiteSpace(PasswordTextBox.Text)) return;

        if (!int.TryParse(RegNumberTextBox.Text, out int regNumber)) return;

        if (regNumber <= 0) return;


        var currentStudent = DataContext as Студент;
        if (currentStudent == null) return;

        var selectedSpeciality = SpecialityComboBox.SelectedItem as Специальность;
        if (selectedSpeciality == null) return;

        using var transaction = App.DbContext.Database.BeginTransaction();

        if (LoginVariableData.selectedStudentInMainWindow != null)
        {
            var student = App.DbContext.Студентs
                .Include(s => s.Сотрудник)
                .Include(s => s.НомерNavigation)
                .FirstOrDefault(s => s.РегНомер == regNumber);

            if (student == null) return;

            student.Номер = selectedSpeciality.Номер;
            student.НомерNavigation = selectedSpeciality;


            if (student.Сотрудник == null)
            {
                var employee = new Сотрудник
                {
                    ТабНомер = regNumber,
                    Фамилия = LastNameTextBox.Text.Trim(),
                    Логин = LoginTextBox.Text.Trim(),
                    Пароль = PasswordTextBox.Text.Trim(),
                    КодРоли = 4, // студент
                    Номер = selectedSpeciality.Номер
                };
                student.Сотрудник = employee;
            }
            else
            {
                student.Сотрудник.Фамилия = LastNameTextBox.Text.Trim();
                student.Сотрудник.Логин = LoginTextBox.Text.Trim();
                student.Сотрудник.Пароль = PasswordTextBox.Text.Trim();
                student.Сотрудник.КодРоли = 4; // студент
                student.Сотрудник.Номер = selectedSpeciality.Номер;
            }
        }
        else
        {

            if (App.DbContext.Студентs.Any(s => s.РегНомер == regNumber)) return;

            if (App.DbContext.Сотрудникs.Any(s => s.ТабНомер == regNumber)) return;

            var student = new Студент
            {
                РегНомер = regNumber,
                Номер = selectedSpeciality.Номер,
                НомерNavigation = selectedSpeciality
            };

            var employee = new Сотрудник
            {
                ТабНомер = regNumber,
                Фамилия = LastNameTextBox.Text.Trim(),
                Логин = LoginTextBox.Text.Trim(),
                Пароль = PasswordTextBox.Text.Trim(),
                КодРоли = 4, // студент
                Номер = selectedSpeciality.Номер,
                ТабНомерNavigation = student
            };

            student.Сотрудник = employee;

            App.DbContext.Студентs.Add(student);
        }

        App.DbContext.SaveChanges();
        transaction.Commit();

        this.Close();
    }

    private void DeleteButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (!int.TryParse(RegNumberTextBox.Text, out int regNumber) || regNumber <= 0) return;

        if (LoginVariableData.selectedStudentInMainWindow == null) return;

        var student = App.DbContext.Студентs
                .Include(s => s.Сотрудник)
                .FirstOrDefault(s => s.РегНомер == regNumber);

        if (student == null) return;

        if (student.Сотрудник != null)
        {
            App.DbContext.Сотрудникs.Remove(student.Сотрудник);
        }
            App.DbContext.Студентs.Remove(student);

            App.DbContext.SaveChanges();
            this.Close();
    }
}