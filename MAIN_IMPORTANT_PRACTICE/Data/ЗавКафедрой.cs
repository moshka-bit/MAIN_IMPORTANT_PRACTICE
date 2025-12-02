using System;
using System.Collections.Generic;

namespace MAIN_IMPORTANT_PRACTICE.Data;

public partial class ЗавКафедрой
{
    public int ТабНомер { get; set; }

    public int Стаж { get; set; }

    public virtual Сотрудник ТабНомерNavigation { get; set; } = null!;
}
