# Simple C# .NET Socket Server Framework

This project is a simple framework for using a C# .NET Socket server with `TcpListener`. It is designed to help developers understand and implement socket programming in C#.

## Features

- Simple and easy-to-understand code structure
- Uses `TcpListener` for handling socket connections
- Provides a basic example of socket communication

## Requirements

- .NET Framework (version 4.7.2 or later)
- Visual Studio 2019 or later

## Getting Started

1. Clone the repository.

    for HTTPS:
    ```
    git clone https://github.com/LuviKunG/csharp-socket-server-framework.git
    ```

    for SSH:
    ```
    git clone git@github.com:LuviKunG/csharp-socket-server-framework.git
    ```

2. Open the solution file (`SocketServer.sln`) in Visual Studio.

3. Build the solution to restore the necessary NuGet packages.

4. Run the project.

## Usage

- The server listens for incoming TCP connections on a specified port.
- Clients can connect to the server and send messages to demonstrate socket communication.

## Example

Here is a simple example of how to connect to the server and send a message:

```csharp
using System;
using System.Net.Sockets;
using System.Text;

class Program
{
    static void Main()
    {
        TcpClient client = new TcpClient("127.0.0.1", 8080);
        NetworkStream stream = client.GetStream();

        string message = "HELLO_SERVER";
        byte[] data = Encoding.ASCII.GetBytes(message);

        stream.Write(data, 0, data.Length);

        // Close everything
        stream.Close();
        client.Close();
    }
}
```

## Contributing

Contributions are welcome! Please fork the repository and submit a pull request.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
