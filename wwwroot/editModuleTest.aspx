<%@ OutputCache Location="None" VaryByParam="None" %>
<%@ Register TagPrefix="uc1" TagName="AuthorsControl" Src="Controls/AuthorsControl.ascx" %>
<%@ Register TagPrefix="uc1" TagName="TopicsControl" Src="Controls/TopicsControl.ascx" %>
<%@ Register TagPrefix="uc1" TagName="PrerequisitesControl" Src="Controls/PrerequisitesControl.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SeeAlsoControl" Src="Controls/SeeAlsoControl.ascx" %>
<%@ Page Language="c#" Debug="true" CodeBehind="editModuleTest.aspx.cs" AutoEventWireup="false" Inherits="SwenetDev.editModuleTest" validateRequest="false" smartNavigation="True"%>
<%@ import Namespace="System.Data" %>
<%@ Register TagPrefix="n0" Namespace="System.Web.UI" Assembly="System.Web, Version=1.0.3300.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" %>
<%@ Register TagPrefix="swenet" TagName="sidebar" Src="Controls\Sidebar.ascx" %>
<%@ Register TagPrefix="swenet" TagName="header" Src="Controls\Header.ascx" %>
<%@ Register TagPrefix="swenet" TagName="materials" Src="Controls\MaterialsControlTest.ascx" %>
<%@ Register TagPrefix="swenet" TagName="authors" Src="Controls\authorsControl.ascx" %>
<%@ Register TagPrefix="swenet" TagName="CategoriesControl" Src="Controls\CategoriesControl.ascx" %>
<%@ Register TagPrefix="swenet" TagName="PrerequisitesControl" Src="Controls\PrerequisitesControl.ascx" %>
<%@ Register TagPrefix="swenet" TagName="TopicsControl" Src="Controls\TopicsControl.ascx" %>
<%@ Register TagPrefix="swenet" TagName="ResourcesControl" Src="Controls\ResourcesControl.ascx" %>
<%@ Register TagPrefix="swenet" TagName="ObjectivesControl" Src="Controls\ObjectivesControl.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Upload Module</title>
		<SCRIPT language="javascript">
			<!--
			function openPopup(controlID) {
				var url = 'popupHelp.aspx?id=' + controlID;
				window.open(url,'popupHelpWindow','height=400,width=400,scrollbars=yes');	
			}
			function openTutorial() {
				var url = 'moduleSubmissionTutorial.aspx';
				window.open(url,'tutorialWindow','width=800,resizable=yes,scrollbars=yes');
			}
			-->
		</SCRIPT>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="style.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body>
		<form id="Form1" action="uploadResult.aspx" runat="server">
			<swenet:header id="Header" runat="server"></swenet:header>
			<table cellspacing="1" cellpadding="1" width="750" id="Table1">
				<tr>
					<td valign="top" width="150"><swenet:sidebar id="Sidebar" runat="server"></swenet:sidebar></td>
					<td width="600">
						<h1>Upload Module (Step
							<asp:label id="StepLbl" runat="server">1</asp:label>&nbsp;of 5) <FONT size="1">[<A href="javascript:openTutorial()">Tutorial</A>]</FONT>
						</h1>
						<p><asp:button id="BackBtnTop" runat="server" text="< Back" cssclass="navButton" causesvalidation="False"
								commandname="Back"></asp:button><asp:button id="NextBtnTop" runat="server" text="Next >" cssclass="navButton" causesvalidation="False"
								commandname="Next"></asp:button><asp:button id="SaveBtnTop" runat="server" text="Save for Later" cssclass="navButton" causesvalidation="False"></asp:button><asp:button id="SubmitBtnTop" onclick="SubmitBtn_Click" runat="server" text="Submit" cssclass="navButton"></asp:button></p>
						<asp:label id="ErrorMessage" runat="server" forecolor="Red"></asp:label><asp:panel id="Panel0" runat="server">
							<P>
								<asp:validationsummary id="ValidationSummary1" runat="server" headertext="The following fields are required:"></asp:validationsummary>All 
								fields are required except where noted.&nbsp; To add a new entry to a table, 
								click&nbsp;<STRONG>New</STRONG>, and then <STRONG>Apply</STRONG> to commit or <STRONG>
									Cancel</STRONG> to cancel that entry.&nbsp; To edit an entry, click on its 
								hyperlinked text.</P>
							<asp:panel id="VariantPanel" runat="server">
								<P>To create a variant of a module, select the module from the list below and click <STRONG>
										Set Variant</STRONG>.&nbsp; This will copy the content of the original 
									module to the variant so you can make changes and submit it as a variant of 
									that module.</P>
								<P>
									<asp:dropdownlist id="ModulesDdl" runat="server"></asp:dropdownlist>
									<asp:button id="VariantBtn" runat="server" causesvalidation="False" cssclass="defaultButton"
										text="Set Variant"></asp:button></P>
							</asp:panel>
							<H2>General Information</H2>
							<TABLE id="GeneralTable" cellSpacing="0" cellPadding="0" width="596">
								<TR>
									<TD style="HEIGHT: 51px"><STRONG>Title</STRONG><BR>
										<asp:textbox id="Title" runat="server" width="580px" columns="74" maxlength="300"></asp:textbox>
										<asp:requiredfieldvalidator id="TitleValidator" runat="server" display="Dynamic" errormessage="Title" controltovalidate="Title">*</asp:requiredfieldvalidator></TD>
								</TR>
								<TR>
									<TD><STRONG>Date</STRONG><BR>
										<asp:label id="DateLbl" runat="server"></asp:label><BR>
										<BR>
									</TD>
								</TR>
								<TR>
									<TD><STRONG>Size</STRONG> <STRONG><A href="javascript:openPopup(1)"><IMG src="images/help.jpg" border="0"></A><BR>
										</STRONG>
										<TABLE id="SizeTable" style="WIDTH: 488px; HEIGHT: 82px" cellSpacing="1" cellPadding="1"
											width="488" border="0">
											<TR>
												<TD style="WIDTH: 9px; HEIGHT: 19px"></TD>
												<TD style="WIDTH: 63px; HEIGHT: 19px"><STRONG>Lecture:</STRONG></TD>
												<TD style="WIDTH: 173px; HEIGHT: 18px">
													<asp:textbox id="Lecture" runat="server" columns="20" maxlength="50"></asp:textbox></TD>
												<TD style="HEIGHT: 18px"><STRONG>Homework:</STRONG></TD>
												<TD style="HEIGHT: 18px">
													<asp:textbox id="Homework" runat="server" columns="20" maxlength="50"></asp:textbox></TD>
											</TR>
											<TR>
												<TD style="WIDTH: 9px"></TD>
												<TD style="WIDTH: 63px"><STRONG>Lab:</STRONG></TD>
												<TD style="WIDTH: 173px">
													<asp:textbox id="Lab" runat="server" columns="20" maxlength="50"></asp:textbox></TD>
												<TD><STRONG>Other:</STRONG></TD>
												<TD>
													<asp:textbox id="Other" runat="server" columns="20" maxlength="50"></asp:textbox></TD>
											</TR>
											<TR>
												<TD style="WIDTH: 9px; HEIGHT: 26px"></TD>
												<TD style="WIDTH: 63px; HEIGHT: 26px"><STRONG>Exercise:</STRONG></TD>
												<TD style="WIDTH: 173px; HEIGHT: 26px">
													<asp:textbox id="Exercise" runat="server" columns="20" maxlength="50"></asp:textbox></TD>
												<TD style="HEIGHT: 26px"></TD>
												<TD style="HEIGHT: 26px"></TD>
											</TR>
										</TABLE>
									</TD>
								</TR>
								<TR>
									<TD><STRONG>Abstract <A href="javascript:openPopup(2)"><IMG src="images/help.jpg" border="0"></A><BR>
										</STRONG>
										<asp:textbox id="Abstract" runat="server" width="580px" columns="74" maxlength="400" textmode="MultiLine"
											rows="5"></asp:textbox>
										<asp:requiredfieldvalidator id="AbstractValidator" runat="server" display="Dynamic" errormessage="Abstract"
											controltovalidate="Abstract">*</asp:requiredfieldvalidator></TD>
								</TR>
							</TABLE>
							<H2>SEEK Categories <FONT size="2"><A href="javascript:openPopup(3)"><IMG src="images/help.jpg" border="0"></A></FONT></H2>
							<swenet:categoriescontrol id="CategoriesControl1" runat="server"></swenet:categoriescontrol>
						</asp:panel><asp:panel id="Panel1" runat="server">
							<H2>Author Information <FONT size="2"><A href="javascript:openPopup(4)"><IMG src="images/help.jpg" border="0"></A></FONT></H2>
							<uc1:authorscontrol id="AuthorsControl1" runat="server"></uc1:authorscontrol>
							<P><STRONG>Author Comments</STRONG> (Optional) <STRONG><FONT size="2"><A href="javascript:openPopup(5)">
											<IMG src="images/help.jpg" border="0"></A><BR>
									</FONT></STRONG>
								<asp:textbox id="Comments" runat="server" width="580px" columns="74" maxlength="400" textmode="MultiLine"
									rows="5"></asp:textbox></P>
						</asp:panel><asp:panel id="Panel2" runat="server">
							<H2>Prerequisite Knowledge <FONT size="2"><A href="javascript:openPopup(6)"><IMG src="images/help.jpg" border="0"></A></FONT></H2>
							<uc1:prerequisitescontrol id="PrerequisitesControl1" runat="server"></uc1:prerequisitescontrol>
							<H2>Learning Objectives <FONT size="2"><A href="javascript:openPopup(7)"><IMG src="images/help.jpg" border="0"></A></FONT></H2>
							<swenet:objectivescontrol id="ObjectivesControl1" runat="server"></swenet:objectivescontrol>
							<H2>Topics <FONT size="2"><A href="javascript:openPopup(8)"><IMG src="images/help.jpg" border="0"></A></FONT></H2>
							<uc1:topicscontrol id="TopicsControl1" runat="server"></uc1:topicscontrol>
						</asp:panel><asp:panel id="Panel3" runat="server">
							<H2>Materials <FONT size="2"><A href="javascript:openPopup(9)"><IMG src="images/help.jpg" border="0"></A></FONT></H2>
							<P>
								<asp:label id="ZipLabel" runat="server" font-italic="True">Note: It is not necessary to provide a zip file for your materials.&nbsp;&nbsp;One will be<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;generated automatically.&nbsp;&nbsp;Also, uploaded files must be 4 MB or smaller.</asp:label></P>
							<P>
								<swenet:materials id="MaterialsControl1" runat="server"></swenet:materials></P>
							<H2>Other Resources <FONT size="2"><A href="javascript:openPopup(10)"><IMG src="images/help.jpg" border="0"></A></FONT></H2>
							<P><EM>(Optional)</EM></P>
							<P>
								<swenet:resourcescontrol id="ResourcesControl1" runat="server"></swenet:resourcescontrol></P>
						</asp:panel><asp:panel id="Panel4" runat="server">
							<H2>See Also... <FONT size="2"><A href="javascript:openPopup(11)"><IMG src="images/help.jpg" border="0"></A></FONT></H2>
							<P><EM>(Optional)</EM></P>
							<P>
								<uc1:SeeAlsoControl id="SeeAlsoControl1" runat="server"></uc1:SeeAlsoControl></P>
							<H2>Check In Comments <FONT size="2"><A href="javascript:openPopup(12)"><IMG src="images/help.jpg" border="0"></A></FONT></H2>
							<P>
								<asp:textbox id="CheckInTxt" runat="server" width="580px" columns="74" maxlength="400" textmode="MultiLine"
									rows="5"></asp:textbox>
								<asp:customvalidator id="CheckInValidator" runat="server" errormessage="Check In Comments">*</asp:customvalidator></P>
						</asp:panel>
						<p><asp:button id="BackBtn" runat="server" text="< Back" cssclass="navButton" causesvalidation="False"
								commandname="Back"></asp:button><asp:button id="NextBtn" runat="server" text="Next >" cssclass="navButton" causesvalidation="False"
								commandname="Next"></asp:button><asp:button id="SaveBtn" runat="server" text="Save for Later" cssclass="navButton" causesvalidation="False"></asp:button><asp:button id="SubmitBtn" onclick="SubmitBtn_Click" runat="server" text="Submit" cssclass="navButton"></asp:button></p>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
