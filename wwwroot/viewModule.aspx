<%@ Register TagPrefix="swenet" TagName="header" Src="Controls\Header.ascx" %>
<%@ Register TagPrefix="swenet" TagName="sidebar" Src="Controls\Sidebar.ascx" %>
<%@ Page language="c#" Debug="true" Codebehind="viewModule.aspx.cs" AutoEventWireup="false" Inherits="SwenetDev.viewModule" smartNavigation="True" %>
<%@ Register TagPrefix="uc1" TagName="RateModuleControl" Src="Controls/RateModuleControl.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>View Module</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="style.css" type="text/css" rel="stylesheet">
		<script language="javascript">
		<!--
			function openWin(url) {
				window.open(url, "comments", "'toolbar=0,scrollbars=1,location=0,statusbar=0,menubar=0,resizable=1,width=538,height=350'");
			}
			
			function changeImage(name, image) {
			
				name.src = image;
				
			}
			
		-->
		</script>
	</HEAD>
	<body>
		<form id="Form1" runat="server">
			<swenet:header id="Header" runat="server"></swenet:header>
			<table width="750" cellspacing="1" cellpadding="1">
				<tr>
					<td valign="top" width="150"><swenet:sidebar id="Sidebar" runat="server"></swenet:sidebar></td>
					<td valign="top" width="600">
						<h1>View Module
						</h1>
						<p><asp:label id="lblMessage" runat="server"></asp:label></p>
						<% if ( ModInfo != null ) { %>
						<h2>General Information</h2>
						<p>
							<table class="borderedTable" id="Table1" cellspacing="0" cellpadding="5" rules="all" width="600"
								border="1">
								<tr>
									<td class="lightGrayHeader" style="WIDTH: 87px"><strong>Title</strong></td>
									<td><%= ModInfo.Title %></td>
								</tr>
								<tr>
									<td class="lightGrayHeader" style="WIDTH: 87px"><strong>Version Info</strong></td>
									<td>Version
										<%= ModInfo.Version %>
										, submitted by
										<%= ModInfo.Submitter %>
										on
										<%= ModInfo.Date.ToShortDateString() %>
										at
										<%= ModInfo.Date.ToShortTimeString() %>
										<br>
										<a href='<%= "javascript:openWin(\"ViewComments.aspx?baseID=" + ModInfo.BaseId + "\");" %>' class=smallText>
											View Revision History</a></td>
								</tr>
								<tr>
									<td class="lightGrayHeader" style="WIDTH: 87px"><strong>Module Identifier</strong></td>
									<td><%= ModInfo.ModuleIdentifier %></td>
								</tr>
								<tr>
									<td class="lightGrayHeader" style="WIDTH: 87px"><strong>Abstract</strong></td>
									<td><%= ModInfo.Abstract %></td>
								</tr>
								<tr>
									<td class="lightGrayHeader" style="WIDTH: 87px"><strong>Size</strong></td>
									<td><asp:repeater id="SizeRepeater" runat="server">
											<itemtemplate>
												<%# Container.DataItem %>
											</itemtemplate>
											<separatortemplate>
												<br />
											</separatortemplate>
										</asp:repeater></td>
								</tr>
								<tr>
									<td class="lightGrayHeader" style="WIDTH: 87px"><strong>Comments</strong></td>
									<td><%= ModInfo.AuthorComments %></td>
								</tr>
							</table>
						</p>
						<h2>SEEK Categories</h2>
						<asp:repeater id="CategoriesRepeater" runat="server">
							<itemtemplate>
								<li>
									<a href='<%# "browseModules.aspx?categoryID=" + DataBinder.Eval( Container.DataItem, "Id" ) %>' title="Click to view other modules in this category.">
										<%# DataBinder.Eval( Container.DataItem, "LongText" ) %>
									</a>
								</li>
							</itemtemplate>
							<headertemplate>
								<ol>
							</headertemplate>
							<footertemplate>
								</ol>
							</footertemplate>
						</asp:repeater>
						<h2>Authors</h2>
						<asp:repeater id="AuthorsRepeater" runat="server">
							<itemtemplate>
								<li>
									<asp:hyperlink id="UserLink" title="Click to view user info." runat="server" text='<%# DataBinder.Eval( Container.DataItem, "Name" ) %>' navigateurl='<%# "viewUserInfo.aspx?username=" + DataBinder.Eval( Container.DataItem, "UserName" ) %>' />
								</li>
							</itemtemplate>
							<headertemplate>
								<ol>
							</headertemplate>
							<footertemplate>
								</ol>
							</footertemplate>
						</asp:repeater>
						<h2>Prerequisites</h2>
						<asp:repeater id="PrereqRepeater" runat="server">
							<itemtemplate>
								<li>
									<%# DataBinder.Eval( Container.DataItem, "Text" ) %>
								</li>
							</itemtemplate>
							<headertemplate>
								<ol>
							</headertemplate>
							<footertemplate>
								</ol>
							</footertemplate>
						</asp:repeater>
						<h2>Learning Objectives</h2>
						<asp:repeater id="ObjectivesRepeater" runat="server">
							<itemtemplate>
								<li>
									<strong>
										<%# DataBinder.Eval( Container.DataItem, "BloomLevel" ) %>
									</strong>-
									<%# DataBinder.Eval( Container.DataItem, "Text" ) %>
								</li>
							</itemtemplate>
							<headertemplate>
								<ol>
							</headertemplate>
							<footertemplate>
								</ol>
							</footertemplate>
						</asp:repeater>
						<h2>Topics</h2>
						<asp:repeater id="TopicsRepeater" runat="server">
							<headertemplate>
								<ol>
							</headertemplate>
							<itemtemplate>
								<li>
									<%# DataBinder.Eval( Container.DataItem, "Text" ) %>
								</li>
							</itemtemplate>
							<footertemplate>
								</ol>
							</footertemplate>
						</asp:repeater>
						<h2>Materials</h2>
						<asp:repeater id="MaterialsRepeater" runat="server">
							<itemtemplate>	
								<li>
									<%# DataBinder.Eval( Container.DataItem, "IdentInfo" ) %>
									<%# DataBinder.Eval( Container.DataItem, "Link" ) %>
									<a href='<%# "materialRatings.aspx?materialId=" + DataBinder.Eval( Container.DataItem, "MatID" ) %>'>
									<asp:Image Runat="server" id="Stars" ImageUrl='<%# DataBinder.Eval( Container.DataItem, "RatingImage" ) %>' ImageAlign="absmiddle" OnLoad="Stars_Ld"/></a>
									<strong><small><%# DataBinder.Eval( Container.DataItem, "Rating" , "{0:f}") %>/5</asp:Label></small></strong> 
									<a>[<asp:HyperLink id="MaterialRating" NavigateUrl='<%# "addMaterialRating.aspx?materialId=" + DataBinder.Eval( Container.DataItem, "MatID" ) %>' Font-Size=7 runat="server" OnLoad="Rate_Ld">Rate Material</asp:HyperLink>]</a>
								</li>	
							</itemtemplate>									
							<headertemplate>
								<ol>
							</headertemplate>
							<footertemplate>
								</ol><br>
							</footertemplate>
							<SeparatorTemplate>
								<hr>
							</SeparatorTemplate>
						</asp:repeater>
						<asp:Panel ID="ButtonPanel" Runat="server">
							<P>&nbsp;&nbsp;&nbsp;
								<asp:Button id="MaterialsZipButton" runat="server" Text="Download Zip" CausesValidation="False" CssClass="defaultButton" Width="110px"></asp:Button>
							</P>
						</asp:Panel>
						<h2>See Also...</h2>
						<asp:repeater id="SeeAlsoRepeater" runat="server">
							<itemtemplate>
								<li>
									<%# DataBinder.Eval( Container.DataItem, "Description" ) %>
									-
									<asp:HyperLink id="SeeAlsoLink" runat="server" NavigateUrl='<%# "/viewModule.aspx?moduleID=" + DataBinder.Eval( Container.DataItem, "ModuleID" ) %>'>
										<%# DataBinder.Eval( Container.DataItem, "Title" ) %>
									</asp:HyperLink>
								</li>
							</itemtemplate>
							<headertemplate>
								<ol>
							</headertemplate>
							<footertemplate>
								</ol>
							</footertemplate>
						</asp:repeater>
						<H2>Other Resources
						</H2>
						<asp:repeater id="ResourcesRepeater" runat="server">
							<headertemplate>
								<ol>
							</headertemplate>
							<itemtemplate>
								<li>
									<%# DataBinder.Eval( Container.DataItem, "Text" ) %>
									<%# renderResourceLink( (SwenetDev.DBAdapter.Resources.ResourceInfo)Container.DataItem ) %>
								</li>
							</itemtemplate>
							<footertemplate>
								</ol>
							</footertemplate>
						</asp:repeater>
						<hr width="100%" color="#999999" noshade size="1">
						<h2>Ratings</h2>
						<a name="rating">
							<uc1:ratemodulecontrol id="RateModuleControl1" runat="server"></uc1:ratemodulecontrol></a>
						<asp:label id="RatingsMessage" runat="server"></asp:label><strong></strong>
						<h2>Discussions</h2>
						<p><a href="Forums/">Discuss this module</a> in the forums.</p>
						<h2 id="H21" runat="server">Related Modules</h2>
						<asp:repeater id="RelatedRepeater" runat="server">
							<itemtemplate>
								<a href='<%# DataBinder.Eval( Container.DataItem, "Id", "viewModule.aspx?moduleID={0}" ) %>'>
									<%# DataBinder.Eval( Container.DataItem, "Title" ) %>
								</a>
							</itemtemplate>
							<separatortemplate>
								<br />
							</separatortemplate>
						</asp:repeater>
						<% } %>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
