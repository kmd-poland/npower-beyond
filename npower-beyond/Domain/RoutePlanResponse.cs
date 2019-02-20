using System;
namespace npower_beyond.Domain
{
    public class RoutePlan
    {
        public Visit[] Visits { get; set; }
     }

    public class Visit
    {
        public TimeSpan StartTime { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Avatar { get; set; }
        public string Address { get; set; }
        public double[] Coordinates { get; set; }
       
    }
}
