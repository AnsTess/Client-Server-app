using System.Net.Sockets;
using System.Text;

class Client
{
    static void Main()
    {
        // IP-адрес и порт сервера
        string serverIP = "127.0.0.1";
        int serverPort = 1111;

        try
        {
            // Создание объекта TcpClient для подключения к серверу
            TcpClient client = new TcpClient(serverIP, serverPort);
            Console.Write("Connected to server.\n");

            // Поток для чтения и записи данных
            NetworkStream stream = client.GetStream();

            while (true)
            {
                // Ввод сообщения
                Console.Write("Enter a message (to close connection, enter 'exit'): ");
                string message = Console.ReadLine();

                // Отправка сообщения серверу
                byte[] messageBuffer = Encoding.UTF8.GetBytes(message);
                stream.Write(messageBuffer, 0, messageBuffer.Length);
                Console.Write("Message sent.\n");

                // Буфер для хранения полученного ответа
                messageBuffer = new byte[1024];

                // Получение ответа от сервера
                int bytesRead = stream.Read(messageBuffer, 0, messageBuffer.Length);
                string response = Encoding.UTF8.GetString(messageBuffer, 0, bytesRead);
                Console.Write("Server reply: " + response + "\n");

                // Проверка условия завершения соединения
                if (message.ToLower() == "exit")
                {
                    break;
                }
            }

            // Закрываем подключение
            client.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
