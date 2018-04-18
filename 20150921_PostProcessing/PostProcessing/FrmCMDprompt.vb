Public Class FrmCMDprompt

    Private WithEvents MyProcess As Process
    Private Delegate Sub AppendOutputTextDelegate(ByVal text As String)

    Private Sub FrmCMDprompt_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Me.Width = 900
        Me.Height = 600

        Me.AcceptButton = ExecuteButton
        Me.WindowState = FormWindowState.Maximized
        MyProcess = New Process
        With MyProcess.StartInfo
            .FileName = "CMD.EXE"
            .UseShellExecute = False
            .CreateNoWindow = True
            '.WorkingDirectory = "c:\"
            .RedirectStandardInput = True
            .RedirectStandardOutput = True
            .RedirectStandardError = True
        End With
        MyProcess.Start()

        MyProcess.BeginErrorReadLine()
        MyProcess.BeginOutputReadLine()
        AppendOutputText("Process Started at: " & MyProcess.StartTime.ToString)
        AppendOutputText(vbCrLf)
        AppendOutputText(vbCrLf & "Shell Form by Sunghun, Chang")
        AppendOutputText(vbCrLf & "Hyundai Motor Group")
        AppendOutputText(vbCrLf & "Advanced Safety CAE Team")
        AppendOutputText(vbCrLf & "Research Engineer")
        AppendOutputText(vbCrLf & "T.0149")
        AppendOutputText(vbCrLf)
        AppendOutputText(vbCrLf & "Original Shell by MicroSoft" & vbCrLf)

        'MyProcess.StandardInput.WriteLine("dir")
    End Sub

    Private Sub FrmCMDprompt_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        MyProcess.StandardInput.WriteLine("EXIT") 'send an EXIT command to the Command Prompt
        MyProcess.StandardInput.Flush()
        MyProcess.Close()
    End Sub

    Private Sub AppendOutputText(ByVal text As String)
        If OutputTextBox.InvokeRequired Then
            Dim myDelegate As New AppendOutputTextDelegate(AddressOf AppendOutputText)
            Try
                Me.Invoke(myDelegate, text)
            Catch ex As Exception

            End Try
        Else
            Try
                OutputTextBox.AppendText(text)
            Catch ex As Exception

            End Try
        End If
    End Sub

    Private Sub MyProcess_ErrorDataReceived(ByVal sender As Object, ByVal e As System.Diagnostics.DataReceivedEventArgs) Handles MyProcess.ErrorDataReceived
        AppendOutputText(vbCrLf & "오류: " & e.Data)
    End Sub

    Private Sub MyProcess_OutputDataReceived(ByVal sender As Object, ByVal e As System.Diagnostics.DataReceivedEventArgs) Handles MyProcess.OutputDataReceived
        AppendOutputText(vbCrLf & e.Data)
    End Sub


    Private Sub ExecuteButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ExecuteButton.Click

        If Not Me.InputTextBox.Text = "" Then Me.InputTextBox.Items.Add(Me.InputTextBox.Text)
        MyProcess.StandardInput.WriteLine(InputTextBox.Text)
        MyProcess.StandardInput.Flush()
        If UCase(InputTextBox.Text) = "EXIT" Then Me.Close()
        If UCase(InputTextBox.Text) = "CLS" Then Me.OutputTextBox.Text = ""
        InputTextBox.Text = ""

    End Sub

    Private Sub FrmCMDprompt_Resize(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Resize
        With Me
            .InputTextBox.Width = .ClientRectangle.Width - .ExecuteButton.Width - 25
        End With
    End Sub

End Class