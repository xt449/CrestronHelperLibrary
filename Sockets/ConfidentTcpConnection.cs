using Crestron.SimplSharp;
using Crestron.SimplSharp.CrestronSockets;
using System;
using System.Text;

namespace CrestronHelperLibrary.Sockets
{
	public class ConfidentTcpConnection : IDisposable
	{
		private readonly TCPClient tcp;

		private readonly Encoding encoding;

		private readonly byte[] heartbeatPayload;
		private readonly int heartbeatTimeoutMs;
		private readonly int receiveTimeoutMs;

		private readonly CTimer receiveTimer;
		private readonly CTimer heartbeatTimer;

		public event EventHandler? StatusConnected;
		public event EventHandler? StatusDisconnected;
		public event EventHandler<byte[]>? DataReceivedAsBytes;
		public event EventHandler<string>? DataReceivedAsString;

		public ConfidentTcpConnection(string host, int port, byte[] heartbeatPayload, Encoding encoding, int receiveTimeoutMs = 3_000, int heartbeatTimeoutMs = 5_000, int bufferSize = 8192)
		{
			tcp = new TCPClient(host, port, bufferSize);
			tcp.SocketSendOrReceiveTimeOutInMs = 60_000; // 1 minute maximum timeout
			tcp.SocketStatusChange += HandleClientSocketStatusChange;

			this.encoding = encoding;

			this.heartbeatPayload = heartbeatPayload;
			this.receiveTimeoutMs = receiveTimeoutMs;
			this.heartbeatTimeoutMs = heartbeatTimeoutMs;

			receiveTimer = new CTimer(ReceiveTimeout, Timeout.Infinite);
			heartbeatTimer = new CTimer(HeartbeatTimeout, Timeout.Infinite);
		}

		public ConfidentTcpConnection(string host, int port, byte[] heartbeatPayload, int receiveTimeoutMs = 3_000, int heartbeatTimeoutMs = 5_000, int bufferSize = 8192) :
			this(host, port, heartbeatPayload, Encoding.GetEncoding(28591), receiveTimeoutMs, heartbeatTimeoutMs, bufferSize)
		{

		}

		public ConfidentTcpConnection(string host, int port, string heartbeatPayload, Encoding encoding, int receiveTimeoutMs = 3_000, int heartbeatTimeoutMs = 5_000, int bufferSize = 8192) :
			this(host, port, encoding.GetBytes(heartbeatPayload), encoding, receiveTimeoutMs, heartbeatTimeoutMs, bufferSize)
		{

		}

		private void ReceiveTimeout(object? _)
		{
			// Send heartbeat data
			tcp.SendData(heartbeatPayload, 0, heartbeatPayload.Length);

			// Reset heartbeat timer
			heartbeatTimer.Reset(heartbeatTimeoutMs);
		}

		private void HeartbeatTimeout(object? _)
		{
			// Handle disconnect
			tcp.HandleLinkLoss();
		}

		private void Connect(object? _)
		{
			if (tcp.ClientStatus != SocketStatus.SOCKET_STATUS_CONNECTED)
			{
				// Begin connecting
				tcp.ConnectToServerAsync(Connect);
			}
		}

		private void HandleClientSocketStatusChange(object _, SocketStatus socketStatus)
		{
			if (socketStatus == SocketStatus.SOCKET_STATUS_CONNECTED)
			{
				// Reset receive timer
				receiveTimer.Reset(receiveTimeoutMs);

				// Cancel heartbeat timer
				heartbeatTimer.Stop();

				// Trigger event
				StatusConnected?.Invoke(this, EventArgs.Empty);

				// Begin listening
				tcp.ReceiveDataAsync(HandleClientReceiveData);
			}
			else
			{
				// Cancel receive timer
				receiveTimer.Stop();

				// Cancel heartbeat timer
				heartbeatTimer.Stop();

				// Trigger event
				StatusDisconnected?.Invoke(this, EventArgs.Empty);

				// Reconnect
				tcp.DisconnectFromServer();
				Connect(null);
			}
		}

		private void HandleClientReceiveData(object _, int bytesReceived)
		{
			// If active connection
			if (bytesReceived > 0)
			{
				// Reset receive timer
				receiveTimer.Reset(receiveTimeoutMs);

				// Cancel heartbeat timer
				heartbeatTimer.Stop();

				// Copy to dedicated array
				byte[] data = new byte[bytesReceived];
				Array.Copy(tcp.IncomingDataBuffer, data, bytesReceived);

				// Trigger events
				DataReceivedAsBytes?.Invoke(this, data);
				DataReceivedAsString?.Invoke(this, encoding.GetString(data, 0, data.Length));

				// Listen again
				tcp.ReceiveDataAsync(HandleClientReceiveData);
			}
		}

		// Public

		public void Start()
		{
			Connect(null);
		}

		public void SendBytes(byte[] buffer, int offset, int length)
		{
			tcp.SendData(buffer, offset, length);
		}

		public void SendBytes(byte[] buffer)
		{
			tcp.SendData(buffer, 0, buffer.Length);
		}

		public void SendString(string text)
		{
			byte[] data = encoding.GetBytes(text);
			tcp.SendData(data, 0, data.Length);
		}

		// IDisposable

		public void Dispose()
		{
			tcp?.Dispose();
			receiveTimer?.Dispose();
			heartbeatTimer?.Dispose();

			GC.SuppressFinalize(this);
		}
	}
}
