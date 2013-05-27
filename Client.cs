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

    public partial class Form1 : Form
    {
        SqlConnection conect = new SqlConnection(@"Data Source=ВАСЯ-ПК\SQLEXPRESS;Initial Catalog=Logic;Integrated Security=True");
        SqlCommand comand = new SqlCommand();
        SqlDataAdapter d_adapter = new SqlDataAdapter();
        SqlDataAdapter dataAdapter = new SqlDataAdapter();
        DataSet data_set = new DataSet();

        //string M = new string[1000, 10];

        string Rental_name = "";
        int tabcontrol_status = 0; // для контролю яка вкладка є активною on tabControl1
        int tabcontrol_status2 = 0; // для контролю яка вкладка є активною on tabControl2

        string name_hotel = "";
        string _id_room_reservation = "";
        string _id_car_reservation = "";

        // запит для підключення до бази даних
        String connectionString = @"Data Source=ВАСЯ-ПК\SQLEXPRESS;Initial Catalog=Logic;Integrated Security=True";

        public Form1()
        {
            InitializeComponent();
        }

        void viseble_vs_hide(DataGridView datagrid, Panel panel, Label label)
        {
            if (datagrid.RowCount != 0)
            {
                panel.Visible = true;
                label.Visible = false;
            }
            else
            {
                panel.Visible = false;
                label.Visible = true;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "logicDataSet.Cars". При необходимости она может быть перемещена или удалена.
            this.carsTableAdapter.Fill(this.logicDataSet.Cars);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "logicDataSet.Hotel". При необходимости она может быть перемещена или удалена.
            this.hotelTableAdapter.Fill(this.logicDataSet.Hotel);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "logicDataSet.Location". При необходимости она может быть перемещена или удалена.
            this.locationTableAdapter.Fill(this.logicDataSet.Location);
            label2.Text = "Локація не вибрана";
            label5.Text = label2.Text;
            label2.Visible = true;
            label5.Visible = true;
            label22.Text = "";
            // ************** Особисті дані **********
            label6.Text = value.Name_turyst;
            label8.Text = value.Surname_turyst;
            label10.Text = value.email_turyst;
            // ***************************************
            Location_view(conect);
        }
        
        private void GetData(string selectCommand, System.Windows.Forms.DataGridView datagrid, System.Windows.Forms.BindingSource bindingSource) // сама чотка функція
        {
            //BindingSource bindingSource = new System.Windows.Forms.BindingSource(this.components);
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

        public void Room_view()
        {
            label3.Visible = false;
            label22.Text = "Вільні кімнати" + value.label;
            GetData("select Room_Number AS Номер,Category AS Категорія,Price AS Ціна_оренди, Hotel_IDFK, Room_ID FROM Room where "
			        +"Room_ID NOT IN (select Room_IDFK FROM Room_reservation where  Status ='дійсна' "
					+"AND((Date_end >= CAST('"+value.date_beginning+"' AS date) and Date_end <= CAST('"+value.date_end+"' AS date)) "
					+"OR (Date_beginning >= CAST('"+value.date_beginning+"' AS date) and Date_beginning <= CAST('"+value.date_end+"' AS date))))"
			        +"AND Hotel_IDFK IN (select Hotel_ID from Hotel where Hotel_Name Like '"+name_hotel+"%' and LocationFK Like '"+value.nameLocation+"%')",dataGridView5,bindingSource_room);
            if (dataGridView5.RowCount == 0) { label20.Visible = true; } else { label20.Visible = false;}
            dataGridView5.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView5.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView5.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView5.Columns[3].Visible = false; // Hotel_IDFK не відображаєм
            dataGridView5.Columns[4].Visible = false; // Room_ID  нк відображаєм
        }
        public void Car_view()
        {
            label20.Visible = false;
            label22.Text = "Вільні автомобілі" + value.label;
            GetData("select Mark AS Марка,Color AS Колір,License_plate AS Номер, Type_kpp AS тип_КПП, Motor AS Двигун, Vypusk AS Рік_випуску, Places AS Мість, Litr_on_100 AS витрати_на_100, Cars.Price AS Ціна_прокату FROM Cars where "
                         + "Car_ID NOT IN (select Car_IDFK FROM Car_reservation where  Status ='дійсна' "
                         + "AND((Date_end >= CAST('" + value.date_beginning + "' AS date) and Date_end <= CAST('" + value.date_end + "' AS date)) "
                         + "OR (Date_bginning >= CAST('" + value.date_beginning + "' AS date) and Date_bginning <= CAST('" + value.date_end + "' AS date)))) "
                         + "AND Rental_Point_IDFK IN (select Rental_Point_ID from Rental_Point where LocationFK Like '" + value.nameLocation + "%' AND Rental_NameFK LIKE '" + Rental_name + "%')", dataGridView3, bindingSource2);
            if (dataGridView3.RowCount == 0) { label3.Visible = true; } else { label3.Visible = false;}
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e) // 
        {
            dataGridView5.Visible = false;
            dataGridView3.Visible = true;
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView2.Rows[e.RowIndex];
           
                value.nameLocation = row.Cells["Локація"].Value.ToString();
                Rental_name = row.Cells["Прокатник"].Value.ToString();
                // пошук доступних автомобілів
                Car_view();
                if (dataGridView3.RowCount > 0)
                {
                    button5.Enabled = true;
                }
                else
                {

                }
            }
        }


        private void dataGridView1_CellClick(object sender, System.Windows.Forms.DataGridViewCellEventArgs e) // вибір конкретного готелю
        {
            dataGridView5.Visible = true;
            dataGridView3.Visible = false;
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                value.nameLocation = row.Cells["Локація"].Value.ToString();
                name_hotel = row.Cells["Назва_готелю"].Value.ToString();
                // пошук доступних кімнат
                Room_view();
                if (dataGridView5.RowCount > 0)
                {
                    button5.Enabled = true;
                }
                else
                {
                }
            }

        }

        private void dataGridView4_CellClick(object sender, DataGridViewCellEventArgs e) // вибір локації
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView4.Rows[e.RowIndex];

                string name_location = row.Cells["Name_Location"].Value.ToString();
                if (name_location == "Усі локації")
                    name_location = "";
                GetData("select Hotel_Name AS Назва_готелю, Star AS Кількість_зірок, LocationFK AS Локація, Phone AS Телефон, email AS Email from Hotel where LocationFK  LIKE '" + name_location + "%' order by Star", dataGridView1, bindingSource3);
                GetData("select Rental_NameFK AS Прокатник, LocationFK AS Локація, Time_beginning, Time_end, email from Rental_Point where LocationFK LIKE '" + name_location + "%'", dataGridView2, bindingSource4);
                dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView2.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                if (dataGridView2.RowCount > 0)
                {
                    label5.Text = "В цій локації пункти прокату відсутні";
                    label5.Visible = false;
                }
                else
                {
                    label5.Text = "В цій локації пункти прокату відсутні";
                    label5.Visible = true;
                }
                if (dataGridView1.RowCount > 0)
                {
                    label2.Text = "В цій локації готелі відсутні";
                    label2.Visible = false;
                }
                else
                {
                    label2.Text = "В цій локації готелі відсутні";
                    label2.Visible = true;
                }
            }
        }


        private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView3.Rows[e.RowIndex];
                string car_number = row.Cells["Номер"].Value.ToString();
                value.price_car = row.Cells["Ціна_прокату"].Value.ToString();
                GetData("select Car_ID from Cars where License_plate LIKE '"+ car_number +"%'", dataGridView_turyst, bindingSource_reserv);
                if (value.date_beginning != "" && value.date_end != "") button4.Enabled = true;
            }
            
        }
        
        private void dataGridView_turyst_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (dataGridView_turyst.RowCount > 0 && dataGridView3.RowCount>0)
            {
                value._id_car = dataGridView_turyst[0, 0].Value.ToString();
            }
        }


        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (e.TabPageIndex == 0)
            {
                label20.Text = "Вільні кімнати відсутні";
                tabcontrol_status = 0;
                button5.Enabled = false;
            }
            if (e.TabPageIndex == 1)
            {
                label3.Text = "Вільні автомобілі відсутні";
                tabcontrol_status = 1;
                button5.Enabled = false;
            }
            if (e.TabPageIndex == 2)
            {
                label18.Text = "На даний момент у вас немає жодної заявки";
                label19.Text = label18.Text;
                tabcontrol_status = 2;
                // завантаження даних про заявки даного користувача про автомобілі
                HasRows_car(conect);
            }
            button6.Enabled = false;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void вихідToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void особистіДаніToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (особистіДаніToolStripMenuItem.Checked == false)
            {
                особистіДаніToolStripMenuItem.Checked = true;
                groupBox1.Visible = true;
            }
            else
            {
                особистіДаніToolStripMenuItem.Checked = false;
                groupBox1.Visible = false;
            }

        }
        private void button5_Click(object sender, EventArgs e)
        {
            Rezervation frm = new Rezervation();
            frm.ShowDialog();
            label22.Text += value.label;
            if (tabcontrol_status == 0) Room_view();
            else
                if (tabcontrol_status == 1) Car_view();
        }
        private void button4_Click_1(object sender, EventArgs e) // резервування автомобілів та кімнат
        {
            if (dataGridView5.Visible == true)
            {
                double price = value.k_day * Convert.ToDouble(value.price_room);
                conect.Open();
                comand.CommandText = "INSERT INTO [dbo].[Room_reservation] "
                                    + "([Room_IDFK]"
                                    + ",[Date_beginning]"
                                    + ",[Date_end]"
                                    + ",[Price]"
                                    + ",[Status]"
                                    + ",[Turyst_IDFK]) "
                                    + "VALUES ('"+value._id_room+"','"+value.date_beginning+"','"+value.date_end+"','"+price.ToString()+"','дійсна','"+value._id_turyst+"')";
                comand.Connection = conect;
                comand.ExecuteNonQuery();
                MessageBox.Show("Заявку додано.");
                conect.Close();
                // обновляєм список доступних автомобілів
                Room_view();
            }
            else
            {
                double price = value.k_day * Convert.ToDouble(value.price_car);
                conect.Open();
                comand.CommandText = "INSERT INTO [dbo].[Car_reservation] "
                + "([Date_bginning]"
                + ",[Date_end]"
                + ",[Turyst_IDFK]"
                + ",[Location]"
                + ",[Price]"
                + ",[Status]"
                + ",[Car_IDFK]) "
                    + "VALUES ('" + value.date_beginning + "','" + value.date_end + "','" + value._id_turyst + "','" + value.nameLocation + "','" + price.ToString() + "','дійсна','" + value._id_car + "')";
                comand.Connection = conect;
                comand.ExecuteNonQuery();
                MessageBox.Show("Заявку додано.");
                conect.Close();
                // обнавляєм список доступних автомобілів
                Car_view();
            }
            button4.Enabled = false;
        }
        public int HasRows_car(SqlConnection connection) // пошук заявок на автомобілі для конкретного користувача
        {
            SqlCommand comand = new SqlCommand("select DISTINCT Date_bginning,Date_end,Mark,License_plate,Location,C.Price,C.ID FROM Car_reservation C,Cars where C.Car_IDFK = Cars.Car_ID AND Turyst_IDFK = '" + value._id_turyst + "' AND Status = 'дійсна'");
            int i = 0;
            connection.Open();
            comand.Connection = connection;
            SqlDataReader reader = comand.ExecuteReader();
            dataGridView_reservation.Rows.Clear();
            dataGridView_reservation.Columns.Clear();
            if (reader.HasRows)
            {
                dataGridView_reservation.Columns.Add("Date_beginning", "Початок резервування");
                dataGridView_reservation.Columns.Add("Date_end", "Кінець резервування");
                dataGridView_reservation.Columns.Add("Mark", "Марка автомобіля");
                dataGridView_reservation.Columns.Add("License_plate", "Номер автомобіля");
                dataGridView_reservation.Columns.Add("Location", "Локація");
                dataGridView_reservation.Columns.Add("Price", "Вартість");
                dataGridView_reservation.Columns.Add("ID", "ID");
                label18.Visible = false;
                while (reader.Read()) { i++;
                    // ************ Відсікання часу від дати ***********
                    string date_begin = "";
                    for(int k = 0; k<10;k++)
                        date_begin += reader.GetDateTime(0).Date.ToString()[k];
                    string date_end = "";
                    for (int k = 0; k < 10; k++)
                        date_end += reader.GetDateTime(1).Date.ToString()[k];
                    //*******************************************************
                    dataGridView_reservation.Rows.Add(date_begin, date_end, reader.GetString(2), reader.GetString(3), reader.GetString(4), reader.GetDecimal(5),reader.GetInt32(6));
                }
                dataGridView_reservation.Columns[6].Visible = false;
                dataGridView_reservation.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView_reservation.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                //dataGridView_reservation.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView_reservation.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView_reservation.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView_reservation.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
               // dataGridView_reservation.Columns[2].Width = 200;
            }
            else
            {
                label18.Visible = true;
            }
            reader.Close();
            connection.Close();
            dataGridView_reservation.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);
            return i;
        }

        public void HasRows_room(SqlConnection connection) // пошук завок на кімнату для конкретного користувача
        {
            SqlCommand comand = new SqlCommand("select DISTINCT Date_beginning,Date_end,Hotel_Name,Room.Room_Number,Hotel.LocationFK,R.Price, R.ID from Room_reservation R,Hotel,Room "
                  + "where Room.Room_ID = R.Room_IDFK AND Turyst_IDFK = '" + value._id_turyst + "' AND Room.Hotel_IDFK = Hotel.Hotel_ID AND R.Status = 'дійсна'");
            int i = 0;
            connection.Open();
            comand.Connection = connection;
            SqlDataReader reader = comand.ExecuteReader();
            dataGridView_reservation_room.Rows.Clear();
            dataGridView_reservation_room.Columns.Clear();
            if (reader.HasRows)
            {
                dataGridView_reservation_room.Columns.Add("Date_beginning", "Початок резервування");
                dataGridView_reservation_room.Columns.Add("Date_end", "Кінець резервування");
                dataGridView_reservation_room.Columns.Add("Hotel", "Готель");
                dataGridView_reservation_room.Columns.Add("Number", "Номер кімнати");
                dataGridView_reservation_room.Columns.Add("Location", "Локація");
                dataGridView_reservation_room.Columns.Add("Price", "Вартість");
                dataGridView_reservation_room.Columns.Add("ID", "ID");
                label19.Visible = false;
                while (reader.Read())
                {
                    // ************ Відсікання часу від дати ***********
                    string date_begin = "";
                    for (int k = 0; k < 10; k++)
                        date_begin += reader.GetDateTime(0).Date.ToString()[k];
                    string date_end = "";
                    for (int k = 0; k < 10; k++)
                        date_end += reader.GetDateTime(1).Date.ToString()[k];
                    //*******************************************************
                    dataGridView_reservation_room.Rows.Add(date_begin, date_end, reader.GetString(2), reader.GetInt32(3), reader.GetString(4), reader.GetDecimal(5),reader.GetInt32(6));
                }
                dataGridView_reservation_room.Columns[6].Visible = false;
                dataGridView_reservation_room.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            else
            {
                label19.Visible = true;
            }
            reader.Close();
            connection.Close();
        }

        public void Location_view(SqlConnection cn)
        {
            SqlCommand comand = new SqlCommand("select Name_Location from dbo.Location order by Name_Location");
            cn.Open();
            comand.Connection = cn;
            SqlDataReader reader = comand.ExecuteReader();
            dataGridView4.Rows.Clear();
            dataGridView4.Columns.Clear();
            if (reader.HasRows)
            {
                dataGridView4.Columns.Add("Name_Location","Локація");
                dataGridView4.Rows.Add("Усі локації");
                while (reader.Read())
                    dataGridView4.Rows.Add(reader.GetString(0));
            }
        }

        private void tabControl2_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (e.TabPageIndex == 0)
            {
                tabcontrol_status2 = 0;
                // завантаження даних про заявки даного користувача про автомобілі
                HasRows_car(conect);
            }
            if (e.TabPageIndex == 1)
            {
                tabcontrol_status2 = 1;
                // завантаження даниx про заявки даного користуача про номери готелів                
                HasRows_room(conect);
            }
            button6.Enabled = false;
        }

        private void button6_Click(object sender, EventArgs e) // скасування заявок
        {
            if (tabcontrol_status2 == 0)
            {
                conect.Open();
                comand.CommandText = "update Car_reservation set Status = 'скасована' where Status = 'дійсна' and ID = '"+_id_car_reservation+"' ";
                comand.Connection = conect;
                comand.ExecuteNonQuery();
                MessageBox.Show("Заявку скасовано.");
                conect.Close();
                HasRows_car(conect);
            }
            if (tabcontrol_status2 == 1)
            {
                conect.Open();
                comand.CommandText = "update Room_reservation set Status = 'скасована' where Status = 'дійсна' and ID = '"+_id_room_reservation+"' ";
                comand.Connection = conect;
                comand.ExecuteNonQuery();
                MessageBox.Show("Заявку скасовано.");
                conect.Close();
                HasRows_room(conect);
            }
            button6.Enabled = false;
        }

        private void dataGridView_reservation_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView_reservation.Rows[e.RowIndex];                
                _id_car_reservation = row.Cells["ID"].Value.ToString();
                button6.Enabled = true;
            }
        }
        private void dataGridView_reservation_room_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView_reservation_room.Rows[e.RowIndex];
                _id_room_reservation = row.Cells["ID"].Value.ToString();
                button6.Enabled = true;
            }
        }
        private void dataGridView5_CellClick(object sender, DataGridViewCellEventArgs e) // вибір кімнати для резервування
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView5.Rows[e.RowIndex];
                value.price_room = row.Cells["Ціна_оренди"].Value.ToString();
                value._id_room = row.Cells["Room_ID"].Value.ToString();
                if (value.date_beginning != "" && value.date_end != "") button4.Enabled = true;
            }
        }

        private void назадToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 frm = new Form2();
            frm.Show();
            base.Hide();
        }

    }
}
