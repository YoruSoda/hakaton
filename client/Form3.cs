using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using library;

namespace client
{
    public partial class Form3 : Form
    {
        Socket tcpSocket;
        List<tovar> tovars = new List<tovar>();//создали список для товаров
        public Form3(Socket tcpSocket)
        {
            InitializeComponent();
            this.tcpSocket = tcpSocket;
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            SendRec.SendInt(tcpSocket, 1); //отправляем 1 на сервер
            int lenghtTovars = SendRec.ReceiveInt(tcpSocket); //получили длину списка товаров
            byte[] bytes = new byte[lenghtTovars];
            tcpSocket.Receive(bytes); //получаем в этот массив товар
            tovars = JsonSerializer.Deserialize<List<tovar>>(bytes); //десереализуем
           // checkedListBox1.DisplayMember = "tovarName"; //настраиваем листбокс так, чтобы отображалось только имя товара
            foreach (tovar t in tovars)
            {
               // checkedListBox1.Items.Add(t); //добавляем товары в листбокс
            }

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            tovars.Clear(); //очищаем список товаров
            textBox1.Text = ""; //очищаем текст в текстбоксе, чтобы при нажатии на кнопку текст не добавлялся, а заполнялся заново
            SendRec.SendInt(tcpSocket, 1); //отправляем серверу число 1 и ждем список товаров
            int lenghtTovars = SendRec.ReceiveInt(tcpSocket); //получили длину списка товаров
            byte[] bytes = new byte[lenghtTovars];
            tcpSocket.Receive(bytes);
            tovars = JsonSerializer.Deserialize<List<tovar>>(bytes);

            foreach (var t in tovars) //для каждого товара из списка печатаем его в текстбоксе
            {
                textBox1.Text += $"Товар: {t.tovarName}  Цена: {t.tovarPrice} рублей \r\n"; //\r\n - символ перевода строки
            }
            //SendRec.SendInt(tcpSocket, 5); //отправляем серверу 4

            //List<string> fraudulentReceivers = new List<string>();
            //fraudulentReceivers.Add("fraudster1");
            //fraudulentReceivers.Add("fraudster2");

            ////Transaction t1 = new Transaction(1, DateTime.Now.Date, "12:00", "sender1", "device1", 100.00m, "receiver1", "device2");
            ////Transaction t2 = new Transaction(2, DateTime.Now.Date, "13:00", "sender2", "device1", 50.00m, "receiver2", "device3");
            ////Transaction t3 = new Transaction(3, DateTime.Now.Date, "14:00", "sender3", "device2", 200.00m, "fraudster1", "device4");
            ////Transaction t4 = new Transaction(4, DateTime.Now.Date, "15:00", "sender4", "device3", 500.00m, "receiver3", "device3");

            //order ord = new order();//создаем объект класса заказ

            //ord.OrderPrice = Convert.ToInt32(label2.Text); //цена заказа = то что написано в label

            //foreach (tovar i in checkedListBox1.CheckedItems) //для каждого выбранного товара
            //{
            //    ord.NamesTovars.Add(i.tovarName);  //добавляем имя товара в список товаров заказа
            //}

            ////tcpSocket.Send(t3.SerialTransation());//сериализуем и отправляем

            //label3.Text = "Заказ был создан";


        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {   
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e) //для выбранных товаров рассчитываем сумму
        {
            int summa = 0; //отвечает за стоимость заказа
           // foreach (tovar i in checkedListBox1.CheckedItems) //для каждого выбранного товара в checkedListBox
            {
               // summa += i.tovarPrice; //суммируем стоимость каждого выбранного товара
            }
            label2.Text = summa.ToString(); //печатаем на формочке
        }
    }
}
