using System;

namespace AdamKinney.RSS
{
	public class RSSImage
	{
		private string url;
		private string title;
		private string link;
		private string width;
		private string height;
		private string description;

		public RSSImage(string url, string title, string link)
		{
			this.url = url;
			this.title = title;
			this.link = link;	
		}

		public RSSImage(string url, string title, string link, string width, string height, string description)
		{
			this.url = url;
			this.title = title;
			this.link = link;	
			this.width = width;
			this.height = height;
			this.description = description;
		}

		public string Width
		{
			get{ return this.width; }
			set{ this.width = value; }
		}

		public string Height
		{
			get{ return this.height; }
			set{ this.height = value; }
		}

		public string Description
		{
			get{ return this.description; }
			set{ this.description = value; }
		}

		public override string ToString()
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();

			sb.Append("<image>");
			sb.Append("<url>" +  this.url + "</url>");
			sb.Append("<title>" + this.title + "</title>");
			sb.Append("<link>" + this.link + "</link>");
			if(this.width != null)
				sb.Append("<width>" + this.width + "</width>");
			if(this.height != null)
				sb.Append("<height>" + this.height + "</height>");
			if(this.description != null)
				sb.Append("<description>" + this.description + "</description>");
			sb.Append("</image>");

			return sb.ToString();
		}

	}
}
