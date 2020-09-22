<%@ Register TagPrefix="uc1" TagName="sidebar" Src="Controls\Sidebar.ascx" %>
<%@ Page language="c#" Codebehind="uploadResult.aspx.cs" AutoEventWireup="false" Inherits="SwenetDev.uploadResult" %>
<%@ Register TagPrefix="uc1" TagName="header" Src="Controls\Header.ascx" %>
<%@ OutputCache Location="None" VaryByParam="None" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>Upload Result</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="style.css" type="text/css" rel="stylesheet">
	</head>
	<body>
		<form id="Form1" method="post" runat="server">
			<uc1:header id="Header1" runat="server"></uc1:header>
			<table>
				<tr>
					<td width="150">
						<uc1:sidebar id="Sidebar1" runat="server"></uc1:sidebar></td>
					<td width="600" valign="top">
						<asp:hyperlink id="ViewNewModule" runat="server" navigateurl="viewModule.aspx">Module</asp:hyperlink>
						uploaded successfully.</td>
				</tr>
			</table>
		</form>
	</body>
</html>
