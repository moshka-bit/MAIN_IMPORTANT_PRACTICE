using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using MAIN_IMPORTANT_PRACTICE.Data;
using MAIN_IMPORTANT_PRACTICE.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace MAIN_IMPORTANT_PRACTICE;

public partial class CreateAndChangeTeacher : Window
{
    public CreateAndChangeTeacher()
    {
        InitializeComponent();

        if (LoginVariableData.selectedTeacherInMainWindow != null)
        {
            DataContext = LoginVariableData.selectedTeacherInMainWindow;
            TabNumberTextBox.IsReadOnly = true;
            DeleteButton.IsVisible = true;
        }
        else
        {
            DataContext = new Преподаватель();
            TabNumberTextBox.IsReadOnly = false;
            DeleteButton.IsVisible = false;
        }

        KafedraComboBox.ItemsSource = App.DbContext.Кафедраs.ToList();
        ChiefComboBox.ItemsSource = App.DbContext.Сотрудникs
            .Where(s => s.КодРоли == 2 || s.КодРоли == 3)
            .ToList();

        if (LoginVariableData.selectedUserInMainWindow.КодРоли == 2)
        {
            DeleteButton.IsVisible = false;
        }
    }

    private void SaveButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(TabNumberTextBox.Text) ||
            string.IsNullOrEmpty(LastNameTextBox.Text) ||
            string.IsNullOrEmpty(LoginTextBox.Text) ||
            string.IsNullOrEmpty(PasswordTextBox.Text)) return;

        if (!int.TryParse(TabNumberTextBox.Text, out int tabNumber)) return;

        if (tabNumber <= 0) return;

        decimal? salary = null;
        if (!string.IsNullOrEmpty(SalaryTextBox.Text))
        {
            if (!decimal.TryParse(SalaryTextBox.Text, out decimal salaryValue)) return;
            salary = salaryValue;
        }

        var currentTeacher = DataContext as Преподаватель;
        if (currentTeacher == null) return;

        var selectedKafedra = KafedraComboBox.SelectedItem as Кафедра;
        var selectedChief = ChiefComboBox.SelectedItem as Сотрудник;

        using var transaction = App.DbContext.Database.BeginTransaction();

        if (LoginVariableData.selectedTeacherInMainWindow != null)
        {
            var employee = App.DbContext.Сотрудникs
                .Include(s => s.Преподаватель)
                .FirstOrDefault(s => s.ТабНомер == tabNumber);

            if (employee == null) return;

            employee.Фамилия = LastNameTextBox.Text.Trim();
            employee.Логин = LoginTextBox.Text.Trim();
            employee.Пароль = PasswordTextBox.Text.Trim();
            employee.Зарплата = salary;
            employee.КодРоли = 3; // преподаватель

            if (selectedKafedra != null)
            {
                employee.Шифр = selectedKafedra.Шифр;
            }
            else
            {
                employee.Шифр = null;
            }

            if (selectedChief != null && selectedChief.ТабНомер != tabNumber)
            {
                employee.Шеф = selectedChief.ТабНомер;
            }
            else
            {
                employee.Шеф = null;
            }

            if (employee.Преподаватель == null)
            {
                employee.Преподаватель = new Преподаватель
                {
                    ТабНомер = tabNumber,
                    Звание = TitleTextBox.Text?.Trim(),
                    Степень = DegreeTextBox.Text?.Trim()
                };
            }
            else
            {
                employee.Преподаватель.Звание = TitleTextBox.Text?.Trim();
                employee.Преподаватель.Степень = DegreeTextBox.Text?.Trim();
            }
        }
        else
        {
            if (App.DbContext.Сотрудникs.Any(s => s.ТабНомер == tabNumber)) return;

            if (!App.DbContext.Студентs.Any(s => s.РегНомер == tabNumber))
            {
                var defaultSpeciality = App.DbContext.Специальностьs.FirstOrDefault();
                if (defaultSpeciality == null)
                {
                    defaultSpeciality = new Специальность
                    {
                        Номер = $"TEMP-{tabNumber}",
                        Направление = "Временная",
                        Шифр = App.DbContext.Кафедраs.FirstOrDefault()?.Шифр ?? "TEMP"
                    };
                    App.DbContext.Специальностьs.Add(defaultSpeciality);
                    App.DbContext.SaveChanges();
                }

                var student = new Студент
                {
                    РегНомер = tabNumber,
                    Номер = defaultSpeciality.Номер
                };
                App.DbContext.Студентs.Add(student);
                App.DbContext.SaveChanges();
            }

            var employee = new Сотрудник
            {
                ТабНомер = tabNumber,
                Фамилия = LastNameTextBox.Text.Trim(),
                Логин = LoginTextBox.Text.Trim(),
                Пароль = PasswordTextBox.Text.Trim(),
                Зарплата = salary,
                КодРоли = 3 // преподаватель
            };

            if (selectedKafedra != null)
            {
                employee.Шифр = selectedKafedra.Шифр;
            }

            if (selectedChief != null && selectedChief.ТабНомер != tabNumber)
            {
                employee.Шеф = selectedChief.ТабНомер;
            }

            var teacher = new Преподаватель
            {
                ТабНомер = tabNumber,
                Звание = TitleTextBox.Text?.Trim(),
                Степень = DegreeTextBox.Text?.Trim(),
                ТабНомерNavigation = employee
            };

            employee.Преподаватель = teacher;

            App.DbContext.Сотрудникs.Add(employee);
        }

        App.DbContext.SaveChanges();
        transaction.Commit();
        this.Close();
    }

    private void DeleteButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (!int.TryParse(TabNumberTextBox.Text, out int tabNumber) || tabNumber <= 0) return;

        if (LoginVariableData.selectedTeacherInMainWindow == null) return;

        var employee = App.DbContext.Сотрудникs
            .Include(s => s.Преподаватель)
            .FirstOrDefault(s => s.ТабНомер == tabNumber);

        if (employee == null) return;

        App.DbContext.Сотрудникs.Remove(employee);
        App.DbContext.SaveChanges();
        this.Close();
    }
}