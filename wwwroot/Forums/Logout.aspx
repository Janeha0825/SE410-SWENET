<%@ Import Namespace="AspNetForums" %>
<%@ Import Namespace="AspNetForums.Components" %>
<%@ Register TagPrefix="AspNetForums" Namespace="AspNetForums.Controls" Assembly="AspNetForums" %>
<%@ Page Language="C#" %>
<html>
	<head>
		<script language="C#" runat="server">

	public void Page_Load(Object sender, EventArgs e) {
		// log the user out
		FormsAuthentication.SignOut();
		
		// Nuke the roles cookie
		UserRoles.SignOut();
	}
	
		</script>
		<aspnetforums:styleskin runat="server" id="Styleskin2" />
	</head>
	<body leftmargin="0" bottommargin="0" rightmargin="0" topmargin="0" marginheight="0" marginwidth="0">
		<form runat="server" id="Form1">
			<table width="100%" cellspacing="0" cellpadding="0" border="0">
				<tr valign="bottom">
					<td>
						<table width="100%" height="100%" cellspacing="0" cellpadding="0" border="0">
							<tr valign="top">
								<!-- left column -->
								<td>&nbsp; &nbsp; &nbsp;</td>
								<!-- center column -->
								<td id="CenterColumn" width="95%" runat="server" class="CenterColumn">
									&nbsp;
									<br>
									<aspnetforums:navigationmenu id="Navigationmenu1" runat="server" />
									<br>
									<table width="100%">
										<tr>
											<td align="center">
												<table cellspacing="1" cellpadding="0" width="50%" class="tableBorder">
													<tr>
														<th align="left">
															&nbsp;<span class="tableHeaderText">Log Out Complete</span>
														</th>
													</tr>
													<tr>
														<td class="forumRow">
															<table cellpadding="3" cellspacing="0">
																<tr>
																	<td>
																		&nbsp;
																	</td>
																	<td>
																		<span class="normalTextSmall">
                                   You have been logged out.  
                                   <p>
																				To log back in, please visit the <a href="<%=Globals.UrlLogin%>">Login page</a>.
                                   <p>
																				<a href="<%=Globals.UrlHome%>">Return to forum home</a>
                                   </span></P>
																	</td>
																</tr>
															</table>
														</td>
													</tr>
												</table>
											</td>
										</tr>
										<tr>
											<td align="center">
												<br>
												<aspnetforums:jumpdropdownlist runat="server" id="Jumpdropdownlist1" />
											</td>
										</tr>
									</table>
								</td>
								<td class="CenterColumn">&nbsp;&nbsp;&nbsp;</td>
								<!-- right margin -->
								<td class="RightColumn">&nbsp;&nbsp;&nbsp;</td>
								<td id="RightColumn" visible="false" nowrap width="230" runat="server" class="RightColumn">
								</td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
		</form>
	</body>
</html>
