<%@ Page language="c#" Codebehind="ViewComments.aspx.cs" AutoEventWireup="false" Inherits="SwenetDev.ViewComments" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>Module Versions and Revision Comments</title>
		<link href="style.css" type="text/css" rel="stylesheet">
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script language="javascript">
			<!--
				function openWin(url) {
					window.opener.location = url;
				}
			-->
		</script>
	</head>
	<body ms_positioning="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<h1>Module Versions and Revision Comments</h1>
			<h2>
				<asp:label id="TitleLabel" runat="server"></asp:label></h2>
			<asp:datagrid id="CommentsGrid" runat="server" autogeneratecolumns="False" borderwidth="1px" cellpadding="3"
				cssclass="borderedTable" width="100%">
				<columns>
					<asp:templatecolumn headertext="Version">
						<headerstyle cssclass="lightGrayHeader" width="55px"></headerstyle>
						<itemtemplate>
							<% if ( Page.User.IsInRole( SwenetDev.DBAdapter.UserRole.Editor.ToString() ) ) { %>
							<asp:hyperlink runat="server" id="VersionLink" navigateurl='<%# DataBinder.Eval( Container.DataItem, "Id", "javascript:openWin(\"viewModule.aspx?moduleID={0}\")" ) %>'>
								<%# DataBinder.Eval( Container.DataItem, "Version" ) %>
							</asp:hyperlink>
							<% } else { %>
								<%# DataBinder.Eval( Container.DataItem, "Version" ) %>
							<% } %>
						</itemtemplate>
					</asp:templatecolumn>
					<asp:boundcolumn datafield="CheckInComments" headertext="Comments">
						<headerstyle cssclass="lightGrayHeader"></headerstyle>
					</asp:boundcolumn>
				</columns>
			</asp:datagrid></form>
	</body>
</html>
