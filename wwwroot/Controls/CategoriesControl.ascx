<%@ Control Language="c#" AutoEventWireup="false" Codebehind="CategoriesControl.ascx.cs" Inherits="SwenetDev.Controls.CategoriesControl" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="cc1" Namespace="SwenetDev.Controls" Assembly="Swenet 0.2" %>
<%@ Register TagPrefix="uc1" TagName="CategoriesSelect" Src="CategoriesSelect.ascx" %>
<p>
	<cc1:dataeditcontrol id="CategoriesEditor" runat="server" reorderingenabled="false" reordingenabled="False">
		<itemtemplate>
			<%# DataBinder.Eval(Container.DataItem, "LongText") %>
		</itemtemplate>
		<headertemplate>
SEEK Categories 
</headertemplate>
		<edititemtemplate>
<strong>Add Category:</strong> (SEEK Unit is optional)<br>
<uc1:categoriesselect id="CatSelect" runat="server"></uc1:categoriesselect><br>
<div style="POSITION: relative; TOP: 3px" align="right">
				<asp:button id="Button1" runat="server" cssclass="defaultButton" commandname="update" text="Apply" causesvalidation="False" ToolTip="Move Up"></asp:button>
				<asp:button id="Button2" runat="server" cssclass="defaultButton" commandname="cancel" text="Cancel" causesvalidation="False" ToolTip="Move Down"></asp:button></div>
</edititemtemplate>
	</cc1:dataeditcontrol>
<p></p>
