using System;
using System.Xml;
using System.Collections;
using System.Collections.Specialized;


namespace AdamKinney.RSS
{
	public class RSSFeed
	{
		#region Private Variables

		private bool implementsSlash;
		private string title;
		private string link;
		private string description;
		private string language;
		private string copyright;
		private string managingEditor;
		private string webMaster;
		private string pubDate;
		private string lastBuildDate;
		private RSSCategoryCollection categories = new RSSCategoryCollection();
		private string generator;
		private string docs;
		private RSSCloud cloud;
		private string ttl;
		private RSSImage image;
		private RSSItemCollection items = new RSSItemCollection();

		#endregion

		#region Constructor

		public RSSFeed(string title, string link, string description)
		{
			this.title = title;
			this.link = link;
			this.description = description;
		}


		#endregion

		#region Public Properties

		public RSSCategoryCollection Categories
		{
			get{ return this.categories; }
			set{ this.categories = value; }
		}

		public RSSCloud Cloud
		{
			get{ return this.cloud; }
			set{ this.cloud = value; }
		}

		public string Copyright
		{
			get{ return this.copyright; }
			set{ this.copyright = value; }
		}

		public string Docs
		{
			get{ return this.docs; }
			set{ this.docs = value; }
		}

		public string Generator
		{
			get{ return this.generator; }
			set{ this.generator = value; }
		}

		public RSSImage Image
		{
			get{ return this.image; }
			set{ this.image = value; }
		}

		public bool ImplementsSlash
		{
			get{ return this.implementsSlash; }
			set{ this.implementsSlash = value; }
		}

		public RSSItemCollection Items
		{
			get{ return this.items; }
			set{ this.items = value; }
		}

		public string Language
		{
			get{ return this.language; }
			set{ this.language = value; }
		}

		public string LastBuildDate
		{
			get{ return this.lastBuildDate; }
			set{ this.lastBuildDate = value; }
		}

		public string ManagingEditor
		{
			get{ return this.managingEditor; }
			set{ this.managingEditor = value; }
		}

		public string PubDate
		{
			get{ return this.pubDate; }
			set{ this.pubDate = value; }
		}

		public string TTL
		{
			get{ return this.ttl; }
			set
			{
				if(isNumber(value))
				{
					this.ttl = value; 
				}
				else
				{
					throw new Exception("Time to live was set to a value other than a number");
				}
			}
		}

		public string WebMaster
		{
			get{ return this.webMaster; }
			set{ this.webMaster = value; }
		}


		#endregion

		#region Public Methods

        public override string ToString()
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder(256);

			sb.Append(@"<?xml version=""1.0"" ?><rss version=""2.0""");
			if(this.implementsSlash)
				sb.Append(@" xmlns:slash=""http://purl.org/rss/1.0/modules/slash/""");
			sb.Append(@">");
			sb.Append(@"<channel>");
			sb.Append(@"<title>" + this.title + "</title>");
			sb.Append(@"<link>" + this.link + "</link>");
			sb.Append(@"<description>" + this.description + "</description>");
			if(this.language != null)
				sb.Append(@"<language>" + this.language + "</language>");
			if(this.copyright != null)
				sb.Append(@"<copyright>" + this.copyright + "</copyright>");
			if(this.managingEditor != null)
				sb.Append(@"<managingEditor>" + this.managingEditor + "</managingEditor>");
			if(this.webMaster != null)
				sb.Append(@"<webMaster>" + this.webMaster + "</webMaster>");
			if(this.pubDate != null)
				sb.Append(@"<pubDate>" + this.pubDate + "</pubDate>");
			if(this.lastBuildDate != null)
				sb.Append(@"<lastBuildDate>" + this.lastBuildDate + "</lastBuildDate>");
			foreach(RSSCategory c in this.Categories)
			{
				sb.Append(c.ToString());
			}
			if(this.generator != null)
				sb.Append(@"<generator>" + this.generator + "</generator>");
			if(this.docs != null)
				sb.Append(@"<docs>" + this.docs + "</docs>");
			if(this.cloud != null)
				sb.Append(this.cloud.ToString());
			if(this.ttl != null)
				sb.Append(@"<ttl>" + this.ttl + "</ttl>");
			if(this.image != null)
				sb.Append(this.image.ToString());
			foreach(RSSItem i in this.Items)
			{
				sb.Append(i.ToString());
			}
			sb.Append(@"</channel>");
			sb.Append(@"</rss>");

			return sb.ToString();
		}


        #endregion

		#region Private Methods

		private bool isNumber(string s)
		{
			bool temp = true;
			Char[] ca = s.ToCharArray();

			foreach (char c in ca)
			{
				if(!Char.IsDigit(c) && !c.Equals('.'))
				{
					temp = false;
					break;
				}
			}
			return temp;
		}


		#endregion

	}
}
