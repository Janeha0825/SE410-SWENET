<%@ Register TagPrefix="uc1" TagName="TopicList" Src="TopicList.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Index.aspx.vb" Inherits="DotWiki.Index"%>
<%@ Register TagPrefix="cc1" Namespace="DotWikiControls" Assembly="DotWikiControls" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title></title>
		<LINK media="screen" href="Styles.css" type="text/css" rel="StyleSheet">
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<cc1:DotWikiPageHeaderControl id="DotWikiPageHeaderControl1" runat="server"></cc1:DotWikiPageHeaderControl>
			<asp:Label id="Label1" runat="server" Visible="False">A grid is added at runtime to display the list of topics.</asp:Label>
		</form>
	</body>
</HTML>
