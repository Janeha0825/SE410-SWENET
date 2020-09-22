using System;

namespace AspNetForums.Components {
	[Serializable()]
	/// <summary>
	/// Summary description for Rating.
	/// </summary>
	public class Rating {
		private int postID = 0;
		private int ratingValue = 0;

		public int PostID {
			get { return postID; }
			set { postID = value; }
		}
		public int Value {
			get { return ratingValue; }
			set { ratingValue = value; }
		}
	}
}
