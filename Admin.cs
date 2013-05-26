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
            // TODO: данная строка кода позволяет загрузить данные в таблицу "logicDataSet.Room_reservation". При необходимости она может быть перемещена или удалена.
            this.room_reservationTableAdapter.Fill(this.logicDataSet.Room_reservation);
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
                label2.Text = "Вільні кімнати в локаціі: " + comboBox1.Text;
                value.nameLocation = comboBox1.Text;
                if (value.nameLocation == "AAA")
                { GetData("select Hotel_Name AS Готель, Room_Number AS Номер,Category AS Категорія, Room.Price AS Ціна_оренди FROM Room, Hotel,Room_reservation where Hotel_IDFK = Hotel_ID AND Room_ID NOT IN (select Room_IDFK FROM Room_reservation where  Status ='дійсна') AND GETDATE() NOT BETWEEN Date_beginning AND Date_end GROUP BY LocationFK, Hotel_Name,Room_Number,Category, Room.Price", dataGridView2, hotelBindingSource); }
                else
                { GetData("select Hotel_Name AS Готель, Room_Number AS Номер,Category AS Категорія, Room.Price AS Ціна_оренди FROM Room, Hotel,Room_reservation where Hotel_IDFK = Hotel_ID AND Room_ID NOT IN (select Room_IDFK FROM Room_reservation where  Status ='дійсна') AND GETDATE() NOT BETWEEN Date_beginning AND Date_end AND LocationFK LIKE'" + value.nameLocation + "%' GROUP BY LocationFK, Hotel_Name,Room_Number,Category, Room.Price", dataGridView2, hotelBindingSource); }
            }
            if (switcher1 == 1)
            {
                label2.Text = "Зарезервовані кімнати в локаціі: " + comboBox1.Text;
                value.nameLocation = comboBox1.Text;
                if (value.nameLocation == "AAA")
                { GetData("select Hotel_Name AS Готель, Room_Number AS Номер, Date_beginning AS Початок_резервування, Date_end AS Кінець_резервування, Category AS Категорія, Room_reservation.Price AS Ціна_оренди from Room_reservation ,Hotel,Room where Room.Room_ID = Room_IDFK AND Hotel_IDFK = Hotel_ID AND Status = 'дійсна' AND GETDATE() BETWEEN Date_beginning AND Date_end GROUP BY LocationFK, Hotel_Name,Room_Number, Date_beginning, Date_end, Category, Room_reservation.Price", dataGridView2, hotelBindingSource); }
                else
                { GetData("select Hotel_Name AS Готель, Room_Number AS Номер, Date_beginning AS Початок_резервування, Date_end AS Кінець_резервування, Category AS Категорія, Room_reservation.Price AS Ціна_оренди from Room_reservation,Hotel,Room where Room.Room_ID = Room_IDFK AND Hotel_IDFK = Hotel_ID AND Status = 'дійсна' AND GETDATE() BETWEEN Date_beginning AND Date_end AND LocationFK LIKE'" + value.nameLocation + "%' GROUP BY LocationFK, Hotel_Name,Room_Number, Date_beginning, Date_end, Category, Room_reservation.Price", dataGridView2, hotelBindingSource);}
            }
            if (switcher1 == 3)
            {
                label2.Text = "Зарезервовані автомобілі в локаціі: " + comboBox1.Text;
                value.nameLocation = comboBox1.Text;
                if (value.nameLocation == "AAA")
                { GetData("SELECT Rental_NameFK AS Пункт_прокату, Date_bginning AS Початок_резервування, Date_end AS Кінець_резервування, Mark AS Марка, Car_reservation.Price AS Ціна_Прокату FROM (Rental_Point JOIN (Car_reservation JOIN Cars ON Car_ID = Car_IDFK) ON Rental_Point_ID=Rental_Point_IDFK) WHERE GETDATE() BETWEEN Date_bginning AND Date_end AND Status='дійсна' GROUP BY Rental_NameFK,Date_bginning,Date_end,Mark,Car_reservation.Price", dataGridView2, car_reservationBindingSource); }
                else
                { GetData("SELECT Rental_NameFK AS Пункт_прокату, Date_bginning AS Початок_резервування, Date_end AS Кінець_резервування, Mark AS Марка, Car_reservation.Price AS Ціна_Прокату FROM (Rental_Point JOIN (Car_reservation JOIN Cars ON Car_ID = Car_IDFK) ON Rental_Point_ID=Rental_Point_IDFK) WHERE GETDATE() BETWEEN Date_bginning AND Date_end AND Status='дійсна' AND Car_reservation.location LIKE'" + value.nameLocation + "%' GROUP BY Rental_NameFK,Date_bginning,Date_end,Mark,Car_reservation.Price", dataGridView2, car_reservationBindingSource); }
            }
            if (switcher1 == 4)
            {
                label2.Text = "Перегляд заявок на " + comboBox1.Text;
                if (comboBox1.Text == "автомобілі")
                {
                    { GetData("SELECT Surname AS Прізвище, Name AS Ім_я, Rental_NameFK AS Пункт_прокату, Date_bginning AS Початок_резервування, Date_end AS Кінець_резервування, Mark AS Марка, Car_reservation.Price AS Ціна_Прокату FROM (Rental_Point JOIN (Car_reservation JOIN Cars ON Car_ID = Car_IDFK) ON Rental_Point_ID=Rental_Point_IDFK), Turyst WHERE Turyst_ID=Turyst_IDFK GROUP BY Rental_NameFK,Date_bginning,Date_end,Mark,Car_reservation.Price, Turyst.Surname, Turyst.Name", dataGridView2, car_reservationBindingSource); }
                }
                else
                {
                    { GetData("select Surname AS Прізвище, Name AS Ім_я, Hotel_Name AS Готель, Room_Number AS Номер, Date_beginning AS Початок_резервування, Date_end AS Кінець_резервування, Category AS Категорія, Room_reservation.Price AS Ціна_оренди from Room_reservation,Hotel,Room, Turyst where Turyst_ID=Turyst_IDFK AND Room.Room_ID = Room_IDFK AND Hotel_IDFK = Hotel_ID GROUP BY LocationFK, Hotel_Name,Room_Number, Date_beginning, Date_end, Category, Room_reservation.Price, Turyst.Surname, Turyst.Name", dataGridView2, hotelBindingSource); }
                }
            }
        }
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox1.Enabled = false;
            textBox1.Enabled = false;
            button1.Enabled = true;
            button2.Enabled = true;
            button2.Text= "Видалити стрічку";
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
            button1.Enabled = false;
            button2.Enabled = false;
            textBox1.Enabled = false;
            if (comboBox2.Text == "Вільні кімнати") { switcher1 = 0; label2.Text = "Вільні кімнати в локаціі: "; }
            if (comboBox2.Text == "Зарезервовані кімнати") { switcher1 = 1; label2.Text = "Зарезервовані кімнати в локаціі: "; }
            if (comboBox2.Text == "Вільні автомобілі")
            {
                switcher1 = 2;
                comboBox1.Enabled = false;
                label2.Text = "Вільні автомобілі";
                GetData("SELECT Rental_NameFK AS Пункт_прокату, Mark AS Марка, Type_kpp AS Тип, Cars.Price AS Ціна FROM Cars, Rental_Point, Car_reservation WHERE Rental_Point_IDFK=Rental_Point_ID AND Car_ID NOT IN(select Car_IDFK FROM Car_reservation WHERE  Status ='дійсна')AND GETDATE() NOT BETWEEN Date_bginning AND Date_end", dataGridView2, car_reservationBindingSource);
            }
            if (comboBox2.Text == "Зарезервовані автомобілі") { switcher1 = 3; label2.Text = "Зарезервовані автомобілі в локаціі: "; }
            if (comboBox2.Text == "Заявки вище певної ціни") { switcher1 = 4; label2.Text = "Перегляд заявок на "; }
            if (switcher1 == 0 || switcher1 == 1 || switcher1 == 3) 
            { comboBox1.Enabled = true; comboBox1.DataSource = locationBindingSource; comboBox1.DisplayMember = "Name_Location"; comboBox1.ValueMember = "Location_ID"; label3.Text = "Виберіть локацію"; }
            if (switcher1 == 4) { textBox1.Enabled = true; button2.Enabled = true; button2.Text = "Ввід ціни"; comboBox1.Enabled = true; comboBox1.DataSource = null; comboBox1.Items.Add("автомобілі"); comboBox1.Items.Add("кімнати"); label3.Text = "    Вид заявки"; }
            comboBox1_SelectedIndexChanged(sender, e);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (switcher1 == 4)
            {
                if (comboBox1.Text == "автомобілі")
                { GetData("SELECT Surname AS Прізвище, Name AS Ім_я, Rental_NameFK AS Пункт_прокату, Date_bginning AS Початок_резервування, Date_end AS Кінець_резервування, Mark AS Марка ,Car_reservation.Price AS Ціна_Прокату FROM (Rental_Point JOIN (Car_reservation JOIN Cars ON Car_ID = Car_IDFK) ON Rental_Point_ID=Rental_Point_IDFK), Turyst WHERE Turyst_ID=Turyst_IDFK AND Car_reservation.Price>'" + Convert.ToInt32(textBox1.Text+0) + "' GROUP BY Rental_NameFK,Date_bginning,Date_end,Mark, Car_reservation.Price, Turyst.Surname, Turyst.Name", dataGridView2, car_reservationBindingSource); }
                else
                { GetData("select Surname AS Прізвище, Name AS Ім_я, Hotel_Name AS Готель, Room_Number AS Номер, Date_beginning AS Початок_резервування, Date_end AS Кінець_резервування, Category AS Категорія, Room_reservation.Price AS Ціна_оренди from Room_reservation,Hotel,Room, Turyst where Turyst_ID=Turyst_IDFK AND Room.Room_ID = Room_IDFK AND Hotel_IDFK = Hotel_ID AND Room_reservation.Price>'" + Convert.ToInt32(textBox1.Text+0) + "' GROUP BY LocationFK, Hotel_Name,Room_Number, Date_beginning, Date_end, Category, Room_reservation.Price, Turyst.Surname, Turyst.Name", dataGridView2, hotelBindingSource); }
            }
        }

        private void comboBox2_Click(object sender, EventArgs e)
        {
            comboBox2_SelectedIndexChanged(sender, e);
        }

        private void comboBox3_Click(object sender, EventArgs e)
        {
            comboBox3_SelectedIndexChanged(sender, e);
        }

        private void textBox1_AcceptsTabChanged(object sender, EventArgs e)
        {
            button2_Click(sender, e);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Adding frm = new Adding();
            frm.Show();
        }
    }
}
