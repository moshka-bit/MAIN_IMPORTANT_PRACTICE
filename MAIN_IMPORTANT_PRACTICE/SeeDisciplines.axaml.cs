using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using MAIN_IMPORTANT_PRACTICE.Data;
using MAIN_IMPORTANT_PRACTICE.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MAIN_IMPORTANT_PRACTICE;

public partial class SeeDisciplines : UserControl
{
    private List<Дисциплина> allDisciplines = new List<Дисциплина>();

    public SeeDisciplines()
    {
        InitializeComponent();
        LoadAllDisciplines();
    }

    private void LoadAllDisciplines()
    {
        allDisciplines = App.DbContext.Дисциплинаs.ToList();
        DisciplineControlDataGrid.ItemsSource = allDisciplines;
    }

    private void FilterTextBox_TextChanged_1(object? sender, TextChangedEventArgs e)
    {
        ApplyFilter();
    }

    private void ApplyFilter()
    {
        string filterText = FilterTextBox.Text?.Trim();

        if (string.IsNullOrEmpty(filterText))
        {
            DisciplineControlDataGrid.ItemsSource = allDisciplines;
            return;
        }

        var filteredDisciplines = allDisciplines
            .Where(p => p.Название.Contains(filterText, StringComparison.OrdinalIgnoreCase))
            .ToList();

        DisciplineControlDataGrid.ItemsSource = filteredDisciplines;
    }
}