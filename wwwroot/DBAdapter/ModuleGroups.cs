using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace SwenetDev.DBAdapter {
	/// <summary>
	/// An adapter to interact with the database table for SWENET module
	/// groups.  Modules are "grouped" when a variant is created and are
	/// related by BaseID.  If A is a variant of B, then A and B are in
	/// the same group.  All future versions of A and B are also in the
	/// same group.
	/// </summary>
	public class ModuleGroups {
		/// <summary>
		/// Get the group id for a module.
		/// </summary>
		/// <param name="baseID">The baseID associated with the groupID.</param>
		/// <returns>Group id or -1 if not found.</returns>
		public static int getGroupID( int baseID ) {
			IDbCommand cmd = new SqlCommand();
			cmd.Connection = new SqlConnection( Globals.ConnectionString );
			cmd.CommandText = "SELECT GroupID FROM ModuleGroups " +
				"WHERE BaseID = @BaseID";
			cmd.Parameters.Add( new SqlParameter( "@BaseID", baseID ) );

			int groupID = -1;

			try {
				cmd.Connection.Open();
				object o = cmd.ExecuteScalar();

				if ( o != null ) {
					groupID = (int)o;
				}
			} catch ( SqlException e ) {
				throw new Exception( "An error occurred while getting the group ID for a module.", e );
			} finally {
				cmd.Connection.Close();
			}

			return groupID;
		}

		/// <summary>
		/// Add a module to a new group.
		/// </summary>
		/// <param name="baseID">The baseID of the new module.</param>
		public static void addToNew( int baseID ) {
			addToExisting( baseID, -1 );
		}

		/// <summary>
		/// Add a module to an existing group.
		/// </summary>
		/// <param name="baseID">The baseID of the module.</param>
		/// <param name="groupID">The groupID of the module. (if -1, makes a new group)</param>
		public static void addToExisting( int newBaseID, int oldBaseID ) {
			IDbCommand cmd = new SqlCommand();
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Connection = new SqlConnection( Globals.ConnectionString );
			cmd.CommandText = "AddModuleBaseToGroup";
			cmd.Parameters.Add( new SqlParameter( "@NewBaseID", newBaseID ) );

			if ( oldBaseID != -1 ) {
				cmd.Parameters.Add( new SqlParameter( "@OldBaseID", oldBaseID ) );
			}

			try {
				cmd.Connection.Open();
				cmd.ExecuteNonQuery();
			} catch ( SqlException e ) {
				throw new Exception( "An error occurred while adding your module to a group." + e.Message, e );
			} finally {
				cmd.Connection.Close();
			}
		}

		/// <summary>
		/// Obtain a list of modules related to the given baseID, that is,
		/// modules in the same group as baseID.
		/// </summary>
		/// <param name="baseID">
		/// The base ID for which to obtain related modules.
		/// </param>
		/// <returns>
		/// A list of modules related to the given baseID.
		/// </returns>
		public static IList getRelatedModules( int baseID ) {
			IDbCommand cmd = new SqlCommand();
			cmd.Connection = new SqlConnection( Globals.ConnectionString );
			cmd.CommandText = "SELECT * FROM ModulesDetailView INNER JOIN " +
				"ModuleGroups ON ModulesDetailView.BaseID = ModuleGroups.BaseID AND ModuleGroups.BaseID <> @BaseID AND ModuleGroups.GroupID = " +
				"(SELECT GroupID FROM ModuleGroups WHERE ModuleGroups.BaseID = @BaseID) AND ModulesDetailView.Status = " + Convert.ToString( (int)ModuleStatus.Approved );

			cmd.Parameters.Add( new SqlParameter( "@BaseID", baseID ) );

			IList list = null;

			try {
				list = Modules.executeGetModulesCommand( cmd );
			} catch ( SqlException e ) {
				throw new Exception( "Error getting related modules. ", e );
			} finally {
				cmd.Connection.Close();
			}

			return list;
		}
	}
}
