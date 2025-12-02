using System;
using System.Collections.Generic;

namespace MAIN_IMPORTANT_PRACTICE.Data;

public partial class Экзамен
{
    public DateOnly Дата { get; set; }

    public int Код { get; set; }

    public int РегНомер { get; set; }

    public int ТабНомер { get; set; }

    public string Аудитория { get; set; } = null!;

    public int Оценка { get; set; }

    public int КодЭкзамена { get; set; }

    public virtual Дисциплина КодNavigation { get; set; } = null!;

    public virtual Сотрудник РегНомерNavigation { get; set; } = null!;

    public virtual Сотрудник ТабНомерNavigation { get; set; } = null!;
}
