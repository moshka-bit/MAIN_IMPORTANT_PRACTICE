using System;
using System.Collections.Generic;

namespace MAIN_IMPORTANT_PRACTICE.Data;

public partial class Роли
{
    public int КодРоли { get; set; }

    public string НазваниеРоли { get; set; } = null!;

    public virtual ICollection<Сотрудник> Сотрудникs { get; set; } = new List<Сотрудник>();
}
