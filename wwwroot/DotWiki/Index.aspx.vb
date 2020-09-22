' description: Creates an index of topic.
'              Needs to be rewriten if the number of topics is very large,
'              but for now this is OK.

Imports DotWiki.BusinessServices

Public Class Index
    Inherits System.Web.UI.Page
    Protected WithEvents DotWikiPageHeaderControl1 As DotWikiControls.DotWikiPageHeaderControl
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

        Dim ds As DataSet
        ds = GetIndexDS()

        Dim c As DotWiki.TopicList
        c = Me.LoadControl("TopicList.ascx")
        c.grdTopics.DataSource = ds
        c.grdTopics.DataMember = "topic"
        c.grdTopics.DataBind()
        Me.Controls.Add(c)

    End Sub

End Class
