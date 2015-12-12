using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public class TriggerJSModel
    {
        public string remark { get; set; }

        public levelInfo levelInfo { get; set; }

        public List<DayModel> beforeList { get; set; }

        public List<DayModel> bookingList { get; set; }

        public List<MatchModel> matchList { get; set; }

        public DateTime start { get; set; }

        public DateTime end { get; set; }
    }

    public class levelInfo
    {
        public int id { get; set; }

        public string text { get; set; }

        public int parentId { get; set; }
    }

    public class DayModel
    {
        public int day { get; set; }

        public decimal percent { get; set; }

    }

    public class MatchModel
    {
        public long sceneId { get; set; }

        public int priority { get; set; }

    }
}
