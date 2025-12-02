using System;
using System.Collections.Generic;

namespace MAIN_IMPORTANT_PRACTICE.Data;

public partial class Преподаватель
{
    public int ТабНомер { get; set; }

    public string? Звание { get; set; }

    public string? Степень { get; set; }

    public virtual Сотрудник ТабНомерNavigation { get; set; } = null!;
}
