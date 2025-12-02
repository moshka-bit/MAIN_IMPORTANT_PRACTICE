using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using MAIN_IMPORTANT_PRACTICE.Data;
using MAIN_IMPORTANT_PRACTICE.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MAIN_IMPORTANT_PRACTICE;

public partial class CreateAndChangeExam : Window
{
    public CreateAndChangeExam()
    {
        InitializeComponent();

        if (LoginVariableData.selectedExamInMainWindow != null)
        {
            DataContext = LoginVariableData.selectedExamInMainWindow;
        }
        else
        {
            // Создаем новый экзамен с текущей датой по умолчанию
            var newExam = new Экзамен
            {
                Дата = DateOnly.FromDateTime(DateTime.Today)
            };
            DataContext = newExam;
        }

        CodeComboBox.ItemsSource = App.DbContext.Дисциплинаs.ToList();
        RegNumberComboBox.ItemsSource = App.DbContext.Сотрудникs.Where(s => s.КодРоли == 4).ToList();
        TabNumberComboBox.ItemsSource = App.DbContext.Сотрудникs.Where(s => s.КодРоли == 3 || s.КодРоли == 2).ToList();
        GradeComboBox.ItemsSource = new List<int> { 2, 3, 4, 5 };
    }

    private void SaveButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        // Проверяем, что все обязательные поля заполнены
        if (string.IsNullOrEmpty(AudienceTextBox.Text) ||
            CodeComboBox.SelectedItem == null ||
            RegNumberComboBox.SelectedItem == null ||
            TabNumberComboBox.SelectedItem == null ||
            GradeComboBox.SelectedItem == null ||
            DatePickerControl.SelectedDate == null)
        {
            // Можно показать сообщение об ошибке
            return;
        }

        // Получаем дату из DatePicker - конвертер уже преобразовал ее в DateOnly
        var currentExam = DataContext as Экзамен;
        if (currentExam == null || currentExam.Дата == DateOnly.MinValue)
        {
            return;
        }

        DateOnly date = currentExam.Дата;

        // Проверка диапазона дат (опционально)
        if (date.Year < 2000 || date.Year > 2100)
        {
            // Можно показать сообщение об ошибке
            return;
        }

        var selectedDiscipline = CodeComboBox.SelectedItem as Дисциплина;
        var selectedStudent = RegNumberComboBox.SelectedItem as Сотрудник;
        var selectedTeacher = TabNumberComboBox.SelectedItem as Сотрудник;

        if (selectedDiscipline == null || selectedStudent == null || selectedTeacher == null)
            return;

        // Получаем значения ключевых полей
        int code = selectedDiscipline.Код;
        int regNumber = selectedStudent.ТабНомер;
        int tabNumber = selectedTeacher.ТабНомер;

        // ПРОВЕРКА НА УНИКАЛЬНОСТЬ КОМБИНАЦИИ ПОЛЕЙ (Код, РегНомер, ТабНомер)
        if (LoginVariableData.selectedExamInMainWindow != null)
        {
            // Редактирование существующего экзамена

            // Получаем текущие значения ключевых полей
            var currentCode = currentExam.Код;
            var currentRegNumber = currentExam.РегНомер;
            var currentTabNumber = currentExam.ТабНомер;

            // Проверяем, изменились ли ключевые поля
            bool keyFieldsChanged = currentCode != code ||
                                   currentRegNumber != regNumber ||
                                   currentTabNumber != tabNumber;

            if (keyFieldsChanged)
            {
                // Проверяем, не существует ли уже экзамена с новой комбинацией ключевых полей
                var existingExam = App.DbContext.Экзаменs
                    .FirstOrDefault(e => e.Код == code &&
                                        e.РегНомер == regNumber &&
                                        e.ТабНомер == tabNumber);

                if (existingExam != null)
                {
                    // Такой экзамен уже существует
                    // Можно добавить MessageBox с сообщением об ошибке
                    return;
                }
            }
        }
        else
        {
            // Создание нового экзамена

            // Проверяем, не существует ли уже экзамена с такой комбинацией ключевых полей
            var existingExam = App.DbContext.Экзаменs
                .FirstOrDefault(e => e.Код == code &&
                                    e.РегНомер == regNumber &&
                                    e.ТабНомер == tabNumber);

            if (existingExam != null)
            {
                // Такой экзамен уже существует
                // Можно добавить MessageBox с сообщением об ошибке
                return;
            }
        }

        // Обновляем значения ключевых полей
        currentExam.Код = code;
        currentExam.РегНомер = regNumber;
        currentExam.ТабНомер = tabNumber;

        // Обновляем остальные поля
        currentExam.Дата = date;
        currentExam.КодNavigation = selectedDiscipline;
        currentExam.РегНомерNavigation = selectedStudent;
        currentExam.ТабНомерNavigation = selectedTeacher;
        currentExam.Аудитория = AudienceTextBox.Text;

        if (GradeComboBox.SelectedItem is int grade)
        {
            currentExam.Оценка = grade;
        }

        if (LoginVariableData.selectedExamInMainWindow != null)
        {
            // Теперь можно использовать Update, так как есть PK
            App.DbContext.Update(currentExam);
        }
        else
        {
            App.DbContext.Add(currentExam);
        }

        App.DbContext.SaveChanges();
        this.Close();
    }

    private void DeleteButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        // Проверяем, что мы редактируем существующий экзамен
        if (LoginVariableData.selectedExamInMainWindow == null) return;

        var currentExam = DataContext as Экзамен;
        if (currentExam == null) return;

        // Получаем ключевые поля для поиска в БД
        int code = currentExam.Код;
        int regNumber = currentExam.РегНомер;
        int tabNumber = currentExam.ТабНомер;

        // Находим экзамен в базе данных
        var examToDelete = App.DbContext.Экзаменs.FirstOrDefault(e => e.Код == code && e.РегНомер == regNumber && e.ТабНомер == tabNumber);

        if (examToDelete == null) return;

        // Удаляем экзамен
        App.DbContext.Экзаменs.Remove(examToDelete);
        App.DbContext.SaveChanges();

        this.Close();
    }
}