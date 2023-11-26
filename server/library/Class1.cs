using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Text.Json;

namespace library
{
   

   
     

    public class SendRec // класс, в котором функции для отправки/получения сообщений
    {
        public static string ReceiveString(Socket tcpsocket) //метод получения строки
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

        public static void SendInt(Socket tcpsocket, int x) //метод отправки числа
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



        public bool IsBadGuy(List<string> badGuys)
        {
            if (SenderDevice == ReceiverDevice)
            {
                Console.WriteLine("Подозрительная транзакция:" + ID + " Устройства получателя и отправителя совпадают.");
                return true;
            }

            if (badGuys.Contains(Receiver))
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
