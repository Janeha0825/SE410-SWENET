<%@ Control Language="c#" AutoEventWireup="false" Codebehind="EditUserInfoControl.ascx.cs" Inherits="SwenetDev.Controls.EditUserInfoControl" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%@ Register TagPrefix="uc1" TagName="SecretQuestion" Src="SecretQuestion.ascx" %>
<p><asp:label id="InstructionsLbl" runat="server">Username, password/confirmation, secret question/answer, name, email address, and affiliation fields are required.</asp:label></p>
<asp:validationsummary id="ValidationSummary1" runat="server" headertext="Please review the following errors and resubmit the form:"></asp:validationsummary>
<h2>User Account Information</h2>
<table id="Table1" cellSpacing="1" cellPadding="1" width="595" border="0">
	<tr>
		<td style="HEIGHT: 26px" vAlign="top" align="right" width="120"><strong>Username:</strong></td>
		<td style="HEIGHT: 26px"><asp:textbox id="txtUserName" runat="server" maxlength="25" columns="25"></asp:textbox><asp:requiredfieldvalidator id="RequiredFieldValidator1" runat="server" controltovalidate="txtUserName" errormessage="Username required.">*</asp:requiredfieldvalidator>&nbsp;<asp:regularexpressionvalidator id="Regularexpressionvalidator3" runat="server" controltovalidate="txtUserName" errormessage="Username must be at least 3 characters."
				validationexpression=".{3,}" display="Dynamic"></asp:regularexpressionvalidator><asp:regularexpressionvalidator id="AlphaNumericUsername" runat="server" controltovalidate="txtUserName" errormessage="Username may only contain letters, numbers, underscores, and hyphens."
				validationexpression="[\w-]*" display="Dynamic"><br>Username may only contain letters, numbers, underscores, and hyphens.</asp:regularexpressionvalidator></td>
	</tr>
	<tr>
		<td style="HEIGHT: 26px" vAlign="top" align="right" width="120"><strong>Password:</strong></td>
		<td style="HEIGHT: 26px"><asp:textbox id="txtPassword" runat="server" maxlength="25" columns="26" textmode="Password"></asp:textbox><asp:requiredfieldvalidator id="RequiredFieldValidator2" runat="server" controltovalidate="txtPassword" errormessage="Password required.">*</asp:requiredfieldvalidator>&nbsp;<asp:regularexpressionvalidator id="RegExValidator1" runat="server" controltovalidate="txtPassword" errormessage="Password must be at least 8 characters."
				validationexpression=".{8,}" display="Dynamic"></asp:regularexpressionvalidator><asp:regularexpressionvalidator id="RegularExpressionValidator2" runat="server" controltovalidate="txtPassword"
				errormessage="Password may only contain letters, numbers, underscores, and hyphens." validationexpression="[\w-]*" display="Dynamic"><br>Password may only contain letters, numbers, underscores, and hyphens.</asp:regularexpressionvalidator></td>
	</tr>
	<tr>
		<td style="HEIGHT: 26px" vAlign="top" align="right" width="120"><strong>Confirm:</strong></td>
		<td style="HEIGHT: 26px"><asp:textbox id="txtPasswordConfirm" runat="server" maxlength="25" columns="26" textmode="Password"></asp:textbox><asp:requiredfieldvalidator id="RequiredFieldValidator3" runat="server" controltovalidate="txtPasswordConfirm"
				errormessage="Confirm password required.">*</asp:requiredfieldvalidator>&nbsp;<asp:customvalidator id="PasswordCompareValidator" runat="server" errormessage="Passwords do not match."></asp:customvalidator></td>
	</tr>
</table>
<h2>Password Retrieval Information</h2>
<P><uc1:secretquestion id="QuestionControl" runat="server"></uc1:secretquestion></P>
<H2>Identifying Information</H2>
<table id="Table2" cellSpacing="1" cellPadding="1" width="595" border="0">
	<tr>
		<td vAlign="middle" align="right" width="120"><strong>Name: </strong>
		</td>
		<td><asp:textbox id="Name" runat="server" maxlength="50" columns="50"></asp:textbox><asp:requiredfieldvalidator id="NameRequiredValidator" runat="server" controltovalidate="Name" errormessage="Name required.">*</asp:requiredfieldvalidator></td>
	</tr>
	<tr>
		<td vAlign="middle" align="right" width="120"><strong>Email Address:</strong></td>
		<td><asp:textbox id="Email" runat="server" maxlength="50" columns="50"></asp:textbox><asp:requiredfieldvalidator id="EmailRequiredValidator" runat="server" controltovalidate="Email" errormessage="Email Address required.">*</asp:requiredfieldvalidator>&nbsp;
			<asp:regularexpressionvalidator id="RegularExpressionValidator1" runat="server" controltovalidate="Email" errormessage="Invalid email address."
				validationexpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" Display="Dynamic"><br>Invalid email address.</asp:regularexpressionvalidator></td>
	</tr>
	<tr>
		<td vAlign="middle" align="right" width="120"><strong>Title:</strong></td>
		<td><asp:textbox id="Title" runat="server" maxlength="200" columns="60" Width="424px"></asp:textbox></td>
	</tr>
	<tr>
		<td style="HEIGHT: 57px" vAlign="middle" align="right" width="120"><strong>Affiliation:</strong></td>
		<td style="HEIGHT: 57px"><asp:textbox id="Affiliation" runat="server" maxlength="255" columns="50" textmode="MultiLine"
				rows="3"></asp:textbox><asp:requiredfieldvalidator id="AffilRequiredValidator" runat="server" controltovalidate="Affiliation" errormessage="Affiliation required">*</asp:requiredfieldvalidator></td>
	</tr>
</table>
<h2>Address</h2>
<table id="Table3" cellSpacing="1" cellPadding="1" width="595" border="0">
	<tr>
		<td vAlign="middle" align="right" width="120"><strong>Address Line 1:</strong></td>
		<td><asp:textbox id="Address1" runat="server" maxlength="50" columns="50"></asp:textbox></td>
	</tr>
	<tr>
		<td vAlign="middle" align="right" width="120"><strong>Address Line 2:</strong></td>
		<td><asp:textbox id="Address2" runat="server" maxlength="50" columns="50"></asp:textbox></td>
	</tr>
	<tr>
		<td vAlign="middle" align="right" width="120"><strong>City:</strong></td>
		<td><asp:textbox id="City" runat="server" maxlength="50" columns="50"></asp:textbox></td>
	</tr>
	<tr>
		<td vAlign="middle" align="right" width="120"><strong>State/Province:</strong></td>
		<td><asp:textbox id="State" runat="server" maxlength="25" columns="25"></asp:textbox></td>
	</tr>
	<tr>
		<td style="HEIGHT: 27px" vAlign="middle" align="right" width="120"><strong>Zip Code:</strong></td>
		<td><asp:textbox id="Zip" runat="server" maxlength="15" columns="15"></asp:textbox></td>
	</tr>
	<tr>
		<td style="HEIGHT: 27px" vAlign="middle" align="right" width="120"><strong>Country:</strong></td>
		<td><asp:textbox id="Country" runat="server" maxlength="50" columns="50">United States</asp:textbox></td>
	</tr>
</table>
<h2>Other Contact Information</h2>
<table id="Table4" cellSpacing="1" cellPadding="1" width="595" border="0">
	<tr>
		<td style="HEIGHT: 28px" vAlign="middle" align="right" width="120"><strong>Phone:<br>
			</strong><span class="smallText">Country Code/<br>
				Number/Extension</span></td>
		<td><asp:textbox id="Phone1" runat="server" maxlength="3" columns="3" width="30px">001</asp:textbox>&nbsp;<asp:textbox id="Phone2" runat="server" maxlength="30" columns="30"></asp:textbox>&nbsp;<asp:textbox id="Phone3" runat="server" maxlength="5" columns="5" width="40px"></asp:textbox></td>
	</tr>
	<tr>
		<td style="HEIGHT: 28px" vAlign="middle" align="right" width="120"><strong>Fax:<br>
			</strong><span class="smallText">Country Code/<br>
				Number</span></td>
		<td style="HEIGHT: 28px"><asp:textbox id="Fax1" runat="server" maxlength="3" columns="3" width="30px">001</asp:textbox>&nbsp;<asp:textbox id="Fax2" runat="server" maxlength="30" columns="30"></asp:textbox><strong></strong></td>
	</tr>
	<tr>
		<td vAlign="middle" align="right" width="120"><strong>Webpage:</strong></td>
		<td><asp:textbox id="Webpage" runat="server" maxlength="100" columns="50">http://</asp:textbox></td>
	</tr>
</table>
