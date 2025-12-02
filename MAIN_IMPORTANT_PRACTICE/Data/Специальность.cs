using System;
using System.Collections.Generic;

namespace MAIN_IMPORTANT_PRACTICE.Data;

public partial class Специальность
{
    public string Номер { get; set; } = null!;

    public string Направление { get; set; } = null!;

    public string Шифр { get; set; } = null!;

    public virtual ICollection<Студент> Студентs { get; set; } = new List<Студент>();

    public virtual Кафедра ШифрNavigation { get; set; } = null!;

    public virtual ICollection<Дисциплина> Кодs { get; set; } = new List<Дисциплина>();
}
