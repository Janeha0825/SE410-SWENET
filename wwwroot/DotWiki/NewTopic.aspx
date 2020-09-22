<%@ Register TagPrefix="cc1" Namespace="DotWikiControls" Assembly="DotWikiControls" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="NewTopic.aspx.vb" Inherits="DotWiki.NewTopic"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>NewTopic</title>
		<LINK media="screen" href="Styles.css" type="text/css" rel="StyleSheet">
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</HEAD>
	<body onLoad="javascript:document.forms['Form1'].txtTopicName.focus()">
		<FORM id="Form1" method="post" runat="server">
			<cc1:DotWikiPageHeaderControl id="DotWikiPageHeaderControl1" runat="server"></cc1:DotWikiPageHeaderControl>
			<br>
			<asp:label id="Label1" runat="server">Name of the topic to add</asp:label>&nbsp;
			<asp:textbox id="txtTopicName" runat="server" CssClass="newTopicTextbox"></asp:textbox>
			<asp:button id="cmdAddIt" runat="server" Text="Add It" CssClass="mainButton"></asp:button>
			<br>
			<asp:label id="lblMessage" runat="server">...</asp:label>
		</FORM>
	</body>
</HTML>
