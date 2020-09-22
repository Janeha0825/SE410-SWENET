<%@ Control Language="c#" AutoEventWireup="false" Codebehind="ViewUserInfoControl.ascx.cs" Inherits="SwenetDev.Controls.ViewUserInfoControl" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>

<script language="JScript">
function generateEmail( acct, domain )  {
   document.write("<a href=\"mail" + "to:" + acct + "@" + domain + "\">") ;
   document.write( acct + "@" + domain );
   document.write("</a>")
}
</script>

<table id="Table1" class="borderedTable" width="400" cellspacing="0" cellpadding="5" rules="all"
	border="1" align="center">
	<tr>
		<td width="110" class="lightGrayHeader"><strong class="lightGrayHeader">Username:</strong></td>
		<td width="290">
			<asp:label id="UsernameLbl" runat="server"></asp:label></td>
	</tr>
	<tr>
		<td width="110" class="lightGrayHeader"><strong>Name:</strong></td>
		<td width="290">
			<asp:label id="NameLbl" runat="server"></asp:label></td>
	</tr>
	<tr>
		<td width="110" class="lightGrayHeader"><strong>Email Address:</strong></td>
		<td width="290">
			<asp:Label id="EmailLbl" runat="server"></asp:Label></td>
	</tr>
	<tr>
		<td width="110" class="lightGrayHeader"><strong>Title:</strong></td>
		<td width="290">
			<asp:label id="TitleLbl" runat="server"></asp:label></td>
	</tr>
	<tr>
		<td width="110" class="lightGrayHeader"><strong>Affiliation:</strong></td>
		<td width="290">
			<asp:label id="AffiliationLbl" runat="server"></asp:label></td>
	</tr>
	<tr>
		<td width="110" class="lightGrayHeader"><strong> Address:</strong></td>
		<td width="290">
			<asp:label id="Address1Lbl" runat="server"></asp:label>
			<asp:label id="Address2Lbl" runat="server"></asp:label>
			<asp:label id="CityLbl" runat="server"></asp:label>
			<asp:label id="StateLbl" runat="server"></asp:label>
			<asp:label id="ZipLbl" runat="server"></asp:label><br>
			<asp:label id="CountryLbl" runat="server"></asp:label></td>
	</tr>
	<tr>
		<td width="110" class="lightGrayHeader"><strong>Phone:</strong></td>
		<td width="290">
			<asp:label id="PhoneCountryLbl" runat="server"></asp:label>&nbsp;
			<asp:label id="PhoneLbl" runat="server"></asp:label>&nbsp;
			<asp:label id="ExtensionLbl" runat="server" font-bold="True">Ext:</asp:label>
			<asp:label id="PhoneExtensionLbl" runat="server"></asp:label></td>
	</tr>
	<tr>
		<td width="110" class="lightGrayHeader"><strong>Fax:</strong></td>
		<td width="290">
			<asp:label id="FaxCountryLbl" runat="server"></asp:label>&nbsp;
			<asp:label id="FaxLbl" runat="server"></asp:label></td>
	</tr>
	<tr>
		<td width="110" class="lightGrayHeader"><strong>Webpage:</strong></td>
		<td width="290">
			<asp:hyperlink id="WebpageLnk" runat="server" target="_blank"></asp:hyperlink></td>
	</tr>
</table>
