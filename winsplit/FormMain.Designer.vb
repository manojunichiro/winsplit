<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormMain))
        btnSplit = New Button()
        btnExit = New Button()
        lblFin = New Label()
        tbFin = New TextBox()
        btnFin = New Button()
        lblDout = New Label()
        btnDout = New Button()
        tbDout = New TextBox()
        lblMode = New Label()
        rbModeLines = New RadioButton()
        rbModeBytes = New RadioButton()
        rbModeFiles = New RadioButton()
        tbModeValue = New TextBox()
        lblModeUnit = New Label()
        ofd = New OpenFileDialog()
        pb = New ProgressBar()
        lblPercent = New Label()
        btnAbout = New Button()
        SuspendLayout()
        ' 
        ' btnSplit
        ' 
        btnSplit.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        btnSplit.BackColor = Color.FromArgb(CByte(192), CByte(255), CByte(192))
        btnSplit.Location = New Point(11, 173)
        btnSplit.Margin = New Padding(2, 1, 2, 1)
        btnSplit.Name = "btnSplit"
        btnSplit.Size = New Size(94, 28)
        btnSplit.TabIndex = 11
        btnSplit.Text = "実行"
        btnSplit.UseVisualStyleBackColor = False
        ' 
        ' btnExit
        ' 
        btnExit.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        btnExit.Location = New Point(326, 173)
        btnExit.Margin = New Padding(2, 1, 2, 1)
        btnExit.Name = "btnExit"
        btnExit.Size = New Size(94, 28)
        btnExit.TabIndex = 12
        btnExit.Text = "終了"
        btnExit.UseVisualStyleBackColor = True
        ' 
        ' lblFin
        ' 
        lblFin.AutoSize = True
        lblFin.Location = New Point(11, 9)
        lblFin.Margin = New Padding(2, 0, 2, 0)
        lblFin.Name = "lblFin"
        lblFin.Size = New Size(77, 15)
        lblFin.TabIndex = 2
        lblFin.Text = "入力ファイル："
        ' 
        ' tbFin
        ' 
        tbFin.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        tbFin.Location = New Point(11, 25)
        tbFin.Margin = New Padding(2, 1, 2, 1)
        tbFin.Name = "tbFin"
        tbFin.ReadOnly = True
        tbFin.Size = New Size(378, 23)
        tbFin.TabIndex = 1
        tbFin.TabStop = False
        ' 
        ' btnFin
        ' 
        btnFin.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btnFin.Location = New Point(393, 25)
        btnFin.Margin = New Padding(2, 1, 2, 1)
        btnFin.Name = "btnFin"
        btnFin.Size = New Size(27, 23)
        btnFin.TabIndex = 2
        btnFin.Text = "..."
        btnFin.UseVisualStyleBackColor = True
        ' 
        ' lblDout
        ' 
        lblDout.AutoSize = True
        lblDout.Location = New Point(11, 62)
        lblDout.Margin = New Padding(2, 0, 2, 0)
        lblDout.Name = "lblDout"
        lblDout.Size = New Size(86, 15)
        lblDout.TabIndex = 5
        lblDout.Text = "出力フォルダー："
        ' 
        ' btnDout
        ' 
        btnDout.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btnDout.Location = New Point(393, 78)
        btnDout.Margin = New Padding(2, 1, 2, 1)
        btnDout.Name = "btnDout"
        btnDout.Size = New Size(27, 23)
        btnDout.TabIndex = 4
        btnDout.Text = "..."
        btnDout.UseVisualStyleBackColor = True
        ' 
        ' tbDout
        ' 
        tbDout.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        tbDout.Location = New Point(11, 78)
        tbDout.Margin = New Padding(2, 1, 2, 1)
        tbDout.Name = "tbDout"
        tbDout.ReadOnly = True
        tbDout.Size = New Size(378, 23)
        tbDout.TabIndex = 3
        tbDout.TabStop = False
        ' 
        ' lblMode
        ' 
        lblMode.AutoSize = True
        lblMode.Location = New Point(11, 116)
        lblMode.Margin = New Padding(2, 0, 2, 0)
        lblMode.Name = "lblMode"
        lblMode.Size = New Size(67, 15)
        lblMode.TabIndex = 8
        lblMode.Text = "分割方法："
        ' 
        ' rbModeLines
        ' 
        rbModeLines.Location = New Point(13, 132)
        rbModeLines.Margin = New Padding(2, 1, 2, 1)
        rbModeLines.Name = "rbModeLines"
        rbModeLines.Size = New Size(90, 19)
        rbModeLines.TabIndex = 5
        rbModeLines.TabStop = True
        rbModeLines.Text = "行数(&L)"
        rbModeLines.UseVisualStyleBackColor = True
        ' 
        ' rbModeBytes
        ' 
        rbModeBytes.AutoSize = True
        rbModeBytes.Location = New Point(105, 132)
        rbModeBytes.Margin = New Padding(2, 1, 2, 1)
        rbModeBytes.Name = "rbModeBytes"
        rbModeBytes.Size = New Size(79, 19)
        rbModeBytes.TabIndex = 6
        rbModeBytes.TabStop = True
        rbModeBytes.Text = "バイト数(&B)"
        rbModeBytes.UseVisualStyleBackColor = True
        ' 
        ' rbModeFiles
        ' 
        rbModeFiles.AutoSize = True
        rbModeFiles.Location = New Point(188, 132)
        rbModeFiles.Margin = New Padding(2, 1, 2, 1)
        rbModeFiles.Name = "rbModeFiles"
        rbModeFiles.Size = New Size(86, 19)
        rbModeFiles.TabIndex = 7
        rbModeFiles.TabStop = True
        rbModeFiles.Text = "ファイル数(&C)"
        rbModeFiles.UseVisualStyleBackColor = True
        ' 
        ' tbModeValue
        ' 
        tbModeValue.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        tbModeValue.Location = New Point(278, 131)
        tbModeValue.Margin = New Padding(2, 1, 2, 1)
        tbModeValue.Name = "tbModeValue"
        tbModeValue.Size = New Size(94, 23)
        tbModeValue.TabIndex = 8
        tbModeValue.Text = "8,888,888,888"
        tbModeValue.TextAlign = HorizontalAlignment.Center
        ' 
        ' lblModeUnit
        ' 
        lblModeUnit.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        lblModeUnit.Location = New Point(376, 134)
        lblModeUnit.Margin = New Padding(2, 0, 2, 0)
        lblModeUnit.Name = "lblModeUnit"
        lblModeUnit.Size = New Size(47, 17)
        lblModeUnit.TabIndex = 13
        lblModeUnit.Text = "バイト"
        ' 
        ' ofd
        ' 
        ofd.FileName = "OpenFileDialog1"
        ' 
        ' pb
        ' 
        pb.Location = New Point(113, 182)
        pb.Name = "pb"
        pb.Size = New Size(80, 12)
        pb.Step = 1
        pb.TabIndex = 10
        ' 
        ' lblPercent
        ' 
        lblPercent.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        lblPercent.Location = New Point(198, 180)
        lblPercent.Margin = New Padding(2, 0, 2, 0)
        lblPercent.Name = "lblPercent"
        lblPercent.Size = New Size(37, 17)
        lblPercent.TabIndex = 14
        lblPercent.Text = "100%"
        ' 
        ' btnAbout
        ' 
        btnAbout.Location = New Point(267, 173)
        btnAbout.Name = "btnAbout"
        btnAbout.Size = New Size(54, 28)
        btnAbout.TabIndex = 15
        btnAbout.Text = "情報"
        btnAbout.UseVisualStyleBackColor = True
        ' 
        ' Form1
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(431, 211)
        Controls.Add(btnAbout)
        Controls.Add(lblPercent)
        Controls.Add(pb)
        Controls.Add(lblModeUnit)
        Controls.Add(tbModeValue)
        Controls.Add(rbModeFiles)
        Controls.Add(rbModeBytes)
        Controls.Add(rbModeLines)
        Controls.Add(lblMode)
        Controls.Add(btnDout)
        Controls.Add(tbDout)
        Controls.Add(lblDout)
        Controls.Add(btnFin)
        Controls.Add(tbFin)
        Controls.Add(lblFin)
        Controls.Add(btnExit)
        Controls.Add(btnSplit)
        Icon = CType(resources.GetObject("$this.Icon"), Icon)
        Margin = New Padding(2, 1, 2, 1)
        Name = "Form1"
        Text = "Form1"
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents btnSplit As Button
    Friend WithEvents btnExit As Button
    Friend WithEvents lblFin As Label
    Friend WithEvents tbFin As TextBox
    Friend WithEvents btnFin As Button
    Friend WithEvents lblDout As Label
    Friend WithEvents btnDout As Button
    Friend WithEvents tbDout As TextBox
    Friend WithEvents lblMode As Label
    Friend WithEvents rbModeLines As RadioButton
    Friend WithEvents rbModeBytes As RadioButton
    Friend WithEvents rbModeFiles As RadioButton
    Friend WithEvents tbModeValue As TextBox
    Friend WithEvents lblModeUnit As Label
    Friend WithEvents ofd As OpenFileDialog
    Friend WithEvents pb As ProgressBar
    Friend WithEvents lblPercent As Label
    Friend WithEvents btnAbout As Button

End Class
