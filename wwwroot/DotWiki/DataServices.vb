' description:  Data services for DotWiki project.
' author:       Sept/2002 - hector@hectorcorrea.com 
' updated:      

Imports System.Data.SqlClient
Imports System.Data

Public Class DataServices

    Public Shared Function ConnectToDB() As SqlConnection
        Dim Conn As SqlConnection
        Dim ConnString As String

        ConnString = ConfigurationSettings.AppSettings("ConnectionString")
        If ConnString Is Nothing OrElse ConnString.Length = 0 Then
            ConnString = "server=(local);database=dotwiki;trusted_connection=yes"
        End If

        Try
            Conn = New SqlConnection(ConnString)
            Conn.Open()
        Catch ex As Exception
            Conn = Nothing
        End Try

        Return Conn

    End Function

End Class
