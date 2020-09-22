using System;

namespace AdamKinney.RSS
{
	public class RSSItem
	{
		#region Private Variables

		private string title;
		private string link;
		private string description;
		private string author;
		private RSSCategoryCollection categories = new RSSCategoryCollection();
		private string comments;
		private RSSEnclosure encolsure;
		private string guid;
		private bool guidIsPermaLink;
		private string pubDate;
		private string sourceUrl;
		private string sourceTitle;
		private SlashItem slash = new SlashItem();

		#endregion

		#region Constructor

		public RSSItem(string title)
		{
			this.title = title;
		}

		public RSSItem(string title, string description)
		{
			this.title = title;
			this.description = description;
		}

		#endregion

		#region Public Properties

		public string Author
		{
			get{ return this.author; }
			set{ this.author = value; }
		}

		public RSSCategoryCollection Categories
		{
			get{ return this.categories; }
			set{ this.categories = value; }
		}

		public string Comments
		{
			get{ return this.comments; }
			set{ this.comments = value; }
		}

		public RSSEnclosure Enclosure
		{
			get{ return this.encolsure; }
			set{ this.encolsure = value; }
		}

		public string Guid
		{
			get{ return this.guid; }
			set{ this.guid = value; }
		}

		public bool GuidIsPermalink
		{
			get{ return this.guidIsPermaLink; }
			set{ this.guidIsPermaLink = value; }
		}

		public string Link
		{
			get{ return this.link; }
			set{ this.link = value; }
		}

		public string PubDate
		{
			get{ return this.pubDate; }
			set{ this.pubDate = value; }
		}

		public SlashItem Slash
		{
			get{ return this.slash; }
			set{ this.slash = value; }
		}

		public string SourceTitle
		{
			get{ return this.sourceTitle; }
			set{ this.sourceTitle = value; }
		}

		public string SourceUrl
		{
			get{ return this.sourceUrl; }
			set{ this.sourceUrl = value; }
		}

		#endregion
        
		#region Public Methods

		public override string ToString()
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder(256);
			
			sb.Append("<item>");
			if(this.title != null)
				sb.Append("<title>" + this.title + "</title>");
			if(this.description != null)
				sb.Append("<description>" + this.description + "</description>");
			if(this.link != null)
				sb.Append("<link>" + this.link + "</link>");
			if(this.author != null)
				sb.Append("<author>" + this.author + "</author>");
			foreach(RSSCategory c in this.Categories)
			{
				sb.Append(c.ToString());
			}
			if(this.comments != null)
				sb.Append("<comments>" + this.comments + "</comments>");
			if(this.encolsure != null)
				sb.Append(this.encolsure.ToString());
			if(this.guid != null)
				sb.Append(@"<guid isPermaLink=""" + this.guidIsPermaLink.ToString().ToLower() + @""">" + this.guid + "</guid>");
			if(this.pubDate != null)
				sb.Append("<pubDate>" + this.pubDate + "</pubDate>");
			if(this.sourceUrl != null)
			{
				if(this.sourceTitle != null)
					sb.Append(@"<source url=""" + this.sourceUrl + @""">" + this.sourceTitle + "</source>");
				else
					sb.Append(@"<source url=""" + this.sourceUrl + @""" />");
			}
			if(this.slash != null)
				sb.Append(this.slash.ToString());
			sb.Append("</item>");

			return sb.ToString();
		}

		#endregion

	}
}
