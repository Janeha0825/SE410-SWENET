' description: Creates an index of topic.
'              Needs to be rewriten but for now this is OK.

Imports DotWiki.BusinessServices

Public Class Index
    Inherits System.Web.UI.Page
    Protected WithEvents cmdAll As System.Web.UI.WebControls.Button
    Protected WithEvents cmdB As System.Web.UI.WebControls.Button
    Protected WithEvents cmdC As System.Web.UI.WebControls.Button
    Protected WithEvents cmdD As System.Web.UI.WebControls.Button
    Protected WithEvents cmdE As System.Web.UI.WebControls.Button
    Protected WithEvents cmdF As System.Web.UI.WebControls.Button
    Protected WithEvents cmdG As System.Web.UI.WebControls.Button
    Protected WithEvents cmdH As System.Web.UI.WebControls.Button
    Protected WithEvents cmdI As System.Web.UI.WebControls.Button
    Protected WithEvents cmdJ As System.Web.UI.WebControls.Button
    Protected WithEvents cmdA As System.Web.UI.WebControls.Button
    Protected WithEvents TextBox1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmdK As System.Web.UI.WebControls.Button
    Protected WithEvents cmdL As System.Web.UI.WebControls.Button
    Protected WithEvents cmdM As System.Web.UI.WebControls.Button
    Protected WithEvents cmdN As System.Web.UI.WebControls.Button
    Protected WithEvents cmdO As System.Web.UI.WebControls.Button
    Protected WithEvents cmdP As System.Web.UI.WebControls.Button
    Protected WithEvents cmdQ As System.Web.UI.WebControls.Button
    Protected WithEvents cmdR As System.Web.UI.WebControls.Button
    Protected WithEvents cmdS As System.Web.UI.WebControls.Button
    Protected WithEvents cmdT As System.Web.UI.WebControls.Button
    Protected WithEvents cmdU As System.Web.UI.WebControls.Button
    Protected WithEvents cmdV As System.Web.UI.WebControls.Button
    Protected WithEvents cmdW As System.Web.UI.WebControls.Button
    Protected WithEvents cmdX As System.Web.UI.WebControls.Button
    Protected WithEvents cmdY As System.Web.UI.WebControls.Button
    Protected WithEvents cmdZ As System.Web.UI.WebControls.Button
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
        Me.lblPageContent.Text = ReadIndex()
    End Sub

    
End Class
