using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using library;

namespace client
{
    public partial class Form1 : Form
    {
        const string ip = "127.0.0.1";
        const int port = 30000;
        Socket tcpSocket;

        public Form1(Socket tcpSocket)
        {
            InitializeComponent();
            this.tcpSocket = tcpSocket;

        }
        static int id = 0;

        private void button1_Click(object sender, EventArgs e)
        {
            SendRec.SendInt(tcpSocket, 5); //отправляем цифру 5
            DateTime time = DateTime.Now;
            transaction newTransaction = new transaction(id, time.Date, time, textBox1.Text, "device2", Convert.ToDecimal(textBox3.Text), textBox2.Text, "device4"); //создаем транзакцию из тех данных,которые вводит клиент
            id++;
            tcpSocket.Send(newTransaction.SerializationTransaction()); //сериализуем и отправляем
        }
    }
}
