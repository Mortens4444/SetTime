using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace SetTime
{
    public class SocketSiteDownloader
	{
		private const int MaxBufferSize = 2048;
		private const int Timeout = 20000;

		public string GetSiteContent(string url, ushort port = 80)
		{
			var protocol = "HTTP";
			//var protocol = url.StartsWith("https") ? "HTTPS" : "HTTP";
			var ipAddress = url.Replace("https://", String.Empty).Replace("http://", String.Empty).TrimEnd('/');
			var message = $"GET / {protocol}/1.1\r\nHost: {ipAddress}\r\nAccept: text/plain, text/html\r\nAccept-Language:en\r\nConnection: Keep-Alive\r\n\r\n\r\n";
			var result = new StringBuilder();
			Socket socket = null;
			try
			{
				socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
				{
					ReceiveBufferSize = MaxBufferSize,
					SendBufferSize = MaxBufferSize,
					SendTimeout = Timeout,
					ReceiveTimeout = Timeout
				};

				var asyncResult = socket.BeginConnect(ipAddress, port, null, null);
				var success = asyncResult.AsyncWaitHandle.WaitOne(Timeout, true);

				if (success)
				{
					var sendBuffer = Encoding.ASCII.GetBytes(message);
					socket.Send(sendBuffer, sendBuffer.Length, SocketFlags.None);
					WaitForBytesOnSocket(socket, 10000);

					int readedBytes;
					do
					{
						var receiveBuffer = new byte[socket.Available];
						readedBytes = socket.Receive(receiveBuffer, receiveBuffer.Length, SocketFlags.None);
						if (readedBytes > 0)
						{
							var s = new String(Encoding.ASCII.GetChars(receiveBuffer, 0, readedBytes));
							result.Append(s);
						}
						WaitForBytesOnSocket(socket, 100);
					} while (socket.Available > 0);
				}
			}
			finally
			{
				if (socket != null)
				{
					socket.Close();
				}
			}

			return result.ToString();
		}

		private static void WaitForBytesOnSocket(Socket socket, ushort repeat)
		{
			int r = 0;
			while ((socket.Available == 0) && (r < repeat))
			{
				Thread.Sleep(1);
				r++;
			}
		}
	}
}
