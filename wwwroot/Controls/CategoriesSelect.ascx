<%@ Control Language="c#" AutoEventWireup="false" Codebehind="CategoriesSelect.ascx.cs" Inherits="SwenetDev.Controls.CategoriesSelect" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<table>
	<tr>
		<td width="45" style="HEIGHT: 24px"><strong>Area:</strong></td>
		<td style="HEIGHT: 24px">
			<asp:dropdownlist id="AreaDrop" runat="server" datatextfield="LongText" datavaluefield="Id" autopostback="True" datasource="<%# SwenetDev.Globals.SEEKAreas %>">
			</asp:dropdownlist></td>
	</tr>
	<tr>
		<td><strong>Unit:</strong></td>
		<td>
			<asp:dropdownlist id="UnitDrop" runat="server" datatextfield="LongText" datavaluefield="Id" datasource="<%# unitsList %>">
			</asp:dropdownlist></td>
	</tr>
</table>
