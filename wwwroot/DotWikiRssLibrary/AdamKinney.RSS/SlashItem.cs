using System;

namespace AdamKinney.RSS
{
	public class SlashItem
	{
		private string section;
		private string department;
		private string comments;
		private string hit_parade;

		public SlashItem(){}

		public SlashItem(string section, string department, string comments, string hit_parade)
		{
			this.section = section;
			this.department = department;
			this.comments = comments;
			this.hit_parade = hit_parade;
		}

		public string Section
		{
			get{ return this.section; }
			set{ this.section = value; }
		}

		public string Department
		{
			get{ return this.department; }
			set{ this.department = value; }
		}

		public string Comments
		{
			get{ return this.comments; }
			set{ this.comments = value; }
		}

		public string Hit_parade
		{
			get{ return this.hit_parade; }
			set{ this.hit_parade = value; }
		}

		public override string ToString()
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
				
			if(this.section != null)
				sb.Append("<slash:section>" + this.section + "</slash:section>");
			if(this.department != null)
				sb.Append("<slash:department>" + this.department + "</slash:department>");
			if(this.comments != null)
				sb.Append("<slash:comments>" + this.comments + "</slash:comments>");
			if(this.hit_parade != null)
				sb.Append("<slash:hit_parade>" + this.hit_parade + "</slash:hit_parade>");
				
			return sb.ToString();
		}

	}
}
