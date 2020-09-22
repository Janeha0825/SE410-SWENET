using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace SwenetDev.DBAdapter {
	public enum ModuleStatus { InProgress = 0, PendingApproval = 1, Approved = 2, All = 3, PreviousVersion = 4 };
	public enum ModuleSearchType { Author = 0, Submitter = 1, Title = 2, ModuleIdentifier = 3, SubmitterIdentifier = 4 };
    /// <summary>
    /// An adapter to the module-related tables in the database.
    /// </summary>
	public class Modules 
	{
		/// <summary>
		/// Encapsulates information about a SWENET module.
		/// </summary>
		[Serializable()]
			public class ModuleInfo : Info, IComparable
		{
			private string title = "";
			private DateTime date = DateTime.Now;
			private string abst = "";
			private string lecture = "";
			private string lab = "";
			private string exercise = "";
			private string homework = "";
			private string other = "";
			private string authorComments = "";
			private ModuleStatus status = ModuleStatus.InProgress;
			private string submitter = "";
			private string submitterID = "";
			private int version = 1;
			private int baseID = 0;
			private string lockedBy = "";
			private string cIComments = "";

			public string Title 
			{
				get { return title;	}
				set { title = value; }
			}
			public DateTime Date 
			{
				get { return date; }
				set { date = value; }
			}
			public int Version 
			{
				get { return version; }
				set { version = value; }
			}
			public string Submitter 
			{
				get { return submitter; }
				set { submitter = value; }
			}
			public string SubmitterID 
			{
				get { return submitterID; }
				set { submitterID = value; }
			}
			public string LockedBy 
			{
				get { return lockedBy; }
				set { lockedBy = value; }
			}
			public string Abstract 
			{
				get { return abst; }
				set { abst = value; }
			}
			public string LectureSize 
			{
				get { return lecture; }
				set { lecture = value; }
			}
			public string LabSize 
			{
				get { return lab; }
				set { lab = value; }
			}
			public string ExerciseSize 
			{
				get { return exercise; }
				set { exercise = value; }
			}
			public string HomeworkSize 
			{
				get { return homework; }
				set { homework = value; }
			}
			public string OtherSize 
			{
				get { return other; }
				set { other = value; }
			}
			public string AuthorComments 
			{
				get { return authorComments; }
				set { authorComments = value; }
			}
			public ModuleStatus Status 
			{
				get { return status; }
				set { status = value; }
			}
			public int BaseId 
			{
				get { return baseID; }
				set { baseID = value; }
			}
			public bool IsRevision 
			{
				get { return Version > 1; }
			}
			public string CheckInComments 
			{
				get { return cIComments; }
				set { cIComments = value; }
			}
			public string ModuleIdentifier 
			{
				get {
					string retval = getModuleIdentifierByBaseID( BaseId );
					return retval == null ? "" : retval;
				}
			}

			//IComparable member CompareTo() used to sort ModuleInfo objects by title

			#region IComparable Members

			public int CompareTo(object obj)
			{
				//Defaults to compare modules by title
				ModuleInfo otherObj = (ModuleInfo)obj;

				return title.CompareTo(otherObj.Title);
			}

			#endregion
		}

		/// <summary>
		/// This method obtains the title for a given controls popup help from the DB
		/// </summary>
		/// <param name="controlID">the id # of the control to get the popup help title for</param>
		/// <returns>a string containing the title of the popup help file</returns>
		public static string[] getHelpContents(int controlID)
		{
			SqlCommand cmd = new SqlCommand();
			cmd.Connection = new SqlConnection( Globals.ConnectionString );
			cmd.CommandText = "SELECT * FROM PopupHelp WHERE Id = " + controlID;
			string[] results = new string[2];

			SqlDataReader myReader = null;

			try 
			{
				cmd.Connection.Open();
				myReader = cmd.ExecuteReader( CommandBehavior.CloseConnection );
				myReader.Read();
				results[0] = (string)myReader["Title"];
				results[1] = (string)myReader["Text"];
			} 
			catch( SqlException e )
			{
				throw;
			} 
			finally 
			{
				myReader.Close();
			}
			return results;
		}

		/// <summary>
		/// Obtain an integer with the maximum length of the data in a specific
		/// column in a specific table.
		/// </summary>
		/// <param name="table">The name of the table to search in.</param>
		/// <param name="column">The name of the column to get the max length of.</param>
		/// <returns>The max length of the data the column will hold.</returns>
		public static int getColumnMaxLength(string table, string column) 
		{
			SqlCommand cmd = new SqlCommand();
			cmd.Connection = new SqlConnection( Globals.ConnectionString );
			cmd.CommandText = "SELECT " + column + " FROM " + table;
			
			DataTable myTable;
			int retVal = -1;
			SqlDataReader myReader = null;

			try 
			{
				cmd.Connection.Open();
				myReader = cmd.ExecuteReader( CommandBehavior.CloseConnection );
				myTable = myReader.GetSchemaTable();
				retVal = (int)myTable.Rows[0]["ColumnSize"];
			} 
			catch ( SqlException e) 
			{
				throw;
			} 
			finally
			{
				myReader.Close();
			}
			return retVal;
		}

		/// <summary>
		/// Obtain an object encapsulating the information about the
		/// specified module.
		/// </summary>
		/// <param name="moduleID">The identifier of the module for which
		/// to obtain information.</param>
		/// <returns>The module information object for the desired module.</returns>
		public static ModuleInfo getModuleInfo( int moduleID ) {
			SqlCommand cmd = new SqlCommand();
			cmd.Connection = new SqlConnection( Globals.ConnectionString );
			cmd.CommandText = "SELECT * FROM ModulesDetailView Where ModuleID = @ModuleID";
			cmd.Parameters.Add(new SqlParameter("@ModuleID", moduleID));

			ModuleInfo retVal = null;
			SqlDataReader moduleInfo = null;

			try {
				cmd.Connection.Open();
				moduleInfo = cmd.ExecuteReader( CommandBehavior.CloseConnection );
				moduleInfo.Read();

				retVal = constructModuleInfo( moduleInfo );
			} catch ( SqlException e ) {
				throw;
			} finally {
				moduleInfo.Close();
			}

			return retVal;
		}

		/// <summary>
		///	Add a new module into the database.
		/// </summary>
		/// <returns>A unique identifier for the module just added.</returns>
		public static int addModule( ModuleInfo mi ) {
			IDbCommand dbCommand = new SqlCommand();
			dbCommand.CommandText = "AddModule";
			dbCommand.CommandType = CommandType.StoredProcedure;
			dbCommand.Connection = new SqlConnection( Globals.ConnectionString );

			addModuleInfoParams( dbCommand, mi );
			IDataParameter moduleIdParam = new SqlParameter("@ModuleID", SqlDbType.Int, 4);
			moduleIdParam.Direction = ParameterDirection.Output;
			dbCommand.Parameters.Add(moduleIdParam);

			int retVal = 0;

			try {
				dbCommand.Connection.Open();
				dbCommand.ExecuteNonQuery();
				retVal = (int)moduleIdParam.Value;
			} catch ( SqlException e ) {
			} finally {
				dbCommand.Connection.Close();
			}

			return retVal;
		}

		/// <summary>
		/// Update a module's information.
		/// </summary>
		/// <param name="mi">The new module information.</param>
		/// <param name="status">The new status.</param>
		public static void updateModule( ModuleInfo mi ) {
			SqlCommand dbCommand = new SqlCommand();
			dbCommand.CommandText = "UpdateModule";
			dbCommand.CommandType = CommandType.StoredProcedure;
			dbCommand.Connection = new SqlConnection( Globals.ConnectionString );

			addModuleInfoUpdateParams( dbCommand, mi );
			dbCommand.Parameters.Add( new SqlParameter("@ModuleID", mi.Id) );

			try {
				dbCommand.Connection.Open();
				dbCommand.ExecuteNonQuery();
			} catch ( SqlException e ) {
			} finally {
				dbCommand.Connection.Close();
			}
		}

		/// <summary>
		/// Updates the status of a module.
		/// </summary>
		/// <param name="moduleID">The module to update.</param>
		/// <param name="status">The new status for the module.</param>
		public static void updateModuleStatus( int moduleID, ModuleStatus status ) {
			SqlCommand cmd = new SqlCommand();
			cmd.Connection = new SqlConnection( Globals.ConnectionString );
			cmd.CommandText = "UPDATE Module SET Status = @Status " +
				"WHERE ModuleID = @ModuleID";

			cmd.Parameters.Add( "@ModuleID", moduleID );
			cmd.Parameters.Add( "@Status", (int)status );
			
			try {
				cmd.Connection.Open();
				cmd.ExecuteNonQuery();
			} catch ( SqlException e ) {
				throw;
			} finally {
				cmd.Connection.Close();
			}
		}

		/// <summary>
		/// Remove a module.  If the module is the first (and only) version, delete
		/// the base as well.  Otherwise, delete the particular version, and unlock
		/// the base (entire module).
		/// </summary>
		/// <param name="moduleID">The identifier of the module to remove.</param>
		public static void remove( int moduleID ) {
			SqlCommand cmd = new SqlCommand();
			cmd.Connection = new SqlConnection( Globals.ConnectionString );
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandText = "RemoveModule";
			cmd.Parameters.Add( new SqlParameter( "@ModuleID", moduleID ) );

			try {
				cmd.Connection.Open();
				cmd.ExecuteNonQuery();
			} catch ( SqlException e ) {
				throw;
			} finally {
				cmd.Connection.Close();
			}
		}

		/// <summary>
		/// Obtain a list of all modules with the given status.
		/// </summary>
		/// <param name="status">Status to filter the results.</param>
		/// <returns>List of modules with given status, or all modules if
		/// status is Status.All.</returns>
		public static IList getAll( ModuleStatus status ) {
			SqlCommand cmd = new SqlCommand();
			cmd.Connection = new SqlConnection( Globals.ConnectionString );
			cmd.CommandText = "SELECT * FROM ModulesDetailView";
			
			addStatusIfNeeded( cmd, status, " WHERE (Status = @Status)" );

			if ( status == ModuleStatus.PendingApproval ) {
				cmd.CommandText += " ORDER BY Date";
			}

			return executeGetModulesCommand( cmd );
		}

		/// <summary>
		/// Obtain a list of modules that fall under the given category.
		/// If the category is just a general SEEK area, the resulting
		/// modules list will include modules with SEEK units under
		/// that area as well.
		/// </summary>
		/// <param name="categoryID">The category identifer to search.</param>
		/// <returns>A list of modules matching the search category.</returns>
		public static IList getModulesByCategory( int categoryID, ModuleStatus status ) {
			SqlCommand command = new SqlCommand();
			command.Connection = new SqlConnection( Globals.ConnectionString );

			// NOTE: You can only select the ModuleID here because you can't
			// use DISTINCT when there are columns of type text (Abstract and
			// AuthorComments).
			command.CommandText = 
				"SELECT DISTINCT Module.ModuleID " +
				"FROM         Module INNER JOIN " +
				"             SEEKCategories ON Module.ModuleID = SEEKCategories.ModuleID " +
				"             CROSS JOIN SEEKAreaUnitLink " +
				"WHERE        (SEEKAreaUnitLink.AreaID = @CategoryID) " + 
				"             AND (SEEKCategories.CategoryID = SEEKAreaUnitLink.UnitID) ";
			addStatusIfNeeded( command, status, " AND Status = @Status" );

			command.CommandText += " OR (SEEKCategories.CategoryID = @CategoryID)";
			addStatusIfNeeded( command, status, " AND Status = @Status" );

			command.Parameters.Add( new SqlParameter( "@CategoryID", categoryID ) );
			
			IList modulesList = new ArrayList();
			IDataReader reader = null;

			try 
			{
				command.Connection.Open();
				reader = command.ExecuteReader( CommandBehavior.CloseConnection );

				while ( reader.Read() ) {
					modulesList.Add( getModuleInfo( reader.GetInt32( 0 ) ) );
				}
			} catch ( SqlException ex ) {
				throw;
			} finally {
				reader.Close();
			}

			return modulesList;
		}

		/// <summary>
		/// Obtain a list of a modules submitted by a given user.
		/// </summary>
		/// <param name="username">The username of the module's submitter.</param>
		/// <param name="status">The status of the modules returned.</param>
		/// <returns>
		/// A list of modules that the particular user has submitted
		/// of the given type.
		/// </returns>
		public static IList getModulesBySubmitter( string username, ModuleStatus status ) {
			SqlCommand cmd = new SqlCommand();
			cmd.Connection = 
				new SqlConnection( Globals.ConnectionString );

			cmd.CommandText = "SELECT * FROM ModulesDetailView " +
				"WHERE Submitter = @UserName";

			cmd.Parameters.Add( "@UserName", username );

			addStatusIfNeeded( cmd, status, " AND (Status = @Status)" );

			return executeGetModulesCommand( cmd );
		}

		/// <summary>
		/// Obtain a list of modules for which the given user is listed as an author.
		/// </summary>
		/// <param name="username">The username of a module's author.</param>
		/// <param name="status">The status of the modules returned.</param>
		/// <returns>
		/// A list of modules that the particular user has authored of
		/// the given type.
		/// </returns>
		public static IList getModulesByAuthor( string username, ModuleStatus status ) {
			SqlCommand cmd = new SqlCommand();
			cmd.Connection = 
				new SqlConnection( Globals.ConnectionString );

			cmd.CommandText = "SELECT * FROM ModulesDetailView " +
				"INNER JOIN " +
				"Author ON ModulesDetailView.ModuleID = Author.ModuleID " +
				"AND Author.UserName = @UserName";

			cmd.Parameters.Add( "@UserName", username );

			addStatusIfNeeded( cmd, status, " WHERE Status = @Status" );

			return executeGetModulesCommand( cmd );
		}

		/// <summary>
		/// Obtain a list of modules submitted or authored by the given user
		/// and with the given status.
		/// </summary>
		/// <param name="username">The username for author/submitter.</param>
		/// <param name="status">The module status.</param>
		/// <returns>A list of matching modules.</returns>
		public static IList getModulesByAuthorAndSubmitter( string username, ModuleStatus status ) {
			SqlCommand cmd = new SqlCommand();
			cmd.Connection = 
				new SqlConnection( Globals.ConnectionString );

			cmd.CommandText = "SELECT * FROM ModulesDetailView " +
				"INNER JOIN " +
				"Author ON ModulesDetailView.ModuleID = Author.ModuleID " +
				"AND Author.UserName = @UserName";

			cmd.Parameters.Add( "@UserName", username );

			addStatusIfNeeded( cmd, status, " WHERE Status = @Status" );

			return executeGetModulesCommand( cmd );
		}

		/// <summary>
		/// Returns the BaseID associated with the given module identifier.
		/// (returns -1 if the module identifier doesn't exist)
		/// </summary>
		/// <param name="identifier">The module identifier</param>
		/// <returns>The BaseID for the given module identifier</returns>
		public static int getBaseIDfromIdentifier( string identifier ) {
			
			int baseID = -1;
			
			SqlCommand cmd = new SqlCommand();
			cmd.Connection = new SqlConnection( Globals.ConnectionString );
			cmd.CommandText = "SELECT BaseID FROM ModuleBases WHERE ModuleIdentifier = @ModIdent";
			cmd.Parameters.Add( "@ModIdent", identifier );
			
			cmd.Connection.Open();
			object temp = cmd.ExecuteScalar();
			cmd.Connection.Close();

			if( temp != null )
				baseID = (int)temp;

			return baseID;
		}

		/// <summary>
		/// Given a module's baseID, this method returns that module's ModuleIdentifier.
		/// </summary>
		/// <param name="baseId">The module's baseID.</param>
		/// <returns>The module's ModuleIdentifier.</returns>
		public static string getModuleIdentifierByBaseID( int baseId ) {

			string modIdentifier = "";

			SqlCommand cmd = new SqlCommand();
			cmd.Connection = new SqlConnection( Globals.ConnectionString );
			cmd.CommandText = "SELECT ModuleIdentifier FROM ModuleBases WHERE BaseID = @baseID";
			cmd.Parameters.Add( "@baseID", baseId );

			cmd.Connection.Open();
			object temp = cmd.ExecuteScalar();
			cmd.Connection.Close();

			if( temp != null )
				modIdentifier = (string)temp;

			return modIdentifier;
		}
		
		/// <summary>
		/// This method returns ModuleID's for all the modules that fit the query.
		/// </summary>
		/// <param name="queryText">The substring to search for</param>
		/// <param name="type">The type of search to perform (from ModuleSearchType enum)</param>
		/// <returns> IList containing the appropriate ModuleID's</returns>
		public static IList getModuleIDs( string queryText, int type ) {

			IList moduleIDs = null;
			SqlCommand cmd = new SqlCommand();
			cmd.Connection = new SqlConnection( Globals.ConnectionString );
			bool valid = true;

			switch( type ) {
				case 0: // Author
					cmd.CommandText =
						"SELECT DISTINCT ModulesDetailView.ModuleID " +
						"FROM ModulesDetailView, Author " +
						"WHERE Author.UserName LIKE @Param " +
						"AND Author.ModuleID = ModulesDetailView.ModuleID " +
						"AND ModulesDetailView.Status = @Status";
					break;
				case 1: // Submitter
					cmd.CommandText = "SELECT ModuleID FROM Module " +
						"WHERE Submitter LIKE @Param AND Status = @Status";
					break;
				case 2: // Title
					cmd.CommandText = "SELECT ModuleID FROM ModulesDetailView " +
						"WHERE Title LIKE @Param AND Status = @Status";
					break;
				case 3: // Module Identifier
					cmd.CommandText = "SELECT ModuleID FROM ModulesDetailView " +
						"WHERE ModuleIdentifier LIKE @Param AND Status = @Status";
					break;
				case 4: // Submitter Identifier
					cmd.CommandText = "SELECT ModuleID FROM Module " +
						"WHERE SubmitterID LIKE @Param AND Status = @Status";
					break;
				default:
					valid = false;
					break;
			}

			if( valid ) {
				moduleIDs = new ArrayList();

				cmd.Parameters.Add( "@Param", "%" + queryText + "%" );
				cmd.Parameters.Add( "@Status", ModuleStatus.Approved );

				cmd.Connection.Open();
				SqlDataReader reader = cmd.ExecuteReader( CommandBehavior.CloseConnection );
				while( reader.Read() ) {
					moduleIDs.Add( reader.GetInt32( 0 ) );
				}
				reader.Close();
			}

			return moduleIDs;
		}

		/// <summary>
		/// Given a submitterID, this method returns the ModuleIdentifier of the last
		/// module from this submitter.
		/// </summary>
		/// <param name="submitterID">The submitter's submitterID.</param>
		/// <returns>The ModuleIdentifier of the last module he/she submitted.</returns>
		public static string getLastIDbySubmitter( string submitterID ) {
			string retval = "";

			SqlCommand cmd = new SqlCommand();
			cmd.Connection = new SqlConnection( Globals.UsersConnectionString );
			cmd.CommandText = "SELECT LastModuleID FROM UserInfo WHERE SubmitterID = @SubID";
			cmd.Parameters.Add( "@SubID", submitterID );
			object temp = null;

			try {
				cmd.Connection.Open();
				temp = cmd.ExecuteScalar();
				cmd.Connection.Close();

				if( temp != null )
					retval = temp.ToString();

			} catch( Exception e ) {
				throw new Exception( "Modules.cs using submitter: " + submitterID + ". Object temp = " + temp + ". " + e.Message, e.InnerException );
			}



			return retval;
		}

		/// <summary>
		/// Updates the field which contains the ModuleIdentifier of the module last
		/// submitted by the submitter associated with the given submitterID.
		/// </summary>
		/// <param name="submitterID">The submitterID used to identify the
		/// appropriate submitter.</param>
		/// <param name="newID">The new ModuleIdentifier to store.</param>
		public static void setLastIDbySubmitter( string submitterID, string newID ) {

			SqlCommand cmd = new SqlCommand();
			cmd.Connection = new SqlConnection( Globals.UsersConnectionString );
			cmd.CommandText = "UPDATE UserInfo SET LastModuleID = @ModID WHERE SubmitterID = @SubID";
			cmd.Parameters.Add( "@ModID", newID );
			cmd.Parameters.Add( "@SubID", submitterID );

			cmd.Connection.Open();
			cmd.ExecuteNonQuery();
			cmd.Connection.Close();
		}

		/// <summary>
		/// Assigns the given ModuleIdentifier to the given baseID.
		/// </summary>
		/// <param name="baseId">The baseID of the group to use.</param>
		/// <param name="newID">The ModuleIdentifier to set for the group.</param>
		public static void setModuleIdentifier( int baseId, string newID ) {

			SqlCommand cmd = new SqlCommand();
			cmd.Connection = new SqlConnection( Globals.ConnectionString );
			cmd.CommandText = "UPDATE ModuleBases SET ModuleIdentifier = @ModID WHERE BaseID = @BaseID";
			cmd.Parameters.Add( "@ModID", newID );
			cmd.Parameters.Add( "@BaseID", baseId );

			cmd.Connection.Open();
			cmd.ExecuteNonQuery();
			cmd.Connection.Close();

		}

		# region ModuleBases methods

		/// <summary>
		/// Add a module base to the database.  This simply adds the module
		/// title, which mustn't change across versions, and returns a
		/// unique ID for the base.
		/// </summary>
		/// <param name="mi"></param>
		/// <returns></returns>
		public static int addModuleBase( ModuleInfo mi ) {
			SqlCommand cmd = new SqlCommand();
			cmd.Connection = new SqlConnection( Globals.ConnectionString );
			cmd.CommandText = "INSERT INTO ModuleBases (Title, ModuleIdentifier) "
				+ "VALUES (@Title, @ModuleIdentifier) SELECT BaseID = @@Identity";
			cmd.Parameters.Add( new SqlParameter( "@Title", mi.Title ) );
			cmd.Parameters.Add( new SqlParameter( "@ModuleIdentifier", "" ) );

			int retVal = 0;

			try {
				cmd.Connection.Open();
				retVal = Convert.ToInt32( cmd.ExecuteScalar() );
			} catch ( SqlException e ) {
				throw;
			} finally {
				cmd.Connection.Close();
			}

			return retVal;
		}

		/// <summary>
		/// Obtain the most recent version of a module.  This includes modules
		/// with any status.
		/// </summary>
		/// <param name="baseID">
		/// The base ID indicating the module for which to obtain the most
		/// recent version.
		/// </param>
		/// <returns>
		/// The most recent version of the module.
		/// </returns>
		public static ModuleInfo getModuleCurrentVersion( int baseID ) {
			SqlCommand cmd = new SqlCommand();
			cmd.Connection = 
				new SqlConnection( Globals.ConnectionString );
			cmd.CommandText = "GetModuleCurrentVersion";
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add( "@BaseID", baseID );

			return (ModuleInfo)executeGetModulesCommand( cmd )[0];
		}

		/// <summary>
		/// Get a list of all versions of a module.
		/// </summary>
		/// <param name="baseID">
		/// The base ID indicating the module for which to obtain all verisons.
		/// </param>
		/// <param name="includePending">
		/// Indicates whether to include modules pending approval, or just
		/// previously approved modules.
		/// </param>
		/// <returns>
		/// A list of the module versions desired.
		/// </returns>
		public static IList getModuleVersions( int baseID, bool includePending ) {
			SqlCommand cmd = new SqlCommand();
			cmd.Connection = 
				new SqlConnection( Globals.ConnectionString );
			cmd.CommandText = "GetModuleVersions";
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add( "@BaseID", baseID );
			cmd.Parameters.Add( "@IncludePending", Convert.ToInt32( includePending ) );

			return executeGetModulesCommand( cmd );
		}

		/// <summary>
		/// Lock a module for editing.
		/// </summary>
		/// <param name="moduleID">The module to lock.</param>
		/// <param name="username">The username locking the module.</param>
		/// <returns>If the lock was successful.</returns>
		public static bool setLock( int moduleID, string username ) {
			IDbCommand cmd = new SqlCommand();
			cmd.Connection =
				new SqlConnection( Globals.ConnectionString );
			cmd.CommandText = "SetLock";
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add( new SqlParameter( "@ModuleID", moduleID ) );
			cmd.Parameters.Add( new SqlParameter( "@UserName", username ) );

			bool retVal = false;
			cmd.Connection.Open();
			
			try {
				string lockedBy = (string)cmd.ExecuteScalar();
				retVal = lockedBy != "";
			} catch ( SqlException e ) {
				throw;
			} finally {
				cmd.Connection.Close();
			}

			return retVal;
		}

		#endregion ModuleBases methods

		/// <summary>
		/// Obtain a list of all Bloom's taxonomy levels as they exist in the database.
		/// </summary>
		/// <returns>The list of Bloom levels.</returns>
		public static IList getBloomLevels() {
			IList bloomLevels;
			SqlCommand command = new SqlCommand();
			command.Connection = new SqlConnection( Globals.ConnectionString );
			command.CommandText = "SELECT BloomLevel FROM BloomLevels ORDER BY LevelID";

			SqlDataReader reader = null;

			try {
				command.Connection.Open();
				reader = command.ExecuteReader( CommandBehavior.CloseConnection );
				bloomLevels = new ArrayList();

				while ( reader.Read() ) {
					bloomLevels.Add( reader.GetString( 0 ) );
				}
			} catch ( SqlException e ) {
				throw;
			} finally {
				reader.Close();
			}

			return bloomLevels;
		}


		#region Private Helper Methods

		/// <summary>
		/// Add ModuleInfo parameters to the INSERT command.
		/// </summary>
		/// <param name="dbCommand">The command to add parameters to.</param>
		/// <param name="mi">The module information object.</param>
		/// <param name="status">The designated module status.</param>
		private static void addModuleInfoParams( IDbCommand dbCommand, ModuleInfo mi ) {
			dbCommand.Parameters.Add( new SqlParameter( "@Title", mi.Title ) );
			dbCommand.Parameters.Add( new SqlParameter( "@Abstract", mi.Abstract) );
			dbCommand.Parameters.Add( new SqlParameter( "@LectureSize", mi.LectureSize ) );
			dbCommand.Parameters.Add( new SqlParameter( "@LabSize", mi.LabSize ) );
			dbCommand.Parameters.Add( new SqlParameter( "@ExerciseSize", mi.ExerciseSize ) );
			dbCommand.Parameters.Add( new SqlParameter( "@HomeworkSize", mi.HomeworkSize ) );
			dbCommand.Parameters.Add( new SqlParameter( "@OtherSize", mi.OtherSize ) );
			dbCommand.Parameters.Add( new SqlParameter( "@Comments", mi.AuthorComments ) );
			dbCommand.Parameters.Add( new SqlParameter( "@Status", (int)mi.Status ) );
			dbCommand.Parameters.Add( new SqlParameter( "@BaseID", mi.BaseId ) );
			dbCommand.Parameters.Add( new SqlParameter( "@Version", mi.Version ) );
			dbCommand.Parameters.Add( new SqlParameter( "@Submitter", mi.Submitter ) );
			dbCommand.Parameters.Add( new SqlParameter( "@SubmitterID", mi.SubmitterID ) );
			dbCommand.Parameters.Add( new SqlParameter( "@CheckInComments", mi.CheckInComments ) );
		}

		/// <summary>
		/// Add ModuleInfo parameters to the UPDATE command.
		/// </summary>
		/// <param name="dbCommand">The command to add parameters to.</param>
		/// <param name="mi">The module information object.</param>
		/// <param name="status">The designated module status.</param>
		private static void addModuleInfoUpdateParams( IDbCommand dbCommand, ModuleInfo mi ) {
			dbCommand.Parameters.Add( new SqlParameter( "@Title", mi.Title ) );
			dbCommand.Parameters.Add( new SqlParameter( "@Abstract", mi.Abstract) );
			dbCommand.Parameters.Add( new SqlParameter( "@LectureSize", mi.LectureSize ) );
			dbCommand.Parameters.Add( new SqlParameter( "@LabSize", mi.LabSize ) );
			dbCommand.Parameters.Add( new SqlParameter( "@ExerciseSize", mi.ExerciseSize ) );
			dbCommand.Parameters.Add( new SqlParameter( "@HomeworkSize", mi.HomeworkSize ) );
			dbCommand.Parameters.Add( new SqlParameter( "@OtherSize", mi.OtherSize ) );
			dbCommand.Parameters.Add( new SqlParameter( "@Comments", mi.AuthorComments ) );
			dbCommand.Parameters.Add( new SqlParameter( "@Status", (int)mi.Status ) );
			dbCommand.Parameters.Add( new SqlParameter( "@CheckInComments", mi.CheckInComments ) );
		}

		/// <summary>
		/// Add the given command text string and status parameter to the
		/// database command to filter the results by status if status is
		/// not ModuleStatus.All.
		/// </summary>
		/// <param name="cmd">The database command.</param>
		/// <param name="status">The desired status.</param>
		/// <param name="addString">The string to add to the command text
		/// if all module statuses aren't desired.</param>
		private static void addStatusIfNeeded( IDbCommand cmd, ModuleStatus status, string addString ) {
			if ( status != ModuleStatus.All ) {
				cmd.CommandText += addString;

				if ( cmd.Parameters.IndexOf( "@Status" ) == -1 ) {
					cmd.Parameters.Add( new SqlParameter( "@Status", (int)status ) );
				}
			}
		}

		/// <summary>
		/// Encapsulates code for a command that gets a list of modules.
		/// </summary>
		/// <param name="cmd">The exact command to execute.</param>
		/// <returns>The resulting list of modules.</returns>
		public static IList executeGetModulesCommand( IDbCommand cmd ) {
			IList modulesList = new ArrayList();

			try {
				cmd.Connection.Open();
				IDataReader reader = cmd.ExecuteReader( CommandBehavior.CloseConnection );

				while ( reader.Read() ) {
					modulesList.Add( constructModuleInfo( reader ) );
				}
			} catch ( SqlException e ) {
				throw;
			} finally {
				cmd.Connection.Close();
			}

			return modulesList;
		}

		/// <summary>
		/// Construct a ModuleInfo object from a data reader.
		/// </summary>
		/// <param name="reader">The reader to extract the fields from.</param>
		/// <returns>The ModuleInfo object created.</returns>
		/// <remarks>
		/// This method assumes that the reader has been advanced to
		/// the next record (the read() method has been called).
		/// </remarks>
		private static ModuleInfo constructModuleInfo( IDataReader reader ) {
			ModuleInfo mi = new ModuleInfo();
			mi.Id = (int)reader["ModuleID"];
			mi.Title = (string)reader["Title"];
			mi.Date = (DateTime)reader["Date"];
			mi.Abstract = (string)reader["Abstract"];
			mi.LectureSize = (string)reader["LectureSize"];
			mi.LabSize = (string)reader["LabSize"];
			mi.ExerciseSize = (string)reader["ExerciseSize"];
			mi.HomeworkSize = (string)reader["HomeworkSize"];
			mi.OtherSize = (string)reader["OtherSize"];
			mi.AuthorComments = (string)reader["AuthorComments"];
			mi.Status = (ModuleStatus)reader["Status"];
			mi.Submitter = (string)reader["Submitter"];
			mi.Version = (int)reader["Version"];
			mi.BaseId = (int)reader["BaseID"];
			mi.LockedBy = (string)reader["LockedBy"];
			mi.CheckInComments = (string)reader["CheckInComments"];

			return mi;
		}

		#endregion
	}
}
