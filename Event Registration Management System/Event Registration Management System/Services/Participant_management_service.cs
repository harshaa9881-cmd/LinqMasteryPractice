using Event_Management_Mod.Models;
using Participant_Management_mod.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Participant_Registration_Management_Serv.Services
{
    public class Participants
    {
        public List<PartMan> Rman = new List<PartMan>();

        //event registration
        public void Reg_participant()
        {
            Console.WriteLine("Welcome to Registration Process");

            Console.Write("Enter participant ID: ");
            string? pid = Console.ReadLine();


            // Check whether Event exists& null as eid
            if (!int.TryParse(pid, out int id))
            {
                Console.WriteLine("Invalid input. Please enter a valid number.");
                return;
            }
            if (string.IsNullOrWhiteSpace(pid))
                Console.WriteLine("Event id cannot be empty ");
            else if (Rman.Any(e => e.PId == id))
                Console.WriteLine("event id already found");

            // Check whether Eventname is null

            Console.Write("Enter Full Name of Participant: ");
            string? EveName = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(EveName))
            {
                Console.WriteLine("Event Name cannot be empty.");
                return;
            }

            //email 
            Console.Write("Enter Email: ");
            string? Email = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(Email))
            {
                Console.WriteLine("Event venue cannot be empty ");
                return;
            }

            
            //Contact
            Console.Write("Enter Contact number: ");
            string? con = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(con))
                Console.WriteLine("Event capacity cannot be empty ");
            if (!int.TryParse(con, out int contact))
            {
                Console.WriteLine("Invalid input. Please enter in a valid number.");
                return;
            }
            if (con.Length != 10)
            {
                Console.WriteLine("Invalid number, must be 10 digit");
                return;
            }

            
        }
    }

}
