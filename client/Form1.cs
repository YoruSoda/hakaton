using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text.Json;
using library;

namespace client
{
    public partial class Form1 : Form
    {
        const string ip = "127.0.0.1";
        const int port = 30000;
        Socket tcpSocket;
        List<transaction> transactions = new List<transaction>();//создали список для товаров
        public Form1(Socket tcpSocket)
        {
            InitializeComponent();
            this.tcpSocket = tcpSocket;


            int lenghtTovars = SendRec.ReceiveInt(tcpSocket); //получили длину списка товаров
            byte[] bytes = new byte[lenghtTovars];
            tcpSocket.Receive(bytes);
            transactions = JsonSerializer.Deserialize<List<transaction>>(bytes);

            foreach (var t in transactions) //для каждого товара из списка печатаем его в текстбоксе
            {
                textBox1.Text += $"Fraud transaction: {t.ID}  Fraud Card: {t.Receiver} \r\n";
            }
        }

        private void button1_Click(object sender, EventArgs e) //событие нажатия на кнопку "показать товары"
        {
            

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            
            
        }

        private void button2_Click(object sender, EventArgs e) //событие нажатия на кнопку "показать заказы"
        {
           

        }

        private void button3_Click(object sender, EventArgs e) //событие нажатия на кнопку "создать товар"
        {
            //Form f2 = new Form2(tcpSocket); //создаем новую форму
            //Form f3 = new Form3(tcpSocket);//создаем новую форму
            //f2.Show();//открываем её
            //f3.Show();//открываем её
        }

        private void button4_Click(object sender, EventArgs e) //событие нажатия на кнопку "создать заказ"
        {
            //Form f2 = new Form3(tcpSocket);//создаем новую форму
            //f2.ShowDialog();//открываем её
        }
    }
}