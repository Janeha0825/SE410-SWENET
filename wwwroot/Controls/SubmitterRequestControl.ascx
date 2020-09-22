<%@ Control Language="c#" AutoEventWireup="false" Codebehind="SubmitterRequestControl.ascx.cs" Inherits="SwenetDev.Controls.SubmitterRequestControl" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>



<h2>Module Submission</H2>
<p><asp:label id=RequestLbl runat="server"></asp:label><asp:panel 
id=FormPanel runat="server">
<p>Would you like the ability to submit modules?&nbsp; If so, fill out the form 
below.&nbsp; You will be notified by email when your request has been 
evaluated.</p>
<p></p>
<p><strong>Desired Submitter Identifier:</strong><br>
<asp:textbox id="SubmitIdTxt" runat="server" maxlength="25"></asp:textbox>&nbsp;(Defaults 
to your username)
<asp:RequiredFieldValidator id="SubmitterIdReqiredVal" runat="server" ErrorMessage="<br>Submitter identifier is required." ControlToValidate="SubmitIdTxt" Display="Dynamic"></asp:RequiredFieldValidator>
<asp:CustomValidator id="SubmitterIdCustomVal" runat="server" ErrorMessage="<br>Submitter identifier already in use." ControlToValidate="SubmitIdTxt" Display="Dynamic" EnableClientScript="False"></asp:CustomValidator></p>
<p><strong>Please enter any information that may help us in evaluating your 
request (Optional):</strong> 
<asp:textbox id="MessageTxt" runat="server" maxlength="500" columns="50" rows="4" textmode="MultiLine"></asp:textbox><br>(Limited 
to 500 characters).<br><br>
<asp:button id="ApplyBtn" runat="server" cssclass="defaultButton" text="Apply"></asp:button></p></asp:panel>
