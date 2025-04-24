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
        const string RequsetBlockAccounts = "RequsetBlockAccounts.txt"; // file path for transactions
        const string RequestCreatAccounts = "RequestCreatAccounts.txt"; // file path for transactions

        // global list (parallel)
        static List<string> UserName = new List<string>();
        static List<int> Age = new List<int>();
        static List<string> AccounstNumber = new List<string>();
        static List<string> Userspassword = new List<string>();
        static List<string> AdminID = new List<string>();
        static List<string> UserID = new List<string>();
        static List<string> UserNationalID = new List<string>();
        static List<string> AdminPassword = new List<string>();
        static Stack<string> Review = new Stack<string>();
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
        // function to creat Admin Account
        static void CreateAdminAccount()
        {

            Console.WriteLine("Please enter your  National ID ");
            string id = Console.ReadLine();
            Console.WriteLine("Please enter your password:");
            string password = Console.ReadLine();
            // check if the ID already exists
            string adminID = "admin" + id;
            if (AdminID.Contains(adminID))
            {
                Console.WriteLine("ID already exists. Please try again.");
                return;
            }
            // concatonate the wordadmin and id in one string and add it to the list of admin id 

            AdminID.Add(adminID);
            AdminPassword.Add(password);
            Console.WriteLine("Admin account created successfully.");
        }
        // mune function for admin ask for the AdminID and password if not creat or try againe the account if yes open the admin menu
        static void Admin()
        {
            Console.WriteLine("Please enter your AdminID:");
            string id = Console.ReadLine();
            Console.WriteLine("Please enter your password:");
            string password = Console.ReadLine();
            // check if the ID and password are correct
            for (int i = 0; i < AdminID.Count; i++)
            {
                if (AdminID[i] == id && AdminPassword[i] == password)
                {
                    Console.WriteLine("Welcome, " + id);
                    AdminMenu();
                    return;
                }
                else
                {
                    Console.WriteLine("Invalid ID or password. Do you want to creat an admin account ");
                    Console.WriteLine("1. Yes");
                    Console.WriteLine("2. No");
                    string choice = Console.ReadLine();
                    switch (choice)
                    {
                        case "1":
                            CreateAdminAccount();
                            break;
                        case "2":
                            Console.WriteLine("Thank you for using " + BankName);
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
            }















        }// Admin menu function
        static void AdminMenu()
        {
            Console.WriteLine("Welcome to the admin menu.");
            Console.WriteLine("Please select an option:");
            Console.WriteLine("1. Create account");
            Console.WriteLine("2. Block account");
            Console.WriteLine("3. View reviews");
            Console.WriteLine("4. View transactions history");
            Console.WriteLine("5. Exit");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    CreateAccount();
                    break;
                case "2":
                    BlockAccount();
                    break;
                case "3":
                    ViewReviews();
                    break;
                case "4":
                    ViewDeletRequset();
                    break;
                case "5":
                    ViewBlockRequset();
                    break;






                case "":
                    WelcomeScreen();
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }

        }
        // function to creat user menu
        static void User()
        {
            Console.WriteLine("Please enter your userID:");
            string id = Console.ReadLine();
            Console.WriteLine("Please enter your password:");
            string password = Console.ReadLine();
            // check if the ID and password are correct
            for (int i = 0; i < UserID.Count; i++)
            {
                if (UserID[i] == id && Userspassword[i] == password)
                {
                    Console.WriteLine("Welcome, " + id);
                    UserMenu();
                    return;
                }
                else
                {
                    Console.WriteLine("Invalid ID or password. Do you want to creat an account ");
                    Console.WriteLine("1. Yes");
                    Console.WriteLine("2. No");
                    string choice = Console.ReadLine();
                    switch (choice)
                    {
                        case "1":
                            CreateACountRequest();
                            break;
                        case "2":
                            Console.WriteLine("Thank you for using " + BankName);
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
            }
        }
        // function to creat user menu
        static void UserMenu()
        {
            Console.WriteLine("Welcome to the user menu.");
            Console.WriteLine("Please select an option:");
            Console.WriteLine("1. View account balance");
            Console.WriteLine("2. Deposit money");
            Console.WriteLine("3. Withdraw money");
            Console.WriteLine("4. View transactions history");
            Console.WriteLine("5. Exit");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                   CheakActiveAccounet();
                case "2":
                    ViewAccountBalance();
                    break;
                case "3":
                    DepositMoney();
                    break;
                case "4":
                    WithdrawMoney();
                    break;
                case "5":
                    ViewTransactionsHistory();
                    break;
                case "6":
                    RequsetBlockRequset();
                    break;
                case "7":
                    submitReview();
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }


        // function to request to creat an account
        static void CreateACountRequest()
        {
            Console.WriteLine("Please enter your name:");
            string name = Console.ReadLine();
            Console.WriteLine("Please enter your age:");
            int age = int.Parse(Console.ReadLine());
            Console.WriteLine("Please enter your password:");
            string password = Console.ReadLine();
            Console.WriteLine("Please enter your National ID:");
            string nationalID = Console.ReadLine();
            Console.WriteLine("Please enter your mony amount(must be ,ore than 100):");
            double amount = double.Parse(Console.ReadLine());
            // check if the ID already exists
            string accountNumber = "account" + AccounstNumber.Count;
            if (AccounstNumber.Contains(accountNumber))
            {
                Console.WriteLine("ID already exists. Please try again.");
                return;
            }
            // check if the age is valid
            if (age < 18)
            {
                Console.WriteLine("You must be at least 18 years old to create an account.");
                return;
            }

            // concatonate the wordadmin and id in one string and add it to the list of admin id 
            AccounstNumber.Add(accountNumber);
            UserName.Add(name);
            Age.Add(age);
            Userspassword.Add(password);
            CreatAccountreadRequest.Enqueue(accountNumber);
            StatesOfAccount.Add("Inproces");
            Amount.Add(amount);
            UserID.Add(name + accountNumber);

            Console.WriteLine("Account request created successfully.");

        }

        //request to block an account
        public static void CreateBlockAccountRequest()
        {
            Console.WriteLine("Please enter your account number:");
            string accountNumber = Console.ReadLine();
            // check if the ID already exists
            if (!AccounstNumber.Contains(accountNumber))
            {
                Console.WriteLine("Invalid Account Number");
                return;
            }
            //  add to blook account request 
            blookAccountreadRequest.Enqueue(accountNumber);
            Console.WriteLine("Account request created successfully.");
        }
        //View the account balance
        static void ViewAccountBalance()
        {
            Console.WriteLine("Please enter your account number:");
            string accountNumber = Console.ReadLine();
            // check if the ID already exists
            if (!AccounstNumber.Contains(accountNumber))
            {
                Console.WriteLine("Invalid Account Number");
                return;
            }
            // get the index of the account number
            int index = AccounstNumber.IndexOf(accountNumber);
            // get the balance of the account
            double balance = Amount[index];
            Console.WriteLine("Your account balance is: " + balance);
        }
        // function to deposit money
        public static void DepositMoney(string UserID)
        {
            Console.WriteLine("Please enter the Amount of mony you want to Deposit");
            double amount = double.Parse(Console.ReadLine());
            // get the index of the account number
            int index = AccounstNumber.IndexOf(UserID);
            // check if the amount is valid
            if (amount <= 0)
            {
                Console.WriteLine("Invalid amount. Please try again.");
                return;
            }
            // check if the account is blocked
            if (StatesOfAccount[index] == "Blocked")
            {
                Console.WriteLine("Your account is blocked. Please contact the admin.");
                return;
            }


            // get the balance of the account
            double balance = Amount[index];
            //add the amount to the balance
            balance += amount;
        }
        // function to creat user account
        public static void createAccount(string UserID)
        {
            int IndexOfUser = UserID.IndexOf(UserID);
            if (IndexOfUser == -1)
            {
                Console.WriteLine("Invalid UserID");

            }
            else
            {
                // check if the account is blocked
                if (StatesOfAccount[IndexOfUser] == "Blocked")
                {
                    Console.WriteLine("Your account is blocked");
                    return;
                }
                // get the balance of the account
                double balance = Amount[IndexOfUser];
                if (StatesOfAccount[IndexOfUser] == "Inproces")
                {
                    if (balance < MinimumBalance)
                    {
                        Console.WriteLine("Your account balance is less than the minimum balance. Please deposit more money.");
                        return;
                    }
                    else
                    {
                        StatesOfAccount[IndexOfUser] = "Active";
                        Console.WriteLine("Your account is active.");
                    }

                }

            }


          




        }
        // function to withdraw money 
        static public void withdraw() {
            
            Console.WriteLine("Please enter the amount of money you want to withdraw:");
            double amount = double.Parse(Console.ReadLine());
            // get the index of the account number
            int index = AccounstNumber.IndexOf(UserID);
            // check if the amount is valid
            if (amount <= 0)
            {
                Console.WriteLine("Invalid amount. Please try again.");
                return;
            }
            double balance = Amount[index];
            // check if the account is blocked
            if (StatesOfAccount[index] == "Blocked")
            {
                Console.WriteLine("Your account is blocked. Please contact the admin.");
                return;
            }
            // check if the amount is less than the balance
            if (amount > balance)
            {
                Console.WriteLine("Invalid amount. Please try again.");
                return;
            }
            // subtract the amount from the balance
            balance -= amount;
            // add the amount to the history of transactions
            HistoryTranscations.Add(amount, "Withdraw");
            // update the balance of the account
            Amount[index] = balance;
            Console.WriteLine("Your account balance is: " + balance);

        }
        // cheak active accounst 
        
        





    }
    
    }

 
