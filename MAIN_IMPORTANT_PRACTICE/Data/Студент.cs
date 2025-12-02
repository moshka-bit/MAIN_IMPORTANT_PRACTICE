using System;
using System.Collections.Generic;

namespace MAIN_IMPORTANT_PRACTICE.Data;

public partial class Студент
{
    public int РегНомер { get; set; }

    public string Номер { get; set; } = null!;

    public virtual Специальность НомерNavigation { get; set; } = null!;

    public virtual Сотрудник? Сотрудник { get; set; }
}
