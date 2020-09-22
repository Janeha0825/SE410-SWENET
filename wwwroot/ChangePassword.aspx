<%@ Page language="c#" Codebehind="ChangePassword.aspx.cs" AutoEventWireup="false" Inherits="SwenetDev.ChangePassword" %>
<%@ Register TagPrefix="uc1" TagName="Sidebar" Src="Controls/Sidebar.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="Controls/Header.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>ChangePassword</title>
		<link href="style.css" type="text/css" rel="stylesheet">
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</HEAD>
	<body MS_POSITIONING="FlowLayout">
		<form id="Form1" runat="server">
			<uc1:header id="Header1" runat="server"></uc1:header>
			<table id="Table1" cellSpacing="1" cellPadding="1" width="750">
				<tr>
					<td vAlign="top" width="150"><uc1:sidebar id="Sidebar1" runat="server"></uc1:sidebar></td>
					<td vAlign="top" width="600">
						<h2>Change User Password</h2>
						<asp:label id="ErrorMessage" runat="server" forecolor="Red"></asp:Label>
						<table id="Table1" cellSpacing="1" cellPadding="1" width="595" border="0">
							<tr>
								<td style="HEIGHT: 26px" vAlign="top" align="right" width="120"><strong>Username:</strong></td>
								<td style="HEIGHT: 26px"><asp:textbox id="txtUserName" runat="server" columns="26" maxlength="25"></asp:textbox><asp:requiredfieldvalidator id="RequiredFieldValidator1" runat="server" errormessage="Username required." controltovalidate="txtUserName">*</asp:requiredfieldvalidator>&nbsp;<asp:regularexpressionvalidator id="Regularexpressionvalidator3" runat="server" errormessage="Username must be at least 3 characters."
										controltovalidate="txtUserName" display="Dynamic" validationexpression=".{3,}"></asp:regularexpressionvalidator><asp:regularexpressionvalidator id="AlphaNumericUsername" runat="server" errormessage="Username may only contain letters, numbers, underscores, and hyphens."
										controltovalidate="txtUserName" display="Dynamic" validationexpression="[\w-]*"><br>Username may only contain letters, numbers, underscores, and hyphens.</asp:regularexpressionvalidator></td>
							</tr>
							<tr>
								<td style="HEIGHT: 26px" vAlign="top" align="right" width="120"><strong>New Password:</strong></td>
								<td style="HEIGHT: 26px"><asp:textbox id="txtPassword" runat="server" columns="26" maxlength="25" textmode="Password"></asp:textbox><asp:requiredfieldvalidator id="RequiredFieldValidator2" runat="server" errormessage="Password required." controltovalidate="txtPassword">*</asp:requiredfieldvalidator>&nbsp;<asp:regularexpressionvalidator id="RegExValidator1" runat="server" errormessage="Password must be at least 8 characters."
										controltovalidate="txtPassword" display="Dynamic" validationexpression=".{8,}"></asp:regularexpressionvalidator><asp:regularexpressionvalidator id="RegularExpressionValidator2" runat="server" errormessage="Password may only contain letters, numbers, underscores, and hyphens."
										controltovalidate="txtPassword" display="Dynamic" validationexpression="[\w-]*"><br>Password may only contain letters, numbers, underscores, and hyphens.</asp:regularexpressionvalidator></td>
							</tr>
							<tr>
								<td style="HEIGHT: 26px" vAlign="top" align="right" width="120"><strong>Confirm:</strong></td>
								<td style="HEIGHT: 26px"><asp:textbox id="txtPasswordConfirm" runat="server" columns="26" maxlength="25" textmode="Password"></asp:textbox><asp:requiredfieldvalidator id="RequiredFieldValidator3" runat="server" errormessage="Confirm password required."
										controltovalidate="txtPasswordConfirm">*</asp:requiredfieldvalidator>&nbsp;<asp:customvalidator id="PasswordCompareValidator" runat="server" errormessage="Passwords do not match."></asp:customvalidator></td>
							</tr>
							<tr>
								<td width="120">&nbsp;</td>
								<td><asp:button id="SubmitBtn" runat="server" cssclass="defaultButton" text="Submit" causesvalidation="False"></asp:button></td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
