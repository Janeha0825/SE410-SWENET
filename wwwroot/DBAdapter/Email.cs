using System;

namespace SwenetDev.DBAdapter {
	/// <summary>
	/// Summary description for Email.
	/// </summary>
	[Serializable()]
	public class Email {
		private EmailType type;
		private string subject;
		private string body;
		private string to;

		public EmailType Type {
			get { return type; }
			set { type = value; }
		}
		public string Subject {
			get { return subject; }
			set { subject = value; }
		}
		public string Body {
			get { return body; }
			set { body = value; }
		}
	}
}
