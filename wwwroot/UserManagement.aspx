<%@ OutputCache Location="None" VaryByParam="None" %>
<%@ Import Namespace="SwenetDev.DBAdapter" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="Controls/Header.ascx" %>
<%@ Page language="c#" Codebehind="UserManagement.aspx.cs" AutoEventWireup="false" Inherits="SwenetDev.UserManagement" %>
<%@ Register TagPrefix="uc1" TagName="Sidebar" Src="Controls/Sidebar.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Admin Page - User Management</title>
		<link href="style.css" type="text/css" rel="stylesheet">
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</HEAD>
	<body ms_positioning="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<uc1:header id="Header1" runat="server"></uc1:header>
			<table id="Table1" cellspacing="1" cellpadding="1" width="750">
				<tr>
					<td valign="top" width="150"><uc1:sidebar id="Sidebar1" runat="server"></uc1:sidebar></td>
					<td valign="top" width="600">
						<h1>Admin Page</h1>
						<asp:label id="PageMessageLbl" runat="server"></asp:label>
						<h2>User Management</h2>
						<p>
							<asp:datagrid id="UsersGrid" runat="server" width="596px" autogeneratecolumns="False" allowpaging="True"
								cssclass="borderedTable" cellpadding="3">
								<columns>
									<asp:templatecolumn headertext="Username">
										<headerstyle cssclass="lightGrayHeader" width="30%"></headerstyle>
										<itemtemplate>
											<asp:HyperLink id="UserLink" runat="server" target="_blank" Text='<%# DataBinder.Eval(Container, "DataItem.Username") %>' NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.Username", "viewUserInfo.aspx?username={0}") %>'>
											</asp:HyperLink>
										</itemtemplate>
									</asp:templatecolumn>
									<asp:templatecolumn headertext="Name">
										<headerstyle cssclass="lightGrayHeader" width="40%"></headerstyle>
										<itemtemplate>
											<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Name") %>' ID="Label1" NAME="Label1">
											</asp:Label>
										</itemtemplate>
									</asp:templatecolumn>
									<asp:templatecolumn headertext="User Role">
										<headerstyle cssclass="lightGrayHeader" width="30%"></headerstyle>
										<itemtemplate>
											<asp:dropdownlist id="RolesDrop" runat="server" onselectedindexchanged="SelectionChanged" autopostback="True" selectedindex='<%# (int)((UserRole)DataBinder.Eval( Container.DataItem, "Role" )) %>'>
											</asp:dropdownlist>
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
