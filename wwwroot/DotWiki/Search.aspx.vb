' description: Allows users to search for topics. 
'              Needs to be rewriten but for now this is OK.

Imports DotWiki.BusinessServices

Public Class SearchPage
    Inherits System.Web.UI.Page
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents txtTextToSearch As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.Button
    Protected WithEvents DotWikiPageHeaderControl1 As DotWikiControls.DotWikiPageHeaderControl
    Protected WithEvents lblPageContent As System.Web.UI.WebControls.Label

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

        Page.RegisterHiddenField("__EVENTTARGET", "cmdSearch")

        If Not Me.IsPostBack() Then
            If Not Me.Request.QueryString.Item("SearchString") Is Nothing Then
                Me.txtTextToSearch.Text = Me.Request.QueryString.Item("SearchString").ToString
                Me.SearchTopics()
            End If
        End If

    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        Me.SearchTopics()
    End Sub

    Private Sub SearchTopics()

        Dim ds As DataSet
        ds = Search(Me.txtTextToSearch.Text)

        If ds.Tables("topic").Rows.Count = 0 Then
            Me.lblPageContent.Text = "No topics were found"
        Else
            Me.lblPageContent.Text = ""
            Dim c As DotWiki.TopicList
            c = Me.LoadControl("TopicList.ascx")
            c.grdTopics.DataSource = ds
            c.grdTopics.DataMember = "topic"
            c.grdTopics.DataBind()
            Me.Controls.Add(c)
        End If

    End Sub

End Class
