<%@ Page language="c#" Codebehind="viewUserInfo.aspx.cs" AutoEventWireup="false" Inherits="SwenetDev.viewUserInfo" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="Controls/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Sidebar" Src="Controls/Sidebar.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ViewUserInfoControl" Src="Controls/ViewUserInfoControl.ascx" %>
<%@ OutputCache Location="None" VaryByParam="None" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>

		<title>User Information</title>
		<link href="style.css" type="text/css" rel="stylesheet">
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</head>
	<body ms_positioning="FlowLayout">
		<form id="Form1" runat="server">
			<uc1:header id="Header1" runat="server"></uc1:header>
			<table id="Table1" width="750">
				<tr>
					<td valign="top" width="150">
						<h1 id="categories"><uc1:sidebar id="Sidebar1" runat="server"></uc1:sidebar></h1>
					</td>
					<td valign="top" width="600">
						<h1>User Information</h1>
						<p>
							<asp:label id="ErrorMessage" runat="server"></asp:label>
							<uc1:viewuserinfocontrol id="ViewUserInfoControl1" runat="server"></uc1:viewuserinfocontrol></p>
					</td>
				</tr>
			</table>
		</form>
	</body>
</html>
