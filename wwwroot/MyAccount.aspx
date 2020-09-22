<%@ Register TagPrefix="uc1" TagName="FacultyConfirmation" Src="Controls/FacultyConfirmation.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SubmitterRequestControl" Src="Controls/SubmitterRequestControl.ascx" %>
<%@ Page language="c#" Codebehind="MyAccount.aspx.cs" AutoEventWireup="false" Inherits="SwenetDev.MyAccount" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="Controls/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Sidebar" Src="Controls/Sidebar.ascx" %>
<%@ OutputCache Location="None" VaryByParam="None" %>
<!doctype html public "-//w3c//dtd html 4.0 transitional//en" >
<HTML>
	<HEAD>
		<title>My Account</title>
		<link href="style.css" type="text/css" rel="stylesheet">
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</HEAD>
	<body ms_positioning="FlowLayout">
		<form id="Form1" runat="server">
			<uc1:header id="Header1" runat="server"></uc1:header>
			<table id="Table1" width="750" cellpadding="1" cellspacing="1">
				<tr>
					<td valign="top" width="150">
						<div id="categories"><uc1:sidebar id="Sidebar1" runat="server"></uc1:sidebar></div>
					</td>
					<td valign="top" width="600">
						<h1>My Account</h1>
						<P>
							<asp:label id="ErrorMessage" runat="server"></asp:label></P>
						<P>
							<asp:LinkButton id="CancelLink" runat="server">Cancel Membership</asp:LinkButton></P>
						<h2>User Information</h2>
						<p><a href="editUserInfo.aspx">Edit</a><br>
							<A href="viewUserInfo.aspx?username=<%= Page.User.Identity.Name %>" >View</A><br>
							<uc1:FacultyConfirmation id="FacultyConfirmationControl" runat="server"></uc1:FacultyConfirmation>
							<uc1:submitterrequestcontrol id="SubmitterRequestControl1" runat="server"></uc1:submitterrequestcontrol></p>
						<asp:panel id="MyModulesPanel" runat="server">
							<H2>My Modules</H2>
							<asp:datagrid id="ModulesGrid" runat="server" width="596px" cellpadding="3" cssclass="borderedTable"
								autogeneratecolumns="False">
								<columns>
									<asp:templatecolumn headertext="Module">
										<headerstyle cssclass="lightGrayHeader"></headerstyle>
										<itemtemplate>
											<asp:HyperLink runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Title") %>' NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.Id", "viewModule.aspx?moduleID={0}") %>'>
											</asp:HyperLink><br>
											<span class="smallText"><strong>Current Version:</strong>
												<%# DataBinder.Eval( Container.DataItem, "Version" ) %>
												<br>
												<strong>Revision Date:</strong>
												<%# DataBinder.Eval( Container.DataItem, "Date" ) %>
												<br>
												<strong>Locked By:</strong>
												<%# DataBinder.Eval( Container.DataItem, "LockedBy" ) %>
											</span>
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
										<headerstyle width="9%" cssclass="lightGrayHeader"></headerstyle>
										<itemtemplate>
											<asp:linkbutton id="DeleteLink" runat="server" text="Delete" commandname="Delete" commandargument='<%# DataBinder.Eval( Container.DataItem, "Id" ) %>'>
											</asp:linkbutton>
										</itemtemplate>
									</asp:templatecolumn>
									<asp:templatecolumn headertext="Undo<br>Checkout">
										<headerstyle width="10%" cssclass="lightGrayHeader"></headerstyle>
										<itemtemplate>
											<asp:linkbutton id="UndoLink" runat="server" text="Undo" commandname="Undo" commandargument='<%# DataBinder.Eval(Container, "DataItem.Id" ) %>'>
											</asp:linkbutton>
										</itemtemplate>
									</asp:templatecolumn>
									<asp:boundcolumn datafield="Status" headertext="Status">
										<headerstyle width="20%" cssclass="lightGrayHeader"></headerstyle>
									</asp:boundcolumn>
									<asp:boundcolumn datafield="Author" headertext="A">
										<headerstyle width="2%" cssclass="lightGrayHeader"></headerstyle>
									</asp:boundcolumn>
									<asp:boundcolumn datafield="Submitter" headertext="S">
										<headerstyle width="2%" cssclass="lightGrayHeader"></headerstyle>
									</asp:boundcolumn>
								</columns>
							</asp:datagrid>
							<P><STRONG>
									<TABLE class="borderedTable" id="Table2" cellSpacing="0" cellPadding="2" width="596" border="0">
										<TR>
											<TD vAlign="top" align="right"><STRONG class="smallText">Module:</STRONG></TD>
											<TD class="smallText" vAlign="top">Module information.</TD>
										</TR>
										<TR>
											<TD vAlign="top" align="right"><STRONG class="smallText">Edit:</STRONG></TD>
											<TD class="smallText" vAlign="top">Click to edit a version of a module.&nbsp; If 
												the module has an "Approved" status, the module will be checked out and locked 
												by you.&nbsp; The module will be locked until you delete the version you are 
												working on or submit a version and it's approved.</TD>
										</TR>
										<TR>
											<TD vAlign="top" align="right"><STRONG class="smallText">Delete:</STRONG></TD>
											<TD class="smallText">Delete a version of a module that you are working on.</TD>
										</TR>
										<TR>
											<TD vAlign="top" align="right"><STRONG class="smallText">Undo Checkout:</STRONG></TD>
											<TD class="smallText">Unlock the module and delete the version you were working on.</TD>
										</TR>
										<TR>
											<TD vAlign="top" align="right"><STRONG class="smallText">Status:</STRONG></TD>
											<TD class="smallText">The current status of the module.</TD>
										</TR>
										<TR>
											<TD vAlign="top" align="right"><STRONG class="smallText">A:</STRONG></TD>
											<TD class="smallText">Designates that you&nbsp;are an author of this module.</TD>
										</TR>
										<TR>
											<TD vAlign="top" align="right"><STRONG class="smallText">S:</STRONG></TD>
											<TD class="smallText">Designates that you submitted this module.</TD>
										</TR>
									</TABLE>
								</STRONG>
								<BR>
							</P>
						</asp:panel></td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
