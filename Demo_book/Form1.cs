using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo_book
{
    public partial class Form1 : Form
    {
        DataBase database = new DataBase(); // объект подключения к БД

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "bookmarketDataSet.product". При необходимости она может быть перемещена или удалена.
            this.productTableAdapter.Fill(this.bookmarketDataSet.product);

            // считываем из Properties роль пользователя
            if (Properties.Settings.Default.role == 0)
            {
                label1.Text = "Вы в правах лошары";
                button1.Visible = false;
                button2.Visible = false;
                button5.Visible = false;
            }
            else if (Properties.Settings.Default.role == 1)
            {
                label1.Text = "Вы в правах админа";
            }
            else if(Properties.Settings.Default.role == 2)
            {
                label1.Text = "Вы в правах менеджер";
                button1.Visible = false;
                button2.Visible = false;       
            }
            else if(Properties.Settings.Default.role == 3)
            {
                label1.Text = "Вы в правах юзера";
                button1.Visible = false;
                button2.Visible = false;
                button5.Visible = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dPic = new OpenFileDialog();
            string FilePic = "";
            dPic.Filter = "Файлы изображений (*.jpg)|*.jpg|Все файлы (*.*)|*.*";
            if (dPic.ShowDialog() == DialogResult.OK)
            {
                FilePic = dPic.FileName;
            }
            else
            {
                FilePic = "";
                return;
            }
            pictureBox1.Image = Image.FromFile(FilePic);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.productBindingSource.EndEdit();
            this.productTableAdapter.Update(this.bookmarketDataSet);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int n = dataGridView1.CurrentCell.RowIndex;
            int id_product = Convert.ToInt32(dataGridView1.Rows[n].Cells[0].Value);

            DateTime date = DateTime.Now.Date;
            Random r = new Random();
            int kod = r.Next(100, 999);

            database.openDataBase();
            SqlCommand myComm = database.getConnection().CreateCommand();
            if (button4.Visible == false)
            {
                myComm.CommandText = "INSERT INTO orders (client, date_order, summa, kod) VALUES ('', @date, 0, @kod)";

                myComm.Parameters.Add("@date", SqlDbType.Date);
                myComm.Parameters["@date"].Value = date;


                myComm.Parameters.Add("@kod", SqlDbType.Int, 4);
                myComm.Parameters["@kod"].Value = kod;

                myComm.ExecuteNonQuery();

                myComm.CommandText = "SELECT MAX(id_order) FROM orders";
                int nom_order = Convert.ToInt32(myComm.ExecuteScalar());
                Properties.Settings.Default.nom_order = nom_order;

                button4.Visible = true;
            }

            int id_order = Properties.Settings.Default.nom_order;
            double price = Convert.ToDouble(textBox5.Text);
            double discount = Convert.ToDouble(textBox6.Text);
            

            myComm.CommandText = "INSERT INTO items (id_order, id_product, quantity, price, discount) VALUES (@id_order, @id_product, 1, @price, @discount)";

            myComm.Parameters.Add("@id_order", SqlDbType.Int, 4);
            myComm.Parameters["@id_order"].Value = id_order;

            myComm.Parameters.Add("@id_product", SqlDbType.Int, 4);
            myComm.Parameters["@id_product"].Value = id_product;

            myComm.Parameters.Add("@price", SqlDbType.Money, 8);
            myComm.Parameters["@price"].Value = price;

            myComm.Parameters.Add("@discount", SqlDbType.Float, 8);
            myComm.Parameters["@discount"].Value = discount;

  

            myComm.ExecuteNonQuery();

            database.closeDataBase();

           
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Orders f = new Orders();
            f.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Orders f = new Orders();
            f.ShowDialog();
        }
    }
}
