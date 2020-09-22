<%@ Control Language="c#" AutoEventWireup="false" Codebehind="PrerequisitesControl.ascx.cs" Inherits="SwenetDev.Controls.PrerequisitesControl" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="cc1" Namespace="SwenetDev.Controls" Assembly="Swenet 0.2" %>
<cc1:dataeditcontrol id="PrereqsEditor" runat="server">
	<headertemplate>Prerequisite Knowledge</headertemplate>
	<itemtemplate>
		<asp:linkbutton id="Linkbutton1" cssclass="editLink" causesvalidation="False" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "Text") %>' commandname="edit"></asp:linkbutton>
	</itemtemplate>
	<edititemtemplate>
		<asp:textbox id="PrereqTextbox" tabindex="0" runat="server" text='<%# SwenetDev.Globals.formatTextOutput( DataBinder.Eval(Container.DataItem, "Text") ) %>' textmode="MultiLine" rows="4" width="100%" columns="70">
		</asp:textbox>
		<br>
		<div style="POSITION: relative; TOP: 3px" align="right">
			<asp:requiredfieldvalidator id="Requiredfieldvalidator2" runat="server" controltovalidate="PrereqTextbox" errormessage="Prerequisite field may not be blank." display="Dynamic"></asp:requiredfieldvalidator>&nbsp;
			<asp:regularexpressionvalidator id="RegExValidator1" runat="server" controltovalidate="PrereqTextbox" errormessage='<%# "Entry exceeds maximum length of " + SwenetDev.Controls.PrerequisitesControl.MaxLength + "."%>' validationexpression='<%# ".{1," + SwenetDev.Controls.PrerequisitesControl.MaxLength + "}" %>' display="Dynamic"></asp:regularexpressionvalidator>
			<asp:button id="Button2" runat="server" text="Apply" commandname="update" cssclass="defaultButton"></asp:button>
			<asp:button id="Button3" runat="server" text="Cancel" commandname="cancel" cssclass="defaultButton"
				causesvalidation="False"></asp:button></div>
	</edititemtemplate>
</cc1:dataeditcontrol>
