<%@ Page language="c#" Codebehind="addMaterialRating.aspx.cs" AutoEventWireup="false" Inherits="SwenetDev.addMaterialRating" %>
<%@ Register TagPrefix="uc1" TagName="sidebar" Src="Controls\Sidebar.ascx" %>
<%@ Register TagPrefix="uc1" TagName="header" Src="Controls\Header.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>addMaterialRating</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="style.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body>
		<form runat="server">
			<uc1:header id="Header2" runat="server"></uc1:header>
			<table width="750" cellspacing="1" cellpadding="1">
				<tr>
					<td valign="top" width="150"><uc1:sidebar id="Sidebar1" runat="server"></uc1:sidebar></td>
					<td width="600" valign="top">
						<p align="center" Runat="server"><asp:label id="TitleLabel" Font-Underline="True" Runat="server"><strong>Add 
									Material Rating</strong></asp:label></p>
							<DIV style="BORDER-RIGHT: black 1px solid; PADDING-RIGHT: 0px; BORDER-TOP: black 1px solid; PADDING-LEFT: 3px; PADDING-BOTTOM: 0px; FONT: 10pt verdana; BORDER-LEFT: black 1px solid; WIDTH: 550px; PADDING-TOP: 0px; BORDER-BOTTOM: black 1px solid; BACKGROUND-COLOR: #669999"><STRONG>Review 
									-
									<asp:Label id="MaterialIdent" Runat="server" ForeColor="#ffffff"></asp:Label></STRONG>
								<asp:HyperLink id="MaterialLink" Runat="server"></asp:HyperLink></DIV>
							<DIV style="BORDER-RIGHT: black 1px solid; PADDING-RIGHT: 0px; BORDER-TOP: black 1px solid; PADDING-LEFT: 3px; PADDING-BOTTOM: 25px; FONT: 10pt verdana; BORDER-LEFT: black 1px solid; WIDTH: 550px; PADDING-TOP: 6px; BORDER-BOTTOM: black 1px solid; BACKGROUND-COLOR: #cccccc">
								<DIV style="BORDER-RIGHT: black 1px solid; PADDING-RIGHT: 0px; BORDER-TOP: black 1px solid; PADDING-LEFT: 6px; PADDING-BOTTOM: 25px; FONT: 10pt verdana; BORDER-LEFT: black 1px solid; WIDTH: 535px; PADDING-TOP: 6px; BORDER-BOTTOM: black 1px solid; BACKGROUND-COLOR: #cccccc"><U><STRONG>1. 
										Rating</U> </STRONG><I>(5 being the best)</I><BR>
									<BR>
									<asp:radiobuttonlist id="RButtonList" Runat="server" BorderStyle="Inset" BackColor="#ffffff" TextAlign="Left"
										RepeatDirection="Horizontal" CellSpacing="4" CellPadding="4">
										<asp:ListItem ID="RButton1" Value="1" />
										<asp:ListItem ID="RButton2" Value="2" />
										<asp:ListItem id="RButton3" Value="3" />
										<asp:ListItem id="RButton4" Value="4" />
										<asp:ListItem id="RButton5" Value="5" Selected="True" />
									</asp:radiobuttonlist></DIV>
								<BR>
								<DIV style="BORDER-RIGHT: black 1px solid; PADDING-RIGHT: 0px; BORDER-TOP: black 1px solid; PADDING-LEFT: 6px; PADDING-BOTTOM: 25px; FONT: 10pt verdana; BORDER-LEFT: black 1px solid; WIDTH: 535px; PADDING-TOP: 6px; BORDER-BOTTOM: black 1px solid; BACKGROUND-COLOR: #cccccc">
									<asp:Label id="Comments" Runat="server">
							<u><strong>2. Comments</u> </strong><i>(Optional)</i>
						</asp:Label><BR>
									<BR>
									<P>
										<asp:Label id="SubjectLabel" runat="server"><strong>Subject:</strong>&nbsp;&nbsp;&nbsp;</asp:Label><BR>
										<asp:TextBox id="SubjectText" runat="server" MaxLength="55" Columns="50" ReadOnly="False"></asp:TextBox><BR>
										<asp:RegularExpressionValidator id="SubjectValidator" runat="server" Display="Dynamic" ValidationExpression=".{1,55}"
											ErrorMessage="Entry exceeds maximum length of 100" ControlToValidate="SubjectText"></asp:RegularExpressionValidator></P>
									<P>
										<asp:Label id="CommentLabel" Runat="server">
											<strong>Comments:</strong> <i>(Max: 450 Characters)</i></asp:Label><BR>
										<asp:TextBox id="CommentText" runat="server" MaxLength="450" Columns="62" ReadOnly="False" Width="400px"
											TextMode="MultiLine" Rows="5"></asp:TextBox><BR>
										<asp:regularexpressionvalidator id="CommentingValidator" runat="server" display="Dynamic" validationexpression=".{1,450}"
											errormessage="Entry exceeds maximum length of 450" controltovalidate="CommentText"></asp:regularexpressionvalidator></P>
								</DIV>
								<BR>
								<asp:button id="SubmitButton" onclick="Submit_Btn" Runat="server" Text="Submit Rating"></asp:button>
								<asp:Button id="CancelButton" onclick="Cancel_Btn" Runat="server" Text="Cancel"></asp:Button><BR>
							</DIV>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
