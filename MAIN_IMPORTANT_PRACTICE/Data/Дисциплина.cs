using System;
using System.Collections.Generic;

namespace MAIN_IMPORTANT_PRACTICE.Data;

public partial class Дисциплина
{
    public int Код { get; set; }

    public int Объем { get; set; }

    public string Название { get; set; } = null!;

    public string Исполнитель { get; set; } = null!;

    public virtual Кафедра ИсполнительNavigation { get; set; } = null!;

    public virtual ICollection<Экзамен> Экзаменs { get; set; } = new List<Экзамен>();

    public virtual ICollection<Специальность> Номерs { get; set; } = new List<Специальность>();
}
