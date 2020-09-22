<%@ Page language="c#" Codebehind="error.aspx.cs" AutoEventWireup="false" Inherits="SwenetDev.error" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="Controls/Header.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>SWEnet Error</title>
		<link href="style.css" type="text/css" rel="stylesheet">
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</head>
	<body ms_positioning="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<uc1:header id="Header1" runat="server"></uc1:header>
			<table id="Table1" cellspacing="1" cellpadding="1" width="750" border="0">
				<tr>
					<td>
						<asp:label id="lblMessage" runat="server"></asp:label></td>
				</tr>
			</table>
		</form>
	</body>
</html>
