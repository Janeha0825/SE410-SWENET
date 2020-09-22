<%@ Page Language="c#" CodeBehind="login.aspx.cs" AutoEventWireup="false" Inherits="SwenetDev.login" %>
<%@ Register TagPrefix="swenet" TagName="sidebar" Src="Controls\Sidebar.ascx" %>
<%@ Register TagPrefix="swenet" TagName="header" Src="Controls\Header.ascx" %>
<HTML>
	<HEAD>
		<title>Login</title>
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="style.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body onload="window.document.loginForm.txtUserName.focus();">
		<form id="loginForm" runat="server">
			<swenet:header id="Header" runat="server"></swenet:header>
			<table id="Table1" width="750" cellspacing="1" cellpadding="1">
				<tr>
					<td valign="top" width="150">
						<div id="categories">
							<swenet:sidebar id="Sidebar" runat="server"></swenet:sidebar>
						</div>
					</td>
					<td valign="top" width="600">
						<h1>Login&nbsp;
						</h1>
						<p>
							<asp:label id="lblMessage" runat="server"></asp:label>
							<asp:validationsummary id="ValidationSummary1" runat="server" displaymode="SingleParagraph" headertext="All fields are required."></asp:validationsummary></p>
						<p>
							<table cellspacing="1" cellpadding="1" width="300">
								<tr>
									<td align="right">
										<strong>Username:</strong></td>
									<td>
										<asp:textbox id="txtUserName" runat="server" tabindex="1" maxlength="25" width="150"></asp:textbox>
										<asp:requiredfieldvalidator id="RequiredFieldValidator1" runat="server" controltovalidate="txtUserName">*</asp:requiredfieldvalidator>
									</td>
								</tr>
								<tr>
									<td align="right">
										<strong>Password:</strong></td>
									<td>
										<asp:textbox id="txtPassword" runat="server" textmode="Password" tabindex="2" width="150"></asp:textbox>
										<asp:requiredfieldvalidator id="RequiredFieldValidator2" runat="server" controltovalidate="txtPassword">*</asp:requiredfieldvalidator></td>
								</tr>
								<tr>
									<td style="HEIGHT: 5px" align="center" colspan="2"></td>
								</tr>
								<tr>
									<td></td>
									<td>
										<asp:button id="LoginBtn" runat="server" text="Login" cssclass="defaultButton" tabindex="3"></asp:button></td>
								</tr>
							</table>
						</p>
						<p>Don't have an account?
							<asp:linkbutton id="RegisterLinkBtn" runat="server" font-bold="True" causesvalidation="False" tabindex="4">Register now.</asp:linkbutton></p>
						<P>Forgot your password?
							<asp:LinkButton id="ForgotPwdBtn" tabIndex="5" runat="server" Font-Bold="True" CausesValidation="False">Click here.</asp:LinkButton></P>
						<p>
						</p>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
