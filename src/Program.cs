// Created by Thanut Panichyotai (@LuviKunG).
// https://www.github.com/LuviKunG
// CC-BY-NC-SA 4.0 International Public License
// https://creativecommons.org/licenses/by-sa/4.0/

using System.Net;
using System.Net.Sockets;
using System.Runtime.Loader;

/// <summary>
/// The main entry point for the server application.
/// </summary>
internal static class Program
{
    private static readonly CancellationTokenSource ServerCancellationTokenSource = new();

    /// <summary>
    /// The main method that starts the server.
    /// </summary>
    /// <param name="args">Command-line arguments.</param>
    /// <returns>Exit code.</returns>
    internal static int Main(string[] args)
    {
        int port = 8080; // Default port
        IPAddress ip = IPAddress.Any; // Default IP address
        AddressFamily addressFamily = AddressFamily.InterNetwork; // Default to IPv4
        int bufferSize = 1024; // Default buffer size

        // Function to check if the argument is a help command
        static bool IsHelp(string arg) =>
            arg.Equals("-help", StringComparison.OrdinalIgnoreCase) ||
            arg.Equals("--help", StringComparison.OrdinalIgnoreCase) ||
            arg.Equals("-h", StringComparison.OrdinalIgnoreCase) ||
            arg.Equals("--h", StringComparison.OrdinalIgnoreCase);

        // Iterate through command-line arguments
        foreach (var arg in args)
        {
            if (IsHelp(arg))
            {
                // Display usage information
                Console.WriteLine("Usage:");
                Console.WriteLine("  -port:<port>         Specify the port number (default: 8080)");
                Console.WriteLine("  -addr:<address>      Specify the IP address (default: Any)");
                Console.WriteLine("  -ipver:<ipv4|ipv6>   Specify the IP version (default: IPv4)");
                Console.WriteLine("  -buffersize:<size>   Specify the buffer size (default: 1024)");
                return 0;
            }
            else if (arg.StartsWith("-port:"))
            {
                // Parse and set the port number
                if (int.TryParse(arg.AsSpan(6), out int parsedPort))
                {
                    port = parsedPort;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Using port {port:D0}.");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Invalid port number. Using default port {port:D0}.");
                    Console.ResetColor();
                }
            }
            else if (arg.StartsWith("-addr:"))
            {
                // Parse and set the IP address
                var address = arg.AsSpan(6).ToString();
                if (address.Equals("localhost", StringComparison.OrdinalIgnoreCase))
                {
                    ip = IPAddress.Loopback;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Using IP address 127.0.0.1 (localhost).");
                    Console.ResetColor();
                }
                else if (IPAddress.TryParse(address, out IPAddress? parsedIP))
                {
                    ip = parsedIP;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Using IP address {ip}.");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Invalid IP address. Using default IP address {ip}.");
                    Console.ResetColor();
                }
            }
            else if (arg.StartsWith("-ipver:"))
            {
                // Parse and set the IP version
                var version = arg.AsSpan(7).ToString();
                if (version.Equals("ipv6", StringComparison.OrdinalIgnoreCase))
                {
                    addressFamily = AddressFamily.InterNetworkV6;
                    ip = IPAddress.IPv6Any;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Using IPv6.");
                    Console.ResetColor();
                }
                else if (version.Equals("ipv4", StringComparison.OrdinalIgnoreCase))
                {
                    addressFamily = AddressFamily.InterNetwork;
                    ip = IPAddress.Any;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Using IPv4.");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid IP version. Using default IPv4.");
                    Console.ResetColor();
                }
            }
            else if (arg.StartsWith("-buffersize:"))
            {
                // Parse and set the buffer size
                if (int.TryParse(arg.AsSpan(12), out int parsedBufferSize))
                {
                    bufferSize = parsedBufferSize;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Using buffer size {bufferSize}.");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Invalid buffer size. Using default buffer size {bufferSize}.");
                    Console.ResetColor();
                }
            }
        }

        // Handle SIGINT signal for graceful shutdown
        AssemblyLoadContext.Default.Unloading += ctx =>
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("SIGINT received, shutting down...");
            Console.ResetColor();
            ServerCancellationTokenSource.Cancel();
        };

        // Start the server with the specified parameters
        return SocketServer.StartServer(ip, port, addressFamily, bufferSize, ServerCancellationTokenSource.Token).Result;
    }
}

