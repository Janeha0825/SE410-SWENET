<%@ Register TagPrefix="cc1" Namespace="SwenetDev.Controls" Assembly="Swenet 0.2" %>
<%@ Control Language="c#" CodeBehind="MaterialsControl.ascx.cs" AutoEventWireup="false" Inherits="SwenetDev.Controls.MaterialsControl" targetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<!-- Insert content here -->
<script language="javascript">
	<!--
	
		function popUp() {
		
			window.open();
			
		}

	-->
</script>
<p>
	<cc1:dataeditcontrol id="MaterialsEditor" runat="server">
		<headertemplate>Materials</headertemplate>
		<itemtemplate>
			<table>
				<tr>
					<td valign="top"><strong>Identifying Info:</strong></td>
					<td>
						<asp:linkbutton id="Linkbutton2" causesvalidation="False" runat="server" cssclass="editLink" commandname="edit" text='<%# DataBinder.Eval(Container.DataItem, "IdentInfo") %>'>
						</asp:linkbutton></td>
				</tr>
				<tr>
					<td><strong>File:</strong></td>
					<td>
						<asp:linkbutton id="Linkbutton3" causesvalidation="False" runat="server" cssclass="editLink" commandname="edit" text='<%# DataBinder.Eval(Container.DataItem, "Link") %>'>
						</asp:linkbutton></td>
				</tr>
			</table>
		</itemtemplate>
		<edititemtemplate>
			<table>
				<tr>
					<td valign="top"><strong>Identifying Info:</strong></td>
					<td>
						<asp:textbox id="InfoTextBox" runat="server" text='<%# SwenetDev.Globals.formatTextOutput( DataBinder.Eval(Container.DataItem, "IdentInfo") ) %>' 
							rows="4" textmode="MultiLine" columns="55" OnLoad="InfoTextBox_Load">
						</asp:textbox>
					</td>
				</tr>
				<tr>
					<td>
						<strong>File:</strong>
					</td>
					<td>
						<asp:RadioButton GroupName="mats" ID="NewRdBtn" Text="Upload New" Checked="True" Runat="server" 
							AutoPostBack="True"	OnCheckedChanged="RdBtn_Changed" OnLoad="NewRdBtn_Load" />
						<asp:RadioButton GroupName="mats" ID="ExistingRdBtn" Text="Use Existing" Checked="False" Runat="server" 
							AutoPostBack="True" OnCheckedChanged="RdBtn_Changed" OnLoad="ExistingRdBtn_Load" />
					</td>
				</tr>
				<tr>
					<td></td>
					<td>
						<asp:Panel ID="InputPanel" Runat="server" OnLoad="InputPanel_Load" Visible="True">
							<input id="FileUpload" style="WIDTH: 100%" type="file" name="FileUpload" runat="server">
						</asp:Panel>
					</td>
				</tr>
				<tr>
					<td></td>
					<td>
						<asp:Panel ID="DDLPanel" Runat="server" OnLoad="DDLPanel_Load" Visible="False">
							<asp:DropDownList ID="MatsDDL" Runat="server" EnableViewState="True" OnLoad="Bind_MatsDDL"/><br>
							<i>NOTE: Only the most recent upload of each file is listed.</i>
						</asp:Panel>
					</td>
				</tr>
				<tr>
					<td><strong>Access:</strong></td>
					<td>
						<asp:DropDownList Runat="server" ID="AccessLevelList">
							<asp:ListItem Value="All" Selected="True" />
							<asp:ListItem Value="Users" />
							<asp:ListItem Value="Faculty" />
						</asp:DropDownList>
					</td>
				</tr>
			</table>
			<br>
			<div style="POSITION: relative; TOP: 3px" align="right">
				<asp:requiredfieldvalidator id="Requiredfieldvalidator3" runat="server" controltovalidate="InfoTextbox" errormessage="Identifying info field may not be blank."
					display="Dynamic"></asp:requiredfieldvalidator>
				<asp:regularexpressionvalidator id="RegExValidator1" runat="server" controltovalidate="InfoTextbox" errormessage='<%# "Entry exceeds maximum length of " + SwenetDev.Controls.MaterialsControl.MaxLength + "."%>' validationexpression='<%# ".{1," + SwenetDev.Controls.MaterialsControl.MaxLength + "}" %>' display="Dynamic">
				</asp:regularexpressionvalidator>&nbsp; 
				<asp:requiredfieldvalidator id="FileValidator" runat="server" controltovalidate="FileUpload" errormessage="File field may not be blank."
					display="Dynamic" OnLoad="FileValidator_Load"/>&nbsp;
				<asp:button id="Button4" runat="server" cssclass="defaultButton" commandname="update" text="Apply"></asp:button>
				<asp:button id="Button5" runat="server" cssclass="defaultButton" commandname="cancel" text="Cancel"
					causesvalidation="False"></asp:button>
		</edititemtemplate>
	</cc1:dataeditcontrol></p>
