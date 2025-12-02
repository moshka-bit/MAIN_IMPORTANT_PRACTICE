using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using MAIN_IMPORTANT_PRACTICE.Data;
using MAIN_IMPORTANT_PRACTICE.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace MAIN_IMPORTANT_PRACTICE;

public partial class CreateAndChangeStudent : Window
{
    public CreateAndChangeStudent()
    {
        InitializeComponent();

        if (LoginVariableData.selectedStudentInMainWindow != null)
        {
            DataContext = LoginVariableData.selectedStudentInMainWindow;
            RegNumberTextBox.IsReadOnly = true;
            DeleteButton.IsVisible = true; // Показываем кнопку удаления
        }
        else
        {
            DataContext = new Студент();
            RegNumberTextBox.IsReadOnly = false;
            DeleteButton.IsVisible = false; // Скрываем кнопку удаления
        }
    }

    private void SaveButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        // ВАЛИДАЦИЯ ОБЯЗАТЕЛЬНЫХ ПОЛЕЙ
        if (string.IsNullOrEmpty(RegNumberTextBox.Text) ||
            string.IsNullOrEmpty(NumberTextBox.Text) ||
            string.IsNullOrEmpty(LastNameTextBox.Text) ||
            string.IsNullOrEmpty(LoginTextBox.Text) ||
            string.IsNullOrEmpty(PasswordTextBox.Text))
        {
            // Просто выходим если не все заполнено
            return;
        }

        // ПАРСИМ РЕГНомер
        if (!int.TryParse(RegNumberTextBox.Text, out int regNumber)) return;

        // Проверяем регномер на положительность
        if (regNumber <= 0) return;

        var currentStudent = DataContext as Студент;
        if (currentStudent == null) return;

        try
        {
            using var transaction = App.DbContext.Database.BeginTransaction();

            if (LoginVariableData.selectedStudentInMainWindow != null)
            {
                // РЕДАКТИРОВАНИЕ СУЩЕСТВУЮЩЕГО СТУДЕНТА

                // Получаем студента из БД с включенным сотрудником
                var student = App.DbContext.Студентs
                    .Include(s => s.Сотрудник)
                    .Include(s => s.НомерNavigation)
                    .FirstOrDefault(s => s.РегНомер == regNumber);

                if (student == null) return;

                // Обновляем данные студента
                student.Номер = NumberTextBox.Text.Trim();

                // Обновляем или создаем связанного сотрудника
                if (student.Сотрудник == null)
                {
                    // Создаем нового сотрудника
                    var employee = new Сотрудник
                    {
                        ТабНомер = regNumber,
                        Фамилия = LastNameTextBox.Text.Trim(),
                        Логин = LoginTextBox.Text.Trim(),
                        Пароль = PasswordTextBox.Text.Trim(),
                        КодРоли = 4 // Предположим, что 4 - код роли студента (проверьте в таблице Роли)
                    };
                    student.Сотрудник = employee;
                }
                else
                {
                    // Обновляем существующего сотрудника
                    student.Сотрудник.Фамилия = LastNameTextBox.Text.Trim();
                    student.Сотрудник.Логин = LoginTextBox.Text.Trim();
                    student.Сотрудник.Пароль = PasswordTextBox.Text.Trim();
                    student.Сотрудник.КодРоли = 4; // Код роли студента
                }
            }
            else
            {
                // СОЗДАНИЕ НОВОГО СТУДЕНТА

                // Проверяем, не существует ли уже студента с таким регномером
                if (App.DbContext.Студентs.Any(s => s.РегНомер == regNumber))
                {
                    return;
                }

                // Проверяем, существует ли сотрудник с таким табномером (чтобы избежать конфликта)
                if (App.DbContext.Сотрудникs.Any(s => s.ТабНомер == regNumber))
                {
                    // Сотрудник с таким табномером уже существует
                    return;
                }

                // Проверяем, существует ли специальность с указанным номером
                var speciality = App.DbContext.Специальностьs
                    .FirstOrDefault(s => s.Номер == NumberTextBox.Text.Trim());

                if (speciality == null)
                {
                    // Специальность не существует
                    return;
                }

                // СОЗДАЕМ СНАЧАЛА СОТРУДНИКА
                var employee = new Сотрудник
                {
                    ТабНомер = regNumber,
                    Фамилия = LastNameTextBox.Text.Trim(),
                    Логин = LoginTextBox.Text.Trim(),
                    Пароль = PasswordTextBox.Text.Trim(),
                    КодРоли = 4 // Код роли студента
                };

                // Добавляем сотрудника
                App.DbContext.Сотрудникs.Add(employee);
                App.DbContext.SaveChanges(); // Сохраняем чтобы получить ID

                // ТЕПЕРЬ СОЗДАЕМ СТУДЕНТА
                var student = new Студент
                {
                    РегНомер = regNumber,
                    Номер = NumberTextBox.Text.Trim(),
                    Сотрудник = employee
                };

                App.DbContext.Студентs.Add(student);
            }

            App.DbContext.SaveChanges();
            transaction.Commit();
            this.Close();
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine($"Ошибка сохранения студента: {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Внутренняя ошибка: {ex.InnerException.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Общая ошибка: {ex.Message}");
        }
    }
}