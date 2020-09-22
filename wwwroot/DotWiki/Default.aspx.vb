' description:  Web page where wiki web pages are rendered.
' author:       Sept/2002 - hector@hectorcorrea.com 
'
Imports DotWiki.BusinessServices
Imports DotWiki.Wiki
Imports System.Configuration.ConfigurationSettings

Public Class WikiTopicPage
    Inherits System.Web.UI.Page
    Protected WithEvents lblPageContent As System.Web.UI.WebControls.Label
    Protected WithEvents cmdEdit As System.Web.UI.WebControls.Button
    Protected WithEvents txtPageContent As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmdSave As System.Web.UI.WebControls.Button
    Protected WithEvents cmdHistory As System.Web.UI.WebControls.Button
    Protected WithEvents txtViewHistory As System.Web.UI.WebControls.HyperLink
    Protected WithEvents chkPageInEditMode As System.Web.UI.WebControls.CheckBox
    Protected WithEvents lblPageTopic As System.Web.UI.WebControls.Label
    Protected WithEvents cmdSaveAndContinue As System.Web.UI.WebControls.Button
    Protected WithEvents txtAddPicture As System.Web.UI.WebControls.HyperLink
    Protected WithEvents cmdXml As System.Web.UI.WebControls.ImageButton
    Protected WithEvents PageHeader As DotWikiControls.DotWikiPageHeaderControl
    Protected WithEvents PageFooter As DotWikiControls.DotWikiPageHeaderControl
    Protected WithEvents lblTopicNameOnHeader As System.Web.UI.WebControls.Label
    Protected WithEvents lblOtherOptions As System.Web.UI.WebControls.Label
    Protected WithEvents cmdTopicSearch As System.Web.UI.WebControls.ImageButton
    Protected WithEvents lblLeftMenu As System.Web.UI.WebControls.Label
    Protected WithEvents tdLeftColumn As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents tdRightColumn As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents cmdCancel As System.Web.UI.WebControls.Button

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

#Region "Form Events"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If Not Me.IsPostBack() Then

            If Me.Request.QueryString.Item("topic") Is Nothing Then
                Me.ViewState("TopicName") = RootObject.HomeTopic
            Else
                Me.ViewState("TopicName") = Me.Request.QueryString.Item("topic").ToString()
                If Not IsCamelCaseWord(Me.ViewState("TopicName")) Then
                    ' Somebody tried to get to a topic that is not a camel case word
                    ' (most likely somebody trying to sneak in an invalid topic name.)
                    Response.Redirect(RootObject.HomePage + "?Topic=" + RootObject.HomeTopic)
                End If
            End If

            If Not Me.Request.QueryString.Item("mode") Is Nothing Then
                If Me.Request.QueryString.Item("mode").ToUpper() = "EDIT" Then
                    Me.chkPageInEditMode.Checked = True
                End If
            End If

            Me.DisplayTopic()

        End If
    End Sub

    Private Sub cmdEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdEdit.Click
        Me.EditPage()
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Me.SaveChanges()
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Me.CancelChanges()
    End Sub

    Private Sub cmdSaveAndContinue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSaveAndContinue.Click
        Me.SaveAndContinue()
    End Sub

    Private Sub txtViewHistory_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtViewHistory.PreRender
        txtViewHistory.NavigateUrl = "TopicHistory.aspx?topic=" + Me.lblPageTopic.Text
    End Sub

    Private Sub txtAddPicture_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtViewHistory.PreRender
        txtAddPicture.NavigateUrl = "FileUpload.aspx?topic=" + Me.lblPageTopic.Text
    End Sub

#End Region

    Private Sub DisplayTopic()

        Me.lblPageTopic.Text = Me.ViewState("TopicName")
        Me.lblTopicNameOnHeader.Text = Me.ViewState("TopicName")

        If Me.chkPageInEditMode.Checked Then

            Me.PageHeader.EnableLinks = False

            Me.txtPageContent.Visible = True
            Me.lblPageContent.Visible = False
            Me.lblPageContent.Text = ""
            Me.cmdEdit.Visible = False
            Me.lblOtherOptions.Visible = False
            Me.txtViewHistory.Visible = False
            Me.txtAddPicture.Visible = False
            Me.cmdXml.Visible = False

            If AppSettings.Item("AllowEdit") = "true" Then
                Me.cmdSave.Visible = True
                Me.cmdSaveAndContinue.Visible = True
                Me.cmdCancel.Visible = True
            Else
                Me.cmdSave.Visible = False
                Me.cmdSaveAndContinue.Visible = False
                Me.cmdCancel.Visible = False
            End If

            Me.lblLeftMenu.Text = WikiText(ReadTopic("LeftMenu"))
            Me.txtPageContent.Text = ReadTopic(Me.ViewState("TopicName"))
            Me.lblPageContent.Text = ""

        Else

            Me.PageHeader.EnableLinks = True

            Me.txtPageContent.Visible = False
            Me.txtPageContent.Text = ""
            Me.lblPageContent.Visible = True
            Me.cmdSave.Visible = False
            Me.cmdSaveAndContinue.Visible = False
            Me.cmdCancel.Visible = False
            Me.txtViewHistory.Visible = True
            Me.lblOtherOptions.Visible = True

            If AppSettings.Item("AllowEdit") = "true" Then
                Me.cmdEdit.Visible = True
                Me.txtAddPicture.Visible = True
            Else
                Me.cmdEdit.Visible = False
                Me.txtAddPicture.Visible = False
            End If

            Dim Content As String = ReadTopic(Me.ViewState("TopicName"))
            Me.lblLeftMenu.Text = WikiText(ReadTopic("LeftMenu"))
            Me.lblPageContent.Text = WikiText(Content)
            Me.txtPageContent.Text = ""
            Me.cmdXml.Visible = (AppSettings.Item("BlogEnabled") = "true")

        End If

        Me.PageHeader.Visible = (AppSettings.Item("ShowHeader") = "true")
        Me.lblTopicNameOnHeader.Visible = (AppSettings.Item("ShowTopicNameOnHeader"))
        Me.PageFooter.Visible = (AppSettings.Item("ShowFooter") = "true")

    End Sub

    Private Sub CancelChanges()
        Me.chkPageInEditMode.Checked = False
        Me.DisplayTopic()
    End Sub

    Private Sub SaveChanges()
        Me.chkPageInEditMode.Checked = False
        SaveTopic(Me.lblPageTopic.Text, Me.txtPageContent.Text)
        Me.DisplayTopic()
    End Sub

    Private Sub SaveAndContinue()
        SaveTopic(Me.lblPageTopic.Text, Me.txtPageContent.Text)
        Me.DisplayTopic()
    End Sub

    Private Sub EditPage()
        If (HttpContext.Current.User.Identity.IsAuthenticated) Then
            Me.chkPageInEditMode.Checked = True
            Me.DisplayTopic()
        Else
            Response.Redirect("~/../login.aspx?ReturnURL=DotWiki/Default.aspx", True)
        End If
    End Sub

    Private Sub cmdXml_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdXml.Click
        Response.Redirect("BlogTopic.aspx?Topic=" + Me.lblPageTopic.Text)
    End Sub

    Private Sub cmdTopicSearch_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdTopicSearch.Click
        Response.Redirect("Search.aspx?SearchString=" + Me.ViewState("TopicName"))
    End Sub
End Class
