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
using System.Globalization;
using System.Text.Json;
using System.Threading;
using System.Net.Sockets;

namespace admin
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

            //textBox1.Text = ""; //очищаем текст в текстбоксе, чтобы при нажатии на кнопку текст не добавлялся, а заполнялся заново
            //SendRec.SendInt(tcpSocket, 1); //отправляем серверу число 1 и ждем список товаров
            // int lenghtTovars = SendRec.ReceiveInt(tcpSocket); //получили длину списка товаров
            //for (int i = 0; i < lenghtTovars; i++) //для каждого товара из списка
            //{
            // int lenght = SendRec.ReceiveInt(tcpSocket); //получаем длину товара
            //byte[] bytes = new byte[lenght]; //создаем массив размером с эту длину
            // tcpSocket.Receive(bytes); //получаем в этот массив сам товар
            //transaction newT = transaction.DeserializationTransactionr(bytes);//десереализуем и добавляем в список товаров
            //List<transaction> transactionList = new List<transaction>(); //список для заказов
            
            //while (true)
            //{
            //    SendRec.SendInt(tcpSocket, 6); //отправляем серверу число 1 и ждем список 
            //    int lenghtTransactions = SendRec.ReceiveInt(tcpSocket); //получили длину списка 
            //    if (lenghtTransactions != 1)
            //    {
            //        for (int i = 0; i < lenghtTransactions; i++) //для кажд
            //        {
            //            int lenght = SendRec.ReceiveInt(tcpSocket); //получаем длину 
            //            byte[] bytes = new byte[lenght]; //создаем массив размером с эту длину
            //            tcpSocket.Receive(bytes); //получаем в этот массив сам 
            //            transactionList.Add(transaction.DeserializationTransactionr(bytes)); //десереализуем и добавляем в список 

            //        }
            //        foreach (var t in transactionList) //для каждого товара из списка печатаем его в текстбоксе
            //        {
            //            textBox1.Text += $"Fraud transaction ID: {t.ID}  Receiver: {t.Receiver}  \r\n";
            //        }
            //    }

            //}
            


                //textBox1.Text += $"Подозрительная транзакция: {newT.ID}  айди: {newT.Receiver} \r\n"; //\r\n - символ перевода строки
                // }

            


        }

            private void button1_Click(object sender, EventArgs e)
            {
            textBox1.Clear();
            List<transaction> transactionList = new List<transaction>(); //список для заказов

            SendRec.SendInt(tcpSocket, 6); //отправляем серверу число 1 и ждем список 
            int lenghtTransactions = SendRec.ReceiveInt(tcpSocket); //получили длину списка 
            for (int i = 0; i < lenghtTransactions; i++) //для кажд
            {
                int lenght = SendRec.ReceiveInt(tcpSocket); //получаем длину 
                byte[] bytes = new byte[lenght]; //создаем массив размером с эту длину
                tcpSocket.Receive(bytes); //получаем в этот массив сам 
                transactionList.Add(transaction.DeserializationTransactionr(bytes)); //десереализуем и добавляем в список 

            }
            foreach (var t in transactionList) //для каждого товара из списка печатаем его в текстбоксе
            {
                textBox1.Text += $"Подозрительная транзакция: {t.ID}  Номер карты получателя: {t.Receiver} \r\n" ; 
            }
        }
        }
    }

        
    
