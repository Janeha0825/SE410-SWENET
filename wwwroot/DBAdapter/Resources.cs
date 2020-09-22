using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;

namespace SwenetDev.DBAdapter {
	/// <summary>
	/// An adapter to interact with the database table containing SWENET
	/// module resources.
	/// </summary>
	public class Resources {

		/// <summary>
		/// Encapsulates SWENET module resources information.
		/// </summary>
		[Serializable()]
		public class ResourceInfo : Info {
			private string text;
			private string link;
			private int orderID;

			public string Text {
				get {
					return text;
				}
				set {
					text = value;
				}
			}
			public string Link {
				get {
					return link;
				}
				set {
					link = value;
				}
			}
			public int OrderId {
				get {
					return orderID;
				}
				set {
					orderID = value;
				}
			}

			public ResourceInfo( string text, string link ) {
				this.text = text;
				this.link = link;
				orderID = 0;
			}
		}

		/// <summary>
		/// Add all resources to a module.
		/// </summary>
		/// <param name="moduleID">The module to which the resources should
		/// be added.</param>
		/// <param name="resourcesList">The list of resources to add.</param>
		public static void addAll( int moduleID, IList resourcesList ) {
			SqlCommand command = new SqlCommand();
			SqlParameter moduleIDParam = new SqlParameter("@ModuleID", SqlDbType.Int, 4, "ModuleID");
			SqlParameter descParam = new SqlParameter("@Description", SqlDbType.VarChar);
			SqlParameter linkParam = new SqlParameter("@Link", SqlDbType.VarChar);
			SqlParameter orderIDParam = new SqlParameter("@OrderID", SqlDbType.Int, 4, "OrderID");

			command.Connection = new SqlConnection( ConfigurationSettings.AppSettings["ConnectionString"] );
			command.CommandText =
				"INSERT INTO OtherResources(ModuleID, ResourceDescriptiveText, ResourceLink, OrderID) " +
				"VALUES (@ModuleID, @Description, @Link, @OrderID)";

			moduleIDParam.Value = moduleID;
			command.Parameters.Add( moduleIDParam );
			command.Parameters.Add( descParam );
			command.Parameters.Add( linkParam );
			command.Parameters.Add( orderIDParam );
			
			try {
				command.Connection.Open();

				for ( int i = 0; i < resourcesList.Count; i++ ) {
					ResourceInfo ri = (ResourceInfo)resourcesList[i];
					descParam.Value = ri.Text;
					linkParam.Value = ri.Link;
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
		/// Obtain a list of all the resources for the specified module.
		/// </summary>
		/// <param name="moduleID">The module for which to obtain resources.</param>
		/// <returns>A list of resources for the given module.</returns>
		public static IList getAll( int moduleID ) {
			SqlCommand sqlSelectCommand = new SqlCommand();
			sqlSelectCommand.Connection = new SqlConnection( ConfigurationSettings.AppSettings["ConnectionString"] );
			sqlSelectCommand.CommandText = "SELECT ResourceDescriptiveText, ResourceLink "
				+ "FROM OtherResources WHERE ModuleID = @ModuleID ORDER BY OrderID";
			sqlSelectCommand.Parameters.Add( new SqlParameter( "@ModuleID", moduleID ) );

			IList resourcesCollection = null;
			SqlDataReader reader = null;

			try 
			{
				sqlSelectCommand.Connection.Open();
				reader = sqlSelectCommand.ExecuteReader( CommandBehavior.CloseConnection );

				resourcesCollection = new ArrayList();

				while ( reader.Read() ) {
					// The link field may be null, if so make it an empty string.
					string link = "";
					if ( !reader.IsDBNull( 1 ) ) {
						link = reader.GetString( 1 );
					}

					resourcesCollection.Add( new ResourceInfo( reader.GetString( 0 ), link ) );
				}
			} catch ( SqlException e ) {
				throw;
			} finally {
				reader.Close();
			}

			return resourcesCollection;
		}

		/// <summary>
		/// Remove all other resources  from the given module.
		/// </summary>
		/// <param name="moduleID">The module from which to remove
		/// the other resources.</param>
		public static void removeAll( int moduleID ) {
			SqlConnection dbConnection =
				new SqlConnection( ConfigurationSettings.AppSettings["ConnectionString"] );
			IDbCommand dbCommand = new SqlCommand();

			dbCommand.CommandText = "DELETE FROM OtherResources WHERE ModuleID = @ModuleID";
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
