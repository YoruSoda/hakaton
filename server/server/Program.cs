using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using library;
using System.Globalization;
using System.Text.Json;
using System.Threading;

namespace server
{
    internal class Program
    {

        const string ip = "127.0.0.1";
        const int port = 30000;
        static Socket tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); //создание сокета
        static List<transaction> transactionList = new List<transaction>(); //список для заказов
        static List<string> badGuys = new List<string>();
        static List<transaction> allTransaction = new List<transaction>();
        static string passwd = "test"; //здесь храним пароль
        static string log = "test"; //здесь храним логин
        static void Main(string[] args)
        {
            EndPoint tcpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port); //создаем точку подключения
            badGuys.Add("1111111111111111");
            badGuys.Add("2222222222222222");
            badGuys.Add("3333333333333333");
            badGuys.Add("4444444444444444");
            badGuys.Add("5555555555555555");
            badGuys.Add("6666666666666666");
            badGuys.Add("7777777777777777");
            badGuys.Add("8888888888888888");
            badGuys.Add("9999999999999999");
            badGuys.Add("0425854939370604");
            badGuys.Add("8999460632268445");


            tcpSocket.Bind(tcpEndPoint);  //назначили сетевой адрес серверному сокету
            tcpSocket.Listen(1000); // перешли в режим прослушки

            while (true) // цикл, с помощью которого осуществляется возможность подключения нескольких клиентов к одному серверу
            {

                Socket client = tcpSocket.Accept(); //подключаемся к клиенту
                Thread t = new Thread(Func); // создали поток в бесконечном цикле 
                t.Start(client);


            }
        }

        static public void Func(object c) //метод для обработки клиент
        {
            Socket client = (Socket)c;// приводим тип из object в Socket
            try //заворачиваем цикл в try, чтобы не появлялась ошибка, если один из клиентов закроется
            {
                int answ = 0;
                string recLogin = SendRec.ReceiveString(client);//получаем и десереализуем логин
                if (recLogin.Contains(log)) //если полученный логин совпадает с тем, что лежит на сервере...
                {
                    Console.WriteLine("Логины совпали, проверка паролей");
                    string recPass = SendRec.ReceiveString(client); //получаем десереализуем пароль
                    if (recPass.Contains(passwd)) //если полученный пароль совпал с паролем на сервере..
                    {
                        answ = 1; //ответ для клиента равен 1
                        Console.WriteLine("Пароли совпали, отправляем ответ");
                    }
                    else //если не совпал
                    {
                        answ = 0; //ответ для клиента равен 0
                        Console.WriteLine("Пароли не совпадают");

                    }
                }
                else //если логины не совпали
                {
                    Console.WriteLine("Логины не совпали");
                    answ = 0; //ответ для клиента равен 0

                }
                SendRec.SendInt(client, answ); //сериализуем ответ сервера и отправляем клиенту

                if (answ == 0) //если ответ равен 0
                {
                    client.Close(); //то закрываем сокеты
                    return;
                }

                while (true)
                {
                    int number = SendRec.ReceiveInt(client);//получили число, отвечающее за то, какая кнопка была нажата
                    switch (number)
                    {
                        case 5:
                            int trnlenght = SendRec.ReceiveInt(client);//получаем длину 
                            byte[] byteTRN = new byte[trnlenght]; //создаем массив размером с полученную длину-4, чтобы не пришло лишнее
                            client.Receive(byteTRN);//получаем в этот массив 
                            transaction trn = transaction.DeserializationTransactionr(byteTRN);
                            allTransaction.Add(trn);
                            if (trn.IsBadGuy(badGuys) == true)
                            {
                                transactionList.Add(trn);
                                Console.WriteLine("Обнаружена подозрительная транзакция");

                            };

                            break;
                        case 6:

                            byte[] transactionslenght = BitConverter.GetBytes(transactionList.Count);
                            if (transactionList.Count > 0)
                            {
                                client.Send(transactionslenght); //отправляем клиенту длину списка 
                                foreach (transaction i in transactionList) //для каждого из списка
                                {
                                    client.Send(i.SerializationTransaction()); //...сериализуем и отправляем клиенту
                                }


                            }

                            byte[] numb = BitConverter.GetBytes(0);

                            if (transactionList.Count == 0)
                            {
                                
                                    SendRec.SendInt(client, 0); //...сериализуем и отправляем клиенту
                                


                            }


                            //byte[] transactions = JsonSerializer.SerializeToUtf8Bytes(transactionList);//сериализуем длину списка транзакция
                            //client.Send(transactionslenght); //отправляем клиенту длину списка транзакция
                            //client.Send(transactions);//потом сам список транзакция


                            break;
                    }
                }
            }
            catch
            { };

        }
    }
}