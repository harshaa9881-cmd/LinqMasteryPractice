using System;
using System.Collections.Generic;
using System.Text;

namespace Event_Management_Mod.Models
{
    public class EventMan
    {
        public int EventId { get; set; }
        public string EventName { get; set; }
        public string venue { get; set; }
        public DateTime EventDate { get; set; }
        public int Capacity { get; set; }
        public decimal RegFee { get; set; }

        public EventMan(int eid,string ename,string evenue,
                        DateTime edate,int ecap,decimal efee
                        )
        {
            EventId =eid;
            EventName = ename;
            venue= evenue;
            EventDate = edate;
            Capacity= ecap;
            RegFee= efee;
        }
    }
}
