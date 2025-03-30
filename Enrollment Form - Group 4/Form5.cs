using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Enrollment_Form___Group_4
{
    public partial class Form5: Form
    {
        public Form5()
        {
            InitializeComponent();
        }

        private void Students_CheckedChanged(object sender, EventArgs e)
        {
            if (Students.Checked)
            {
                Form6 form = new Form6();
                form.Show();
                this.Hide();
            }
        
        }

        private void ExitBtn_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Go back to login?", "Exit Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Hide();
                Form1 loginForm = new Form1();
                loginForm.Show();
            }
        }
    }
}
