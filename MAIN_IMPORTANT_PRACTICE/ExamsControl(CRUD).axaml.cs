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

public partial class ExamsControl_CRUD_ : UserControl
{
    // экзамены для фильтрации
    private List<Экзамен> allExams = new List<Экзамен>();

    public ExamsControl_CRUD_()
    {
        InitializeComponent();
        LoadAllExams(); // Переименовал метод
    }

    // загружаем экзамены ВМЕСТЕ с навигационными свойствами
    private void LoadAllExams()
    {
        allExams = App.DbContext.Экзаменs
            .Include(e => e.КодNavigation)           // Загружаем дисциплину
            .Include(e => e.РегНомерNavigation)      // Загружаем сотрудника (для студента)
            .ThenInclude(s => s.ТабНомерNavigation)  // Загружаем студента через сотрудника
            .Include(e => e.ТабНомерNavigation)      // Загружаем преподавателя
            .ToList();

        ExamControlDataGrid.ItemsSource = allExams;
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
            ExamControlDataGrid.ItemsSource = allExams;
            return;
        }

        // применяем фильтрацию по оценке
        var filteredExams = allExams
            .Where(p => p.Оценка.ToString().Contains(filterText, StringComparison.OrdinalIgnoreCase))
            .ToList();

        ExamControlDataGrid.ItemsSource = filteredExams;
    }

    private async void AddButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        LoginVariableData.selectedExamInMainWindow = null;

        var parent = this.VisualRoot as Window;
        var createAndChangeExam = new CreateAndChangeExam();
        await createAndChangeExam.ShowDialog(parent);

        // Обновляем список после закрытия окна
        LoadAllExams();
    }

    private async void ExamControlDataGrid_DoubleTapped(object? sender, Avalonia.Input.TappedEventArgs e)
    {
        // забираем экзамен
        var selectedExam = ExamControlDataGrid.SelectedItem as Экзамен;
        if (selectedExam == null) return;

        // записываем Exam
        LoginVariableData.selectedExamInMainWindow = selectedExam;

        var parent = this.VisualRoot as Window;
        if (parent == null) return;

        var createAndChangeExam = new CreateAndChangeExam();
        await createAndChangeExam.ShowDialog(parent);

        LoadAllExams();
    }
}