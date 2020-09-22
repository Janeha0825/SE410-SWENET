using System;
using System.Text;
using System.Web.Security;
using System.Web.Mail;
using System.Security.Cryptography;
using System.Data.SqlClient;
using AspNetForums;
using AspNetForums.Components;

namespace SwenetDev {
	using DBAdapter;
	/// <summary>
	/// Summary description for UsersControl.
	/// </summary>
	public class UsersControl {
		public const int SALT_SIZE = 5;

		/// <summary>
		/// Register the supplied user in the system.
		/// </summary>
		/// <param name="user">The user information to register.</param>
		public static void registerUser( UserAccounts.UserInfo user ) {
			// First create forums user with an empty password.  Passwords
			// are stored in the Swenet user database.
			User forumsUser = new User();
			forumsUser.Username = user.Username;
			forumsUser.Password = user.Username;  // Not used, but needed so it won't send an email.
			forumsUser.Email = user.Email;
			CreateUserStatus status = Users.CreateNewUser(forumsUser, false);

			// Determine if the account was created successfully
			// -- from AspNetForums\Controls\User\CreateUser.cs
			switch (status) {

					// Username already exists!
				case CreateUserStatus.DuplicateUsername:
					throw new Exception( "A user with this username already exists." );
					break;

					// Email already exists!
				case CreateUserStatus.DuplicateEmailAddress:
					throw new Exception( "A user with this email address already exists." );
					break;

					// Unknown failure has occurred!
				case CreateUserStatus.UnknownFailure:
					throw new Exception( "An unexpected failure has occurred.  Please notify the Web site administrator of this error." );
					break;

					// Everything went off fine, good
				case CreateUserStatus.Created:
					string salt = CreateSalt( SALT_SIZE );
					string passwordHash = CreatePasswordHash( user.Password, salt );

					try {
						UserAccounts.addUser( user.Username, passwordHash, salt, UserRole.User );
						try {
							UserAccounts.addUserInfo( user );
						} catch ( SqlException e1 ) {
							// TODO: Should delete user from swenet user database.
							// Rethrow to delete from forums database in outer catch.
							throw;
						}
					} catch ( SqlException e2 ) {
						// TODO: Should delete user from Forums database here.
						throw new Exception( "User not created." );
					}
					break;
			}

		}

		public static void updateUser( UserAccounts.UserInfo user ) {
			// Update password, if necessary
			if ( user.Password != string.Empty ) {
				string salt = CreateSalt( SALT_SIZE );
				string hash = CreatePasswordHash( user.Password, salt );
				UserAccounts.changePassword( user.Username, hash, salt );
			}

			// Update Secret Question info only if they provided info this time
			if ( user.QuestionAnswer == "" ) {
				user.QuestionID = UserAccounts.getUserInfo( user.Username ).QuestionID;
				user.QuestionAnswer = UserAccounts.getUserInfo( user.Username ).QuestionAnswer;
			}

			// Update user role and info
			UserAccounts.updateUserInfo( user );
		}

		public static void updateUserRole( UserAccounts.UserInfo user ) {
			UserAccounts.updateUserRole( user );
			
			// Update forums roles
			try 
			{
				if ( user.Role == UserRole.Submitter )
				{	
					UserAccounts.setSubmitterId( user.Username, user.Username );
				}
				if ( user.Role == UserRole.Editor ) 
				{
					UserRoles.AddUserToRole( user.Username, "Forum-Moderators" );
					UserRoles.RemoveUserFromRole( user.Username, "Forum-Administrators" );
					UserAccounts.setSubmitterId( user.Username, user.Username );
				} 
				else if ( user.Role == UserRole.Admin ) 
				{
					UserRoles.AddUserToRole( user.Username, "Forum-Moderators" );
					UserRoles.AddUserToRole( user.Username, "Forum-Administrators" );
					UserAccounts.setSubmitterId( user.Username, user.Username );
				} 
				else 
				{
					UserRoles.RemoveUserFromRole( user.Username, "Forum-Moderators" );
					UserRoles.RemoveUserFromRole( user.Username, "Forum-Administrators" );
				}
			} 
			catch( ArgumentException e ) 
			{
			}


			if ( Globals.EmailsEnabled ) {
				Email email = Emails.getEmail( EmailType.UserRoleChanged );
				Emails.formatEmail( email, user.Username );
				MailMessage msg = Emails.constructMailMessage( email, user.Username, Globals.AdminsEmail );
				SmtpMail.SmtpServer = AspNetForums.Components.Globals.SmtpServer;
				SmtpMail.Send( msg );
			}
		}

		/// <summary>
		/// Log a user out of the system.
		/// </summary>
		public static void logoutUser() {
			FormsAuthentication.SignOut();
			UserRoles.SignOut();
		}

		/// <summary>
		/// Log a user into the system.
		/// </summary>
		/// <param name="user">User to login.</param>
		/// <returns>True if the login was successful, false otherwise.</returns>
		public static bool loginUser( UserAccounts.UserInfo user ) {
			bool retVal = UserAccounts.VerifyPassword( user.Username, user.Password );

			// If the password is verified, the code below is needed for
			// proper functioning of the forums.
			if ( retVal ) {
				User forumsUser = new User();
				forumsUser.Username = user.Username;
				forumsUser.Password = "";
				Users.ValidUser( forumsUser );
			}

			return retVal;
		}

		/// <summary>
		/// Obtain information about a user's request for submitter privileges
		/// if it exists.
		/// </summary>
		/// <param name="username">
		/// The username for which to obtain the information.
		/// </param>
		/// <returns>
		/// The requested information, or null if the user does not have a
		/// request pending approval.
		/// </returns>
		public static SubmitterRequestInfo getSubmitterRequestInfo( string username ) {
			return SubmitterRequests.getSubmitterRequest( username );
		}

		/// <summary>
		/// Obtain information about a user's request for faculty privileges
		/// if it exists.
		/// </summary>
		/// <param name="username">
		/// The username for which to obtain the information.
		/// </param>
		/// <returns>
		/// The requested information, or null if the user does not have a
		/// request pending approval.
		/// </returns>
		public static FacultyRequestInfo getFacultyRequestInfo( string username ) 
		{
			return FacultyRequests.getFacultyRequest( username );
		}

		/// <summary>
		/// Add a request for submitter privileges.
		/// Notify Editors of the submission request.
		/// </summary>
		/// <param name="sri">
		/// The information about the request.
		/// </param>
		public static void addSubmitterRequest( SubmitterRequestInfo sri ) 
		{
			SubmitterRequests.addSubmitterRequest( sri );

			Email email = Emails.getEmail( EmailType.SubmitterRequest );
			Emails.formatEmail( email, sri.UserName );
			MailMessage msg = Emails.constructEditorsMail( email, Globals.AdminsEmail );
			SmtpMail.SmtpServer = AspNetForums.Components.Globals.SmtpServer;
			SmtpMail.Send( msg );
		}

		/// <summary>
		/// Add a request for faculty privileges.
		/// Notify Editors of the faculty request.
		/// </summary>
		/// <param name="sri">
		/// The information about the request.
		/// </param>
		public static void addFacultyRequest( FacultyRequestInfo fri ) 
		{
			FacultyRequests.addFacultyRequest( fri );

			Email email = Emails.getEmail( EmailType.ConfirmFaculty );
			Emails.formatEmail( email, fri.UserName );
			Emails.formatEmailBody( email, fri.Proof ); 
			MailMessage msg = Emails.constructEditorsMail( email, Globals.AdminsEmail );
			SmtpMail.SmtpServer = AspNetForums.Components.Globals.SmtpServer;
			SmtpMail.Send( msg );
		}

		/// <summary>
		/// Approve a user's request for submitter status.
		/// </summary>
		/// <param name="username">The username of the user to approve.</param>
		public static void approveSubmitter( string username ) 
		{
			UserAccounts.UserInfo user = new UserAccounts.UserInfo();

			user.Username = username;
			user.Role = UserRole.Submitter;

			UserAccounts.updateUserRole( user );
			SubmitterRequestInfo sri = SubmitterRequests.getSubmitterRequest( username );
			UserAccounts.setSubmitterId( username, sri.SubmitterId );
			SubmitterRequests.remove( username );
		}

		/// <summary>
		/// Approve a user's request for submitter status.
		/// </summary>
		/// <param name="username">The username of the user to approve.</param>
		public static void approveFaculty( string username ) 
		{
			UserAccounts.UserInfo user = new UserAccounts.UserInfo();

			user.Username = username;
			user.Role = UserRole.Faculty;

			UserAccounts.updateUserRole( user );
			FacultyRequestInfo fri = FacultyRequests.getFacultyRequest( username );
			FacultyRequests.remove( username );
		}

		public static void rejectSubmitter( string username ) 
		{
			SubmitterRequests.remove( username );
		}

		public static void rejectFaculty( string username ) 
		{
			FacultyRequests.remove( username );
		}

		/// <summary>
		/// Determine if there is a submitter request or an existing submitter
		/// with the given submitter id.
		/// </summary>
		/// <param name="id">The submitter id to search for.</param>
		/// <returns>True if an existing submitter or submitter request is found.</returns>
		public static bool submitterIdExists( string id ) 
		{
			return UserAccounts.submitterIdExists( id ) 
				|| SubmitterRequests.submitterIdExists( id );
		}

		private static string CreateSalt(int size) {
			// Generate a cryptographic random number using the cryptographic
			// service provider
			RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
			byte[] buff = new byte[size];
			rng.GetBytes(buff);
			// Return a Base64 string representation of the random number
			return Convert.ToBase64String(buff);
		}

		private static string CreatePasswordHash(string pwd, string salt) {
			string saltAndPwd = String.Concat(pwd, salt);
			string hashedPwd = 
				FormsAuthentication.HashPasswordForStoringInConfigFile(
				saltAndPwd, "SHA1");
			return hashedPwd;
		}
	}
}
