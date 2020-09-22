using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace SwenetDev.DBAdapter {
    /// <summary>
    /// An adapter to interact with the database table containing authors
    /// of SWENET modules.
    /// </summary>
    public class Authors {
		private static SqlConnection dbConnection =
			new SqlConnection( ConfigurationSettings.AppSettings["ConnectionString"] );

		/// <summary>
		/// Encapsulates information of an individual SWENET author.
		/// </summary>
		[Serializable()]
		public class AuthorInfo : Info {
			private string name;
			private string email;
			private int orderID;
			private string username;

			/// <summary>
			/// The author's user name.
			/// </summary>
			public string UserName {
				get { return username; }
				set { username = value; }
			}

			/// <summary>
			/// Gets author's name.
			/// </summary>
			public string Name {
				get { return name; }
				set { name = value; }
			}

			/// <summary>
			/// Gets the author's email address.
			/// </summary>
			public string Email {
				get { return email; }
				set { email = value; }
			}

			/// <summary>
			/// The order identifer.
			/// </summary>
			public int OrderId {
				get { return orderID;  }
				set { orderID = value; }
			}

			/// <summary>
			/// Creates a new instance of AuthorInfo.
			/// </summary>
			/// <param name="username">The username for the author.</param>
			/// <param name="name">The name for the author.</param>
			/// <param name="email">The email address for the author.</param>
			public AuthorInfo( string username, string name, string email ) {
				this.username = username;
				this.name = name;
				this.email = email;
				orderID = 0;
			}

			/// <summary>
			/// Creates a new instance of AuthorInfo.
			/// </summary>
			/// <param name="username">The username for the author.</param>
			/// <param name="name">The name for the author.</param>
			/// <param name="email">The email address for the author.</param>
			/// <param name="orderID">The order number for the author for a particular module.</param>
			public AuthorInfo( string username, string name, string email, int orderID ) {
				this.username = username;
				this.name = name;
				this.email = email;
				this.orderID = orderID;
			}
		}

		/// <summary>
		/// Add all authors for the given module.  If the author is new, its
		/// ID should be 0.
		/// </summary>
		/// <param name="moduleID">The module for which to add the authors.</param>
		/// <param name="authorsList">The list of authors to add.</param>
		public static void addAll( int moduleID, IList items ) {
			try {
				dbConnection.Open();
    
				for ( int i = 0; i < items.Count; i++ ) {
					AuthorInfo ai = (AuthorInfo)items[i];
					string username = ai.UserName;

					addAuthor( moduleID, username, i + 1 );
				}
			} finally {
				dbConnection.Close();
			}

		}

		/// <summary>
		/// Adds an author to the database, obtaining a unique identifer for this author.
		/// </summary>
		/// <param name="name">The name of the author to add.</param>
		/// <param name="email">The email address of the author to add.</param>
		/// <returns>A unique identifier for the author inserted.</returns>
        private static void addAuthor( int moduleID, string username, int orderID ) {
            string queryString = "INSERT INTO Author (ModuleID, UserName, OrderID) " +
				"VALUES (@ModuleID, @UserName, @OrderID)";
            IDbCommand dbCommand = new SqlCommand();

            dbCommand.CommandText = queryString;
            dbCommand.Connection = dbConnection;

			dbCommand.Parameters.Add( new SqlParameter( "@ModuleID", moduleID ) );
            dbCommand.Parameters.Add( new SqlParameter( "@UserName", username ) );
			dbCommand.Parameters.Add( new SqlParameter( "@OrderID", orderID ) );

			dbCommand.ExecuteNonQuery();
        }

		/// <summary>
		/// Gets the authors associated with the given module or all
		/// registered authors.
		/// </summary>
		/// <param name="moduleID">The identifier of the module for which
		/// to obtain authors.</param>
		/// <returns>The resulting list of authors for the given module.</returns>
		public static IList getAll( int moduleID ) {
			SqlCommand sqlCommand = new SqlCommand();

			// If the parameter is -1, get all authors.
			if ( moduleID != -1 ) {
				sqlCommand.Connection =
					new SqlConnection( Globals.ConnectionString );
				sqlCommand.CommandText = "SELECT UserName, OrderID FROM Author " +
					"WHERE ModuleID = @ModuleID " +
					"ORDER BY OrderID";
				sqlCommand.Parameters.Add(new SqlParameter("@ModuleID", moduleID));
			} else {
				sqlCommand.Connection =
					new SqlConnection( Globals.UsersConnectionString );
				sqlCommand.CommandText = "SELECT UserName, Name, Email FROM UserInfo";
			}

			sqlCommand.Connection.Open();
			SqlDataReader reader = sqlCommand.ExecuteReader( CommandBehavior.CloseConnection );
			IList authorsList = new ArrayList();

			while ( reader.Read() ) {
				AuthorInfo ai = null;

				// Construct appropriate AuthorInfo object.
				if ( moduleID != -1 ) {
					ai = getAuthorInfo( reader.GetString( 0 ).Trim() );
					ai.OrderId = reader.GetInt32( 1 );
				} else {
					ai = new AuthorInfo( reader.GetString( 0 ).Trim(),
						reader.GetString( 1 ).Trim(), reader.GetString( 2 ).Trim() );
				}

				authorsList.Add( ai );
			}

			reader.Close();

			return authorsList;
		}

		/// <summary>
		/// Remove all authors from the given module.
		/// </summary>
		/// <param name="moduleID">The module from which to remove
		/// the authors.</param>
		public static void removeAll( int moduleID ) {
			IDbCommand dbCommand = new SqlCommand();

			dbCommand.CommandText = "DELETE FROM Author WHERE ModuleID = @ModuleID";
			dbCommand.Connection = dbConnection;

			dbCommand.Parameters.Add( new SqlParameter( "@ModuleID", moduleID ) );

			dbCommand.Connection.Open();
			try {
				dbCommand.ExecuteNonQuery();
			} catch ( SqlException e ) {
				throw e;
			} finally {
				dbCommand.Connection.Close();
			}
		}

		/// <summary>
		/// Gets a list of all current authors.
		/// </summary>
		/// <returns>The resulting list of authors.</returns>
		public static IList getAll() {
			return getAll( -1 );
		}

		/// <summary>
		/// Gets an author's information.
		/// </summary>
		/// <param name="authorID">The identifier of the author for which to obtain information.</param>
		/// <returns>An object encapsulating the given author's information.</returns>
		public static AuthorInfo getAuthorInfo( string username ) {
			SqlCommand sqlCommand = new SqlCommand();
			sqlCommand.Connection =
				new SqlConnection( Globals.UsersConnectionString );
			sqlCommand.CommandText = "SELECT UserName, Name, Email FROM UserInfo " +
				"WHERE UserName = @UserName";
			sqlCommand.Parameters.Add(new SqlParameter("@UserName", username));

			sqlCommand.Connection.Open();

			SqlDataReader authorInfo = sqlCommand.ExecuteReader( CommandBehavior.CloseConnection );
			authorInfo.Read();

			AuthorInfo retVal = new AuthorInfo( authorInfo.GetString( 0 ).Trim(),
												authorInfo.GetString( 1 ).Trim(),
												authorInfo.GetString( 2 ).Trim() );

			authorInfo.Close();

			return retVal;
		}
    }
}
