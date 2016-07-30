using System;

namespace PosApp.MockService
{
    public interface IDateTime
    {
        string GetWeekDay();
    }

    class SystemDateTime : IDateTime
    {
        public string GetWeekDay()
        {
            return DateTime.Now.DayOfWeek.ToString();
        }
    }
}