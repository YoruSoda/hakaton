using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace library
{
    public class order // класс заказ
    {

        public order() //создаем конструктор
        {
            NamesTovars = new List<string>();

        }

        public List<string> NamesTovars { get; set; }  //свойство - список имен товаров в заказе
        public int OrderPrice { get; set; } // свойство - стоимость заказа

        public byte[] SerialOrder() //метод сериализации заказа
        {
            byte[] serBytes = JsonSerializer.SerializeToUtf8Bytes(this); //сериализуем весь класс заказ
            byte[] serLength = BitConverter.GetBytes(serBytes.Length); //сериализуем длину получившегося массива
            serLength = serLength.Concat(serBytes).ToArray(); //объединяем их
            return serLength; //возвращаем общий массив
        }

        public void DeserialOrder(byte[] a) //метод десереализации заказа
        {
            order ord = JsonSerializer.Deserialize<order>(a); //десереализуем все сразу в заказ
            this.NamesTovars = ord.NamesTovars;
            this.OrderPrice = ord.OrderPrice;


        }
    }

    public class tovar // класс товар 
    {
        public tovar(string tovarName, int tovarPrice) //создаем класс для хранения названия и цены товара
        {
            this.tovarName = tovarName; //имя товара
            this.tovarPrice = tovarPrice; //цена товара
        }

        public string tovarName { get; set; } //свойство
        public int tovarPrice { get; set; } // свойство

        public byte[] SerializationTovar() //класс сериализации товара
        {
            byte[] serialBytes = JsonSerializer.SerializeToUtf8Bytes(this); //сериализовали все поля из класса товар
            byte[] seriaLength = BitConverter.GetBytes(serialBytes.Length); //сериализуем длину полученного массива
            seriaLength = seriaLength.Concat(serialBytes).ToArray(); //склеиваем
            return seriaLength;//возвращаем полученный массив

        }

        public static tovar DeserializationTovar(byte[] a) //класс десериализации товара
        {
            tovar tovarNew = JsonSerializer.Deserialize<tovar>(a);//десерализуем все сразу
            return tovarNew;

        }
    }

    public  class SendRec // класс, в котором функции для отправки/получения сообщений
    {
        public static  string ReceiveString(Socket tcpsocket) //метод получения строки
        {
            int l = ReceiveInt(tcpsocket); //получаем длину строки
            byte[] buffer = new byte[l]; //создаем массив байт размером с длину строки
            tcpsocket.Receive(buffer); //получаем строку в этот массив 
            return Encoding.Unicode.GetString(buffer); //десерализуем полученную строку
        }
        public static int ReceiveInt(Socket tcpsocket) //метод получения числа
        {
            byte[] length = new byte[4]; //десериализуем длину числа
            tcpsocket.Receive(length); //получаем число
            return BitConverter.ToInt32(length, 0); //десереализуем само число
        }

        public static  void SendInt(Socket tcpsocket, int x) //метод отправки числа
        {
            byte[] number = BitConverter.GetBytes(x); //сериализуем число
            tcpsocket.Send(number);//отправляем число
        }
    }

       

        
    public class transaction // класс транзакции 
    {
        public transaction(int Id, DateTime date, DateTime time, string sender, string senderDevice, decimal amount, string receiver, string receiverDevice)
        {
            this.ID = Id;
            this.Date = date;
            this.Time = time;
            this.Sender = sender;
            this.SenderDevice = senderDevice;
            this.Amount = amount;
            this.Receiver = receiver;
            this.ReceiverDevice = receiverDevice;
        }

        public int ID { get; set; }
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }
        public string Sender { get; set; }
        public string SenderDevice { get; set; }
        public decimal Amount { get; set; }
        public string Receiver { get; set; }
        public string ReceiverDevice { get; set; }

       

        public bool IsFraudulent(List<string> fraudulentReceivers)
        {
            if (SenderDevice == ReceiverDevice)
            {
                Console.WriteLine("Подозрительная транзакция:" + ID + " Устройства получателя и отправителя совпадают.");
                return true;
            }

            if (fraudulentReceivers.Contains(Receiver))
            {
                Console.WriteLine("Подозрительная транзакция:" + ID + " Получатель есть в БД злоумышленников");
                return true;
            }

            return false;
        }
        public byte[] SerializationTransaction() //класс сериализации транзакции
        {
            byte[] serialBytes = JsonSerializer.SerializeToUtf8Bytes(this); //сериализовали все поля из класса транзакции
            byte[] seriaLength = BitConverter.GetBytes(serialBytes.Length); //сериализуем длину полученного массива
            seriaLength = seriaLength.Concat(serialBytes).ToArray(); //склеиваем
            return seriaLength;//возвращаем полученный массив
        }

        public static transaction DeserializationTransactionr(byte[] a) //класс десериализации транзакции
        {
            transaction newTransaction = JsonSerializer.Deserialize<transaction>(a);//десерализуем все сразу
            return newTransaction;

        }
    }
}
