
Imports System.ComponentModel
Imports System.Web.UI

<DefaultProperty("Text"), ToolboxData("<{0}:DotWikiPageHeaderControl runat=server></{0}:DotWikiPageHeaderControl>")> _
Public Class DotWikiPageHeaderControl
    Inherits System.Web.UI.WebControls.WebControl

    Const BackgroundColor As String = "#94AFC0"
    Dim _text As String
    Public EnableLinks As Boolean = True
    Public SelectedPage As String = "Default.aspx"

    <Bindable(True), Category("Appearance"), DefaultValue("")> Property [Text]() As String
        Get
            Return _text
        End Get

        Set(ByVal Value As String)
            _text = Value
        End Set
    End Property

    Protected Overrides Sub Render(ByVal o As System.Web.UI.HtmlTextWriter)

        o.AddStyleAttribute("margin-left", "0")
        o.AddStyleAttribute("text-align", "left")
        o.AddAttribute("cellSpacing", "0")
        o.AddAttribute("cellPadding", "0")
        o.AddAttribute("WIDTH", "750px")
        o.RenderBeginTag("table")
        o.RenderBeginTag("tr")

        AddLogo(o, "../images/title4.jpg")

        o.RenderEndTag()    'tr
        o.RenderEndTag()    'table

        o.AddAttribute("class", "mainToolbar")
        'o.AddAttribute("border", "1")
        o.AddAttribute("cellSpacing", "1")
        o.AddAttribute("cellPadding", "1")
        o.AddStyleAttribute("WIDTH", "750px")
        'o.AddStyleAttribute("HEIGHT", "20px")
        o.RenderBeginTag("table")
        o.RenderBeginTag("tr")

        'AddLogo(o, "logo.jpg")
        AddOption(o, "SWEnet Modules", "../default.aspx", 18, "")
        AddOption(o, "Wiki Home", "Default.aspx", 12, "")
        AddOption(o, "Index", "Index.aspx", 8, "")
        AddOption(o, "Recent Changes", "RecentChanges.aspx", 17, "")
        AddOption(o, "Search", "Search.aspx", 8, "")
        AddOption(o, "New Topic", "NewTopic.aspx", 12, "")

        o.RenderEndTag()    'tr
        o.RenderEndTag()    ' table
        
    End Sub

    Private Sub AddLogo(ByVal o As System.Web.UI.HtmlTextWriter, ByVal LogoFile As String)
        o.AddAttribute("bgColor", Me.BackgroundColor)
        o.AddAttribute("align", "center")
        o.RenderBeginTag("td")
        o.AddAttribute("class", "header")
        o.AddAttribute("src", LogoFile)
        o.RenderBeginTag("img")
        o.RenderEndTag() ' img
        o.RenderEndTag() 'td
    End Sub

    Private Sub AddOption(ByVal o As System.Web.UI.HtmlTextWriter, ByVal Name As String, ByVal Link As String, ByVal Width As Integer, ByVal PictureFile As String)

        If Name.IndexOf("SWEnet") <> -1 Then
        Else
            'o.AddAttribute("bgColor", Me.BackgroundColor)
            o.AddAttribute("width", "5%")
            o.RenderBeginTag("td")

            If PictureFile.Equals("") Then
                o.Write(" | ")
            Else
                If Me.EnableLinks Then
                    o.AddAttribute("href", Link)
                End If

                o.RenderBeginTag("a")

                o.AddAttribute("src", PictureFile)
                o.AddAttribute("border", "0")
                o.RenderBeginTag("img")
                o.RenderEndTag() ' img
                o.RenderEndTag() ' a
            End If

            o.RenderEndTag() 'td
        End If

        'o.AddAttribute("bgColor", Me.BackgroundColor)
        o.AddAttribute("width", Width.ToString + "%")
        o.RenderBeginTag("td")
        If Me.EnableLinks Then
            o.AddAttribute("href", Link)
            o.AddAttribute("class", "navigation")
        End If

        o.RenderBeginTag("a")

        o.Write(Name)

        o.RenderEndTag() ' a
        o.RenderEndTag() 'td
    End Sub

End Class
