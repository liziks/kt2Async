using System;
using System.IO.Pipes;
using System.Text;
using System.Threading.Tasks;

namespace SenderApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            NamedPipeServerStream pipeServer = new NamedPipeServerStream("MyPipe", PipeDirection.Out);

            try
            {
                Console.WriteLine("Процесс-отправитель запущен. Ожидание подключения...");

                await pipeServer.WaitForConnectionAsync();

                Console.WriteLine("Процесс-получатель подключен. Начинаем отправку сообщений.");

                while (true)
                {
                    Console.Write("Введите сообщение для отправки: ");
                    string message = Console.ReadLine();

                    if (string.IsNullOrEmpty(message))
                    {
                        Console.WriteLine("Пустое сообщение. Попробуйте снова.");
                        continue;
                    }

                    byte[] buffer = Encoding.UTF8.GetBytes(message + "\n");
                    await pipeServer.WriteAsync(buffer, 0, buffer.Length);
                    Console.WriteLine("Сообщение отправлено.");

                    await Task.Delay(100);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            finally
            {
                pipeServer.Close();
            }
        }
    }
}