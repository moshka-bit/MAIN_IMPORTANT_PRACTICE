using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using MAIN_IMPORTANT_PRACTICE.Data;
using MAIN_IMPORTANT_PRACTICE.Models;
using System.Linq;

namespace MAIN_IMPORTANT_PRACTICE;

public partial class CreateAndChangeDiscipline : Window
{
    public CreateAndChangeDiscipline()
    {
        InitializeComponent();

        if (LoginVariableData.selectedDisciplineInMainWindow != null)
        {
            DataContext = LoginVariableData.selectedDisciplineInMainWindow;
            CodeTextBox.IsReadOnly = true;
        }
        else
        {
            DataContext = new Дисциплина();
            CodeTextBox.IsReadOnly = false;
        }

        KafedraComboBox.ItemsSource = App.DbContext.Кафедраs.ToList();
    }

    private async void SaveButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(CodeTextBox.Text) || string.IsNullOrEmpty(VolumeTextBox.Text) || string.IsNullOrEmpty(NameTextBox.Text) || KafedraComboBox.SelectedItem == null)
        {
            return;
        }

        if (!int.TryParse(CodeTextBox.Text, out int code)) return;

        if (!int.TryParse(VolumeTextBox.Text, out int volume)) return;

        if (code == 0) return;

        if (volume == 0) return;

        var currentDiscipline = DataContext as Дисциплина;
        if (currentDiscipline == null) return;

        if (LoginVariableData.selectedDisciplineInMainWindow != null)
        {
            if (currentDiscipline.Код != code) return;

            currentDiscipline.Объем = volume;
            currentDiscipline.Название = NameTextBox.Text;
            currentDiscipline.ИсполнительNavigation = KafedraComboBox.SelectedItem as Кафедра;

            App.DbContext.Update(currentDiscipline);
        }
        else
        {
            var existing_code = App.DbContext.Дисциплинаs.FirstOrDefault(d => d.Код == code);
            if (existing_code != null) return;

            currentDiscipline.Код = code;
            currentDiscipline.Объем = volume;
            currentDiscipline.Название = NameTextBox.Text;
            currentDiscipline.ИсполнительNavigation = KafedraComboBox.SelectedItem as Кафедра;

            App.DbContext.Add(currentDiscipline);
        }

        App.DbContext.SaveChanges();
        this.Close();
    }

    private void DeleteButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (LoginVariableData.selectedDisciplineInMainWindow != null)
        {
            var currentDiscipline = DataContext as Дисциплина;
            if (currentDiscipline != null)
            {
                App.DbContext.Дисциплинаs.Remove(currentDiscipline);
                App.DbContext.SaveChanges();
            }
        }
        this.Close();
    }
}