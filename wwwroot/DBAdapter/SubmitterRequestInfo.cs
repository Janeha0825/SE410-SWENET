using System;

namespace SwenetDev.DBAdapter {
	/// <summary>
	/// Summary description for SubmitterRequestInfo.
	/// </summary>
	public class SubmitterRequestInfo {
		private string username;
		private DateTime date;
		private string message;
		private string sId;

		public string UserName {
			get { return username; }
			set { username = value; }
		}

		public DateTime Date {
			get { return date; }
			set { date = value; }
		}

		public string Message { 
			get { return message; }
			set { message = value; }
		}

		public string SubmitterId {
			get { return sId; }
			set { sId = value; }
		}
	}
}
