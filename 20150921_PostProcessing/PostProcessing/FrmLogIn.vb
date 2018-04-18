Public Class FrmLogIn

    Private Sub FrmLogIn_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.TxtID.Text = Environment.UserName.ToString
        Me.AcceptButton = Me.Button1

        SessionInfor(0) = ""
        SessionInfor(1) = ""
        SessionInfor(2) = ""
        SessionInfor(3) = ""
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If Me.TxtIP.Text <> "" And Me.TxtID.Text <> "" And Me.TxtPW.Text <> "" And Me.TxtKey.Text <> "" Then
        Else
            MsgBox("Insert Informations", MsgBoxStyle.Critical, "Try Again")
            Exit Sub
        End If

        SessionInfor(0) = Me.TxtIP.Text
        SessionInfor(1) = Me.TxtID.Text
        SessionInfor(2) = Me.TxtPW.Text
        If Me.TxtKey.Text = "Any SSH Key" Then
            SessionInfor(3) = "Any SSH Key"
        Else
            SessionInfor(3) = Me.TxtKey.Text
        End If

        Me.Close()
    End Sub
End Class