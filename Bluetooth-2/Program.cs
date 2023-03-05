
using InTheHand.Net;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Devices.Enumeration;
using Windows.Media.Protection.PlayReady;

namespace Bluetooth_2
{
    internal class Program
    {
        static BluetoothClient _Client;
        static void Main(string[] args)
        {
            _Client = new BluetoothClient();

            Console.WriteLine("Hello, World!");

            // M7
            var phones = _Client.DiscoverDevices();
            var phone = phones.FirstOrDefault();

            //String deviceAddr = "00:00:00:00:00:00";
            BluetoothAddress addr = BluetoothAddress.Parse(phone.DeviceAddress.ToString());
            //var uid = getname(Int32);
            BluetoothEndPoint rep = new BluetoothEndPoint(addr, BluetoothService.BluetoothBase); // BluetoothService is responsible for connectio type

            _Client.Connect(rep);
            var x = _Client.GetStream();

            //End of M7


            // M6
            //var PairedDevices = _Client.DiscoverDevices();

            //BluetoothDeviceInfo di = PairedDevices.FirstOrDefault();

            //di.SetServiceState(BluetoothService.SerialPort, true);
            //_Client.Authenticate = true;
            //_Client.Connect(di.DeviceAddress, BluetoothService.SerialPort);
            //if (!di.Connected)
            //{

            //}

            Thread t = new Thread(new ThreadStart(HandleConnection));
            t.Start();


            //M5
            startBT();

            //M4
            //var cli = new BluetoothClient();
            //var dev = new BluetoothDevice();
            ////var x = dev.DeviceAccessInformation;
            //var peers = cli.DiscoverDevices();
            //foreach (var peer in peers)
            //{
            //    Console.WriteLine($"Device Name: {peer.DeviceName}, Connected: {peer.Connected}");
            //    //await BluetoothDevice.FromBluetoothAddressAsync(peer.DeviceAddress);
            //    var serviceRecords = peer.GetServiceRecords;
            //    var InstalledServices = peer.InstalledServices;
            //    var ClassOfDevice = peer.ClassOfDevice;
            //    //peer.SetServiceState = true;

            //    var data = BluetoothDevice.GetDeviceSelectorFromDeviceName(peer.DeviceName);

            //    var id = new Guid();
            //    BluetoothListener bluetoothListener = new BluetoothListener(BluetoothService.SerialPort);
            //    //bluetoothListener.ServiceClass = ClassOfDevice;
            //    bluetoothListener.Start();
            //    // Now accept new connections, perhaps using the thread pool to handle each
            //    BluetoothClient conn = bluetoothListener.AcceptBluetoothClient();
            //    Stream peerStream = conn.GetStream();

            //}



            // M3
            //var watcher = new BluetoothLEAdvertisementWatcher();
            //watcher.Received += Watcher_Received;
            //watcher.Start();

            // M2
            //temp();


            // M1
            //BluetoothClient client = new BluetoothClient();
            //List<string> items = new List<string>();
            //BluetoothDeviceInfo[] devices = client.DiscoverDevicesInRange();
            //foreach (BluetoothDeviceInfo d in devices)
            //{
            //    items.Add(d.DeviceName);
            //}

            Console.ReadLine();

        }

        static void HandleConnection()
        {
            while (true)
            {
                if (!_Client.Connected) { break; }
                Stream s = _Client.GetStream();

                if (s.CanRead)
                {
                    int a = int.Parse(s.Length.ToString());
                    byte[] buffer = new byte[a];
                    s.Read(buffer, 0, a);
                    string msg = System.Text.ASCIIEncoding.ASCII.GetString(buffer);
                    if (true)
                    {

                    }
                    else
                    {

                    }
                }
            }
        }

        static BluetoothListener _Listener;

        public static void startBT()
        {
            _Listener = new BluetoothListener(BluetoothService.SerialPort);
            _Listener.Start();
            Thread t = new Thread(new ThreadStart(Listen));
            t.Start();
        }

        private static void Listen()
        {
            while (true)
            {
                if (_Listener.Pending())
                {
                    BluetoothClient c = _Listener.AcceptBluetoothClient();
                    ListenProcessor p = new ListenProcessor(c);
                }
            }
        }

        class ListenProcessor
        {
            private BluetoothClient _Client;
            public ListenProcessor(BluetoothClient c)
            {
                _Client = c;
                Thread t = new Thread(new ThreadStart(Do));
                t.Start();
            }

            private void Do()
            {
                Stream s = _Client.GetStream();
                while (true)
                {
                    if (s.CanRead)
                    {
                        int a = int.Parse(s.Length.ToString());
                        byte[] buffer = new byte[a];
                        s.Read(buffer, 0, a);
                        string msg = System.Text.ASCIIEncoding.ASCII.GetString(buffer);
                        //if (InvokeRequired)
                        //{
                        //    BeginInvoke((MethodInvoker)delegate { textBoxReceived.AppendText(msg); });
                        //}
                        //else
                        //{
                        //    textBoxReceived.AppendText(msg);
                        //}
                        _Client.Client.Send(buffer);
                    }
                }
            }
        }


        //private void textBoxInput_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    if (e.KeyChar == (char)Keys.Return)
        //    {
        //        string msg = textBoxInput.Text.Trim();
        //        _Client.Client.Send(System.Text.ASCIIEncoding.ASCII.GetBytes(msg));
        //        textBoxInput.Text = string.Empty;
        //        textBoxReceived.AppendText("> " + msg);
        //        _Client.Client.Send(buffer); // System.Net.Sockets.SocketException: 'A request to send or receive data was disallowed because the socket is not connected and (when sending on a datagram socket using a sendto call) no address was supplied'
        //    }
        //}

        private static void Watcher_Received(BluetoothLEAdvertisementWatcher sender, BluetoothLEAdvertisementReceivedEventArgs args)
        {
            Console.WriteLine(args.BluetoothAddress.ToString("x") + ";" + args.RawSignalStrengthInDBm);
        }

        static async void temp()
        {
            DeviceInformationCollection PairedBluetoothDevices = await DeviceInformation.FindAllAsync(BluetoothDevice.GetDeviceSelectorFromPairingState(true));

        }
    }
}