<%@ Page Language="vb" AutoEventWireup="false" Codebehind="TopicHistory.aspx.vb" Inherits="DotWiki.TopicHistory"%>
<%@ Register TagPrefix="cc1" Namespace="DotWikiControls" Assembly="DotWikiControls" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>TopicHistory</title>
		<LINK media="screen" href="Styles.css" type="text/css" rel="StyleSheet">
		<meta name="GENERATOR" content="Microsoft Visual Studio.NET 7.0">
		<meta name="CODE_LANGUAGE" content="Visual Basic 7.0">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<cc1:DotWikiPageHeaderControl id="DotWikiPageHeaderControl1" runat="server"></cc1:DotWikiPageHeaderControl>
			<asp:label id="Label1" runat="server">History for Topic  </asp:label>&nbsp;
			<asp:label id="lblPageTopic" runat="server" Font-Size="Large">DotWiki</asp:label>&nbsp;
			<asp:label id="lblDateTime" runat="server">(history as of...)</asp:label>&nbsp;<br>
			<asp:label id="lblPageContent" runat="server">PageContent</asp:label></P>
			<HR width="100%" SIZE="1">
			<P>
				<asp:Label id="lblRestorePassword" runat="server" DESIGNTIMEDRAGDROP="80">Restore Password</asp:Label>&nbsp;
				<asp:TextBox id="txtRestorePassword" runat="server" TextMode="Password"></asp:TextBox>
				<asp:Button id="cmdRestore" runat="server" Text="Restore" ToolTip="Restore this version of the topic as the current version"
					CssClass="mainButton"></asp:Button>
			</P>
			<P>&nbsp;&nbsp;</P>
			<P>&nbsp;</P>
		</form>
	</body>
</HTML>
