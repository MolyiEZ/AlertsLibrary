using System;
using System.Threading;

namespace Molyi.Alerts.Models
{
	internal class Alert(string message, string alertType) : IDisposable
	{
		public string Message { get; set; } = message;
		public string AlertType { get; set; } = alertType;
		public DateTime StartTime { get; set; } = DateTime.Now;
		public CancellationTokenSource CancelToken { get; set; } = new();

		public bool IsExpired => (DateTime.Now - StartTime).Milliseconds >= 3500;

		public void Dispose() => CancelToken.Cancel();
	}
}
