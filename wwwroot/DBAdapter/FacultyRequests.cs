using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace SwenetDev.DBAdapter 
{
	/// <summary>
	/// Summary description for FacultyRequests.
	/// </summary>
	public class FacultyRequests 
	{
		/// <summary>
		/// User requests faculty status.
		/// </summary>
		/// <param name="sri">The information about the request.</param>
		public static void addFacultyRequest( FacultyRequestInfo fri ) {
			IDbCommand cmd = new SqlCommand();
			cmd.Connection = new SqlConnection( Globals.UsersConnectionString );
			cmd.CommandText = "INSERT INTO FacultyRequests(UserName, Date, Name, Affiliation, Proof) " +
				"VALUES (@UserName, @Date, @Name, @Affiliation, @Proof)";
			cmd.Parameters.Add( new SqlParameter( "@UserName", fri.UserName ) );
			cmd.Parameters.Add( new SqlParameter( "@Date", fri.Date ) );
			cmd.Parameters.Add( new SqlParameter( "@Name", fri.Name ) );
			cmd.Parameters.Add( new SqlParameter( "@Affiliation", fri.Affiliation ) );
			cmd.Parameters.Add( new SqlParameter( "@Proof", fri.Proof ) );
			
			try {
				cmd.Connection.Open();
				cmd.ExecuteNonQuery();
			} 
			catch ( SqlException e ) {
				throw new Exception( "An error occurred while submitting your request. " +
					"Please try again.  ", e );
			} 
			finally {
				cmd.Connection.Close();
			}
		}

		/// <summary>
		/// Remove a faculty request.
		/// </summary>
		/// <param name="username">The username to remove.</param>
		public static void remove( string username ) 
		{
			SqlCommand cmd = new SqlCommand();
			cmd.Connection = new SqlConnection( Globals.UsersConnectionString );
			cmd.CommandText = "DELETE FROM FacultyRequests WHERE UserName = @UserName";
			cmd.Parameters.Add( new SqlParameter( "@UserName", username ) );

			try 
			{
				cmd.Connection.Open();
				cmd.ExecuteNonQuery();
			} 
			catch ( SqlException ) 
			{
				throw new Exception( "Error removing faculty's request." );
			}
			finally 
			{
				cmd.Connection.Close();
			}
		}

		/// <summary>
		/// Obtain a single request for faculty status.
		/// </summary>
		/// <param name="username">The username.</param>
		/// <returns>An object containing information about the request.</returns>
		public static FacultyRequestInfo getFacultyRequest( string username ) {
			IDbCommand cmd = new SqlCommand();
			cmd.Connection = new SqlConnection( Globals.UsersConnectionString );
			cmd.CommandText = "SELECT Date, Name, Affiliation, Proof FROM FacultyRequests " +
				"WHERE UserName = @UserName";
			cmd.Parameters.Add( new SqlParameter( "@UserName", username ) );
			
			FacultyRequestInfo retVal = null;
			IDataReader reader = null;

			try {
				cmd.Connection.Open();
				reader = cmd.ExecuteReader( CommandBehavior.CloseConnection );

				if ( reader.Read() ) {
					retVal = new FacultyRequestInfo();
					retVal.UserName = username;
					retVal.Date = Convert.ToDateTime( reader["Date"] );
					retVal.Name = Convert.ToString( reader["Name"] );
					retVal.Affiliation = Convert.ToString( reader["Affiliation"] );
					retVal.Proof = Convert.ToString( reader["Proof"] );
				}
			} 
			catch ( SqlException e ) {
				throw new Exception( "An error occurred obtaining your " +
					"faculty request status.  Please try again.", e );
			} 
			finally {
				reader.Close();
			}

			return retVal;
		}

		/// <summary>
		/// Get a list of all faculty requests ordered by date.
		/// </summary>
		/// <returns>The list of faculty requests.</returns>
		public static IList getFacultyRequests() 
		{
			IDbCommand cmd = new SqlCommand();
			cmd.Connection = new SqlConnection( Globals.UsersConnectionString );
			cmd.CommandText = "SELECT UserName, Date, Name, Affiliation, Proof FROM FacultyRequests " +
				"ORDER BY Date";
			
			IList retVal = new ArrayList();
			IDataReader reader = null;

			try 
			{
				cmd.Connection.Open();
				reader = cmd.ExecuteReader( CommandBehavior.CloseConnection );

				while ( reader.Read() ) 
				{
					FacultyRequestInfo fri = null;
					fri = new FacultyRequestInfo();
					fri.UserName = Convert.ToString( reader["UserName"] );
					fri.Date = Convert.ToDateTime( reader["Date"] );
					fri.Name = Convert.ToString( reader["Name"] );
					fri.Affiliation = Convert.ToString( reader["Affiliation"] );
					fri.Proof = Convert.ToString( reader["Proof"] );
					retVal.Add( fri );
				}
			} 
			catch ( SqlException e ) 
			{
				throw new Exception( "An error occurred obtaining the " +
					"faculty requests.  ", e );
			} 
			finally 
			{
				reader.Close();
			}

			return retVal;
		}

		/// <summary>
		/// Determine if there is a faculty request with the given
		/// name.
		/// </summary>
		/// <param name="id">The name to search for.</param>
		/// <returns>True if a faculty request is found.</returns>
		public static bool facultyRequestExists( string name ) 
		{
			bool retVal = false;

			IDbCommand cmd = new SqlCommand();
			cmd.Connection = new SqlConnection( Globals.UsersConnectionString );
			cmd.Parameters.Add( new SqlParameter( "@Name", name ) );
			cmd.CommandText = "SELECT Name FROM FacultyRequests " +
				"WHERE Name = @Name";

			IDataReader reader = null;

			try 
			{
				cmd.Connection.Open();
				reader = cmd.ExecuteReader( CommandBehavior.CloseConnection );

				retVal = reader.Read();
			} 
			finally 
			{
				reader.Close();
			}

			return retVal;
		}

	}
}
