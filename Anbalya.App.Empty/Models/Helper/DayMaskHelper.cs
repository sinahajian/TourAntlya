using System.Collections.Generic;
using System.Linq;

namespace Models.Helper
{
    public static class DayMaskHelper
    {
        private static readonly DayMaskOption[] _options = new[]
        {
            new DayMaskOption(0, "Monday", "Mon"),
            new DayMaskOption(1, "Tuesday", "Tue"),
            new DayMaskOption(2, "Wednesday", "Wed"),
            new DayMaskOption(3, "Thursday", "Thu"),
            new DayMaskOption(4, "Friday", "Fri"),
            new DayMaskOption(5, "Saturday", "Sat"),
            new DayMaskOption(6, "Sunday", "Sun")
        };

        public static IReadOnlyList<DayMaskOption> Options => _options;

        public static int FromSelectedDays(IEnumerable<int> selected)
        {
            if (selected is null) return 0;
            int mask = 0;
            foreach (var index in selected.Distinct())
            {
                if (index < 0 || index >= 7) continue;
                mask |= (1 << index);
            }
            return mask;
        }

        public static List<int> ToSelectedDays(int mask)
        {
            mask = NormalizeMask(mask);
            var list = new List<int>(7);
            for (int i = 0; i < 7; i++)
            {
                if ((mask & (1 << i)) != 0)
                {
                    list.Add(i);
                }
            }
            return list;
        }

        private static int NormalizeMask(int mask)
        {
            if (mask <= 0)
            {
                return 0;
            }

            var digits = mask.ToString();
            if (digits.Length == 7 && digits.All(c => c is '0' or '1'))
            {
                int normalized = 0;
                for (int i = 0; i < digits.Length; i++)
                {
                    if (digits[digits.Length - 1 - i] == '1')
                    {
                        normalized |= 1 << i;
                    }
                }
                return normalized;
            }

            return mask;
        }
    }

    public record DayMaskOption(int Index, string DisplayName, string Abbreviation);
}
