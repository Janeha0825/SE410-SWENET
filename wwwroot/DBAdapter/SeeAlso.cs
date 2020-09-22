using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;

namespace SwenetDev.DBAdapter {
	/// <summary>
	/// An adapter to interact with the database table containing modules
	/// that this module references.
	/// </summary>
	public class SeeAlso {

		/// <summary>
		/// Encapsulates SWENET module See Also information.
		/// </summary>
		[Serializable()]
		public class SeeAlsoInfo : Info {
			private string desc;
			private string altModule;
			private int orderID;

			public string Description {
				get { return desc; }
				set { desc = value;	}
			}
			public string AltModule {
				get { return altModule;	}
				set { altModule = value; }
			}
			public int OrderId {
				get { return orderID; }
				set { orderID = value; }
			}
			public string Title {
				get {
					string title = "(Module does not exist yet)";
					int baseID = Modules.getBaseIDfromIdentifier( altModule );
					if( baseID != -1 )
						title = Modules.getModuleCurrentVersion( baseID ).Title;
					return title;
				}
			}
			public int ModuleID {
				get {
					//return Modules.getBaseIDfromIdentifier( AltModule );
					IList testing = Modules.getModuleIDs(AltModule, 3);
					int mID = 0;
					if (testing.Count > 0) {

						if (testing.Count > 1) {

							for (int index = 0; index < testing.Count; index++) {
 
								if (mID < (int)testing[index]) mID = (int)testing[index];

							}

						}
						else {

							mID = (int)testing[0];

						}

					}

					return mID;

						
				}
			}

			public SeeAlsoInfo( string desc, string altModule ) {
				this.desc = desc;
				this.altModule = altModule;
				orderID = 0;
			}
		}

		/// <summary>
		/// Add all See Also modules to a module.
		/// </summary>
		/// <param name="moduleID">The module to which the See Also modules should
		/// be added.</param>
		/// <param name="resourcesList">The list of See Also modules to add.</param>
		public static void addAll( int moduleID, IList seeAlsoList ) {
										
			SqlCommand command = new SqlCommand();
			SqlParameter moduleIDParam = new SqlParameter("@ModuleID", SqlDbType.Int, 4, "ModuleID");
			SqlParameter descParam = new SqlParameter("@Description", SqlDbType.VarChar);
			SqlParameter identParam = new SqlParameter("@Identifier", SqlDbType.VarChar);
			SqlParameter orderIDParam = new SqlParameter("@OrderID", SqlDbType.Int, 4, "OrderID");

			command.Connection = new SqlConnection( ConfigurationSettings.AppSettings["ConnectionString"] );
			command.CommandText =
				"INSERT INTO SeeAlso(ModuleID, Description, AltModuleIdentifier, OrderID) " +
				"VALUES (@ModuleID, @Description, @Identifier, @OrderID)";

			moduleIDParam.Value = moduleID;
			command.Parameters.Add( moduleIDParam );
			command.Parameters.Add( descParam );
			command.Parameters.Add( identParam );
			command.Parameters.Add( orderIDParam );
			
			try {
				command.Connection.Open();

				for ( int i = 0; i < seeAlsoList.Count; i++ ) {
					SeeAlsoInfo sai = (SeeAlsoInfo)seeAlsoList[i];
					descParam.Value = sai.Description;
					identParam.Value = sai.AltModule;
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
		/// Obtain a list of all the See Also modules for the specified module.
		/// </summary>
		/// <param name="moduleID">The module for which to obtain See Also modules.</param>
		/// <returns>A list of See Also modules for the given module.</returns>
		public static IList getAll( int moduleID ) {
			SqlCommand sqlSelectCommand = new SqlCommand();
			sqlSelectCommand.Connection = new SqlConnection( ConfigurationSettings.AppSettings["ConnectionString"] );
			sqlSelectCommand.CommandText = "SELECT Description, AltModuleIdentifier "
				+ "FROM SeeAlso WHERE ModuleID = @ModuleID ORDER BY OrderID";
			sqlSelectCommand.Parameters.Add( new SqlParameter( "@ModuleID", moduleID ) );

			IList seeAlsoList = null;
			SqlDataReader reader = null;

			try 
			{
				sqlSelectCommand.Connection.Open();
				reader = sqlSelectCommand.ExecuteReader( CommandBehavior.CloseConnection );

				seeAlsoList = new ArrayList();

				while ( reader.Read() ) {
					seeAlsoList.Add( new SeeAlsoInfo( reader.GetString( 0 ), reader.GetString( 1 ) ) );
				}
			} catch ( SqlException e ) {
				throw;
			} finally {
				reader.Close();
			}

			return seeAlsoList;
		}

		/// <summary>
		/// Remove all other See Also modules from the given module.
		/// </summary>
		/// <param name="moduleID">The module from which to remove
		/// the other See Also modules.</param>
		public static void removeAll( int moduleID ) {
			SqlConnection dbConnection =
				new SqlConnection( ConfigurationSettings.AppSettings["ConnectionString"] );
			IDbCommand dbCommand = new SqlCommand();

			dbCommand.CommandText = "DELETE FROM SeeAlso WHERE ModuleID = @ModuleID";
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
