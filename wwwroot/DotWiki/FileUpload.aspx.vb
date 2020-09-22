Imports DotWiki.StringServices
Imports DotWiki.BusinessServices
Imports System.Configuration.ConfigurationSettings

Public Class FileUpload
    Inherits System.Web.UI.Page
    Protected WithEvents btnUpload As System.Web.UI.WebControls.Button
    Protected WithEvents lblResults As System.Web.UI.WebControls.Label
    Protected WithEvents lblMaxSize As System.Web.UI.WebControls.Label
    Protected WithEvents txtPassword As System.Web.UI.WebControls.TextBox
    Protected WithEvents DotWikiPageHeaderControl1 As DotWikiControls.DotWikiPageHeaderControl
    Protected WithEvents lblTopicName As System.Web.UI.WebControls.Label
    Protected WithEvents txtUpload As System.Web.UI.HtmlControls.HtmlInputFile

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

    Public Sub LoadPicture()

        Dim FileName As String
        Dim FileExtension As String
        Dim FullPath As String
        Dim BasePath As String
        Dim PicturesPath As String
        Dim FileNamePieces() As String
        Dim MaxSizeInKB As Integer
        Dim EnteredPassword As String
        Dim UploadPassword As String

        Try

            UploadPassword = AppSettings.Item("UploadPassword")
            If UploadPassword Is Nothing OrElse UploadPassword.Trim.Length = 0 Then
                Throw New Exception("Invalid Password")
            End If

            EnteredPassword = Me.txtPassword.Text.Trim
            If EnteredPassword <> UploadPassword Then
                Throw New Exception("Invalid Password")
            End If

            FileName = txtUpload.PostedFile.FileName

            FileNamePieces = Split(FileName, ".")
            If FileNamePieces.Length <> 2 Then
                Throw New Exception("Invalid file name")
            End If

            FileExtension = FileNamePieces(1)
            FileExtension = FileExtension.ToUpper()
            If FileExtension <> "JPG" And FileExtension <> "GIF" Then
                Throw New Exception("Invalid file extension. File extension must be JPG or GIF.")
            End If

            MaxSizeInKB = CType(AppSettings("UpLoadMaxSize"), Integer)
            If txtUpload.PostedFile.ContentLength > (MaxSizeInKB * 1024) Then
                Throw New Exception("File is too big")
            End If

            FileNamePieces = Split(FileName, "\")
            FileName = FileNamePieces(UBound(FileNamePieces))
            FileName = FileName.Replace(" ", "_")

            BasePath = Server.MapPath(".")
            PicturesPath = AppSettings("UpLoadPath")
            FullPath = AddBS(BasePath) & AddBS(PicturesPath) & FileName

            'Internet Anonymous User (ANONYMOUS LOGON under XP) must have 
            ' Write (or Full Control) permissions.
            txtUpload.PostedFile.SaveAs(FullPath)
            lblResults.Text = "Upload of File [" & FileName & "] to folder [" & FullPath & "] succeeded"

            Dim TopicName As String
            TopicName = Request.QueryString.Item("topic")
            If Not TopicName Is Nothing Then
                AddPictureToTopic(TopicName, PicturesPath & FileName)
                Dim GoBackToTopicURL As String
                GoBackToTopicURL = RootObject.HomePage + "?topic=" + TopicName
                Response.Redirect(GoBackToTopicURL)
            End If

        Catch Ex As Exception
            lblResults.Text = "Upload of file [" & FileName & "] to folder [" & FullPath & "] failed for the following reason: " & Ex.Message
        Finally
            lblResults.Font.Bold = True
            lblResults.Visible = True
        End Try
    End Sub

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not IsPostBack() Then
            Dim MaxSizeInKB As Integer
            MaxSizeInKB = CType(AppSettings("UpLoadMaxSize"), Integer)
            Me.lblMaxSize.Text = "(Max. " & MaxSizeInKB & "KB)"
            Me.lblTopicName.Text = Request.QueryString.Item("topic")
        End If
    End Sub
End Class
