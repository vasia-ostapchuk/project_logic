using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace project_logic_client_on_form
{
    public partial class Admin_Form : Form
    {
        SqlConnection conect = new SqlConnection(@"Data Source=SERGIUS-PC;Initial Catalog=Logic;Integrated Security=True");
        SqlCommand comand = new SqlCommand();
        SqlDataAdapter d_adapter = new SqlDataAdapter();
        SqlDataAdapter dataAdapter = new SqlDataAdapter();
        DataSet data_set = new DataSet();
        int switcher1 = 0;

        // запит для підключення до бази даних
        String connectionString = @"Data Source=SERGIUS-PC;Initial Catalog=Logic;Integrated Security=True";
        public Admin_Form()
        {
            InitializeComponent();
        }
        private void Admin_Form_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "logicDataSet.Turyst". При необходимости она может быть перемещена или удалена.
            this.turystTableAdapter.Fill(this.logicDataSet.Turyst);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "logicDataSet.Room". При необходимости она может быть перемещена или удалена.
            this.roomTableAdapter.Fill(this.logicDataSet.Room);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "logicDataSet.Rental_Point". При необходимости она может быть перемещена или удалена.
            this.rental_PointTableAdapter.Fill(this.logicDataSet.Rental_Point);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "logicDataSet.Rental". При необходимости она может быть перемещена или удалена.
            this.rentalTableAdapter.Fill(this.logicDataSet.Rental);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "logicDataSet.Hotel". При необходимости она может быть перемещена или удалена.
            this.hotelTableAdapter.Fill(this.logicDataSet.Hotel);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "logicDataSet.Administrator". При необходимости она может быть перемещена или удалена.
            this.administratorTableAdapter.Fill(this.logicDataSet.Administrator);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "logicDataSet.Car_reservation". При необходимости она может быть перемещена или удалена.
            this.car_reservationTableAdapter.Fill(this.logicDataSet.Car_reservation);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "logicDataSet.Location". При необходимости она может быть перемещена или удалена.
            this.locationTableAdapter.Fill(this.logicDataSet.Location);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "logicDataSet.Cars". При необходимости она может быть перемещена или удалена.
            this.carsTableAdapter.Fill(this.logicDataSet.Cars);
        }
        private void GetData(string selectCommand, System.Windows.Forms.DataGridView datagrid, System.Windows.Forms.BindingSource bindingSource) // сама чотка функція
        {
            datagrid.RowHeadersVisible = false;
            datagrid.DataSource = bindingSource;
            try
            {

                dataAdapter = new SqlDataAdapter(selectCommand, connectionString);
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);

                DataTable table = new DataTable();
                table.Locale = System.Globalization.CultureInfo.InvariantCulture;
                dataAdapter.Fill(table);
                bindingSource.DataSource = table;
            }
            catch (SqlException)
            {
                MessageBox.Show("Помилка при доступі до даних");
            }
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
           // Form1 frm = new Form1();
            //frm.Show();
        }



        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (switcher1 == 0)
            {
                string name_location = "";
                name_location = comboBox1.Text;
                label2.Text = "Вільні кімнати в локації: " + name_location;
                if (name_location == "AAA")
                { GetData("select LocationFK, Hotel_Name, Room_Number, Price from Hotel, Room where Hotel_IDFK = Hotel_ID AND Room_ID!=ALL(SELECT Room_IDFK FROM Room_reservation WHERE Status = 'дійсна') GROUP BY LocationFK, Hotel_Name,Room_Number, Price", dataGridView2, hotelBindingSource); }
                else
                    GetData("select LocationFK, Hotel_Name, Room_Number, Price from Hotel, Room where Hotel_IDFK = Hotel_ID AND Room_ID!=ALL(SELECT Room_IDFK FROM Room_reservation WHERE Status = 'дійсна') AND LocationFK LIKE'" + name_location + "%' GROUP BY LocationFK, Hotel_Name,Room_Number, Price", dataGridView2, hotelBindingSource);
            }
            if (switcher1 == 1)
            {
                string name_location = "";
                name_location = comboBox1.Text;
                label2.Text = "Зарезервовані кімнати в локації: " + name_location;
                if (name_location == "AAA")
                { GetData("select Hotel.LocationFK, Hotel_Name, Room.Room_Number, Date_beginning, Date_end, R.Price from Room_reservation R,Hotel,Room where Room.Room_ID = R.Room_IDFK AND Room.Hotel_IDFK = Hotel.Hotel_ID AND R.Status = 'дійсна' GROUP BY Hotel.LocationFK, Hotel_Name, Room.Room_Number, Date_beginning, Date_end, R.Price", dataGridView2, hotelBindingSource); }
                else
                    GetData("select Hotel.LocationFK, Hotel_Name, Room.Room_Number, Date_beginning, Date_end, R.Price from Room_reservation R,Hotel,Room where Room.Room_ID = R.Room_IDFK AND Room.Hotel_IDFK = Hotel.Hotel_ID AND R.Status = 'дійсна' AND Hotel.LocationFK LIKE'" + name_location + "%' GROUP BY Hotel.LocationFK, Hotel_Name, Room.Room_Number, Date_beginning, Date_end, R.Price", dataGridView2, hotelBindingSource);
            }
            /*if (switcher1 == 2)
            {
                string name_location = "";
                name_location = comboBox1.Text;
                label2.Text = "Вільні автомобілі в локації: " + name_location;
                if (name_location == "AAA")
                { GetData("SELECT Rental_NameFK AS Пункт_прокату, Date_bginning AS Початок_резервування, Date_end AS Кінець_резервування, Mark AS Марка FROM (Rental_Point JOIN (Car_reservation JOIN Cars ON Car_ID = Car_IDFK) ON Rental_Point_ID=Rental_Point_IDFK) WHERE GETDATE() BETWEEN Date_bginning AND Date_end GROUP BY Rental_NameFK,Date_bginning,Date_end,Mark", dataGridView2, car_reservationBindingSource); }
                else
                    GetData("SELECT Rental_NameFK AS Пункт_прокату, Date_bginning AS Початок_резервування, Date_end AS Кінець_резервування, Mark AS Марка FROM (Rental_Point JOIN (Car_reservation JOIN Cars ON Car_ID = Car_IDFK) ON Rental_Point_ID=Rental_Point_IDFK) WHERE GETDATE() BETWEEN Date_bginning AND Date_end AND Car_reservation.location LIKE'" + name_location + "%' GROUP BY Rental_NameFK,Date_bginning,Date_end,Mark", dataGridView2, car_reservationBindingSource);
            }*/
            if (switcher1 == 3)
            {
                string name_location = "";
                name_location = comboBox1.Text;
                label2.Text = "Зарезервовані автомобілі в локації: " + name_location;
                if (name_location == "AAA")
                { GetData("SELECT Rental_NameFK AS Пункт_прокату, Date_bginning AS Початок_резервування, Date_end AS Кінець_резервування, Mark AS Марка FROM (Rental_Point JOIN (Car_reservation JOIN Cars ON Car_ID = Car_IDFK) ON Rental_Point_ID=Rental_Point_IDFK) WHERE GETDATE() BETWEEN Date_bginning AND Date_end GROUP BY Rental_NameFK,Date_bginning,Date_end,Mark", dataGridView2, car_reservationBindingSource); }
                else
                GetData("SELECT Rental_NameFK AS Пункт_прокату, Date_bginning AS Початок_резервування, Date_end AS Кінець_резервування, Mark AS Марка FROM (Rental_Point JOIN (Car_reservation JOIN Cars ON Car_ID = Car_IDFK) ON Rental_Point_ID=Rental_Point_IDFK) WHERE GETDATE() BETWEEN Date_bginning AND Date_end AND Car_reservation.location LIKE'" + name_location + "%' GROUP BY Rental_NameFK,Date_bginning,Date_end,Mark", dataGridView2, car_reservationBindingSource);
            }
        }


        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox3.Text == "Автомобілі") { GetData("SELECT * FROM Cars", dataGridView2, carsBindingSource);}
            if (comboBox3.Text == "Адміністратори") { GetData("SELECT * FROM Administrator", dataGridView2, administratorBindingSource); }
            if (comboBox3.Text == "Готелі") { GetData("SELECT * FROM Hotel", dataGridView2, hotelBindingSource); }
            if (comboBox3.Text == "Кімнати") { GetData("SELECT * FROM Room", dataGridView2,roomBindingSource ); }
            if (comboBox3.Text == "Прокатники") { GetData("SELECT * FROM Rental", dataGridView2, rentalBindingSource); }
            if (comboBox3.Text == "Пункти прокату") { GetData("SELECT * FROM Rental_Point", dataGridView2, rental_PointBindingSource); }
            if (comboBox3.Text == "Туристи") { GetData("SELECT * FROM Turyst", dataGridView2, turystBindingSource); }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.Text == "Вільні кімнати") { switcher1 = 0; label3.Text = "Виберіть локацію"; comboBox1.Enabled = true; }
            if (comboBox2.Text == "Зарезервовані кімнати") { switcher1 = 1; comboBox1.Enabled = true; }
            if (comboBox2.Text == "Вільні автомобілі") { switcher1 = 2; comboBox1.Enabled = true; }
            if (comboBox2.Text == "Зарезервовані автомобілі") { switcher1 = 3; comboBox1.Enabled = true; }
        }
    }
}
