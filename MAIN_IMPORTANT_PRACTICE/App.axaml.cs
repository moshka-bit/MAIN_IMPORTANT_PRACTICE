using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using MAIN_IMPORTANT_PRACTICE.Data;
using System.Linq;

namespace MAIN_IMPORTANT_PRACTICE
{
    public partial class App : Application
    {
        public static AppDbContext DbContext { get; private set; } = new AppDbContext();

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
            DbContext.Дисциплинаs.ToList();
            DbContext.ЗавКафедройs.ToList();
            DbContext.Инженерs.ToList();
            DbContext.Кафедраs.ToList();
            DbContext.Преподавательs.ToList();
            DbContext.Ролиs.ToList();
            DbContext.Сотрудникs.ToList();
            DbContext.Специальностьs.ToList();
            DbContext.Факультетs.ToList();
            DbContext.Экзаменs.ToList();
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow();
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}