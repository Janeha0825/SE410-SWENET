using System;

namespace AdamKinney.RSS
{
	public class RSSEnclosure
	{
		private string url;
		private string length;
		private string type;

		public RSSEnclosure(string url, string length, string type)
		{
            this.url = url;
			this.length = length;
			this.type = type;
		}

		public override string ToString()
		{
			return @"<enclosure url=""" + this.url + @""" length=""" + this.length + @""" type=""" + this.type + @""" />";
		}

	}
}
