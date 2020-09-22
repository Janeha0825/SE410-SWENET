<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Search.aspx.vb" Inherits="DotWiki.SearchPage"%>
<%@ Register TagPrefix="cc1" Namespace="DotWikiControls" Assembly="DotWikiControls" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Search</title>
		<LINK media="screen" href="Styles.css" type="text/css" rel="StyleSheet">
		<meta name="GENERATOR" content="Microsoft Visual Studio.NET 7.0">
		<meta name="CODE_LANGUAGE" content="Visual Basic 7.0">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	<body onLoad="javascript:document.forms['Form1'].txtTextToSearch.focus()">
		<form id="Form1" method="post" runat="server">
				<cc1:DotWikiPageHeaderControl id="DotWikiPageHeaderControl1" runat="server"></cc1:DotWikiPageHeaderControl>
				<br>
				<asp:Label id="Label1" runat="server">Text to look for</asp:Label>&nbsp;
				<asp:TextBox id="txtTextToSearch" runat="server" Width="300px" CssClass="searchTextbox"></asp:TextBox>
				<asp:Button id="cmdSearch" runat="server" Text="Go" CssClass="mainButton"></asp:Button>
				<br>
				<asp:Label id="lblPageContent" runat="server">...</asp:Label>
		</form>
	</body>
</HTML>
