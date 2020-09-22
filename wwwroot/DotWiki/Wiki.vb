' description:  Wiki Engine class for DotWiki project. This
'               is the class that parses wiki web pages.
' author:       Sept/2002 - hector@hectorcorrea.com
'
Imports Microsoft.VisualBasic
Imports System.Text.RegularExpressions

Public Class Wiki

    ' This class uses regular expressions to perform most of the text
    ' parsing. Here are some links to regular expressions information:
    '
    '   http://www.fawcette.com/vsm/2003_01/magazine/features/balena/default.aspx 
    '   http://sitescooper.org/tao_regexps.html

    '
    ' Regular expression to detect e-mail addresses:
    ' \w+                   One or more word characters (a-z, 0-9)
    ' [@]                   One @
    ' [^\s]+                One or more non-space characters 
    Const EmailRegEx As String = "\w+[@][^\s]+"

    ' Regular expression to detect HTTP links:
    ' (?<=[^\=|\""]\s*)     Ignore links that are prefixed with = or " and spaces
    ' http://[^\s]+         http:// followed by one or more non-space characters
    Const HttpRegEx As String = "(?<=[^\=|\""]\s*)http://[^\s]+"

    ' Regular expression to detect words in CamelCase notation:
    ' [A-Z]                 Begin with an upper case character 
    ' \w*                   Zero or more word characters
    ' [a-z]                 A lower case character
    ' \w*                   Zero or more word characters
    ' [A-Z]                 An upper case character
    ' \w*                   Zero or more word characters
    '(?=\b)                 Ends with a word boundary
    '(?![\\]|[/]|[\.]\w)    Cannot include a "\" or "/" (like in paths), 
    '                       or a "." followed by characters (like file 
    '                       extensions.)
    Const CamelCaseRegEx As String = "[A-Z]\w*[a-z]\w*[A-Z]\w*(?=\b)(?![\\]|[/]|[\.]\w)"

    Const CamelCaseOptions As RegexOptions = RegexOptions.Compiled Or RegexOptions.Multiline

    'Wiki Syntax

    'Regular expressions to detect wiki formatting:
    Const H3RegEx As String = "\={4,4}.*?\={4,4}"
    Const H2RegEx As String = "\={3,3}.*?\={3,3}"
    Const H1RegEx As String = "\={2,2}.*?\={2,2}"
    Const BoldItalicRegEx As String = "\'{4,4}.*?\'{4,4}"
    Const BoldRegEx As String = "\'{3,3}.*?\'(?!\'){3,3}"
    Const ItalicRegEx As String = "\'{2,2}.*?\'{2,2}"
    'Make one regex for every line using backreferences to distinguish
    'the existence of '*' or '#' syntax to indicate list items.
    Const ListRegEx As String = "^(?:<.+?>)*([#|*]*)(.*?)$"  '

    Private Shared listIndent As Integer = 0
    Private Shared ordered As Boolean = False


    ' Parses a string and returns its Wiki equivalent.
    Public Shared Function WikiText(ByRef RawText As String) As String

        ' Use our predefined CamelCase regular expression.
        Return WikiText(CamelCaseRegEx, RawText)

    End Function

    ' Parses a string and returns its Wiki equivalent.
    Private Shared Function WikiText( _
                            ByRef CamelCaseRegExp As String, _
                            ByRef RawText As String) As String

        Dim ParsedText As String

        ' Hyperlink HTTP references 
        ParsedText = Regex.Replace(" " + RawText, HttpRegEx, AddressOf EvaluateHttpReference, CamelCaseOptions Or RegexOptions.IgnoreCase)

        ' Hyperlink e-mail addresses
        ParsedText = Regex.Replace(ParsedText, EmailRegEx, AddressOf EvaluateEmailAddress)

        ' Hyperlink CamelCase words.
        ParsedText = Regex.Replace(ParsedText, CamelCaseRegEx, AddressOf EvaluateCamelCaseWord, CamelCaseOptions)

        ' Format Wiki Syntax
        ParsedText = Regex.Replace(ParsedText, H3RegEx, AddressOf EvaluateH3)
        ParsedText = Regex.Replace(ParsedText, H2RegEx, AddressOf EvaluateH2)
        ParsedText = Regex.Replace(ParsedText, H1RegEx, AddressOf EvaluateH1)
        ParsedText = Regex.Replace(ParsedText, BoldItalicRegEx, AddressOf EvaluateBoldItalics)
        ParsedText = Regex.Replace(ParsedText, BoldRegEx, AddressOf EvaluateBold)
        ParsedText = Regex.Replace(ParsedText, ItalicRegEx, AddressOf EvaluateItalics)

        'Use RegexOptions.Singleline so '.' matches newline characters
        'Use RegexOptions.Multiline so '^' and '$' match the start and end of a line
        ParsedText = Regex.Replace(ParsedText, ListRegEx, AddressOf EvaluateList, RegexOptions.Multiline)

        ' Process tables
        ' (             find groups of rows
        '   (               find groups of cells
        '     [^\|^\r|^\n]*       0 or more characters that are not a pipe (nor \r nor \n)
        '     \|                  followed by a pipe
        '   )+              
        '   \r\n            \r\n after the cells marks the end of the row
        ' )+                
        ParsedText = Regex.Replace(ParsedText, "(([^\|^\r|^\n]*\|)+\r\n)+", AddressOf EvaluateTables)

        ' Replace carriage returns with <br> when they are...   
        ParsedText = Regex.Replace(ParsedText, "\b\r\n", "<br>", CamelCaseOptions)          ' ...at the begining of the line
        ParsedText = Regex.Replace(ParsedText, "([^\>])\r\n", "$1<br>", CamelCaseOptions)   ' ...not prefixed with a > (because we don't want to add it after most HTML closing tags)
        ParsedText = Regex.Replace(ParsedText, "([biua]\>)\r\n", "$1<br>", CamelCaseOptions) ' ...prefixed by b> i> u> a> (because we do want it after these HTML tags)
        ParsedText = Regex.Replace(ParsedText, "\<br\>\r\n", "<br><br>", CamelCaseOptions)  ' ...prefixed by <br> (and this other one)

        ' Render IMG SRC tags as pop up windows
        ' Source: <img src=folder\file.jpg> 
        ' Target: <a href=folder\file.jpg target=other><img src=folder\file.jpg></a>
        '   \<img src=  starts with <img src=
        '   .+?\>       include any character until a > is found
        ParsedText = Regex.Replace(ParsedText, "\<img src=.+?\>", AddressOf EvaluatePopUpImage)

        ParsedText = Regex.Replace(ParsedText, "\<thumbnail=.+?\>", AddressOf EvaluateThumbnail)

        Return ParsedText

    End Function

    ' To do: 
    '   * Add code to detect whether the topic already exist or not.
    '     Existing topics should be rendered as hyperlinks while non-existing
    '     topics should be rendered with a '?' at the end.
    Public Shared Function EvaluateCamelCaseWord(ByVal m As Match) As String
        Return "<a href=" + RootObject.HomePage + "?topic=" + m.Value + ">" + m.Value + "</a>"
    End Function

    ' Notice that I am forcing the link to lower case to prevent 
    ' them from being considered as CamelCase words should they come
    ' in CamelCase notation. I might need to change this later on.
    Public Shared Function EvaluateHttpReference(ByVal m As Match) As String
        Return "<a href=" + Chr(34) + m.Value + Chr(34) + _
            " target=" + Chr(34) + "other" + Chr(34) + ">" + _
            m.Value + "</a>"
    End Function

    ' Notice that I am forcing the link to lower case to prevent 
    ' them from being considered as CamelCase words should they come
    ' in CamelCase notation. I might need to change this later on.
    '
    ' Also notice that we HTML-encode e-mail addresses to prevent
    ' spammers from picking them up from our web site.
    Public Shared Function EvaluateEmailAddress(ByVal m As Match) As String
        Dim EmailAddress As String = m.Value.ToLower()
        Dim MailTo As String = "mailto:" + EmailAddress
        Return "<a href=" + Chr(34) + HtmlEncoded(MailTo) + Chr(34) + ">" + HtmlEncoded(EmailAddress) + "</a>"
    End Function

    ' Parses text that reprents tables like this:
    '   text|text|...|
    '   text|text|...|
    ' and returns its HTML equivalent.
    Public Shared Function EvaluateTables(ByVal m As Match) As String

        Dim HtmlTable As String

        Try
            HtmlTable = "<table class=""regularTable"">"

            Dim Lines As String() = Split(m.Value.Replace(Chr(10), ""), Chr(13))
            For i As Integer = 0 To Lines.Length - 2
                If Lines(i).Trim.Length > 0 Then
                    HtmlTable += "<tr class=""regularTableRow"">"
                    Dim Columns As String() = Split(Lines(i), "|")
                    For j As Integer = 0 To Columns.Length - 2
                        If i = 0 Then
                            HtmlTable += "<th class=""regularTableHeader"">" + Columns(j) + "</th>"
                        Else
                            HtmlTable += "<td class=""regularTableCell"">" + Columns(j) + "</td>"
                        End If
                    Next
                    HtmlTable += "</tr>"
                End If

            Next

            HtmlTable += "</table>"

        Catch ex As Exception
            HtmlTable = m.Value

        End Try

        Return HtmlTable

    End Function

    ' Parses HTML IMG SRC tags and surrounds them with an anchor tag.
    ' If m.value is equal to <img src=folder\file.ext>
    ' Result is equal to <a href=...><img src=folder\file.ext></a>
    Public Shared Function EvaluatePopUpImage(ByVal m As Match) As String

        Dim NewImgTag As String

        Try
            ' Extract image file name from HTML IMG tag.
            Dim m2 As Match = Regex.Match(m.Value, "=[\w\-\.\\//]*(?=\b)")
            Dim PictureFile As String = m2.Value.Substring(1)
            PictureFile = PictureFile.Replace("\", "/")

            ' Define HREF to pop up the image
            Dim ViewPicture As String
            ViewPicture = "javascript:viewpicture(" + Chr(39) + PictureFile + Chr(39) + ")"

            ' Define the new text to render
            NewImgTag = "<a href=" + Chr(34) + ViewPicture + Chr(34) + ">" + m.Value + "</a>"

        Catch ex As Exception
            ' Something went wrong...don't do any translation.
            NewImgTag = m.Value

        End Try

        Return NewImgTag

    End Function

    ' Parses "thumbnail" tags and surrounds them with an anchor tag.
    ' If m.value is equal to <img src=folder\file.ext>
    ' Result is equal to <a href=...><img src=folder\file.ext></a>
    Public Shared Function EvaluateThumbnail(ByVal m As Match) As String

        Dim NewTag As String

        Try

            Dim m2 As Match = Regex.Match(m.Value, "=[\w\.\\//]*(?=\b)")
            Dim PictureFile As String = m2.Value.Substring(1)
            PictureFile = PictureFile.Replace("\", "/")

            Dim ThumbnailPicture As String = PictureFile.Replace(".", "_thumb.")
            Dim ViewPicture As String = "javascript:viewpicture(" + Chr(39) + PictureFile + Chr(39) + ")"
            Dim ImgTag As String = "<img src=" + ThumbnailPicture + " border=1>"

            NewTag = "<a href=" + Chr(34) + ViewPicture + Chr(34) + ">" + ImgTag + "</a>"

        Catch ex As Exception
            ' Something went wrong...don't do any translation.
            NewTag = m.Value

        End Try

        Return NewTag

    End Function

    ' Returns an HTML-encoded version of the OriginalText.
    ' For example "abc" becomes "&#65;&#66;&#67;"
    Public Shared Function HtmlEncoded(ByVal OriginalText As String)
        Dim EncodedText As String = ""
        For i As Integer = 0 To OriginalText.Length - 1
            Dim ThisChar As Char
            ThisChar = OriginalText.Chars(i)
            If (ThisChar >= "a" And ThisChar <= "z") Or (ThisChar >= "A" And ThisChar <= "Z") Then
                EncodedText += "&#" & Asc(ThisChar) & ";"
            ElseIf ThisChar = "." Then
                EncodedText += "&#46;"
            ElseIf ThisChar = "@" Then
                EncodedText += "&#64;"
            ElseIf ThisChar = ":" Then
                EncodedText += "&#58;"
            Else
                EncodedText += ThisChar
            End If
        Next
        Return EncodedText
    End Function

    Public Shared Function IsCamelCaseWord(ByRef Word As String)
        Dim re As New Regex(CamelCaseRegEx)
        Return (re.Matches(Word).Count = 1)
    End Function

#Region "Wiki Syntax Formatting"

    Public Shared Function EvaluateH3(ByVal m As Match) As String
        Dim mVal As String = m.Value
        Dim value As String = mVal.Substring(4, mVal.Length - 8)
        Return "<H3>" + value + "</H3>"
    End Function

    Public Shared Function EvaluateH2(ByVal m As Match) As String
        Dim mVal As String = m.Value
        Dim value As String = mVal.Substring(3, mVal.Length - 6)
        Return "<H2>" + value + "</H2>"
    End Function

    Public Shared Function EvaluateH1(ByVal m As Match) As String
        Dim mVal As String = m.Value
        Dim value As String = mVal.Substring(2, mVal.Length - 4)
        Return "<H1>" + value + "</H1>"
    End Function

    Public Shared Function EvaluateBoldItalics(ByVal m As Match) As String
        Dim mVal As String = m.Value
        Dim value As String = mVal.Substring(4, mVal.Length - 8)
        Return "<i><b>" + value + "</b></i>"
    End Function

    Public Shared Function EvaluateBold(ByVal m As Match) As String
        Dim mVal As String = m.Value
        Dim value As String = mVal.Substring(3, mVal.Length - 6)
        Return "<b>" + value + "</b>"
    End Function

    Public Shared Function EvaluateItalics(ByVal m As Match) As String
        Dim mVal As String = m.Value
        Dim value As String = mVal.Substring(2, mVal.Length - 4)
        Return "<i>" + value + "</i>"
    End Function

    Public Shared Function EvaluateList(ByVal m As Match) As String
        Dim value As String
        Dim inc As Integer
        Dim cTag As String

        If (ordered) Then
            cTag = "</OL>"
        Else
            cTag = "</UL>"
        End If

        If (m.Groups(1).Value.IndexOf("*") > -1) Then       'unordered list
            Dim indent As Integer = m.Groups(1).Length
            Dim item As String = m.Groups(2).Value

            If (indent > listIndent) Then       'indent inward
                For inc = listIndent To indent - 1
                    value += "<UL>"
                Next inc
                value += "<LI>" + item + "</LI>"
            ElseIf (indent < listIndent) Then   'close imbedded lists
                For inc = indent To listIndent - 1
                    value += cTag
                Next inc
                value += "<LI>" + item + "</LI>"
            Else                                'add a new item
                value = "<LI>" + item + "</LI>"
            End If

            listIndent = indent
            ordered = False

            Return value
        ElseIf (m.Groups(1).Value.IndexOf("#") > -1) Then       'ordered list
            Dim indent As Integer = m.Groups(1).Length
            Dim item As String = m.Groups(2).Value

            If (indent > listIndent) Then       'indent inward
                For inc = listIndent To indent - 1
                    value += "<OL>"
                Next inc
                value += "<LI>" + item + "</LI>"
            ElseIf (indent < listIndent) Then   'close imbedded lists
                For inc = indent To listIndent - 1
                    value += cTag
                Next inc
                value += "<LI>" + item + "</LI>"
            Else                                'add a new item
                value = "<LI>" + item + "</LI>"
            End If

            listIndent = indent
            ordered = True

            Return value
        Else        'normal line of text
        'if the previous line was part of a list
        If (listIndent > 0) Then
            For inc = 0 To listIndent - 1
                    value += cTag
            Next inc
            value += m.Value
        Else
            value = m.Value
        End If

        listIndent = 0
        Return value
        End If

    End Function

#End Region

#Region "Parser Version One (before regular expression)"

    ' Takes a text and converts all CamelCase words to hyperlinks.
    '
    ' This is version 1.0 - It does the job without any optimizations.
    ' Should we use StringBuilder instead of String to optimize speed?
    '
    ' Sample Input:
    '   WebPage = "Default.aspx"
    '   TextToProcess = "HelloDolly by LouisArmstrong"
    ' Sample Output:
    '   "<a href=Default.aspx?topic=HelloDolly>HelloDolly</a> by <a href=Default.aspx?topic=LouisArmstrong>LouisArmstrong</a>"
    '
    Public Shared Function MakeDisplayableText_version1( _
                            ByRef WebPage As String, _
                            ByRef TextToProcess As String)
        Dim DisplayableText As New String("")
        Dim i As Integer
        Dim ThisCharIsAlpha, ThisCharIsUpper, ThisCharIsLower As Boolean
        Dim LastCharWasAlpha, LastCharWasLower As Boolean
        Dim CamelWordInProgress, CamelWordWasInProgress As Boolean
        Dim CamelWord As New String("")
        Dim UpperChars As Integer = 0
        Dim ThisChar As Char
        Dim ThisAscii, LastAscii As Integer

        For i = 0 To TextToProcess.Length - 1
            ThisChar = TextToProcess.Chars(i)
            ThisAscii = AscW(ThisChar)

            If CamelWordInProgress Then
                ThisCharIsAlpha = Char.IsLetter(ThisChar) Or Char.IsDigit(ThisChar) Or ThisChar = "_"
                ThisCharIsUpper = Char.IsUpper(ThisChar) Or Char.IsDigit(ThisChar) Or ThisChar = "_"
            Else
                ThisCharIsAlpha = Char.IsLetter(ThisChar)
                ThisCharIsUpper = Char.IsUpper(ThisChar)
            End If
            ThisCharIsLower = Char.IsLower(ThisChar)

            If ThisCharIsAlpha Then
                If ThisCharIsUpper Then
                    UpperChars += 1
                    If CamelWordInProgress Then
                        CamelWord += ThisChar
                    Else
                        CamelWordInProgress = True
                        CamelWord = ThisChar
                    End If
                Else
                    If ThisCharIsLower Then
                        If CamelWordInProgress Then
                            CamelWord += ThisChar
                        Else
                            If CamelWordWasInProgress Then
                                If UpperChars > 1 And _
                                    CamelWord <> CamelWord.ToUpper() Then
                                    DisplayableText += Wiki.HyperLinkWord(WebPage, CamelWord)
                                Else
                                    DisplayableText += CamelWord
                                End If
                                CamelWord = ""
                                UpperChars = 0
                            End If
                            DisplayableText += ThisChar
                        End If
                    Else
                        CamelWordInProgress = False
                        DisplayableText += ThisChar
                    End If
                End If
            Else
                CamelWordInProgress = False
                If CamelWordWasInProgress Then
                    If UpperChars > 1 And _
                        CamelWord <> CamelWord.ToUpper() Then
                        DisplayableText += Wiki.HyperLinkWord(WebPage, CamelWord)
                    Else
                        DisplayableText += CamelWord
                    End If
                    CamelWord = ""
                    UpperChars = 0
                End If
                Select Case ThisAscii
                    Case 10
                        'ignore
                    Case 13
                        DisplayableText += "<br>"
                    Case Else
                        DisplayableText += ThisChar
                End Select
            End If
            CamelWordWasInProgress = CamelWordInProgress
        Next

        If CamelWordWasInProgress Then
            If UpperChars > 1 And _
                CamelWord <> CamelWord.ToUpper() Then
                DisplayableText += Wiki.HyperLinkWord(WebPage, CamelWord)
            Else
                DisplayableText += CamelWord
            End If
        End If

        Return DisplayableText
    End Function

    Public Shared Function HyperLinkWord(ByRef WebPage As String, ByRef Word As String) As String
        Return "<a href=" + WebPage + "?topic=" + Word + ">" + Word + "</a>"
    End Function
#End Region

#Region "Junk Yard"
    '' m.value is equal to <a href=http...
    ''                     012345678901
    'Public Shared Function MaskHttpReference(ByVal m As Match) As String
    '    Return "<a href=xttp" + m.Value.Substring(12)
    'End Function

    '' m.value is equal to <a href=xttp...
    ''                     012345678901
    'Public Shared Function UnMaskHttpReference(ByVal m As Match) As String
    '    Return "<a href=http" + m.Value.Substring(12)
    'End Function

    '' m.value is equal to <a href="http...
    ''                     0123456789012
    'Public Shared Function MaskHttpReferenceWithQuote(ByVal m As Match) As String
    '    Return "<a href=" + Chr(34) + "xttp" + m.Value.Substring(13)
    'End Function

    '' m.value is equal to <a href="xttp...
    ''                     0123456789012
    'Public Shared Function UnMaskHttpReferenceWithQuote(ByVal m As Match) As String
    '    Return "<a href=" + Chr(34) + "http" + m.Value.Substring(13)
    'End Function

#End Region

End Class
