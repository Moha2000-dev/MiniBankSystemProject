// needed libraryes 
using System;
using System.Collections.Generic;
using System.IO;

namespace MiniBankSystemProject
{
    internal class Program
    {
        //Constanta 
        const string BankName = "Yanqule Bank System"; // name of the bank
        const double MinimumBalance = 100.00; // minimum balance for accounts
        const string AccountsFilePath = "accounts.txt"; // file path for accounts
        const string ReviewsFilePath = "reviews.txt"; // file path for reviews
        const string LoginFilePath = "login.txt"; // file path for login
        const string AdminNAme = "Admin"; // admin name 
        const string AdminPassword = "admin"; // admin password

        // global list (parallel)
        static List<string> UserName  = new List<string>();
        static List<int> Age = new List<int>();
        static List<string> AccountNumber = new List<string>();
        static List<string> password = new List<string>();
        static List<double> Amount = new List<double>();
        static List<string> StatesOfAccount = new List<string>();
        static Queue<string> CreatAccountreadRequest= new Queue<string>();
        static Queue<string> blookAccountreadRequest=new Queue<string>();
        static Stack<string> AccountreadRequest = new Stack<string>();
        static Dictionary<double,string> HistoryTranscations = new Dictionary<double,string>();

        

       static void Main(string[] args)
        {
            
        }
        //creat the welcom function
        static void welcomScreen() { 
        
        
        
        }



    }
}
