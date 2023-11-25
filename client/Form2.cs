using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using library;

namespace client
{
    public partial class Form2 : Form
    {
        Socket tcpSocket;
        public Form2(Socket tcpSocket)
        {
            InitializeComponent();
            this .tcpSocket = tcpSocket;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SendRec.SendInt(tcpSocket, 5); //отправляем цифру 5
            DateTime time = DateTime.Now;
            transaction newTransaction = new transaction(3, time.Date, time, textBox1.Text, "device2", Convert.ToDecimal(textBox3.Text), textBox2.Text, "device4"); //создаем транзакцию из тех данных,которые вводит клиент
            tcpSocket.Send(newTransaction.SerializationTransaction()); //сериализуем и отправляем
            


        }
    }

    
}
