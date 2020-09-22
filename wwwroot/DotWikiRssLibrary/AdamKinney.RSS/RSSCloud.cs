using System;

namespace AdamKinney.RSS
{
	public class RSSCloud
	{
		private string domain;
		private string port;
		private string path;
		private string registerProcedure;
		private string protocol;

		public RSSCloud(string domain, string port, string path, string registerProcedure, string protocol)
		{
			this.domain = domain;
			this.port = port;
			this.path = path;
			this.registerProcedure = registerProcedure;
			this.protocol = protocol;
		}

		public override string ToString()
		{
			return @"<cloud domain=""" + this.domain + @""" port=""" + this.port + @""" path=""" + this.path + @""" registerProcedure=""" + this.registerProcedure + @""" protocol=""" + this.protocol + @""" />";
		}
	}
}
