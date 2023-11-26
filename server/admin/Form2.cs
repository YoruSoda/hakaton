using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using library;
using System.Net.Sockets;
using System.Net;

namespace admin
{
    public partial class Form2 : Form
    {
        const string ip = "127.0.0.1";
        const int port = 30000;
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Socket tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); //создаем сокет клиента
            EndPoint tcpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port); //создаем точку подключения
            tcpSocket.Connect(tcpEndPoint); //подключаемся к серверу
            byte[] login = Encoding.Unicode.GetBytes(textBox1.Text); //сериализовали логин
            byte[] logLength = BitConverter.GetBytes(login.Length); //сериализовали длину логина
            tcpSocket.Send(logLength); //отправляем сначала длину
            tcpSocket.Send(login);//потом сам логин

            byte[] password = Encoding.Unicode.GetBytes(textBox2.Text); //сериализовали пароль
            byte[] pasLength = BitConverter.GetBytes(password.Length); //серализовали длину пароля
            tcpSocket.Send(pasLength); //отправляем сначала длину
            tcpSocket.Send(password);//потом сам пароль

            int answer = SendRec.ReceiveInt(tcpSocket); //получили ответ от клиента
            if (answer == 1) //если ответ 1
            {

                Form f1 = new Form1(tcpSocket);
                this.Visible = false;  //форму регистрации делаем невидимой
                f1.Show(); //открываем форму предприятия

            }
            else
            {
                // label3.Text = "Неверный логин или пароль";
            }
        }
    }
}
