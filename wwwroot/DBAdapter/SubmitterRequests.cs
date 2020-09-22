using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace SwenetDev.DBAdapter {
	/// <summary>
	/// Summary description for SubmitterRequests.
	/// </summary>
	public class SubmitterRequests {
		/// <summary>
		/// User requests submitter status.
		/// </summary>
		/// <param name="sri">The information about the request.</param>
		public static void addSubmitterRequest( SubmitterRequestInfo sri ) {
			IDbCommand cmd = new SqlCommand();
			cmd.Connection = new SqlConnection( Globals.UsersConnectionString );
			cmd.CommandText = "INSERT INTO SubmitterRequests(UserName, Date, Message, SubmitterId) " +
				"VALUES (@UserName, @Date, @Message, @SubmitterId)";
			cmd.Parameters.Add( new SqlParameter( "@UserName", sri.UserName ) );
			cmd.Parameters.Add( new SqlParameter( "@Date", sri.Date ) );
			cmd.Parameters.Add( new SqlParameter( "@Message", sri.Message ) );
			cmd.Parameters.Add( new SqlParameter( "@SubmitterId", sri.SubmitterId ) );
			
			try {
				cmd.Connection.Open();
				cmd.ExecuteNonQuery();
			} catch ( SqlException e ) {
				throw new Exception( "An error occurred while submitting your request. " +
					"Please try again.  ", e );
			} finally {
				cmd.Connection.Close();
			}
		}

		/// <summary>
		/// Remove a submitter request.
		/// </summary>
		/// <param name="username">The username to remove.</param>
		public static void remove( string username ) {
			SqlCommand cmd = new SqlCommand();
			cmd.Connection = new SqlConnection( Globals.UsersConnectionString );
			cmd.CommandText = "DELETE FROM SubmitterRequests WHERE UserName = @UserName";
			cmd.Parameters.Add( new SqlParameter( "@UserName", username ) );

			try {
				cmd.Connection.Open();
				cmd.ExecuteNonQuery();
			} catch ( SqlException ) {
				throw new Exception( "Error removing submitter's request." );
			}finally {
				cmd.Connection.Close();
			}
		}

		/// <summary>
		/// Obtain a single request for submitter status.
		/// </summary>
		/// <param name="username">The username.</param>
		/// <returns>An object containing information about the request.</returns>
		public static SubmitterRequestInfo getSubmitterRequest( string username ) {
			IDbCommand cmd = new SqlCommand();
			cmd.Connection = new SqlConnection( Globals.UsersConnectionString );
			cmd.CommandText = "SELECT Date, Message, SubmitterId FROM SubmitterRequests " +
				"WHERE UserName = @UserName";
			cmd.Parameters.Add( new SqlParameter( "@UserName", username ) );
			
			SubmitterRequestInfo retVal = null;
			IDataReader reader = null;

			try {
				cmd.Connection.Open();
				reader = cmd.ExecuteReader( CommandBehavior.CloseConnection );

				if ( reader.Read() ) {
					retVal = new SubmitterRequestInfo();
					retVal.UserName = username;
					retVal.Date = Convert.ToDateTime( reader["Date"] );
					retVal.Message = Convert.ToString( reader["Message"] );
					retVal.SubmitterId = Convert.ToString( reader["SubmitterId"] );
				}
			} catch ( SqlException e ) {
				throw new Exception( "An error occurred obtaining your module " +
					"submission request status.  Please try again.  ", e );
			} finally {
				reader.Close();
			}

			return retVal;
		}

		/// <summary>
		/// Get a list of all submitter requests ordered by date.
		/// </summary>
		/// <returns>The list of submitter requests.</returns>
		public static IList getSubmitterRequests() {
			IDbCommand cmd = new SqlCommand();
			cmd.Connection = new SqlConnection( Globals.UsersConnectionString );
			cmd.CommandText = "SELECT UserName, Date, Message, SubmitterId FROM SubmitterRequests " +
				"ORDER BY Date";
			
			IList retVal = new ArrayList();
			IDataReader reader = null;

			try {
				cmd.Connection.Open();
				reader = cmd.ExecuteReader( CommandBehavior.CloseConnection );

				while ( reader.Read() ) {
					SubmitterRequestInfo sri = null;
					sri = new SubmitterRequestInfo();
					sri.UserName = Convert.ToString( reader["UserName"] );
					sri.Date = Convert.ToDateTime( reader["Date"] );
					sri.Message = Convert.ToString( reader["Message"] );
					sri.SubmitterId = Convert.ToString( reader["SubmitterId"] );
					retVal.Add( sri );
				}
			} catch ( SqlException e ) {
				throw new Exception( "An error occurred obtaining the module " +
					"submission requests.  ", e );
			} finally {
				reader.Close();
			}

			return retVal;
		}

		/// <summary>
		/// Determine if there is a submitter request with the given
		/// submitter id.
		/// </summary>
		/// <param name="id">The submitter id to search for.</param>
		/// <returns>True if a submitter request is found.</returns>
		public static bool submitterIdExists( string id ) {
			bool retVal = false;

			IDbCommand cmd = new SqlCommand();
			cmd.Connection = new SqlConnection( Globals.UsersConnectionString );
			cmd.Parameters.Add( new SqlParameter( "@SubmitterId", id ) );
			cmd.CommandText = "SELECT SubmitterId FROM SubmitterRequests " +
				"WHERE SubmitterId = @SubmitterId";

			IDataReader reader = null;

			try {
				cmd.Connection.Open();
				reader = cmd.ExecuteReader( CommandBehavior.CloseConnection );

				retVal = reader.Read();
			} finally {
				reader.Close();
			}

			return retVal;
		}

	}
}
