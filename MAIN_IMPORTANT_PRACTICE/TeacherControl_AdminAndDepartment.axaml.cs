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

public partial class TeacherControl_AdminAndDepartment : UserControl
{
    // дисциплины для фильтрации
    private List<Сотрудник> allTeachers = new List<Сотрудник>();
    public TeacherControl_AdminAndDepartment()
    {
        InitializeComponent();
        LoadAllTeachers();
    }
    // загружаем преподавателей
    private void LoadAllTeachers()
    {
        allTeachers = App.DbContext.Сотрудникs.Where(s => s.КодРоли == 3).ToList();
        TeacherControlDataGrid.ItemsSource = allTeachers;
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
            TeacherControlDataGrid.ItemsSource = allTeachers;
            return;
        }

        // применяем фильтрацию
        var filteredTeachers = allTeachers
            .Where(p => p.Фамилия.Contains(filterText, StringComparison.OrdinalIgnoreCase))
            .ToList();

        TeacherControlDataGrid.ItemsSource = filteredTeachers;
    }

    private async void TeacherControlDataGrid_DoubleTapped(object? sender, Avalonia.Input.TappedEventArgs e)
    {
        // забираем преподавателя
        var selectedTeacher = TeacherControlDataGrid.SelectedItem as Сотрудник;
        if (selectedTeacher == null) return;

        var TabNumberProperty = selectedTeacher.GetType().GetProperty("ТабНомер");
        if (TabNumberProperty == null) return;

        var TabNumber = (int)TabNumberProperty.GetValue(selectedTeacher);

        var teacher = App.DbContext.Преподавательs.Include(t => t.ТабНомерNavigation).FirstOrDefault(t => t.ТабНомер == TabNumber);
        if (teacher == null) return;

        LoginVariableData.selectedTeacherInMainWindow = teacher;

        var parent = this.VisualRoot as Window;
        if (parent == null) return;
        var createAndChangeTeacher = new CreateAndChangeTeacher();
        await createAndChangeTeacher.ShowDialog(parent);

        LoadAllTeachers();
    }

    private async void AddButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        LoginVariableData.selectedTeacherInMainWindow = null;

        var parent = this.VisualRoot as Window;
        var createAndChangeTeacher = new CreateAndChangeTeacher();
        await createAndChangeTeacher.ShowDialog(parent);
    }
}