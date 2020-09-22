<%@ Page language="c#" Codebehind="forgotpwd.aspx.cs" AutoEventWireup="false" Inherits="SwenetDev.forgotpwd" %>
<%@ Register TagPrefix="uc1" TagName="SecretQuestion" Src="Controls/SecretQuestion.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SeeAlsoControl" Src="Controls/SeeAlsoControl.ascx" %>
<%@ Register TagPrefix="swenet" TagName="sidebar" Src="Controls\Sidebar.ascx" %>
<%@ Register TagPrefix="swenet" TagName="header" Src="Controls\Header.ascx" %>
<HTML>
	<HEAD>
		<title>Login</title>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="style.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body onload="window.document.loginForm.txtUserName.focus();">
		<form id="loginForm" runat="server">
			<swenet:header id="Header" runat="server"></swenet:header>
			<table id="Table1" cellSpacing="0" cellPadding="0">
				<tr>
					<td style="WIDTH: 159px" vAlign="top" width="140">
						<div id="categories"><swenet:sidebar id="Sidebar" runat="server"></swenet:sidebar></div>
					</td>
					<td vAlign="top" width="600">
						<h1>Login&nbsp;
						</h1>
						<p><asp:label id="lblMessage" runat="server"></asp:label></p>
						<table cellSpacing="1" cellPadding="1" style="WIDTH: 456px; HEIGHT: 24px">
							<tr>
								<td style="WIDTH: 120px" width="120" align="right"><strong>Username:</strong></td>
								<td><asp:textbox id="txtUserName" tabIndex="1" runat="server" maxlength="25" Width="200px"></asp:textbox><asp:requiredfieldvalidator id="RequiredFieldValidator" runat="server" controltovalidate="txtUserName">*</asp:requiredfieldvalidator></td>
							</tr>
						</table>
						<uc1:secretquestion id="secretQuestion" runat="server"></uc1:secretquestion>
						<P>
							<asp:button id="SubmitBtn" tabIndex="4" runat="server" CssClass="defaultButton" Text="Submit"></asp:button>&nbsp;
							<asp:button id="ContinueBtn" tabIndex="5" runat="server" CssClass="defaultButton" Text="Continue..."></asp:button></P>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
