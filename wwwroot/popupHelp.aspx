<%@ Page language="c#" Codebehind="popupHelp.aspx.cs" AutoEventWireup="false" Inherits="SwenetDev.popupHelp" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>SWEnet Help</title>
		<SCRIPT language="javascript">
<!--
function closePopup() {
	window.close();
}

-->
		</SCRIPT>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="style.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<P align="center"><STRONG><FONT size="4">
						<asp:Label id="titleLabel" runat="server">Title</asp:Label></P>
			</FONT></STRONG>
			<P>
				<asp:Label id="textLabel" runat="server">Help text</asp:Label></P>
			<P align="center">[ <A href="javascript:closePopup()">close</A> ]</P>
		</form>
	</body>
</HTML>
