'' winSplit / literals.vb
'' 2024 (c) manojunichiro, garagekids.

Public Class literals

#Region "public enum"

    Public Enum lang
        english = 0
        japanese = 1
        taiwanese = 2
        chinese = 3
        korean = 4
    End Enum

#End Region

#Region "private var"

    Private _lang As Integer
    Private ReadOnly _errmsg As New List(Of Dictionary(Of Integer, String))
    Private ReadOnly _uitext As New List(Of Dictionary(Of String, String))

#End Region

#Region "constructor"

    Public Sub New()

        Dim cnt As Integer = [Enum].GetValues(GetType(lang)).Length()

        For ii As Integer = 0 To cnt - 1
            _errmsg.Add(New Dictionary(Of Integer, String))
            _uitext.Add(New Dictionary(Of String, String))
        Next

    End Sub

#End Region

#Region "lang"

    Public Sub set_lang(lang As lang)

        _lang = CType(lang, Integer)

    End Sub

#End Region

#Region "errmsg"

    Public Sub set_errmsg(errno As Integer, s As String)

        _errmsg(_lang)(errno) = s

    End Sub

    Public Function errmsg(errno As Integer) As String

        Return _errmsg(_lang)(errno)

    End Function

#End Region

#Region "uitext"

    Public Sub set_uitext(c As Control, s As String)

        _uitext(_lang)(c.Name) = s

    End Sub

    Public Sub uitext(ByRef cp As Control) '' modify cp.Text directly

        cp.Text = _uitext(_lang)(cp.Name)

    End Sub

#End Region

#Region "uitext2"

    Public Sub set_uitext2(c1 As Control, c2 As Control, s As String)

        _uitext(_lang)(name2(c1, c2)) = s

    End Sub

    Public Sub set_uitext2(c As Control, errno As Integer, s As String)

        _uitext(_lang)(name2(c, errno)) = s

    End Sub

    Public Sub set_uitext2(c As Control, member As String, s As String)

        _uitext(_lang)(name2(c, member)) = s

    End Sub

    Public Function uitext2(c1 As Control, c2 As Control) As String

        Return _uitext(_lang)(name2(c1, c2))

    End Function

    Public Function uitext2(c As Control, errno As Integer) As String

        Return _uitext(_lang)(name2(c, errno))

    End Function

    Public Function uitext2(c As Control, member As String) As String

        Return _uitext(_lang)(name2(c, member))

    End Function

#End Region

#Region "_name2, private"

    Private Shared Function name2(c1 As Control, c2 As Control) As String

        Return (c1.Name + "/" + c2.Name)

    End Function

    Private Shared Function name2(c As Control, errno As Integer) As String

        Return (c.Name + "/" + errno.ToString)

    End Function

    Private Shared Function name2(c As Control, member As String) As String

        Return (c.Name + "/" + member)

    End Function

#End Region

End Class

'' EOF
