<%@ Page language="c#" Codebehind="searchModules.aspx.cs" AutoEventWireup="false" Inherits="SwenetDev.searchModules" %>
<%@ Register TagPrefix="uc1" TagName="sidebar" Src="Controls\Sidebar.ascx" %>
<%@ Register TagPrefix="uc1" TagName="header" Src="Controls\Header.ascx" %>
<%@ import Namespace="SwenetDev.DBAdapter" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Modules Search</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="style.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body>
		<form id="Form2" method="post" runat="server">
			<uc1:header id="Header1" runat="server"></uc1:header>
			<table id="Table1" width="750" cellspacing="1" cellpadding="1">
				<tr>
					<td width="150" valign="top">
						<h1><uc1:sidebar id="Sidebar1" runat="server"></uc1:sidebar></h1>
					</td>
					<td width="600" valign="top">
						<P>
							<asp:Label id="lblError" runat="server" ForeColor="Red"></asp:Label></P>
						<P>
							Search for:
							<asp:TextBox id="txtSearch" runat="server"></asp:TextBox>&nbsp;from field:
							<asp:DropDownList id="ddlFields" runat="server" Width="152px"></asp:DropDownList>&nbsp;
							<asp:Button id="SearchBtn" runat="server" Text="Search"></asp:Button></P>
						<P>(note: all searches are performed with substrings.&nbsp; ie - "abc" and 
							"abc.2004.1"<br>
							&nbsp;will both return "abc.2004.1")</P>
						<P>
							<asp:Label id="lblResults" runat="server">Results:</asp:Label>
							<asp:Label id="lblNoResults" runat="server">No matching Modules were found.  Please revise your search and try again.</asp:Label></P>
						<P>
							<asp:Repeater id="ResultsRepeater" runat="server">
								<itemtemplate>
									<asp:HyperLink id="HyperLink1" runat="server" NavigateUrl='<%# "viewModule.aspx?moduleID=" + Container.DataItem %>'>
										<%# Modules.getModuleInfo(((int)Container.DataItem)).Title %>
									</asp:HyperLink>
								</itemtemplate>
								<separatortemplate>
									<br />
								</separatortemplate>
							</asp:Repeater>
						</P>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
