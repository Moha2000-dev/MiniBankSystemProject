﻿// needed libraryes 
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
        const string RequestDeletAccountsFile = "RequestDeletAccounts.txt"; // file path for transactions

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
        static Stack<string> AccountDeletRequest = new Stack<string>();
        static Dictionary<double, string> HistoryTranscations = new Dictionary<double, string>();



        static void Main(string[] args)
        {
            // load the accounts from the file
            LoadAccountsToFile();
            // load the login information from the file
            LoadLoginToFile();
            // load the requests to creat accounts from the file
            LoadRequestCreatAccountsToFile();
            // load the requests to delet accounts from the file
            LoadRequestDeletAccountsToFile();
            // load the requests to block accounts from the file
            LoadRequestBlockAccountsToFile();
            // load the reviews from the file
            LoadReviewsToFile();
            // show the welcome screen
            WelcomeScreen();
            // save the accounts to the file
            SaveAccountsToFile();
            // save the login information to the file
            SaveLoginToFile();
            // save the requests to creat accounts to the file
            SaveRequestCreatAccountsToFile();
            // save the requests to delet accounts to the file
            SaveRequestDeletAccountsToFile();
            // save the requests to block accounts to the file
            SaveRequestBlockAccountsToFile();
            // save the reviews to the file
            SaveReviewsToFile();
            // exit the program
        }
        //creat the welcom function
        static string WelcomeScreen()
        {
            try
            {
                Console.Clear(); // Clear the console for a fresh start
                // Display welcome message and menu options
                Console.WriteLine("Welcome to " + BankName);
                Console.WriteLine("Please select an option:");
                Console.WriteLine("1. Admin");
                Console.WriteLine("2. User");
                Console.WriteLine("3. Exit");

                // Read the user's choice
                string choice = Console.ReadLine();

                // Process the user's choice
                switch (choice)
                {
                    case "1":
                        Admin(); // Call the Admin function
                        break;
                    case "2":
                        User(); // Call the User function
                        break;
                    case "3":
                        Console.WriteLine("Thank you for using " + BankName); // Exit message
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again."); // Handle invalid input
                        break;
                }

                // Return the user's choice
                return choice;
            }
            catch (Exception ex)
            {
                // Handle any unexpected errors
                Console.WriteLine($"An error occurred: {ex.Message}");
                return "error"; // Optional: return an error indicator
            }


        }



        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++admin functions +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // function to creat Admin Account
        static void CreateAdminAccount()
        {

            try
            {
                // Ask the user to enter National ID
                Console.WriteLine("Please enter your National ID:");
                string id = Console.ReadLine();

                // Ask the user to enter a password
                Console.WriteLine("Please enter your password:");
                string password = Console.ReadLine();

                // Combine "admin" + ID to create a unique admin ID
                string adminID = "admin" + id;

                // Check if this admin ID already exists
                if (AdminID.Contains(adminID))
                {
                    Console.WriteLine("ID already exists. Please try again.");

                    // Return to the welcome screen if ID is already taken
                    WelcomeScreen();
                    return;
                }

                // Add the new admin ID and password to the lists
                AdminID.Add(adminID);
                AdminPassword.Add(password);

                Console.WriteLine("Admin account created successfully.");

                // Save the new login information to the file
                SaveLoginToFile();

                // Return to the Admin Menu
                AdminMenu();
            }
            catch (Exception ex)
            {
                // Handle any unexpected errors
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

        }
        // mune function for admin ask for the AdminID and password if not creat or try againe the account if yes open the admin menu
        static void Admin()
        {
            Console.Clear(); // Clear the console for a fresh start
            try
            {
                // Ask the user to enter Admin ID
                Console.WriteLine("Please enter your AdminID:");
                string id = Console.ReadLine();

                // Ask the user to enter password
                Console.WriteLine("Please enter your password:");
                string password = Console.ReadLine();

                // Check if the ID and password match any existing admin account
                bool isValidAdmin = false; // flag to track login success
                for (int i = 0; i < AdminID.Count; i++)
                {
                    if (AdminID[i] == id && AdminPassword[i] == password)
                    {
                        Console.WriteLine("Welcome, " + id);
                        AdminMenu(); // go to admin menu
                        isValidAdmin = true;
                        return; // exit after successful login
                    }
                }

                // If not valid, ask the user if they want to create a new admin account
                if (!isValidAdmin)
                {
                    Console.WriteLine("Invalid ID or password. Do you want to create an admin account?");
                    Console.WriteLine("1. Yes");
                    Console.WriteLine("2. No");

                    // Read the user's choice
                    string inputChoice = Console.ReadLine();
                    int choice;

                    // Try to parse the input into an integer
                    if (int.TryParse(inputChoice, out choice))
                    {
                        switch (choice)
                        {
                            case 1:
                                CreateAdminAccount(); // create a new admin account
                                break;
                            case 2:
                                Console.WriteLine("Thank you for using " + BankName); // exit message
                                WelcomeScreen();
                                break;
                            default:
                                Console.WriteLine("Invalid choice. Please select 1 or 2."); // invalid option
                              
                                break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a number (1 or 2)."); // input is not a number
                    }
                    // invalid choise pless  press againe 
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey(); // wait for user input
                     
                }
            }
            catch (Exception ex)
            {
                // Catch and display any unexpected errors
                Console.WriteLine($"An error occurred: {ex.Message}");
            }


        }// Admin menu function
        static void AdminMenu()
        {
            bool flag = true;
            while (flag)
            {
                Console.Clear(); // Clear the console for a fresh start
                try
                {
                    // Display the Admin Menu options
                    Console.WriteLine("Welcome to the admin menu.");
                    Console.WriteLine("Please select an option:");
                    Console.WriteLine("1. View account requests");
                    Console.WriteLine("2. Block account");
                    Console.WriteLine("3. View reviews");
                    Console.WriteLine("4. View delete requests");
                    Console.WriteLine("5. View block requests");
                    Console.WriteLine("6. View all accounts");
                    Console.WriteLine("7. Proceed to Create account");
                    Console.WriteLine("8. Proceed to Delete account");
                    Console.WriteLine("9. Exit");

                    // Read the user input
                    string input = Console.ReadLine();
                    int choice;

                    // Try to parse the input into an integer
                    if (int.TryParse(input, out choice))
                    {
                        // Process the user's choice
                        switch (choice)
                        {
                            case 1:
                                ViewAcounetRequest(); // Function to view account requests
                                break;
                            case 2:
                                BlockAccount(); // Function to block an account
                                break;
                            case 3:
                                ViewReviews(); // Function to view reviews
                                break;
                            case 4:
                                ViewDeletRequsets(); // Function to view delete requests
                                break;
                            case 5:
                                ViewBlockRequset(); // Function to view block requests
                                break;
                            case 6:
                                ViewallAccount(); // Function to view all accounts
                                break;
                            case 7:
                                CreateAccount(); // Function to create an account
                                break;
                            case 8:
                                DeletAccount(); // Function to delete an account
                                break;
                            case 9:
                                flag = false; // Return to welcome screen
                                break;
                            default:
                                // Handle invalid option (not between 1-9)
                                Console.WriteLine("Invalid choice. Please select a number between 1 and 9.");
                                break;
                        }
                    }
                    else
                    {
                        // Handle if the user input is not a number
                        Console.WriteLine("Invalid input. Please enter a number (1-9).");
                    }
                    Console.WriteLine("press any key to continue");
                    Console.ReadLine();
                }
                catch (Exception ex)
                {
                    // Catch and display any unexpected errors
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }

        }

        // function to view account requests
        public static void ViewAcounetRequest()
        {
            try
            {
                // Variable to control the loop (keep asking until user wants to exit)
                bool exit = false;

                while (!exit) // keep running until the user types "yes" to exit
                {

                    // If valid, display all account creation requests
                    Console.WriteLine("Requests for account creation:");
                    foreach (string transaction in CreatAccountreadRequest)
                    {
                        Console.WriteLine("Request: " + transaction);
                    }

                    // Ask if the user wants to exit
                    Console.WriteLine("Do you want to exit? (yes/no)");
                    string choice = Console.ReadLine();

                    if (choice.ToLower() == "yes")
                    {
                        exit = true; // Exit the while loop
                    }
                }

            }
            catch (Exception ex)
            {
                // Handle unexpected errors gracefully
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }


        // function to block an account
        public static void BlockAccount()
        {
            try
            {
                Console.WriteLine("Please enter your UserID:");
                string userID = Console.ReadLine();

                // Check if the UserID exists in the account list
                int index = AccounstNumber.IndexOf(userID);

                if (index == -1)
                {
                    Console.WriteLine("Invalid Account Number. No such user found.");
                    return; // Exit if ID not found
                }

                // Check if the account is already blocked
                if (StatesOfAccount[index] == "Blocked")
                {
                    Console.WriteLine("This account is already blocked. Please contact the admin if needed.");
                    return; // Exit if already blocked
                }

                // Check if the account is in process
                if (StatesOfAccount[index] == "Inproces")
                {
                    Console.WriteLine("This account is currently in process.");
                }
                else
                {
                    Console.WriteLine("This account is not in process.");
                }

                // Change the account state to "Blocked"
                StatesOfAccount[index] = "Blocked";

                // Add the blocked account number to the blocked requests queue
                blookAccountreadRequest.Enqueue(AccounstNumber[index]);

                Console.WriteLine("The account has been blocked successfully.");

                
               
            }
            catch (Exception ex)
            {
                // Handle any unexpected errors
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }


        // function to view reviews
        public static void ViewReviews()
        {
            try
            {
                // Check if there are any reviews to show
                if (Review.Count == 0)
                {
                    Console.WriteLine("There are no reviews available yet.");
                }
                else
                {
                    // Loop through each review and display it
                    Console.WriteLine("List of Reviews:");
                    foreach (string transaction in Review)
                    {
                        Console.WriteLine("Review: " + transaction);
                    }
                }

                // After showing the reviews, return to the AdminMenu;
              
            }
            catch (Exception ex)
            {
                // Handle any unexpected errors
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
        // View account delete requests
        public static void ViewDeletRequsets()
        {
            try
            {
                // Check if there are any delete requests to show
                if (AccountDeletRequest.Count == 0)
                {
                    Console.WriteLine("There are no delete requests available.");
                }
                else
                {
                    // Display all delete requests
                    Console.WriteLine("List of Account Delete Requests:");
                    foreach (string transaction in AccountDeletRequest)
                    {
                        Console.WriteLine("Request: " + transaction);
                    }
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        // View blocked account requests
        public static void ViewBlockRequset()
        {
            try
            {
                // Check if there are any blocked account requests to show
                if (blookAccountreadRequest.Count == 0)
                {
                    Console.WriteLine("There are no block requests available.");
                }
                else
                {
                    // Display all block requests
                    Console.WriteLine("List of Blocked Account Requests:");
                    foreach (string transaction in blookAccountreadRequest)
                    {
                        Console.WriteLine("Blocked Account: " + transaction);
                    }
                }

                // Return to the AdminMenu screen after displaying
             
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        // View all existing accounts
        public static void ViewallAccount()
        {
            try
            {
                // Check if there are any accounts to display
                if (AccounstNumber.Count == 0)
                {
                    Console.WriteLine("There are no accounts available.");
                }
                else
                {
                    // Loop through all accounts and display their details
                    Console.WriteLine("List of All Accounts:");
                    for (int i = 0; i < AccounstNumber.Count; i++)
                    {
                        Console.WriteLine($"Account {i + 1}:");
                        Console.WriteLine("Account Number: " + AccounstNumber[i]);
                        Console.WriteLine("User Name: " + UserName[i]);
                        Console.WriteLine("Age: " + Age[i]);
                        Console.WriteLine("Password: " + Userspassword[i]);
                        Console.WriteLine("National ID: " + UserNationalID[i]);
                        Console.WriteLine("Amount: " + Amount[i]);
                        Console.WriteLine("State of Account: " + StatesOfAccount[i]);
                        Console.WriteLine("---------------------------------------");
                    }
                }

                // Return to the AdminMenu screen
            
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        // Delete an account safely
        public static void DeletAccount()
        {
            try
            {
                Console.WriteLine("Please enter your UserID:");
                string userID = Console.ReadLine();

                // Find the index of the entered UserID in AccounstNumber list
                int index = AccounstNumber.IndexOf(userID);

                // Check if the account exists
                if (index == -1)
                {
                    Console.WriteLine("Invalid Account Number. No such user found.");
                    return;
                }

                // Check if the account is blocked
                if (StatesOfAccount[index] == "Blocked")
                {
                    Console.WriteLine("Your account is blocked. Please contact the admin.");
                    return;
                }

                // Confirm before deleting the account
                Console.WriteLine("Are you sure you want to delete this account? (yes/no)");
                string confirmation = Console.ReadLine().ToLower();
                if (confirmation != "yes")
                {
                    Console.WriteLine("Account deletion cancelled.");
                    WelcomeScreen();
                    return;
                }

                // Remove the account data from all lists
                AccounstNumber.RemoveAt(index);
                UserName.RemoveAt(index);
                Age.RemoveAt(index);
                Userspassword.RemoveAt(index);
                UserNationalID.RemoveAt(index);
                Amount.RemoveAt(index);
                StatesOfAccount.RemoveAt(index);
                // remove from the rquest to delet account
                AccountDeletRequest.Pop();

                Console.WriteLine("Account deleted successfully.");

                // Return to AdminMenu screen
              
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public static void CreateAccount()
        {
            try
            {
                // Ask for the user's ID
                Console.WriteLine("Please enter UserID:");
                string userID = Console.ReadLine();

                // Find the index of the entered UserID in the account list
                int IndexOfUser = userID.IndexOf(userID);

                // Check if the UserID exists
                if (IndexOfUser == -1)
                {
                    Console.WriteLine("Invalid UserID. Please check and try again.");
                }
                else
                {
                    // Check if the account is blocked
                    if (StatesOfAccount[IndexOfUser] == "Blocked")
                    {
                        Console.WriteLine("This account is blocked. Cannot proceed.");
                        WelcomeScreen(); // Return after blocked
                        return;
                    }

                    // Get the balance of the account
                    double balance = Amount[IndexOfUser];

                    // Check if account is still "In process"
                    if (StatesOfAccount[IndexOfUser] == "Inproces")
                    {
                        // Check if the balance meets the minimum balance requirement
                        if (balance < MinimumBalance)
                        {
                            Console.WriteLine($"Account balance ({balance}) is less than the minimum required ({MinimumBalance}).");
                            Console.WriteLine("Please ask the user to deposit more money.");
                        }
                        else
                        {
                            // If balance is sufficient, activate the account
                            StatesOfAccount[IndexOfUser] = "Active";
                            Console.WriteLine("The account has been successfully activated.");
                            //remove from the request to creat account
                            CreatAccountreadRequest.Dequeue();
                            // Save the updated account information to the file

                        }
                    }
                    else
                    {
                        Console.WriteLine("Account is already active or in another state.");
                    }
                }

                
              
            }
            catch (Exception ex)
            {
                // Handle any unexpected errors
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }




        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++user functions +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++





        // function to creat user 
        static void User()
        {
            Console.Clear(); // Clear the console for a fresh start
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
                
            }
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
        // function to creat user menu
        static void UserMenu()
        {
            Console.Clear(); // Clear the console for a fresh start
            Console.WriteLine("Welcome to the user menu.");//Welcome message
            Console.WriteLine("Please select an option:");//Select an option
            Console.WriteLine("1. cheak your accountd stat:");  // Display the menu options
            Console.WriteLine("2. View account balance"); //View account balance
            Console.WriteLine("3. Deposit money");//Deposit money
            Console.WriteLine("4. Withdraw money");//Withdraw money
            Console.WriteLine("5. Rview Transactions History");//RView Transactions History
            Console.WriteLine("6. Request to block an account");// Request to block an account
            Console.WriteLine("7. submit Review"); // Submit a review
            Console.WriteLine("8. Request to delete an account");// Request to delete an account
            Console.WriteLine("9. Transfer money"); // Transfer money
            Console.WriteLine("10. Exit"); // Exit the program
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    CheakActiveAccounet();// Check account status
                    break;
                case "2":
                    ViewAccountBalance();// View account balance
                    break;
                case "3":
                    DepositMoney();// Deposit money
                    break;
                case "4":
                    Withdraw(); // Withdraw money
                    break;
                case "5":
                    ViewTransactionsHistory(); // View transactions history
                    break;
                case "6":
                    CreateBlockAccountRequest(); // Request to block an account
                    break;
                case "7":
                    SubmitReview(); // Submit a review
                    break;
                case "8":
                    RequestDeleteAccount(); // Request to delete an account
                    break;
                case "9":
                    TransferMoneyOption(); // Transfer money
                    break;
                case "10":
                    Console.WriteLine("Thanks for  using " + BankName); // Exit message
                    WelcomeScreen();
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again."); 
                    break;
            }
            Console.WriteLine("press any key ");
            Console.ReadKey();
            UserMenu();
        }


        // Function to create a new account request
        public static void CreateACountRequest()
        {
            try
            {
                Console.WriteLine("Please enter your name:");
                string name = Console.ReadLine();

                Console.WriteLine("Please enter your age:");
                if (!int.TryParse(Console.ReadLine(), out int age) || age < 18)
                {
                    Console.WriteLine("Invalid age or under 18. Cannot create account.");
                    return;
                }

                Console.WriteLine("Please enter your password:");
                string password = Console.ReadLine();

                Console.WriteLine("Please enter your National ID:");
                string nationalID = Console.ReadLine();

                Console.WriteLine("Please enter your money amount (must be more than 100):");
                if (!double.TryParse(Console.ReadLine(), out double amount) || amount < 100)
                {
                    Console.WriteLine("Invalid amount. Must be more than 100.");
                    return;
                }

                // Generate new account number
                string accountNumber = "account" + AccounstNumber.Count;

                // Check if account number already exists (very rare)
                if (AccounstNumber.Contains(accountNumber))
                {
                    Console.WriteLine("Account number already exists. Try again.");
                    return;
                }

                // Add details to lists
                AccounstNumber.Add(accountNumber);
                UserName.Add(name);
                Age.Add(age);
                Userspassword.Add(password);
                UserNationalID.Add(nationalID);
                Amount.Add(amount);
                StatesOfAccount.Add("Inproces");
                UserID.Add(name + accountNumber);
                CreatAccountreadRequest.Enqueue(name + accountNumber);
                SaveAccountsToFile();
                Console.WriteLine("Account request created successfully.");
               
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
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
        public static void ViewAccountBalance()
        {
            try
            {
                Console.WriteLine("Please enter your UserID:");
                string userID = Console.ReadLine();

                int index = userID.IndexOf(userID);
                if (index == -1)
                {
                    Console.WriteLine("Invalid Account Number.");
                    return;
                }

                Console.WriteLine($"Your account balance is: {Amount[index]}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
       
        }
        // function to deposit money
        public static void DepositMoney()
        {
            try
            {
                Console.WriteLine("Please enter your UserID:");
                string userID = Console.ReadLine();

                int index =userID.IndexOf(userID);
                if (index == -1)
                {
                    Console.WriteLine("Invalid Account Number.");
                    return;
                }

                if (StatesOfAccount[index] == "Blocked")
                {
                    Console.WriteLine("Account is blocked. Cannot deposit.");
                    return;
                }

                Console.WriteLine("Please enter the amount to deposit:");
                if (!double.TryParse(Console.ReadLine(), out double amount) || amount <= 0)
                {
                    Console.WriteLine("Invalid amount.");
                    return;
                }

                Amount[index] += amount;
                Console.WriteLine($"Deposit successful. New balance: {Amount[index]}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            
        }

        // function to withdraw money 
        public static void Withdraw()
        {
            try
            {
                Console.WriteLine("Please enter your UserID:");
                string userID = Console.ReadLine();

                int index = AccounstNumber.IndexOf(userID);
                if (index == -1)
                {
                    Console.WriteLine("Invalid Account Number.");
                    return;
                }

                if (StatesOfAccount[index] == "Blocked")
                {
                    Console.WriteLine("Account is blocked. Cannot withdraw.");
                    return;
                }

                Console.WriteLine("Please enter the amount to withdraw:");
                if (!double.TryParse(Console.ReadLine(), out double amount) || amount <= 0)
                {
                    Console.WriteLine("Invalid amount.");
                    return;
                }

                if (amount > Amount[index])
                {
                    Console.WriteLine("Insufficient balance.");
                    return;
                }

                Amount[index] -= amount;
                HistoryTranscations.Add(amount, "Withdraw");
                Console.WriteLine($"Withdrawal successful. New balance: {Amount[index]}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
           
        }
        // PROCESS TO Cheak Active Accounet with try and catch ;
        public static void CheakActiveAccounet()
        {
            try
            {
                Console.WriteLine("Please enter your UserID");
                string UserID = Console.ReadLine();
                // check if the ID already exists
                if (!UserID.Contains(UserID))
                {
                    Console.WriteLine("Invalid Account Number");
                    return;
                }
                // get the index of the account number
                int index = UserID.IndexOf(UserID);
                // check if the account is blocked
                if (StatesOfAccount[index] == "Blocked")
                {
                    Console.WriteLine("Your account is blocked. Please contact the admin.");
                    return;
                }
                // check if the account is active
                if (StatesOfAccount[index] == "Active")
                {
                    Console.WriteLine("Your account is active.");
                }
                else
                {
                    Console.WriteLine("Your account is not active.");
                }
                // check if the account is in proces
                if (StatesOfAccount[index] == "Inproces")
                {
                    Console.WriteLine("Your account is in proces.");
                }
                else
                {
                    Console.WriteLine("Your account is not in proces.");
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid UserID. Please try again.");
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
                return;
            }

           





        }
        // function to view the transactions history
        public static void ViewTransactionsHistory()
        {
            try
            {
                Console.WriteLine("Please enter your UserID:");
                string userID = Console.ReadLine();

                int index = userID.IndexOf(userID);
                if (index == -1)
                {
                    Console.WriteLine("Invalid Account Number.");
                    return;
                }

                if (StatesOfAccount[index] == "Blocked")
                {
                    Console.WriteLine("Your account is blocked. Please contact the admin.");
                    return;
                }

                if (HistoryTranscations.Count == 0)
                {
                    Console.WriteLine("No transactions found.");
                    return;
                }

                Console.WriteLine("Transactions:");
                foreach (KeyValuePair<double, string> transaction in HistoryTranscations)
                {
                    Console.WriteLine($"Amount: {transaction.Key}, Type: {transaction.Value}");
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                
            }
         
        }

        //submit a review
        // Submit a review
        public static void SubmitReview()
        {
            try
            {
                Console.WriteLine("Please enter your review:");
                string review = Console.ReadLine();
                Review.Push(review);
                Console.WriteLine("Your review has been submitted successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
           
        }

        // function to request to delete an account
        public static void RequestDeleteAccount()
        {
            try
            {
                Console.WriteLine("Please enter your UserID:");
                string userID = Console.ReadLine();

                int index = userID.IndexOf(userID);
                if (index == -1)
                {
                    Console.WriteLine("Invalid Account Number.");
                    return;
                }

                if (StatesOfAccount[index] == "Blocked")
                {
                    Console.WriteLine("Your account is blocked. Cannot request deletion.");
                    return;
                }

                AccountDeletRequest.Push(AccounstNumber[index]);
                Console.WriteLine("Delete request submitted successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
         
        }



        // transactions functions
        public static void TransferMoney(string senderID, string receiverID, double amount)
        {
            try
            {
                // Validate amount
                if (amount <= 0)
                {
                    Console.WriteLine("Invalid transfer amount. Must be greater than 0.");
                    return;
                }

                // Find sender and receiver index
                int senderIndex = AccounstNumber.IndexOf(senderID);
                int receiverIndex = AccounstNumber.IndexOf(receiverID);

                // Validate sender and receiver accounts
                if (senderIndex == -1)
                {
                    Console.WriteLine("Sender account not found.");
                    return;
                }
                if (receiverIndex == -1)
                {
                    Console.WriteLine("Receiver account not found.");
                    return;
                }

                // Check if sender has enough balance
                if (Amount[senderIndex] < amount)
                {
                    Console.WriteLine("Sender does not have enough balance to transfer.");
                    return;
                }

                // Check if sender account is blocked
                if (StatesOfAccount[senderIndex] == "Blocked")
                {
                    Console.WriteLine("Sender account is blocked. Cannot proceed.");
                    return;
                }

                // Check if receiver account is blocked
                if (StatesOfAccount[receiverIndex] == "Blocked")
                {
                    Console.WriteLine("Receiver account is blocked. Cannot receive money.");
                    return;
                }

                // Perform the balance transfer
                Amount[senderIndex] -= amount;
                Amount[receiverIndex] += amount;

                // Record the transaction
                HistoryTranscations.Add(amount, $"Transfer from {senderID} to {receiverID}");

                Console.WriteLine($"Transfer successful! {amount} transferred from {senderID} to {receiverID}.");
            }
            catch (ArgumentException)
            {
                Console.WriteLine("A transaction with this amount already exists in the history. Try again with a different amount.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during transfer: {ex.Message}");
            }
        }
        public static void TransferMoneyOption()
        {
            try
            {
                Console.WriteLine("Enter your (Sender) UserID:");
                string senderID = Console.ReadLine();

                Console.WriteLine("Enter Receiver's UserID:");
                string receiverID = Console.ReadLine();

                Console.WriteLine("Enter the amount to transfer:");
                if (!double.TryParse(Console.ReadLine(), out double amount))
                {
                    Console.WriteLine("Invalid amount entered. Please try again.");
                    return;
                }

                // Now call the TransferMoney method you built earlier
                TransferMoney(senderID, receiverID, amount);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while transferring: {ex.Message}");
            }
        }


        // ============================================================================Files functions==========================================================================
        // function to save the accounts to a file with the name accounts.txt and try and catch
        public static void SaveAccountsToFile()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(AccountsFilePath))
                {
                    for (int i = 0; i < AccounstNumber.Count; i++)
                    {
                        writer.WriteLine(AccounstNumber[i] + "," + UserName[i] + "," + Age[i] + "," + Userspassword[i] + "," + UserNationalID[i] + "," + Amount[i] + "," + StatesOfAccount[i]);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while saving the accounts to the file: " + ex.Message);
            }
        }
        // function to load the accounts from a file with the name accounts.txt and try and catch
        public static void LoadAccountsToFile()
        {
            try
            {
                using (StreamReader reader = new StreamReader(AccountsFilePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts = line.Split(',');
                        AccounstNumber.Add(parts[0]);
                        UserName.Add(parts[1]);
                        Age.Add(int.Parse(parts[2]));
                        Userspassword.Add(parts[3]);
                        UserNationalID.Add(parts[4]);
                        Amount.Add(double.Parse(parts[5]));
                        StatesOfAccount.Add(parts[6]);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while loading the accounts from the file: " + ex.Message);
            }


        }
        // function to save the reviews to a file with the name reviews.txt and try and catch
        public static void SaveReviewsToFile()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(ReviewsFilePath))
                {
                    foreach (string review in Review)
                    {
                        writer.WriteLine(review);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while saving the reviews to the file: " + ex.Message);
            }
        }
        // function to load the reviews from a file with the name reviews.txt and try and catch
        public static void LoadReviewsToFile()
        {
            try
            {
                using (StreamReader reader = new StreamReader(ReviewsFilePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        Review.Push(line);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while loading the reviews from the file: " + ex.Message);
            }
        }
        // function to save the login information to a file with the name login.txt  for the user and admin
        public static void SaveLoginToFile()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(LoginFilePath))
                {
                    for (int i = 0; i < AdminID.Count; i++)
                    {
                        writer.WriteLine(AdminID[i] + "," + AdminPassword[i]);
                    }
                    for (int i = 0; i < UserID.Count; i++)
                    {
                        writer.WriteLine(UserID[i] + "," + Userspassword[i]);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while saving the login information to the file: " + ex.Message);
            }
        }


        // function to load the login information from a file with the name login.txt  for the user and admin
        public static void LoadLoginToFile()
        {
            try
            {
                using (StreamReader reader = new StreamReader(LoginFilePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts = line.Split(',');
                        if (parts[0].StartsWith("admin"))
                        {
                            AdminID.Add(parts[0]);
                            AdminPassword.Add(parts[1]);
                        }
                        else
                        {
                            UserID.Add(parts[0]);
                            Userspassword.Add(parts[1]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while loading the login information from the file: " + ex.Message);
            }
        }
        // function to save the requests to a file with the name requsetblockaccounts.txt 
        public static void SaveRequestBlockAccountsToFile()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(RequsetBlockAccounts))
                {
                    foreach (string request in blookAccountreadRequest)
                    {
                        writer.WriteLine(request);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while saving the requests to the file: " + ex.Message);
            }
        }
        // function to load the requests from a file with the name requsetblockaccounts.txt 
        public static void LoadRequestBlockAccountsToFile()
        {
            try
            {
                using (StreamReader reader = new StreamReader(RequsetBlockAccounts))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        blookAccountreadRequest.Enqueue(line);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while loading the requests from the file: " + ex.Message);
            }
        }
        // function to save the request to creat accounts to a file with the name RequestCreatAccounts.txt 
        public static void SaveRequestCreatAccountsToFile()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(RequestCreatAccounts))
                {
                    foreach (string request in CreatAccountreadRequest)
                    {
                        writer.WriteLine(request);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while saving the requests to the file: " + ex.Message);
            }
        }
        // function to load the request to creat accounts from a file with the name RequestCreatAccounts.txt
        public static void LoadRequestCreatAccountsToFile()
        {
            try
            {
                using (StreamReader reader = new StreamReader(RequestCreatAccounts))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        CreatAccountreadRequest.Enqueue(line);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while loading the requests from the file: " + ex.Message);
            }
        }
        // function to save the request to delet accounts to a file with the name RequestDeletAccounts.txt
        public static void SaveRequestDeletAccountsToFile()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(RequestDeletAccountsFile))
                {
                    foreach (string request in AccountDeletRequest)
                    {
                        writer.WriteLine(request);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while saving the requests to the file: " + ex.Message);
            }
        }
        // function to load the request to delet accounts from a file with the name RequestDeletAccounts.txt
        public static void LoadRequestDeletAccountsToFile()
        {
            try
            {
                using (StreamReader reader = new StreamReader(RequestDeletAccountsFile))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        AccountDeletRequest.Push(line);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while loading the requests from the file: " + ex.Message);
            }
        }

    }


}

 
