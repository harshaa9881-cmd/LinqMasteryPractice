using LearningApp1;
using System;
using System.Collections.Generic;
using System.Text;

/*namespace Library_Lending_and_Fine_Management_System
{
    internal class Dummy
    {

    }
}
*/

namespace LearningApp1
{
    public class Instagram
    {
        public string? UserName { get; private set; }//property - controlling the fields 
        public int followers
        {
            get{ 
                return followers
            } set
            {
                if (followers > 0)
                    this.Followers = Followers;
            }
        }
        public string? UserName { get; private set; }//property - controlling the fields 
        private int followers;
        public int Followers
        {
            get => followers;
            set
            {
                if (value > 0)
                    followers = value;
                Console.WriteLine("setter  followers");
                
            }
        }
        //recursive call occurs that the method is equal to a method calling itself 
        public int postCount;//fields
        public void AddStory;
    }
}
Instagram ax1 = new Instagram("ax1", 200, 3);