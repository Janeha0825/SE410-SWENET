using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Collections;

namespace SwenetDev.DBAdapter {

	/// <summary>
	/// An enumeration of user roles.  The roles are hierarchical--users
	/// with a higher role have the privlidges of users below it in
	/// addition to their own.
	/// </summary>
	public enum UserRole {
		/// <summary>
		/// An anonymous user who hasn't logged in.
		/// </summary>
		Anonymous = -1,

		/// <summary>
		/// A basic user who has logged in and can view all information
		/// and make posts in the forums.
		/// </summary>
		User = 0,

		/// <summary>
		/// A confirmed faculty member.
		/// </summary>
		Faculty = 1,
		
		/// <summary>
		/// A user who may submit modules.
		/// </summary>
		Submitter = 2,
		
		/// <summary>
		/// A user who may approve or deny modules.
		/// </summary>
		Editor = 3,
		
		/// <summary>
		/// An administrator who has advanced privileges for administering the system.
		/// </summary>
		Admin = 4,
	
		/// <summary>
		/// A user who has cancelled his/her membership.
		/// </summary>
		Canceled = 5,
	
		/// <summary>
		/// A user whose account has been disabled by an Admin
		/// </summary>
		Disabled = 6};

		

	/// <summary>
	/// A class for managing users and user information in the database.
	/// </summary>
	public class UserAccounts {
		public class UserInfo {
			private string username = "";
			private string password = "";
			private int questionID = -1;
			private string questionAnswer = "";
			private string name = "";
			private string email = "";
			private string title = "";
			private string affiliation = "";
			private string street1 = "";
			private string street2 = "";
			private string city = "";
			private string state = "";
			private string zip = "";
			private string country = "";
			private string phone = "";
			private string countryCode = "";
			private string faxCountryCode = "";
			private string extension = "";
			private string fax = "";
			private string webpage = "";
			private string submitterID = "";
			private string lastModuleID = "";
			private UserRole role = UserRole.User;

			public string Username {
				get { return username; }
				set { username = value; }
			}
			public string Password {
				get { return password; }
				set { password = value; }
			}
			public int QuestionID {
				get { return questionID; }
				set { questionID = value; }
			}
			public string QuestionAnswer {
				get { return questionAnswer; }
				set { questionAnswer = value; }
			}
			public UserRole Role {
				get { return role; }
				set { role = value; }
			}
			public string Name {
				get { return name; }
				set { name = value; }
			}
			public string Email {
				get { return email; }
				set { email = value; }
			}
			public string Title {
				get { return title; }
				set { title = value; }
			}
			public string Affiliation {
				get { return affiliation; }
				set { affiliation = value; }
			}
			public string Street1 {
				get { return street1; }
				set { street1 = value; }
			}
			public string Street2 {
				get { return street2; }
				set { street2 = value; }
			}
			public string City {
				get { return city; }
				set { city = value; }
			}
			public string State {
				get { return state; }
				set { state = value; }
			}
			public string Zip {
				get { return zip; }
				set { zip = value; }
			}
			public string Country {
				get { return country; }
				set { country = value; }
			}
			public string Phone {
				get { return phone; }
				set { phone = value; }
			}
			public string PhoneCountryCode {
				get { return countryCode; }
				set { countryCode = value; }
			}
			public string PhoneExtension {
				get { return extension; }
				set { extension = value; }
			}
			public string Fax {
				get { return fax; }
				set { fax = value; }
			}
			public string FaxCountryCode {
				get { return faxCountryCode; }
				set { faxCountryCode = value; }
			}
			public string Webpage {
				get { return webpage; }
				set { webpage = value; }
			}
			public string SubmitterID {
				get { return submitterID; }
				set { submitterID = value; }
			}
			public string LastModuleID {
				get { return lastModuleID; }
				set { lastModuleID = value; }
			}
		}

		/// <summary>
		/// Add a user to the users table.
		/// </summary>
		/// <param name="userName">A unique username.</param>
		/// <param name="passwordHash">Password hash.</param>
		/// <param name="salt">Password salt.</param>
		/// <param name="role">User role.</param>
		public static void addUser( string userName,
			string passwordHash,
			string salt, UserRole role ) {
			// See "How To Use DPAPI (Machine Store) from ASP.NET" for information 
			// about securely storing connection strings.
			SqlConnection conn =
				new SqlConnection( Globals.UsersConnectionString );

			SqlCommand cmd = new SqlCommand("RegisterUser", conn );
			cmd.CommandType = CommandType.StoredProcedure;
			SqlParameter sqlParam = null;

			sqlParam = cmd.Parameters.Add("@userName", SqlDbType.VarChar, 255);
			sqlParam.Value = userName;

			sqlParam = cmd.Parameters.Add("@passwordHash", SqlDbType.VarChar, 40);
			sqlParam.Value = passwordHash;

			sqlParam = cmd.Parameters.Add("@salt", SqlDbType.VarChar, 10);
			sqlParam.Value = salt;

			sqlParam = cmd.Parameters.Add("@role", SqlDbType.Int);
			sqlParam.Value = (int)role;

			executeCommand( cmd, "Exception adding account. " );
		}

		/// <summary>
		/// Add detailed user info.
		/// </summary>
		/// <param name="ui">The user information to add.</param>
		public static void addUserInfo( UserInfo ui ) {
			SqlCommand dbCommand = new SqlCommand();
			dbCommand.CommandText = "INSERT INTO UserInfo " +
				"(UserName, Name, Email, Title, Affiliation, Street1, " +
				"Street2, City, State, Zip, Country, PhoneCountry, Phone, " +
				"PhoneExtension, FaxCountry, Fax, Webpage, QuestionID, QuestionAnswer, " +
				"SubmitterID, LastModuleID) " +
				"VALUES (@UserName, @Name, @Email, @Title, @Affiliation, @Street1, " +
				"@Street2, @City, @State, @Zip, @Country, @PhoneCountry, @Phone, " +
				"@PhoneExtension, @FaxCountry, @Fax, @Webpage, @QuestionID, @QuestionAnswer, " +
				"@SubmitterID, @LastModuleID)";
			dbCommand.Connection =
				new SqlConnection( Globals.UsersConnectionString );

			addUserInfoParams( dbCommand, ui );

			try {
				dbCommand.Connection.Open();
				dbCommand.ExecuteNonQuery();
			} catch ( Exception e ) {
				throw new Exception( "Insert new user failed." + e.Message, e );
			} finally {
				dbCommand.Connection.Close();
			}
		}

		/// <summary>
		/// Obtain a user's information.
		/// </summary>
		/// <param name="username">The user for which to obtain information.</param>
		/// <returns>The user's info.</returns>
		public static UserInfo getUserInfo( string username ) {
			IList list = getUserOrUsers( username );
			
			if( list.Count == 0 )
				return null;
			else
				return (UserInfo)list[0];
		}

		/// <summary>
		/// Get all registered users.
		/// </summary>
		/// <returns>A list of all registered users.</returns>
		public static IList getAll() {
			return getUserOrUsers( null );
		}

		/// <summary>
		/// Get a list of all users or one user of the given username.
		/// </summary>
		/// <param name="username">
		/// The user to get or null to get all users.
		/// </param>
		/// <returns>
		/// A list of users containing either one user or all users in the system.
		/// </returns>
		private static IList getUserOrUsers( string username ) {
			SqlCommand cmd = new SqlCommand();
			cmd.CommandText = "SELECT " +
				"U.UserName, Name, Email, Title, Affiliation, Street1, Street2, " +
				"City, State, Zip, Country, PhoneCountry, Phone, PhoneExtension, " +
				"FaxCountry, Fax, Webpage, U.Role, QuestionID, QuestionAnswer, " +
				"SubmitterID, LastModuleID " +
				"FROM UserInfo UI INNER JOIN Users U ON U.UserName = UI.UserName";
			cmd.Connection =
				new SqlConnection( Globals.UsersConnectionString );

			if ( username != null ) {
				cmd.CommandText += " WHERE UI.UserName = @UserName";
				cmd.Parameters.Add( new SqlParameter( "@UserName", username ) );
			}
			
			IList list = new ArrayList();
			IDataReader reader = null;

			try 
			{
				cmd.Connection.Open();
				reader = cmd.ExecuteReader( CommandBehavior.CloseConnection );

				while ( reader.Read() ) {
					UserInfo ui = new UserInfo();
					ui.Username = (string)reader["UserName"];
					ui.Name = (string)reader["Name"];
					ui.Email = (string)reader["Email"];
					ui.Title = (string)reader["Title"];
					ui.Affiliation = (string)reader["Affiliation"];
					ui.Street1 = (string)reader["Street1"];
					ui.Street2 = (string)reader["Street2"];
					ui.City = (string)reader["City"];
					ui.State = (string)reader["State"];
					ui.Zip = (string)reader["Zip"];
					ui.Country = (string)reader["Country"];
					ui.PhoneCountryCode = ((string)reader["PhoneCountry"]).Trim();
					ui.Phone = (string)reader["Phone"];
					ui.PhoneExtension = (string)reader["PhoneExtension"];
					ui.FaxCountryCode = ((string)reader["FaxCountry"]).Trim();
					ui.Fax = (string)reader["Fax"];
					ui.Webpage = (string)reader["Webpage"];
					ui.Role = (UserRole)reader["Role"];
					ui.QuestionID = (int)reader["QuestionID"];
					ui.QuestionAnswer = (string)reader["QuestionAnswer"];
					ui.SubmitterID = (string)reader["SubmitterID"];
					ui.LastModuleID = (string)reader["LastModuleID"];
					list.Add( ui );
				}
			} catch ( SqlException e ) {
				throw new Exception( "Get user info failed. ", e );
			} finally {
				reader.Close();
			}

			return list;
		}

		/// <summary>
		/// Check to see if the supplied password is valid for the user.
		/// </summary>
		/// <param name="suppliedUserName">
		/// The user in question.</param>
		/// <param name="suppliedPassword">
		/// The password to verify.
		/// </param>
		/// <returns>
		/// True, if the password is valid.  False if the password was
		/// invalid or the user doesn't exist.
		/// </returns>
		/// <exception cref=""
		public static bool VerifyPassword(string suppliedUserName,
			string suppliedPassword ) { 
			bool passwordMatch = false;
			// Get the salt and pwd from the database based on the user name.
			// See "How To: Use DPAPI (Machine Store) from ASP.NET," "How To:
			// Use DPAPI (User Store) from Enterprise Services," and "How To:
			// Create a DPAPI Library" for more information about how to use
			// DPAPI to securely store connection strings.
			SqlConnection conn =
				new SqlConnection( Globals.UsersConnectionString );

			SqlCommand cmd = new SqlCommand( "LookupUser", conn );
			cmd.CommandType = CommandType.StoredProcedure;

			SqlParameter sqlParam = cmd.Parameters.Add("@userName",
				SqlDbType.VarChar,
				255);
			sqlParam.Value = suppliedUserName;
			SqlDataReader reader = null;

			try 
			{
				conn.Open();
				reader = cmd.ExecuteReader( CommandBehavior.CloseConnection );

				// Advance to the one and only row
				if ( reader.Read() ) {
					// Return output parameters from returned data stream
					string dbPasswordHash = Convert.ToString( reader["PasswordHash"] );
					string salt = Convert.ToString( reader["salt"] );
					UserRole role = (UserRole)(Convert.ToInt32( reader["Role"] ) );
					reader.Close();
					// Now take the salt and the password entered by the user
					// and concatenate them together.
					string passwordAndSalt = String.Concat(suppliedPassword, salt);
					// Now hash them
					string hashedPasswordAndSalt =       
						FormsAuthentication.HashPasswordForStoringInConfigFile(
						passwordAndSalt,
						"SHA1");
					// Now verify them.
					passwordMatch = hashedPasswordAndSalt.Equals(dbPasswordHash);

					// If passwords match, create authentication ticket.
					if ( passwordMatch ) {
						string roles = role.ToString();
						
						if ( role == UserRole.Faculty || role == UserRole.Submitter || role == UserRole.Editor || role == UserRole.Admin ) {
							roles += "|" + UserRole.User.ToString();
						}
						if ( role == UserRole.Submitter || role == UserRole.Editor || role == UserRole.Admin ) 
						{
							roles += "|" + UserRole.Faculty.ToString();
						}
						if ( role == UserRole.Editor || role == UserRole.Admin ) {
							roles += "|" + UserRole.Submitter.ToString();
						}
						if ( role == UserRole.Admin ) {
							roles += "|" + UserRole.Editor.ToString();
						}

						FormsAuthenticationTicket authTicket = new
							FormsAuthenticationTicket( 1, // version
							suppliedUserName,             // user name
							DateTime.Now,                 // creation
							DateTime.Now.AddMinutes( Globals.Timeout ),  // Expiration
							false,                        // Persistent
							roles );		              // User data
						// Now encrypt the ticket.
						string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
						// Create a cookie and add the encrypted ticket to the
						// cookie as data.
						HttpCookie authCookie = 
							new HttpCookie(FormsAuthentication.FormsCookieName,
							encryptedTicket);
						HttpContext.Current.Response.Cookies.Add( authCookie );
					}
				} else {
					passwordMatch = false;
				}
			} catch (Exception ex) {
				throw new Exception("Exception verifying password. " +
					ex.Message);
			} finally {
				reader.Close();
			}
			
			return passwordMatch;
		}

		/// <summary>
		/// Update a user's role.
		/// </summary>
		/// <param name="user">
		/// The user object with an updated role.
		/// </param>
		public static void updateUserRole( UserInfo user ) {
			IDbCommand cmd = new SqlCommand();
			cmd.Connection = new SqlConnection( Globals.UsersConnectionString );
			cmd.CommandText = "UPDATE Users SET Role = @Role " +
				"WHERE UserName = @UserName";
			cmd.Parameters.Add( new SqlParameter( "@Role", user.Role ) );
			cmd.Parameters.Add( new SqlParameter( "@UserName", user.Username ) );

			executeCommand( cmd, "Failed to update role. " );
		}

		/// <summary>
		/// Change a user's password.
		/// </summary>
		/// <param name="username">Username to change password for.</param>
		/// <param name="hash">Password hash.</param>
		/// <param name="salt">Password salt.</param>
		public static void changePassword( string username, string hash, string salt ) {
			IDbCommand cmd = new SqlCommand();
			cmd.Connection = new SqlConnection( Globals.UsersConnectionString );
			cmd.CommandText = "UPDATE Users SET PasswordHash = @Hash, Salt = @Salt " +
				"WHERE UserName = @UserName";
			cmd.Parameters.Add( new SqlParameter( "@UserName", username ) );
			cmd.Parameters.Add( new SqlParameter( "@Hash", hash ) );
			cmd.Parameters.Add( new SqlParameter( "@Salt", salt ) );
            			
			executeCommand( cmd, "Failed to change password. " );
		}

		/// <summary>
		/// Set the submitter identifier for a user when their submitter status is approved.
		/// </summary>
		/// <param name="username"></param>
		/// <param name="submitterId"></param>
		public static void setSubmitterId( string username, string submitterId ) {
			IDbCommand cmd = new SqlCommand();
			cmd.Connection = new SqlConnection( Globals.UsersConnectionString );
			cmd.CommandText = "UPDATE UserInfo SET SubmitterId = @SubmitterId " +
				"WHERE UserName = @UserName";
			cmd.Parameters.Add( new SqlParameter( "@UserName", username ) );
			cmd.Parameters.Add( new SqlParameter( "@SubmitterId", submitterId ) );
            			
			executeCommand( cmd, "Failed to set submitter identifier. " );
		}

		/// <summary>
		/// Update a user's extended information.
		/// </summary>
		/// <param name="user">The user information to update with.</param>
		public static void updateUserInfo( UserInfo user ) {
			IDbCommand cmd = new SqlCommand();
			cmd.Connection = new SqlConnection( Globals.UsersConnectionString );
			cmd.CommandText = "UPDATE UserInfo SET " +
				"Name = @Name, Email = @Email, Title = @Title, Affiliation = @Affiliation, " +
				"Street1 = @Street1, Street2 = @Street2, City = @City, State = @State, " +
				"Zip = @Zip, Country = @Country, PhoneCountry = @PhoneCountry, Phone = @Phone, " +
				"PhoneExtension = @PhoneExtension, FaxCountry = @FaxCountry, Fax = @Fax, " +
				"Webpage = @Webpage, QuestionID = @QuestionID, QuestionAnswer = @QuestionAnswer " +
				"WHERE UserName = @UserName";
			addUserInfoParams( cmd, user );

			executeCommand( cmd, "Failed to update user info. " );
		}

		/// <summary>
		/// Determine if there is a submitter with the given submitter id.
		/// </summary>
		/// <param name="id">The submitter id to search for.</param>
		/// <returns>True if an existing submitter is found.</returns>
		public static bool submitterIdExists( string id ) {
			bool retVal = false;

			IDbCommand cmd = new SqlCommand();
			cmd.Connection = new SqlConnection( Globals.UsersConnectionString );
			cmd.Parameters.Add( new SqlParameter( "@SubmitterId", id ) );
			cmd.CommandText = "SELECT SubmitterId FROM UserInfo " +
				"WHERE SubmitterId = @SubmitterId";

			IDataReader reader = null;

			try 
			{
				cmd.Connection.Open();
				reader = cmd.ExecuteReader( CommandBehavior.CloseConnection );

				retVal = reader.Read();
			} finally {
				reader.Close();
			}

			return retVal;
		}

		/// <summary>
		/// Executes a non-query command and uses the given
		/// message if an exception is thrown.
		/// </summary>
		/// <param name="cmd">
		/// The command to execute.
		/// </param>
		/// <param name="message">
		/// The message to use if an exception is thrown.
		/// </param>
		private static void executeCommand( IDbCommand cmd, string message ) {
			try {
				cmd.Connection.Open();
				cmd.ExecuteNonQuery();
			} catch ( SqlException e ) {
				throw new Exception( message, e );
			} finally {
				cmd.Connection.Close();
			}
		}

		/// <summary>
		/// Add the user info parameters to the given database command.
		/// </summary>
		/// <param name="dbCommand">
		/// The command to add parameters to.
		/// </param>
		/// <param name="ui">
		/// The object containing the parameter values.
		/// </param>
		private static void addUserInfoParams( IDbCommand dbCommand, UserInfo ui ) {
			dbCommand.Parameters.Add( new SqlParameter( "@UserName", ui.Username ) );
			dbCommand.Parameters.Add( new SqlParameter( "@Name", ui.Name ) );
			dbCommand.Parameters.Add( new SqlParameter( "@Email", ui.Email ) );
			dbCommand.Parameters.Add( new SqlParameter( "@Title", ui.Title ) );
			dbCommand.Parameters.Add( new SqlParameter( "@Affiliation", ui.Affiliation ) );
			dbCommand.Parameters.Add( new SqlParameter( "@Street1", ui.Street1 ) );
			dbCommand.Parameters.Add( new SqlParameter( "@Street2", ui.Street2 ) );
			dbCommand.Parameters.Add( new SqlParameter( "@City", ui.City ) );
			dbCommand.Parameters.Add( new SqlParameter( "@State", ui.State ) );
			dbCommand.Parameters.Add( new SqlParameter( "@Country", ui.Country ) );
			dbCommand.Parameters.Add( new SqlParameter( "@Zip", ui.Zip ) );
			dbCommand.Parameters.Add( new SqlParameter( "@PhoneCountry", ui.PhoneCountryCode ) );
			dbCommand.Parameters.Add( new SqlParameter( "@Phone", ui.Phone ) );
			dbCommand.Parameters.Add( new SqlParameter( "@PhoneExtension", ui.PhoneExtension) );
			dbCommand.Parameters.Add( new SqlParameter( "@FaxCountry", ui.FaxCountryCode ) );
			dbCommand.Parameters.Add( new SqlParameter( "@Fax", ui.Fax ) );
			dbCommand.Parameters.Add( new SqlParameter( "@Webpage", ui.Webpage ) );
			dbCommand.Parameters.Add( new SqlParameter( "@QuestionID", ui.QuestionID ) );
			dbCommand.Parameters.Add( new SqlParameter( "@QuestionAnswer", ui.QuestionAnswer ) );
			dbCommand.Parameters.Add( new SqlParameter( "@SubmitterID", String.Empty ) );
			dbCommand.Parameters.Add( new SqlParameter( "@LastModuleID", String.Empty ) );
		}
	}
}
