using Aspose.Pdf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo_book
{
    public partial class Orders : Form
    {
        DataBase database = new DataBase(); // объект подключения к БД

        public Orders()
        {
            InitializeComponent();
        }

        public void calc_sum()
        {
            double sum = 0;
            for (int i = 0; i < dataGridView2.RowCount; i++)
            {
                sum = sum + Convert.ToInt32(dataGridView2.Rows[i].Cells[2].Value) *
                    Convert.ToDouble(dataGridView2.Rows[i].Cells[3].Value);
            }
            textBox2.Text = sum.ToString();
        }

        public void calc_discount()
        {
            double dis = 0;
            for (int i = 0; i < dataGridView2.RowCount; i++)
            {
                dis = dis + (Convert.ToInt32(dataGridView2.Rows[i].Cells[2].Value) *
                    Convert.ToDouble(dataGridView2.Rows[i].Cells[3].Value)) *
                    (Convert.ToDouble(dataGridView2.Rows[i].Cells[4].Value) / 100);
            }
            textBox8.Text = dis.ToString();
        }

        private void Orders_Load(object sender, EventArgs e)
        {

            // TODO: данная строка кода позволяет загрузить данные в таблицу "bookmarketDataSet.items". При необходимости она может быть перемещена или удалена.
            this.itemsTableAdapter.Fill(this.bookmarketDataSet.items);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "bookmarketDataSet.orders". При необходимости она может быть перемещена или удалена.
            this.ordersTableAdapter.Fill(this.bookmarketDataSet.orders);

            int id_order = Properties.Settings.Default.nom_order;

            if (id_order != 0)
            {
                ordersBindingSource.Filter = $"id_order = {id_order}";
            }
            calc_sum();
            calc_discount();


        }

        private void dataGridView2_CurrentCellChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView2_SelectionChanged(object sender, EventArgs e)
        {
      
                int n = dataGridView2.CurrentCell.RowIndex;
                int id_product = Convert.ToInt32(dataGridView2.Rows[n].Cells[1].Value);


                database.openDataBase();
                SqlCommand myComm = database.getConnection().CreateCommand();

                myComm.CommandText = "SELECT * FROM product WHERE id_prod = @id_product";
                myComm.Parameters.Add("@id_product", SqlDbType.Int, 4);
                myComm.Parameters["@id_product"].Value = id_product;

                // выполняем запрос
                SqlDataReader myData = myComm.ExecuteReader();

                while (myData.Read()) // читаем из запроса
                {
                    textBox3.Text = myData[1].ToString();
                    textBox4.Text = myData[2].ToString();
                    textBox5.Text = myData[3].ToString();
         
                    byte[] byteArray = (byte[])myData[6];

                    MemoryStream ms = new MemoryStream(byteArray);
                    System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
                    pictureBox1.Image = img;
                    ms.Close();
                }

                database.closeDataBase();
        
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Validate();
            ordersBindingSource.EndEdit();
            fKitemsorderBindingSource.EndEdit();
            ordersTableAdapter.Update(this.bookmarketDataSet);
            itemsTableAdapter.Update(this.bookmarketDataSet);
        }

        //формирование pdf 
        private void button2_Click(object sender, EventArgs e)
        {
            // Инициализировать объект документа
            Document document = new Document();

            // Добавить страницу
            Page page = document.Pages.Add();

            // Добавить текст на новую страницу
            page.Paragraphs.Add(new Aspose.Pdf.Text.TextFragment($"Номер заказа: {textBox11.Text}"));
            page.Paragraphs.Add(new Aspose.Pdf.Text.TextFragment($"Дата заказа: {textBox12.Text}"));
            page.Paragraphs.Add(new Aspose.Pdf.Text.TextFragment($"Сумма заказа: {textBox2.Text} руб."));
            page.Paragraphs.Add(new Aspose.Pdf.Text.TextFragment($"Сумма скидки: {textBox8.Text} руб."));
            page.Paragraphs.Add(new Aspose.Pdf.Text.TextFragment($"Пункт выдачи: {textBox10.Text}"));
            page.Paragraphs.Add(new Aspose.Pdf.Text.TextFragment($"Код получения: {textBox9.Text}"));
            
            
            // Сохранить PDF 
            document.Save("document.pdf");

            System.Diagnostics.Process.Start("document.pdf");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int n = dataGridView2.CurrentCell.RowIndex;
            dataGridView2.Rows.RemoveAt(n);
            itemsTableAdapter.Update(bookmarketDataSet);
        }

        private void dataGridView2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            calc_sum();
            calc_discount();
        }
    }
}
