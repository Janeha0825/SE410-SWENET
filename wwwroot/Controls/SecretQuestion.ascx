<%@ Register TagPrefix="cc1" Namespace="SwenetDev.Controls" Assembly="Swenet 0.2" %>
<%@ Control Language="c#" CodeBehind="SecretQuestion.ascx.cs" AutoEventWireup="false" Inherits="SwenetDev.Controls.SecretQuestion" %>
<%@ import Namespace="System.Data" %>
<table cellSpacing="1" cellPadding="1" style="WIDTH: 340px; HEIGHT: 46px">
	<tr>
		<td style="WIDTH: 120px; HEIGHT: 17px" width="120" align="right"><strong>Secret Question:</strong></td>
		<td style="HEIGHT: 17px"><asp:dropdownlist id="SecretQuestionDdl" tabIndex="2" width="200px" runat="server"></asp:dropdownlist></td>
	</tr>
	<tr>
		<td style="WIDTH: 120px" width="120" align="right"><strong>Answer:</strong></td>
		<td><asp:textbox id="txtAnswer" tabIndex="3" runat="server" maxlength="25" Width="200px"></asp:textbox><asp:requiredfieldvalidator id="RequiredfieldValidator" runat="server" controltovalidate="txtAnswer">*</asp:requiredfieldvalidator></td>
	</tr>
</table>
