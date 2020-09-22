using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;

namespace SwenetDev.DBAdapter {
	/// <summary>
	/// An adapter to interact with the database table containing the
	/// SEEK categories to which each SWENET module pertains.
	/// </summary>
	public class Categories {
		private static IList areasList;
		/// <summary>
		/// Encapsulates information for a SWENET category.
		/// </summary>
		[Serializable()]
			public class CategoryInfo : Info {
			private string category = "";
			private string abbrev = "";
			private string text = "";
			private static CategoryInfo empty =	new CategoryInfo();
			private int forumID = 0;

			public static CategoryInfo Empty {
				get { return empty; }
			}

			/// <summary>
			/// The SEEK category abbreviation in the form:
			/// 
			///     AREA.unit
			/// </summary>
			public string Abbreviation {
				get { return abbrev; }
				set { abbrev = value; }
			}
			/// <summary>
			/// The SEEK category text description.
			/// </summary>
			public string Text {
				get { return text; }
				set { text = value; }
			}
			/// <summary>
			/// The long text for this SEEK category, including the text description
			/// and the abbreviation in the form:
			/// 
			///     Text (Abbreviation)
			/// </summary>
			public string LongText {
				get {
					// If it's the empty category, return "<None>".
					if ( id == 0 && text == String.Empty && abbrev == String.Empty ) {
						return "<None>";
					} else {
						return text + " (" + abbrev + ")";
					}
				}
			}

			/// <summary>
			/// The forum ID where modules of this category can be discussed.
			/// </summary>
			public int ForumID { 
				get { return forumID; }
				set { forumID = value; }
			}
		}

		/// <summary>
		/// Obtain all the categories for a given module.
		/// </summary>
		/// <param name="moduleID">The identifier of the module for which to
		/// obtain its categories.</param>
		/// <returns>The categories to which the given module pertains.</returns>
		public static IList getAll( int moduleID ) {
			SqlCommand command = new SqlCommand();
			command.Connection = new SqlConnection( Globals.ConnectionString );
			command.CommandText =
				"SELECT DISTINCT SEEKLookups.CategoryID, Abbrev, Text, ForumID FROM SEEKLookups " +
				"INNER JOIN SEEKCategories " +
				"ON SEEKCategories.ModuleID = @ModuleID " +
					"AND SEEKLookups.CategoryID = SEEKCategories.CategoryID";
			command.Parameters.Add( new SqlParameter( "@ModuleID", moduleID ) );

			IList categories = null;
			SqlDataReader reader = null;

			try {
				command.Connection.Open();
				reader = command.ExecuteReader( CommandBehavior.CloseConnection );

				categories = new ArrayList();

				while ( reader.Read() ) {
					categories.Add( buildCategoryInfo( reader ) );
				}
			} catch ( SqlException e ) {
				throw;
			} finally {
				reader.Close();
			}

			return categories;
		}

		/// <summary>
		/// Obtain a list of all SEEK areas.
		/// </summary>
		/// <returns>The list of SEEK areas.</returns>
		public static IList getSEEKAreas() {
			SqlCommand command = new SqlCommand();
			command.Connection = new SqlConnection( Globals.ConnectionString );
			command.CommandText =
				"SELECT DISTINCT SEEKLookups.CategoryID, Abbrev, Text, ForumID "
				+ "FROM SEEKLookups INNER JOIN "
				+ "SEEKAreaUnitLink ON SEEKLookups.CategoryID = SEEKAreaUnitLink.AreaID";

			IList areasList = null;
			SqlDataReader reader = null;

			try {
				command.Connection.Open();
				reader = command.ExecuteReader( CommandBehavior.CloseConnection );

				areasList = new ArrayList();

				while ( reader.Read() ) {
					areasList.Add( buildCategoryInfo( reader ) );
				}
			} catch ( SqlException e ) {
				throw;
			} finally {
				reader.Close();
			}
			

			return areasList;
		}

		/// <summary>
		/// Obtain the SEEK units for the given SEEK area.
		/// </summary>
		/// <param name="areaID">The area identifier for which to obtain SEEK
		/// units.</param>
		/// <returns>A list of SEEK units for the desired area.</returns>
		public static IList getSEEKUnits( int areaID ) {
			SqlCommand command = new SqlCommand();
			command.Connection = new SqlConnection( Globals.ConnectionString );
			command.CommandText =
				"SELECT SEEKLookups.CategoryID, Abbrev, Text, ForumID "
				+ "FROM SEEKLookups INNER JOIN "
				+ "SEEKAreaUnitLink ON SEEKAreaUnitLink.AreaID = @AreaID "
				+ "AND SEEKLookups.CategoryID = SEEKAreaUnitLink.UnitID";
			SqlParameter areaParam = new SqlParameter( "@AreaID", areaID );
			command.Parameters.Add( areaParam );

			IList unitsCollection = null;
			SqlDataReader reader = null;
			
			try 
			{
				command.Connection.Open();
				reader = command.ExecuteReader( CommandBehavior.CloseConnection );

				unitsCollection = new ArrayList();

				while ( reader.Read() ) {
					unitsCollection.Add( buildCategoryInfo( reader ) );
				}
			} catch ( SqlException e ) {
				throw;
			} finally {
				reader.Close();
			}

			return unitsCollection;
		}

		/// <summary>
		/// Add all of the categories to the specified module.
		/// </summary>
		/// <param name="moduleID">The module to which the categories should be added.</param>
		/// <param name="categories">The categories to add.</param>
		public static void addAll( int moduleID, IList categories ) {
			SqlCommand command = new SqlCommand();
			SqlParameter moduleIDParam = new SqlParameter("@ModuleID", SqlDbType.Int, 4, "ModuleID");
			SqlParameter categoryIDParam = new SqlParameter("@CategoryID", SqlDbType.Int, 4, "CategoryID");

			command.Connection = new SqlConnection( Globals.ConnectionString );
			command.CommandText =
				"INSERT INTO SEEKCategories(ModuleID, CategoryID) VALUES (@ModuleID, @CategoryID)";
			moduleIDParam.Value = moduleID;
			command.Parameters.Add( moduleIDParam );
			command.Parameters.Add( categoryIDParam );

			try {
				command.Connection.Open();

				foreach ( CategoryInfo ci in categories ) {
					categoryIDParam.Value = ci.Id;
					command.ExecuteNonQuery();
				}
			} catch ( SqlException e ) {
				throw;
			} finally {
				command.Connection.Close();
			}

		}

		/// <summary>
		/// Obtain an object encapsulating information about the specified
		/// category.
		/// </summary>
		/// <param name="categoryID">The category identifier for which to obtain an
		/// information object.</param>
		/// <returns>The category information object.</returns>
		public static CategoryInfo getCategoryInfo( int categoryID ) {
			SqlCommand command = new SqlCommand();
			command.Connection = new SqlConnection( Globals.ConnectionString );
			command.CommandText =
				"SELECT CategoryID, Abbrev, Text, ForumID FROM SEEKLookups WHERE CategoryID = @CategoryID";
			SqlParameter categoryParam = new SqlParameter( "@CategoryID", categoryID );
			command.Parameters.Add( categoryParam );


			CategoryInfo retVal = null;
			SqlDataReader reader = null;

			try 
			{
				command.Connection.Open();
				reader = command.ExecuteReader( CommandBehavior.CloseConnection );


				if ( reader.Read() ) {
					retVal = buildCategoryInfo( reader );
				}
			} catch ( SqlException e ) {
				throw;
			} finally {
				reader.Close();
			}

			return retVal;
		}

		/// <summary>
		/// Remove all SEEK categories from the given module.
		/// </summary>
		/// <param name="moduleID">The module from which to remove
		/// the SEEK categories.</param>
		public static void removeAll( int moduleID ) {
			SqlConnection dbConnection =
				new SqlConnection( Globals.ConnectionString );
			IDbCommand dbCommand = new SqlCommand();

			dbCommand.CommandText = "DELETE FROM SEEKCategories WHERE ModuleID = @ModuleID";
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

		/// <summary>
		/// Creates and returns a CategoryInfo object which is created by getting
		/// data from the IDataReader.
		/// </summary>
		/// <param name="reader">The IDataReader associated with the SQL query.</param>
		/// <returns>The CategoryInfo object which was created by data from
		/// the passed IDataReader.</returns>
		private static CategoryInfo buildCategoryInfo( IDataReader reader ) {
			CategoryInfo retVal = new CategoryInfo();

			retVal = new CategoryInfo();
			retVal.Id = Convert.ToInt32( reader["CategoryID"] );
			retVal.Abbreviation = Convert.ToString( reader["Abbrev"] );
			retVal.Text = Convert.ToString( reader["Text"] );
			retVal.ForumID = Convert.ToInt32( reader["ForumID"] );

			return retVal;
		}
	}
}
