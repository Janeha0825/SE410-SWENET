<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ViewImage.aspx.vb" Inherits="DotWiki.ViewImage"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>View Picture</title>
		<LINK REL="StyleSheet" HREF="Styles.css" TYPE="text/css" MEDIA="screen">
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<P>
				<asp:Button id="cmdOriginal" runat="server" Text="Original Size" CssClass="mainButton"></asp:Button>
				<asp:Button id="cmdFitHorizontal" runat="server" Text="Fit Horizontal" CssClass="mainButton"></asp:Button>
				<asp:Button id="cmdFitVertical" runat="server" Text="Fit Vertical" CssClass="mainButton"></asp:Button></P>
			<P>
				<asp:Label id="lblContent" runat="server">...</asp:Label></P>
			<P>
				<asp:Label id="lblPictureFile" runat="server">PictureFile</asp:Label>
				<asp:Label id="lblPictureZoom" runat="server">100 </asp:Label></P>
		</form>
	</body>
</HTML>
