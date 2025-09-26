using System.Reflection;
using System.ComponentModel.DataAnnotations;
using Models.Entities;
namespace Models.Helper
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            var member = enumValue.GetType().GetMember(enumValue.ToString()).First();
            var attr = member.GetCustomAttribute<DisplayAttribute>();
            return attr?.GetName() ?? enumValue.ToString();
        }
        public static string GetDisplayName(this Category category, string lang)
        {
            return lang switch
            {
                "de" => category switch
                {
                    Category.Relaxing => "Entspannung",
                    Category.Adventure => "Abenteuer",
                    Category.History => "Geschichte",
                    Category.Nature => "Natur",
                    Category.Family => "Familie",
                    Category.Special => "Spezial",
                    _ => category.ToString()
                },
                "fa" => category switch
                {
                    Category.Relaxing => "آرامش",
                    Category.Adventure => "ماجراجویی",
                    Category.History => "تاریخ",
                    Category.Nature => "طبیعت",
                    Category.Family => "خانواده",
                    Category.Special => "ویژه",
                    _ => category.ToString()
                },
                "ru" => category switch
                {
                    Category.Relaxing => "Отдых",
                    Category.Adventure => "Приключение",
                    Category.History => "История",
                    Category.Nature => "Природа",
                    Category.Family => "Семья",
                    Category.Special => "Особый",
                    _ => category.ToString()
                },
                "pl" => category switch
                {
                    Category.Relaxing => "Relaks",
                    Category.Adventure => "Przygoda",
                    Category.History => "Historia",
                    Category.Nature => "Natura",
                    Category.Family => "Rodzina",
                    Category.Special => "Specjalny",
                    _ => category.ToString()
                },
                "ar" => category switch
                {
                    Category.Relaxing => "استرخاء",
                    Category.Adventure => "مغامرة",
                    Category.History => "تاريخ",
                    Category.Nature => "طبيعة",
                    Category.Family => "عائلة",
                    Category.Special => "خاص",
                    _ => category.ToString()
                },
                _ => category.ToString() // fallback → انگلیسی
            };
        }


    }
}


