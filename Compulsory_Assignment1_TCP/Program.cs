using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading.Tasks;
using Compulsory_Assignment1_Model_Book;

namespace Compulsory_Assignment1_TCP
{
    public class Program
    {

        static void Main(string[] args)
        {

            TcpListener listener = new TcpListener(System.Net.IPAddress.Loopback, 4646);
            listener.Start();
            Console.WriteLine("Server started");


            while (true)
            {
                TcpClient socket = listener.AcceptTcpClient();
                Task.Run(() =>
                {
                    HandleClient(socket);
                });
            }
        }


        public static void HandleClient(TcpClient socket)
        {

            NetworkStream ns = socket.GetStream();
            StreamReader reader = new StreamReader(ns);
            StreamWriter writer = new StreamWriter(ns);
            BookRepo repo = new BookRepo();
            Console.WriteLine("Client connected");

            string message = reader.ReadLine();
            Console.WriteLine("Client wrote " + message);
            writer.WriteLine(message);
            writer.Flush();
            socket.Close();


            while (true)
            {
                string command = reader.ReadLine();
                string parameter = reader.ReadLine();

                if (command == "GetAll" && string.IsNullOrEmpty(parameter))
                {
                    List<Book> books = repo.GetAll();

                    var options = new JsonSerializerOptions { WriteIndented = true };
                    string jsonstring = JsonSerializer.Serialize(books, options);

                    writer.WriteLine(jsonstring);
                    writer.Flush();
                }
                else if (command == "Get" && BookRepo.bookList.Any(b => b.ISBN13 == parameter))
                {
                    Book fb = repo.Get(parameter);

                    var options = new JsonSerializerOptions { WriteIndented = true };
                    string jsonstring = JsonSerializer.Serialize(fb, options);

                    writer.WriteLine(jsonstring);
                    writer.Flush();
                }
                else if (command == "Save" && !string.IsNullOrEmpty(parameter))
                {
                    try
                    {
                        Book b = JsonSerializer.Deserialize<Book>(parameter);
                        repo.Add(b);

                        writer.WriteLine("Book was added");
                        writer.Flush();

                    }

                    catch(Exception e)
                    {
                        writer.WriteLine(e.Message);
                        writer.WriteLine("Invalid json");
                        writer.Flush();
                    }
                   
                }

                else
                {
                    writer.WriteLine("Invalid command, current commands are GetAll, Get, Save");
                    writer.Flush();
                }

            }

        }

    }
}
