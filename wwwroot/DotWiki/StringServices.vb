Public Class StringServices

    Public Shared Function AddBS(ByRef Path As String) As String
        Dim PathWithBS As String
        If Right(Path, 1) <> "\" Then
            PathWithBS = Path & "\"
        Else
            PathWithBS = Path
        End If
        Return PathWithBS
    End Function

End Class
