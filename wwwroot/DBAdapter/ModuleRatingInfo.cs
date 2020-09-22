using System;

namespace SwenetDev.DBAdapter {
	[Serializable()]
	/// <summary>
	/// Rating information for a particular module.
	/// </summary>
	public class ModuleRatingInfo {

		private int moduleID = 0;
		private float rating = 0;
		private float numRatings = 0;
		private int threadID = 0;

		/// <summary>
		/// The identifier of the module that this rating applies to.
		/// </summary>
		public int ModuleID {
			get { return moduleID; }
			set { moduleID = value; }
		}

		/// <summary>
		/// The module's rating average.
		/// </summary>
		public float Rating {
			get { return rating; }
			set { rating = value; }
		}

		/// <summary>
		/// The number of times the module was rated.
		/// </summary>
		public float NumRatings {
			get { return numRatings; }
			set { numRatings = value; }
		}

		/// <summary>
		/// The threadID where rating posts are to be added.
		/// </summary>
		public int ThreadID {
			get { return threadID; }
			set { threadID = value; }
		}

		public void incrementNumRatings() {
			numRatings++;
		}
	}
}
