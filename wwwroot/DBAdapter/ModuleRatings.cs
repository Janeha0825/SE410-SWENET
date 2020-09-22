using System;
using System.Data;
using System.Data.SqlClient;

namespace SwenetDev.DBAdapter {
	/// <summary>
	/// Summary description for ModuleRatings.
	/// </summary>
	public class ModuleRatings {

		public static void createRating( ModuleRatingInfo ri ) {
			IDbCommand cmd = new SqlCommand();
			cmd.Connection = new SqlConnection( Globals.ConnectionString );
			cmd.CommandText = "INSERT INTO ModuleRatings " +
				"( ModuleID, Rating, NumRatings, ThreadID ) " +
				"VALUES ( @ModuleID, @Rating, @NumRatings, @ThreadID )";
			cmd.Parameters.Add( new SqlParameter( "@ModuleID", ri.ModuleID ) );
			cmd.Parameters.Add( new SqlParameter( "@Rating", ri.Rating ) );
			cmd.Parameters.Add( new SqlParameter( "@NumRatings", ri.NumRatings ) );
			cmd.Parameters.Add( new SqlParameter( "@ThreadID", ri.ThreadID ) );

			try {
				cmd.Connection.Open();
				cmd.ExecuteNonQuery();
			} catch ( SqlException e ) {
				throw e;
			} finally {
				cmd.Connection.Close();
			}
		}

		public static void updateRating( ModuleRatingInfo ri ) {
			IDbCommand cmd = new SqlCommand();
			cmd.Connection = new SqlConnection( Globals.ConnectionString );
			cmd.CommandText = "UPDATE ModuleRatings " +
				"SET Rating = @Rating, NumRatings = @NumRatings WHERE ModuleID = @ModuleID";
			cmd.Parameters.Add( new SqlParameter( "@Rating", ri.Rating ) );
			cmd.Parameters.Add( new SqlParameter( "@NumRatings", ri.NumRatings ) );
			cmd.Parameters.Add( new SqlParameter( "@ModuleID", ri.ModuleID ) );

			try {
				cmd.Connection.Open();
				cmd.ExecuteNonQuery();
			} catch ( SqlException e ) {
				throw e;
			} finally {
				cmd.Connection.Close();
			}
		}

		public static ModuleRatingInfo getRating( int moduleID ) {
			ModuleRatingInfo retVal = new ModuleRatingInfo();
			retVal.ModuleID = moduleID;

			IDbCommand cmd = new SqlCommand();
			cmd.Connection = new SqlConnection( Globals.ConnectionString );
			cmd.CommandText = "SELECT Rating, NumRatings, ThreadID " +
				"FROM ModuleRatings WHERE ModuleID = @ModuleID";
			cmd.Parameters.Add( new SqlParameter( "@ModuleID", moduleID ) );

			IDataReader reader = null;

			try { 
				cmd.Connection.Open();
				reader = cmd.ExecuteReader( CommandBehavior.CloseConnection );

				if ( reader.Read() ) {
					retVal.Rating = Convert.ToSingle( reader["Rating"] );
					retVal.NumRatings = Convert.ToInt32( reader["NumRatings"] );
					retVal.ThreadID = Convert.ToInt32( reader["ThreadID"] );
				}
			} catch ( SqlException e ) {
				throw e;
			} finally {
				reader.Close();
			}
				
			return retVal;
		}
	}
}
