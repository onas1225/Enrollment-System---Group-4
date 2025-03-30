using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Enrollment_Form___Group_4
{
    public partial class Form1 : Form
    {
        SqlConnection connect = new SqlConnection(@"Data Source=DESKTOP-CVS0J8H\SQLEXPRESS;Initial Catalog=login;Integrated Security=True;Encrypt=True;TrustServerCertificate=True");

        public Form1()
        {
            InitializeComponent();
        }

        private void ExitBtn_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to exit the app?", "Exit Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void CreateAcc_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form2 lForm = new Form2();
            lForm.Show();
            this.Hide();
        }

      

        private void LoginBtn_Click(object sender, EventArgs e)
        {
            if (UserTxtBox.Text == "" || PasswordTxtBox.Text == "")
            {
                MessageBox.Show("Please fill all blank fields", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (connect.State != ConnectionState.Open)
                {
                    try
                    {
                        connect.Open();
                        String Check = "SELECT user_type FROM login WHERE username = @Username AND pass = @Pass";

                        using (SqlCommand checkUser = new SqlCommand(Check, connect))
                        {
                            checkUser.Parameters.AddWithValue("@Username", UserTxtBox.Text.Trim());
                            checkUser.Parameters.AddWithValue("@Pass", PasswordTxtBox.Text.Trim());

                            object result = checkUser.ExecuteScalar();

                            if (result != null)
                            {
                                string userType = result.ToString();

                                if (userType == "admin")
                                {
                                    MessageBox.Show("Login Successful", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                    MessageBox.Show("Welcome, Admin!");
                                    Form5 adminForm = new Form5();
                                    adminForm.Show();
                                }
                                else
                                {
                                    MessageBox.Show("Login Successful", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                    Form4 homeForm = new Form4();
                                    homeForm.Show();
                                }

                                this.Hide();
                            }
                            else
                            {
                                MessageBox.Show("Invalid Username or Password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error connecting to Database: " + ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        connect.Close();
                    }
                }
            }
        }


        private void ShowPass_CheckedChanged(object sender, EventArgs e)
        {
            if (ShowPass.Checked)
            {
                PasswordTxtBox.PasswordChar = '\0';
            }
            else
            {
                PasswordTxtBox.PasswordChar = '•';
            }
        }
    }
}
