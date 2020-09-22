<%@ Page Language="c#" CodeBehind="register.aspx.cs" AutoEventWireup="false" Inherits="SwenetDev.register" %>
<%@ Register TagPrefix="uc1" TagName="EditUserInfoControl" Src="Controls/EditUserInfoControl.ascx" %>
<%@ Register TagPrefix="swenet" TagName="sidebar" Src="Controls\Sidebar.ascx" %>
<%@ Register TagPrefix="swenet" TagName="header" Src="Controls\Header.ascx" %>
<%@ OutputCache Location="None" VaryByParam="None" %>
<html>
	<head>
		<title>Register</title>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="style.css" type="text/css" rel="stylesheet">
	</head>
	<body>
		<form runat="server">
			<swenet:header id="Header" runat="server"></swenet:header>
			<table id="Table1" width="750" cellspacing="1" cellpadding="1">
				<tr>
					<td valign="top" width="150">
						<div id="categories"><swenet:sidebar id="Sidebar" runat="server"></swenet:sidebar></div>
					</td>
					<td valign="top" width="600">
						<h1>Register</h1>
						<asp:label id="lblMessage" runat="server"></asp:label>
						<uc1:edituserinfocontrol id="EditUserInfoControl1" runat="server"></uc1:edituserinfocontrol>
						<br>
						<table cellspacing="1" cellpadding="1" border="0">
							<tr>
								<td width="110">&nbsp;</td>
								<td><asp:button id="RegisterBtn" runat="server" cssclass="defaultButton" text="Register" causesvalidation="False"></asp:button></td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
		</form>
	</body>
</html>
