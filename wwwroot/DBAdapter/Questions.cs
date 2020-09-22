using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;

namespace SwenetDev.DBAdapter
{
	/// <summary>
	/// Summary description for Questions.
	/// </summary>
	public class Questions {

		public class Question {
			private int ID;
			private string question;

			public Question( int id, string question ) {
				ID = id;
				this.question = question;
			}

			public int getID() {
				return ID;
			}

			public string getQuestion() {
				return question;
			}
		}

		/// <summary>
		/// Returns the text of the Question associated with the given questionID.
		/// </summary>
		/// <param name="questionID">The questionID which points to the question text.</param>
		/// <returns>The question text associated with the passed questionID.</returns>
		public static Question getQuestion( int questionID ) {
	
			Question retQ = null;

			SqlCommand sqlSelectCommand = new SqlCommand();
			sqlSelectCommand.Connection = new SqlConnection( Globals.UsersConnectionString );
			sqlSelectCommand.CommandText = "SELECT QuestionID, QuestionText FROM Questions WHERE QuestionID = @QuestionID";
			sqlSelectCommand.Parameters.Add( new SqlParameter( "@QuestionID", questionID ) );

			SqlDataReader reader = null;

			try 
			{
				sqlSelectCommand.Connection.Open();
				reader = sqlSelectCommand.ExecuteReader( CommandBehavior.CloseConnection );

				while ( reader.Read() ) {
					retQ = new Question( reader.GetInt32( 0 ), reader.GetString( 1 ) );
				}
			} catch ( SqlException e ) {
				throw;
			} finally {
				reader.Close();
			}

			return retQ;
		}

		/// <summary>
		/// Returns a list of all of the questions used for the Secret Question control.
		/// </summary>
		/// <returns>The list of questions.</returns>
		public static IList getQuestions() {
			IList questions = new ArrayList();

			SqlCommand sqlSelectCommand = new SqlCommand();
			sqlSelectCommand.Connection = new SqlConnection( Globals.UsersConnectionString );
			sqlSelectCommand.CommandText = "SELECT QuestionID, QuestionText FROM Questions";
			
			SqlDataReader reader = null;

			try 
			{
				sqlSelectCommand.Connection.Open();
				reader = sqlSelectCommand.ExecuteReader( CommandBehavior.CloseConnection );

				while ( reader.Read() ) {
					questions.Add( new Question( reader.GetInt32( 0 ), reader.GetString( 1 ) ).getQuestion() );
				}
			} catch ( SqlException e ) {
				throw;
			} finally {
				reader.Close();
			}

			return questions;
		}
	}
}
