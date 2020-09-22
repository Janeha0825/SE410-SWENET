<%@ Page Language="c#" CodeBehind="search.aspx.cs" AutoEventWireup="false" Inherits="SwenetDev.search" %>
<%@ Register TagPrefix="uc1" TagName="CategoriesSelect" Src="Controls/CategoriesSelect.ascx" %>
<%@ Register TagPrefix="swenet" TagName="sidebar" Src="Controls\Sidebar.ascx" %>
<%@ Register TagPrefix="swenet" TagName="header" Src="Controls\Header.ascx" %>
<html>
	<head>
		<title>Search</title>
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="style.css" type="text/css" rel="stylesheet">
	</head>
	<body>
		<form runat="server">
			<swenet:header id="Header" runat="server"></swenet:header>
			<table id="Table1" width="750" cellspacing="1" cellpadding="1">
				<tr>
					<td valign="top" width="150">
						<div id="categories">
							<swenet:sidebar id="Sidebar" runat="server"></swenet:sidebar>
						</div>
					</td>
					<td valign="top" width="600">
						<h1>Search
						</h1>
						<p>
							Search by SEEK Area and, optionally, by SEEK Unit.
						</p>
						<p>
							<uc1:categoriesselect id="CategoriesSelect1" runat="server"></uc1:categoriesselect>
						</p>
						<p><input type="button" value="Search" id="Button1" name="Button1" runat="server" class="defaultButton">
						</p>
					</td>
				</tr>
			</table>
		</form>
	</body>
</html>
