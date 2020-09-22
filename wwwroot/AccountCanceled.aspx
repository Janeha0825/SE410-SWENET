<%@ Page language="c#" Codebehind="AccountCanceled.aspx.cs" AutoEventWireup="false" Inherits="SwenetDev.AccountCanceled" %>
<%@ Register TagPrefix="uc1" TagName="sidebar" Src="Controls\Sidebar.ascx" %>
<%@ Register TagPrefix="uc1" TagName="header" Src="Controls\Header.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Account Canceled</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="style.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body>
		<form id="Form2" method="post" runat="server">
			<uc1:header id="Header1" runat="server"></uc1:header>
			<table id="Table1" width="750" cellspacing="1" cellpadding="1">
				<tr>
					<td width="150" valign="top">
						<h1><uc1:sidebar id="Sidebar1" runat="server"></uc1:sidebar></h1>
					</td>
					<td width="600" valign="top">
						<P>
							<asp:Label id="ACLabel" runat="server"></asp:Label></P>
						<P>
							<asp:Button id="ContinueButton" runat="server" Text="Continue..." CssClass="defaultButton"></asp:Button>&nbsp;
							<asp:Button id="YesButton" runat="server" Width="56px" Text="Yes" CssClass="defaultButton"></asp:Button>&nbsp;
							<asp:Button id="NoButton" runat="server" Text="No" CssClass="defaultButton" Width="56px"></asp:Button></P>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
