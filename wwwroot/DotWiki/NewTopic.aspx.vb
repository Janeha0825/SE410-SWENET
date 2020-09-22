Public Class NewTopic
    Inherits System.Web.UI.Page
    Protected WithEvents txtTopicName As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmdAddIt As System.Web.UI.WebControls.Button
    Protected WithEvents lblMessage As System.Web.UI.WebControls.Label
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

        ' This causes the page to associate Enter key with cmdAddIt button click event.
        ' (source: Universal Thread Message ID: 814613, http://www.universalthread.com)
        Page.RegisterHiddenField("__EVENTTARGET", "cmdAddIt")

    End Sub

    Private Sub cmdAddIt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAddIt.Click
        Me.AddTopic()
    End Sub

    Private Sub AddTopic()
        If (HttpContext.Current.User.Identity.IsAuthenticated) Then
            Dim TopicName As String = Me.txtTopicName.Text
            Select Case TopicName.Length
                Case Is = 0
                    Me.lblMessage.Text = "Please indicate the name of the topic to add."
                Case Is > 50
                    Me.lblMessage.Text = "Topic Name cannot be longer than 50 characters."
                Case Else
                    If Wiki.IsCamelCaseWord(TopicName) Then
                        Dim URL As String = RootObject.HomePage + "?topic=" + TopicName + "&mode=Edit"
                        Me.Response.Redirect(URL)
                    Else
                        Me.lblMessage.Text = "Cannot add topic Name [" + TopicName + "] " + _
                                            "since it is not in CamelCase notation."
                    End If
            End Select
        Else
            Response.Redirect("~/../login.aspx?ReturnURL=DotWiki/NewTopic.aspx")
        End If
    End Sub

End Class
