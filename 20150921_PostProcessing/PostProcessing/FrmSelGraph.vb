Public Class FrmSelGraph

    Private Sub FrmSelGraph_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim i As Integer = 0

        With Me
            .CmbDR.Items.Add("DATA Value Drop")
            .CmbDR.Items.Add("DATA Value Rise")
            .CmbDR.SelectedIndex = 0
            For i = 0 To UBound(GraphTitle) - 1
                .LstGraph.Items.Add(GraphTitle(i))
            Next
            .LstGraph.SelectedIndex = 0
        End With
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        'SelGraphDetail
        'Index 0 : 그래프 번호 
        'Index 1 : Drop/Rise
        'Index 2 : 타임윈도우
        'Index 3 : Different값

        If IsNumeric(Me.TxtTime.Text) Then
        Else
            MsgBox("Numeric Only!!", MsgBoxStyle.Critical, "Wrong Value")
            Exit Sub
        End If
        If IsNumeric(Me.TxtDiffVal.Text) Then
        Else
            MsgBox("Numeric Only!!", MsgBoxStyle.Critical, "Wrong Value")
            Exit Sub
        End If

        SelGraphDetail(0) = Me.LstGraph.SelectedIndex
        SelGraphDetail(1) = Me.CmbDR.SelectedIndex + 1
        SelGraphDetail(2) = CDbl(Me.TxtTime.Text)
        SelGraphDetail(3) = CDbl(Me.TxtDiffVal.Text)

        Me.Close()
    End Sub

    Private Sub FrmSelGraph_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseDoubleClick
        'SelGraphDetail
        'Index 0 : 그래프 번호 
        'Index 1 : Drop/Rise
        'Index 2 : 타임윈도우
        'Index 3 : Different값

        If IsNumeric(Me.TxtTime.Text) Then
        Else
            MsgBox("Numeric Only!!", MsgBoxStyle.Critical, "Wrong Value")
            Exit Sub
        End If
        If IsNumeric(Me.TxtDiffVal.Text) Then
        Else
            MsgBox("Numeric Only!!", MsgBoxStyle.Critical, "Wrong Value")
            Exit Sub
        End If

        SelGraphDetail(0) = Me.LstGraph.SelectedIndex
        SelGraphDetail(1) = Me.CmbDR.SelectedIndex + 1
        SelGraphDetail(2) = CDbl(Me.TxtTime.Text)
        SelGraphDetail(3) = CDbl(Me.TxtDiffVal.Text)

        Me.Close()
    End Sub
End Class