using Event_Management_Mod;
using Event_Management_Mod.Models;
using Event_Management_Serv.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;


namespace Program_main
{
    public class Mainclass
    {
        public static void Main(string[] args)
        {
            EventManagement obj = new EventManagement();
            obj.RegEvent();
            obj.SearchbyId();
            obj.Searchbyname();
            obj.Update();

        }
    }
}

