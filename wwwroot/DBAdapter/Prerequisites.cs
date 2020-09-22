using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;

namespace SwenetDev.DBAdapter {
	/// <summary>
	/// An adapter to interact with the database table containing SWENET module
	/// prerequisites.
	/// </summary>
	public class Prerequisites {

		/// <summary>
		/// Encapsulates information associated with SWENET module prerequisites.
		/// </summary>
		[Serializable()]
		public class PrereqInfo : Info {
			private string text;
			private int orderID;

			public string Text {
				get {
					return text;
				}
				set {
					text = value;
				}
			}

			/// <summary>
			/// The order identifer.
			/// </summary>
			public int OrderId {
				get {
					return orderID;
				}
				set {
					orderID = value;
				}
			}

			public PrereqInfo( string text ) {
				this.text = text;
				orderID = 0;
			}
		}

		/// <summary>
		/// Add all the prerequisites in prereqsList to the module specified
		/// by moduleID.
		/// </summary>
		/// <param name="moduleID"></param>
		/// <param name="prereqsList"></param>
		public static void addAll( int moduleID, IList prereqsList ) {
			SqlCommand command = new SqlCommand();
			SqlParameter moduleIDParam = new SqlParameter("@ModuleID", SqlDbType.Int, 4, "ModuleID");
			SqlParameter prereqTextParam = new SqlParameter("@PrereqText", SqlDbType.VarChar);
			SqlParameter orderIDParam = new SqlParameter("@OrderID", SqlDbType.Int, 4, "OrderID");

			command.Connection = new SqlConnection( Globals.ConnectionString );
			command.CommandText =
				"INSERT INTO Prereqs(ModuleID, PrerequisiteText, OrderID) " +
				"VALUES (@ModuleID, @PrereqText, @OrderID)";
			moduleIDParam.Value = moduleID;
			command.Parameters.Add( moduleIDParam );
			command.Parameters.Add( prereqTextParam );
			command.Parameters.Add( orderIDParam );
			
			try {
				command.Connection.Open();

				for ( int i = 0; i < prereqsList.Count; i++ ) {
					PrereqInfo pi = (PrereqInfo)prereqsList[i];
					prereqTextParam.Value = pi.Text;
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
		/// Get all the prerequisites for the module specified by moduleID.
		/// </summary>
		/// <param name="moduleID"></param>
		/// <returns></returns>
		public static IList getAll( int moduleID ) {
			SqlCommand sqlSelectCommand = new SqlCommand();
			sqlSelectCommand.Connection = new SqlConnection( Globals.ConnectionString );
			sqlSelectCommand.CommandText = "SELECT PrerequisiteText FROM Prereqs " +
				"WHERE ModuleID = @ModuleID ORDER BY OrderID";
			sqlSelectCommand.Parameters.Add( new SqlParameter( "@ModuleID", moduleID ) );

			IList prereqsCollection = null;
			SqlDataReader reader = null;
			
			try {
				sqlSelectCommand.Connection.Open();
				reader = sqlSelectCommand.ExecuteReader( CommandBehavior.CloseConnection );

				prereqsCollection = new ArrayList();

				while ( reader.Read() ) {
					prereqsCollection.Add( new PrereqInfo( reader.GetString( 0 ) ) );
				}
			} catch ( SqlException e ) {
				throw;
			} finally {
				reader.Close();
			}

			return prereqsCollection;
		}

		/// <summary>
		/// Remove all prerequisites from the given module.
		/// </summary>
		/// <param name="moduleID">The module from which to remove
		/// the prerequisites.</param>
		public static void removeAll( int moduleID ) {
			SqlConnection dbConnection =
				new SqlConnection( Globals.ConnectionString );
			IDbCommand dbCommand = new SqlCommand();

			dbCommand.CommandText = "DELETE FROM Prereqs WHERE ModuleID = @ModuleID";
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
