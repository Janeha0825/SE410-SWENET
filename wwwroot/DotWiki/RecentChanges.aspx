<%@ Register TagPrefix="cc1" Namespace="DotWikiControls" Assembly="DotWikiControls" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="RecentChanges.aspx.vb" Inherits="DotWiki.RecentChanges"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>RecentChanges</title>
		<LINK media="screen" href="Styles.css" type="text/css" rel="StyleSheet">
		<meta name="GENERATOR" content="Microsoft Visual Studio.NET 7.0">
		<meta name="CODE_LANGUAGE" content="Visual Basic 7.0">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<cc1:DotWikiPageHeaderControl id="DotWikiPageHeaderControl1" runat="server"></cc1:DotWikiPageHeaderControl>
			<asp:Label id="Label1" runat="server" Visible="False">A grid is added at runtime to display the list of topics.</asp:Label><br>
			<asp:Button id="cmdLast24Hrs" runat="server" Text="Last 24 Hours" CssClass="mainButton"></asp:Button>
			<asp:Button id="cmdLast7Days" runat="server" Text="Last 7 days" CssClass="mainButton"></asp:Button>
			<asp:Button id="cmdLastMonth" runat="server" Text="Last Month" CssClass="mainButton"></asp:Button>
			<asp:ImageButton id="cmdXml" runat="server" ImageUrl="xml.gif"></asp:ImageButton>
			<asp:Label id="lblMessage" runat="server">...</asp:Label>&nbsp;
		</form>
	</body>
</HTML>
