using Event_Management_Mod;
using Event_Management_Mod.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Event_Management_Serv.Services
{
    public class EventManagement
    {
        //list for eventmanagement
        public List<EventMan> Eman = new List<EventMan>();

        //event registration
        public void RegEvent()
        {
            Console.WriteLine("Welcome to Registration Process");

            Console.Write("Enter Event ID: ");
            string? EventMId = Console.ReadLine();


            // Check whether Event exists& null as eid
            if (!int.TryParse(EventMId, out int eid))
            {
                Console.WriteLine("Invalid input. Please enter a valid number.");
                return;
            }
            if (string.IsNullOrWhiteSpace(EventMId))
                Console.WriteLine("Event id cannot be empty ");
            else if (Eman.Any(e => e.EventId == eid))
                Console.WriteLine("event id already found");

            // Check whether Eventname is null

            Console.Write("Enter Event Name: ");
            string? EveName = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(EveName))
            {
                Console.WriteLine("Event Name cannot be empty.");
                return;
            }

            //venue available 
            Console.Write("Enter Event Venue: ");
            string? EveVenue = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(EveVenue))
            {
                Console.WriteLine("Event venue cannot be empty ");
                return;
            }

            //Date availability as DT
            Console.Write("Enter Event Venue (DD-MM-YYYY),(HH:MM): ");
            string? edate = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(edate))
                Console.WriteLine("Event date cannot be empty ");
            if (!DateTime.TryParse(edate, out DateTime DT))
            {
                Console.WriteLine("Invalid input. Please enter in a valid format.");
                return;
            }
            if (DT < DateTime.Now)
            {
                Console.WriteLine("Date and time must be Future date.");
                return;
            }

            //Capacity as capacity
            Console.Write("Enter capacity of the event: ");
            string? cap = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(cap))
                Console.WriteLine("Event capacity cannot be empty ");
            if (!int.TryParse(cap, out int capacity))
            {
                Console.WriteLine("Invalid input. Please enter in a valid number.");
                return;
            }
            if (capacity! <= 0)
            {
                Console.WriteLine("Invalid number, must be positive");
                return;
            }
            //non-negative as efee
            Console.Write("Enter the Fee: ");
            string? fee = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(fee))
                Console.WriteLine("Event fee cannot be empty ");
            if (!int.TryParse(fee, out int efee))
            {
                Console.WriteLine("Invalid input. Please enter in a valid number.");
                return;
            }
            if (efee! >= 0)
            {
                Console.WriteLine("Invalid number, must be positive");
                return;
            }
            EventMan eventRecord = new EventMan(eid, EveName, EveVenue, DT, capacity, efee);

            Eman.Add(eventRecord);
        }
        public void Display()
        {
            if (Eman == null)
            {
                Console.WriteLine("no record found");
            }
            else
            {
                foreach (var e in Eman)
                {
                    Console.WriteLine("Eventid: " + e.EventId, "EventName: " + e.EventName, "EventVenue: " + e.venue, "Event date and time: " + e.EventDate, "EventCapacity: " + e.Capacity, "EventFee: " + e.RegFee);
                }
            }

        }
        public void SearchbyId()
        {
            Console.WriteLine("Enter the Id to search ");
            string? EventMId = Console.ReadLine();


            // Check whether Event exists& null as eid
            if (!int.TryParse(EventMId, out int eid))
            {
                Console.WriteLine("Invalid input. Please enter a valid number.");
                return;
            }
            if (string.IsNullOrWhiteSpace(EventMId))
                Console.WriteLine("Event id cannot be empty ");
            else if (Eman.Any(e => e.EventId == eid))
            {
                Console.WriteLine("id found");
                var id = Eman.Where(e => e.EventId == eid);
                foreach (var i in id)
                {
                    Console.WriteLine($"Eventid: {i.EventId}, EventName: {i.EventName}, EventVenue:  {i.venue}, Event date and time:  {i.EventDate}, EventCapacity:  {i.Capacity}, EventFee: {i.RegFee} ");
                }
            }
            else
            {
                Console.WriteLine("id not found");
            }
        }
        //Search events by name or venue.
        public void Searchbyname()
        {
            Console.WriteLine("Enter the name or venue to search ");
            string Eventnorv = Console.ReadLine().ToLower();


            // Check whether Event exists& null as eid

            if (string.IsNullOrWhiteSpace(Eventnorv))
                Console.WriteLine("Event id cannot be empty ");
            else if (Eman.Any(e => e.venue.ToLower() == Eventnorv) || Eman.Any(e => e.EventName.Contains(""+ Eventnorv,StringComparison.CurrentCultureIgnoreCase)))
            {
                Console.WriteLine("name or venue found");
                var id = Eman.Where(e => e.venue.ToLower() == Eventnorv || e.EventName.Contains("" + Eventnorv,StringComparison.CurrentCultureIgnoreCase)) ;
                foreach (var i in id)
                {
                    Console.WriteLine($"Eventid: {i.EventId}, EventName: {i.EventName}, EventVenue:  {i.venue}, Event date and time:  {i.EventDate}, EventCapacity:  {i.Capacity}, EventFee: {i.RegFee} ");
                }
            }
            else
            {
                Console.WriteLine("name not found");
            }
        }
        //Update venue, date, capacity, and fee.
        public void Update()
        {
            //event id
            Console.Write("Enter Event ID: ");
            string? EventMId = Console.ReadLine();
            if (!int.TryParse(EventMId, out int eid))
            {
                Console.WriteLine("Invalid input. Please enter a valid number.");
                return;
            }
            if (string.IsNullOrWhiteSpace(EventMId))
                Console.WriteLine("Event id cannot be empty ");
            else if (!Eman.Any(e => e.EventId == eid))
                Console.WriteLine("event id not found");
            else
            {

                //event venue
                Console.WriteLine("Enter the venue to update ");
                string? Eventv = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(Eventv))
                    Console.WriteLine("Event venue cannot be empty ");


                //Date availability as DT
                Console.Write("Enter Event Venue (DD-MM-YYYY),(HH:MM)to update: ");
                string? edate = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(edate))
                    Console.WriteLine("Event date cannot be empty ");
                if (!DateTime.TryParse(edate, out DateTime DT))
                {
                    Console.WriteLine("Invalid input. Please enter in a valid format.");
                    return;
                }
                if (DT < DateTime.Now)
                {
                    Console.WriteLine("Date and time must be Future date.");
                    return;
                }


                //Capacity as capacity
                Console.Write("Enter capacity of the event to update: ");
                string? cap = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(cap))
                    Console.WriteLine("Event capacity cannot be empty ");
                if (!int.TryParse(cap, out int capacity))
                {
                    Console.WriteLine("Invalid input. Please enter in a valid number.");
                    return;
                }
                if (capacity! >= 0)
                {
                    Console.WriteLine("Invalid number, must be positive");
                    return;
                }


                //non-negative as efee
                Console.Write("Enter the Fee to update: ");
                string? fee = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(fee))
                    Console.WriteLine("Event fee cannot be empty ");
                if (!int.TryParse(fee, out int efee))
                {
                    Console.WriteLine("Invalid input. Please enter in a valid number.");
                    return;
                }
                if (efee! >= 0)
                {
                    Console.WriteLine("Invalid number, must be positive");
                    return;
                }

                var updatedetails = Eman.Where(e => e.EventId == eid);
                foreach (var i in updatedetails)
                {
                    i.venue = Eventv;
                    i.EventDate = DT;
                    i.Capacity = capacity;
                    i.RegFee = efee;
                }
                Console.WriteLine("Details updated");
            }
        }
    }
}
