<%@ Register TagPrefix="cc1" Namespace="DotWikiControls" Assembly="DotWikiControls" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Default.aspx.vb" Inherits="DotWiki.WikiTopicPage" trace="false" smartNavigation="False"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>DotWiki</title>
		<LINK media="screen" href="Styles.css" type="text/css" rel="StyleSheet">
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script language="javascript">
		/* Pop up a new window to show a picture 
		   Source: JavaScript Bible by Danny Goodman, chapter 16. */
		var NewWindow
		function viewpicture( PictureFile ) {
			if (NewWindow && !NewWindow.closed) {
				NewWindow.close()
			}
			NewWindow = window.open("ViewImage.aspx?PictureFile=" + PictureFile, "_blank","TOP=100,LEFT=100,HEIGHT=400,WIDTH=600,resizable=yes,scrollbars=yes")
		}
		</script>
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<cc1:dotwikipageheadercontrol id="PageHeader" runat="server" Visible="false" Width="112px"></cc1:dotwikipageheadercontrol>
			<table cellSpacing="0" cellPadding="0" width="750">
				<tr>
					<td class="maintable_leftcolumn" id="tdLeftColumn" runat="server"><asp:label id="lblLeftMenu" runat="server" Font-Size="Small" EnableViewState="False"></asp:label></td>
					<td class="maintable_rightcolumn" id="tdRightColumn" width="85%" runat="server"><asp:label id="lblTopicNameOnHeader" runat="server" CssClass="topicOnHeader">DotWiki</asp:label>&nbsp;
						<asp:imagebutton id="cmdTopicSearch" runat="server" ToolTip="Search for topics that reference this one"
							ImageUrl="magnifier.gif"></asp:imagebutton><br>
						<asp:label id="lblPageContent" runat="server" Font-Size="Small" Height="5px" EnableViewState="False"></asp:label><br>
						<asp:textbox id="txtPageContent" runat="server" EnableViewState="False" Columns="72" Rows="30"
							TextMode="MultiLine"></asp:textbox><br>
					</td>
				</tr>
				<tr>
					<td class="maintable_leftcolumn">&nbsp;
					</td>
					<td class="maintable_rightcolumn"><asp:button id="cmdEdit" runat="server" CssClass="mainButton" Height="21px" Text="Edit"></asp:button><asp:button id="cmdSave" runat="server" CssClass="mainButton" Height="21px" Text="Save"></asp:button><asp:button id="cmdCancel" runat="server" CssClass="mainButton" Height="21px" Text="Cancel"></asp:button><asp:label id="lblPageTopic" runat="server" Font-Size="Medium">DotWiki</asp:label><asp:button id="cmdSaveAndContinue" runat="server" CssClass="mainButton" Height="21px" Text="Save and Continue"></asp:button><br>
					</td>
				</tr>
			</table>
			<cc1:dotwikipageheadercontrol id="PageFooter" runat="server" Visible="False" Width="112px"></cc1:dotwikipageheadercontrol>
			<hr>
			<font size="-2">
				<asp:label id="lblOtherOptions" runat="server" Font-Size="XX-Small">Other Options</asp:label></font><br>
			<table border="0">
				<TBODY>
					<tr>
						<td></td>
						<td><asp:hyperlink id="txtAddPicture" runat="server" Font-Size="XX-Small">Add Picture To Topic</asp:hyperlink><br>
							<asp:hyperlink id="txtViewHistory" runat="server" Font-Size="XX-Small">View Topic History</asp:hyperlink><br>
							<asp:imagebutton id="cmdXml" runat="server" ToolTip="View XML version of this topic" ImageUrl="xml.gif"></asp:imagebutton></td>
					</tr>
				</TBODY>
			</table>
			<P><asp:checkbox id="chkPageInEditMode" runat="server" Visible="False"></asp:checkbox></P>
		</form>
		</TR></TBODY></TABLE></FORM>
	</body>
</HTML>
