using System;

namespace AdamKinney.RSS
{
	public class RSSCategory
	{
		private string innerText;
		private string domain;

		public RSSCategory(string innerText)
		{
			this.innerText = innerText;
		}

		public RSSCategory(string innerText, string domain)
		{
			this.innerText = innerText;
			this.domain = domain;
		}

		public string Domain
		{
			get{ return this.domain; }
			set{ this.domain = value; }
		}

		public string InnerText
		{
			get{ return this.innerText; }
			set{ this.innerText = value; }
		}

		public override string ToString()
		{
			return "<category" + (this.domain != null ? @" domain=""" + this.domain + @"""" : "" ) + ">" + this.innerText + "</category>";
		}
	}
}
