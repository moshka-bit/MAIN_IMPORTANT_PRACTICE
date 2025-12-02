using System;
using System.Collections.Generic;

namespace MAIN_IMPORTANT_PRACTICE.Data;

public partial class Сотрудник
{
    public int ТабНомер { get; set; }

    public string? Шифр { get; set; }

    public string Фамилия { get; set; } = null!;

    public decimal? Зарплата { get; set; }

    public int? Шеф { get; set; }

    public int КодРоли { get; set; }

    public string Логин { get; set; } = null!;

    public string Пароль { get; set; } = null!;

    public string? Номер { get; set; }

    public virtual ICollection<Сотрудник> InverseШефNavigation { get; set; } = new List<Сотрудник>();

    public virtual ЗавКафедрой? ЗавКафедрой { get; set; }

    public virtual Инженер? Инженер { get; set; }

    public virtual Роли КодРолиNavigation { get; set; } = null!;

    public virtual Преподаватель? Преподаватель { get; set; }

    public virtual Студент ТабНомерNavigation { get; set; } = null!;

    public virtual Сотрудник? ШефNavigation { get; set; }

    public virtual Кафедра? ШифрNavigation { get; set; }

    public virtual ICollection<Экзамен> ЭкзаменРегНомерNavigations { get; set; } = new List<Экзамен>();

    public virtual ICollection<Экзамен> ЭкзаменТабНомерNavigations { get; set; } = new List<Экзамен>();
}
