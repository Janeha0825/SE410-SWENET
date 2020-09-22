using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace SwenetDev.DBAdapter {
	/// <summary>
	/// Summary description for ModuleCheckouts.
	/// </summary>
	public class ModuleCheckouts {
		/// <summary>
		/// Add an entry for the given module and user.
		/// </summary>
		/// <param name="moduleID">The identifier of the module.</param>
		/// <param name="username">The user submitting the module.</param>
		public static void add( int moduleID, string username ) {
			IDbCommand cmd = new SqlCommand();
			cmd.Connection =
				new SqlConnection( ConfigurationSettings.AppSettings["ConnectionString"] );
			cmd.CommandText = "INSERT INTO ModuleCheckouts " +
				"(ModuleID, UserName) VALUES (@ModuleID, @UserName)";

			cmd.Parameters.Add( new SqlParameter( "@ModuleID", moduleID ) );
			cmd.Parameters.Add( new SqlParameter( "@UserName", username ) );

			try {
				cmd.Connection.Open();
				cmd.ExecuteNonQuery();
			} catch ( SqlException ex ) {
				throw;
			} finally {
				cmd.Connection.Close();
			}
		}

		public static string getSubmitter( int moduleID ) {
			IDbCommand cmd = new SqlCommand();
			cmd.Connection =
				new SqlConnection( Globals.ConnectionString );
			cmd.CommandText = "SELECT UserName FROM ModuleCheckouts " +
				"WHERE ModuleID = @ModuleID";

			cmd.Parameters.Add( new SqlParameter( "@ModuleID", moduleID ) );

			string retVal = null;

			try {
				cmd.Connection.Open();
				retVal = (string)cmd.ExecuteScalar();
			} catch ( SqlException ex ) {
				throw new Exception( "Error getting submitter. ", ex );
			} finally {
				cmd.Connection.Close();
			}

			return retVal;
		}
	}
}
