<%@ Register TagPrefix="uc1" TagName="sidebar" Src="Controls\Sidebar.ascx" %>
<%@ Register TagPrefix="uc1" TagName="header" Src="Controls\Header.ascx" %>
<%@ Page language="c#" Codebehind="browseModules.aspx.cs" AutoEventWireup="false" Inherits="SwenetDev.browseModules" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Browse Modules</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="style.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<uc1:header id="Header1" runat="server"></uc1:header>
			<table id="Table1" cellSpacing="1" cellPadding="1" width="750">
				<tr>
					<td vAlign="top" width="150">
						<h1><uc1:sidebar id="Sidebar1" runat="server"></uc1:sidebar></h1>
					</td>
					<td vAlign="top" width="600">
						<h1>Browse Modules</h1>
						<asp:label id="InfoMessage" runat="server"></asp:label>
						<p><asp:datagrid id="ModuleGrid" runat="server" cellpadding="3" cssclass="borderedTable" allowpaging="False"
								allowsorting="True" autogeneratecolumns="False" width="596px">
								<columns>
									<asp:templatecolumn>
										<HeaderTemplate>
											<b>Module Name</b>
											<asp:HyperLink ID="ModArrowLink" Runat="server" ImageUrl="DotWiki/images/dnarrow.jpg" OnLoad="ModArrowLink_Load"></asp:HyperLink>
										</HeaderTemplate>
										<headerstyle cssclass="lightGrayHeader" width="30%"></headerstyle>
										<itemtemplate>
											<asp:hyperlink id="HyperLink1" runat="server" navigateurl='<%# "viewModule.aspx?moduleID=" + DataBinder.Eval( Container.DataItem, "Id" ) %>'>
												<%# DataBinder.Eval( Container.DataItem, "Title" ) %>
											</asp:hyperlink>
										</itemtemplate>
									</asp:templatecolumn>
									<asp:TemplateColumn>
										<HeaderTemplate>
											<b>Submitted By</b>
											<asp:HyperLink ID="SubArrowLink" Runat="server" ImageUrl="DotWiki/images/dnarrow.jpg" OnLoad="SubArrowLink_Load"></asp:HyperLink>
										</HeaderTemplate>
										<HeaderStyle CssClass="lightGrayHeader" Width="12%"></HeaderStyle>
										<ItemTemplate>
											<%# DataBinder.Eval( Container.DataItem, "Submitter" ) %>
										</ItemTemplate>
									</asp:TemplateColumn>
									<asp:TemplateColumn>
										<HeaderTemplate>
											<b>Date Submitted</b>
											<asp:HyperLink ID="DateArrowLink" Runat="server" ImageUrl="DotWiki/images/dnarrow.jpg" OnLoad="DateArrowLink_Load"></asp:HyperLink>
										</HeaderTemplate>
										<HeaderStyle CssClass="lightGrayHeader" Width="18%"></HeaderStyle>
										<itemtemplate>
											<%# DataBinder.Eval( Container.DataItem, "Date" ) %>
										</itemtemplate>
									</asp:TemplateColumn>
								</columns>
							</asp:datagrid></p>
						<p><asp:label id="DiscussLbl" runat="server"></asp:label></p>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
