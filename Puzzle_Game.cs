using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System;
namespace Puzzle_Game
{
    public partial class Puzzle_Game : Form
    {
        List<string> icons = new List<string> { "!", ",", "b", "k", "v", "w", "z", "N",
                                                "!", ",", "b", "k", "v", "w", "z", "N" };
        Random rand = new Random();
        int randomindex;
        Timer revealTimer = new Timer();  // Timer for the initial 5-second reveal on form load
        Timer hideFirstButtonTimer = new Timer();  // Timer to hide the first button if the second is not clicked
        Timer hideMismatchTimer = new Timer();  // Timer to hide mismatched buttons
        Timer countdownTimer = new Timer();  // Timer for countdown display
        Button first, second;
        int matchCount = 0;

        // Players
        int currentPlayer = 1;  // 1 for Player 1, 2 for Player 2
        int player1Score = 0;
        int player2Score = 0;


        // To track matched buttons
        HashSet<Button> matchedButtons = new HashSet<Button>();

        // Countdown variable
        int countdownTime = 5;  // 5 seconds countdown

        public Puzzle_Game()
        {
            InitializeComponent();

            // Initialize countdown label
            countdownLabel.Text = $"Time left: {countdownTime} seconds";


            revealTimer.Tick += RevealTimer_Tick;  // Attach event handler for reveal timer
            hideMismatchTimer.Tick += HideMismatchTimer_Tick;
            hideFirstButtonTimer.Tick += HideFirstButtonTimer_Tick;
            countdownTimer.Tick += CountdownTimer_Tick;  // Add countdown timer event

            revealTimer.Interval = 5000;  // 5 seconds for initial reveal
            hideMismatchTimer.Interval = 1000;  // 1 second to show mismatched buttons before hiding
            hideFirstButtonTimer.Interval = 5000;  // 5 seconds to hide first button if second isn't clicked
            countdownTimer.Interval = 1000;  // Countdown timer interval (1 second)

            revealTimer.Start();
            ShowAllButtons();
        }

        private void RevealTimer_Tick(object sender, EventArgs e)
        {
            revealTimer.Stop();
            // Hide all buttons after 5 seconds
            foreach (Control item in Controls)
            {
                if (item is Button btn)
                {
                    btn.ForeColor = btn.BackColor;  // Hide the buttons
                }
            }
            countdownTimer.Start();  // Start countdown after reveal
        }

        private void CountdownTimer_Tick(object sender, EventArgs e)
        {
            countdownTime--;
            countdownLabel.Text = $"Time left: {countdownTime} seconds";

            if (countdownTime <= 0)
            {
                countdownTimer.Stop();
                if (first != null)
                {
                    first.ForeColor = first.BackColor;  // Hide the first button
                    first = null;  // Reset first button selection
                    SwitchPlayer(); // Switch player
                }
            }
        }

        private void HideFirstButtonTimer_Tick(object sender, EventArgs e)
        {
            hideFirstButtonTimer.Stop();
            // If the second button isn't clicked, hide the first button
            if (first != null && second == null)
            {
                first.ForeColor = first.BackColor;  // Hide the first button
                first = null;  // Reset first button selection
                SwitchPlayer(); // Switch player
            }
        }

        private void HideMismatchTimer_Tick(object sender, EventArgs e)
        {
            hideMismatchTimer.Stop();
            // Null checks to avoid errors if one of the buttons is null
            if (first != null) first.ForeColor = first.BackColor;
            if (second != null) second.ForeColor = second.BackColor;

            first = null;
            second = null;
            SwitchPlayer();  // Switch player if there's a mismatch
        }

        private void ShowAllButtons()
        {
            foreach (Control item in Controls)
            {
                if (item is Button btn)
                {
                    randomindex = rand.Next(icons.Count);
                    btn.Text = icons[randomindex];
                    btn.ForeColor = Color.Black;  // Show all button icons initially
                }
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;

            // If the button is already matched, return without doing anything
            if (matchedButtons.Contains(clickedButton))
            {
                return;
            }

            if (first == null)
            {
                first = clickedButton;
                first.ForeColor = Color.Black;  // Show the first button
                hideFirstButtonTimer.Start();  // Start timer to hide if the second button is not clicked
                countdownTime = 5;  // Reset countdown time
                countdownLabel.Text = $"Time left: {countdownTime} seconds";  // Update label
                countdownTimer.Start();  // Start countdown timer
                return;
            }

            second = clickedButton;
            second.ForeColor = Color.Black;  // Show the second button

            hideFirstButtonTimer.Stop();  // Stop the timer since the second button is clicked
            countdownTimer.Stop();  // Stop the countdown timer

            if (first.Text == second.Text)
            {
                // Mark buttons as matched
                matchedButtons.Add(first);
                matchedButtons.Add(second);

                first.ForeColor = Color.Black;
                second.ForeColor = Color.Black;
                first = null;
                second = null;
                matchCount++;

                // Increase current player's score
                if (currentPlayer == 1)
                {
                    player1Score++;
                    player1Label.Text = $"Player 1: {player1Score}";
                }
                else
                {
                    player2Score++;
                    player2Label.Text = $"Player 2: {player2Score}";
                }

                if (matchCount == 8)
                {
                    Close();  // Close the game when all matches are found
                }
            }
            else
            {
                hideMismatchTimer.Start();  // Start the timer to hide mismatched buttons
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void SwitchPlayer()
        {
            if (currentPlayer == 1)
            {
                currentPlayer = 2;
                player1Label.BackColor = Color.Gray;
                player2Label.BackColor = Color.Red;  // Highlight Player 2's turn
            }
            else
            {
                currentPlayer = 1;
                player1Label.BackColor = Color.Red;  // Highlight Player 1's turn
                player2Label.BackColor = Color.Gray;
            }
        }
    }
}
