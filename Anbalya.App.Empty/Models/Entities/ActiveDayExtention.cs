


using Microsoft.Net.Http.Headers;

namespace Models.Entities
{
    public class TourDays
    {
        public bool Monday { get; set; } = false;
        public bool Tuesday { get; set; } = false;
        public bool Wednesday { get; set; } = false;
        public bool Thursday { get; set; } = false;
        public bool Friday { get; set; } = false;
        public bool Saturday { get; set; } = false;
        public bool Sunday { get; set; } = false;
        public TourDays()
        {

        }
        public void SetDay(int day)
        {
            switch (day)
            {
                case 0: Monday = true; break;
                case 1: Tuesday = true; break;
                case 2: Wednesday = true; break;
                case 3: Thursday = true; break;
                case 4: Friday = true; break;
                case 5: Saturday = true; break;
                case 6: Sunday = true; break;
            }
        }
    }

    public static class DaysOfTour
    {
        public static int ToInt(this TourDays day)
        {
            var num = 0b1111111;
            if (day.Sunday == false)
            {
                num &= ~(1 << 0);
            }
            if (day.Saturday == false)
            {
                num &= ~(1 << 1);
            }
            if (day.Friday == false)
            {
                num &= ~(1 << 2);
            }
            if (day.Thursday == false)
            {
                num &= ~(1 << 3);
            }
            if (day.Wednesday == false)
            {
                num &= ~(1 << 4);
            }
            if (day.Tuesday == false)
            {
                num &= ~(1 << 5);
            }
            if (day.Monday == false)
            {
                num &= ~(1 << 6);
            }



            return num;
        }
        public static TourDays ToDays(this int dayInt)
        {
            var tourDays = new TourDays();
            var day = dayInt.ToString("");
            var dayNum = 6;
            for (var i = day.Length; 0 < day.Length; i--)
            {
                if (day[dayNum] == 1) { tourDays.SetDay(dayNum); }
                dayNum--;
            }

            return tourDays;
        }
    }
}



