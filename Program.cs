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
        static List<string> UserName = new List<string>();
        static List<int> Age = new List<int>();
        static List<string> AccounstNumber = new List<string>();
        static List<string> Userspassword = new List<string>();
        static List<double> Amount = new List<double>();
        static List<string> StatesOfAccount = new List<string>();
        static Queue<string> CreatAccountreadRequest = new Queue<string>();
        static Queue<string> blookAccountreadRequest = new Queue<string>();
        static Stack<string> AccountreadRequest = new Stack<string>();
        static Dictionary<double, string> HistoryTranscations = new Dictionary<double, string>();



        static void Main(string[] args)
        {

        }
        //creat the welcom function
        static string WelcomeScreen()
        {
            Console.WriteLine("Welcome to " + BankName);
            Console.WriteLine("Please select an option:");
            Console.WriteLine("1.admin");
            Console.WriteLine("2.user ");
            Console.WriteLine("3. Exit");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    Admin(); // admin function
                    break;
                case "2":
                    User(); // user function
                    break;
                case "3":
                    Console.WriteLine("Thank you for using " + BankName); // exit the program
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again."); // invalid choice
                    break;
            }
            return choice;

        }
        // creat the function for the admin
        static void Admin()
        {
            Console.WriteLine("Welcome to the Admin Panel");
            Console.WriteLine("Please enter your username:");
            string username = Console.ReadLine();
            Console.WriteLine("Please enter your password:");
            string password = Console.ReadLine();
            if (username == AdminNAme && password == AdminPassword)
            {
                Console.WriteLine("Welcome, Admin!");
                // Add admin functionality here
            }
            else
            {

                Console.WriteLine("Invalid username or password.");
            }


        }
        // creat the function for the user
        static void User()
        {
            Console.WriteLine("Welcome to the User Panel");
            Console.WriteLine("Please enter your username:");
            string username = Console.ReadLine();
            Console.WriteLine("Please enter your password:");
            string password = Console.ReadLine();
            // Check if the username and password  same at the needed locations 
            for (int i = 0; i < UserName.Count; i++)
            {
                if (UserName[i] == username && Userspassword[i] == password)
                {
                    Console.WriteLine("Welcome, " + username + "!");
                    // Add user functionality here
                    return;
                }

            }
            Console.WriteLine("Invalid username or password.");

        }
        //  request creat the function for the creat account use try parse
        static void CreateAccountrequest()
        {
            Console.WriteLine("Please enter your username:");
            string username = Console.ReadLine();
            Console.WriteLine("Please enter your age:");
            string ageInput = Console.ReadLine();
            int age;
            if (!int.TryParse(ageInput, out age))
            {
                Console.WriteLine("Invalid age. Please enter a valid number.");
                return;
            }
            Console.WriteLine("Please enter your password:");
            string password = Console.ReadLine();
            Console.WriteLine("Please enter the amount to deposit:");
            string amountInput = Console.ReadLine();
            double amount;
            if (!double.TryParse(amountInput, out amount) || amount < MinimumBalance)
            {
                Console.WriteLine("Invalid amount. Please enter a valid number greater than " + MinimumBalance);
                return;
            }
            // Add account creation logic here 

        }
        // functions get  requset of the creat account and add it to the list 

        static void CreateAccount(string username, int age, string password, double amount)
        {
            // Check if the account already exists
            for (int i = 0; i < UserName.Count; i++)
            {
                if (UserName[i] == username)
                {
                    Console.WriteLine("Account already exists for this username.");
                    return;
                }
            }
            // Add the new account to the lists
            UserName.Add(username);
            Age.Add(age);
            Userspassword.Add(password);
            Amount.Add(amount);
            AccounstNumber.Add(Guid.NewGuid().ToString());
            StatesOfAccount.Add("Active");
            Console.WriteLine("Account created successfully!");
            Console.WriteLine("Account Number: " + AccounstNumber[AccounstNumber.Count - 1]);
            Console.WriteLine("Username: " + UserName[UserName.Count - 1]);
            Console.WriteLine("Password: " + Userspassword[Userspassword.Count - 1]);
            Console.WriteLine("Amount: " + Amount[Amount.Count - 1]);
            Console.WriteLine("State of Account: " + StatesOfAccount[StatesOfAccount.Count - 1]);
        }
        //
        

    }

}  
