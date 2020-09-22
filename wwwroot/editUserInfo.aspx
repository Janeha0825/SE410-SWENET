<%@ Register TagPrefix="uc1" TagName="Header" Src="Controls/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Sidebar" Src="Controls/Sidebar.ascx" %>
<%@ Page language="c#" Codebehind="editUserInfo.aspx.cs" AutoEventWireup="false" Inherits="SwenetDev.editUserInfo" %>
<%@ Register TagPrefix="uc1" TagName="EditUserInfoControl" Src="Controls/EditUserInfoControl.ascx" %>
<%@ OutputCache Location="None" VaryByParam="None" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>Edit User Info</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="style.css" type="text/css" rel="stylesheet">
	</head>
	<body ms_positioning="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<uc1:header id="Header1" runat="server"></uc1:header>
			<table id="BodyTable" width="750">
				<tr>
					<td valign="top" width="150"><uc1:sidebar id="Sidebar1" runat="server"></uc1:sidebar></td>
					<td width="597" valign="top">
						<h1>Edit User Information</h1>
						<p><asp:label id="ErrorMessage" runat="server" forecolor="Red"></asp:label></p>
						<p><uc1:edituserinfocontrol id="EditUserInfoControl1" runat="server"></uc1:edituserinfocontrol>
							<table id="Table1" cellspacing="0" cellpadding="1" width="589" border="0">
								<tr>
									<td style="WIDTH: 111px" valign="middle" align="right"></td>
									<td style="WIDTH: 487px">&nbsp;
										<asp:button id="Submit" runat="server" text="Submit" cssclass="defaultButton"></asp:button>&nbsp;
										<asp:button id="CancelBtn" runat="server" text="Cancel" cssclass="defaultButton"></asp:button></td>
								</tr>
							</table>
						</p>
					</td>
				</tr>
			</table>
		</form>
	</body>
</html>
