using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;

namespace SwenetDev.DBAdapter
{
	/// <summary>
	/// Summary description for MaterialComments.
	/// </summary>
	public class MaterialComments {

		[Serializable()]
		public class MaterialCommentsInfo : Info {
			
			private string comments;
			private string subject;
			private string date;
			private string ratingImage;
			private string author;
			private int orderID;
			private int materialID;
			private double rating;

			public string Comments {
				get {
					return comments;
				}
				set {
					comments = value;
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
			public int MaterialID {
				get {
					return materialID;
				}
				set {
					materialID = value;
				}
			}
			public string Subject {
				get {
					return subject;
				}
				set {
					subject = value;
				}
			}
			public string Date {
				get {
					return date;
				}
				set {
					date = value;
				}
			}
			public double Rating {
				get {
					return rating;
				}
				set {
					rating = value;
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
			public string Author 
			{
				get 
				{
					return author;
				}
				set 
				{
					author = value;
				}
			}

			public MaterialCommentsInfo(int materialID, string comments, string subject, string date, double rating, string ratingImage, string author) {

				this.materialID  = materialID;
				this.comments    = comments;
				this.subject     = subject;
				this.date        = date;
				this.rating      = rating;
				this.ratingImage = ratingImage;
				this.author      = author;
				orderID	= 0;

			}
		}

		/// <summary>
		/// Returns all of the material comments associated with the passed materialID
		/// </summary>
		/// <param name="matID">The materialID of the material to get the material comments</param>
		/// <returns>Collection of all the material comments associated with the materialID</returns>
		public static IList getAll(int matID) {

			SqlCommand dbCommand = new SqlCommand();
			SqlConnection dbConnection = new SqlConnection(Globals.ConnectionString);
			dbCommand.Connection =  dbConnection;
			
			dbCommand.CommandText = "SELECT MaterialID, Comments, Subject, Date, Rating, RatingImage, Author FROM MaterialComments WHERE MaterialID = @MatID ORDER BY Date DESC";
			dbCommand.Parameters.Add(new SqlParameter("@MatID", matID));

			IList commentList = null;
			SqlDataReader dataReader = null;

			try {

				dbCommand.Connection.Open(); 
				dataReader = dbCommand.ExecuteReader(CommandBehavior.CloseConnection);
				commentList = new ArrayList();

				while (dataReader.Read()) {
				
					commentList.Add( new MaterialCommentsInfo(dataReader.GetInt32(0),
														      dataReader.GetString(1).Trim(),
															  dataReader.GetString(2).Trim(),
															  dataReader.GetString(3).Trim(),
															  dataReader.GetDouble(4),
															  dataReader.GetString(5).Trim(),
															  dataReader.GetString(6).Trim() )); 
				
				}

			} 
			catch (SqlException e) {
				throw;
			}
			finally {
				dataReader.Close();
			}

			return commentList;

		}

		/// <summary>
		/// Take the passed MaterialComment and add it to the database
		/// </summary>
		/// <param name="mci">The MaterialComment </param>
		public static void addComment(MaterialComments.MaterialCommentsInfo mci) 
		{

			IDbCommand dbCommand = new SqlCommand();
			SqlConnection dbConnection = new SqlConnection(Globals.ConnectionString);
			dbCommand.Connection = dbConnection;

			//Translate the NumericalRating into a VisualRating
			if (mci.Rating == 1) mci.RatingImage = "images/stars1.gif";
			else if (mci.Rating == 2) mci.RatingImage = "images/stars2.gif";
			else if (mci.Rating == 3) mci.RatingImage = "images/stars3.gif";
			else if (mci.Rating == 4) mci.RatingImage = "images/stars4.gif";
			else if (mci.Rating == 5) mci.RatingImage = "images/stars5.gif";
			else mci.RatingImage = "images/stars0.gif";

			dbCommand.CommandText = "INSERT INTO [MaterialComments] ([MaterialID], [Comments], [Subject], [Date], [Rating], [RatingImage], [Author]) VALUES (@MatID, @Comments, @Subject, @Date, @Rating, @RatingImage, @Author)";
			dbCommand.Parameters.Add(new SqlParameter("@MatID", mci.MaterialID));
			dbCommand.Parameters.Add(new SqlParameter("@Comments", mci.Comments));
			dbCommand.Parameters.Add(new SqlParameter("@Subject", mci.Subject));
			dbCommand.Parameters.Add(new SqlParameter("@Date", mci.Date));
			dbCommand.Parameters.Add(new SqlParameter("@Rating", mci.Rating));
			dbCommand.Parameters.Add(new SqlParameter("@RatingImage", mci.RatingImage));
			dbCommand.Parameters.Add(new SqlParameter("@Author", mci.Author));

			if (mci.Subject == String.Empty) mci.Subject = "N/A";
			
			try 
			{
				dbCommand.Connection.Open();
				dbCommand.ExecuteNonQuery();
			}
			catch (SqlException e) 
			{
				throw;
			}
			finally 
			{
				dbCommand.Connection.Close();
			}
		}
	}
}
