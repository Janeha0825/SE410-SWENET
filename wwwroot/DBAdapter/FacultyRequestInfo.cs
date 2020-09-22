using System;

namespace SwenetDev.DBAdapter 
{
	/// <summary>
	/// Summary description for FacultyRequestInfo.
	/// </summary>
	public class FacultyRequestInfo 
	{
		private string username;
		private DateTime date;
		private string name;
		private string aff;
		private string proof;

		public string UserName 
		{
			get { return username; }
			set { username = value; }
		}

		public DateTime Date 
		{
			get { return date; }
			set { date = value; }
		}

		public string Name 
		{ 
			get { return name; }
			set { name = value; }
		}

		public string Affiliation 
		{
			get { return aff; }
			set { aff = value; }
		}

		public string Proof
		{
			get { return proof; }
			set { proof = value; }
		}
	}
}
