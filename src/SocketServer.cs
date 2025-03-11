// Created by Thanut Panichyotai (@LuviKunG).
// https://www.github.com/LuviKunG
// CC-BY-NC-SA 4.0 International Public License
// https://creativecommons.org/licenses/by-sa/4.0/

using System.Net;
using System.Net.Sockets;
using System.Text;

/// <summary>
/// A class that handles the socket server operations.
/// </summary>
internal static class SocketServer
{
    /// <summary>
    /// Starts the socket server.
    /// </summary>
    /// <param name="ip">The IP address to bind to.</param>
    /// <param name="port">The port to listen on.</param>
    /// <param name="addressFamily">The address family (IPv4 or IPv6).</param>
    /// <param name="bufferSize">The buffer size for reading data.</param>
    /// <param name="cancellationTokenSource">The cancellation token to stop the server.</param>
    /// <returns>Task representing the asynchronous operation, with an exit code.</returns>
    public static async Task<int> StartServer(IPAddress ip, int port, AddressFamily addressFamily, int bufferSize, CancellationToken cancellationTokenSource)
    {
        try
        {
            // Create a new TcpListener to listen for incoming connections
            var listener = new TcpListener(new IPEndPoint(ip, port));
            if (addressFamily == AddressFamily.InterNetworkV6)
            {
                // Allow both IPv4 and IPv6 connections
                listener.Server.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, false);
            }
            // Start the listener
            listener.Start();
            Console.WriteLine($"Server started on {ip}:{port}");

            // Accept incoming client connections in a loop
            while (!cancellationTokenSource.IsCancellationRequested)
            {
                // Accept a new client connection
                var client = await listener.AcceptTcpClientAsync(cancellationTokenSource);
                // Handle the client connection in a separate task
                _ = Task.Run(() => HandleClient(client, bufferSize, cancellationTokenSource), cancellationTokenSource);
            }

            // Stop the listener when cancellation is requested
            listener.Stop();
            Console.WriteLine("Server stopped.");
            return 0;
        }
        catch (Exception ex)
        {
            // Log any exceptions that occur
            Console.WriteLine($"Server error: {ex.Message}");
            return 1; // Return error code 1 for any exception
        }
    }

    /// <summary>
    /// Handles the client connection.
    /// </summary>
    /// <param name="client">The connected client.</param>
    /// <param name="bufferSize">The buffer size for reading data.</param>
    /// <param name="cancellationTokenSource">The cancellation token to stop the server.</param>
    private static void HandleClient(TcpClient client, int bufferSize, CancellationToken cancellationTokenSource)
    {
        // Create a buffer to read data from the client
        var buffer = new byte[bufferSize];
        var stream = client.GetStream();
        int bytesRead;

        // Read data from the client in a loop
        while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
        {
            if (cancellationTokenSource.IsCancellationRequested)
            {
                // Log a message if the server is shutting down
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Server is shutting down...");
                Console.ResetColor();
                break;
            }
            // Convert the received bytes to a string
            string data = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            Console.WriteLine($"Received: {data}");
            // Handle the received message
            MessageHandler.HandleMessage(ref stream, data);
        }

        // Close the client connection
        client.Close();
    }
}
