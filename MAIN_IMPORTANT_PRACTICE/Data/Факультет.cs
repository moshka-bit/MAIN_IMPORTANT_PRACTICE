using System;
using System.Collections.Generic;

namespace MAIN_IMPORTANT_PRACTICE.Data;

public partial class Факультет
{
    public string Аббревиатура { get; set; } = null!;

    public string Название { get; set; } = null!;

    public virtual ICollection<Кафедра> Кафедраs { get; set; } = new List<Кафедра>();
}
