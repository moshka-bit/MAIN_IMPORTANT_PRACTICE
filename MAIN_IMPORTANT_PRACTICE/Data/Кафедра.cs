using System;
using System.Collections.Generic;

namespace MAIN_IMPORTANT_PRACTICE.Data;

public partial class Кафедра
{
    public string Шифр { get; set; } = null!;

    public string Название { get; set; } = null!;

    public string Факультет { get; set; } = null!;

    public virtual ICollection<Дисциплина> Дисциплинаs { get; set; } = new List<Дисциплина>();

    public virtual ICollection<Сотрудник> Сотрудникs { get; set; } = new List<Сотрудник>();

    public virtual ICollection<Специальность> Специальностьs { get; set; } = new List<Специальность>();

    public virtual Факультет ФакультетNavigation { get; set; } = null!;
}
