using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;

namespace project_logic_client_on_form
{

    public partial class Form1 : Form
    {
        SqlConnection conect = new SqlConnection(@"Data Source=ВАСЯ-ПК\SQLEXPRESS;Initial Catalog=Logic;Integrated Security=True");
        SqlCommand comand = new SqlCommand();
        SqlDataAdapter d_adapter = new SqlDataAdapter();
        SqlDataAdapter dataAdapter = new SqlDataAdapter();
        DataSet data_set = new DataSet();
        SqlCommandBuilder comand_b;

        //string M = new string[1000, 10];

        string Rental_name = "";
        int tabcontrol_status = 0; // для контролю яка вкладка є активною on tabControl1
        int tabcontrol_status2 = 0; // для контролю яка вкладка є активною on tabControl2

        string data_begin = "", data_end = ""; // для визначеня дати активної заявки 
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
           // name_LocationLabel.
           //Height = 376;
           // Width = 445;

            // ************** Особисті дані **********
            label6.Text = value.Name_turyst;
            label8.Text = value.Surname_turyst;
            label10.Text = value.email_turyst;
            // ***************************************
            GetData("select Name_Location from dbo.Location order by Name_Location,Location_ID", dataGridView4, bindingSource1);
            dataGridView4.Rows[0].SetValues("Усі локації");
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
            GetData("select Room_Number AS Номер,Category AS Ктегорія,Price AS Ціна_оренди, Hotel_IDFK, Room_ID from Room "
                        + "where "
                        + "Room_ID NOT IN (select Room_IDFK from Room_reservation where status = 'дійсна') "
                        + "AND Hotel_IDFK IN (select Hotel_ID from Hotel where Hotel_Name ='" + name_hotel + "' and LocationFK = '" + value.nameLocation + "') order by Category", dataGridView5, bindingSource_room);
            dataGridView5.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView5.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView5.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView5.Columns[3].Visible = false; // Hotel_IDFK не відображаєм
            dataGridView5.Columns[4].Visible = false; // Room_ID  нк відображаєм
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e) // 
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView2.Rows[e.RowIndex];
           
                value.nameLocation = row.Cells["Локація"].Value.ToString();
                Rental_name = row.Cells["Прокатник"].Value.ToString();
                // пошук доступних автомобілів
                GetData("select  Mark AS Марка,Color AS Колір,License_plate AS Номер, Type_kpp AS тип_КПП, Motor AS Двигун, Vypusk AS Рік_випуску, Places AS Мість, Litr_on_100 AS витрати_на_100, Cars.Price AS Ціна_прокату from Cars where "
                    + "Car_ID NOT IN (select Car_IDFK From Car_reservation where Status = 'дійсна')"// AND Date_bginning >= CAST('"+value.date_beginning+"' AS date) AND Date_end <= CAST('"+value.date_end+"' AS date)) "
                    + "AND "
                    + " Rental_Point_IDFK = any (select Rental_Point_ID from Rental_Point where LocationFK LIKE '" + value.nameLocation + "%' AND Rental_NameFK LIKE '"+Rental_name+"%')", dataGridView3, bindingSource2);
                if (dataGridView3.RowCount > 0)
                {
                  //  button4.Enabled = true;
                    label3.Visible = false;
                }
                else
                {
                    label3.Visible = true;
                    button4.Enabled = false;
                }
            }
        }

        void tabPage2_Click(object sender, System.EventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                value.nameLocation = row.Cells["Локація"].Value.ToString();
                name_hotel = row.Cells["Назва_готелю"].Value.ToString();
                // пошук доступних кімнат
                Room_view();
                if (dataGridView5.RowCount > 0)
                {
                      //button4.Enabled = true;
                    label20.Visible = false;
                }
                else
                {
                    label20.Visible = true;
                    //button4.Enabled = false;
                }
            }

        }

        private void dataGridView4_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView4.Rows[e.RowIndex];
                //dataGridView4.RowsAdded

                string name_location = row.Cells["Name_Location"].Value.ToString();
                if (name_location == "Усі локації")
                    name_location = "";
                GetData("select Hotel_Name AS Назва_готелю, Star AS Кількість_зірок, LocationFK AS Локація, Phone AS Телефон, email AS Email from Hotel where LocationFK  LIKE '" + name_location + "%' order by Star", dataGridView1, bindingSource3);
                GetData("select Rental_NameFK AS Прокатник, LocationFK AS Локація, Time_beginning, Time_end, email from Rental_Point where LocationFK LIKE '" + name_location + "%'", dataGridView2, bindingSource4);
                dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView2.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                //viseble_vs_hide(dataGridView2, panel2,label5);
                                
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
            button4.Enabled = true;
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView3.Rows[e.RowIndex];
                string car_number = row.Cells["Номер"].Value.ToString();
                label21.Text = car_number;
                value.price_car = row.Cells["Ціна_прокату"].Value.ToString();
                GetData("select Car_ID from Cars where License_plate LIKE '"+ car_number +"%'", dataGridView_turyst, bindingSource_reserv);
                button4.Enabled = true;
            }
            
        }
        

        //private void button6_Click_1(object sender, EventArgs e)
        //{
        //    if (radioButton_Client.Checked == true)
        //    {
        //        if (textBox_Name.Text != "" && textBox_Surname.Text != "" && textBox_Password.Text != "")
        //        {
        //            if (dataGridView_turyst.RowCount >= 0)
        //            {
        //                int i = 0, j = 0, k = 0;
        //                // пошук імені
        //                for (i = 0; i < dataGridView_turyst.RowCount; i++)
        //                    if (dataGridView_turyst[1, i].FormattedValue.ToString().Contains(textBox_Name.Text))
        //                    {
        //                        dataGridView_turyst.CurrentCell = dataGridView_turyst[1, i];
        //                        break;
        //                    }
        //                // пошук прізвища
        //                for (j = 0; j < dataGridView_turyst.RowCount; j++)
        //                    if (dataGridView_turyst[2, j].FormattedValue.ToString().Contains(textBox_Surname.Text))
        //                    {
        //                        dataGridView_turyst.CurrentCell = dataGridView_turyst[2, j];
        //                        break;
        //                    }
        //                // пошук пароля
        //                for (k = 0; k < dataGridView_turyst.RowCount; k++)
        //                    if (dataGridView_turyst[3, k].FormattedValue.ToString().Contains(textBox_Password.Text))
        //                    {
        //                        dataGridView_turyst.CurrentCell = dataGridView_turyst[3, k];
        //                        break;
        //                    }
        //                if (i == j & j == k)
        //                {
        //                    value._id_turyst = dataGridView_turyst[0, i].Value.ToString();
        //                    value.Name_turyst = dataGridView_turyst[1, i].Value.ToString();
        //                    value.Surname_turyst = dataGridView_turyst[2, i].Value.ToString();

        //                    base.Hide();
        //                    Width = 1000;
        //                    Height = 500;
        //                    panel_log.Visible = false;
        //                    base.Show();
        //                   
        //                }
        //            }
        //           // 
        //        }
        //        else
        //            MessageBox.Show("Поля для вводу не всі є заповненми. Заповніть поля.", "Попередження");
        //       /* else
        //            if (checkBox1.Checked == true)
        //            {
        //                //MessageBox.Show("Поля для вводу не всі є заповненми. Заповніть поля.", "Попередження");
                
        //            base.Hide();
        //            Width = 1000;
        //            Height = 500;
        //            panel_log.Visible = false;
        //            base.Show();

        //            GetData("select Name_Location from dbo.Location order by Name_Location", dataGridView4, bindingSource1);
        //            }    */
        //    }
        //    if (radioButton_admin.Checked == true) {
        //        Admin frm = new Admin();
        //        base.Hide(); 
        //        frm.Show();
        //    }
        //}

        //private void radioButton_Client_CheckedChanged(object sender, EventArgs e)
        //{
        //    GetData("select * from Turyst", dataGridView_turyst, bindingSource_turyst);
        //}

        //private void radioButton_admin_CheckedChanged(object sender, EventArgs e)
        //{
        //    GetData("select * from Administrator", dataGridView_turyst, bindingSource_admin);
        //}

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
                label3.Text = "Вільні кімнати відсутні";
                tabcontrol_status = 0;
                dataGridView5.Visible = true;
                dataGridView3.Visible = false;
            }
            if (e.TabPageIndex == 1)
            {
                label3.Text = "Вільні автомобілі відсутні";
                tabcontrol_status = 1;
                dataGridView5.Visible = false;
                dataGridView3.Visible = true;
            }
            if (e.TabPageIndex == 2)
            {
                label18.Text = "На даний момент у вас немає жодної заявки";
                label19.Text = label18.Text;
                tabcontrol_status = 2;
                // завантаження даних про заявки даного користувача про автомобілі
                SqlCommand comand = new SqlCommand("select DISTINCT Date_bginning,Date_end,Mark,License_plate,Location,C.Price from Car_reservation C,Cars where C.Car_IDFK = Cars.Car_ID AND Turyst_IDFK = '" + value._id_turyst + "' AND Status = 'дійсна'");
                HasRows_car(conect);
            }
            button6.Enabled = false;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void dataGridView2_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
           /* if(tabcontrol_status == 1)
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView2.Rows[e.RowIndex];

                value.nameLocation = row.Cells["Локація"].Value.ToString();
                Rental_name = row.Cells["Прокатник"].Value.ToString();
                // пошук доступних автомобілів
                GetData("select  Mark AS Марка,Color AS Колір,License_plate AS Номер, Type_kpp AS тип_КПП, Motor AS Двигун, Vypusk AS Рік_випуску, Places AS Мість, Litr_on_100 AS витрати_на_100, Cars.Price AS Ціна_прокату from Cars where "
                    + "Car_ID NOT IN (select Car_IDFK From Car_reservation where Status = 'дійсна')"// AND Date_bginning >= CAST('"+value.date_beginning+"' AS date) AND Date_end <= CAST('"+value.date_end+"' AS date)) "
                    + "AND "
                    + " Rental_Point_IDFK = any (select Rental_Point_ID from Rental_Point where LocationFK LIKE '" + value.nameLocation + "%' AND Rental_NameFK LIKE '" + Rental_name + "%')", dataGridView3, bindingSource2);
                if (dataGridView3.RowCount > 0)
                {
                   // button4.Enabled = true;
                    label3.Visible = false;
                }
                else
                {
                    button4.Enabled = false;
                    label3.Visible = true;
                }
            }*/
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

        private void dataGridView4_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            //dataGridView4_CellClick(sender, e);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Rezervation frm = new Rezervation();
            frm.ShowDialog();
            button4.Enabled = true;
        }
        private void button4_Click_1(object sender, EventArgs e)
        {
            if (dataGridView5.Visible == true)
            {
                label1.Text = value.date_beginning;
                label11.Text = value.date_end;
                label12.Text = value.k_day.ToString();
                label13.Text = value._id_turyst;
                label14.Text = value.nameLocation;
                label15.Text = value.price_room;
                double price = value.k_day * Convert.ToDouble(value.price_room);
                label16.Text = price.ToString();
                label17.Text = value._id_room;
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
                // пошук доступних кімнат
                Room_view();
            }
            else
            {
                label1.Text = value.date_beginning;
                label11.Text = value.date_end;
                label12.Text = value.k_day.ToString();
                label13.Text = value._id_turyst;
                label14.Text = value.nameLocation;
                label15.Text = value.price_car;
                double price = value.k_day * Convert.ToDouble(value.price_car);
                label16.Text = price.ToString();
                label17.Text = value._id_car;
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
            }
            button4.Enabled = false;
        }
        public int HasRows_car(SqlConnection connection)
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
                dataGridView_reservation.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView_reservation.Columns[2].Width = 100;
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

        public void HasRows_room(SqlConnection connection)
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
                label18.Visible = false;
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
            }
            else
            {
                label19.Visible = true;
            }
            reader.Close();
            connection.Close();
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

        private void button6_Click(object sender, EventArgs e)
        {
            if (tabcontrol_status2 == 0)
            {
                conect.Open();
                comand.CommandText = "update Car_reservation set Status = 'завершена' where Status = 'дійсна' and ID = '"+_id_car_reservation+"' ";
                comand.Connection = conect;
                comand.ExecuteNonQuery();
                MessageBox.Show("Заявку скасовано.");
                conect.Close();
                HasRows_car(conect);
            }
            if (tabcontrol_status2 == 1)
            {
                conect.Open();
                comand.CommandText = "update Room_reservation set Status = 'завершена' where Status = 'дійсна' and ID = '"+_id_room_reservation+"' ";
                comand.Connection = conect;
                comand.ExecuteNonQuery();
                MessageBox.Show("Заявку скасовано.");
                conect.Close();
                HasRows_room(conect);
            }
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
        private void dataGridView5_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            button4.Enabled = true;
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView5.Rows[e.RowIndex];
                value.price_room = row.Cells["Ціна_оренди"].Value.ToString();
                value._id_room = row.Cells["Room_ID"].Value.ToString();
                label21.Text = row.Cells["Номер"].Value.ToString();
                label1.Text = value.date_beginning;
                label11.Text = value.date_end;
                label12.Text = value.k_day.ToString();
                label13.Text = value._id_turyst;
                label14.Text = value.nameLocation;
                label15.Text = value.price_room;
                double price = value.k_day * Convert.ToDouble(value.price_room);
                label16.Text = price.ToString();
                label17.Text = value._id_room;
            }
        }

    }
}
