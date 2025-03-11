// Created by Thanut Panichyotai (@LuviKunG).
// https://www.github.com/LuviKunG
// CC-BY-NC-SA 4.0 International Public License
// https://creativecommons.org/licenses/by-sa/4.0/

using System.Net.Sockets;
using System.Text;

/// <summary>
/// A class that handles incoming messages from clients.
/// </summary>
internal static class MessageHandler
{
    /// <summary>
    /// Handles the incoming message and sends a response.
    /// </summary>
    /// <param name="stream">The network stream to send the response.</param>
    /// <param name="message">The incoming message.</param>
    public static void HandleMessage(ref NetworkStream stream, string message)
    {
        // Currently just echo the message back to the client.
        Response(ref stream, message);
    }

    /// <summary>
    /// Sends a response message back to the client.
    /// </summary>
    /// <param name="stream">The network stream to send the response.</param>
    /// <param name="message">The message to send.</param>
    private static void Response(ref NetworkStream stream, string message)
    {
        var response = Encoding.UTF8.GetBytes(message);
        stream.Write(response, 0, response.Length);
    }
}
