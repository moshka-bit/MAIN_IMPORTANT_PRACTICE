using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using MAIN_IMPORTANT_PRACTICE.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MAIN_IMPORTANT_PRACTICE;

public partial class SeeStudents : UserControl
{
    // студенты для фильтрации
    private List<Сотрудник> allStudents = new List<Сотрудник>();
    public SeeStudents()
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
}