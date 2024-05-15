using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TextMessagingApp
{
    public partial class TextingApp : Form
    {
        public Point mouseLocation;
        public TextingApp()
        {
            InitializeComponent();
        }

        private int times_clicked = 0;
        private int times_clicked2 = 0;
        private int display_once  = 0;
        private bool isIndexChanged = false;

        private void TextingAppForm_Load(object sender, EventArgs e)
        {
            // Sit dalk die name in 'n list en loop deur hulle.
            lstContacts.Items.Add("Jacques Cloete");
            lstContacts.Items.Add("Danika Le Roux");
            lstContacts.Items.Add("Andre Pretorius");
            lstContacts.Items.Add("Ariël Truter");
            lstContacts.Items.Add("Christian Coetzee");
            lstContacts.Items.Add("Henk Mooiman");
            lstContacts.Items.Add("Jean-Luc Bégué");
            lstContacts.Items.Add("Waldo Nieman");
            lstContacts.Items.Add("Anri van Tonder");

            txtMessage.Text = "Enter a message here";
        }

        private void txtMessage_Click(object sender, EventArgs e)
        {
            if (times_clicked == 0)
            {
                txtMessage.Clear();
                txtMessage.ForeColor = Color.Black;
                times_clicked = 1;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

            if (lstContacts.SelectedIndex != -1)
            {
                
                if (!(txtMessage.Text == "Enter a message here") && !(String.IsNullOrEmpty(txtMessage.Text)))
                {
                    
                    if (isIndexChanged)
                    {
                        lstMessagesSent.Items.Clear();
                        lstMessagesSent.Items.Add($"You are now communicating with {lstContacts.SelectedItem}");
                        lstMessagesSent.Items.Add("-----------------------------------------------------------------------");
                    }
                    else //if (first_contact == lstContacts.SelectedIndex)
                    {               
                        if (display_once == 0)
                        {
                            lstMessagesSent.Items.Add($"You are now communicating with {lstContacts.SelectedItem}");
                            lstMessagesSent.Items.Add("-----------------------------------------------------------------------");
                            display_once = 1;
                        }   
                    }

                    string filteredMessage = filterSpecificWords(txtMessage.Text);
                    lstMessagesSent.Items.Add("You: " + filteredMessage);

                }
                else
                {
                    MessageBox.Show("Message cannot be an empty space.", "Something Title" , MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Please select a contact to send a message to.", "Something Title", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                pictureBox1_Click(pictureBox1, EventArgs.Empty);
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private string filterSpecificWords(string sentence)
        {
            // List of words that should not be in the sentence
            string[] unwantedWords = { "bad", "ugly", "nasty" };

            // Split the sentence into words
            string[] words = sentence.Split(new[] { ' ', ',', '.', ';', ':', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);

            // Filter out unwanted words            
            string filteredSentence = string.Join(" ", sentence.Split(new[] { ' ', ',', '.', ';', ':', '!', '?' }, StringSplitOptions.RemoveEmptyEntries)
                                                   .Select(word => unwantedWords.Contains(word.ToLower()) ? new string('*', word.Length) : word));


            return filteredSentence;
        }

        private void lstContacts_SelectedIndexChanged(object sender, EventArgs e)
        {    
            checkIfContactChanged(true);
        }

        private bool checkIfContactChanged(bool temp)
        {
            // Flawed logic function will always be true when called.
            return isIndexChanged = temp;
        }

        private void txtMessage_TextChanged(object sender, EventArgs e)
        {
            if (times_clicked2 == 0)
            {
                txtMessage.Clear();
                txtMessage.ForeColor = Color.Black;
                times_clicked2 = 1;
            }
        }

        private void lstMessagesSent_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = lstMessagesSent.SelectedIndex;

            if (index >= 0 && index != 0 && index != 1)
            {
                DialogResult result = MessageBox.Show("Do you want to delete this message?", "Confirmation", MessageBoxButtons.OKCancel);

                if (result == DialogResult.OK)
                {
                    lstMessagesSent.Items.RemoveAt(index);
                }
            }
        }

        private void mouse_Down(object sender, MouseEventArgs e)
        {
            mouseLocation = new Point(-e.X, -e.Y);
        }

        private void mouse_Move(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point mousePose = Control.MousePosition;
                mousePose.Offset(mouseLocation.X, mouseLocation.Y);
                Location = mousePose;
            }

        }
    }
}
