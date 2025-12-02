using System;
using System.Collections.Generic;

namespace MAIN_IMPORTANT_PRACTICE.Data;

public partial class Инженер
{
    public int ТабНомер { get; set; }

    public string Специальность { get; set; } = null!;

    public virtual Сотрудник ТабНомерNavigation { get; set; } = null!;
}
