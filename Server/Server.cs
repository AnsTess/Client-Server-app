using System.Net;
using System.Net.Sockets;
using System.Text;

class Server
{
    static void Main()
    {
        // IP-адрес и порт для сервера
        IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
        int port = 1111;

        // Создание объекта TcpListener для прослушивания входящих подключений
        TcpListener server = new TcpListener(ipAddress, port);

        try
        {
            // Запуск прослушивания
            server.Start();
            Console.Write("Server is running. Waiting for client...\n");

            while (true)
            {
                // Принятие входящего подключения
                TcpClient client = server.AcceptTcpClient();
                Console.Write("Client connected\n");

                // Поток для чтения и записи данных
                NetworkStream stream = client.GetStream();

                // Буфер для хранения полученных данных
                byte[] messageBuffer = new byte[1024];  

                while (true)
                {
                    // Чтение данных из потока
                    int bytesRead = stream.Read(messageBuffer, 0, messageBuffer.Length);

                    // Преобразование полученных данных в строку
                    string message = Encoding.UTF8.GetString(messageBuffer, 0, bytesRead);
                    Console.Write("Message received: " + message + "\n\n");

                    // Отправка ответа клиенту
                    byte[] reply = Encoding.UTF8.GetBytes("Message received");
                    stream.Write(reply, 0, reply.Length);

                    // Проверка условия завершения соединения
                    if (message.ToLower() == "exit")
                    {
                        break;
                    }
                }

                // Закрытие подключения
                client.Close();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            // Остановка сервера
            server.Stop();
        }
    }
}
