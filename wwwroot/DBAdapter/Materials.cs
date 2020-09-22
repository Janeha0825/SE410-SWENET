using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;

namespace SwenetDev.DBAdapter {
	/// <summary>
	/// An adapter to interact with the database table for SWENET materials.
	/// </summary>
	public class Materials {

		[Serializable()]
		public class MaterialInfo : Info {
			private string identInfo;
			private string link;
			private double rating;
			private int orderID;
			private int matID;
			private int accessFlag;
			private string ratingImage;
			private int modID;

			public string IdentInfo {
				get {
					return identInfo;
				}
				set {
					identInfo = value;
				}
			}
			public string RatingImage {
				get {
					return ratingImage;
				}
				set {
					ratingImage = value;
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
			public double Rating 
			{
				get 
				{
					return rating;
				}
				set 
				{
					rating = value;
				}
			}
			public int MatID 
			{
				get 
				{
					return matID;
				}
				set 
				{
					matID = value;
				}
			}
			public int OrderID {
				get {
					return orderID;
				}
				set {
					orderID = value;
				}
			}
			public int AccessFlag {
				get {
					return accessFlag;
				}
				set {
					accessFlag = value;
				}
			}
			public int ModuleID {
				get {
					return modID;
				}
				set 
				{
					modID = value;
				}
			}

			public MaterialInfo( int id, string identInfo, string link, int moduleID, string ratingImage, double rating, int accessFlag) {
				this.id = id;
				this.identInfo = identInfo;
				this.link = link;
				this.modID = moduleID;
				this.matID = id;
				this.ratingImage = ratingImage;
				orderID = 0;
				this.rating = rating;
				this.accessFlag = accessFlag;
			}

			/// <summary>
			/// Determines if material is equal to this material by comparing
			/// their links for equality.
			/// </summary>
			/// <param name="obj">The object to compare.</param>
			/// <returns>True if the objects are equal, false otherwise.</returns>
			public override bool Equals( object obj ) {
				bool retVal = false;

				retVal = obj != null && obj.GetType() == GetType();

				if ( retVal ) {
					MaterialInfo mi = (MaterialInfo)obj;
					retVal = link == mi.link;
				}

				return retVal;

			}

		}

		/// <summary>
		/// Adds the given Materials from the materialsList to the module
		/// identified by the moduleID.
		/// </summary>
		/// <param name="moduleID">Identifies the correct module to add
		/// the Materials to.</param>
		/// <param name="materialsList">The list of Materials to add.</param>
		public static void addAll( int moduleID, IList materialsList ) {
			MaterialInfo mi = null;
			int matID = 0;

			for ( int i = 0; i < materialsList.Count; i++ ) {
				mi = (MaterialInfo)materialsList[i];
				mi.OrderID = i + 1;

				matID = add( mi );
                
				addLink( moduleID, matID, mi.OrderID );
			}
		}

		/// <summary>
		/// Add a material to the database.
		/// </summary>
		/// <param name="info">Identifying info.</param>
		/// <param name="link">Link to the file.</param>
		/// <returns>The id of the newly inserted material.</returns>
		private static int add( MaterialInfo mi ) {
			// info, link

			int retval = 0;

			IDbCommand dbCommand = new SqlCommand();
    
			dbCommand.Connection = new SqlConnection( Globals.ConnectionString );
			dbCommand.CommandText = "MaterialExist";
			dbCommand.CommandType = CommandType.StoredProcedure;
    
			IDataParameter dbParam_info = new SqlParameter( "@IdentifyingInfo", mi.IdentInfo );
			dbCommand.Parameters.Add(dbParam_info);

			IDataParameter dbParam_link = new SqlParameter( "@LinkToMaterial", mi.Link );
			dbCommand.Parameters.Add(dbParam_link);

			IDataParameter dbParam_modID = new SqlParameter( "@InitialModuleID", mi.ModuleID );
			dbCommand.Parameters.Add(dbParam_modID);

			IDataParameter dbParam_rating = new SqlParameter( "@Rating", "0" );
			dbCommand.Parameters.Add(dbParam_rating);

			IDataParameter dbParam_image = new SqlParameter( "@RatingImage", "images/stars0.gif" );
			dbCommand.Parameters.Add(dbParam_image);

			IDataParameter dbParam_access = new SqlParameter( "@AccessFlag", mi.AccessFlag );
			dbCommand.Parameters.Add(dbParam_access);
   
			IDataReader dbReader = null;

			try 
			{
				dbCommand.Connection.Open();
				dbReader = dbCommand.ExecuteReader( CommandBehavior.CloseConnection );

				if( dbReader.Read() ) 
				{
					retval = dbReader.GetInt32( 0 );
				}
			} 
			catch ( SqlException e ) 
			{
				throw;
			} 
			finally 
			{
				dbReader.Close();
			}

			return retval;
	 
		}

		/// <summary>
		/// Link the module and material ID's.
		/// </summary>
		/// <param name="moduleID">The module.</param>
		/// <param name="materialID">The material.</param>
		/// <returns></returns>
		private static void addLink( int moduleID, int materialID, int orderID ) {

			SqlCommand cmd = new SqlCommand();
			cmd.Connection = new SqlConnection( Globals.ConnectionString );
			cmd.CommandText = "MaterialLinkExist";
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add( "@ModuleID", moduleID );
			cmd.Parameters.Add( "@MaterialID", materialID );
			cmd.Parameters.Add( "@OrderID", orderID );

			try 
			{
				cmd.Connection.Open();
				cmd.ExecuteNonQuery();
			} 
			catch ( SqlException e ) 
			{
				throw;
			} 
			finally 
			{
				cmd.Connection.Close();
			}

		}

		 
		/// <summary>
		/// Returns the MaterialInfo object associated with the passed materialID.
		/// </summary>
		/// <param name="materialID">The materialID key to the MaterialInfo object.</param>
		/// <returns>The MaterialInfo object associated with the passed materialID.</returns>
		public static MaterialInfo getMaterialInfo( int materialID ) {
			string queryString = "SELECT MaterialID, IdentifyingInfo, LinkToMaterial, " +
								"InitialModuleID, RatingImage, Rating, AccessFlag FROM " +
								"[Materials] WHERE ([Materials].[MaterialID] = @MaterialID)";
			IDbCommand dbCommand = new SqlCommand();
			dbCommand.CommandText = queryString;
			dbCommand.Connection = new SqlConnection( Globals.ConnectionString );
    
			IDataParameter dbParam_materialID = new SqlParameter( "@MaterialID", materialID );
			dbCommand.Parameters.Add(dbParam_materialID);
    
			MaterialInfo materialInfo = null;
			IDataReader dataReader = null;

			try {
				dbCommand.Connection.Open();
				dataReader = dbCommand.ExecuteReader(CommandBehavior.CloseConnection);
				dataReader.Read();

				materialInfo = new MaterialInfo( dataReader.GetInt32(0),
					dataReader.GetString(1).Trim(),
					dataReader.GetString(2).Trim(),
					dataReader.GetInt32(3),
					dataReader.GetString(4).Trim(),
					dataReader.GetDouble(5),
					dataReader.GetInt32(6)   );
			} catch ( SqlException e ) {
				throw;
			} finally {
				dataReader.Close();
			}
			return materialInfo;
		}

		/// <summary>
		/// Reads from the database every available material.
		/// </summary>
		/// <returns>A Hashtable of LinkToMaterial and InitialModuleID</returns>
		public static Hashtable getAll() {
			SqlCommand cmd = new SqlCommand();
			cmd.Connection = new SqlConnection( Globals.ConnectionString );
			cmd.CommandText = "SELECT LinkToMaterial, MaterialID FROM Materials WHERE InitialModuleID > 0";

			Hashtable matMap = null;
			SqlDataReader reader = null;

			try {
				cmd.Connection.Open();
				reader = cmd.ExecuteReader( CommandBehavior.CloseConnection );

				matMap = new Hashtable();
								
				while( reader.Read() ) 
				{
					string fName = Convert.ToString( reader["LinkToMaterial"] );
					int modID = Convert.ToInt32( reader["MaterialID"] );
					
					// Only return the most recent upload of a specific material
					if( matMap.ContainsKey( fName ) ) 
					{
						int oldValue = 0;

						// Since collections are extremely limited in C#...
						// Write your own getValue code!
						foreach( DictionaryEntry pair in matMap ) 
						{
							if( pair.Key.Equals( fName ) ) 
							{
								oldValue = Convert.ToInt32( pair.Value );
							}
						}// end getValue

						if( modID > oldValue ) 
						{
							matMap.Remove( fName );
							matMap.Add( fName, modID );
						}
					} else {
						matMap.Add( fName, modID );
					}
				}
			} catch( Exception e ) {
				throw;
			} finally {
				reader.Close();
			}

			return matMap;

		}

		/// <summary>
		/// Returns a collection of the MaterialInfo objects associated with the
		/// passed moduleID.
		/// </summary>
		/// <param name="moduleID">The moduleID of the module to get Materials from.</param>
		/// <returns>Collection of MaterialInfo objects associated with the moduleID.</returns>
		public static IList getAll( int moduleID ) {
			SqlCommand sqlSelectCommand = new SqlCommand();
			sqlSelectCommand.Connection = new SqlConnection( Globals.ConnectionString );
			sqlSelectCommand.CommandText = "SELECT Materials.MaterialID, IdentifyingInfo, LinkToMaterial, " +
				"InitialModuleID, RatingImage, Rating, AccessFlag " +
				"FROM Materials INNER JOIN ModuleMaterialsLink " +
				"ON Materials.MaterialID = ModuleMaterialsLink.MaterialID " +
				"WHERE ModuleID = @ModuleID ORDER BY OrderID";
			sqlSelectCommand.Parameters.Add( new SqlParameter( "@ModuleID", moduleID ) );
			
			IList materialsList = null;
			SqlDataReader reader = null;

			try {
				sqlSelectCommand.Connection.Open();
				reader = sqlSelectCommand.ExecuteReader(CommandBehavior.CloseConnection);

				materialsList = new ArrayList();

				while ( reader.Read() ) {
					materialsList.Add( new MaterialInfo( reader.GetInt32( 0 ),
						reader.GetString( 1 ).Trim(),
						reader.GetString( 2 ).Trim(),
						reader.GetInt32( 3 ),
						reader.GetString( 4 ).Trim(),
						reader.GetDouble( 5 ),
						reader.GetInt32( 6 )   ));
				}
			} catch ( SqlException e ) {
				throw;
			} finally {
				reader.Close();
			}

			return materialsList;
		}

		/// <summary>
		/// Remove all materials from the given module.
		/// </summary>
		/// <param name="moduleID">The module from which to remove
		/// the materials.</param>
		public static void removeAll( int moduleID ) {
			SqlConnection dbConnection =
				new SqlConnection( Globals.ConnectionString );
			IDbCommand dbCommand = new SqlCommand();

			dbCommand.CommandText = "DELETE FROM ModuleMaterialsLink WHERE ModuleID = @ModuleID";
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
		/// Sets the material rating and visual rating for the material associated with the passed materialID
		/// </summary>
		/// <param name="newRating">The new rating for the material</param>
		/// <param name="matID">The materialID for the associated material</param>
		public static void setMaterialRating(int newRating, int matID) {

			double  totalNewRating = newRating;
			SqlConnection dbConnection = new SqlConnection(Globals.ConnectionString);
			IDbCommand dbCommand = new SqlCommand();

			totalNewRating += (getMaterialRating(matID) * (getNumberOfRatings(matID) - 1));
			totalNewRating /= getNumberOfRatings(matID); 

			//Sets the visual material rating based on the numerical rating
			string imageLink = null;
			if (totalNewRating < .75) imageLink = "images/stars05.gif";
			else if (totalNewRating >= .75 && totalNewRating < 1.25)  imageLink = "images/stars1.gif";
			else if (totalNewRating >= 1.25 && totalNewRating < 1.75) imageLink = "images/stars15.gif";
			else if (totalNewRating >= 1.75 && totalNewRating < 2.25) imageLink = "images/stars2.gif";
			else if (totalNewRating >= 2.25 && totalNewRating < 2.75) imageLink = "images/stars25.gif";
			else if (totalNewRating >= 2.75 && totalNewRating < 3.25) imageLink = "images/stars3.gif";
			else if (totalNewRating >= 3.25 && totalNewRating < 3.75) imageLink = "images/stars35.gif";
			else if (totalNewRating >= 3.75 && totalNewRating < 4.25) imageLink = "images/stars4.gif";
			else if (totalNewRating >= 4.25 && totalNewRating < 4.75) imageLink = "images/stars45.gif";
			else if (totalNewRating >= 4.75)  imageLink = "images/stars5.gif";
			else imageLink = "images/stars0.gif";

			dbCommand.CommandText = "UPDATE Materials SET Rating = @TotalNewRating, RatingImage = @ImageLink WHERE MaterialID = @MatID";
			dbCommand.Connection = dbConnection;

			dbCommand.Parameters.Add(new SqlParameter("@TotalNewRating", totalNewRating));
			dbCommand.Parameters.Add(new SqlParameter("@MatID", matID));
			dbCommand.Parameters.Add(new SqlParameter("@ImageLink", imageLink));

			try {
				dbCommand.Connection.Open();
				dbCommand.ExecuteNonQuery();
			} 
			catch (SqlException e) {
				throw;
			} 
			finally {
				dbCommand.Connection.Close();
			}
		}

		/// <summary>
		/// Gets the material rating of the material associated with the passed materialID
		/// </summary>
		/// <param name="matID">The materialID for the associated material</param>
		/// <returns>Returns the rating for the material</returns>
		public static double getMaterialRating(int matID) {

			SqlConnection dbConnection = new SqlConnection(Globals.ConnectionString);
			IDbCommand dbCommand = new SqlCommand();

			dbCommand.CommandText = "SELECT Rating FROM Materials WHERE MaterialID = @MatID";
			dbCommand.Connection = dbConnection;

			dbCommand.Parameters.Add(new SqlParameter("@MatID", matID));

			double materialRating = 0;
			IDataReader dataReader = null;

			try {
				dbCommand.Connection.Open();
				dataReader = dbCommand.ExecuteReader(CommandBehavior.CloseConnection);
				dataReader.Read();
				
				materialRating = dataReader.GetDouble(0);
			}		
			catch ( SqlException e ) {
				throw;
			} 
			finally {
				dataReader.Close();
			}
	
			return materialRating;

		}

		/// <summary>
		/// Sets the number of number of ratings for the material associated with the passed materialID
		/// </summary>
		/// <param name="numberOfRatings">The new number of ratings for the material</param>
		/// <param name="matID">The materialID for the associated material</param>
		public static void setNumberOfRatings(int numberOfRatings, int matID) {

			SqlCommand dbCommand = new SqlCommand();
			SqlConnection dbConnection = new SqlConnection(Globals.ConnectionString);
			dbCommand.Connection = dbConnection;
		
			dbCommand.CommandText = "UPDATE Materials SET NumberOfRatings = @NumberOfRatings WHERE MaterialID = @MatID";

			dbCommand.Parameters.Add(new SqlParameter("@MatID", matID));
			dbCommand.Parameters.Add(new SqlParameter("@NumberOfRatings", numberOfRatings));

			try {
				dbCommand.Connection.Open();
				dbCommand.ExecuteNonQuery();
			} 
			catch (SqlException e) {
				throw;
			} 
			finally {
				dbCommand.Connection.Close();
			}

		}

		/// <summary>
		/// Gets the number of ratings for the material associated with the passed materialID
		/// </summary>
		/// <param name="matID">The materialID for the associated material</param>
		/// <returns>Returns the number of ratings for the passed materialID</returns>
		public static int getNumberOfRatings(int matID) {

			SqlConnection dbConnection = new SqlConnection(Globals.ConnectionString);
			IDbCommand dbCommand = new SqlCommand();

			dbCommand.CommandText = "SELECT NumberOfRatings FROM Materials WHERE MaterialID = @MatID";
			dbCommand.Connection = dbConnection;

			dbCommand.Parameters.Add(new SqlParameter("@MatID", matID));

			int numberOfRatings = 0;
			IDataReader dataReader = null;

			try {
				dbCommand.Connection.Open();
				dataReader = dbCommand.ExecuteReader(CommandBehavior.CloseConnection);
				dataReader.Read();
				
				numberOfRatings = dataReader.GetInt32(0);
			}		
			catch ( SqlException e ) {
				throw;
			} 
			finally {
				dataReader.Close();
			}
	
			return numberOfRatings;

		}

		/// <summary>
		/// Gets the moduleID of the module that the passed materialID is associated with
		/// </summary>
		/// <param name="matID">The materialID for the material in the module</param>
		/// <returns>Returns the moduleID of the module that the passed materialID is in</returns>
		public static int getModuleOfMaterial(int matID) {

			SqlCommand dbCommand = new SqlCommand();
			SqlConnection dbConnection = new SqlConnection(Globals.ConnectionString);
			dbCommand.Connection = dbConnection;

			dbCommand.CommandText = "SELECT ModuleID FROM ModuleMaterialsLink WHERE MaterialID = @MatID";

			dbCommand.Parameters.Add("@MatID", matID);

			int moduleID = 0;
			IDataReader dataReader = null;

			try {
				dbCommand.Connection.Open();
				dataReader = dbCommand.ExecuteReader(CommandBehavior.CloseConnection);
				dataReader.Read();

				moduleID = dataReader.GetInt32(0);
			}
			catch (SqlException e) {
				throw;
			}
			finally {
				dataReader.Close();
			}

			return moduleID;
		}
	}
}
