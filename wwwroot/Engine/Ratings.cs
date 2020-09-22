using System;

namespace AspNetForums {
	using Components;
	/// <summary>
	/// Summary description for Ratings.
	/// </summary>
	public class Ratings {
		public static void AddRating( Rating rating ) {
			// Create Instance of the IDataProviderBase
			IDataProviderBase dp = DataProvider.Instance();

			dp.AddRating( rating );
		}

		public static Rating GetRating( int postID ) {
			// Create Instance of the IDataProviderBase
			IDataProviderBase dp = DataProvider.Instance();

			return dp.GetRating( postID );
		}

		public static Rating GetRatingForUser( string username, int parentID ) {
			// Create Instance of the IDataProviderBase
			IDataProviderBase dp = DataProvider.Instance();

			return dp.GetRatingForUser( username, parentID );
		}
	}
}
