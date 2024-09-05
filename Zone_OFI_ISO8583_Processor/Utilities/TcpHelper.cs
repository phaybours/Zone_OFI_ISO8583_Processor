using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Zone_OFI_ISO8583_Processor.Utilities;

public static class TcpHelper
{
    public static string TcpClientSendClientRequest(string ipAddress, int port, string request)
    {
        try
        {
            // Create a TCP client and connect to the server
            using (TcpClient client = new TcpClient(ipAddress, port))
            {
                // Convert the request string to a byte array
                byte[] dataToSend = Encoding.ASCII.GetBytes(request);

                // Get the stream to write to the server
                using (NetworkStream stream = client.GetStream())
                {
                    // Send the request to the server
                    stream.Write(dataToSend, 0, dataToSend.Length);

                    // Buffer to store the response bytes
                    byte[] responseBuffer = new byte[1024];
                    int bytesRead = stream.Read(responseBuffer, 0, responseBuffer.Length);

                    // Convert the response bytes to a string
                    string response = Encoding.ASCII.GetString(responseBuffer, 0, bytesRead);

                    return response;
                }
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions (e.g., connection issues)
            return $"Error: {ex.Message}";
        }
    }

    public static string SocketSendClientRequest(string isoMessage)
    {
        var connDetail = ZNConnection.GetDetiails();

        byte[] buffer = new byte[1024];
        string response = string.Empty;

        try
        {
            // Get the IP address of the host
            //IPHostEntry hostEntry = Dns.GetHostEntry(connDetail.ZNIpAddress);
            IPAddress ipAddress = IPAddress.Parse(connDetail.ZNIpAddress);
            IPEndPoint remoteEndPoint = new IPEndPoint(
                ipAddress,
                Convert.ToInt32(connDetail.ZNPort));

            // Create a TCP/IP socket
            using (Socket socket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp))
            {
                // Connect to the remote endpoint
                socket.Connect(remoteEndPoint);
                Console.WriteLine($"Socket connected to {socket.RemoteEndPoint}");

                // Encode the ISO message into a byte array
                byte[] messageBytes = Encoding.ASCII.GetBytes($"{isoMessage}<EOF>");

                // Send the ISO message to the remote server
                socket.Send(messageBytes);

                // Receive the response from the remote server
                int bytesReceived = socket.Receive(buffer);
                response = Encoding.ASCII.GetString(buffer, 0, bytesReceived);

                Console.WriteLine($"Received response: {response}");

                // Shutdown and close the socket
                socket.Shutdown(SocketShutdown.Both);
            }
        }
        catch (ArgumentNullException argEx)
        {
            Console.WriteLine($"ArgumentNullException: {argEx.Message}");
        }
        catch (SocketException socketEx)
        {
            Console.WriteLine($"SocketException: {socketEx.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected exception: {ex.Message}");
        }

        return response;
    }
}