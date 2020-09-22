<%@ OutputCache Location="None" VaryByParam="None" %>
<%@ Register TagPrefix="uc1" TagName="Sidebar" Src="Controls/Sidebar.ascx" %>
<%@ Page language="c#" Codebehind="EditorsPage.aspx.cs" AutoEventWireup="false" Inherits="SwenetDev.EditorsPage" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="Controls/Header.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Editor's Page</title>
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
						<h1>Editor's Page</h1>
						<asp:label id="PageMessageLbl" runat="server"></asp:label>
						<h2>Faculty Requests</h2>
						<p><asp:datagrid id="FacultyGrid" runat="server" cellpadding="3" cssclass="borderedTable" autogeneratecolumns="False"
								width="596px">
								<columns>
									<asp:templatecolumn headertext="Date Requested">
										<headerstyle width="15%" cssclass="lightGrayHeader"></headerstyle>
										<itemtemplate>
											<asp:Label id="Label4" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Date", "{0:d}") %>'>
											</asp:Label>
										</itemtemplate>
									</asp:templatecolumn>
									<asp:templatecolumn headertext="Username">
										<headerstyle width="20%" cssclass="lightGrayHeader"></headerstyle>
										<itemtemplate>
											<asp:HyperLink id="Hyperlink6" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.UserName") %>' NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.UserName", "viewUserInfo.aspx?username={0}") %>'>
											</asp:HyperLink>
										</itemtemplate>
									</asp:templatecolumn>
									<asp:templatecolumn headertext="Evidence">
										<headerstyle cssclass="lightGrayHeader"></headerstyle>
										<itemstyle verticalalign="Top"></itemstyle>
										<itemtemplate>
											<asp:linkbutton id="ShowHideBtn0" runat="server" text="Hide Evidence" commandname="ShowHideText"
												causesvalidation="false"></asp:linkbutton>
											<asp:label id="ProofLbl" text='<%# "<br>" + DataBinder.Eval( Container.DataItem, "Proof" ) %>' runat="server" visible="True">
											</asp:label>
										</itemtemplate>
									</asp:templatecolumn>
									<asp:templatecolumn headertext="Approve">
										<headerstyle width="10%" cssclass="lightGrayHeader"></headerstyle>
										<itemtemplate>
											<asp:HyperLink id="Hyperlink7" runat="server" Text="Approve" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.UserName", "editorActionEmail.aspx?type=0&username={0}&approved=true") %>'>Approve</asp:HyperLink>
										</itemtemplate>
									</asp:templatecolumn>
									<asp:templatecolumn headertext="Reject">
										<headerstyle width="7%" cssclass="lightGrayHeader"></headerstyle>
										<itemtemplate>
											<asp:HyperLink id="Hyperlink8" runat="server" Text="Reject" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.UserName", "editorActionEmail.aspx?type=0&username={0}&approved=false") %>'>Reject</asp:HyperLink>
										</itemtemplate>
									</asp:templatecolumn>
								</columns>
							</asp:datagrid></p>
						<h2>Submitter Requests</h2>
						<p><asp:datagrid id="SubmitterGrid" runat="server" cellpadding="3" cssclass="borderedTable" autogeneratecolumns="False"
								width="596px">
								<columns>
									<asp:templatecolumn headertext="Date Requested">
										<headerstyle width="15%" cssclass="lightGrayHeader"></headerstyle>
										<itemtemplate>
											<asp:Label id="Label1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Date", "{0:d}") %>'>
											</asp:Label>
										</itemtemplate>
									</asp:templatecolumn>
									<asp:templatecolumn headertext="Username">
										<headerstyle width="20%" cssclass="lightGrayHeader"></headerstyle>
										<itemtemplate>
											<asp:HyperLink id="HyperLink1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.UserName") %>' NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.UserName", "viewUserInfo.aspx?username={0}") %>'>
											</asp:HyperLink>
										</itemtemplate>
									</asp:templatecolumn>
									<asp:templatecolumn headertext="User Message">
										<headerstyle cssclass="lightGrayHeader"></headerstyle>
										<itemstyle verticalalign="Top"></itemstyle>
										<itemtemplate>
											<asp:linkbutton id="ShowHideBtn" runat="server" text="Hide Message" commandname="ShowHideText" causesvalidation="false"></asp:linkbutton>
											<asp:label id="MessageLbl" text='<%# "<br>" + DataBinder.Eval( Container.DataItem, "Message" ) %>' runat="server" visible="True">
											</asp:label>
										</itemtemplate>
									</asp:templatecolumn>
									<asp:templatecolumn headertext="Approve">
										<headerstyle width="10%" cssclass="lightGrayHeader"></headerstyle>
										<itemtemplate>
											<asp:HyperLink id="HyperLink2" runat="server" Text="Approve" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.UserName", "editorActionEmail.aspx?type=1&username={0}&approved=true") %>'>Approve</asp:HyperLink>
										</itemtemplate>
									</asp:templatecolumn>
									<asp:templatecolumn headertext="Reject">
										<headerstyle width="7%" cssclass="lightGrayHeader"></headerstyle>
										<itemtemplate>
											<asp:HyperLink id="HyperLink3" runat="server" Text="Reject" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.UserName", "editorActionEmail.aspx?type=1&username={0}&approved=false") %>'>Reject</asp:HyperLink>
										</itemtemplate>
									</asp:templatecolumn>
								</columns>
							</asp:datagrid></p>
						<h2>Modules Awaiting Approval</h2>
						<p><asp:datagrid id="ModulesGrid" runat="server" cellpadding="3" cssclass="borderedTable" autogeneratecolumns="False"
								width="596px">
								<columns>
									<asp:templatecolumn headertext="Module Date">
										<headerstyle width="15%" cssclass="lightGrayHeader"></headerstyle>
										<itemtemplate>
											<asp:Label id="Label2" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Date", "{0:d}") %>'>
											</asp:Label>
										</itemtemplate>
									</asp:templatecolumn>
									<asp:templatecolumn headertext="Submitter">
										<headerstyle width="20%" cssclass="lightGrayHeader"></headerstyle>
										<itemtemplate>
											<asp:Label id="Label3" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.UserName") %>'>
											</asp:Label>
										</itemtemplate>
									</asp:templatecolumn>
									<asp:hyperlinkcolumn target="_blank" datanavigateurlfield="Id" datanavigateurlformatstring="viewModule.aspx?moduleID={0}"
										datatextfield="Title" headertext="Module Title">
										<headerstyle cssclass="lightGrayHeader"></headerstyle>
									</asp:hyperlinkcolumn>
									<asp:templatecolumn headertext="Approve">
										<headerstyle width="10%" cssclass="lightGrayHeader"></headerstyle>
										<itemtemplate>
											<asp:HyperLink id="HyperLink4" runat="server" Text="Approve" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.ApproveUrl") %>'>Approve</asp:HyperLink>
										</itemtemplate>
									</asp:templatecolumn>
									<asp:templatecolumn headertext="Reject">
										<headerstyle width="7%" cssclass="lightGrayHeader"></headerstyle>
										<itemtemplate>
											<asp:HyperLink id="HyperLink5" runat="server" Text="Reject" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.RejectUrl") %>'>Reject</asp:HyperLink>
										</itemtemplate>
									</asp:templatecolumn>
								</columns>
							</asp:datagrid></p>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
