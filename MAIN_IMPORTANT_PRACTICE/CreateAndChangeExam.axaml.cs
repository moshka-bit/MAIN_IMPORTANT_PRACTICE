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
            var loadedExam = App.DbContext.Экзаменs
                .Include(e => e.КодNavigation)
                .Include(e => e.РегНомерNavigation)
                .Include(e => e.ТабНомерNavigation)
                .FirstOrDefault(e => e.КодЭкзамена == LoginVariableData.selectedExamInMainWindow.КодЭкзамена);

            DataContext = loadedExam ?? LoginVariableData.selectedExamInMainWindow;
        }
        else
        {
            var newExam = new Экзамен
            {
                Дата = DateOnly.FromDateTime(System.DateTime.Today)
            };
            DataContext = newExam;
        }

        CodeComboBox.ItemsSource = App.DbContext.Дисциплинаs.ToList();
        RegNumberComboBox.ItemsSource = App.DbContext.Сотрудникs.Where(s => s.КодРоли == 4).ToList();
        TabNumberComboBox.ItemsSource = App.DbContext.Сотрудникs.Where(s => s.КодРоли == 3 || s.КодРоли == 2).ToList();
        GradeComboBox.ItemsSource = new List<int> { 2, 3, 4, 5 };

        var currentUser = LoginVariableData.selectedUserInMainWindow;
        if (currentUser != null && currentUser.КодРоли == 3)
        {
            CodeComboBox.IsEnabled = false;
            RegNumberComboBox.IsEnabled = false;
            TabNumberComboBox.IsEnabled = false;
            AudienceTextBox.IsReadOnly = true;
            DatePickerControl.IsEnabled = false;
        }
    }

    private void SaveButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var currentUser = LoginVariableData.selectedUserInMainWindow;
        var currentExam = DataContext as Экзамен;
        if (currentExam == null) return;

        if (GradeComboBox.SelectedItem is not int grade) return;

        if (currentUser != null && currentUser.КодРоли == 3)
        {
            currentExam.Оценка = grade;
            App.DbContext.Update(currentExam);
            App.DbContext.SaveChanges();
            this.Close();
            return;
        }

        if (string.IsNullOrWhiteSpace(AudienceTextBox.Text) ||
            CodeComboBox.SelectedItem == null ||
            RegNumberComboBox.SelectedItem == null ||
            TabNumberComboBox.SelectedItem == null ||
            DatePickerControl.SelectedDate == null) return;

        if (currentExam.Дата == DateOnly.MinValue) return;

        DateOnly date = currentExam.Дата;
        if (date.Year < 2000 || date.Year > 2100) return;

        var selectedDiscipline = CodeComboBox.SelectedItem as Дисциплина;
        var selectedStudent = RegNumberComboBox.SelectedItem as Сотрудник;
        var selectedTeacher = TabNumberComboBox.SelectedItem as Сотрудник;

        if (selectedDiscipline == null || selectedStudent == null || selectedTeacher == null) return;

        int code = selectedDiscipline.Код;
        int regNumber = selectedStudent.ТабНомер;
        int tabNumber = selectedTeacher.ТабНомер;

        if (LoginVariableData.selectedExamInMainWindow != null)
        {
            var currentCode = currentExam.Код;
            var currentRegNumber = currentExam.РегНомер;
            var currentTabNumber = currentExam.ТабНомер;

            bool keyFieldsChanged = currentCode != code ||
                                   currentRegNumber != regNumber ||
                                   currentTabNumber != tabNumber;

            if (keyFieldsChanged)
            {
                var existingExam = App.DbContext.Экзаменs
                    .FirstOrDefault(e => e.Код == code &&
                                         e.РегНомер == regNumber &&
                                         e.ТабНомер == tabNumber);

                if (existingExam != null) return;
            }
        }
        else
        {
            var existingExam = App.DbContext.Экзаменs
                .FirstOrDefault(e => e.Код == code &&
                                     e.РегНомер == regNumber &&
                                     e.ТабНомер == tabNumber);

            if (existingExam != null) return;
        }

        currentExam.Код = code;
        currentExam.РегНомер = regNumber;
        currentExam.ТабНомер = tabNumber;

        currentExam.Дата = date;
        currentExam.КодNavigation = selectedDiscipline;
        currentExam.РегНомерNavigation = selectedStudent;
        currentExam.ТабНомерNavigation = selectedTeacher;
        currentExam.Аудитория = AudienceTextBox.Text.Trim();
        currentExam.Оценка = grade;

        if (LoginVariableData.selectedExamInMainWindow != null)
        {
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
        if (LoginVariableData.selectedExamInMainWindow == null) return;

        var currentExam = DataContext as Экзамен;
        if (currentExam == null) return;

        int code = currentExam.Код;
        int regNumber = currentExam.РегНомер;
        int tabNumber = currentExam.ТабНомер;

        var examToDelete = App.DbContext.Экзаменs
            .FirstOrDefault(e => e.Код == code &&
                                 e.РегНомер == regNumber &&
                                 e.ТабНомер == tabNumber);

        if (examToDelete == null) return;
        App.DbContext.Экзаменs.Remove(examToDelete);
        App.DbContext.SaveChanges();

        this.Close();
    }
}