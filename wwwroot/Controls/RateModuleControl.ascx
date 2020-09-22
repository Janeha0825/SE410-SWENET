<%@ Control Language="c#" AutoEventWireup="false" Codebehind="RateModuleControl.ascx.cs" Inherits="SwenetDev.Controls.RateModuleControl" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<table cellspacing="0" cellpadding="0">
	<% if ( UserRating != null ) { %>
	<tr>
		<td><asp:hyperlink id="YourRatingLnk" runat="server">Your Rating:</asp:hyperlink></td>
		<td><asp:label id="YourRatingLbl" runat="server" font-bold="True"></asp:label><strong>/ 
				5</strong></td>
	</tr>
	<% } %>
	<tr>
		<td>Number of Ratings:&nbsp;&nbsp;</td>
		<td><asp:label id="NumRatingsLbl" runat="server" font-bold="True"></asp:label></td>
	</tr>
	<% if ( RatingInfo != null && RatingInfo.NumRatings > 0 ) { %>
	<tr>
		<td>Average Rating:</td>
		<td><asp:label id="AveRatingLbl" runat="server" font-bold="True"></asp:label><strong>&nbsp;/ 
				5</strong></td>
	</tr>
	<tr>
		<td colspan="2"><asp:hyperlink id="HyperLink1" runat="server" target="_blank">Current Rating Comments</asp:hyperlink></td>
	</tr>
	<% } %>
</table>
<% if ( !Context.User.Identity.IsAuthenticated ) { %>
<p><asp:hyperlink id="RateLoginLnk" runat="server">Log in</asp:hyperlink>&nbsp;to 
	rate this module.</p>
<% } %>
<br>
<table class="borderedTable" id="RatingsTable" cellspacing="0" cellpadding="5" rules="all"
	align="center" border="1" runat="server">
	<tr>
		<td bgcolor="#dedede">
			<h3>Add a Rating and Comments</h3>
			<asp:label id="ErrorMessage" runat="server" forecolor="Red"></asp:label>
			<p><strong>Your Rating (5 is best):</strong></p>
			<p>
				<table id="Table1" cellspacing="0" cellpadding="2" border="0">
					<tr>
						<td align="center">1</td>
						<td align="center">2</td>
						<td align="center">3</td>
						<td align="center">4</td>
						<td align="center">5</td>
					</tr>
					<tr>
						<td align="center"><asp:radiobutton id="Rating1" runat="server" groupname="RatingGroup"></asp:radiobutton></td>
						<td align="center"><asp:radiobutton id="Rating2" runat="server" groupname="RatingGroup"></asp:radiobutton></td>
						<td align="center"><asp:radiobutton id="Rating3" runat="server" groupname="RatingGroup"></asp:radiobutton></td>
						<td align="center"><asp:radiobutton id="Rating4" runat="server" groupname="RatingGroup"></asp:radiobutton></td>
						<td align="center"><asp:radiobutton id="Rating5" runat="server" groupname="RatingGroup"></asp:radiobutton></td>
					</tr>
				</table>
			</p>
			<p><strong>Optionally, add comments to accompany your rating:</strong></p>
			<p><strong>Subject<br>
					<asp:textbox id="SubjectTxtBox" runat="server" columns="30"></asp:textbox></strong></p>
			<p><strong>Comments</strong><br>
				<asp:textbox id="CommentsTxtBox" runat="server" columns="60" rows="9" textmode="MultiLine"></asp:textbox></p>
			<p><asp:button id="SubmitBtn" runat="server" text="Submit Rating"></asp:button></p>
		</td>
	</tr>
</table>
