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

        // Показываем кнопку удаления только при редактировании существующего преподавателя
        if (LoginVariableData.selectedTeacherInMainWindow != null)
        {
            DataContext = LoginVariableData.selectedTeacherInMainWindow;
            TabNumberTextBox.IsReadOnly = true;
            DeleteButton.IsVisible = true; // Показываем кнопку удаления
        }
        else
        {
            DataContext = new Преподаватель();
            TabNumberTextBox.IsReadOnly = false;
            DeleteButton.IsVisible = false; // Скрываем кнопку удаления
        }

        KafedraComboBox.ItemsSource = App.DbContext.Кафедраs.ToList();
        ChiefComboBox.ItemsSource = App.DbContext.Сотрудникs
            .Where(s => s.КодРоли == 2 || s.КодРоли == 3)
            .ToList();
    }

    private void SaveButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        // ВАЛИДАЦИЯ ОБЯЗАТЕЛЬНЫХ ПОЛЕЙ
        if (string.IsNullOrEmpty(TabNumberTextBox.Text) ||
            string.IsNullOrEmpty(LastNameTextBox.Text) ||
            string.IsNullOrEmpty(LoginTextBox.Text) ||
            string.IsNullOrEmpty(PasswordTextBox.Text))
        {
            // Просто выходим если не все заполнено
            return;
        }

        // ПАРСИМ ТАБНомер
        if (!int.TryParse(TabNumberTextBox.Text, out int tabNumber)) return;

        // Проверяем табномер на положительность
        if (tabNumber <= 0) return;

        // ПАРСИМ ЗАРПЛАТУ (если заполнена)
        decimal? salary = null;
        if (!string.IsNullOrEmpty(SalaryTextBox.Text))
        {
            if (!decimal.TryParse(SalaryTextBox.Text, out decimal salaryValue))
            {
                return;
            }
            salary = salaryValue;
        }

        var currentTeacher = DataContext as Преподаватель;
        if (currentTeacher == null) return;

        // Получаем выбранные значения
        var selectedKafedra = KafedraComboBox.SelectedItem as Кафедра;
        var selectedChief = ChiefComboBox.SelectedItem as Сотрудник;

        try
        {
            using var transaction = App.DbContext.Database.BeginTransaction();

            if (LoginVariableData.selectedTeacherInMainWindow != null)
            {
                // РЕДАКТИРОВАНИЕ СУЩЕСТВУЮЩЕГО ПРЕПОДАВАТЕЛЯ

                // Получаем сотрудника из БД
                var employee = App.DbContext.Сотрудникs
                    .Include(s => s.Преподаватель)
                    .FirstOrDefault(s => s.ТабНомер == tabNumber);

                if (employee == null) return;

                // Обновляем данные сотрудника
                employee.Фамилия = LastNameTextBox.Text.Trim();
                employee.Логин = LoginTextBox.Text.Trim();
                employee.Пароль = PasswordTextBox.Text.Trim();
                employee.Зарплата = salary;
                employee.КодРоли = 3; // Код роли преподавателя

                // Устанавливаем кафедру через ID
                if (selectedKafedra != null)
                {
                    employee.Шифр = selectedKafedra.Шифр;
                }
                else
                {
                    employee.Шифр = null;
                }

                // Устанавливаем шефа через ID
                if (selectedChief != null && selectedChief.ТабНомер != tabNumber)
                {
                    employee.Шеф = selectedChief.ТабНомер;
                }
                else
                {
                    employee.Шеф = null;
                }

                // Обновляем данные преподавателя
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
                // СОЗДАНИЕ НОВОГО ПРЕПОДАВАТЕЛЯ

                // Проверяем, не существует ли уже сотрудника с таким табномером
                if (App.DbContext.Сотрудникs.Any(s => s.ТабНомер == tabNumber))
                {
                    return;
                }

                // ПЕРВОЕ: Создаем запись студента (если ее еще нет)
                if (!App.DbContext.Студентs.Any(s => s.РегНомер == tabNumber))
                {
                    // Получаем первую специальность
                    var defaultSpeciality = App.DbContext.Специальностьs.FirstOrDefault();
                    if (defaultSpeciality == null)
                    {
                        // Создаем временную специальность если нет ни одной
                        defaultSpeciality = new Специальность
                        {
                            Номер = $"TEMP-{tabNumber}",
                            Направление = "Временная",
                            Шифр = App.DbContext.Кафедраs.FirstOrDefault()?.Шифр ?? "TEMP"
                        };
                        App.DbContext.Специальностьs.Add(defaultSpeciality);
                        App.DbContext.SaveChanges(); // Сохраняем чтобы получить ID
                    }

                    var student = new Студент
                    {
                        РегНомер = tabNumber,
                        Номер = defaultSpeciality.Номер
                    };
                    App.DbContext.Студентs.Add(student);
                    App.DbContext.SaveChanges(); // Сохраняем студента
                }

                // ВТОРОЕ: Создаем сотрудника
                var employee = new Сотрудник
                {
                    ТабНомер = tabNumber,
                    Фамилия = LastNameTextBox.Text.Trim(),
                    Логин = LoginTextBox.Text.Trim(),
                    Пароль = PasswordTextBox.Text.Trim(),
                    Зарплата = salary,
                    КодРоли = 3 // Код роли преподавателя
                };

                // Устанавливаем кафедру
                if (selectedKafedra != null)
                {
                    employee.Шифр = selectedKafedra.Шифр;
                }

                // Устанавливаем шефа
                if (selectedChief != null && selectedChief.ТабНомер != tabNumber)
                {
                    employee.Шеф = selectedChief.ТабНомер;
                }

                // ТРЕТЬЕ: Создаем преподавателя
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
        catch (DbUpdateException ex)
        {
            Console.WriteLine($"Ошибка сохранения: {ex.Message}");
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

    private void DeleteButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        // Получаем табельный номер из формы
        if (!int.TryParse(TabNumberTextBox.Text, out int tabNumber) || tabNumber <= 0)
        {
            return;
        }

        // Проверяем, редактируем ли мы существующего преподавателя
        if (LoginVariableData.selectedTeacherInMainWindow == null)
        {
            return;
        }

        // Находим сотрудника (преподавателя) для удаления
        var employee = App.DbContext.Сотрудникs
            .Include(s => s.Преподаватель)
            .FirstOrDefault(s => s.ТабНомер == tabNumber);

        if (employee == null)
        {
            Console.WriteLine("Преподаватель не найден");
            return;
        }

        // Удаляем сотрудника (каскадное удаление настроено в БД)
        App.DbContext.Сотрудникs.Remove(employee);

        // Сохраняем изменения
        App.DbContext.SaveChanges();

        // Закрываем окно после успешного удаления
        this.Close();
    }
}