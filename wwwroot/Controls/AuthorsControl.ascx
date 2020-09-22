<%@ import Namespace="System.Data" %>
<%@ Control Language="c#" CodeBehind="AuthorsControl.ascx.cs" AutoEventWireup="false" Inherits="SwenetDev.Controls.AuthorsControl" %>
<%@ Register TagPrefix="cc1" Namespace="SwenetDev.Controls" Assembly="Swenet 0.2" %>
<cc1:dataeditcontrol id="AuthorsEditor" runat="server">
	<headertemplate>Authors</headertemplate>
	<itemtemplate>
		<asp:hyperlink id="UserLink" runat="server" text='<%# DataBinder.Eval( Container.DataItem, "Name" ) %>' target="_blank" navigateurl='<%# "~/viewUserInfo.aspx?username=" + DataBinder.Eval( Container.DataItem, "UserName" ) %>' />
	</itemtemplate>
	<edititemtemplate>
		<strong>Existing Authors:</strong><br />
		<asp:dropdownlist id="existAuthLst" runat="server" datatextfield="Name" datavaluefield="UserName" datasource='<%# SwenetDev.DBAdapter.Authors.getAll() %>'>
		</asp:dropdownlist>
		<br>
		<div style="POSITION: relative; TOP: 3px" align="right">
			<asp:button id="Button4" runat="server" text="Apply" commandname="update" cssclass="defaultButton"></asp:button>
			<asp:button id="Button5" runat="server" text="Cancel" commandname="cancel" cssclass="defaultButton"
				causesvalidation="False"></asp:button></div>
	</edititemtemplate>
</cc1:dataeditcontrol>
<p><asp:label id="ErrorMessage" runat="server" forecolor="Red"></asp:label></p>
