Imports DotWiki.BusinessServices
Imports System.Configuration.ConfigurationSettings

Public Class RecentChanges
    Inherits System.Web.UI.Page
    Protected WithEvents cmdLast24Hrs As System.Web.UI.WebControls.Button
    Protected WithEvents cmdLast7Days As System.Web.UI.WebControls.Button
    Protected WithEvents cmdLastMonth As System.Web.UI.WebControls.Button
    Protected WithEvents lblMessage As System.Web.UI.WebControls.Label
    Protected WithEvents DotWikiPageHeaderControl1 As DotWikiControls.DotWikiPageHeaderControl
    Protected WithEvents cmdXml As System.Web.UI.WebControls.ImageButton
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not Me.IsPostBack() Then
            Me.GetRecentChanges(1)
        End If

        If AppSettings.Item("BlogEnabled") = "true" Then
            Me.cmdXml.Visible = True
        Else
            Me.cmdXml.Visible = False
        End If

    End Sub

    Private Sub GetRecentChanges(ByRef Days As Integer)

        Dim ds As DataSet
        ds = GetRecentChangesDS(Days)
        If ds.Tables(0).Rows.Count = 0 Then
            If Days = 1 Then
                Me.lblMessage.Text = "No topics have been changed since yesterday"
            Else
                Me.lblMessage.Text = "No topics have been changed in the last " + Days.ToString() + " days."
            End If
        Else
            Me.lblMessage.Text = ""
            Dim c As DotWiki.TopicListByDate
            c = Me.LoadControl("TopicListByDate.ascx")
            c.grdTopics.DataSource = ds
            c.grdTopics.DataMember = "topic"
            c.grdTopics.DataBind()
            Me.Controls.Add(c)
        End If

    End Sub

    Private Sub cmdLast24Hrs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdLast24Hrs.Click
        Me.GetRecentChanges(1)
    End Sub

    Private Sub cmdLast7Days_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdLast7Days.Click
        Me.GetRecentChanges(7)
    End Sub

    Private Sub cmdLastMonth_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdLastMonth.Click
        Me.GetRecentChanges(31)
    End Sub

    Private Sub cmdXml_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdXml.Click
        Response.Redirect("BlogRecentChanges.aspx")
    End Sub
End Class
