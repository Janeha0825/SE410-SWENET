using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;

namespace SwenetDev.DBAdapter {
	/// <summary>
	/// An adapter to interact with the database table containing SWENET
	/// module objectives.
	/// </summary>
	public class Objectives {

		/// <summary>
		/// Encapsulates information of a SWENET module objective.
		/// </summary>
		[Serializable()]
		public class ObjectiveInfo : Info {
			private string level;
			private string text;

			public string BloomLevel {
				get {
					return level;
				}
				set {
					level = value;
				}
			}
			public string Text {
				get {
					return text;
				}
				set {
					text = value;
				}
			}

			public ObjectiveInfo( string level, string text ) {
				this.level = level;
				this.text = text;
			}
		}

		/// <summary>
		/// Add a list of objectives for a particular module.
		/// </summary>
		/// <param name="moduleID">The module the objectives should be identified with.</param>
		/// <param name="objectivesList">The objectives to add for the given module.</param>
		public static void addAll( int moduleID, IList objectivesList ) {
			SqlCommand command = new SqlCommand();
			SqlParameter moduleIDParam = new SqlParameter("@ModuleID", SqlDbType.Int, 4, "ModuleID");
			SqlParameter bloomParam = new SqlParameter("@BloomLevel", SqlDbType.VarChar);
			SqlParameter textParam = new SqlParameter("@ObjectiveText", SqlDbType.VarChar);
			SqlParameter orderIDParam = new SqlParameter("@OrderID", SqlDbType.Int, 4, "OrderID");

			command.Connection = new SqlConnection( Globals.ConnectionString );
			command.CommandText =
				"INSERT INTO Objectives(ModuleID, BloomLevel, ObjectiveText, OrderID) " +
				"VALUES (@ModuleID, @BloomLevel, @ObjectiveText, @OrderID)";
			moduleIDParam.Value = moduleID;
			command.Parameters.Add( moduleIDParam );
			command.Parameters.Add( bloomParam );
			command.Parameters.Add( textParam );
			command.Parameters.Add( orderIDParam );
			
			try {
				command.Connection.Open();

				for ( int i = 0; i < objectivesList.Count; i++ ) {
					ObjectiveInfo oi = (ObjectiveInfo)objectivesList[i];
					bloomParam.Value = oi.BloomLevel;
					textParam.Value = oi.Text;
					orderIDParam.Value = i + 1;
					command.ExecuteNonQuery();
				}
			} catch ( SqlException e ) {
				throw;
			} finally {
				command.Connection.Close();
			}
		}

		/// <summary>
		/// Obtain a list of all objectives for a given module.
		/// </summary>
		/// <param name="moduleID">The module for which to obtain its objectives.</param>
		/// <returns>A list of objectives.</returns>
		public static IList getAll( int moduleID ) {
			SqlCommand sqlSelectCommand = new SqlCommand();
			sqlSelectCommand.Connection = new SqlConnection( Globals.ConnectionString );
			sqlSelectCommand.CommandText = "SELECT BloomLevel, ObjectiveText " +
				"FROM Objectives Where ModuleID = @ModuleID ORDER BY OrderID";
			sqlSelectCommand.Parameters.Add( new SqlParameter( "@ModuleID", moduleID ) );
			
			IList objectivesCollection = null;
			SqlDataReader reader = null;
			
			try {
				sqlSelectCommand.Connection.Open();
				reader = sqlSelectCommand.ExecuteReader( CommandBehavior.CloseConnection );
				objectivesCollection = new ArrayList();

				while ( reader.Read() ) {
					objectivesCollection.Add( new ObjectiveInfo( reader.GetString( 0 ), reader.GetString( 1 ) ) );
				}
			} catch ( SqlException e ) {
				throw;
			} finally {
				reader.Close();
			}

			return objectivesCollection;
		}

		/// <summary>
		/// Remove all objectives from the given module.
		/// </summary>
		/// <param name="moduleID">The module from which to remove
		/// the objectives.</param>
		public static void removeAll( int moduleID ) {
			SqlConnection dbConnection =
				new SqlConnection( Globals.ConnectionString );
			IDbCommand dbCommand = new SqlCommand();

			dbCommand.CommandText = "DELETE FROM Objectives WHERE ModuleID = @ModuleID";
			dbCommand.Connection = dbConnection;

			dbCommand.Parameters.Add( new SqlParameter( "@ModuleID", moduleID ) );

			try {
				dbCommand.Connection.Open();
				dbCommand.ExecuteNonQuery();
			} catch ( SqlException e ) {
				throw;
			} finally {
				dbCommand.Connection.Close();
			}
		}
	}
}
