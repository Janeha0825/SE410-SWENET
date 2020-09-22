<%@ Page CodeBehind="FileUpload.aspx.vb" Language="vb" AutoEventWireup="false" Inherits="DotWiki.FileUpload" %>
<%@ Register TagPrefix="cc1" Namespace="DotWikiControls" Assembly="DotWikiControls" %>
<HTML>
	<HEAD>
		<title>Upload Picture</title>
		<script language="VB" runat="server">

       Sub DoUpload(Sender As Object, e As System.EventArgs)
          Me.LoadPicture()		
       End Sub

		</script>
		<LINK REL="StyleSheet" HREF="Styles.css" TYPE="text/css" MEDIA="screen">
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</HEAD>
	<body>
		<form enctype="multipart/form-data" runat="server">
			<FONT face="+1"><B>
					<P>
						<cc1:DotWikiPageHeaderControl id="DotWikiPageHeaderControl1" runat="server" DESIGNTIMEDRAGDROP="27"></cc1:DotWikiPageHeaderControl></P>
					<P>
						<TABLE id="Table1" style="WIDTH: 816px; HEIGHT: 108px" cellSpacing="1" cellPadding="1"
							width="816" border="0">
							<TR>
								<TD style="WIDTH: 167px">Upload file to topic</TD>
								<TD>
									<asp:Label id="lblTopicName" runat="server" Width="424px">(topicname)</asp:Label></TD>
								<TD></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 167px">File to upload</TD>
								<TD>
									<input id="txtUpload" type="file" runat="server" style="WIDTH: 349px; HEIGHT: 22px" size="39"
										tabIndex="1">&nbsp;
									<asp:label id="lblMaxSize" runat="server" Font-Size="Smaller" Font-Italic="True"></asp:label></TD>
								<TD></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 167px">Upload password</TD>
								<TD>
									<asp:TextBox id="txtPassword" runat="server" TextMode="Password" tabIndex="2"></asp:TextBox></TD>
								<TD></TD>
							</TR>
						</TABLE>
					</P>
					<P>
					&nbsp;</B></FONT>
			<asp:button id="btnUpload" Text="Upload File" OnClick="DoUpload" runat="server" CssClass="mainButton"
				tabIndex="3" /></P>
			<hr noshade>
			<P>
			</P>
			<P>
				<asp:label id="lblResults" Visible="false" runat="server" /></P>
		</form>
	</body>
</HTML>
