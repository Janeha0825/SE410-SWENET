Imports AdamKinney.RSS
Imports System.Configuration.ConfigurationSettings
Imports DotWiki.BusinessServices
Imports DotWiki.Wiki

Public Class BlogRecentChanges
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
        Me.BlogRecentChanges()
    End Sub

    Private Sub BlogRecentChanges()

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

        Dim item As RSSItem


        Dim ds As DataSet
        ds = GetRecentChangesDS(1)
        If ds.Tables(0).Rows.Count = 0 Then
            ' nothing to blog
        Else

            ' Add each topic to the RSS Feed 
            Dim r As DataRow
            For Each r In ds.Tables(0).Rows

                Dim TopicName As String = r.Item("Name")
                Dim TopicURL As String = AppSettings.Item("BlogTopicLink") & "?Topic=" + TopicName
                Dim TopicContent As String = ReadTopic(TopicName)
                Dim TopicExpandedContent As String = WikiText(TopicContent)
                Dim TopicUpdatedOn As Date = r.Item("UpdatedOn")

                item = New RSSItem(TopicName, "<![CDATA[ " + TopicExpandedContent + " ]]>")
                item.Author = AppSettings.Item("BlogAuthor")
                item.Categories.Add(New RSSCategory("DotWiki"))
                item.Comments = TopicURL
                'item.Slash.Comments = ""
                item.Guid = TopicURL
                item.GuidIsPermalink = True
                item.PubDate = TopicUpdatedOn.ToString("r")
                item.SourceUrl = TopicURL
                rssFeed.Items.Add(item)
            Next

        End If

        ' Return the content as XML.
        Response.ContentType = "text/xml"
        Response.Write(rssFeed.ToString())
        Response.End()

    End Sub

End Class
