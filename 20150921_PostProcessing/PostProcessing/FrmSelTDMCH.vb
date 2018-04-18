Public Class FrmSelTDMCH

    Public Sub New(ByVal CH_name() As String)

        ' 이 호출은 디자이너에 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하십시오.

        Dim i As Integer

        For i = 1 To UBound(CH_name)
            Me.TDMChLst.Items.Add(CH_name(i))
        Next
        Me.TDMChLst.SelectedIndex = 0


    End Sub

    Private Sub FrmSelTDMCH_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub CmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdOK.Click
        With Me
            TDM_Sel_CHGroup_Num = .TDMChLst.SelectedIndex + 1
        End With
        Me.Close()
    End Sub
End Class