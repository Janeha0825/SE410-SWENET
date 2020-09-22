' description:  Business services for DotWiki project.
' author:       Sept/2002 - hector@hectorcorrea.com 
' updated:      

Imports System.Web
Imports System.Data.SqlClient
Imports DotWiki.DataServices
Imports System.Configuration.ConfigurationSettings

Public Class BusinessServices

    Const Quote As Char = Chr(34)

    Public Shared Function AddPictureToTopic(ByRef TopicName As String, _
        ByRef Picture As String)

        Dim TopicContent As String
        TopicContent = ReadTopic(TopicName)
        TopicContent += vbCrLf + "<img src=" + Picture + " border=1>"

        SaveTopic(TopicName, TopicContent)

    End Function

    Public Shared Function Search(ByVal SearchString As String) As DataSet

        Dim HomePage As String = ConfigurationSettings.AppSettings("HomePage")
        Dim WikiSet As String = ConfigurationSettings.AppSettings("WikiSet")
        Dim SearchMethod As String = ConfigurationSettings.AppSettings("SearchMethod")

        ' If search string is empty, return all topics.
        ' (We might want to prevent this in the future)
        If SearchString Is Nothing Then
            Return SearchAll(HomePage, WikiSet)
        End If

        SearchString = SearchString.Trim()
        If SearchString.Length = 0 Then
            Return SearchAll(HomePage, WikiSet)
        End If

        ' Do the actual search
        Select Case SearchMethod.ToUpper()
            Case "FULLTEXT"
                Return SearchFullText(SearchString, HomePage, WikiSet)
            Case "FULLTEXTWITHRANKS"
                Return SearchFullTextWithRanking(SearchString, HomePage, WikiSet)
            Case Else
                Return SearchNormal(SearchString, HomePage, WikiSet)
        End Select
    End Function

    Public Shared Function SearchFullTextWithRanking(ByVal SearchString As String, ByVal HomePage As String, ByVal WikiSet As String) As DataSet
        ' To do: Actually implement this method.
        ' For now, just use other full text search method.
        Return SearchFullText(SearchString, HomePage, WikiSet)

        ' Here is a sample SQL SELECT that uses FreeTextTable
        'select name, content, rank
        'from topic 
        'inner join freetexttable( topic, *, 'wiki diagram' ) as ft on topic.topicpk = ft.[key]
        'order by rank

    End Function

    Public Shared Function SearchFullText(ByVal SearchString As String, ByVal HomePage As String, ByVal WikiSet As String) As DataSet

        Dim ds As DataSet
        Dim oConn As SqlConnection
        oConn = ConnectToDB()
        If Not IsNothing(oConn) Then
            Try

                ' Build a where clause with the following format:
                '   contains( *, 'FormsOf( inflectional, "word1" ) and FormsOf( inflectional, "word2" )' )
                Dim WhereClause As String
                WhereClause = "contains( *, '"

                Dim Words() As String
                Words = Split(SearchString, " ")
                For i As Integer = 0 To Words.Length - 1
                    Dim SearchTerm As String
                    SearchTerm = "FormsOf( inflectional, " + Quote + Words(i) + Quote + ") "
                    If i < Words.Length - 1 Then
                        SearchTerm += " AND "
                    End If
                    WhereClause += SearchTerm
                Next

                WhereClause += "' )"

                Dim sc As New SqlCommand
                sc.Connection = oConn
                sc.Parameters.Add("@WikiSet", WikiSet)
                sc.Parameters.Add("@HomePageLink", HomePage + "?topic=")
                sc.CommandText = "select name, updatedon, " + _
                                    "@HomePageLink + name as link " + _
                                    "from topic " + _
                                    "where wikiset = @WikiSet and " + _
                                    WhereClause + " " + _
                                    "order by name"

                Dim da As New SqlDataAdapter(sc)
                ds = New DataSet
                da.Fill(ds, "topic")

            Catch ex As Exception
                ' to do: tell the user that something went wrong
                ' (e.g. maybe the user entered only non-searchable words, like "the")
            Finally
                oConn.Close()
            End Try
        End If
        Return ds

    End Function

    Public Shared Function SearchNormal(ByVal SearchString As String, ByVal HomePage As String, ByVal WikiSet As String) As DataSet

        Dim ds As DataSet
        Dim oConn As SqlConnection
        oConn = ConnectToDB()
        If Not IsNothing(oConn) Then
            Try

                Dim sc As New SqlCommand
                sc.Connection = oConn
                sc.Parameters.Add("@WikiSet", WikiSet)
                sc.Parameters.Add("@SearchString", "%" + SearchString + "%")
                sc.Parameters.Add("@HomePageLink", HomePage + "?topic=")
                sc.CommandText = "select name, updatedon, " + _
                                    "@HomePageLink + name as link " + _
                                    "from topic " + _
                                    "where wikiset = @WikiSet and " + _
                                    "( name like @SearchString or content like @SearchString ) " + _
                                    "order by name"

                Dim da As New SqlDataAdapter(sc)
                ds = New DataSet
                da.Fill(ds, "topic")

            Finally
                oConn.Close()
            End Try
        End If
        Return ds

    End Function

    Public Shared Function SearchAll(ByVal HomePage As String, ByVal WikiSet As String) As DataSet
        Dim ds As DataSet
        Dim oConn As SqlConnection
        oConn = ConnectToDB()
        If Not IsNothing(oConn) Then
            Try
                Dim sc As New SqlCommand
                sc.Connection = oConn
                sc.Parameters.Add("@WikiSet", WikiSet)
                sc.Parameters.Add("@HomePageLink", HomePage + "?topic=")
                sc.CommandText = "select name, updatedon, " + _
                                "@HomePageLink + name as link " + _
                                "from topic " + _
                                "where wikiset = @WikiSet " + _
                                "order by name"
                Dim da As New SqlDataAdapter(sc)
                ds = New DataSet
                da.Fill(ds, "topic")
            Finally
                oConn.Close()
            End Try
        End If
        Return ds
    End Function

    Public Shared Function GetIndexDS() As DataSet
        ' Index page currently displays all topics. Eventually
        ' we'll probably need to break down the topics by some
        ' criteria (by letter)
        Dim HomePage As String = ConfigurationSettings.AppSettings("HomePage")
        Dim WikiSet As String = ConfigurationSettings.AppSettings("WikiSet")
        Return SearchAll(HomePage, WikiSet)

    End Function

    Public Shared Function GetRecentChangesDS(ByVal Days As Integer) As DataSet
        Dim ds As DataSet
        Dim oConn As SqlConnection
        oConn = ConnectToDB()
        If Not IsNothing(oConn) Then
            Try
                Dim HomePage As String = ConfigurationSettings.AppSettings("HomePage")
                Dim WikiSet As String = ConfigurationSettings.AppSettings("WikiSet")
                Dim sc As New SqlCommand
                sc.Connection = oConn
                sc.Parameters.Add("@WikiSet", WikiSet)
                sc.Parameters.Add("@Days", Days)
                sc.Parameters.Add("@HomePageLink", HomePage + "?topic=")
                sc.CommandText = _
"select name, updatedon, " + _
"@HomePageLink + name as link " + _
"from topic " + _
"where wikiset = @WikiSet and " + _
"DateDiff(d, updatedon, getdate()) <= @Days " + _
"order by updatedon desc"

                Dim da As New SqlDataAdapter(sc)
                ds = New DataSet
                da.Fill(ds, "topic")
            Finally
                oConn.Close()
            End Try
        End If
        Return ds
    End Function

    Public Shared Function ReadTopicHistory(ByRef PK As String, _
                                            ByRef DateTime As String) As String
        Dim oConn As SqlConnection
        Dim TopicContent As String

        oConn = ConnectToDB()
        If Not IsNothing(oConn) Then
            Try
                Dim sc As New SqlCommand
                sc.Connection = oConn
                sc.Parameters.Add("@PK", PK)
                sc.CommandText = "select content, updatedon from topichistory where topichistorypk = @PK"
                Dim da As New SqlDataAdapter(sc)
                Dim ds As New DataSet
                da.Fill(ds, "topichistory")
                If ds.Tables(0).Rows.Count <> 1 Then
                    Throw New System.Exception("Incorrect number of records found in topichistory")
                End If
                TopicContent = CType(ds.Tables(0).Rows(0).Item("content"), String)
                DateTime = CType(ds.Tables(0).Rows(0).Item("updatedon"), String)
            Finally
                oConn.Close()
            End Try
        Else
            TopicContent = ""
            DateTime = ""
        End If

        Return TopicContent

    End Function

    Public Shared Function ReadTopic(ByRef TopicName As String) As String
        Dim oConn As SqlConnection
        Dim TopicContent As String

        oConn = ConnectToDB()
        If Not IsNothing(oConn) Then
            Try
                Dim WikiSet As String = ConfigurationSettings.AppSettings("WikiSet")
                Dim sc As New SqlCommand
                sc.Connection = oConn
                sc.Parameters.Add("@TopicName", TopicName)
                sc.Parameters.Add("@WikiSet", WikiSet)
                sc.CommandText = "select content from topic " + _
                                 "where name = @TopicName and wikiset = @WikiSet"

                TopicContent = CType(sc.ExecuteScalar(), String)

            Finally
                oConn.Close()
            End Try
        Else
            TopicContent = ""
        End If

        Return TopicContent

    End Function

    Public Shared Function SaveTopic(ByRef TopicName As String, _
                                    ByRef TopicContent As String) As String
        Dim oConn As SqlConnection
        Dim RetVal As String

        If AppSettings.Item("AllowEdit") <> "true" Then
            Return "no changes allowed"
        End If

        oConn = ConnectToDB()
        If Not IsNothing(oConn) Then

            Try
                Dim WikiSet As String = ConfigurationSettings.AppSettings("WikiSet")
                Dim user As String = HttpContext.Current.User.Identity.Name
                Dim sc As New SqlCommand
                sc.Connection = oConn
                sc.Parameters.Add("@TopicName", TopicName)
                sc.Parameters.Add("@WikiSet", WikiSet)
                sc.CommandText = "select content, name, topicpk, updatedon, wikiset, editedby " + _
                                 "from topic " + _
                                 "where name = @TopicName and wikiset = @WikiSet"

                Dim da As New SqlDataAdapter(sc)
                Dim cb As New SqlCommandBuilder(da)
                Dim ds As New DataSet
                da.Fill(ds, "topic")
                If ds.Tables("topic").Rows.Count = 0 Then
                    ' new topic
                    Dim NewRow As DataRow
                    NewRow = ds.Tables("topic").NewRow()
                    ds.Tables("topic").Rows.Add(NewRow)
                    ds.Tables("topic").Rows(0).Item("name") = TopicName
                    ds.Tables("topic").Rows(0).Item("wikiset") = WikiSet
                Else
                    BackupTopic(ds.Tables("topic").Rows(0).Item("topicpk").ToString(), oConn)
                End If
                ds.Tables("topic").Rows(0).Item("content") = TopicContent
                ds.Tables("topic").Rows(0).Item("updatedon") = Date.Now()
                ds.Tables("topic").Rows(0).Item("editedby") = user
                da.Update(ds, "topic")

            Catch e As Exception
                RetVal = e.Message
                Throw e
            Finally
                oConn.Close()
            End Try

        Else
            RetVal = "Could not connect to database"
        End If

        Return RetVal
    End Function

    Public Shared Function BackupTopic(ByRef TopicPK As String, ByRef oConn As SqlConnection) As String
        Dim sc As New SqlCommand
        sc.Parameters.Add("@TopicPK", TopicPK)
        sc.CommandText = "insert into topichistory( " + _
                         "topicfk, name, content, updatedon, wikiset, editedby ) " + _
                         "select topicpk, name, content, updatedon, wikiset, editedby " + _
                         "from topic " + _
                         "where topicpk = @TopicPK"
        sc.Connection = oConn
        sc.ExecuteNonQuery()
    End Function

    Public Shared Function CreateHistoryTable(ByRef TopicName As String) As String

        Dim TableHeader As String
        TableHeader = "<tr><td><b>Topic</b></td>" + _
                    "<td><b>Updated On </b><img border=0 src=images/uparrow.jpg></td>" + _
                    "<td><b>By User</b></td></tr>"

        Dim TopicTable As String
        TopicTable = ""

        Dim oConn As SqlConnection
        oConn = ConnectToDB()
        If Not IsNothing(oConn) Then
            Try

                Dim WikiSet As String = ConfigurationSettings.AppSettings("WikiSet")
                Dim sc As New SqlCommand
                sc.Parameters.Add("@WikiSet", WikiSet)
                sc.Parameters.Add("@Name", TopicName)
                sc.CommandText = "select name, updatedon, topichistorypk, editedby " + _
                                "from topichistory " + _
                                "where name = @Name and wikiset = @WikiSet " + _
                                "order by updatedon desc"
                sc.Connection = oConn
                Dim da As New SqlDataAdapter(sc)
                Dim ds As New DataSet
                da.Fill(ds, "topic")
                If ds.Tables(0).Rows.Count > 0 Then
                    TopicTable += "<table>" + TableHeader
                    Dim dr As DataRow
                    For Each dr In ds.Tables(0).Rows
                        Dim ThisTopic, ThisPK, LastUpdate, User As String
                        ThisTopic = dr.Item("name").ToString().TrimEnd()
                        ThisPK = dr.Item("topichistorypk").ToString()
                        LastUpdate = dr.Item("updatedon").ToString()
                        User = dr.Item("editedby").ToString()
                        TopicTable += "<tr>" + _
                                      "<td>" + _
                                      ThisTopic + _
                                      "</td>" + _
                                      "<td>" + _
                                      "<a href=TopicHistory.aspx" + _
                                      "?topic=" + ThisTopic + "&pk=" + ThisPK + ">" + _
                                      LastUpdate + "</a> " + _
                                      "</td>" + _
                                      "<td>" + _
                                      User + _
                                      "</td>" + _
                                      "</tr>"
                    Next
                    TopicTable += "</table>"
                End If
            Finally
                oConn.Close()
            End Try
        End If

        Return TopicTable

    End Function

End Class
