using System.Reflection;
using System.ComponentModel.DataAnnotations;


namespace ToDoList.Domain.Extensions
{
    public static class EnumExtension
    {
        public static string GetDisplayName(this System.Enum EnumValue)
        { // Получает свойства из Display(Name="")
            return EnumValue.GetType()
                .GetMember(EnumValue.ToString())
                .First()
                .GetCustomAttribute<DisplayAttribute>()
                ?.GetName() ?? "Неопределенный";
        }
    }
}
