<%@ OutputCache Location="None" VaryByParam="None" %>
<%@ Import Namespace="SwenetDev.DBAdapter" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="Controls/Header.ascx" %>
<%@ Page language="c#" Codebehind="ModuleManagement.aspx.cs" AutoEventWireup="false" Inherits="SwenetDev.ModuleManagement" %>
<%@ Register TagPrefix="uc1" TagName="Sidebar" Src="Controls/Sidebar.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Admin Page - Module Management</title>
		<LINK href="style.css" type="text/css" rel="stylesheet">
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</HEAD>
	<body ms_positioning="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<uc1:header id="Header1" runat="server"></uc1:header>
			<table id="Table1" cellSpacing="1" cellPadding="1" width="750">
				<tr>
					<td vAlign="top" width="150"><uc1:sidebar id="Sidebar1" runat="server"></uc1:sidebar></td>
					<td vAlign="top" width="600">
						<h1>Admin Page</h1>
						<asp:label id="PageMessageLbl" runat="server"></asp:label>
						<h2>Module Management</h2>
						<p><asp:datagrid id="ModuleGrid" runat="server" width="596px" autogeneratecolumns="False" allowsorting="True"
								allowpaging="False" cssclass="borderedTable" cellpadding="3">
								<columns>
									<asp:templatecolumn headertext="Module Name">
										<headerstyle cssclass="lightGrayHeader" width="30%"></headerstyle>
										<itemtemplate>
											<asp:HyperLink id="ModuleLink" runat="server" target="_blank" Text='<%# DataBinder.Eval(Container, "DataItem.Title") %>' NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.Id", "viewModule.aspx?moduleID={0}") %>'>
											</asp:HyperLink>
										</itemtemplate>
									</asp:templatecolumn>
									<asp:templatecolumn headertext="Status">
										<headerstyle cssclass="lightGrayHeader" width="10%"></headerstyle>
										<itemtemplate>
											<%# DataBinder.Eval(Container, "DataItem.Status") %>
											</asp:HyperLink>
										</itemtemplate>
									</asp:templatecolumn>
									<asp:templatecolumn headertext="Edit">
										<headerstyle width="5%" cssclass="lightGrayHeader"></headerstyle>
										<itemtemplate>
											<asp:linkbutton id="EditLink" runat="server" Text="Edit" commandname="Edit" commandargument='<%# DataBinder.Eval(Container, "DataItem.Id" ) %>'>
											</asp:linkbutton>
										</itemtemplate>
									</asp:templatecolumn>
									<asp:templatecolumn headertext="Delete">
										<headerstyle width="5%" cssclass="lightGrayHeader"></headerstyle>
										<itemtemplate>
											<asp:linkbutton id="DeleteLink" runat="server" text="Delete" commandname="Delete" commandargument='<%# DataBinder.Eval( Container.DataItem, "Id" ) %>'>
											</asp:linkbutton>
										</itemtemplate>
									</asp:templatecolumn>
								</columns>
								<pagerstyle font-bold="True" horizontalalign="Center" backcolor="#DEDEDE"></pagerstyle>
							</asp:datagrid></p>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
