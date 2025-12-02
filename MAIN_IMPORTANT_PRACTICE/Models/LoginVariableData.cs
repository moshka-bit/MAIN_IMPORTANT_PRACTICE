using MAIN_IMPORTANT_PRACTICE.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAIN_IMPORTANT_PRACTICE.Models
{
    internal class LoginVariableData
    {
        // основной пользователь
        public static Сотрудник selectedUserInMainWindow {  get; set; }
        // дисциплина
        public static Дисциплина selectedDisciplineInMainWindow { get; set; }
        // преподаватель
        public static Преподаватель selectedTeacherInMainWindow { get; set; }
        // экзамен
        public static Экзамен selectedExamInMainWindow { get; set; }
        // студент
        public static Студент selectedStudentInMainWindow {  get; set; }
    }
}
