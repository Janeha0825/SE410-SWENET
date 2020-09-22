using System;
using System.Collections;
using System.Text;
using System.Web.Mail;
using System.Data;
using System.Data.SqlClient;

namespace SwenetDev.DBAdapter {
	public enum EmailType {
		SubmitterApproved = 1,
		SubmitterDenied = 2,
		ModuleApproved = 3,
		ModuleDeniedSave = 4,
		ModuleDeniedDelete = 5,
		UserRoleChanged = 6,
		PasswordReset = 7,
		SubmitterRequest = 8,
		ApproveModule = 9,
		ConfirmFaculty = 10,
		FacultyApproved = 11,
		FacultyDenied = 12,
		CriticalError = 13
	};

	/// <summary>
	/// A utility class for formatting and creating emails.
	/// </summary>
	public class Emails {
		public const string ModuleTitle = "<ModuleTitle>";
		public const string ModuleID = "<ModuleID>";
		public const string Username = "<UserName>";
		public const string Name = "<Name>";
		public const string CustomMessage = "<CustomMessage>";
		public const string SiteUrl = "<SiteURL>";
		public const string Role = "<Role>";
		public const string Password = "<Password>";

		/// <summary>
		/// Get a base email from the database.
		/// </summary>
		/// <param name="type">
		/// The email type to get (EmailType).
		/// </param>
		/// <returns>
		/// A SWEnet Email object.
		/// </returns>
		public static Email getEmail( EmailType type ) {
			IDbCommand cmd = new SqlCommand();
			cmd.Connection = new SqlConnection( Globals.ConnectionString );
			cmd.CommandText = "SELECT EmailID, Subject, Message " +
				"FROM Emails WHERE EmailID = @EmailID";
			cmd.Parameters.Add( new SqlParameter( "@EmailID", (int)type ) );
			
			Email message = null;
			IDataReader reader = null;

			try 
			{
				cmd.Connection.Open();
				reader = cmd.ExecuteReader( CommandBehavior.CloseConnection );

				if ( reader.Read() ) {
					message = new Email();
					message.Type = type;
					message.Subject = Convert.ToString( reader["Subject"] );
					message.Body = Convert.ToString( reader["Message"] );
				}
			} catch ( SqlException e ) {
				throw new Exception( "An error occurred while getting an email message.", e );
			} finally {
				reader.Close();
			}

			return message;
		}

		/// <summary>
		/// Format an email message with information for the given user.
		/// </summary>
		/// <param name="message">
		/// The message to format.
		/// </param>
		/// <param name="username">
		/// The username whose information should be used.
		/// </param>
		public static void formatEmail( Email message, string username ) {
			formatEmail( message, username, -1 );
		}

		/// <summary>
		/// Format an email message with information for the given user and module.
		/// </summary>
		/// <param name="message">
		/// The message to format.
		/// </param>
		/// <param name="username">
		/// The username whose information should be used.
		/// </param>
		/// <param name="moduleID">
		/// The module to get information for to use in formatting.
		/// </param>
		public static void formatEmail( Email message, string username, int moduleID ) {
			UserAccounts.UserInfo user = UserAccounts.getUserInfo( username );
			message.Subject = message.Subject.Replace( Username, user.Username );
			message.Body = message.Body.Replace( Name, user.Name );
			message.Body = message.Body.Replace( Emails.SiteUrl, Globals.SiteUrl );
			message.Body = message.Body.Replace( Emails.Role, user.Role.ToString() );

			if ( moduleID != -1 ) {
				Modules.ModuleInfo module = Modules.getModuleInfo( moduleID );
				message.Subject = message.Subject.Replace( ModuleTitle, module.Title );
				message.Body = message.Body.Replace( ModuleTitle, module.Title );
				message.Body = message.Body.Replace( ModuleID, moduleID.ToString() );
			}
		}

		/// <summary>
		/// Format an email message with information for the given user and password.
		/// </summary>
		/// <param name="message"></param>
		/// The message to format.
		/// <param name="username"></param>
		/// The username for the account.
		/// <param name="password"></param>
		/// The new password that has been set.
		public static void formatEmail( Email message, string username, string password ) {
			UserAccounts.UserInfo user = UserAccounts.getUserInfo( username );
			message.Body = message.Body.Replace( Name, user.Name );
			message.Body = message.Body.Replace( Password, password );
		}

		/// <summary>
		/// Format the email body with the given custom message.  Also converts
		/// br's into \n's.
		/// </summary>
		/// <param name="message">The message to format.</param>
		/// <param name="customMessage">The custom message to include.</param>
		public static void formatEmailBody( Email message, string customMessage ) {
			if ( message != null && customMessage != null ) {
				message.Body = message.Body.Replace( CustomMessage, customMessage );
				message.Body = message.Body.Replace( "<br>", "\n" );
			} else {
				throw new Exception( "Message is null." );
			}
		}

		/// <summary>
		/// Construct a <code>System.Web.Mail.MailMessage</code> object from the given 
		/// Swenet <code>Email</code> object and the from address.
		/// </summary>
		/// <param name="e">
		/// The email to construct the subject and body from.
		/// </param>
		/// <param name="toUsername">
		/// The destination username from which to determine the email address.
		/// </param>
		/// <param name="from">
		/// A sender's address.
		/// </param>
		/// <returns>
		/// A mail message with the subject and body of <code>e</code> and
		/// the given destination and sender addresses.</returns>
		public static MailMessage constructMailMessage( Email e, string toUsername, string from ) {
			MailMessage retVal = null;
			
			if ( e != null ) {
				retVal = new MailMessage();
				retVal.Subject = e.Subject;
				retVal.Body = e.Body;
				UserAccounts.UserInfo user = UserAccounts.getUserInfo( toUsername );
				retVal.To = user.Email;
				retVal.From = from;
			}

			return retVal;
		}

		/// <summary>
		/// Construct a <code>System.Web.Mail.MailMessage</code> object addressed to
		/// the Admins of Swenet from the given <code>Email</code> object and
		/// the from address
		/// </summary>
		public static MailMessage constructErrorMessage( Email e, string from ) {
			MailMessage retVal = null;

			if( e != null ) 
			{
				retVal = new MailMessage();
				retVal.Subject = e.Subject;
				retVal.Body = e.Body;
				retVal.To = getRoleEmail( (int)UserRole.Admin );
				retVal.From = from;
			}

			return retVal;
		}

		/// <summary>
		/// Construct a <code>System.Web.Mail.MailMessage</code> object addressed to
		/// the Editors of Swenet from the given Swenet <code>Email</code> object 
		/// and the from address.
		/// </summary>
		/// 
		public static MailMessage constructEditorsMail( Email e, string from ) {
			MailMessage retVal = null;

			if ( e != null ) {
				retVal = new MailMessage();
				retVal.Subject = e.Subject;
				retVal.Body = e.Body;
				retVal.To = getRoleEmail( (int)UserRole.Editor ) + getRoleEmail( (int)UserRole.Editor );
				retVal.From = from;
			}

			return retVal;

		}

		/// <summary>
		/// Creates a semicolon-delimited list of the specific role's e-mail addresses
		/// </summary>
		/// <param name="role">The int representation of the desired user role</param>
		public static string getRoleEmail( int role ) {
			string getEditors = "SELECT Email FROM UserInfo JOIN Users "
								+ "ON( UserInfo.UserName = Users.UserName ) "
								+ "WHERE Role = @Role";
			StringBuilder retVal = new StringBuilder();

			SqlConnection userConnect = new SqlConnection( Globals.UsersConnectionString );
			SqlCommand userCmd = new SqlCommand( getEditors, userConnect );

			userCmd.Parameters.Add( new SqlParameter( "@Role", role ) );

			SqlDataReader userReader = null;

			try {
				userConnect.Open();
				userReader = userCmd.ExecuteReader( CommandBehavior.CloseConnection );

				// Populate a list of editor emails
				while( userReader.Read() ) {
					retVal.Append( Convert.ToString( userReader["Email"] ) + ";" );
				}
			} catch( Exception e ) {
				throw new Exception( "An error occured while accumulating a list of user e-mails" );
			} finally {
				userReader.Close();
			}

			return retVal.ToString();
		}
	}
}
