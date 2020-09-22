<%@ Control Language="vb" AutoEventWireup="false" Codebehind="TopicListByDate.ascx.vb" Inherits="DotWiki.TopicListByDate" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<asp:datagrid id="grdTopics" runat="server" GridLines="None" AutoGenerateColumns="False">
	<AlternatingItemStyle CssClass="topicListRowAlternate"></AlternatingItemStyle>
	<ItemStyle CssClass="topicListRow"></ItemStyle>
	<HeaderStyle Font-Bold="True" ForeColor="White" CssClass="topicListHeader"></HeaderStyle>
	<Columns>
		<asp:BoundColumn DataField="name" HeaderText="Topic">
			<HeaderStyle Width="70%"></HeaderStyle>
		</asp:BoundColumn>
		<asp:HyperLinkColumn DataNavigateUrlField="link" DataTextField="updatedon" HeaderText="Update On">
			<HeaderStyle Width="30%"></HeaderStyle>
			<ItemStyle HorizontalAlign="Right"></ItemStyle>
		</asp:HyperLinkColumn>
	</Columns>
</asp:datagrid>
