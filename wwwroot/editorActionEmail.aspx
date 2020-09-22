<%@ OutputCache Location="None" VaryByParam="None" %>
<%@ Page language="c#" Codebehind="editorActionEmail.aspx.cs" AutoEventWireup="false" Inherits="SwenetDev.editorActionEmail" smartNavigation="True"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Send Email</title>
		<link href="style.css" type="text/css" rel="stylesheet">
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</HEAD>
	<body ms_positioning="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<p><asp:label id="ErrorMessage" runat="server" forecolor="Red"></asp:label><asp:checkbox id="DeleteCheck" runat="server" autopostback="True" text="Permanently Delete this module."
					Visible="False"></asp:checkbox></p>
			<p><strong>Custom Message:</strong><br>
				<asp:textbox id="CustomMessage" runat="server" textmode="MultiLine" columns="50" rows="4"></asp:textbox><br>
				<asp:requiredfieldvalidator id="CustomMsgValidator" runat="server" controltovalidate="CustomMessage" errormessage="Message is required."></asp:requiredfieldvalidator></p>
			<p><strong>Message to be sent to user:</strong></p>
			<table class="borderedTable" cellspacing="0" cellpadding="5" rules="all" width="500">
				<tr>
					<td bgcolor="#dedede">
						<p><strong>Subject:</strong>
							<asp:label id="SubjectLbl" runat="server"></asp:label></p>
						<p><asp:label id="MessageBody" runat="server"></asp:label></p>
					</td>
				</tr>
			</table>
			<br>
			<asp:button id="SendBtn" runat="server" text="Send" cssclass="defaultButton"></asp:button>&nbsp;
			<asp:button id="CancelBtn" runat="server" text="Cancel" cssclass="defaultButton" causesvalidation="False"></asp:button></form>
	</body>
</HTML>
