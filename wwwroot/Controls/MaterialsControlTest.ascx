<%@ Control Language="c#" CodeBehind="MaterialsControlTest.ascx.cs" AutoEventWireup="false" Inherits="SwenetDev.Controls.MaterialsControlTest" targetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="cc1" Namespace="SwenetDev.Controls" Assembly="Swenet 0.2" %>
<!-- Insert content here -->
<p>
	<cc1:dataeditcontrol id="MaterialsEditor" runat="server">
		<headertemplate>Materials</headertemplate>
		<itemtemplate>
			<table>
				<tr>
					<td valign="top"><strong>Identifying Info:</strong></td>
					<td>
						<asp:linkbutton id="Linkbutton2" causesvalidation="False" runat="server" cssclass="editLink" commandname="edit" text='<%# DataBinder.Eval(Container.DataItem, "IdentInfo") %>'>
						</asp:linkbutton></td>
				</tr>
				<tr>
					<td><strong>File:</strong></td>
					<td>
						<asp:linkbutton id="Linkbutton3" causesvalidation="False" runat="server" cssclass="editLink" commandname="edit" text='<%# DataBinder.Eval(Container.DataItem, "Link") %>'>
						</asp:linkbutton></td>
				</tr>
			</table>
		</itemtemplate>
		<edititemtemplate>
			<table>
				<tr>
					<td valign="top"><strong>Identifying Info:</strong></td>
					<td>
						<asp:textbox id="InfoTextbox" runat="server" text='<%# SwenetDev.Globals.formatTextOutput( DataBinder.Eval(Container.DataItem, "IdentInfo") ) %>' rows="4" textmode="MultiLine" columns="55">
						</asp:textbox></td>
				</tr>
				<tr>
					<td><strong>File:</strong></td>
					<td><input id="FileUpload" style="WIDTH: 100%" type="file" name="FileUpload" runat="server"></td>
				</tr>
			</table>
			<br>
			<div style="POSITION: relative; TOP: 3px" align="right">
				<asp:requiredfieldvalidator id="Requiredfieldvalidator3" runat="server" controltovalidate="InfoTextbox" errormessage="Identifying info field may not be blank." display="Dynamic"></asp:requiredfieldvalidator>
				<asp:regularexpressionvalidator id="RegExValidator1" runat="server" controltovalidate="InfoTextbox" errormessage='<%# "Entry exceeds maximum length of " + SwenetDev.Controls.MaterialsControlTest.MaxLength + "."%>' validationexpression='<%# ".{1," + SwenetDev.Controls.MaterialsControlTest.MaxLength + "}" %>' display="Dynamic"></asp:regularexpressionvalidator>&nbsp;
				<asp:requiredfieldvalidator id="Requiredfieldvalidator4" runat="server" controltovalidate="FileUpload" errormessage="File field may not be blank." display="Dynamic"></asp:requiredfieldvalidator>&nbsp;
				<asp:button id="Button4" runat="server" cssclass="defaultButton" commandname="update" text="Apply"></asp:button>
				<asp:button id="Button5" runat="server" cssclass="defaultButton" commandname="cancel" text="Cancel" 
 causesvalidation="False"></asp:button></div>
		</edititemtemplate>
	</cc1:dataeditcontrol></p>
