Imports System.Windows.Forms

Public Class ThicknessDlgFEconverting

    'FE Converting (DYNA→MADYMO)
    'Null Property를 만들기 위해 두께를 입력받는다.

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If IsNumeric(Me.TextBox1.Text) = False Then
            MsgBox("Insert Numeric Value!", MsgBoxStyle.Critical, "Not a Number")
            Me.TextBox1.Text = ""
            Exit Sub
        End If
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub ThicknessDlgFEconverting_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.TextBox1.Focus()
    End Sub
End Class
