using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace SetTime
{
    public class SocketSiteDownloader : ISiteDownloader
	{
		private const int MaxBufferSize = 2048;
		private const int Timeout = 20000;

		public string GetSiteContent(string url)
		{
			//var protocol = "HTTP";
			var protocol = url.StartsWith("https") ? "HTTPS" : "HTTP";
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

				var port = 80;
				var asyncResult = socket.BeginConnect(ipAddress, port, null, null);
				var success = asyncResult.AsyncWaitHandle.WaitOne(Timeout, true);

				if (success)
				{
					var sendBuffer = Encoding.UTF8.GetBytes(message);
					socket.Send(sendBuffer, sendBuffer.Length, SocketFlags.None);
					
					while (socket.Available == 0)
                    {
						WaitForBytesOnSocket(socket, 10000);
                    }

					do
					{
						var receiveBuffer = new byte[socket.Available];
						var readedBytes = socket.Receive(receiveBuffer, receiveBuffer.Length, SocketFlags.None);
						if (readedBytes > 0)
						{
							var pagePart = new String(Encoding.ASCII.GetChars(receiveBuffer, 0, readedBytes));
							result.Append(pagePart);
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

		private static void WaitForBytesOnSocket(Socket socket, ushort maxIteration)
		{
			int iteration = 0;
			while ((socket.Available == 0) && (iteration < maxIteration))
			{
				Thread.Sleep(1);
				iteration++;
			}
		}
    }
}
