// needed libraryes 
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Text;

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
        public static string RatingsFilePath = "ratings.txt"; // file path for user feedback ratings
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
        static List<(string UserID, DateTime Date, double Amount, string Type)> HistoryTransactions= new List<(string, DateTime, double, string)>(); // fixd to use tuple for transactions history v2
        static List<string> UserPhoneNumbers = new List<string>();
        static List<string> UserAddresses = new List<string>();
        static List<bool> HasActiveLoan = new List<bool>(); // Parallel to users
        static Queue<string> LoanRequests = new Queue<string>(); // Stores UserIDs
        static Dictionary<string, (double Amount, double Interest)> LoanDetails = new Dictionary<string, (double, double)>(); // 
        static List<int> UserFeedbackRatings = new List<int>();  //User Feedback  Add Storage List
        static Queue<(string UserID, DateTime AppointmentTime)> Appointments = new Queue<(string, DateTime)>();
        static Dictionary<string, double> ExchangeRates = new Dictionary<string, double>()
{
    { "OMR", 1.0 },
    { "USD", 3.8 },
    { "EUR", 4.1 }
}; // list to do the ExchangeRates
        static Dictionary<string, int> FailedLoginAttempts = new Dictionary<string, int>();
        static HashSet<string> LockedAccounts = new HashSet<string>();
        public static List<double> LoanAmount = new List<double>();
        public static List<double> LoanInterestRate = new List<double>();





        static void Main(string[] args)
        {

            LoadAllData();// Load everything
            EnsureRatingsFileExists();
            WelcomeScreen();      // Let user interact (admin or user)
            SaveAllData();        // Save everything
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
                        PromptBackup();
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

                // Combine "admin" + ID to create a unique admin ID
                string adminID = "admin" + id;

                // Check if this admin ID already exists
                if (AdminID.Contains(adminID))
                {
                    Console.WriteLine("ID already exists. Please try again.");
                    WelcomeScreen();
                    return;
                }

                // Check if admin ID has valid format
                if (!adminID.StartsWith("admin"))
                {
                    Console.WriteLine("Access denied. Invalid admin ID format.");
                    return;
                }

                // Ask the user to enter a password
                Console.WriteLine("Please enter your password:");
                string password = ReadPassword(); // masks password
                string hashedPassword = HashPassword(password);

                // Save to login file
                File.AppendAllText("admin_login.txt", $"{adminID},{hashedPassword}\n");

                // Add to runtime lists
                AdminID.Add(adminID);
                AdminPassword.Add(hashedPassword); // store hashed, not plain

                Console.WriteLine(" Admin account created successfully.");

                // Save the login info to file
                SaveLoginToFile();

                // Go to admin menu
                AdminMenu();
            }
            catch (Exception ex)
            {
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
                string password = ReadPassword();
                string hashedPassword = HashPassword(password);

                if (LockedAccounts.Contains(id))
                {
                    Console.WriteLine("Account is locked. Contact admin.");
                    return;
                }


                // Check if the ID and password match any existing admin account
                bool isValidAdmin = false; // flag to track login success
                for (int i = 0; i < AdminID.Count; i++)
                {
                    string hashedInput = HashPassword(password);
                    if (AdminID[i] == id && AdminPassword[i] == hashedInput)

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

                    string inputChoice = Console.ReadLine();
                    int choice;

                    if (int.TryParse(inputChoice, out choice))
                    {
                        switch (choice)
                        {
                            case 1:
                                CreateAdminAccount();
                                break;
                            case 2:
                                Console.WriteLine("Thank you for using " + BankName);
                                break;
                            default:
                                Console.WriteLine("Invalid choice. Please select 1 or 2.");
                                break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a number (1 or 2).");
                    }

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
                    Console.WriteLine("10. Review Loan Requests");
                    Console.WriteLine("11. View Average User Feedback");
                    Console.WriteLine("12. View Full Transaction History");
                    Console.WriteLine("13. Unlock Locked Accounts");
                    Console.WriteLine("14. Advanced Reports (LINQ)");






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
                                PromptBackup();
                                flag = false; // Return to welcome screen
                                break;
                            case 10:
                                ReviewLoans();
                                break;
                            case 11:
                                ViewFeedbackStats();
                                break;
                            case 12:
                                PrintFullTransactionHistory();
                                break;
                            case 13:
                                UnlockAccount();
                                break;
                            case 14:
                                ShowLinqStats();
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
            Console.Clear(); // Fresh screen

            Console.WriteLine("Please enter your userID:");
            string id = Console.ReadLine();

            // Check if account is locked
            if (LockedAccounts.Contains(id))
            {
                Console.WriteLine("Your account is locked. Please contact the admin to unlock it.");
                return;
            }

            int index = UserID.IndexOf(id);
            if (index == -1)
            {
                Console.WriteLine("User not found. Do you want to create an account?");
                Console.WriteLine("1. Yes\n2. No");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        CreateACountRequest();
                        return;
                    case "2":
                        Console.WriteLine("Thank you for using " + BankName);
                        return;
                    default:
                        Console.WriteLine("Invalid choice.");
                        return;
                }
            }

            // Password attempt logic
            int attempts = 0;
            while (attempts < 3)
            {
                Console.WriteLine("Please enter your password:");
                string password = ReadPassword(); // Masked input
                string hashedInput = HashPassword(password);

                if (Userspassword[index] == hashedInput)
                {
                    Console.WriteLine("Welcome, " + id);
                    FailedLoginAttempts[id] = 0; // reset on success
                    UserMenu();
                    return;
                }

                attempts++;
                Console.WriteLine($"Incorrect password. Attempts left: {3 - attempts}");
            }

            // Lock the account after 3 failed tries
            LockedAccounts.Add(id);
            FailedLoginAttempts[id] = 3;
            Console.WriteLine("Your account has been locked after 3 failed login attempts.");
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
            Console.WriteLine("11. Generate Monthly Statement"); // Generate Monthly Statement v2
            Console.WriteLine("12. Update Phone Number or Address");
            Console.WriteLine("13. Request a Loan"); // Request a loan
            Console.WriteLine("14. View Recent Transactions or by Date");
            Console.WriteLine("15. View Full Transaction History");
            Console.WriteLine("16. Book Appointment");
            






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
                case "11":
                    Console.WriteLine("Generating Monthly Statement..."); // Generate Monthly Statement
                    GenerateMonthlyStatement();
                    Console.WriteLine("Monthly Statement generated successfully.");
                    break;
                case "12":
                    UpdateUserInfo(); // Update phone number or address
                    break;
                case "13":
                    RequestLoan();
                    break;
                case "14":
                    FilteredTransactionView();
                    break;


                case "15":
                    PrintFullTransactionHistory();
                    break;

                case "16":
                    BookAppointment();
                    break;

                case "18":
                    ShowLinqStats();
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
                string password = ReadPassword();
                string hashedPassword = HashPassword(password);


                Console.WriteLine("Please enter your National ID:");
                string nationalID = Console.ReadLine();

                Console.WriteLine("Please enter your money amount (must be more than 100):");
                if (!double.TryParse(Console.ReadLine(), out double amount) || amount < 100)
                {
                    Console.WriteLine("Invalid amount. Must be more than 100.");
                    return;
                }
                Console.WriteLine("Please enter your phone number:");
                string phone = Console.ReadLine();
                UserPhoneNumbers.Add(phone);

                Console.WriteLine("Please enter your address:");
                string address = Console.ReadLine();
                UserAddresses.Add(address);


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
                Userspassword.Add(hashedPassword); // 

                UserNationalID.Add(nationalID);
                Amount.Add(amount);
                StatesOfAccount.Add("Inproces");
                UserID.Add(name + accountNumber);
                string finalUserID = name + accountNumber;
                File.AppendAllText("login.txt", $"{finalUserID},{hashedPassword}\n");

                CreatAccountreadRequest.Enqueue(name + accountNumber);
                LoanAmount.Add(0);         // Default loan amount is 0
                LoanInterestRate.Add(0);   // Default interest is 0
                HasActiveLoan.Add(false); // No loan by default
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

                int index = UserID.IndexOf(userID);  // ✅ FIXED
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

                // Currency conversion
                Console.WriteLine("Enter currency (OMR, USD, EUR):");
                string currency = Console.ReadLine().ToUpper();

                if (!ExchangeRates.ContainsKey(currency))
                {
                    Console.WriteLine("Unsupported currency.");
                    return;
                }

                Console.WriteLine("Enter amount:");
                if (!double.TryParse(Console.ReadLine(), out double originalAmount) || originalAmount <= 0)
                {
                    Console.WriteLine("Invalid amount.");
                    return;
                }

                double omrAmount = originalAmount / ExchangeRates[currency];
                Amount[index] += omrAmount;

                // Record the transaction
                HistoryTransactions.Add((UserID[index], DateTime.Now, omrAmount, $"Deposit in {currency} ({originalAmount})"));
                Console.WriteLine($"Deposit successful. New balance: {Amount[index]:F2} OMR");

                // Feedback
                Console.WriteLine("Rate the service (1 to 5):");
                if (int.TryParse(Console.ReadLine(), out int rating) && rating >= 1 && rating <= 5)
                {
                    UserFeedbackRatings.Add(rating);
                    SaveRatingsToFile();

                }
                else
                {
                    Console.WriteLine("Invalid rating. Skipped.");
                }
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

                int index = UserID.IndexOf(userID); 

                if (index == -1)
                {
                    Console.WriteLine("Invalid User ID.");
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

                Amount[index] -= amount; // : You should subtract, not add
                Console.WriteLine($"Withdrawal successful. New balance: {Amount[index]}");

                HistoryTransactions.Add((UserID[index], DateTime.Now, amount, "Withdraw"));

                // Feedback feature
                Console.WriteLine("Rate the service (1 to 5):");
                if (int.TryParse(Console.ReadLine(), out int rating) && rating >= 1 && rating <= 5)
                {
                    UserFeedbackRatings.Add(rating);
                    SaveRatingsToFile();

                }
                else
                {
                    Console.WriteLine("Invalid rating. Skipped.");
                }

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

                if (HistoryTransactions.Count == 0)
                {
                    Console.WriteLine("No transactions found.");
                    return;
                }

                Console.WriteLine("Transactions:");
                foreach (var transaction in HistoryTransactions)
                {
                    Console.WriteLine($"Date: {transaction.Date}, Amount: {transaction.Amount}, Type: {transaction.Type}");
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                
            }
         
        }

        //submit a review
       
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
                HistoryTransactions.Add((senderID, DateTime.Now, amount, $"Transfer to {receiverID}"));
                HistoryTransactions.Add((receiverID, DateTime.Now, amount, $"Transfer from {senderID}"));


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



                Console.WriteLine("Rate the service (1 to 5):");
                if (int.TryParse(Console.ReadLine(), out int rating) && rating >= 1 && rating <= 5)
                {
                    UserFeedbackRatings.Add(rating);
                }
                else
                {
                    Console.WriteLine("Invalid rating. Skipped.");
                }

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
                    int minCount = new[] {
                AccounstNumber.Count, UserName.Count, Age.Count,
                Userspassword.Count, UserNationalID.Count, Amount.Count,
                StatesOfAccount.Count, UserPhoneNumbers.Count, UserAddresses.Count
            }.Min();

                    for (int i = 0; i < minCount; i++)
                    {
                        writer.WriteLine($"{AccounstNumber[i]},{UserName[i]},{Age[i]},{Userspassword[i]},{UserNationalID[i]},{Amount[i]},{StatesOfAccount[i]},{UserPhoneNumbers[i]},{UserAddresses[i]},{HasActiveLoan[i]},{LoanAmount[i]},{LoanInterestRate[i]}");

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
                        UserPhoneNumbers.Add(parts[7]);
                        UserAddresses.Add(parts[8]);
                        HasActiveLoan.Add(bool.Parse(parts[9]));
                        LoanAmount.Add(double.Parse(parts[10]));
                        LoanInterestRate.Add(double.Parse(parts[11]));

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
                    foreach (string review in Review.Reverse()) // To preserve correct order
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
                if (File.Exists(ReviewsFilePath))
                {
                    string[] lines = File.ReadAllLines(ReviewsFilePath);
                    foreach (string line in lines)
                    {
                        Review.Push(line); // Or use .Add(line) if Review is a List
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
                int minCount = Math.Min(UserID.Count, Userspassword.Count);
                using (StreamWriter writer = new StreamWriter("login.txt"))
                {
                    for (int i = 0; i < minCount; i++)
                    {
                        writer.WriteLine($"{UserID[i]},{Userspassword[i]}");
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


        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++V2 functions +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        //read the password from the console without showing it
        static string ReadPassword()
        {
            string password = "";
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    password += key.KeyChar;
                    Console.Write("*");
                }
                else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password = password.Substring(0, password.Length - 1);
                    Console.Write("\b \b");
                }
            } while (key.Key != ConsoleKey.Enter);

            Console.WriteLine(); // Move to next line
            return password;
        }
       // masking the functions so it can help 


         static string HashPassword(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder builder = new StringBuilder();
            foreach (byte b in bytes)
                builder.Append(b.ToString("x2")); // Convert to hexadecimal
            return builder.ToString();
        }
    }


        // GenerateMonthlyStatement()
        public static void GenerateMonthlyStatement()
        {
            try
            {
                Console.WriteLine("Enter your UserID:");
                string userID = Console.ReadLine();

                Console.WriteLine("Enter month (1-12):");
                int month = int.Parse(Console.ReadLine());

                Console.WriteLine("Enter year (e.g., 2025):");
                int year = int.Parse(Console.ReadLine());

                var userTransactions = HistoryTransactions
                    .Where(t => t.UserID == userID && t.Date.Month == month && t.Date.Year == year)
                    .ToList();

                if (userTransactions.Count == 0)
                {
                    Console.WriteLine("No transactions found for this period.");
                    return;
                }

                string fileName = $"Statement_{userID}_{year}-{month:D2}.txt";
                using (StreamWriter writer = new StreamWriter(fileName))
                {
                    writer.WriteLine($"Monthly Statement for {userID} - {month:D2}/{year}");
                    writer.WriteLine("--------------------------------------------------");
                    foreach (var transaction in userTransactions)
                    {
                        writer.WriteLine($"{transaction.Date}: {transaction.Type} - {transaction.Amount}");
                    }
                }

                Console.WriteLine($"Statement saved to {fileName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while generating the statement: " + ex.Message);
            }

        }
        //UpdateUserInfo()
        public static void UpdateUserInfo()
        {
            try
            {
                Console.WriteLine("Enter your UserID:");
                string userID = Console.ReadLine();

                int index = UserID.IndexOf(userID);
                if (index == -1)
                {
                    Console.WriteLine("Invalid UserID.");
                    return;
                }

                Console.WriteLine("What would you like to update?");
                Console.WriteLine("1. Phone Number");
                Console.WriteLine("2. Address");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.WriteLine("Enter new phone number:");
                        string newPhone = Console.ReadLine();
                        UserPhoneNumbers[index] = newPhone;
                        Console.WriteLine("Phone number updated successfully.");
                        break;
                    case "2":
                        Console.WriteLine("Enter new address:");
                        string newAddress = Console.ReadLine();
                        UserAddresses[index] = newAddress;
                        Console.WriteLine("Address updated successfully.");
                        break;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }

                SaveAccountsToFile(); // Save changes
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while updating info: " + ex.Message);
            }
        }

        // RequestLoan() 
        public static void RequestLoan()
        {
            try
            {
                Console.WriteLine("Enter your UserID:");
                string userID = Console.ReadLine();
                int index = UserID.IndexOf(userID);

                if (index == -1)
                {
                    Console.WriteLine("Invalid UserID.");
                    return;
                }

                if (Amount[index] < 5000)
                {
                    Console.WriteLine("You must have at least 5000 to be eligible.");
                    return;
                }

                if (HasActiveLoan[index])
                {
                    Console.WriteLine("You already have an active loan.");
                    return;
                }

                Console.WriteLine("Enter loan amount:");
                if (!double.TryParse(Console.ReadLine(), out double loanAmount) || loanAmount <= 0)
                {
                    Console.WriteLine("Invalid amount.");
                    return;
                }

                Console.WriteLine("Enter interest rate (e.g., 5 for 5%):");
                if (!double.TryParse(Console.ReadLine(), out double rate) || rate < 0)
                {
                    Console.WriteLine("Invalid interest rate.");
                    return;
                }

                LoanRequests.Enqueue(userID);
                LoanDetails[userID] = (loanAmount, rate);
                Console.WriteLine("Loan request submitted. Awaiting admin approval.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error requesting loan: " + ex.Message);
            }
        }

        public static void ReviewLoans()
        {
            try
            {
                while (LoanRequests.Count > 0)
                {
                    string userID = LoanRequests.Dequeue();
                    int index = UserID.IndexOf(userID);

                    if (index == -1)
                    {
                        Console.WriteLine($"User {userID} not found.");
                        continue;
                    }

                    var (amount, rate) = LoanDetails[userID];
                    Console.WriteLine($"Loan request from {userID}");
                    Console.WriteLine($"Amount: {amount}, Interest Rate: {rate}%");
                    Console.WriteLine("1. Approve");
                    Console.WriteLine("2. Reject");
                    string decision = Console.ReadLine();

                    if (decision == "1")
                    {
                        Amount[index] += amount;
                        HasActiveLoan[index] = true;

                        // ✅ Add this:
                        LoanAmount[index] = amount;
                        LoanInterestRate[index] = rate;

                        HistoryTransactions.Add((userID, DateTime.Now, amount, "Loan Approved"));
                        Console.WriteLine("Loan approved and amount added to balance.");
                    }
                    else
                    {
                        Console.WriteLine("Loan request rejected.");
                    }

                    LoanDetails.Remove(userID);
                }

                if (LoanRequests.Count == 0)
                {
                    Console.WriteLine("No pending loan requests.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Reviewing loans: " + ex.Message);
            }
        }

        // FilteredTransactionView function 
        public static void FilteredTransactionView()
        {
            Console.WriteLine("Enter your UserID:");
            string userID = Console.ReadLine();

            Console.WriteLine("Choose filter:");
            Console.WriteLine("1. Last N transactions");
            Console.WriteLine("2. Transactions after specific date");

            string choice = Console.ReadLine();
            if (choice == "1")
            {
                Console.WriteLine("How many recent transactions to view?");
                int count = int.Parse(Console.ReadLine());

                var recent = HistoryTransactions
                    .Where(t => t.UserID == userID)
                    .OrderByDescending(t => t.Date)
                    .Take(count);

                foreach (var t in recent)
                    Console.WriteLine($"{t.Date}: {t.Type} - {t.Amount}");
            }
            else if (choice == "2")
            {
                Console.WriteLine("Enter date (yyyy-mm-dd):");
                DateTime date = DateTime.Parse(Console.ReadLine());

                var filtered = HistoryTransactions
                    .Where(t => t.UserID == userID && t.Date > date);

                foreach (var t in filtered)
                    Console.WriteLine($"{t.Date}: {t.Type} - {t.Amount}");
            }
        }
        // ViewFeedbackStats fuctions 
        public static void ViewFeedbackStats()
        {
            if (UserFeedbackRatings.Count == 0)
            {
                Console.WriteLine("No feedback submitted yet.");
                return;
            }

            double avg = UserFeedbackRatings.Average();
            Console.WriteLine($"Average feedback score: {avg:F2} out of 5");
        }


        // PrintFullTransactionHistory function
        public static void PrintFullTransactionHistory()
        {
            Console.WriteLine("Enter UserID to view history:");
            string userID = Console.ReadLine();

            var userTransactions = HistoryTransactions
                .Where(t => t.UserID == userID)
                .OrderBy(t => t.Date);

            double runningBalance = 0;

            foreach (var t in userTransactions)
            {
                if (t.Type.Contains("Withdraw")) runningBalance -= t.Amount;
                else runningBalance += t.Amount;

                Console.WriteLine($"{t.Date:yyyy-MM-dd HH:mm} | {t.Type,-20} | Amount: {t.Amount,-10} | Balance after: {runningBalance}");
            }

            if (!userTransactions.Any())
                Console.WriteLine("No transactions found for this user.");
        }

        public static void BookAppointment()
        {
            Console.WriteLine("Enter your UserID:");
            string userID = Console.ReadLine();

            if (Appointments.Any(a => a.UserID == userID))
            {
                Console.WriteLine("You already have an appointment booked.");
                return;
            }

            Console.WriteLine("Enter date and time (yyyy-MM-dd HH:mm):");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime apptTime))
            {
                Console.WriteLine("Invalid date/time.");
                return;
            }

            Appointments.Enqueue((userID, apptTime));
            Console.WriteLine("Appointment booked successfully.");
        }




        public static void UnlockAccount()
        {
            Console.WriteLine("Enter UserID to unlock:");
            string userID = Console.ReadLine();

            if (LockedAccounts.Contains(userID))
            {
                LockedAccounts.Remove(userID);
                FailedLoginAttempts[userID] = 0;
                Console.WriteLine("Account unlocked successfully.");
            }
            else
            {
                Console.WriteLine("This account is not locked.");
            }
        }



        public static void ShowLinqStats()
        {
            Console.WriteLine("1. Top 5 Users by Balance");
            Console.WriteLine("2. Group Transactions by Type (user)");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    var top = Amount
                        .Select((amt, i) => new { Name = UserName[i], Balance = amt })
                        .OrderByDescending(x => x.Balance)
                        .Take(5);
                    foreach (var u in top)
                        Console.WriteLine($"{u.Name}: {u.Balance} OMR");
                    break;

                case "2":
                    Console.WriteLine("Enter UserID:");
                    string uid = Console.ReadLine();
                    var grouped = HistoryTransactions
                        .Where(t => t.UserID == uid)
                        .GroupBy(t => t.Type);
                    foreach (var g in grouped)
                    {
                        Console.WriteLine($"\n--- {g.Key} ---");
                        foreach (var t in g)
                            Console.WriteLine($"{t.Date}: {t.Amount} OMR");
                    }
                    break;
            }
        }




        // some improvement 

        static void LoadAllData()
        {
            LoadAccountsToFile();
            LoadLoginToFile();
            LoadRequestCreatAccountsToFile();
            LoadRequestDeletAccountsToFile();
            LoadRequestBlockAccountsToFile();
            LoadReviewsToFile();
            LoadAdminLoginToFile();
            LoadRatingsToFile();
            LoadTransactionsFromFile();
        }

        static void SaveAllData()
        {
            SaveAccountsToFile();
            SaveLoginToFile();
            SaveRequestCreatAccountsToFile();
            SaveRequestDeletAccountsToFile();
            SaveRequestBlockAccountsToFile();
            SaveReviewsToFile();
            SaveRatingsToFile();
            SaveTransactionsToFile();


        }

        static void PromptBackup()
        {
            Console.WriteLine("Would you like to back up your data before exiting? (yes/no)");
            string input = Console.ReadLine().Trim().ToLower();

            if (input == "yes")
            {
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HHmm");
                string fileName = $"Backup_{timestamp}.txt";

                try
                {
                    using (StreamWriter writer = new StreamWriter(fileName))
                    {
                        writer.WriteLine("== ACCOUNTS ==");
                        for (int i = 0; i < AccounstNumber.Count; i++)
                        {
                            writer.WriteLine($"{AccounstNumber[i]}, {UserName[i]}, {Amount[i]}, {StatesOfAccount[i]}");
                        }

                        writer.WriteLine("\n== TRANSACTIONS ==");
                        foreach (var t in HistoryTransactions)
                        {
                            writer.WriteLine($"{t.UserID}, {t.Date}, {t.Amount}, {t.Type}");
                        }

                        writer.WriteLine("\n== FEEDBACK ==");
                        foreach (var f in UserFeedbackRatings)
                        {
                            writer.WriteLine($"Rating: {f}");
                        }
                    }

                    Console.WriteLine($" Backup saved to: {fileName}");
                    ExitAndSave();

                }
                catch (Exception ex)
                {
                    Console.WriteLine($" Failed to save backup: {ex.Message}");
                }
            }
            else if (input == "no")
            {
                Console.WriteLine("No backup created.");
                ExitAndSave();

            }
            else
            {
                Console.WriteLine("Invalid input. Skipping backup.");
                ExitAndSave();

            }
        }


        public static void LoadAdminLoginToFile()
        {
            try
            {
                if (File.Exists("admin_login.txt"))
                {
                    string[] lines = File.ReadAllLines("admin_login.txt");
                    foreach (string line in lines)
                    {
                        string[] parts = line.Split(',');
                        if (parts.Length == 2)
                        {
                            AdminID.Add(parts[0]);
                            AdminPassword.Add(parts[1]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading admin login data: " + ex.Message);
            }
        }



        public static void EnsureRatingsFileExists()
        {
            string path = "ratings.txt";
            if (!File.Exists(path))
            {
                File.Create(path).Close(); // Creates the file and immediately closes it
                Console.WriteLine("ratings.txt created automatically.");
            }
        }



        public static void LoadRatingsToFile()
        {
            try
            {
                if (File.Exists(RatingsFilePath))
                {
                    string[] lines = File.ReadAllLines(RatingsFilePath);
                    UserFeedbackRatings = lines.Select(int.Parse).ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading feedback ratings: " + ex.Message);
            }
        }

        public static void SaveRatingsToFile()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter("ratings.txt", false)) // overwrite mode
                {
                    foreach (int rating in UserFeedbackRatings)
                    {
                        writer.WriteLine(rating);
                    }
                }
                Console.WriteLine("Ratings saved to file.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saving ratings: " + ex.Message);
            }
        }


        public static void SaveTransactionsToFile()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter("transactions.txt"))
                {
                    foreach (var t in HistoryTransactions)
                    {
                        writer.WriteLine($"{t.UserID},{t.Date},{t.Amount},{t.Type}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saving transactions: " + ex.Message);
            }
        }

   


        public static void LoadTransactionsFromFile()
        {
            try
            {
                if (!File.Exists("transactions.txt")) return;

                foreach (string line in File.ReadLines("transactions.txt"))
                {
                    string[] parts = line.Split(',');
                    if (parts.Length == 4 &&
                        DateTime.TryParse(parts[1], out DateTime date) &&
                        double.TryParse(parts[2], out double amount))
                    {
                        HistoryTransactions.Add((parts[0], date, amount, parts[3]));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading transactions: " + ex.Message);
            }
        }

        public static void ExitAndSave()
        {
            try
            {
                SaveAllData(); // Save everything before exiting
                Console.WriteLine("All data saved successfully. Exiting now.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while saving data: " + ex.Message);
            }
            finally
            {
                Environment.Exit(0); // Safe exit
            }
        }



    }


}

 
