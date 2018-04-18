Imports System.IO

Public Class FrmSetUpGeneral

    Dim CorrelGridSel As Integer = -1

    Private Sub FrmSetUpGeneral_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
        MainMDI.Statuslbl.Text = "General Set-Up Window is Closed."
    End Sub

    Private Sub FrmSetUpGeneral_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        'Me.AcceptButton = Me.BtnSave
        Me.Size = New Size(320, 500)
        MainMDI.Statuslbl.Text = "Open General Set-Up"

        With Me
            With .CmbH3
                .Items.Add("US-NCAP")
                .Items.Add("DOM-Frontal")
                .Items.Add("DOM-Offset")
                .Items.Add("China-Frontal")
                .Items.Add("China-Offset")
                .Items.Add("China-Rear")
                .Items.Add("Euro-Frontal")
                .Items.Add("Euro-Offset")
            End With
            With .CmbTHOR
                .Items.Add("US-NCAP [THOR]")
            End With
            With .CorrelWeightGrid
                .set_ColWidth(0, 3200)
                .set_ColWidth(1, 1000)
                .set_ColAlignment(0, 4)
                .set_ColAlignment(1, 4)
                .set_TextMatrix(0, 0, "DUC [Factor]")
                .set_TextMatrix(1, 0, "DUC [Relative]")
                .set_TextMatrix(2, 0, "WIFac")
                .set_TextMatrix(3, 0, "S&G")
                .set_TextMatrix(4, 0, "S&G-Magnitude")
                .set_TextMatrix(5, 0, "S&G-Phase")
                .set_TextMatrix(6, 0, "GPV")
                .set_TextMatrix(7, 0, "GPT")
            End With
        End With

        '파일 확인
        If Not System.IO.File.Exists(Application.StartupPath & "\DATA\General.setup") Then
            Dim NewParaFile As StreamWriter
            NewParaFile = New StreamWriter(Application.StartupPath & "\DATA\General.setup")
            '파일 쓰기
            NewParaFile.WriteLine("H3\0")
            NewParaFile.WriteLine("THOR\0")
            NewParaFile.WriteLine("POPUP_WIDTH\490")
            NewParaFile.WriteLine("DUC_F\1")
            NewParaFile.WriteLine("DUC_R\1")
            NewParaFile.WriteLine("WIFac\1")
            NewParaFile.WriteLine("SnG\1")
            NewParaFile.WriteLine("SnG_M\1")
            NewParaFile.WriteLine("SnG_P\1")
            NewParaFile.WriteLine("GPV\1")
            NewParaFile.WriteLine("GPT\1")
            NewParaFile.Close()
        End If

        '파일 읽기
        Dim SetupFile As StreamReader
        'InjuryFile = New StreamReader(Me.GraphBasedDlg.FileName)
        SetupFile = New StreamReader(Application.StartupPath & "\DATA\General.setup")
        Dim Tmp_Str() As String

        Tmp_Str = SetupFile.ReadLine.Split("\")
        Me.CmbH3.SelectedIndex = CInt(Tmp_Str(1))
        Tmp_Str = SetupFile.ReadLine.Split("\")
        Me.CmbTHOR.SelectedIndex = CInt(Tmp_Str(1))
        Tmp_Str = SetupFile.ReadLine.Split("\")
        PopUpWidth = CInt(Tmp_Str(1))
        Me.TxtWidth.Text = PopUpWidth
        Tmp_Str = SetupFile.ReadLine.Split("\")
        PopUpHeight = CInt(Tmp_Str(1))
        Me.TxtHeight.Text = PopUpHeight

        Tmp_Str = SetupFile.ReadLine.Split("\")
        Me.CorrelWeightGrid.set_TextMatrix(0, 1, Format(CDbl(Tmp_Str(1)), "#0.0#"))
        Tmp_Str = SetupFile.ReadLine.Split("\")
        Me.CorrelWeightGrid.set_TextMatrix(1, 1, Format(CDbl(Tmp_Str(1)), "#0.0#"))
        Tmp_Str = SetupFile.ReadLine.Split("\")
        Me.CorrelWeightGrid.set_TextMatrix(2, 1, Format(CDbl(Tmp_Str(1)), "#0.0#"))
        Tmp_Str = SetupFile.ReadLine.Split("\")
        Me.CorrelWeightGrid.set_TextMatrix(3, 1, Format(CDbl(Tmp_Str(1)), "#0.0#"))
        Tmp_Str = SetupFile.ReadLine.Split("\")
        Me.CorrelWeightGrid.set_TextMatrix(4, 1, Format(CDbl(Tmp_Str(1)), "#0.0#"))
        Tmp_Str = SetupFile.ReadLine.Split("\")
        Me.CorrelWeightGrid.set_TextMatrix(5, 1, Format(CDbl(Tmp_Str(1)), "#0.0#"))
        Tmp_Str = SetupFile.ReadLine.Split("\")
        Me.CorrelWeightGrid.set_TextMatrix(6, 1, Format(CDbl(Tmp_Str(1)), "#0.0#"))
        Tmp_Str = SetupFile.ReadLine.Split("\")
        Me.CorrelWeightGrid.set_TextMatrix(7, 1, Format(CDbl(Tmp_Str(1)), "#0.0#"))
        SetupFile.Close()

    End Sub

    Private Sub FrmSetUpGeneral_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        With Me
            .NCAPsetting.Location = New Point(5, 10)
            .NCAPsetting.Size = New Size(Me.ClientRectangle.Width - 10, 100)
            .PopupSetting.Location = New Point(.NCAPsetting.Location.X, .NCAPsetting.Location.Y + .NCAPsetting.Height + 10)
            .PopupSetting.Size = New Size(Me.NCAPsetting.Width, 75) 'Me.ClientRectangle.Height - .NCAPsetting.Height - 30 - .BtnSave.Height - 5)
            .CorrelSetting.Location = New Point(.PopupSetting.Location.X, .PopupSetting.Location.Y + .PopupSetting.Height + 10)
            .CorrelSetting.Size = New Size(Me.NCAPsetting.Width, Me.ClientRectangle.Height - .NCAPsetting.Height - .PopupSetting.Height - 30 - .BtnSave.Height - 10)
            .CorrelWeightGrid.Location = New Point(5, 15)
            .CorrelWeightGrid.Size = New Size(.CorrelSetting.ClientRectangle.Width - 10, .CorrelSetting.ClientRectangle.Height - 50)
            .CorelTxt.Location = New Point(.CorrelWeightGrid.Location.X + .CorrelWeightGrid.Width - 150, .CorrelWeightGrid.Location.Y + .CorrelWeightGrid.Height + 5)
            .CorelTxt.Size = New Size(150, 23)
            .BtnSave.Location = New Point(.CorrelSetting.Location.X + .CorrelSetting.Width - 100, .CorrelSetting.Location.Y + .CorrelSetting.Height + 5)

            .Label1.Location = New Point(5, 20)
            .Label2.Location = New Point(21, 44)
            .Label3.Location = New Point(.Label2.Location.X, 73)
            .CmbH3.Location = New Point(90, 41)
            .CmbH3.Width = .NCAPsetting.ClientRectangle.Width - .CmbH3.Location.Y - 60
            .CmbTHOR.Location = New Point(.CmbH3.Location.X, 70)
            .CmbTHOR.Width = .CmbH3.Width

            .Label4.Location = New Point(.Label2.Location.X, 20)
            .Label5.Location = New Point(.Label2.Location.X, 49)
            .TxtWidth.Location = New Point(.CmbH3.Location.X, .Label4.Location.Y - 3)
            .TxtHeight.Location = New Point(.TxtWidth.Location.X, .Label5.Location.Y - 3)
            .TxtWidth.Width = 130
            .TxtHeight.Width = 130
        End With
    End Sub

    Private Sub BtnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        If IsNumeric(Me.TxtWidth.Text) = False Or IsNumeric(Me.TxtHeight.Text) = False Then
            MsgBox("Numeric Value Only!!", MsgBoxStyle.Information, "Try Again")
            MainMDI.Statuslbl.Text = "Numeric Value Only!!"
            Exit Sub
        End If

        StartUpTab_NCAP = Me.CmbH3.SelectedIndex
        StartUpTab_THOR = Me.CmbTHOR.SelectedIndex
        PopUpWidth = CInt(Me.TxtWidth.Text)
        PopUpHeight = CInt(Me.TxtHeight.Text)

        '파일 저장
        Dim NewParaFile As StreamWriter
        'NewParaFile = New StreamWriter(Me.CorrelSaveFile.FileName)
        NewParaFile = New StreamWriter(Application.StartupPath & "\DATA\General.setup")

        '파일 쓰기
        NewParaFile.WriteLine("H3\" & StartUpTab_NCAP)
        NewParaFile.WriteLine("THOR\" & StartUpTab_THOR)
        NewParaFile.WriteLine("POPUP_WIDTH\" & PopUpWidth)
        NewParaFile.WriteLine("POPUP_HEIGHT\" & PopUpHeight)

        NewParaFile.WriteLine("DUC_F\" & Me.CorrelWeightGrid.get_TextMatrix(0, 1))
        NewParaFile.WriteLine("DUC_R\" & Me.CorrelWeightGrid.get_TextMatrix(1, 1))
        NewParaFile.WriteLine("WIFac\" & Me.CorrelWeightGrid.get_TextMatrix(2, 1))
        NewParaFile.WriteLine("SnG\" & Me.CorrelWeightGrid.get_TextMatrix(3, 1))
        NewParaFile.WriteLine("SnG_M\" & Me.CorrelWeightGrid.get_TextMatrix(4, 1))
        NewParaFile.WriteLine("SnG_P\" & Me.CorrelWeightGrid.get_TextMatrix(5, 1))
        NewParaFile.WriteLine("GPV\" & Me.CorrelWeightGrid.get_TextMatrix(6, 1))
        NewParaFile.WriteLine("GPT\" & Me.CorrelWeightGrid.get_TextMatrix(7, 1))

        NewParaFile.Close()

        MainMDI.Statuslbl.Text = "Setting Complete.."
    End Sub

    Private Sub CorrelWeightGrid_ClickEvent(ByVal sender As Object, ByVal e As System.EventArgs) Handles CorrelWeightGrid.ClickEvent
        With Me
            CorrelGridSel = .CorrelWeightGrid.RowSel
            .CorelTxt.Text = .CorrelWeightGrid.get_TextMatrix(.CorrelWeightGrid.RowSel, 1)
            .CorelTxt.Focus()
        End With
    End Sub

    Private Sub CorelTxt_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles CorelTxt.KeyDown

        If CorrelGridSel = -1 Then Exit Sub

        If e.KeyCode = Keys.Enter Then
            With Me
                If IsNumeric(CDbl(Me.CorelTxt.Text)) Then
                    .CorrelWeightGrid.set_TextMatrix(CorrelGridSel, 1, Me.CorelTxt.Text)
                Else
                    Exit Sub
                End If
            End With
        End If
    End Sub

End Class