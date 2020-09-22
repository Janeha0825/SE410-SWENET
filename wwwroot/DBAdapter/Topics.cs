using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;

namespace SwenetDev.DBAdapter {
	/// <summary>
	/// An adapter to interact with the database table containing topics of
	/// SWENET modules.
	/// </summary>
	public class Topics {

		/// <summary>
		/// Encapsulates SWENET module topic information.
		/// </summary>
		[Serializable()]
		public class TopicInfo : Info {
			private string text;
			private int orderID;

			/// <summary>
			/// The text of the topic.
			/// </summary>
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

			public TopicInfo( string text ) {
				this.text = text;
				orderID = 0;
			}
			public TopicInfo( string text, int orderID ) {
				this.text = text;
				this.orderID = orderID;
			}
		}

		/// <summary>
		/// Adds all of the passed TopicInfo objects to the list of topics for the
		/// module associated with the given moduleID.
		/// </summary>
		/// <param name="moduleID">The moduleID of the module to be changed.</param>
		/// <param name="topics">The topics to add.</param>
		public static void addAll( int moduleID, IList topics ) {
			SqlCommand command = new SqlCommand();
			SqlParameter moduleIDParam = new SqlParameter("@ModuleID", SqlDbType.Int, 4, "ModuleID");
			SqlParameter topicParam = new SqlParameter("@TopicText", SqlDbType.VarChar);
			SqlParameter orderIDParam = new SqlParameter("@OrderID", SqlDbType.Int, 4, "OrderID");

			command.Connection = new SqlConnection( Globals.ConnectionString );
			command.CommandText =
				"INSERT INTO Topics(ModuleID, TopicText, OrderID) VALUES (@ModuleID, @TopicText, @OrderID)";
			moduleIDParam.Value = moduleID;
			command.Parameters.Add( moduleIDParam );
			command.Parameters.Add( topicParam );
			command.Parameters.Add( orderIDParam );
			
			try {
				command.Connection.Open();

				for ( int i = 0; i < topics.Count; i++ ) {
					topicParam.Value = ((TopicInfo)topics[i]).Text;
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
		/// Obtain a list of all the topics for a given module.
		/// </summary>
		/// <param name="moduleID">The module for which to obtain topics.</param>
		/// <returns>An ordered list of the topics for the desired module.</returns>
		public static IList getAll( int moduleID ) {
			SqlCommand sqlSelectCommand = new SqlCommand();
			sqlSelectCommand.Connection = new SqlConnection( Globals.ConnectionString );
			sqlSelectCommand.CommandText = "SELECT TopicText, OrderID FROM Topics Where ModuleID = @ModuleID ORDER BY OrderID";
			sqlSelectCommand.Parameters.Add( new SqlParameter( "@ModuleID", moduleID ) );

			IList topicsCollection = new ArrayList();
			SqlDataReader reader = null;

			try {
				sqlSelectCommand.Connection.Open();
				reader = sqlSelectCommand.ExecuteReader( CommandBehavior.CloseConnection );

				while ( reader.Read() ) {
					topicsCollection.Add( new TopicInfo( reader.GetString( 0 ), reader.GetInt32( 1 ) ) );
				}
			} catch ( SqlException e ) {
				throw;
			} finally {
				reader.Close();
			}

			return topicsCollection;
		}

		/// <summary>
		/// Remove all topics from the given module.
		/// </summary>
		/// <param name="moduleID">The module from which to remove
		/// the topics.</param>
		public static void removeAll( int moduleID ) {
			SqlConnection dbConnection =
				new SqlConnection( Globals.ConnectionString );
			IDbCommand dbCommand = new SqlCommand();

			dbCommand.CommandText = "DELETE FROM Topics WHERE ModuleID = @ModuleID";
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
