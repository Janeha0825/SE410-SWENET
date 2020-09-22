<%@ Register TagPrefix="cc1" Namespace="SwenetDev.Controls" Assembly="Swenet 0.2" %>
<%@ Control Language="c#" AutoEventWireup="false" Codebehind="TopicsControl.ascx.cs" Inherits="SwenetDev.Controls.TopicsControl" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<cc1:dataeditcontrol id="TopicsEditor" runat="server">
	<headertemplate>Topics</headertemplate>
	<itemtemplate>
		<asp:linkbutton id="EditLink" causesvalidation="False" cssclass="editLink" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "Text") %>' commandname="edit"></asp:linkbutton>
	</itemtemplate>
	<edititemtemplate>
		<asp:textbox id="EditTextbox" tabindex="0" runat="server" text='<%# SwenetDev.Globals.formatTextOutput( DataBinder.Eval(Container.DataItem, "Text") ) %>' textmode="MultiLine" rows="4" width="100%" columns="70">
		</asp:textbox>
		<br>
		<div style="POSITION: relative; TOP: 3px" align="right">
			<asp:requiredfieldvalidator id="RequiredFieldValidator1" runat="server" controltovalidate="EditTextbox" errormessage="Topic field may not be blank." display="Dynamic"></asp:requiredfieldvalidator>&nbsp;
			<asp:regularexpressionvalidator id="RegExValidator1" runat="server" controltovalidate="EditTextbox" errormessage='<%# "Entry exceeds maximum length of " + SwenetDev.Controls.TopicsControl.MaxLength + "."%>' validationexpression='<%# ".{1," + SwenetDev.Controls.TopicsControl.MaxLength + "}" %>' display="Dynamic"></asp:regularexpressionvalidator>
			<asp:button id="TopicsApplyButton" runat="server" text="Apply" commandname="update" cssclass="defaultButton"></asp:button>
			<asp:button id="TopicsCancelButton" runat="server" text="Cancel" commandname="cancel" cssclass="defaultButton"
				causesvalidation="False"></asp:button></div>
	</edititemtemplate>
</cc1:dataeditcontrol>