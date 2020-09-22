Imports AdamKinney.RSS
Imports System.Configuration.ConfigurationSettings
Imports DotWiki.BusinessServices
Imports DotWiki.Wiki


Public Class BlogTopic
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

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
        BlogTopic()
    End Sub

    Private Sub BlogTopic()

        ' Find out what topic we want to blog  
        Dim TopicName As String
        If Me.Request.QueryString.Item("topic") Is Nothing Then
            TopicName = "HomeWiki"
        Else
            TopicName = Me.Request.QueryString.Item("topic").ToString()
        End If

        ' Get the topic content 
        Dim TopicURL As String = AppSettings.Item("BlogTopicLink") & "?Topic=" + TopicName
        Dim TopicContent As String = ReadTopic(TopicName)
        Dim TopicExpandedContent As String = WikiText(TopicContent)

        ' Define an RSS Feed 
        Dim BlogTitle As String = AppSettings.Item("BlogTitle")
        Dim BlogLink As String = AppSettings.Item("BlogLink")
        Dim BlogDescription As String = AppSettings.Item("BlogDescription")
        Dim rssFeed As New RSSFeed(BlogTitle, BlogLink, BlogDescription)
        rssFeed.ImplementsSlash = True
        rssFeed.Language = AppSettings.Item("BlogLanguage")
        rssFeed.ManagingEditor = AppSettings.Item("BlogEmail")
        rssFeed.WebMaster = AppSettings.Item("BlogEmail")
        rssFeed.TTL = "60"

        ' Add an item for the topic
        Dim item As RSSItem
        item = New RSSItem(TopicName, "<![CDATA[ " + TopicExpandedContent + " ]]>")
        item.Author = AppSettings.Item("BlogAuthor")
        item.Categories.Add(New RSSCategory("DotWiki"))
        item.Comments = TopicURL
        'item.Slash.Comments = ""
        item.Guid = TopicURL
        item.GuidIsPermalink = True
        item.PubDate = Now.ToString("r")
        item.SourceUrl = TopicURL
        rssFeed.Items.Add(item)

        ' Return the content as XML.
        Response.ContentType = "text/xml"
        Response.Write(rssFeed.ToString())
        Response.End()

    End Sub

End Class
