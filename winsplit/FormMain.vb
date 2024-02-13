'' winSplit / FormMain.vb
'' 2024 (c) manojunichiro, garagekids.

Imports System.ComponentModel
Imports System.IO

Public Class FormMain

#Region "Const"

    Public APP_NAME As String = "winSplit"
    Public COPYRIGHT As String = "2024 (c) mano."
    Public COMPANY As String = "GarageKids"
    Public SUPPORT_EMAIL As String = "support_winsplit@garagekids.jp"
    Public GITHUB As String = "https://github.com/manojunichiro/winsplit"

    Enum Mode As Integer

        Notset = 0
        Lines = 1
        Bytes = 2
        Files = 3

    End Enum

    Enum Errno As Integer

        None = 0
        FinIsNotFound = 1
        NewlineIsNotFound = 2
        IllegalLineLen = 3
        FinIsEmpty = 4
        FoutWillOverwrite = 5
        Exception = 99

    End Enum

    '' MessageBox Style
    Const MBS_okInfo As MsgBoxStyle = MsgBoxStyle.OkOnly Or MsgBoxStyle.Information
    Const MBS_okWarn As MsgBoxStyle = MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation

    Private Literals As New literals

#End Region

#Region "Entry Point"

    Private Sub Form1_Load(sender As Object, e As EventArgs) _
        Handles MyBase.Load

        'My.Settings.Reset()

        Me.Hide()

        Me.Text = $"{APP_NAME} {get_version_str()}"
        init_lang(env_lang())
        load_conf()
        setEnabled(False)
        pb_end()

        Me.Show()

    End Sub

#End Region

#Region "Version"

    Private Sub btnAbout_Click(sender As Object, e As EventArgs) Handles _
                btnAbout.Click

        FormAbout.ShowDialog()

    End Sub

    Public Function get_version_str() As String

        ''Dim v As Version = Application.ProductVersion '' NG
        Dim v As Version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version
        Dim ret As String = String.Format("{0}.{1}.{2}", v.Major, v.Minor, v.Build)

        Return ret

    End Function

#End Region

#Region "Intl"

    Private Function get_lang_from_args() As String

        Const KEY As String = "--locale"

        Dim args As String() = Environment.GetCommandLineArgs()
        If args.Contains(KEY) Then
            Dim pos As Integer = Array.IndexOf(args, KEY)
            If pos < args.Length - 1 Then
                Return args(pos + 1).Trim()
            End If
        End If

        Return ""

    End Function

    Private Function env_lang() As literals.lang

        '' share with project when required.
        Dim locale As String
        Dim language As String
        Dim country As String

        '' locale, language, country
        locale = get_lang_from_args()
        If locale = "" Then
            locale = System.Threading.Thread.CurrentThread.CurrentCulture.Name
        End If
        If locale.IndexOf("-"c) < 0 Then
            language = locale
            country = ""
        Else
            Dim s() As String = locale.Split("-"c)
            language = s(0)
            country = s(1)
        End If

        '' lang
        Dim ret As literals.lang
        Select Case language
            Case "ja" : ret = literals.lang.japanese
            Case "zh" : ret = IIf(country = "TW", literals.lang.taiwanese, literals.lang.chinese)
            Case "ko" : ret = literals.lang.korean
            Case Else : ret = literals.lang.english
        End Select

        '' success
        Return ret

    End Function

    Private Sub init_lang(lang As literals.lang)

        '' set language after init
        init_literals()
        Literals.set_lang(lang)

        '' set static ui text 
        Literals.uitext(lblFin)
        Literals.uitext(lblDout)
        Literals.uitext(lblMode)
        Literals.uitext(rbModeLines)
        Literals.uitext(rbModeBytes)
        Literals.uitext(rbModeFiles)
        Literals.uitext(btnSplit)
        Literals.uitext(btnAbout)
        Literals.uitext(btnExit)

    End Sub

#End Region

#Region "Event handler"

#Region "fin"

    Private Sub tbFin_DoubleClick(sender As Object, e As EventArgs) _
        Handles tbFin.DoubleClick

        Dim f As String = tbFin.Text.Trim()
        Dim d As String = Path.GetDirectoryName(f)

        If Directory.Exists(d) Then
            open_explorer(d)
        End If

    End Sub

    Private Sub btnFin_Click(sender As Object, e As EventArgs) _
        Handles btnFin.Click

        With ofd
            Dim fi As String = tbFin.Text.Trim()
            Dim f As String = Path.GetFileName(fi)
            Dim d As String = Path.GetDirectoryName(fi)
            .InitialDirectory = IIf(Directory.Exists(d), d, "c:\")
            .FileName = IIf(File.Exists(fi), f, "")
            .Filter = ""
            .CheckFileExists = True
            '' others
            .CheckPathExists = True
            .Multiselect = False
            '' show
            If .ShowDialog() = DialogResult.OK Then
                If .FileName <> "" Then
                    '' success
                    tbFin.Text = .FileName
                    My.Settings.fin = tbFin.Text : My.Settings.Save()
                End If
            End If
        End With

    End Sub

#End Region

#Region "dout"

    Private Sub tbDout_DoubleClick(sender As Object, e As EventArgs) _
        Handles tbDout.DoubleClick

        Dim d As String = tbDout.Text.Trim()

        If Directory.Exists(d) Then
            open_explorer(d)
        End If

    End Sub

    Private Sub btnDout_Click(sender As Object, e As EventArgs) _
        Handles btnDout.Click

        With ofd
            '' default 
            Dim dout As String = tbDout.Text.Trim()
            If Directory.Exists(dout) Then
                .InitialDirectory = IIf(Directory.Exists(dout), dout, "c:\")
            End If
            .FileName = Literals.uitext2(btnDout, "FileName")
            '' for folder selection
            .Filter = Literals.uitext2(btnDout, "Filter")
            .CheckFileExists = False
            '' others
            .CheckPathExists = True
            .Multiselect = False
            '' show
            If .ShowDialog() = DialogResult.OK Then
                If .FileName <> "" Then
                    '' success
                    tbDout.Text = Path.GetDirectoryName(.FileName)
                    My.Settings.dout = tbDout.Text : My.Settings.Save()
                End If
            End If
        End With

    End Sub

#End Region

#Region "mode"

    Private Sub rbModeLines_CheckedChanged(sender As Object, e As EventArgs) _
        Handles rbModeLines.CheckedChanged

        rbMode_CheckedChanged(Mode.Lines)

    End Sub

    Private Sub rbModeBytes_CheckedChanged(sender As Object, e As EventArgs) _
        Handles rbModeBytes.CheckedChanged

        rbMode_CheckedChanged(Mode.Bytes)

    End Sub

    Private Sub rbModeFiles_CheckedChanged(sender As Object, e As EventArgs) _
        Handles rbModeFiles.CheckedChanged

        rbMode_CheckedChanged(Mode.Files)

    End Sub

    Private Sub rbMode_CheckedChanged(mode As Mode)

        '' mode
        My.Settings.mode = mode : My.Settings.Save()

        '' value
        Dim val As Long = get_mode_value()
        tbModeValue.Text = $"{val:#,##0}"

        '' unit
        Dim unit As String = get_mode_unit()
        lblModeUnit.Text = unit

        '' dbg
        Debug.Print($"rb_changed: mode={mode} val={val} unit={unit}")

    End Sub

#End Region

#Region "mode value"

    Private isInitialized As Boolean '' set in form_load()
    Private isLeaving As Boolean

    Private Sub tbModeValue_GotFocus(sender As Object, e As EventArgs) _
        Handles tbModeValue.GotFocus

        Dim s As String = tbModeValue.Text.Trim.Replace(",", "")
        tbModeValue.Text = s

    End Sub

    Private Sub tbModeValue_TextChanged(sender As Object, e As EventArgs) _
        Handles tbModeValue.TextChanged

        If Not isLeaving Then
            Dim v As Long
            Dim s As String = tbModeValue.Text.Trim.Replace(",", "")
            If Not Long.TryParse(s, v) OrElse v < 1 Then
                MsgBox($"{Literals.uitext2(tbModeValue, 1)}", MBS_okInfo, APP_NAME)
            Else
                set_mode_value(v)
            End If
        End If

    End Sub

    Private Sub tbModeValue_Leave(sender As Object, e As EventArgs) _
        Handles tbModeValue.Leave

        isLeaving = True

        Dim s As String = tbModeValue.Text.Trim()
        Dim v As Long
        If s.Length < 1 OrElse Not IsNumeric(s) Then
            v = get_mode_value() '' from prev-saved
        Else
            v = Long.Parse(s) '' from ui
        End If
        tbModeValue.Text = $"{v:#,##0}"

        isLeaving = False

    End Sub

    Private Sub set_mode_value(v As Long)

        If isInitialized Then
            Select Case CType(My.Settings.mode, Mode)
                Case Mode.Lines : My.Settings.value_lines = v
                Case Mode.Bytes : My.Settings.value_bytes = v
                Case Mode.Files : My.Settings.value_files = v
                Case Else : Return
            End Select
            My.Settings.Save()
        End If

    End Sub

    Private Function get_mode_value() As Long

        Dim ret As Long

        Select Case CType(My.Settings.mode, Mode)
            Case Mode.Lines : ret = My.Settings.value_lines
            Case Mode.Bytes : ret = My.Settings.value_bytes
            Case Mode.Files : ret = My.Settings.value_files
            Case Else : ret = 1
        End Select

        Return ret

    End Function

    Private Function get_mode_unit() As String

        Dim ret As String

        Select Case CType(My.Settings.mode, Mode)
            Case Mode.Lines : ret = Literals.uitext2(lblModeUnit, rbModeLines)
            Case Mode.Bytes : ret = Literals.uitext2(lblModeUnit, rbModeBytes)
            Case Mode.Files : ret = Literals.uitext2(lblModeUnit, rbModeFiles)
            Case Else : ret = ""
        End Select

        Return ret

    End Function

#End Region

#Region "go"

    Private Sub btnSplit_Click(sender As Object, e As EventArgs) _
        Handles btnSplit.Click

        '' ui begin
        setEnabled(True)
        pb_begin()

        '' go
        Dim ret As Errno = go_split()

        '' msg
        If (ret <> Errno.None And ret <> Errno.Exception) Then
            MsgBox(Literals.errmsg(ret), MBS_okInfo, APP_NAME)
        End If

        '' ui end
        pb_end()
        setEnabled(False)

    End Sub

#End Region

#Region "exit"

    Private Sub btnExit_Click(sender As Object, e As EventArgs) _
        Handles btnExit.Click

        Application.Exit()

    End Sub

#End Region

#End Region

#Region "Controls"

    Private Sub setEnabled(isRun As Boolean)

        tbFin.Enabled = Not isRun
        tbDout.Enabled = Not isRun
        btnFin.Enabled = Not isRun
        btnDout.Enabled = Not isRun
        rbModeFiles.Enabled = Not isRun
        rbModeBytes.Enabled = Not isRun
        rbModeLines.Enabled = Not isRun
        tbModeValue.Enabled = Not isRun
        btnSplit.Enabled = Not isRun
        btnExit.Enabled = Not isRun

    End Sub

#End Region

#Region "Conf"

    Private Sub load_conf()

        '' fin
        tbFin.Text = My.Settings.fin

        '' dout
        tbDout.Text = My.Settings.dout

        '' mode >> rbMode_CheckedChanged()
        isInitialized = False
        Select Case CType(My.Settings.mode, Mode)
            Case Mode.Lines : rbModeLines.Checked = True
            Case Mode.Bytes : rbModeBytes.Checked = True
            Case Mode.Files : rbModeFiles.Checked = True
            Case Else : rbModeLines.Checked = True
        End Select
        isInitialized = True

    End Sub

#End Region

#Region "Fout stream writer"

    Private Function sw_rotate(ByRef sw As StreamWriter,
                          dout As String,
                          foFormat As String, foNoext As String, foCount As Integer, foExt As String) As String
        '' close 
        sw_close(sw)

        '' fname
        Dim fname_nodir As String = String.Format(foFormat, foNoext, foCount, foExt)
        Dim fname As String = Path.Combine(dout, fname_nodir)

        '' open
        sw = New StreamWriter(fname)

        '' success
        Return fname

    End Function

    Private Sub sw_close(ByRef sw As StreamWriter)

        If Not IsNothing(sw) Then
            sw.Close()
            sw.Dispose()
            sw = Nothing
        End If

    End Sub

#End Region

#Region "Fin contents information"

    Private Function get_newline(fname As String, max_bytes As Integer) As String

        Const CR As Integer = 13
        Const LF As Integer = 10

        Dim newline As String = ""

        Using sr As New StreamReader(fname)

            Dim b As Integer
            For ii As Integer = 0 To max_bytes
                If sr.EndOfStream Then Exit For
                b = sr.Read()
                If b = LF Then
                    newline = vbLf : Exit For
                ElseIf b = CR Then
                    b = sr.Read()
                    If b = LF Then
                        newline = vbCrLf : Exit For
                    Else
                        newline = vbCr : End If
                End If
            Next

        End Using

        Return newline

    End Function

    Private Function get_linebytes(fname As String, max_lines As Integer) As Integer '' -1:ex 0:openerr *:success

        Dim max_len As Integer

        Try
            max_len = 0 '' upd
            Using sr As New StreamReader(fname)
                Dim s As String
                For ii As Integer = 0 To max_lines
                    If sr.EndOfStream Then Exit For
                    s = sr.ReadLine()
                    If max_len < s.Length Then max_len = s.Length '' upd
                Next
            End Using
        Catch ex As Exception
            max_len = -1 '' upd
        End Try

        Return max_len

    End Function

#End Region

#Region "Progress bar"

    Private Sub pb_begin()

        pb.Minimum = 0
        pb.Maximum = 100
        pb.Step = 1

        pb.Value = 0
        lblPercent.Text = "0%"

        pb.Visible = True
        lblPercent.Visible = True

    End Sub

    Private Sub pb_update(value As Integer)

        pb.Value = value
        lblPercent.Text = $"{value}%"

    End Sub

    Private Sub pb_end()

        pb.Visible = False
        lblPercent.Visible = False

    End Sub

#End Region

#Region "Process open"

    Private Sub open_notepad(f As String)

        open_process("notepad.exe", f)

    End Sub

    Private Sub open_explorer(d As String)

        open_process("explorer.exe", d)

    End Sub

    Private Sub open_process(exe As String, arg As String)

        Dim p As New Process()
        p.StartInfo.FileName = exe
        p.StartInfo.Arguments = arg
        p.Start()

    End Sub

#End Region

#Region "Exception"

    Class recent
        Structure recent_item
            Public line As String          '' w/o newline
            Public lineBytes As Long
            Public fiReadLines As Long      '' row
            Public fiReadBytes As Long      '' pos
        End Structure

        Public bank(0 To 9) As recent_item
        Public bank_max As Integer = 9

        Public pos As Integer = 0

        Public Sub update(l As String, lb As Long, rl As Long, rb As Long)

            '' set
            bank(pos).line = l
            bank(pos).lineBytes = lb
            bank(pos).fiReadLines = rl
            bank(pos).fiReadBytes = rb

            '' next
            pos += 1
            If pos > bank_max Then pos = 0

        End Sub
    End Class

    Structure state_split
        '' mode
        Dim mode As Mode
        Dim mode_value As Integer
        '' fi
        Dim fi As String
        Dim fiNewline As String
        Dim fiLineBytes0 As Integer
        Dim fiLineBytes As Integer
        Dim fiBytes As Long
        '' fo limits
        Dim foMaxLines As Long
        Dim foMaxBytes As Long
        Dim foMaxFileCnt As Integer
        '' fo name
        Dim dout As String
        Dim foFilter As String
        Dim foSufix As String
        Dim foFormat As String
        Dim fo As String
        '' counter  
        Dim fiReadLines As Long
        Dim fiReadBytes As Long
        Dim foCount As Integer
        Dim foWroteLines As Long
        Dim foWroteBytes As Long
        '' line
        Dim line0 As String
        Dim line As String
        Dim lineBytes As Long
        '' ex
        Dim ex As Exception
        Dim recent As recent
    End Structure

    Private Function dump_ex_split(state As state_split) As String

        '' fname
        Dim fname As String = $"{APP_NAME}_exception_{Now():yyyyMMdd-hhmmss}.log"
        fname = Path.Combine(state.dout, fname)

        '' format
        Dim sl As New List(Of String)
        sl.Add($"This file: {fname}")
        sl.Add($"App : {APP_NAME}")
        sl.Add($"Version: {get_version_str()}")
        sl.Add($"Datetime: {DateTime.Now()}")
        sl.Add("--")
        sl.Add($"Input File: {state.fi}")
        Dim nl As String = ""
        If state.fiNewline.Contains(vbCr) Then nl += "CR"
        If state.fiNewline.Contains(vbLf) Then nl += "LF"
        sl.Add($"Newline: '{nl}'")
        sl.Add($"Line Bytes: {state.fiLineBytes:#,##0}")
        sl.Add($"File Bytes: {state.fiBytes:#,##0}")
        sl.Add("--")
        sl.Add($"Mode Name: {state.mode}")
        sl.Add($"Mode Argument: {state.mode_value}")
        sl.Add("--")
        sl.Add($"Output File: {state.fo}")
        sl.Add($"File Count: {state.foCount:#,##0}")
        sl.Add($"Max Files: {state.foMaxFileCnt:#,##0}")
        sl.Add($"Max Lines: {state.foMaxLines:#,##0}")
        sl.Add($"Max Bytes: {state.foMaxBytes:#,##0}")
        sl.Add("--")
        sl.Add($"Input File / Read Lines: {state.fiReadLines:#,##0}")
        sl.Add($"Input File / Read Bytes: {state.fiReadBytes:#,##0}")
        sl.Add($"Output File / Wrote Lines: {state.foWroteLines:#,##0}")
        sl.Add($"Output File / Wrote Bytes: {state.foWroteBytes:#,##0}")
        sl.Add($"Last Line String: '{state.line0}'")
        sl.Add($"Last Line Bytes: {state.lineBytes:#,##0}")
        sl.Add("--")
        For ii As Integer = 0 To state.recent.bank_max
            With state.recent.bank(ii)
                Dim s As String = ""
                s += $"recent {ii}:"
                s += $" row={ .fiReadLines:#,##0}"
                s += $" pos={ .fiReadBytes:#,##0}"
                s += $" len={ .lineBytes:#,##0}"
                s += $" str={ .line}"
                sl.Add(s)
            End With
        Next
        sl.Add("--")
        sl.Add($"Exception Message:{vbCrLf}{state.ex.Message}")
        sl.Add($"Exception Source:{vbCrLf}{state.ex.Source}")
        sl.Add($"StackTrace:{vbCrLf}{state.ex.StackTrace}")
        sl.Add($"Dump 10,000 bytes:{vbCrLf}")

        '' save info
        Using swerr As New StreamWriter(fname)
            For Each s As String In sl : swerr.WriteLine(s) : Next
        End Using

        '' save raw 
        Dim PREV_LINES As Integer = 5
        Using fsi As New FileStream(state.fi, FileMode.Open, FileAccess.Read)
            '' read
            Dim offset As Long = state.fiReadBytes - state.fiLineBytes * PREV_LINES
            fsi.Seek(offset, SeekOrigin.Begin)
            Dim buff(10000) As Byte
            fsi.Read(buff, 0, buff.Length)
            '' write
            Using fso As New FileStream(fname, FileMode.Append, FileAccess.Write)
                fso.Write(buff, 0, buff.Length)
            End Using
        End Using

        '' finish
        Return fname

    End Function

#End Region

#Region "Success msg"

    Private Function get_success_msg_split(fiReadLines As Long, fiReadBytes As Long, dt0 As DateTime, dt1 As DateTime) As String

        Dim ts As TimeSpan = dt1.Subtract(dt0)
        Dim msg As String = ""

        '' message
        msg += Literals.errmsg(Errno.None)
        msg += vbCrLf + vbCrLf

        '' lines
        Dim label_lines As String = Literals.uitext2(btnSplit, "lines")
        Dim unit_lines As String = Literals.uitext2(lblModeUnit, rbModeLines)
        msg += $"{label_lines}{fiReadLines:#,##0} {unit_lines}"
        msg += vbCrLf

        '' bytes
        Dim label_bytes As String = Literals.uitext2(btnSplit, "bytes")
        Dim unit_bytes As String = Literals.uitext2(lblModeUnit, rbModeBytes)
        msg += $"{label_bytes}{fiReadBytes:#,##0} {unit_bytes}"
        msg += vbCrLf

        '' ts
        Dim label_ts As String = Literals.uitext2(btnSplit, "ts")
        Dim unit_days As String = Literals.uitext2(btnSplit, "ts_days")
        Dim unit_hours As String = Literals.uitext2(btnSplit, "ts_hours")
        Dim unit_minutes As String = Literals.uitext2(btnSplit, "ts_minutes")
        Dim unit_seconds As String = Literals.uitext2(btnSplit, "ts_seconds")
        msg += $"{label_ts}"
        If ts.TotalDays > 1 Then
            msg += $"{ts.Days}{unit_days} {ts.Hours}{unit_hours} {ts.Minutes}{unit_minutes} {ts.Seconds}{unit_seconds}"
        ElseIf ts.TotalHours > 1 Then
            msg += $"{ts.Hours}{unit_hours} {ts.Minutes}{unit_minutes} {ts.Seconds}{unit_seconds}"
        ElseIf ts.TotalMinutes > 1 Then
            msg += $"{ts.Minutes}{unit_minutes} {ts.Seconds}{unit_seconds}"
        Else
            msg += $"{ts.Seconds}{unit_seconds}"
        End If
        msg += vbCrLf

        '' speed
        Dim label_speed As String = Literals.uitext2(btnSplit, "speed")
        msg += $"{label_speed}{fiReadLines / ts.TotalSeconds:#,##0} {unit_lines}/{unit_seconds}"
        msg += vbCrLf

        '' return
        Return msg

    End Function

#End Region

#Region "Go"

    Private Function go_split() As Errno

        Dim ret As Errno = Errno.None

        '' mode & value
        Dim mode As Mode = CType(My.Settings.mode, Mode)
        Dim mode_value As Long = get_mode_value()

        '' fin
        Dim fi As String = tbFin.Text.Trim()
        If Not File.Exists(fi) Then Return Errno.FinIsNotFound '' 
        '' fin newline
        Dim fiNewline As String = get_newline(fi, 1000)
        If fiNewline.Length < 1 Then Return Errno.NewlineIsNotFound ''
        '' fin bytes of line + newline
        Dim fiLineBytes0 As Integer = get_linebytes(fi, 10) '' 0: w/o newline
        If fiLineBytes0 < 1 Then Return Errno.IllegalLineLen '' 
        Dim fiLineBytes As Integer = fiLineBytes0 + fiNewline.Length()
        '' fin bytes of file
        Dim fiBytes As Long = (New FileInfo(fi)).Length()
        If fiBytes < 1 Then Return Errno.FinIsEmpty

        '' fout max lines/bytes/fileCount
        Dim foMaxLines As Long
        Dim foMaxBytes As Long
        Dim foMaxFiles As Long
        Select Case mode
            Case Mode.Lines
                foMaxLines = mode_value
                foMaxBytes = fiLineBytes * foMaxLines
                foMaxFiles = CType(Math.Ceiling(fiBytes / foMaxBytes), Long)
            Case Mode.Bytes
                foMaxBytes = mode_value
                foMaxLines = CType(Math.Floor(foMaxBytes / fiLineBytes), Long)
                foMaxFiles = CType(Math.Ceiling(fiBytes / (fiLineBytes * foMaxLines)), Long)
            Case Mode.Files
                foMaxFiles = mode_value
                foMaxBytes = CType(Math.Ceiling(fiBytes / foMaxFiles), Long)
                foMaxLines = CType(Math.Ceiling(foMaxBytes / fiLineBytes), Long)
            Case Else  '' 
        End Select
        '' fout fname static
        Dim dout As String = tbDout.Text.Trim()
        Dim foNoExt As String = Path.GetFileNameWithoutExtension(fi)
        Dim foExt As String = Path.GetExtension(fi)
        Dim foFilter As String = foNoExt + "_*" + foExt
        If Directory.Exists(dout) AndAlso Directory.GetFiles(dout, foFilter).Length() > 0 Then Return Errno.FoutWillOverwrite '' 
        If Not Directory.Exists(dout) Then Directory.CreateDirectory(dout)
        '' fout fname dynamic
        Dim foSufix As New String("0", foMaxFiles.ToString().Length())
        Dim foFormat As String = "{0}_{1:" + foSufix + "}{2}"

        '' fin 
        Dim fiReadLines As Long = 0
        Dim fiReadBytes As Long = 0
        '' fout
        Dim fo As String = ""
        Dim foCount As Integer = 0 '' _sufix number
        Dim sw As StreamWriter = Nothing
        Dim foWroteLines As Long = 0
        Dim foWroteBytes As Long = 0
        '' line
        Dim line0 As String = "" '' w/o newline
        Dim line As String = ""
        Dim lineBytes As Long = 0
        '' progress bar
        Dim progress0 As Integer = 0
        Dim progress As Single
        '' exception
        Dim recent As New recent

        Dim dt0 As DateTime = DateTime.Now()
        Try
            'Throw New Exception("test")
            Using sr As New StreamReader(fi)
                Do Until sr.EndOfStream
                    Application.DoEvents()
                    '' fin read
                    line0 = sr.ReadLine()
                    line = line0 + fiNewline
                    lineBytes = line.Length()
                    fiReadLines += 1
                    fiReadBytes += lineBytes
                    recent.update(line0, lineBytes, fiReadLines, fiReadBytes)
                    '' pb update
                    progress = 100 * fiReadBytes / fiBytes
                    If progress > progress0 Then
                        pb_update(progress0)
                        progress0 += 1
                    End If
                    '' fout reopen check 
                    Dim required_rotate_fo As Boolean = False
                    Select Case mode
                        Case Mode.Lines : required_rotate_fo = (foWroteLines + 1 > foMaxLines)
                        Case Mode.Bytes : required_rotate_fo = (foWroteBytes + lineBytes > foMaxBytes)
                        Case Mode.Files : required_rotate_fo = (foWroteLines + 1 > foMaxLines)
                        Case Else '' 
                    End Select
                    '' fout reopen execute
                    If required_rotate_fo OrElse (foCount = 0) Then '' (foCount = 0): first open
                        foCount += 1
                        fo = sw_rotate(sw, dout, foFormat, foNoExt, foCount, foExt)
                        foWroteLines = 0
                        foWroteBytes = 0
                    End If
                    '' fout write
                    sw.Write(line)
                    foWroteLines += 1
                    foWroteBytes += lineBytes
                Loop
            End Using
        Catch ex As Exception
            '' set return code
            ret = Errno.Exception
            '' collect exception info
            Dim p As New state_split With {
                .mode = mode,
                .mode_value = mode_value,
                .fi = fi,
                .fiNewline = fiNewline,
                .fiLineBytes0 = fiLineBytes0,
                .fiLineBytes = fiLineBytes,
                .fiBytes = fiBytes,
                .foMaxLines = foMaxLines,
                .foMaxBytes = foMaxBytes,
                .foMaxFileCnt = foMaxFiles,
                .dout = dout,
                .foFilter = foFilter,
                .foSufix = foSufix,
                .foFormat = foFormat,
                .fo = fo,
                .fiReadLines = fiReadLines,
                .fiReadBytes = fiReadBytes,
                .foCount = foCount,
                .foWroteLines = foWroteLines,
                .foWroteBytes = foWroteBytes,
                .line0 = line0,
                .line = line,
                .lineBytes = lineBytes,
                .ex = ex,
                .recent = recent
            }
            '' dump exception info
            Dim fdump As String = dump_ex_split(p)
            '' show exception msg and fdump
            MsgBox($"{Literals.errmsg(ret)}{vbCrLf + vbCrLf}{fdump}", MBS_okWarn, APP_NAME)
            '' open dump
            open_notepad(fdump)
        Finally
            sw_close(sw)
            pb_update(pb.Maximum)
        End Try
        Dim dt1 As DateTime = DateTime.Now()

        '' success msg
        If ret = Errno.None Then
            MsgBox(get_success_msg_split(fiReadLines, fiReadBytes, dt0, dt1), MBS_okInfo, APP_NAME)
        End If

        '' return
        Return ret

    End Function

#End Region

End Class

'' EOF
