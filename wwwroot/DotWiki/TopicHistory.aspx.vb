Imports DotWiki.BusinessServices
Imports DotWiki.Wiki
Imports System.Configuration.ConfigurationSettings


Public Class TopicHistory
    Inherits System.Web.UI.Page
    Protected WithEvents lblPageTopic As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageContent As System.Web.UI.WebControls.Label
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents DotWikiPageHeaderControl1 As DotWikiControls.DotWikiPageHeaderControl
    Protected WithEvents cmdRestore As System.Web.UI.WebControls.Button
    Protected WithEvents txtRestorePassword As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblRestorePassword As System.Web.UI.WebControls.Label
    Protected WithEvents lblDateTime As System.Web.UI.WebControls.Label

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
            If Not Me.Request.QueryString.Item("pk") Is Nothing Then
                Me.ShowOldVersion()
            Else
                Me.ShowTopicHistoryList()
            End If
        End If

    End Sub

    Private Sub ShowOldVersion()

        Dim HistoryPK, DateTime, HomePage As String
        HistoryPK = Me.Request.QueryString.Item("pk")
        Me.lblPageTopic.Text = Me.Request.QueryString.Item("topic").ToString()
        Me.lblPageContent.Text = ReadTopicHistory(HistoryPK, DateTime)
        Me.lblPageContent.Text = WikiText(Me.lblPageContent.Text)
        Me.lblDateTime.Text = "(history as of " + DateTime + ")"

        If (AppSettings.Item("AllowEdit") = "true") Then
            Me.cmdRestore.Visible = True
            Me.lblRestorePassword.Visible = True
            Me.txtRestorePassword.Visible = True
        Else
            Me.cmdRestore.Visible = False
            Me.lblRestorePassword.Visible = False
            Me.txtRestorePassword.Visible = False
        End If

    End Sub

    Private Sub ShowTopicHistoryList()

        Me.cmdRestore.Visible = False
        Me.lblRestorePassword.Visible = False
        Me.txtRestorePassword.Visible = False

        If Not Me.Request.QueryString.Item("topic") Is Nothing Then
            Me.lblPageTopic.Text = Me.Request.QueryString.Item("topic").ToString()
            Me.lblDateTime.Text = "..."
            Me.lblPageContent.Text = "Select what version of this topic you want to review.<br>" + _
                                    CreateHistoryTable(Me.lblPageTopic.Text)
        End If

    End Sub

    Private Sub cmdRestore_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRestore.Click

        If Me.txtRestorePassword.Text = AppSettings.Item("RestorePassword") Then
            Dim HistoryPK, DateTime As String
            HistoryPK = Me.Request.QueryString.Item("pk")
            Me.lblPageContent.Text = ReadTopicHistory(HistoryPK, DateTime)
            SaveTopic(Me.lblPageTopic.Text, Me.lblPageContent.Text)
            Me.Response.Redirect(RootObject.HomePage + "?topic=" & Me.lblPageTopic.Text)
        Else
            Me.Response.Redirect(RootObject.HomePage)
        End If

    End Sub
End Class
