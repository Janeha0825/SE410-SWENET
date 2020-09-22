<%@ Page language="c#" Codebehind="materialRatings.aspx.cs" AutoEventWireup="false" Inherits="SwenetDev.materialRatings" %>
<%@ Register TagPrefix="uc1" TagName="header" Src="Controls\Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="sidebar" Src="Controls\Sidebar.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<style>

	.TopButtonStyle { BACKGROUND-COLOR: white; BORDER-BOTTOM: black 1px solid}
	
</style>
<HTML>
	<HEAD>
		<title>materialRatings</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="style.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<uc1:header id="Header1" runat="server"></uc1:header>
			<table width="750" cellspacing="1" cellpadding="1">
				<tr>
					<td valign="top" width="150"><uc1:sidebar id="Sidebar1" runat="server"></uc1:sidebar></td>
					<td valign="top" width="600">
						<p align="center"><strong><u>Material Ratings &amp; Comments</u></strong></p>
						<strong><u>Reviews</u> -  </strong><asp:Label Runat="server" ID="MaterialLabel"></asp:Label>
						<asp:HyperLink ID="MaterialLink" Runat="server"></asp:HyperLink><br>
						<a><strong>Average Rating: </strong>
							<asp:Label Runat="server" ID="NumericalRating"></asp:Label></a> &nbsp;&nbsp;&nbsp;&nbsp;
						<asp:Image ImageUrl="images/stars4.gif" Runat="server" ID="RatingImage" /><BR>
						<a><strong>Number of Ratings: </strong>
						<asp:Label Runat="server" ID="NumberOfRatings"></asp:Label></a>
						<asp:Panel HorizontalAlign="Right" Runat="server" id="DoneButtonTopPanel" CssClass="TopButtonStyle">
						<asp:Button id="DoneButtonTop" onclick="DoneButton_Click" runat="server" Width="72px" Text="Done" BorderStyle="Groove" BorderWidth="2px"></asp:Button>
						</asp:Panel>
						<div style="BORDER-RIGHT: black 1px solid; PADDING-RIGHT: 10px; BORDER-TOP: black 1px solid; PADDING-LEFT: 3px; PADDING-BOTTOM: 0px; FONT: 10pt verdana; BORDER-LEFT: black 1px solid; PADDING-TOP: 0px; BORDER-BOTTOM: black 1px solid; BACKGROUND-COLOR: #669999"><strong><u>Comments</u></strong></div>
						<div style="BORDER-RIGHT: black 1px solid; PADDING-RIGHT: 10px; BORDER-TOP: black 1px solid; PADDING-LEFT: 3px; PADDING-BOTTOM: 25px; FONT: 10pt verdana; BORDER-LEFT: black 1px solid; PADDING-TOP: 0px; BORDER-BOTTOM: black 1px solid; BACKGROUND-COLOR: #cccccc">
							<asp:Repeater id="CommentRepeater" Runat="server">
								<ItemTemplate>
									<div style="BORDER-RIGHT: black 1px solid; PADDING-RIGHT: 10px; BORDER-TOP: black 1px solid; PADDING-LEFT: 3px; PADDING-BOTTOM: 0px; FONT: 10pt verdana; BORDER-LEFT: black 1px solid; PADDING-TOP: 0px; BORDER-BOTTOM: black 1px solid; BACKGROUND-COLOR: #ffffff"> 
										<asp:Image ImageUrl='<%# DataBinder.Eval( Container.DataItem, "RatingImage" ) %>' Runat="server" ID="CommentImage" ImageAlign="Baseline"/>
										<strong>
											<%# DataBinder.Eval( Container.DataItem, "Subject" ) %>
											-
											<%# DataBinder.Eval( Container.DataItem, "Date" ) %>
											by
											<%# DataBinder.Eval( Container.DataItem, "Author" ) %>
										</strong>
									</div>
									<li>
										<%# DataBinder.Eval( Container.DataItem, "Comments" ) %>
									</li>
								</ItemTemplate>
								<SeparatorTemplate>
									<br>
									<br>
									<hr>
								</SeparatorTemplate>
							</asp:Repeater>
						</div>
						<p align="center"><asp:Button ID="DoneButtonBottom" Text="Done" Runat="server" OnClick="DoneButton_Click"></asp:Button></p>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
