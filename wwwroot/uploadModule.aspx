<%@ Page language="c#" Codebehind="uploadModule.aspx.cs" AutoEventWireup="false" Inherits="SwenetDev.uploadModule" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="Controls/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Sidebar" Src="Controls/Sidebar.ascx" %>
<%@ OutputCache Location="None" VaryByParam="None" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>Upload Module</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="style.css" type="text/css" rel="stylesheet">
	</head>
	<body ms_positioning="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<uc1:header id="Header1" runat="server"></uc1:header>
			<table width="750">
				<tr>
					<td valign="top" width="150">
						<h1><uc1:sidebar id="Sidebar1" runat="server"></uc1:sidebar></h1>
					</td>
					<td valign="top" width="600">
						<h1>Incomplete Module</h1>
						<p>You have an&nbsp;incomplete module<asp:label id="ModuleLbl" runat="server"></asp:label>that 
							you previously saved.&nbsp; You must choose to continue editing it or 
							delete it before submitting a new one.</p>
						<p>What would you like to do?</p>
						<p><asp:radiobuttonlist id="RadioButtonList1" runat="server">
								<asp:listitem selected="True" value="Edit"><strong>Edit</strong> saved module.</asp:listitem>
								<asp:listitem value="Delete"><strong>Delete</strong> saved module and start new module.</asp:listitem>
							</asp:radiobuttonlist></p>
						<p><asp:button id="ContinueBtn" runat="server" text="Continue"></asp:button></p>
					</td>
				</tr>
			</table>
		</form>
	</body>
</html>
