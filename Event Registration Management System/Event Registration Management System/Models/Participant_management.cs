using System;
using System.Collections.Generic;
using System.Text;

namespace Participant_Management_mod.Models
{
    public class PartMan
    {
        public int PId { get; set; }
        public string PName { get; set; }
        public string Email { get; set; }
        public int Phno { get; set; }

        public PartMan(int pid, string pname, string email,int phno)
        {
            PId = pid;
            PName = pname;
            Email = email;
            Phno = phno;
        }
    }
}
