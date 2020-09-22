<%@ Control Language="c#" AutoEventWireup="false" Codebehind="ObjectivesControl.ascx.cs" Inherits="SwenetDev.Controls.ObjectivesControl" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="cc1" Namespace="SwenetDev.Controls" Assembly="Swenet 0.2" %>
<p>
	<cc1:dataeditcontrol id="ObjectivesEditor" runat="server">
		<headertemplate>Learning Objectives</headertemplate>
		<itemtemplate>
			<table>
				<tr>
					<td><strong>Bloom Level:</strong></td>
					<td>
						<asp:linkbutton id="Linkbutton2" causesvalidation="False" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "BloomLevel") %>' commandname="edit" cssclass="editLink">
						</asp:linkbutton></td>
				</tr>
				<tr>
					<td valign="top"><strong>Objective Text:</strong></td>
					<td>
						<asp:linkbutton id="Linkbutton3" causesvalidation="False" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "Text") %>' commandname="edit" cssclass="editLink">
						</asp:linkbutton></td>
				</tr>
			</table>
		</itemtemplate>
		<edititemtemplate>
			<table>
				<tr>
					<td><strong>Bloom Level:</strong></td>
					<td>
						<asp:dropdownlist id="BloomDrop" datasource='<%# SwenetDev.Globals.BloomLevels %>' selectedindex='<%# getSelectedIndex( (string)DataBinder.Eval( Container.DataItem, "BloomLevel" ) ) %>' runat="server"/></td>
				</tr>
				<tr>
					<td valign="top"><strong>Objective Text:</strong></td>
					<td>
						<asp:textbox id="ObjectiveTextbox" runat="server" text='<%# SwenetDev.Globals.formatTextOutput( DataBinder.Eval(Container.DataItem, "Text") ) %>' columns="55" textmode="MultiLine" rows="4">
						</asp:textbox>
					</td>
				</tr>
			</table>
			<br>
			<div style="POSITION: relative; TOP: 3px" align="right">
				<asp:requiredfieldvalidator id="Requiredfieldvalidator1" runat="server" errormessage="Objective text field may not be blank."
					controltovalidate="ObjectiveTextbox" display="Dynamic"></asp:requiredfieldvalidator>&nbsp;
				<asp:regularexpressionvalidator id="RegExValidator1" runat="server" controltovalidate="ObjectiveTextbox" errormessage='<%# "Entry exceeds maximum length of " + SwenetDev.Controls.ObjectivesControl.MaxLength + "characters."%>' validationexpression='<%# ".{1," + SwenetDev.Controls.ObjectivesControl.MaxLength + "}" %>' display="Dynamic"></asp:regularexpressionvalidator>
				<asp:button id="Button4" runat="server" text="Apply" commandname="update" cssclass="defaultButton"></asp:button>
				<asp:button id="Button5" runat="server" text="Cancel" commandname="cancel" cssclass="defaultButton"
					causesvalidation="False"></asp:button></div>
		</edititemtemplate>
	</cc1:dataeditcontrol></p>
