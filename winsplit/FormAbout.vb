'' winSplit / FormAbout.vb
'' 2024 (c) manojunichiro, garagekids.

Public NotInheritable Class FormAbout

    Private Sub FormAbout_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) _
        Handles MyBase.Load

        '' title
        Dim ApplicationTitle As String
        If My.Application.Info.Title <> "" Then
            ApplicationTitle = My.Application.Info.Title
        Else
            ApplicationTitle = System.IO.Path.GetFileNameWithoutExtension(My.Application.Info.AssemblyName)
        End If
        Me.Text = String.Format("About {0}", ApplicationTitle)

        '' major info
        Me.LabelProductName.Text = $"Product name : {FormMain.APP_NAME}"
        Me.LabelVersion.Text = $"Version : {FormMain.get_version_str()}"
        Me.LabelCopyright.Text = $"Copyright : {FormMain.COPYRIGHT}, {FormMain.COMPANY}"
        Me.LabelCompanyName.Text = $"License : MIT"

        '' desc
        Dim desc As String = ""
        desc += $"Descriptions : "
        desc += vbCrLf + vbCrLf
        desc += $"Support : {FormMain.SUPPORT_EMAIL}"
        desc += vbCrLf + vbCrLf
        desc += $"GitHub : {FormMain.GITHUB}"
        desc += vbCrLf
        Me.TextBoxDescription.Text = desc

    End Sub

    Private Sub OKButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
        Handles OKButton.Click

        Me.Close()

    End Sub

End Class
