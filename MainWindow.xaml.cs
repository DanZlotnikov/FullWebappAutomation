using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static FullWebappAutomation.TestOnChrome;

namespace FullWebappAutomation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string[] usernames; // Stores the available usernames
        public static string[] tests; // Stores the available tests

        public string chosenUsername;

        Dictionary<string, bool> testsToRun; // <testName, bool> format to store which tests were chosen

        public static int marginTop = 15;
        public static int marginLeft = 23;

        public RadioButton[] usersRadioButtons;

        /// <summary>
        /// Initializes all class variables
        /// </summary>
        private void InitVariables()
        {
            usernames = new string[] { "automation@pepperitest.com", "flatqa@pepperitest.com", "daniel3@pepperitest.com" };
            tests = new string[]
            {
                "Resync",
                "Config Home Button",
                "Config App Buttons",
                "Sales Order",
                "Item Search",
                "Minimum Quantity",
                "Delete Cart Item",
                "Unit Price Discount",
                "Continue Ordering",
                "Duplicate Line Item", 
                "Inventory Alert",
                "Search Activity",
                "Delete Activity"
            };

            testsToRun = new Dictionary<string, bool>();
            for (int i = 0; i < tests.Length; i++)
            {
                testsToRun[tests[i]] = false;
            }

            usersRadioButtons = new RadioButton[usernames.Length];
        }

        private void InitRadioStackPanel()
        {
            // Create stack panel
            StackPanel usersStackPanel = new StackPanel()
            {
                Name = "UsersStackPanel"
            };

            MainGrid.Children.Add(usersStackPanel);

            // Create radiobuttons according to usernames array
            for (int i = 0; i < usernames.Length; i++)
            {
                usersRadioButtons[i] = new RadioButton
                {
                    Name = "Button" + i.ToString(),
                    Content = usernames[i],
                    GroupName = "userRadioButtons",
                    HorizontalAlignment = new HorizontalAlignment(),
                    Margin = i == 0 ? new Thickness(marginLeft, marginTop * 3, 0, 0) : new Thickness(marginLeft, marginTop, 0, 0),
                    VerticalAlignment = new VerticalAlignment()
                };
                usersRadioButtons[i].Checked += RadioButton_Checked;
            }

            foreach (RadioButton radiobutton in usersRadioButtons)
            {
                usersStackPanel.Children.Add(radiobutton);
            }
        }

        private void InitTextBlocks()
        {
            TextBlock usersTextBlock = new TextBlock
            {
                HorizontalAlignment = new HorizontalAlignment(),
                Height = 28,
                Margin = new Thickness(marginLeft, marginTop, 0, 0),
                TextWrapping = new TextWrapping(),
                VerticalAlignment = new VerticalAlignment(),
                Width = 339,
                Text = "Select the desired user to log into:",
                FontWeight = FontWeights.Bold
            };

            TextBlock testsTextBlock = new TextBlock
            {
                HorizontalAlignment = new HorizontalAlignment(),
                Height = 28,
                Margin = new Thickness(marginLeft, ((marginTop * 7) + (marginTop * usersRadioButtons.Length + 1)), 0, 0),
                TextWrapping = new TextWrapping(),
                VerticalAlignment = new VerticalAlignment(),
                Width = 339,
                Text = "Select tests to run:",
                FontWeight = FontWeights.Bold
            };

            MainGrid.Children.Add(usersTextBlock);
            MainGrid.Children.Add(testsTextBlock);
        }

        private void InitCheckboxes()
        {
            CheckBox[] testsCheckboxes = new CheckBox[tests.Length];

            int checkboxMarginLeft, checkboxMarginTop;

            // Create checkboxes according to tests array
            for (int i = 0; i < tests.Length; i++)
            {
                checkboxMarginLeft = marginLeft + (marginLeft * 7) * (i / 8);
                checkboxMarginTop = (marginTop * 12) + marginTop * (i % 8);
                testsCheckboxes[i] = new CheckBox
                {
                    Content = tests[i],
                    Margin = new Thickness(checkboxMarginLeft, checkboxMarginTop, 0, 0)

                };
                testsCheckboxes[i].Checked += Checkbox_Changed;
                testsCheckboxes[i].Unchecked += Checkbox_Changed;
            }

            foreach (CheckBox checkBox in testsCheckboxes)
                MainGrid.Children.Add(checkBox);
        }

        private void InitButtons()
        {
            Button runButton = new Button
            {
                Content = "Run Tests",
                Width = 156,
                Margin = new Thickness(-280, marginTop * 20, 0, 0),
                Height = 28,
            };
            runButton.Click += RunButtonClicked;

            Button exitButton = new Button
            {
                Content = "Exit",
                Width = 156,
                Margin = new Thickness(40, marginTop * 20, 0, 0),
                Height = 28,
            };
            exitButton.Click += ExitButtonClicked;

            MainGrid.Children.Add(exitButton);        
            MainGrid.Children.Add(runButton);
        }
    
        private void InitGrid()
        {
            InitTextBlocks();
            InitRadioStackPanel();
            InitCheckboxes();
            InitButtons();
        }

        private void RunButtonClicked(object sender, RoutedEventArgs e)
        {
            if (chosenUsername == null)
                return;
            else RunTests(chosenUsername, testsToRun);
        }

        private void ExitButtonClicked(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            // Check which radiobutton was chosen
            for (int i = 0; i < usersRadioButtons.Length; i++)
            {
                if (usersRadioButtons[i].IsChecked == true)
                    chosenUsername = usersRadioButtons[i].Content.ToString();
            }

        }

        private void Checkbox_Changed(object sender, RoutedEventArgs e)
        {
            // Get the sender of the action (Check/Uncheck)
            CheckBox obj = sender as CheckBox;
            string content = obj.Content.ToString();

            // Change testsToRun accordingly
            testsToRun[content] = (bool)obj.IsChecked;

        }

        public MainWindow()
        {
            InitializeComponent();
            InitVariables();
            InitGrid();
        }
    }
}
