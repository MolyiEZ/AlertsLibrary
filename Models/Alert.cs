using System;

namespace Molyi.Alerts.Models
{
	public class Alert(string message, string alertType)
	{
		public string Message { get; set; } = message;
		public string AlertType { get; set; } = alertType;
		public DateTime StartTime { get; set; } = DateTime.Now;

		public bool IsExpired => (DateTime.Now - StartTime).TotalSeconds >= 5;
	}
}
