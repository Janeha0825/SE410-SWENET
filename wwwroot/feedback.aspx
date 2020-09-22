<%@ Register TagPrefix="uc1" TagName="header" Src="Controls\Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="sidebar" Src="Controls\Sidebar.ascx" %>
<%@ Page language="c#" Codebehind="feedback.aspx.cs" AutoEventWireup="false" Inherits="SwenetDev.feedback" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>Feedback</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="style.css" type="text/css" rel="stylesheet">
	</head>
	<body>
		<form id="Form1" method="post" runat="server">
			<uc1:header id="Header1" runat="server"></uc1:header>
			<table id="Table1" width="750">
				<tr>
					<td width="150" valign="top">
						<h1>
							<uc1:sidebar id="Sidebar1" runat="server"></uc1:sidebar></h1>
					</td>
					<td width="600" valign="top">
						<h1>Feedback</h1>
						<p>We would appreciate hearing from you.&nbsp; Please send us any feedback you 
							may have about the site--it's organization, content, or problems you've 
							encountered.&nbsp; Any suggestions you have are welcome.</p>
						<p>
							<table id="Table2" cellspacing="1" cellpadding="1" width="596" border="0">
								<tr>
									<td style="WIDTH: 120px"><strong>Name</strong></td>
									<td><strong>
											<asp:textbox id="NameTxt" runat="server" columns="30" maxlength="30"></asp:textbox></strong></td>
								</tr>
								<tr>
									<td style="WIDTH: 120px"><strong>Email Address</strong></td>
									<td><strong>
											<asp:textbox id="EmailTxt" runat="server" columns="30" maxlength="30"></asp:textbox></strong></td>
								</tr>
								<tr>
									<td style="WIDTH: 120px"><strong>Subject</strong></td>
									<td><strong>
											<asp:textbox id="SubjectTxt" runat="server" columns="50" maxlength="50"></asp:textbox></strong></td>
								</tr>
								<tr>
									<td style="WIDTH: 120px" valign="top"><strong>Message<br>
											(Limited to 1000 characters; Required)</strong></td>
									<td>
										<asp:textbox id="MessageTxt" runat="server" columns="55" maxlength="1000" textmode="MultiLine"
											rows="8"></asp:textbox><br>
										<asp:requiredfieldvalidator id="RequiredFieldValidator1" runat="server" errormessage="Message field is required."
											controltovalidate="MessageTxt"></asp:requiredfieldvalidator></td>
								</tr>
								<tr>
									<td style="WIDTH: 120px" valign="top"></td>
									<td>
										<asp:button id="SendBtn" runat="server" text="Send" cssclass="defaultButton"></asp:button></td>
								</tr>
							</table>
						</p>
					</td>
				</tr>
			</table>
		</form>
	</body>
</html>
