<%@ Control Language="vb" AutoEventWireup="false" Codebehind="TopicList.ascx.vb" Inherits="DotWiki.TopicList" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<asp:datagrid id="grdTopics" runat="server" GridLines="None" AutoGenerateColumns="False">
	<AlternatingItemStyle CssClass="topicListRowAlternate"></AlternatingItemStyle>
	<ItemStyle CssClass="topicListRow"></ItemStyle>
	<HeaderStyle Font-Bold="True" ForeColor="White" CssClass="topicListHeader"></HeaderStyle>
	<Columns>
		<asp:HyperLinkColumn DataNavigateUrlField="link" DataTextField="name" HeaderText="Topic">
			<HeaderStyle Width="70%"></HeaderStyle>
		</asp:HyperLinkColumn>
		<asp:BoundColumn DataField="updatedon" HeaderText="Updated On">
			<HeaderStyle Width="30%"></HeaderStyle>
			<ItemStyle HorizontalAlign="Right"></ItemStyle>
		</asp:BoundColumn>
	</Columns>
</asp:datagrid>
