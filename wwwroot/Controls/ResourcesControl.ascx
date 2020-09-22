<%@ Register TagPrefix="cc1" Namespace="SwenetDev.Controls" Assembly="Swenet 0.2" %>
<%@ Control Language="c#" AutoEventWireup="false" Codebehind="ResourcesControl.ascx.cs" Inherits="SwenetDev.Controls.ResourcesControl" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<cc1:dataeditcontrol id="ResourcesEditor" runat="server">
	<headertemplate>Resources</headertemplate>
	<itemtemplate>
		<table>
			<tr>
				<td valign="top"><strong>Description:</strong></td>
				<td>
					<asp:linkbutton id="Linkbutton2" causesvalidation="False" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "Text") %>' commandname="edit" cssclass="editLink">
					</asp:linkbutton></td>
			</tr>
			<tr>
				<td><strong>Link:</strong></td>
				<td>
					<asp:linkbutton id="Linkbutton3" causesvalidation="False" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "Link") %>' commandname="edit" cssclass="editLink">
					</asp:linkbutton></td>
			</tr>
		</table>
	</itemtemplate>
	<edititemtemplate>
		<table>
			<tr>
				<td valign="top"><strong>Description:</strong></td>
				<td>
					<asp:textbox id="DescriptionTextbox" runat="server" text='<%# SwenetDev.Globals.formatTextOutput( DataBinder.Eval(Container.DataItem, "Text") ) %>' columns="54" textmode="MultiLine" rows="4">
					</asp:textbox></td>
			</tr>
			<tr>
				<td><strong>Link</strong> (Optional):</td>
				<td>
					<asp:textbox id="LinkTextbox" runat="server" text='<%# SwenetDev.Globals.formatTextOutput( DataBinder.Eval(Container.DataItem, "Link") ) %>' columns="70">
					</asp:textbox></td>
			</tr>
		</table>
		<br>
		<div style="POSITION: relative; TOP: 3px" align="right">
			<asp:regularexpressionvalidator id="LinkValidator" controltovalidate="LinkTextbox" runat="server" errormessage="The link must begin with http:// or https://.<br/>"
				validationexpression="https?://.*" display="Dynamic"></asp:regularexpressionvalidator>
			<asp:requiredfieldvalidator id="Requiredfieldvalidator1" runat="server" errormessage="Description field may not be blank."
				controltovalidate="DescriptionTextbox" display="Dynamic"></asp:requiredfieldvalidator>
			<asp:regularexpressionvalidator id="RegExValidator1" runat="server" controltovalidate="DescriptionTextbox" errormessage='<%# "Description exceeds maximum length of " + SwenetDev.Controls.ResourcesControl.DescriptionLength + "."%>' validationexpression='<%# ".{1," + SwenetDev.Controls.ResourcesControl.DescriptionLength + "}" %>' display="Dynamic"></asp:regularexpressionvalidator>
			<asp:regularexpressionvalidator id="RegExValidator2" runat="server" controltovalidate="LinkTextbox" errormessage='<%# "Link exceeds maximum length of " + SwenetDev.Controls.ResourcesControl.LinkLength + "."%>' validationexpression='<%# ".{1," + SwenetDev.Controls.ResourcesControl.LinkLength + "}" %>' display="Dynamic"></asp:regularexpressionvalidator>&nbsp;
			<asp:button id="Button4" runat="server" text="Apply" commandname="update" cssclass="defaultButton"></asp:button>
			<asp:button id="Button5" runat="server" text="Cancel" commandname="cancel" cssclass="defaultButton"
				causesvalidation="False"></asp:button></div>
	</edititemtemplate>
</cc1:dataeditcontrol>
