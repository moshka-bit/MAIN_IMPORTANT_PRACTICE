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

public partial class StudentControl_CRUD_ : UserControl
{
    // студенты для фильтрации
    private List<Сотрудник> allStudents = new List<Сотрудник>();
    public StudentControl_CRUD_()
    {
        InitializeComponent();
        LoadAllStudents();
    }
    // загружаем студентов
    private void LoadAllStudents()
    {
        allStudents = App.DbContext.Сотрудникs.Where(s => s.КодРоли == 4).ToList();
        StudentControlDataGrid.ItemsSource = allStudents;
    }

    // Обработчик изменения текста в поле фильтра
    private void FilterTextBox_TextChanged_1(object? sender, TextChangedEventArgs e)
    {
        ApplyFilter();
    }

    // применяем фильтр
    private void ApplyFilter()
    {
        string filterText = FilterTextBox.Text?.Trim();

        // если фильтра нет, то показываем всё
        if (string.IsNullOrEmpty(filterText))
        {
            StudentControlDataGrid.ItemsSource = allStudents;
            return;
        }

        // применяем фильтрацию
        var filteredTeachers = allStudents
            .Where(p => p.Фамилия.Contains(filterText, StringComparison.OrdinalIgnoreCase))
            .ToList();

        StudentControlDataGrid.ItemsSource = filteredTeachers;
    }

    private async void AddButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        LoginVariableData.selectedStudentInMainWindow = null;

        var parent = this.VisualRoot as Window;
        var createAndChangeStudent = new CreateAndChangeStudent();
        await createAndChangeStudent.ShowDialog(parent);
    }

    private async void StudentControlDataGrid_DoubleTapped(object? sender, Avalonia.Input.TappedEventArgs e)
    {
        // забираем студента
        var selectedStudent = StudentControlDataGrid.SelectedItem as Сотрудник;
        if (selectedStudent == null) return;

        var RegNumberProperty = selectedStudent.GetType().GetProperty("ТабНомер");
        if (RegNumberProperty == null) return;

        var RegNumber = (int)RegNumberProperty.GetValue(selectedStudent);

        var student = App.DbContext.Студентs.Include(t => t.Сотрудник).FirstOrDefault(t => t.РегНомер == RegNumber);
        if (student == null) return;

        LoginVariableData.selectedStudentInMainWindow = student;

        var parent = this.VisualRoot as Window;
        if (parent == null) return;
        var createAndChangeStudent = new CreateAndChangeStudent();
        await createAndChangeStudent.ShowDialog(parent);

        LoadAllStudents();
    }
}