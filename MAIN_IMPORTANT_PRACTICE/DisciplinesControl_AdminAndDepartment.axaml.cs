using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using MAIN_IMPORTANT_PRACTICE.Data;
using MAIN_IMPORTANT_PRACTICE.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MAIN_IMPORTANT_PRACTICE;

public partial class DisciplinesControl_AdminAndDepartment : UserControl
{
    // дисциплины для фильтрации
    private List<Дисциплина> allDisciplines = new List<Дисциплина>();

    public DisciplinesControl_AdminAndDepartment()
    {
        InitializeComponent();
        LoadAllDisciplines();
    }

    // загружаем товары
    private void LoadAllDisciplines()
    {
        allDisciplines = App.DbContext.Дисциплинаs.ToList();
        DisciplineControlDataGrid.ItemsSource = allDisciplines;
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
            DisciplineControlDataGrid.ItemsSource = allDisciplines;
            return;
        }

        // применяем фильтрацию
        var filteredDisciplines = allDisciplines
            .Where(p => p.Название.Contains(filterText, StringComparison.OrdinalIgnoreCase))
            .ToList();

        DisciplineControlDataGrid.ItemsSource = filteredDisciplines;
    }

    private async void AddButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        LoginVariableData.selectedDisciplineInMainWindow = null;

        var parent = this.VisualRoot as Window;
        var createAndChangeDiscipline = new CreateAndChangeDiscipline();
        await createAndChangeDiscipline.ShowDialog(parent);
    }

    private async void DisciplineControlDataGrid_DoubleTapped(object? sender, Avalonia.Input.TappedEventArgs e)
    {
        // забираем кликнутую дисциплину
        var selectedDiscipline = DisciplineControlDataGrid.SelectedItem as Дисциплина;
        if (selectedDiscipline == null) return;

        // записываем Discipline
        LoginVariableData.selectedDisciplineInMainWindow = selectedDiscipline;

        var parent = this.VisualRoot as Window;
        if (parent == null) return;
        var createAndChangeDiscipline = new CreateAndChangeDiscipline();
        await createAndChangeDiscipline.ShowDialog(parent);

        LoadAllDisciplines();
    }
}