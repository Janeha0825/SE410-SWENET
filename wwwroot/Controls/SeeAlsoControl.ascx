<%@ Control Language="c#" AutoEventWireup="false" Codebehind="SeeAlsoControl.ascx.cs" Inherits="SwenetDev.Controls.SeeAlsoControl" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%@ Register TagPrefix="cc1" Namespace="SwenetDev.Controls" Assembly="Swenet 0.2" %>
<%@ import Namespace="System.Data" %>
<cc1:dataeditcontrol id="SeeAlsoEditor" runat="server">
	<headertemplate>See Also... </headertemplate>
	<itemtemplate>
		<table>
			<tr>
				<td>
					<strong>Description:</strong>
					<asp:linkbutton id="Linkbutton1" causesvalidation="False" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "Description") %>' commandname="edit" cssclass="editLink"></asp:linkbutton>
				</td>
			</tr>
			<tr>
				<td>
					<strong>Module Identifier:</strong>
					<asp:linkbutton id="Linkbutton2" causesvalidation="False" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "AltModule") %>' commandname="edit" cssclass="editLink"></asp:linkbutton>	
				</td>
			</tr>
			<tr>
				<td>
					<strong>Module Title:</strong>
					<asp:linkbutton id="Linkbutton3" causesvalidation="False" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "Title") %>' commandname="edit" cssclass="editLink"></asp:linkbutton>
				</td>
			</tr>
		</table>
	</itemtemplate>
	<edititemtemplate>
		<table>
			<tr>
				<td valign="top" style="WIDTH: 125px"><strong>Description:</strong></td>
				<td>
					<asp:textbox id="DescriptionTextbox" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "Description") %>' columns="54" textmode="MultiLine" rows="4" Width="394px">
					</asp:textbox></td>
			</tr>
			<tr>
				<td style="WIDTH: 125px"><strong>Module Identifier:</strong></td>
				<td>
					<asp:textbox id="ModuleIDTextbox" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "AltModule") %>' columns="5" Width="208px" MaxLength="35">
						</asp:textbox>
				</td>
			</tr>
		</table>
		<div style="POSITION: relative; TOP: 3px" align="right">
			<asp:requiredfieldvalidator id="Requiredfieldvalidator1" runat="server" errormessage="Description field may not be blank." controltovalidate="DescriptionTextbox" display="Dynamic"></asp:requiredfieldvalidator>
			<asp:requiredfieldvalidator id="Requiredfieldvalidator2" runat="server" errormessage="Identifier may not be blank."	controltovalidate="ModuleIDTextbox" display="Dynamic"></asp:requiredfieldvalidator>
			<asp:regularexpressionvalidator id="RegExValidator1" runat="server" controltovalidate="DescriptionTextbox" errormessage='<%# "Entry exceeds maximum length of " + SwenetDev.Controls.SeeAlsoControl.MaxLength + "."%>' validationexpression='<%# ".{1," + SwenetDev.Controls.SeeAlsoControl.MaxLength + "}" %>' display="Dynamic"></asp:regularexpressionvalidator>
			<asp:button id="Button4" runat="server" text="Apply" commandname="update" cssclass="defaultButton"></asp:button>&nbsp;
			<asp:button id="Button5" runat="server" text="Cancel" commandname="cancel" cssclass="defaultButton"
				causesvalidation="False"></asp:button></div>
	</edititemtemplate>
</cc1:dataeditcontrol>
<p><asp:label id="ErrorMessage" runat="server" forecolor="Red"></asp:label></p>
