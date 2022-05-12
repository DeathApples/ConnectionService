using System.IO;
using System.Net;
using System.Text;
using System.Net.Sockets;
using System.ServiceProcess;
using System.Threading.Tasks;

using System.Threading;

namespace Server
{
    public partial class Service1 : ServiceBase
    {
        private int size;
        private bool isExit;
        private Socket socket;
        private IPEndPoint endPoint;

        private const int port = 52000;
        private const string ip = "127.0.0.1";
        private byte[] data = new byte[1024];
        private StringBuilder name = new StringBuilder();

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            InitializeConnection();

            Task.Run(() => Listening());
        }

        protected override void OnStop()
        {
            isExit = true;
            socket.Close();
        }

        private void InitializeConnection()
        {
            isExit = false;
            endPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            socket.Bind(endPoint);
            socket.Listen(5);
        }

        private void Listening()
        {
            while (!isExit)
            {
                var listener = socket.Accept();

                listener.Receive(data);

                name.Append(Encoding.UTF8.GetString(data, 1, data[0]));

                using (FileStream fstream = new FileStream($"C:\\Users\\User\\Desktop\\{name}", FileMode.OpenOrCreate))
                {
                    do
                    {
                        size = listener.Receive(data);
                        fstream.Write(data, 0, size);
                        Thread.Sleep(1);

                    } while (listener.Available > 0);
                }

                name.Clear();

                listener.Shutdown(SocketShutdown.Both);
                listener.Close();
            }
        }
    }
}
