<%@ Register TagPrefix="uc1" TagName="Sidebar" Src="Controls/Sidebar.ascx" %>
<%@ Page language="c#" Codebehind="AdminPage.aspx.cs" AutoEventWireup="false" Inherits="SwenetDev.AdminPage" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="Controls/Header.ascx" %>
<%@ Import Namespace="SwenetDev.DBAdapter" %>
<%@ OutputCache Location="None" VaryByParam="None" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Admin Page</title>
		<link href="style.css" type="text/css" rel="stylesheet">
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</HEAD>
	<body ms_positioning="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<uc1:header id="Header1" runat="server"></uc1:header>
			<table id="Table1" cellspacing="1" cellpadding="1" width="750">
				<tr>
					<td valign="top" width="150"><uc1:sidebar id="Sidebar1" runat="server"></uc1:sidebar></td>
					<td valign="top" width="600">
						<h1>Admin Page</h1>
						<p>
						<a href="UserManagement.aspx">User Management</a><br>
						<a href="ChangePassword.aspx">Change User Password</a><br>
						<a href="ModuleManagement.aspx">Module Management</a><br>
						</p>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
