<%@ Control Language="c#" targetSchema="http://schemas.microsoft.com/intellisense/ie5" CodeBehind="Header.ascx.cs" AutoEventWireup="false" Inherits="SwenetDev.Controls.Header" %>
<table cellspacing="0" cellpadding="0" width="750">
	<tr>
		<td align="center" bgcolor="#94afc0" colspan="2">
			<img alt="SWEnet" src="images/title4.jpg"></td>
	</tr>
	<tr>
		<td class="navigation" width="60%" bgcolor="gray" style="HEIGHT: 19px">
			<a class="navigation" href="default.aspx">Home</a>&nbsp;&nbsp;|&nbsp;&nbsp;<a href="DotWiki/" class="navigation">Wiki</a>&nbsp;&nbsp;|&nbsp;&nbsp;<a class="navigation" href="about.aspx">About</a>&nbsp;&nbsp;|&nbsp;&nbsp;<a class="navigation" href="search.aspx">Search</a>&nbsp;&nbsp;|&nbsp;&nbsp;<a class="navigation" href="searchModules.aspx">Advanced 
				Search</a>&nbsp;&nbsp;|&nbsp;&nbsp;<a href="Forums/" class="navigation">Forums</a></td>
		<td align="right" width="40%" bgcolor="gray" class="navigation" style="HEIGHT: 19px">
			<% if ( !Page.User.Identity.IsAuthenticated ) { %>
			<a class="navigation" href="login.aspx">Login</a>&nbsp;&nbsp;|&nbsp;&nbsp;<a class="navigation" href="register.aspx">Register</a>
			<% } else { %>
			<asp:linkbutton id="LogoutLink" cssclass="navigation" runat="server" causesvalidation="False">Logout</asp:linkbutton><% } %></td>
	</tr>
	<tr>
		<td colspan="2" height="12">
		</td>
	</tr>
</table>
