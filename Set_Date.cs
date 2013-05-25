using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Data.SqlClient;
using System.Data;

namespace project_logic_client_on_form
{
    public partial class Rezervation : Form
    {
        SqlConnection conect = new SqlConnection(@"Data Source=ВАСЯ-ПК\SQLEXPRESS;Initial Catalog=Logic;Integrated Security=True");
        SqlCommand comand = new SqlCommand();
        SqlDataReader data_read;
        public Rezervation()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "")
            {
                try{
                DateTime data_start = DateTime.Parse(textBox1.Text, CultureInfo.InvariantCulture);
                DateTime data_end = DateTime.Parse(textBox2.Text, CultureInfo.InvariantCulture);
                value.label = " на період резервування з " + textBox1.Text + " до " + textBox2.Text;
                if (data_start < data_end)
                {
                    TimeSpan data = new TimeSpan();
                    data = data_end - data_start;
                    value.k_day = data.Days;
                    value.date_beginning = data_start.ToString();
                    value.date_end = data_end.ToString();
                    Close();
                }
                else
                    MessageBox.Show("Дата задана не коректно");
                }
                catch(DataException)
                {
                    MessageBox.Show("Дата задана не коректно");
                }                
            }
            else
                MessageBox.Show("Дата задана не коректно");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
