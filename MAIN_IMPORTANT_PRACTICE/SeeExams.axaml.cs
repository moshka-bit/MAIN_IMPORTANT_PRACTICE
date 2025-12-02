using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using MAIN_IMPORTANT_PRACTICE.Data;
using MAIN_IMPORTANT_PRACTICE.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace MAIN_IMPORTANT_PRACTICE;

public partial class SeeExams : UserControl
{
    private List<Экзамен> allExams = new List<Экзамен>();

    public SeeExams()
    {
        InitializeComponent();
        LoadFilteredExams();
    }

    private void LoadFilteredExams()
    {
        var currentUser = LoginVariableData.selectedUserInMainWindow;

        IQueryable<Экзамен> examsQuery = App.DbContext.Экзаменs
            .Include(e => e.КодNavigation)
            .Include(e => e.РегНомерNavigation)
            .Include(e => e.ТабНомерNavigation);

        if (currentUser != null)
        {
            examsQuery = examsQuery.Where(e => e.РегНомер == currentUser.ТабНомер);
        }

        allExams = examsQuery.ToList();

        ExamControlDataGrid.ItemsSource = allExams;
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
            ExamControlDataGrid.ItemsSource = allExams;
            return;
        }

        var filteredExams = allExams
            .Where(p => p.Оценка.ToString().Contains(filterText, System.StringComparison.OrdinalIgnoreCase))
            .ToList();

        ExamControlDataGrid.ItemsSource = filteredExams;
    }
}