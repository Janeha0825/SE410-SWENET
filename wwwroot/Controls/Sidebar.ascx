<%@ Import Namespace="SwenetDev.DBAdapter" %>
<%@ Import Namespace="System.Web.Security" %>
<%@ Control Language="c#" CodeBehind="Sidebar.ascx.cs" AutoEventWireup="false" Inherits="SwenetDev.Controls.Sidebar" targetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript">
<!--

	function displaySEEK(role) { SEEKAreaDiv.className = "SEEKStyle" + role; }
	
	function undisplaySEEK() { SEEKAreaDiv.className = "HiddenStyle"; }
	
	function highlightLabel(selectedLabel)   { selectedLabel.className = "ItemStyleHigh"; }
	
	function unhighlightLabel(selectedLabel) { selectedLabel.className = "ItemStyle"; }
	
	function displayAdmin()    { AdminDiv.className  = "AdminStyle"; }
	
	function undisplayAdmin()  { AdminDiv.className  = "HiddenStyle"; }
	
	function displayEditor()   { EditorDiv.className = "EditorStyle"; }
	
	function undisplayEditor() { EditorDiv.className = "HiddenStyle"; }
	
	function displayUser()     { UserDiv.className   = "UserStyle"; }
	
	function undisplayUser()   { UserDiv.className   = "HiddenStyle"; }

-->
</script>
<style> 
		.HiddenStyle { VISIBILITY: hidden; POSITION: absolute } 
		.SEEKStyle-1 { BORDER-RIGHT: gray thin solid; BORDER-TOP: gray thin solid; DISPLAY: block; Z-INDEX: 5; LEFT: 144px; WIDTH: 300px; BORDER-BOTTOM: gray thin solid; POSITION: absolute; TOP: 20px; BACKGROUND-COLOR: #94afc0 } 
		.SEEKStyle0 { BORDER-RIGHT: gray thin solid; BORDER-TOP: gray thin solid; DISPLAY: block; Z-INDEX: 5; LEFT: 144px; WIDTH: 300px; BORDER-BOTTOM: gray thin solid; POSITION: absolute; TOP: 123px; BACKGROUND-COLOR: #94afc0 } 
		.SEEKStyle2 { BORDER-RIGHT: gray thin solid; BORDER-TOP: gray thin solid; DISPLAY: block; Z-INDEX: 5; LEFT: 144px; WIDTH: 300px; BORDER-BOTTOM: gray thin solid; POSITION: absolute; TOP: 123px; BACKGROUND-COLOR: #94afc0 } 
		.SEEKStyle3 { BORDER-RIGHT: gray thin solid; BORDER-TOP: gray thin solid; DISPLAY: block; Z-INDEX: 5; LEFT: 144px; WIDTH: 300px; BORDER-BOTTOM: gray thin solid; POSITION: absolute; TOP: 123px; BACKGROUND-COLOR: #94afc0 } 
		.SEEKStyle4 { BORDER-RIGHT: gray thin solid; BORDER-TOP: gray thin solid; DISPLAY: block; Z-INDEX: 5; LEFT: 144px; WIDTH: 300px; BORDER-BOTTOM: gray thin solid; POSITION: absolute; TOP: 123px; BACKGROUND-COLOR: #94afc0 } 
		.AdminStyle { BORDER-RIGHT: gray thin solid; BORDER-TOP: gray thin solid; DISPLAY: block; Z-INDEX: 5; LEFT: 144px; WIDTH: 300px; BORDER-BOTTOM: gray thin solid; POSITION: absolute; TOP: 203px; BACKGROUND-COLOR: #94afc0 } 
		.EditorStyle { BORDER-RIGHT: gray thin solid; BORDER-TOP: gray thin solid; DISPLAY: block; Z-INDEX: 5; LEFT: 144px; WIDTH: 425px; BORDER-BOTTOM: gray thin solid; POSITION: absolute; TOP: 168px; BACKGROUND-COLOR: #94afc0 } 
		.UserStyle { BORDER-RIGHT: gray thin solid; BORDER-TOP: gray thin solid; DISPLAY: block; Z-INDEX: 5; LEFT: 144px; WIDTH: 300px; BORDER-BOTTOM: gray thin solid; POSITION: absolute; TOP: 95px; BACKGROUND-COLOR: #94afc0 } 
		.ItemStyle { FONT: bold small-caps 8pt Verdana Bold} 
		.EditorHeaderStyle { FONT: bold small-caps 10pt Verdana Bold } 
		.UserHeaderStyle { FONT: bold small-caps 10pt Verdana Bold } 
		.ItemStyleHigh { Z-INDEX: 5; FONT: bold small-caps 8pt Verdana Bold; COLOR: white; BACKGROUND-COLOR: #339999 }
		.ViewSEEKStyle { FONT-WEIGHT: bold; FONT-SIZE: 10pt; LEFT: 24px; POSITION: relative; TOP: 27px } 
		.ViewSEEKStyle0 { FONT-WEIGHT: bold; FONT-SIZE: 10pt; LEFT: 24px; POSITION: relative; TOP: 106px } 
		.ViewSEEKStyle2 { FONT-WEIGHT: bold; FONT-SIZE: 10pt; LEFT: 24px; POSITION: relative; TOP: 128px } 
		.ViewSEEKStyle3 { FONT-WEIGHT: bold; FONT-SIZE: 10pt; LEFT: 24px; POSITION: relative; TOP: 149px } 
		.ViewSEEKStyle4 { FONT-WEIGHT: bold; FONT-SIZE: 10pt; LEFT: 24px; POSITION: relative; TOP: 133px }
		.MyAccountStyle { FONT-WEIGHT: bold; FONT-SIZE: 10pt; LEFT: 60px; POSITION: relative; TOP: 75px }
		.MyAccountStyle:hover { COLOR: #FF3333; }
		.ModuleSubmitStyle { FONT-WEIGHT: bold; FONT-SIZE: 10pt; LEFT: 39px; POSITION: relative; TOP: 95px } 
		.ModuleSubmitStyle:hover { COLOR: #FF3333; }
		.EditorLabelStyle { FONT-WEIGHT: bold; FONT-SIZE: 10pt; LEFT: 50px; POSITION: relative; TOP: 115px } 
		.EditorLabelStyle:hover { COLOR: #FF3333; }
		.AdminLabelStyle  { FONT-WEIGHT: bold; FONT-SIZE: 10pt; LEFT: 58px; POSITION: relative; TOP: 135px }
		.AdminLabelStyle:hover { COLOR: #FF3333; }
		.BackgroundStyle-1 { BACKGROUND-IMAGE: url(../images/SidebarFade.jpg); WIDTH: 145px; BACKGROUND-REPEAT: no-repeat; POSITION: relative; TOP: 0px; HEIGHT: 600px } 
		.BackgroundStyle0  { BACKGROUND-IMAGE: url(../images/SidebarAuthFade.jpg); WIDTH: 145px; BACKGROUND-REPEAT: no-repeat; POSITION: relative; TOP: 0px; HEIGHT: 600px } 
		.BackgroundStyle2  { BACKGROUND-IMAGE: url(../images/SidebarAuthSubFade.jpg); WIDTH: 145px; BACKGROUND-REPEAT: no-repeat; POSITION: relative; TOP: 0px; HEIGHT: 600px } 
		.BackgroundStyle3  { BACKGROUND-IMAGE: url(../images/SidebarAuthSubEdFade.jpg); WIDTH: 145px; BACKGROUND-REPEAT: no-repeat; POSITION: relative; TOP: 0px; HEIGHT: 600px } 
		.BackgroundStyle4  { BACKGROUND-IMAGE: url(../images/SidebarAuthSubEdAdFade.jpg); WIDTH: 145px; BACKGROUND-REPEAT: no-repeat; POSITION: relative; TOP: 0px; HEIGHT: 600px } 
		.LoggedLabel { FONT-SIZE: 10pt; LEFT: 27px; POSITION: absolute; TOP: 25px }
</style>
	<% if (userRole == 0 && Page.User.Identity.IsAuthenticated) { %>
	<div id="categories" class="BackgroundStyle0">
		<% } else if (userRole == 1) { %>
		<div id="categories" class="BackgroundStyle0">
			<% } else if (userRole == 2) { %>
			<div id="categories" class="BackgroundStyle2">
				<% } else if (userRole == 3) { %>
				<div id="categories" class="BackgroundStyle3">
					<% } else if (userRole == 4) { %>
						<div id="categories" class="BackgroundStyle4">
					<% } else { %>
						<div id="categories" class="BackgroundStyle-1"><% } %>
	<% if ( Page.User.Identity.IsAuthenticated ) { %>
			<a class="LoggedLabel">Logged in as<br>
						<strong>
							<%= Page.User.Identity.Name %>
						</strong></a>
					<br>
					<br>
						<asp:Label id="UserLabel" OnLoad="UserLabel_Ld" Runat="server">
							<strong><A class="MyAccountStyle" href="MyAccount.aspx">My Account</A></strong>
						</asp:Label>
						<DIV class="HiddenStyle" id="UserDiv">
						<asp:Panel ID="UserPanel" Runat="server" OnLoad="UserPanel_Ld" Width="300px">
						<A class="UserHeaderStyle">Your Account</A>
							<HR>
							<A class="ItemStyle" href="editUserInfo.aspx">Edit</A><BR>
							<A class=ItemStyle href="viewUserInfo.aspx?username=<%= Page.User.Identity.Name %>">View</A>
							<BR>
							<BR>
						</asp:Panel>
						</DIV>
						<br>
					<% if ( Page.User.IsInRole( UserRole.Submitter.ToString() ) ) { %>
					<strong><A class="ModuleSubmitStyle" href="editModule.aspx">Upload Module</A></strong>
					<br>
					<% } if ( Page.User.IsInRole( UserRole.Editor.ToString() ) ) { %>
						<asp:Label id="EditorLabel" OnLoad="EditorLabel_Ld" Runat="server">
							<strong><A class="EditorLabelStyle" href="EditorsPage.aspx">Editor's Page</A></strong>
						</asp:Label>
						<DIV class="HiddenStyle" id="EditorDiv">
						<asp:Panel ID="EditorPanel" Runat="server" OnLoad="EditorPanel_Ld" Width="425px">
							<asp:Label id="Editor_FacReqs" Runat="server" CssClass="EditorHeaderStyle">Pending Faculty Requests</asp:Label>
							<asp:datagrid id="FacultyGrid" GridLines="None" cssclass="borderedTable" AutoGenerateColumns="False"
								runat="server" cellpadding="3" width="425px">
								<columns>
									<asp:templatecolumn headertext="Date">
										<headerstyle width="12%" cssclass="lightGrayHeader"></headerstyle>
										<itemtemplate>
											<asp:Label id="Label4" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Date", "{0:d}") %>'>
											</asp:Label>
										</itemtemplate>
									</asp:templatecolumn>
									<asp:templatecolumn headertext="Username">
										<headerstyle width="23%" cssclass="lightGrayHeader"></headerstyle>
										<itemtemplate>
											<asp:HyperLink id="Hyperlink6" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.UserName") %>' NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.UserName", "../viewUserInfo.aspx?username={0}") %>'>
											</asp:HyperLink>
										</itemtemplate>
									</asp:templatecolumn>
									<asp:templatecolumn headertext="Approve">
										<headerstyle width="10%" cssclass="lightGrayHeader"></headerstyle>
										<itemtemplate>
											<asp:HyperLink id="Hyperlink7" runat="server" Text="Approve" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.UserName", "../editorActionEmail.aspx?type=0&username={0}&approved=true") %>'>Approve</asp:HyperLink>
										</itemtemplate>
									</asp:templatecolumn>
									<asp:templatecolumn headertext="Reject">
										<headerstyle width="7%" cssclass="lightGrayHeader"></headerstyle>
										<itemtemplate>
											<asp:HyperLink id="Hyperlink8" runat="server" Text="Reject" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.UserName", "../editorActionEmail.aspx?type=0&username={0}&approved=false") %>'>Reject</asp:HyperLink>
										</itemtemplate>
									</asp:templatecolumn>
								</columns>
							</asp:datagrid>
							<HR>
							<asp:Label id="Editor_SubmReqs" Runat="server" CssClass="EditorHeaderStyle">Pending Submitter Requests</asp:Label><BR>
							<asp:datagrid id="SubmitterGrid" GridLines="None" cssclass="borderedTable" runat="server" cellpadding="3"
								width="425px" autogeneratecolumns="False">
								<columns>
									<asp:templatecolumn headertext="Date">
										<headerstyle width="12%" cssclass="lightGrayHeader"></headerstyle>
										<itemtemplate>
											<asp:Label id="Label3" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Date", "{0:d}") %>'>
											</asp:Label>
										</itemtemplate>
									</asp:templatecolumn>
									<asp:templatecolumn headertext="Username">
										<headerstyle width="23%" cssclass="lightGrayHeader"></headerstyle>
										<itemtemplate>
											<asp:HyperLink id="HyperLink1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.UserName") %>' NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.UserName", "../viewUserInfo.aspx?username={0}") %>'>
											</asp:HyperLink>
										</itemtemplate>
									</asp:templatecolumn>
									<asp:templatecolumn headertext="Approve">
										<headerstyle width="10%" cssclass="lightGrayHeader"></headerstyle>
										<itemtemplate>
											<asp:HyperLink id="HyperLink2" runat="server" Text="Approve" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.UserName", "../editorActionEmail.aspx?type=1&username={0}&approved=true") %>'>Approve</asp:HyperLink>
										</itemtemplate>
									</asp:templatecolumn>
									<asp:templatecolumn headertext="Reject">
										<headerstyle width="7%" cssclass="lightGrayHeader"></headerstyle>
										<itemtemplate>
											<asp:HyperLink id="HyperLink3" runat="server" Text="Reject" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.UserName", "../editorActionEmail.aspx?type=1&username={0}&approved=false") %>'>Reject</asp:HyperLink>
										</itemtemplate>
									</asp:templatecolumn>
								</columns>
							</asp:datagrid>
							<HR>
							<asp:Label id="Editor_ModuleReqs" Runat="server" CssClass="EditorHeaderStyle">Pending Module Requests</asp:Label><BR>
							<asp:datagrid id="ModulesGrid" GridLines="None" cssclass="borderedTable" runat="server" cellpadding="3"
								width="425px" autogeneratecolumns="False">
								<columns>
									<asp:templatecolumn headertext="Date">
										<headerstyle width="12%" cssclass="lightGrayHeader"></headerstyle>
										<itemtemplate>
											<asp:Label id="Label5" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Date", "{0:d}") %>'>
											</asp:Label>
										</itemtemplate>
									</asp:templatecolumn>
									<asp:hyperlinkcolumn target="_blank" datanavigateurlfield="Id" datanavigateurlformatstring="../viewModule.aspx?moduleID={0}"
										datatextfield="Title" headertext="Module Title">
										<headerstyle width="23%" cssclass="lightGrayHeader"></headerstyle>
									</asp:hyperlinkcolumn>
									<asp:templatecolumn headertext="Approve">
										<headerstyle width="10%" cssclass="lightGrayHeader"></headerstyle>
										<itemtemplate>
											<asp:HyperLink id="HyperLink4" runat="server" Text="Approve" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.ApproveUrl", "../{0}") %>'>Approve</asp:HyperLink>
										</itemtemplate>
									</asp:templatecolumn>
									<asp:templatecolumn headertext="Reject">
										<headerstyle width="7%" cssclass="lightGrayHeader"></headerstyle>
										<itemtemplate>
											<asp:HyperLink id="HyperLink5" runat="server" Text="Reject" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.RejectUrl", "../{0}") %>'>Reject</asp:HyperLink>
										</itemtemplate>
									</asp:templatecolumn>
								</columns>
							</asp:datagrid><BR>
							</asp:Panel>
						</DIV>
						<BR>
					<% } if ( Page.User.IsInRole( UserRole.Admin.ToString() ) ) { %>
					
						<asp:Label id="AdminLabel" OnLoad="AdminLabel_Ld" Runat="server">
							<strong><A class="AdminLabelStyle" href="AdminPage.aspx">Admin Page</A></strong></asp:Label>
						<DIV class="HiddenStyle" id="AdminDiv">
						<asp:Panel ID="AdminPanel" Runat="server" OnLoad="AdminPanel_Ld" Width="300px">
							<asp:Label id="Admin_UserManLabel" Runat="server" CssClass="ItemStyle">
								<a href="UserManagement.aspx">User Management</a></asp:Label><BR>
							<HR>
							<asp:Label id="Admin_ChangePassLabel" Runat="server" CssClass="ItemStyle">
								<a href="ChangePassword.aspx">Change User Password</a></asp:Label><BR>
							<HR>
							<asp:Label id="Admin_ModuleManLabel" Runat="server" CssClass="ItemStyle">
								<a href="ModuleManagement.aspx">Module Management</a></asp:Label>
						</asp:Panel>		
						</DIV>
					<br>
					<br>
					<% } %>
					<br>
					<br>
					<% } %>
						<asp:HyperLink id="SEEKLabel" OnLoad="SEEKLabel_Ld" Runat="server" Text="View SEEK Areas" CssClass="ViewSEEKStyle" NavigateUrl="../browseModules.aspx" />
						<DIV class="HiddenStyle" id="SEEKAreaDiv">
						<asp:Panel ID="RepeaterPanel" Runat="server" OnLoad="RepeaterPanel_Ld" Width="300px">
								<asp:Label id="AllLabel" Runat="server" CssClass="ItemStyle">
								<a href="browseModules.aspx" style="ItemStyle">All</a></asp:Label>
							<HR>
							<asp:repeater id="SEEKAreaRepeater" runat="server">
								<itemtemplate>
									<asp:Label Runat="server" CssClass="ItemStyle" ID="SEEKAreasLabel" OnLoad="SEEKAreasLabel_Ld">
										<a href='<%# "browseModules.aspx?categoryID=" + DataBinder.Eval( Container.DataItem, "Id" ) %>'>
											<%#  DataBinder.Eval( Container.DataItem, "Text" ) %>
										</a>
									</asp:Label>
								</itemtemplate>
								<separatortemplate>
									<br>
									<hr>
								</separatortemplate>
							</asp:repeater>
							</asp:Panel>
						</DIV>
					
					<br>
				
</div>
