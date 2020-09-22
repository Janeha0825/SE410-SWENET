<%@ Control Language="c#" AutoEventWireup="false" Codebehind="FacultyConfirmation.ascx.cs" Inherits="SwenetDev.Controls.FacultyConfirmation" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<h2>Faculty Registration</h2>
<p><asp:label id="ConfirmLbl" runat="server"></asp:label><asp:panel id="FormPanel" runat="server">
<P>Register as an authentic faculty member if you want access to faculty-only materials 
			(such as answer keys) within the modules.&nbsp; If you wish to upload modules 
			of your own, skip this step and register as a submitter below.</P>
<P></P>
<TABLE style="WIDTH: 500px; HEIGHT: 46px" cellSpacing="1" cellPadding="1">
			<TR>
				<TD style="WIDTH: 80px; HEIGHT: 17px" align="left" width="80"><STRONG>Name:</STRONG></TD>
				<TD>
					<asp:textbox id="NameBox" tabIndex="2" runat="server" width="200px"></asp:textbox>
					<asp:requiredfieldvalidator id="NameValidator" runat="server" controltovalidate="NameBox">*</asp:requiredfieldvalidator></TD>
			</TR>
			<TR>
				<TD style="WIDTH: 80px" align="left" width="80"><STRONG>Affiliation:</STRONG></TD>
				<TD>
					<asp:textbox id="AffiliationTxt" tabIndex="3" runat="server" Width="200px"></asp:textbox>
					<asp:requiredfieldvalidator id="AffiliationValidator" runat="server" controltovalidate="AffiliationTxt">*</asp:requiredfieldvalidator></TD>
			</TR>
		</TABLE>
<P><STRONG>Please reference a valid source as proof to your standing.</STRONG></P>
<asp:textbox id="ProofTxt" runat="server" textmode="MultiLine" rows="4" columns="50" maxlength="500"></asp:textbox><BR>(Limited 
to 500 characters).<BR><BR>
<asp:button id="ApplyBtn" runat="server" text="Apply" cssclass="defaultButton"></asp:button></asp:panel>
