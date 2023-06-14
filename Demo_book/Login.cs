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
    public partial class Login : Form
    {
        DataBase database = new DataBase(); // объект подключения к БД

        public Login()
        {
            InitializeComponent();
        }


        // функции принимает в параметрах логин и пароль пользователя с формы
        bool fun_login(string login, string pass)
        {
            // создаем подключение к БД
            database.openDataBase();
            // создаем объект для выполнения запроса
            SqlCommand myComm = database.getConnection().CreateCommand();
            // запрос выборки данных из таблицы по указанному логину
            myComm.CommandText = "SELECT * FROM users WHERE user_login = @login";
            // создаем параметр в который будем передавать логин
            myComm.Parameters.Add("@login", SqlDbType.NVarChar, 50);
            myComm.Parameters["@login"].Value = login;
            // выполняем запрос
            SqlDataReader myData = myComm.ExecuteReader();
            // считываем запись из myData (она должна быть всего одна)
            if (myData.Read() == false) // если записей не выбрано
            {
                // то такого логина не существует
                label1.Text = "Ошибка в логине";
                return false;  // возвращаем неудачу 
            }
            // получаем из записи myData пароль
            string pass_bd = myData["pass"].ToString();
            // если пароль пользователя и пароль из БД не совпали
            if (pass_bd != pass)
            {
                label3.Text = "Ошибка в пароле";  // ошибка в пароле
                return false;  // возвращем неудачу
            }
            // если код дошел сюда, то авторизация успешна

            // получаем из результатов запроса роль юзера
            // 1 - админ
            // 2 - менеджер
            // 3 - юзер
            int role = Convert.ToInt32(myData["user_role"]);
            // заносим в настройки проекта 
            Properties.Settings.Default.role = role;

            myData.Close();  // закрывваем данные запроса
            database.closeDataBase();  // разрываем соединение

            return true;  // возвращаем признак успешной авторизации
        }


        // Нажатие кнопки "Вход"
        private void button1_Click(object sender, EventArgs e)
        {
            string login = textBox1.Text;  // считываем из формы логин 
            string pass = textBox2.Text;   // считываем из формы пароль    
            bool at = fun_login(login, pass);  // вызываем функцию проверки 
                                               // успешности авторизации
            if (at == true)   // если авторизация прошла успешно
            {
                Form1 f = new Form1();  // создаем и 
                f.Show();     // показываем главную форму
                this.Hide();  // убираем форму авторизации
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 f = new Form1();  // создаем и 
            f.Show();     // показываем главную форму
            this.Hide();  // убираем форму авторизации
        }
    }
}
