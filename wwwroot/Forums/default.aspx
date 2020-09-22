<%@ Import Namespace="AspNetForums.Components" %>
<%@ Register TagPrefix="AspNetForumsSearch" Namespace="AspNetForums.Controls.Search" Assembly="AspNetForums" %>
<%@ Register TagPrefix="AspNetForums" Namespace="AspNetForums.Controls" Assembly="AspNetForums" %>
<html>
	<head>
		<aspnetforums:styleskin runat="server" id="Styleskin1" />
		<meta http-equiv="Refresh" content="300">
	</head>
	<body leftmargin="0" bottommargin="0" rightmargin="0" topmargin="0" marginheight="0" marginwidth="0">
		<form runat="server">
			<table width="100%" cellspacing="0" cellpadding="0" border="0">
				<tr valign="bottom">
					<td>
						<table width="100%" height="100%" cellspacing="0" cellpadding="0" border="0">
							<tr valign="top">
								<!-- left column -->
								<td class="LeftColumn">&nbsp;&nbsp;&nbsp;</td>
								<td id="LeftColumn" visible="true" nowrap width="180" runat="server" class="LeftColumn">
									<p>
										&nbsp;
										<br>
										<aspnetforumssearch:searchredirect id="SearchRedirect" runat="server" />
										<br>
										<aspnetforums:sitestats runat="server" id="Sitestats1" />
										<br>
										<aspnetforums:whoisonline runat="server" id="Whoisonline1" /></p>
								</td>
								<td class="LeftColumn">&nbsp;&nbsp;&nbsp;</td>
								<!-- center column -->
								<td class="CenterColumn">&nbsp;&nbsp;&nbsp;</td>
								<td id="CenterColumn" width="95%" runat="server" class="CenterColumn">
									<br>
									<aspnetforums:navigationmenu id="NavigationMenu2" runat="server" />
									<br>
									<table cellpadding="0" cellspacing="2" width="100%">
										<tr>
											<td align="left">
												<aspnetforums:currenttime runat="server" id="CurrentTime1" />
											</td>
											<td align="right">
												<%--                        <AspNetForums:JumpDropDownList runat="server" /> --%>
											</td>
										</tr>
									</table>
									<aspnetforums:forumgrouprepeater runat="server" id="ForumGroupRepeater1"></aspnetforums:forumgrouprepeater>
									<p></p>
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
