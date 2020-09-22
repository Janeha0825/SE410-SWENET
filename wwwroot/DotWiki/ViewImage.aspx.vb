Public Class ViewImage
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblContent As System.Web.UI.WebControls.Label
    Protected WithEvents lblPictureFile As System.Web.UI.WebControls.Label
    Protected WithEvents lblPictureZoom As System.Web.UI.WebControls.Label
    Protected WithEvents cmdOriginal As System.Web.UI.WebControls.Button
    Protected WithEvents cmdFitHorizontal As System.Web.UI.WebControls.Button
    Protected WithEvents cmdFitVertical As System.Web.UI.WebControls.Button

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If Not IsPostBack() Then
            If Me.Request.QueryString.Item("PictureFile") Is Nothing Then
                Me.lblPictureFile.Text = ""
            Else
                Me.lblPictureFile.Text = Me.Request.QueryString.Item("PictureFile")

            End If
            Me.lblPictureZoom.Text = "ORIGINAL"
        End If

        Me.RenderImage()

    End Sub

    

    Private Sub RenderImage()
        If Me.lblPictureFile.Text.Length = 0 Then
            Me.lblContent.Text = "(no image file available)"
        Else
            Select Case Me.lblPictureZoom.Text
                Case Is = "FIT-HORIZONTAL"
                    Me.lblContent.Text = "<img src=" + Me.lblPictureFile.Text + " width=90%"
                Case Is = "FIT-VERTICAL"
                    Me.lblContent.Text = "<img src=" + Me.lblPictureFile.Text + " height=90%"
                Case Else
                    Me.lblContent.Text = "<img src=" + Me.lblPictureFile.Text + ">"
            End Select
        End If
    End Sub

    Private Sub cmdOriginal_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOriginal.Click
        Me.lblPictureZoom.Text = "ORIGINAL"
        Me.RenderImage()
    End Sub

    Private Sub cmdFitHorizontal_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdFitHorizontal.Click
        Me.lblPictureZoom.Text = "FIT-HORIZONTAL"
        Me.RenderImage()
    End Sub

    Private Sub cmdFitVertical_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdFitVertical.Click
        Me.lblPictureZoom.Text = "FIT-VERTICAL"
        Me.RenderImage()
    End Sub
End Class
