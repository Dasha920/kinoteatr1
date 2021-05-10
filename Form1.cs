using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data; //Збірка потрібна для підключення до БД
using System.Data.SqlClient; //Потрібна для підключення до БД
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace appliances_store
{
    public partial class Form1 : Form
    {
        SqlConnection sqlConnection;
        ToolStripLabel dateLabel;
        ToolStripLabel timeLabel;
        ToolStripLabel infoLabel;
        Timer timer;

        public Form1()
        {
            InitializeComponent();

            infoLabel = new ToolStripLabel();
            infoLabel.Text = "Поточна дата та час:";
            dateLabel = new ToolStripLabel();
            timeLabel = new ToolStripLabel();

            statusStrip1.Items.Add(infoLabel);
            statusStrip1.Items.Add(dateLabel);
            statusStrip1.Items.Add(timeLabel);

            timer = new Timer() { Interval = 1000 };
            timer.Tick += timer_Tick;
            timer.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {//Метод що бере час 
            dateLabel.Text = DateTime.Now.ToLongDateString();
            timeLabel.Text = DateTime.Now.ToLongTimeString();
        }

        private async void Form1_Load(object sender, EventArgs e) //async асинхронного використання методу
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\Kyrsova741\курсова\Database.mdf;Integrated Security=True";
            // @ - Переводить в Юнікод те що в лапках
            // підключаемось до бази даних
            sqlConnection = new SqlConnection(connectionString);
            await sqlConnection.OpenAsync(); //відчиняємо базу даних

            SqlDataReader sqlReader = null;

            SqlCommand command = new SqlCommand("SELECT * FROM [Seans]", sqlConnection); //визиваємо всі продукти
            try
            {
                sqlReader = await command.ExecuteReaderAsync();
                //await усюди потрібен для того щоб програма виконувалася одночасно
                while (await sqlReader.ReadAsync()) //читаємо одну строку продукту
                {
                    textBox7.Text += ("\r\nId: " + Convert.ToString(sqlReader["Id"]) +
                        "\r\nНазва: " + Convert.ToString(sqlReader["Movie"]) +
                        "\r\nЖанри: " + Convert.ToString(sqlReader["Genre"]) +
                        "\r\nТривалість: " + Convert.ToString(sqlReader["Duration"]) +
                        "\r\nЦіна: " + Convert.ToString(sqlReader["Price"]) +
                        "\r\nПочаток сеансу: " + Convert.ToString(sqlReader["Pochatok"]) +
                        "\r\n****************************************************************************");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Close(); 
            }
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit(); //зачиняє з'єдання з БД
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (sqlConnection != null && sqlConnection.State != ConnectionState.Closed)
                sqlConnection.Close(); //зачиняє з'єдання з БД
        }

        private async void button1_Click_1(object sender, EventArgs e)
        {//додавання нового рядка в базу даних
            if (label7.Visible)
                label7.Visible = false; //якщо користувач увів все вірно

            if (!string.IsNullOrEmpty(textBox1.Text) && !string.IsNullOrWhiteSpace(textBox1.Text) &&
                !string.IsNullOrEmpty(textBox2.Text) && !string.IsNullOrWhiteSpace(textBox2.Text) &&
                !string.IsNullOrEmpty(maskedTextBox1.Text) && !string.IsNullOrWhiteSpace(maskedTextBox1.Text) &&
                !string.IsNullOrEmpty(textBox8.Text) && !string.IsNullOrWhiteSpace(textBox8.Text) &&
                !string.IsNullOrEmpty(maskedTextBox2.Text) && !string.IsNullOrWhiteSpace(maskedTextBox2.Text) &&
                !string.IsNullOrEmpty(textBox13.Text) && !string.IsNullOrWhiteSpace(textBox13.Text))
            {
             SqlCommand command = new SqlCommand
                    ("INSERT INTO [Seans] (Movie, Genre, Duration, Price, Pochatok, Description) VALUES(@Movie, @Genre, @Duration, @Price, @Pochatok, @Description)", sqlConnection);

                command.Parameters.AddWithValue("Movie", textBox15.Text);
                command.Parameters.AddWithValue("Genre", textBox12.Text);
                command.Parameters.AddWithValue("Duration", maskedTextBox1.Text);
                command.Parameters.AddWithValue("Price", textBox8.Text);
                command.Parameters.AddWithValue("Pochatok", Convert.ToDateTime(maskedTextBox2.Text));
                command.Parameters.AddWithValue("Description", textBox13.Text);

                await command.ExecuteNonQueryAsync();
            }
            else
            {
                label7.Visible = true;
                label7.Text = "Не заповнені всі поля!";
            }
        }

        private async void оновитиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox7.Clear();

            SqlDataReader sqlReader = null;

            SqlCommand command = new SqlCommand("SELECT * FROM [Seans]", sqlConnection); //визиваємо всі продукти
            try
            {
                sqlReader = await command.ExecuteReaderAsync();
                //await усюди потрібен для того щоб програма виконувалася одночасно
                while (await sqlReader.ReadAsync()) //читаємо одну строку продукту
                {
                    textBox7.Text += 
                        ("\r\nId: " + Convert.ToString(sqlReader["Id"]) +
                        "\r\nНазва: " + Convert.ToString(sqlReader["Movie"]) +
                        "\r\nЖанри: " + Convert.ToString(sqlReader["Genre"]) +
                        "\r\nТривалість: " + Convert.ToString(sqlReader["Duration"]) +
                        "\r\nЦіна: " + Convert.ToString(sqlReader["Price"]) +
                        "\r\nПочаток сеансу: " + Convert.ToString(sqlReader["Pochatok"]) +
                        "\r\n****************************************************************************");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Close();
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {//редагування в базі даних
            if (label8.Visible)
                label8.Visible = false; //якщо користувач увів все вірно

            if (!string.IsNullOrEmpty(textBox15.Text) && !string.IsNullOrWhiteSpace(textBox15.Text) &&
                !string.IsNullOrEmpty(textBox12.Text) && !string.IsNullOrWhiteSpace(textBox12.Text) &&
                !string.IsNullOrEmpty(textBox11.Text) && !string.IsNullOrWhiteSpace(textBox11.Text) &&
                !string.IsNullOrEmpty(textBox5.Text) && !string.IsNullOrWhiteSpace(textBox5.Text) &&
                !string.IsNullOrEmpty(textBox4.Text) && !string.IsNullOrWhiteSpace(textBox4.Text) &&
                !string.IsNullOrEmpty(textBox3.Text) && !string.IsNullOrWhiteSpace(textBox3.Text) &&
                !string.IsNullOrEmpty(textBox14.Text) && !string.IsNullOrWhiteSpace(textBox14.Text))
            {
                SqlCommand command = new SqlCommand
                    ("UPDATE [Seans] SET [Movie]=@Movie, [Genre]=@Genre [Duration]=@Duration, [Price]=@Price, [Pochatok]=@Pochatok, [Description]=@Description,  WHERE [Id]=@Id", sqlConnection);

                command.Parameters.AddWithValue("Id", textBox15.Text);
                command.Parameters.AddWithValue("Movie", textBox12.Text);
                command.Parameters.AddWithValue("Genre", textBox11.Text);
                command.Parameters.AddWithValue("Duration", textBox5.Text);
                command.Parameters.AddWithValue("Price", textBox4.Text);
                command.Parameters.AddWithValue("Pochatok", textBox3.Text);
                command.Parameters.AddWithValue("Description", textBox14.Text);

                await command.ExecuteNonQueryAsync();
            }
            else if (!string.IsNullOrEmpty(textBox15.Text) && !string.IsNullOrWhiteSpace(textBox15.Text))
            {
                label8.Visible = true;
                label8.Text = "Id повинен бути заповнений!";
            }
            else
            {
                label8.Visible = true;
                label8.Text = "Не заповнені всі поля!";
            }
        }

        private async void button3_Click(object sender, EventArgs e)
        {//видалення рядку в базі даних
            if (label8.Visible)
                label8.Visible = false; //якщо користувач увів все вірно

            if (!string.IsNullOrEmpty(textBox6.Text) && !string.IsNullOrWhiteSpace(textBox6.Text))
            {
                SqlCommand command = new SqlCommand("DELETE FROM [Seans] Where [Id]=@Id", sqlConnection);

                command.Parameters.AddWithValue("Id", textBox6.Text);

                await command.ExecuteNonQueryAsync();
            }
            else 
            {
                label8.Visible = true;
                label8.Text = "Id повинен бути заповнений!";
            }
        }

        private void проПрограмуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 aboutBox1 = new AboutBox1();
            aboutBox1.Show();
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void textBox15_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private async void button4_Click(object sender, EventArgs e)
        {
            {//додавання нового рядка в базу даних
                if (label7.Visible)
                    label7.Visible = false; //якщо користувач увів все вірно

                if (!string.IsNullOrEmpty(textBox1.Text) && !string.IsNullOrWhiteSpace(textBox1.Text) &&
                    !string.IsNullOrEmpty(textBox2.Text) && !string.IsNullOrWhiteSpace(textBox2.Text) &&
                    !string.IsNullOrEmpty(maskedTextBox1.Text) && !string.IsNullOrWhiteSpace(maskedTextBox1.Text) &&
                    !string.IsNullOrEmpty(textBox8.Text) && !string.IsNullOrWhiteSpace(textBox8.Text) &&
                    !string.IsNullOrEmpty(maskedTextBox2.Text) && !string.IsNullOrWhiteSpace(maskedTextBox2.Text) &&
                    !string.IsNullOrEmpty(textBox13.Text) && !string.IsNullOrWhiteSpace(textBox13.Text))
                {
                    SqlCommand command = new SqlCommand
                           ("INSERT INTO [Seans] (Movie, Genre, Duration, Price, Pochatok, Description) VALUES(@Movie, @Genre, @Duration, @Price, @Pochatok, @Description)", sqlConnection);

                    command.Parameters.AddWithValue("Movie", textBox9.Text);
                    command.Parameters.AddWithValue("Genre", textBox10.Text);
                    command.Parameters.AddWithValue("Duration", maskedTextBox1.Text);
                    command.Parameters.AddWithValue("Price", textBox16.Text);
                    command.Parameters.AddWithValue("Pochatok", Convert.ToDateTime(maskedTextBox2.Text));
                    command.Parameters.AddWithValue("Description", textBox17.Text);

                    await command.ExecuteNonQueryAsync();
                }
                else
                {
                    label7.Visible = true;
                    label7.Text = "Не заповнені всі поля!";
                }
            }
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox17_TextChanged(object sender, EventArgs e)
        {

        }
    }
    
}
