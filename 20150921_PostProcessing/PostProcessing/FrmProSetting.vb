Imports System.IO

Public Class ProfileSetting

    '파라미터 임시 저장 변수
    Dim Ext_Para() As String
    Dim Col_Para() As String
    Dim Row_Para() As String
    Dim CFactor_Para() As Decimal

    'TEST
    Dim TESTdummyPara As String

    '이벤트를 발생시키지 않기위한 불리언 변수
    Dim EvenPass As Boolean

    Private Sub ProfileSetting_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        MainMDI.Statuslbl.Text = "Status"
    End Sub

    Private Sub ProfileSetting_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        MainMDI.Statuslbl.Text = "Profile Setting [Analysis/TEST/Graph Title etc.]"
        'Path를 채워준다.=========================================================================
        Dim NewPathFile As StreamReader
        Dim ProgramFolder64 As String
        Dim TmpPath() As String
        NewPathFile = New StreamReader(Application.StartupPath & "\DATA\EnvironPATH_HW.dat")
        TmpPath = NewPathFile.ReadLine().Split("=")
        ProgramFolder64 = TmpPath(UBound(TmpPath))
        'ProgramFolder64 = "C:\Program Files\Altair\12.0\hw\bin\win64\hw.exe"
        NewPathFile.Close()
        Me.TxtPathHW.Text = ProgramFolder64

        NewPathFile = New StreamReader(Application.StartupPath & "\DATA\EnvironPATH.dat")
        TmpPath = NewPathFile.ReadLine().Split("=")
        ProgramFolder64 = TmpPath(UBound(TmpPath))
        NewPathFile.Close()
        Me.TxtPath.Text = ProgramFolder64
        '========================================================================================

        MainMDI.ProgressBarMain.Maximum = 1100

        MainMDI.ProgressBarMain.Value = 100

        Me.Width = 900
        Me.Height = 500

        EvenPass = False

        Dim i As Integer = 0
        Dim ParaFile As StreamReader
        Dim Tmp_Str() As String
        Dim ColRow_Str() As String


        ReDim Ext_Para(31)
        ReDim Col_Para(31)
        ReDim Row_Para(31)
        ReDim CFactor_Para(31)

        With Me
            .MdiParent = MainMDI
            .TabPage1.Text = "Analysis[Hybid-Ⅲ]"
            '.TabPage2.Text = "해석 데이터[THOR]"
            .TabPage3.Text = "TEST[DAT-R64/tdm-tdx]"
            .TabPage4.Text = "Graph Titles"
            .TabPage2.Text = "Correl. Factor"
            .TabPage5.Text = "Path"
            .TabPage6.Text = "Graph Based Correl."

            .SetUpTab.Location = New Point(10, 10)
            .SetUpTab.Size = New Point(.ClientRectangle.Width - 20, .ClientRectangle.Height - 20)

            MainMDI.ProgressBarMain.Value = 200

            '파일을 열고 파라미터를 읽어온다.
            '해석 H-3
            ParaFile = New StreamReader(Application.StartupPath & "\Profile\ParameterSetting.Set")
            For i = 0 To 31

                MainMDI.ProgressBarMain.Value = 200 + i * 25

                Tmp_Str = ParaFile.ReadLine.Split("=")
                Ext_Para(i) = Tmp_Str(LBound(Tmp_Str))
                ColRow_Str = Tmp_Str(UBound(Tmp_Str)).Split("\")
                Col_Para(i) = ColRow_Str(LBound(ColRow_Str))
                Row_Para(i) = ColRow_Str(LBound(ColRow_Str) + 1)
                CFactor_Para(i) = CDec(ColRow_Str(LBound(ColRow_Str) + 2))

                .ExtLst.Items.Add(i + 1 & ". " & Ext_Para(i))
            Next

            MainMDI.ProgressBarMain.Value = 900

            ParaFile.Close()

            MainMDI.ProgressBarMain.Value = 1000

            'GraphTitle 읽어들인다 (글로벌 변수로부터 읽어온다)

            With Me
                For i = 0 To 31 Step 1
                    .GraphLblLst.Items.Add(i + 1 & ". " & GraphTitle(i))
                    .TESTLblLst.Items.Add(i + 1 & ". " & GraphTitle(i))
                    .GraphScaleLst.set_TextMatrix(i, 0, GraphTitle(i))
                Next
                .GraphScaleLst.set_ColAlignment(0, 4)
                .GraphScaleLst.set_ColAlignment(1, 4)
                .InjuryScaleLst.set_ColAlignment(0, 4)
                .InjuryScaleLst.set_ColAlignment(1, 4)
            End With

            '상해 이름들을 읽어온다 (상관성 탭 라벨)
            .InjuryScaleLst.set_TextMatrix(0, 0, "Head3MSG")                          'H3MS_inj
            .InjuryScaleLst.set_TextMatrix(1, 0, "Chest_G_CUMULATIVE_T3MS_inj")       'T3MS_inj
            .InjuryScaleLst.set_TextMatrix(2, 0, "HIC15")             'HIC15_inj
            .InjuryScaleLst.set_TextMatrix(3, 0, "HIC36")             'HIC36_inj
            .InjuryScaleLst.set_TextMatrix(4, 0, "NTE")               'NTE_inj
            .InjuryScaleLst.set_TextMatrix(5, 0, "NTF")               'NTF_inj
            .InjuryScaleLst.set_TextMatrix(6, 0, "NCE")               'NCE_inj
            .InjuryScaleLst.set_TextMatrix(7, 0, "NCF")               'NCF_inj
            .InjuryScaleLst.set_TextMatrix(8, 0, "Head_Peak_G")       'HaccRpeak_inj
            .InjuryScaleLst.set_TextMatrix(9, 0, "Chest_D")           'ThCC_inj
            .InjuryScaleLst.set_TextMatrix(10, 0, "KneeS_L")           'kneesliderL_inj
            .InjuryScaleLst.set_TextMatrix(11, 0, "KneeS_R")           'kneesliderR_inj
            .InjuryScaleLst.set_TextMatrix(12, 0, "Tibia_Comp_L")      'TCFCLowL_inj
            .InjuryScaleLst.set_TextMatrix(13, 0, "Tibia_Comp_R")      'TCFCLowR_inj
            .InjuryScaleLst.set_TextMatrix(14, 0, "TI_upr_L")          'TIUpL_inj
            .InjuryScaleLst.set_TextMatrix(15, 0, "TI_lwr_L")          'TILowL_inj
            .InjuryScaleLst.set_TextMatrix(16, 0, "TI_upr_R")          'TIUpR_inj
            .InjuryScaleLst.set_TextMatrix(17, 0, "TI_lwr_R")          'TILowR_inj
            .InjuryScaleLst.set_TextMatrix(18, 0, "Chest_VC")          'VC_inj_CFC180
            .InjuryScaleLst.set_TextMatrix(19, 0, "FemurL")            'FFCL_inj
            .InjuryScaleLst.set_TextMatrix(20, 0, "FemurR")            'FFCR_inj
            .InjuryScaleLst.set_TextMatrix(21, 0, "Neck_Comp")         'FNICtension_inj
            .InjuryScaleLst.set_TextMatrix(22, 0, "Neck_Tens")         'FNICtension_inj
            .InjuryScaleLst.set_TextMatrix(23, 0, "Neck_Shear")        'FNICshear_inj
            .InjuryScaleLst.set_TextMatrix(24, 0, "Neck_Exten")        'FNICbending_inj
            '============================== For THOR 50% ATD Injury Values ==============================
            .InjuryScaleLst.set_TextMatrix(25, 0, "ThxIrUpL")           'ThxIrTraccRibL_CFC600_dis
            .InjuryScaleLst.set_TextMatrix(26, 0, "ThxIrUpR")           'ThxIrTraccRibR_CFC600_dis
            .InjuryScaleLst.set_TextMatrix(27, 0, "ThxIrLowL")          'ThxLowIrTraccRibL_CFC600_dis
            .InjuryScaleLst.set_TextMatrix(28, 0, "ThxIrLowR")          'ThxLowIrTraccRibR_CFC600_dis

            MainMDI.ProgressBarMain.Value = 1100

            For i = 0 To 31
                .CmbHICgraph.Items.Add(GraphTitle(i))
                .CmbFxGraph.Items.Add(GraphTitle(i))
                .CmbFzGraph.Items.Add(GraphTitle(i))
                .CmbMyGraph.Items.Add(GraphTitle(i))
                .CmbMocyGraph.Items.Add(GraphTitle(i))
                .CmbNTE.Items.Add(GraphTitle(i))
                .CmbNTF.Items.Add(GraphTitle(i))
                .CmbNCE.Items.Add(GraphTitle(i))
                .CmbNCF.Items.Add(GraphTitle(i))
            Next
            .CmbHICgraph.Items.Add("Not Used")
            .CmbFxGraph.Items.Add("Not Used")
            .CmbFzGraph.Items.Add("Not Used")
            .CmbMyGraph.Items.Add("Not Used")
            .CmbMocyGraph.Items.Add("Not Used")
            .CmbNTE.Items.Add("Not Used")
            .CmbNTF.Items.Add("Not Used")
            .CmbNCE.Items.Add("Not Used")
            .CmbNCF.Items.Add("Not Used")
            .CmbHICgraph.SelectedIndex = 32
            .CmbFxGraph.SelectedIndex = 32
            .CmbFzGraph.SelectedIndex = 32
            .CmbMyGraph.SelectedIndex = 32
            .CmbMocyGraph.SelectedIndex = 32
            .CmbNTE.SelectedIndex = 32
            .CmbNTF.SelectedIndex = 32
            .CmbNCE.SelectedIndex = 32
            .CmbNCF.SelectedIndex = 32

        End With
    End Sub

    Private Sub ProfileSetting_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize

        On Error Resume Next

        With Me
            .SetUpTab.Location = New Point(10, 10)
            .SetUpTab.Size = New Size(.ClientRectangle.Width - 20, .ClientRectangle.Height - 20)
            .ExtLst.Location = New Point(5, 5)
            .ExtLst.Size = New Size(100, .SetUpTab.ClientRectangle.Height - 21 - 15 - 31 - 30)
            .ColLst.Location = New Point(110, 5)
            .ColLst.Size = New Size((SetUpTab.ClientRectangle.Width - 130) / 2, .ExtLst.Height)
            .RowLst.Location = New Point(115 + .ColLst.Width, 5)
            .RowLst.Size = New Size(.ColLst.Width, .ColLst.Height)
            .ExtTxt.Location = New Point(5, .ExtLst.Height + 10)
            .ExtTxt.Size = New Size(.ExtLst.Width, 21)
            .CFactorTxt.Location = New Point(5, .ExtTxt.Location.Y + 21 + 5)
            .CFactorTxt.Size = New Size(.ExtLst.Width, 21)
            .CFactorLbl.Location = New Point(.CFactorTxt.Location.X + .CFactorTxt.Width + 5, .CFactorTxt.Location.Y + 3)
            .ColTxt.Location = New Point(.ColLst.Location.X, .ColLst.Height + 10)
            .ColTxt.Size = New Size(.ColLst.Width, 21)
            .RowTxt.Location = New Point(.RowLst.Location.X, .RowLst.Height + 10)
            .RowTxt.Size = New Size(.RowLst.Width, 21)
            .AddBtn.Location = New Point(.SetUpTab.ClientRectangle.Width - 220, .SetUpTab.ClientRectangle.Height - 31 - 30)
            .AddBtn.Size = New Size(100, 31)
            .SaveBtn.Location = New Point(.AddBtn.Location.X + 100 + 5, .AddBtn.Location.Y)
            .SaveBtn.Size = New Size(100, .AddBtn.Height)

            .GraphLblLst.Location = New Point(5, 5)
            .GraphLblLst.Size = New Size(.SetUpTab.ClientRectangle.Width / 2, .SetUpTab.ClientRectangle.Height - 21 - 15)
            .GraphLblTxt.Location = New Point(.GraphLblLst.Location.X + .GraphLblLst.Width + 5, 5)
            .GraphLblTxt.Size = New Size(.SetUpTab.ClientRectangle.Width / 2 - 25, 21)
            .BtnLblReplace.Location = New Point(.SetUpTab.ClientRectangle.Width - 220, .GraphLblTxt.Location.Y + 21 + 5)
            .BtnLblReplace.Size = New Size(100, 31)
            .BtnLblSave.Location = New Point(.BtnLblReplace.Location.X + 100 + 5, .BtnLblReplace.Location.Y)
            .BtnLblSave.Size = New Size(100, .BtnLblReplace.Height)

            .TESTLblLst.Location = .ExtLst.Location
            .TESTLblLst.Size = New Size((.SetUpTab.ClientRectangle.Width - 10) * 0.3, .ExtLst.Height)
            .TESTXLst.Location = New Point(.TESTLblLst.Location.X + .TESTLblLst.Width + 5, .TESTLblLst.Location.Y)
            .TESTXLst.Size = New Size((.SetUpTab.ClientRectangle.Width - 10) * 0.2 - 10, .TESTLblLst.Height)
            .TESTYLst.Location = New Point(.TESTXLst.Location.X + .TESTXLst.Width + 5, .TESTLblLst.Location.Y)
            .TESTYLst.Size = New Size(.SetUpTab.ClientRectangle.Width - .TESTYLst.Location.X - 15, .TESTLblLst.Height)
            .TESTLblTxt.Location = .ExtTxt.Location
            .TESTLblTxt.Size = New Size(.TESTLblLst.Width, .ExtTxt.Height)
            .TESTXTxt.Location = New Point(.TESTXLst.Location.X, .TESTLblTxt.Location.Y)
            .TESTXTxt.Size = New Size(.TESTXLst.Width, .TESTXTxt.Height)
            .TESTYTxt.Location = New Point(.TESTYLst.Location.X, .TESTLblTxt.Location.Y)
            .TESTYTxt.Size = New Size(.TESTYLst.Width, .TESTXTxt.Height)
            .TESTaddBtn.Location = .AddBtn.Location
            .TESTaddBtn.Size = .AddBtn.Size
            .TESTSaveBtn.Location = .SaveBtn.Location
            .TESTSaveBtn.Size = .SaveBtn.Size
            .Label1.Location = .CFactorTxt.Location
            .Label1.Size = New Size(45, .CFactorTxt.Height)
            .Label2.Size = New Size(45, .CFactorTxt.Height)
            .Label3.Size = New Size(45, .CFactorTxt.Height)
            .TESTIdenTxt1.Location = New Point(.Label1.Location.X + .Label1.Width, .Label1.Location.Y)
            .TESTIdenTxt1.Size = New Size(150, .CFactorTxt.Height)
            .Label2.Location = New Point(.TESTIdenTxt1.Location.X + .TESTIdenTxt1.Width, .TESTIdenTxt1.Location.Y)
            .TESTIdenTxt2.Location = New Point(.Label2.Location.X + .Label2.Width, .Label2.Location.Y)
            .TESTIdenTxt2.Size = New Size(150, .TESTIdenTxt1.Height)
            .Label3.Location = New Point(.TESTIdenTxt2.Location.X + .TESTIdenTxt2.Width, .TESTIdenTxt2.Location.Y)
            .TESTIdenTxt3.Location = New Point(.Label3.Location.X + .Label3.Width, .Label3.Location.Y)
            .TESTIdenTxt3.Size = New Size(150, .TESTIdenTxt2.Height)

            .GraphScaleLst.Location = .GraphLblLst.Location
            .GraphScaleLst.Size = New Size(.SetUpTab.ClientRectangle.Width / 2 - 10, .SetUpTab.ClientRectangle.Height - 26 - 28 - 45)
            .SelectedGraphTxt.Location = New Point(5, .GraphScaleLst.Top + .GraphScaleLst.Height + 5)
            .SelectedGraphTxt.Size = New Size(.GraphScaleLst.Width * 0.7, 26)
            .GraphScaleVal.Location = New Point(.SelectedGraphTxt.Left + .SelectedGraphTxt.Width, .SelectedGraphTxt.Top)
            .GraphScaleVal.Size = New Size(.GraphScaleLst.Width - .SelectedGraphTxt.Width, 26)
            .InjuryScaleLst.Location = New Point(.GraphScaleLst.Left + .GraphScaleLst.Width, 5)
            .InjuryScaleLst.Size = New Size(.GraphScaleLst.Width, .GraphScaleLst.Height)
            .SelectedInjuryTxt.Location = New Point(.InjuryScaleLst.Left, .GraphScaleVal.Top)
            .SelectedInjuryTxt.Size = New Size(.InjuryScaleLst.Width * 0.7, 26)
            .InjuryScaleVal.Location = New Point(.SelectedInjuryTxt.Left + .SelectedInjuryTxt.Width, .SelectedInjuryTxt.Top)
            .InjuryScaleVal.Size = New Size(.InjuryScaleLst.Width - .SelectedInjuryTxt.Width, 26)
            .OpenCorrel.Location = New Point(.SetUpTab.ClientRectangle.Width - 240 - 10 - 15, .SelectedGraphTxt.Top + 26 + 5)
            .OpenCorrel.Size = New Size(120, 28)
            .SaveCorrel.Location = New Point(.OpenCorrel.Left + OpenCorrel.Width + 10, .OpenCorrel.Top)
            .SaveCorrel.Size = New Size(120, 28)
            .GraphScaleLst.set_ColWidth(1, 1080)
            .GraphScaleLst.set_ColWidth(0, CInt(Me.GraphScaleLst.Width * 14 - .GraphScaleLst.get_ColWidth(1)))
            .InjuryScaleLst.set_ColWidth(1, 1080)
            .InjuryScaleLst.set_ColWidth(0, CInt(Me.InjuryScaleLst.Width * 14 - .InjuryScaleLst.get_ColWidth(1)))

            .LblXMADgic.Location = New Point(10, 14)
            .LblXMADgic.Size = New Size(105, 15)
            .TxtPath.Location = New Point(.LblXMADgic.Location.X + .LblXMADgic.Width + 10, 10)
            .TxtPath.Size = New Size(.SetUpTab.ClientRectangle.Width - 20 - 90 - 83 - 30, 23)
            .BtnFindFolder.Size = New Size(90, 23)
            .BtnFindFolder.Location = New Point(.TxtPath.Location.X + .TxtPath.Width + 10, .TxtPath.Location.Y)
            .LblHW.Location = New Point(10, .LblXMADgic.Location.Y + .LblXMADgic.Height + 15)
            .LblHW.Size = New Size(.LblXMADgic.Width, .LblXMADgic.Height)
            .TxtPathHW.Location = New Point(.TxtPath.Location.X, .LblHW.Location.Y - 4)
            .TxtPathHW.Size = New Size(.SetUpTab.ClientRectangle.Width - 20 - 90 - 83 - 30, 23)
            .BtnFindFolderHW.Size = New Size(90, 23)
            .BtnFindFolderHW.Location = New Point(.TxtPathHW.Location.X + .TxtPathHW.Width + 10, .TxtPathHW.Location.Y)

            .CmbHICgraph.Size = New Size(250, 28)
            .CmbFxGraph.Size = .CmbHICgraph.Size
            .CmbFzGraph.Size = .CmbHICgraph.Size
            .CmbMyGraph.Size = .CmbHICgraph.Size
            .CmbMocyGraph.Size = .CmbHICgraph.Size
            .CmbNTE.Size = .CmbHICgraph.Size
            .CmbNTF.Size = .CmbHICgraph.Size
            .CmbNCE.Size = .CmbHICgraph.Size
            .CmbNCF.Size = .CmbHICgraph.Size

            .CmbHICgraph.Location = New Point(156, 28)
            .CmbFxGraph.Location = New Point(.CmbHICgraph.Location.X, .CmbHICgraph.Location.Y + 28 + 5)
            .CmbFzGraph.Location = New Point(.CmbFxGraph.Location.X, .CmbFxGraph.Location.Y + 28 + 5)
            .CmbMyGraph.Location = New Point(.CmbFzGraph.Location.X, .CmbFzGraph.Location.Y + 28 + 5)
            .CmbMocyGraph.Location = New Point(.CmbMyGraph.Location.X, .CmbMyGraph.Location.Y + 28 + 5)
            .CmbNTE.Location = New Point(.CmbMocyGraph.Location.X, .CmbMocyGraph.Location.Y + 28 + 5)
            .CmbNTF.Location = New Point(.CmbNTE.Location.X, .CmbNTE.Location.Y + 28 + 5)
            .CmbNCE.Location = New Point(.CmbNTF.Location.X, .CmbNTF.Location.Y + 28 + 5)
            .CmbNCF.Location = New Point(.CmbNCE.Location.X, .CmbNCE.Location.Y + 28 + 5)

            .BtnGbasedOpen.Location = New Point(.CmbNCF.Location.X, .CmbNCF.Location.Y + .CmbNCF.Height + 15)
            .BtnGbasedOpen.Size = New Size(.CmbNCF.Width / 2 - 5, 35)
            .BtnApplyNSave.Location = New Point(.BtnGbasedOpen.Location.X + .BtnGbasedOpen.Width + 10, .CmbNCF.Location.Y + .CmbNCF.Height + 15)
            .BtnApplyNSave.Size = .BtnGbasedOpen.Size

            .Label4.Location = New Point(.CmbHICgraph.Location.X - 150, .CmbHICgraph.Location.Y + 3)
            .Label5.Location = New Point(.Label4.Location.X, .CmbFxGraph.Location.Y + 3)
            .Label6.Location = New Point(.Label4.Location.X, .CmbFzGraph.Location.Y + 3)
            .Label7.Location = New Point(.Label4.Location.X, .CmbMyGraph.Location.Y + 3)
            .Label8.Location = New Point(.Label4.Location.X, .CmbMocyGraph.Location.Y + 3)
            .Label9.Location = New Point(.Label4.Location.X, .CmbNTE.Location.Y + 3)
            .Label10.Location = New Point(.Label4.Location.X, .CmbNTF.Location.Y + 3)
            .Label11.Location = New Point(.Label4.Location.X, .CmbNCE.Location.Y + 3)
            .Label12.Location = New Point(.Label4.Location.X, .CmbNCF.Location.Y + 3)

            .Label13.Location = New Point(.CmbHICgraph.Location.X + .CmbHICgraph.Width + 20, .Label4.Location.Y)
        End With
    End Sub

    Private Sub ExtLst_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExtLst.SelectedIndexChanged

        If EvenPass = False Then

            Dim i As Integer
            Dim Tmp_Str() As String

            '기존의 목록을 삭제한다.
            For i = Me.ColLst.Items.Count To 1 Step -1
                Me.ColLst.Items.RemoveAt(i - 1)
            Next
            For i = Me.RowLst.Items.Count To 1 Step -1
                Me.RowLst.Items.RemoveAt(i - 1)
            Next

            '파라미터를 읽어와서 리스트에 써준다. 'Column 파라미터
            Tmp_Str = Col_Para(Me.ExtLst.SelectedIndex).Split(",")
            For i = 1 To Tmp_Str.Length
                Me.ColLst.Items.Add(Tmp_Str(i - 1))
            Next

            '파라미터를 읽어와서 리스트에 써준다. 'Row 파라미터
            Tmp_Str = Row_Para(Me.ExtLst.SelectedIndex).Split(",")
            For i = 1 To Tmp_Str.Length
                Me.RowLst.Items.Add(Tmp_Str(i - 1))
            Next

            Tmp_Str = Me.ExtLst.Items(Me.ExtLst.SelectedIndex).ToString.Split(".")
            Me.ExtTxt.Text = LTrim(Tmp_Str(UBound(Tmp_Str)))
            Me.CFactorTxt.Text = CFactor_Para(Me.ExtLst.SelectedIndex)
            'Me.ColTxt.Text = Me.ColLst.Items(Me.ColLst.SelectedIndex)
            'Me.RowTxt.Text = Me.RowLst.Items(Me.RowLst.SelectedIndex)

        End If
    End Sub

    Private Sub AddBtn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddBtn.Click

        Dim IndexN As Integer = Me.ExtLst.SelectedIndex

        'Conversion Factor에 숫자가 아니면 그냥 루틴을 그만둔다.
        If IsNumeric(Me.CFactorTxt.Text) = False Then
            MsgBox("수치로 입력 : Unit Conversion Factor", MsgBoxStyle.Critical, "잘못된 데이터")
            Exit Sub
        End If

        If ExtTxt.Text <> "" Then '확장자가 비워져있지 않을 때만 실행한다.

            '확장자 교체 ========================================================================
            If Me.ExtTxt.Text <> Ext_Para(IndexN) Then
                '이벤트를 발생시키지 않기위한 불리언 변수
                Ext_Para(IndexN) = Me.ExtTxt.Text
                EvenPass = True
                Me.ExtLst.Items.RemoveAt(IndexN)
                Me.ExtLst.Items.Insert(IndexN, IndexN + 1 & ". " & Me.ExtTxt.Text)
                '(Me.ExtLst.SelectedIndex).Replace(Me.ExtLst.Items(Me.ExtLst.SelectedIndex).ToString, Me.ExtTxt.Text)
                EvenPass = False

                Me.ExtLst.SelectedItem = Me.ExtLst.Items(IndexN)
            End If
            '====================================================================================


            'Col / Row 둘 다 비워져있으면 Conversion Factor만 저장한다.==========================
            If ColTxt.Text = "" And RowTxt.Text = "" Then
                CFactor_Para(IndexN) = CDec(Me.CFactorTxt.Text)
                Exit Sub
            End If


            'Col 추가============================================================================
            If ColTxt.Text = "" Then
                Col_Para(IndexN) = Col_Para(IndexN) & "," & "None"
                Me.ColLst.Items.Add("None")
            Else
                Col_Para(IndexN) = Col_Para(IndexN) & "," & Me.ColTxt.Text
                Me.ColLst.Items.Add(Me.ColTxt.Text)
            End If
            '====================================================================================

            'Row 추가============================================================================
            If RowTxt.Text = "" Then
                Row_Para(IndexN) = Row_Para(IndexN) & "," & "None"
                Me.RowLst.Items.Add("None")
            Else
                Row_Para(IndexN) = Row_Para(IndexN) & "," & Me.RowTxt.Text
                Me.RowLst.Items.Add(Me.RowTxt.Text)
            End If
            '====================================================================================

        Else '확장자가 비워져 있을경우 확장자에도 None을 입력-----------------------------------
            '이벤트를 발생시키지 않기위한 불리언 변수
            Ext_Para(IndexN) = "None"
            EvenPass = True
            Me.ExtLst.Items.RemoveAt(IndexN)
            Me.ExtLst.Items.Insert(IndexN, IndexN + 1 & ". " & "None")
            EvenPass = False

            Me.ExtLst.SelectedItem = Me.ExtLst.Items(IndexN)
        End If

        CFactor_Para(IndexN) = CDec(Me.CFactorTxt.Text)
    End Sub

    Private Sub ColLst_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles ColLst.KeyPress
        Dim i As Integer

        If Me.ColLst.SelectedIndex = -1 Then Exit Sub

        If e.KeyChar = ChrW(Keys.Back) Then
            If Me.ColLst.Items.Count <= 1 Then Exit Sub

            With Me
                .ColLst.Items.RemoveAt(.ColLst.SelectedIndex)
            End With

            '파라미터를 변수에 다시 저장해준다.
            Col_Para(Me.ExtLst.SelectedIndex) = Me.ColLst.Items(0)
            For i = 1 To Me.ColLst.Items.Count - 1 Step 1
                Col_Para(Me.ExtLst.SelectedIndex) = Col_Para(Me.ExtLst.SelectedIndex) & "," & Me.ColLst.Items(i)
            Next
        End If
    End Sub

    Private Sub RowLst_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles RowLst.KeyPress

        Dim i As Integer

        If Me.RowLst.SelectedIndex = -1 Then Exit Sub

        If e.KeyChar = ChrW(Keys.Back) Then
            If Me.RowLst.Items.Count <= 1 Then Exit Sub

            Me.RowLst.Items.RemoveAt(Me.RowLst.SelectedIndex)

            '파라미터를 변수에 다시 저장해준다.
            Row_Para(Me.ExtLst.SelectedIndex) = Me.RowLst.Items(0)
            For i = 1 To Me.RowLst.Items.Count - 1 Step 1
                Row_Para(Me.ExtLst.SelectedIndex) = Row_Para(Me.ExtLst.SelectedIndex) & "," & Me.RowLst.Items(i)
            Next
        End If

    End Sub

    Private Sub SaveBtn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveBtn.Click
        Dim i As Integer
        Dim NewParaFile As StreamWriter
        NewParaFile = New StreamWriter(Application.StartupPath & "\Profile\ParameterSetting.Set")

        '파일 쓰기
        MainMDI.ProgressBarMain.Value = 0
        MainMDI.ProgressBarMain.Maximum = 330
        For i = 0 To 31 Step 1
            MainMDI.ProgressBarMain.Value = i * 10
            NewParaFile.WriteLine(Ext_Para(i) & "=" & Col_Para(i) & "\" & Row_Para(i) & "\" & CFactor_Para(i).ToString)
        Next
        NewParaFile.Close()
        MainMDI.ProgressBarMain.Value = 320

        '파라미터를 다시 로드한다
        ParaReading()

        MainMDI.ProgressBarMain.Value = 330

        MainMDI.Statuslbl.Text = "데이터 파라미터 저장 및 적용 완료"
    End Sub

    Private Sub GraphLblLst_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GraphLblLst.SelectedIndexChanged

        If EvenPass = False Then
            Dim TmpStr() As String
            Dim i As Integer
            With Me
                .GraphLblTxt.Text = ""
                TmpStr = .GraphLblLst.Items(.GraphLblLst.SelectedIndex).ToString.Split(". ")
                For i = 1 To UBound(TmpStr)
                    .GraphLblTxt.Text = .GraphLblTxt.Text & TmpStr(i)
                Next
            End With
        End If
    End Sub

    Private Sub BtnLblReplace_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnLblReplace.Click

        Dim Index_N As Integer

        EvenPass = True

        With Me
            If .GraphLblTxt.Text <> "" Then
                Index_N = .GraphLblLst.SelectedIndex
                .GraphLblLst.Items.RemoveAt(Index_N)
                .GraphLblLst.Items.Insert(Index_N, Index_N + 1 & ". " & .GraphLblTxt.Text)
                EvenPass = False
            Else
                Exit Sub
            End If
        End With

    End Sub

    Private Sub BtnLblSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnLblSave.Click
        Dim i, k As Integer
        Dim TmpStr2 As String = ""
        Dim NewParaFile As StreamWriter
        NewParaFile = New StreamWriter(Application.StartupPath & "\Profile\Titles.Set")

        '파일 쓰기
        MainMDI.ProgressBarMain.Value = 0
        MainMDI.ProgressBarMain.Maximum = 330
        For i = 0 To 31 Step 1
            Dim TmpStr() As String
            TmpStr2 = ""
            MainMDI.ProgressBarMain.Value = i * 10
            TmpStr = Me.GraphLblLst.Items(i).ToString.Split(". ")
            For k = 1 To UBound(TmpStr)
                TmpStr2 = TmpStr2 & LTrim(TmpStr(k))
            Next
            NewParaFile.WriteLine(TmpStr2)
        Next
        NewParaFile.Close()
        MainMDI.ProgressBarMain.Value = 320

        '파라미터를 다시 로드한다
        ReadingTitles()

        MainMDI.ProgressBarMain.Value = 330

        MainMDI.Statuslbl.Text = "그래프 파라미터 저장 완료"
    End Sub

    Private Sub TESTLblLst_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TESTLblLst.SelectedIndexChanged
        Dim Tmp_Str() As String
        Dim i As Integer
        With Me
            '기존의 목록을 삭제한다.
            For i = .TESTXLst.Items.Count To 1 Step -1
                .TESTXLst.Items.RemoveAt(i - 1)
            Next
            For i = TESTYLst.Items.Count To 1 Step -1
                .TESTYLst.Items.RemoveAt(i - 1)
            Next

            '파라미터를 읽어와서 리스트에 써준다. 'x 파라미터
            Tmp_Str = TEST_Para(.TESTLblLst.SelectedIndex, 0).Split("/")
            For i = 1 To Tmp_Str.Length
                .TESTXLst.Items.Add(Tmp_Str(i - 1))
            Next

            '파라미터를 읽어와서 리스트에 써준다. 'y 파라미터
            Tmp_Str = TEST_Para(.TESTLblLst.SelectedIndex, 1).Split("/")
            For i = 1 To Tmp_Str.Length
                .TESTYLst.Items.Add(Tmp_Str(i - 1))
            Next

            .TESTLblTxt.Text = .TESTLblLst.Items(.TESTLblLst.SelectedIndex)

            .TESTIdenTxt1.Text = ""
            .TESTIdenTxt2.Text = ""
            .TESTIdenTxt3.Text = ""
            .TESTIdenTxt1.Text = TEST_DRV_Para(0)
            .TESTIdenTxt2.Text = TEST_PAS_Para(0)
            .TESTIdenTxt3.Text = TEST_Etc_Para(0)
            For i = 1 To UBound(TEST_DRV_Para)
                .TESTIdenTxt1.Text = .TESTIdenTxt1.Text & "/" & TEST_DRV_Para(i)
            Next
            For i = 1 To UBound(TEST_PAS_Para)
                .TESTIdenTxt2.Text = .TESTIdenTxt2.Text & "/" & TEST_PAS_Para(i)
            Next
            For i = 1 To UBound(TEST_Etc_Para)
                .TESTIdenTxt3.Text = .TESTIdenTxt3.Text & "/" & TEST_Etc_Para(i)
            Next
        End With
    End Sub

    Private Sub TESTaddBtn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TESTaddBtn.Click
        With Me
            If .TESTXTxt.Text <> "" Then .TESTXLst.Items.Add(.TESTXTxt.Text)
            If .TESTYTxt.Text <> "" Then .TESTYLst.Items.Add(.TESTYTxt.Text)
            SaveTESTPara(.TESTLblLst.SelectedIndex)
            .TESTXTxt.Text = ""
            .TESTYTxt.Text = ""
        End With
    End Sub

    Private Sub TESTXLst_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TESTXLst.KeyPress
        If Me.TESTXLst.SelectedIndex = -1 Then Exit Sub

        If e.KeyChar = ChrW(Keys.Back) Then
            If Me.TESTXLst.Items.Count <= 1 Then Exit Sub

            Me.TESTXLst.Items.RemoveAt(Me.TESTXLst.SelectedIndex)
        End If

        SaveTESTPara(Me.TESTLblLst.SelectedIndex)

    End Sub

    Private Sub TESTYLst_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TESTYLst.KeyPress
        'Dim i As Integer
        If Me.TESTYLst.SelectedIndex = -1 Then Exit Sub

        If e.KeyChar = ChrW(Keys.Back) Then
            If Me.TESTYLst.Items.Count <= 1 Then Exit Sub

            Me.TESTYLst.Items.RemoveAt(Me.TESTYLst.SelectedIndex)
        End If

        SaveTESTPara(Me.TESTLblLst.SelectedIndex)

    End Sub

    '변수에만 저장한다.
    Private Sub SaveTESTPara(ByVal indexN As Integer)
        'TEST 파라미터 저장==============================================
        Dim j As Integer
        Dim TmpX, TmpY As String
        With Me
            TmpX = .TESTXLst.Items(0)
            TmpY = .TESTYLst.Items(0)
            If .TESTXLst.Items.Count >= 2 Then
                For j = 2 To .TESTXLst.Items.Count
                    TmpX = TmpX & "/" & .TESTXLst.Items(j - 1)
                Next
            End If
            If .TESTYLst.Items.Count >= 2 Then
                For j = 2 To .TESTYLst.Items.Count
                    TmpY = TmpY & "/" & .TESTYLst.Items(j - 1)
                Next
            End If
            TEST_Para(indexN, 0) = TmpX
            TEST_Para(indexN, 1) = TmpY
        End With
        '==========================================================
        TESTdummyPara = TESTIdenTxt1.Text & "\" & TESTIdenTxt2.Text & "\" & TESTIdenTxt3.Text
    End Sub

    Private Sub TESTSaveBtn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TESTSaveBtn.Click

        Dim i As Integer
        Dim NewTestFile As StreamWriter

        NewTestFile = New StreamWriter(Application.StartupPath & "\Profile\ParameterTESTSetting.set")
        For i = 0 To 31
            NewTestFile.WriteLine(TEST_Para(i, 0) & "\" & TEST_Para(i, 1))
        Next
        TESTdummyPara = TESTIdenTxt1.Text & "\" & TESTIdenTxt2.Text & "\" & TESTIdenTxt3.Text
        NewTestFile.WriteLine(TESTdummyPara)
        NewTestFile.Close()

        ReadingTESTPara()

        MainMDI.Statuslbl.Text = "시험 파라미터 저장 및 적용 완료"
    End Sub

    Private Sub GraphScaleLst_ClickEvent(ByVal sender As Object, ByVal e As System.EventArgs) Handles GraphScaleLst.ClickEvent
        'MsgBox(Me.GraphScaleLst.RowSel)
        Me.SelectedGraphTxt.Text = Me.GraphScaleLst.get_TextMatrix(Me.GraphScaleLst.RowSel, 0)
        Me.GraphScaleVal.Text = Me.GraphScaleLst.get_TextMatrix(Me.GraphScaleLst.RowSel, 1)
        Me.GraphScaleVal.Focus()
    End Sub

    Private Sub InjuryScaleLst_ClickEvent(ByVal sender As Object, ByVal e As System.EventArgs) Handles InjuryScaleLst.ClickEvent
        Me.SelectedInjuryTxt.Text = Me.InjuryScaleLst.get_TextMatrix(Me.InjuryScaleLst.RowSel, 0)
        Me.InjuryScaleVal.Text = Me.InjuryScaleLst.get_TextMatrix(Me.InjuryScaleLst.RowSel, 1)
        Me.InjuryScaleVal.Focus()
    End Sub

    Private Sub OpenCorrel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenCorrel.Click

        Dim Graph_Correl(31) As Single
        Dim Injury_Correl(28) As Single
        Dim i As Integer

        '파일 확인 
        'If Not System.IO.File.Exists(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\ParameterSetting.correl") Then
        If Not System.IO.File.Exists(Application.StartupPath & "\Profile\ParameterSetting.correl") Then
            '파일 생성
            Dim NewParaFile As StreamWriter
            NewParaFile = New StreamWriter(Application.StartupPath & "\Profile\ParameterSetting.correl")
            '파일 쓰기
            NewParaFile.WriteLine("Head3MSG\1.0")
            NewParaFile.WriteLine("Chest_G_CUMULATIVE_T3MS_inj\1.0")
            NewParaFile.WriteLine("HIC15\1.0")
            NewParaFile.WriteLine("HIC36\1.0")
            NewParaFile.WriteLine("NTE\1.0")
            NewParaFile.WriteLine("NTF\1.0")
            NewParaFile.WriteLine("NCE\1.0")
            NewParaFile.WriteLine("NCF\1.0")
            NewParaFile.WriteLine("Head_Peak_G\1.0")
            NewParaFile.WriteLine("Chest_D\1.0")
            NewParaFile.WriteLine("KneeS_L\1.0")
            NewParaFile.WriteLine("KneeS_R\1.0")
            NewParaFile.WriteLine("Tibia_Comp_L\1.0")
            NewParaFile.WriteLine("Tibia_Comp_R\1.0")
            NewParaFile.WriteLine("TI_upr_L\1.0")
            NewParaFile.WriteLine("TI_lwr_L\1.0")
            NewParaFile.WriteLine("TI_upr_R\1.0")
            NewParaFile.WriteLine("TI_lwr_R\1.0")
            NewParaFile.WriteLine("Chest_VC\1.0")
            NewParaFile.WriteLine("FemurL\1.0")
            NewParaFile.WriteLine("FemurR\1.0")
            NewParaFile.WriteLine("Neck_Comp\1.0")
            NewParaFile.WriteLine("Neck_Tens\1.0")
            NewParaFile.WriteLine("Neck_Shear\1.0")
            NewParaFile.WriteLine("Neck_Exten\1.0")
            NewParaFile.WriteLine("ThxIrUpL\1.0")
            NewParaFile.WriteLine("ThxIrUpR\1.0")
            NewParaFile.WriteLine("ThxIrLowL\1.0")
            NewParaFile.WriteLine("ThxIrLowR\1.0")
            For i = 1 To 32 Step 1
                NewParaFile.WriteLine("Graph #" & i & "\1.0")
            Next
            NewParaFile.Close()
        End If

        With Me.CorrelOpenFile
            .Title = "Select Correlation Factor File"
            .Multiselect = False
            .FileName = ""                   '초기에 표시되는 파일 이름
            .InitialDirectory = Application.StartupPath & "\Profile"  '초기에 표시되는 폴더 위치 (예약어)
            .Filter = "Correl 파일|*.correl|모든 파일|*.*"
            .ShowDialog()
        End With

        If Me.CorrelOpenFile.FileNames(0) = "" Then
            MainMDI.ProgressBarMain.Value = 0
            MainMDI.Statuslbl.Text = "Cancel"
            Exit Sub
        End If

        CorrelFactorReading(Me.CorrelOpenFile.FileName, Injury_Correl, Graph_Correl)

        MainMDI.Statuslbl.Text = "Load / Display Correlation Factors"
        MainMDI.ProgressBarMain.Value = 0
        MainMDI.ProgressBarMain.Maximum = 61
        For i = 1 To 29 Step 1
            Me.InjuryScaleLst.set_TextMatrix(i - 1, 1, Injury_Correl(i - 1))
            MainMDI.ProgressBarMain.Value = i
        Next

        For i = 1 To 32 Step 1
            MainMDI.ProgressBarMain.Value = 29 + i
            Me.GraphScaleLst.set_TextMatrix(i - 1, 1, Graph_Correl(i - 1))
        Next

    End Sub

    Private Sub SaveCorrel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveCorrel.Click
        With Me.CorrelSaveFile
            .Title = "Select Correlation Factor File"
            .FileName = "Correlation_Factor"                   '초기에 표시되는 파일 이름
            .InitialDirectory = Application.StartupPath & "\Profile"  '초기에 표시되는 폴더 위치 (예약어)
            .Filter = "Correl 파일|*.Correl|모든 파일|*.*"
            .ShowDialog()
        End With

        Dim NewParaFile As StreamWriter
        NewParaFile = New StreamWriter(Me.CorrelSaveFile.FileName)

        '파일 쓰기
        NewParaFile.WriteLine("Head3MSG\" & Me.InjuryScaleLst.get_TextMatrix(0, 1))
        NewParaFile.WriteLine("Chest_G_CUMULATIVE_T3MS_inj\" & Me.InjuryScaleLst.get_TextMatrix(1, 1))
        NewParaFile.WriteLine("HIC15\" & Me.InjuryScaleLst.get_TextMatrix(2, 1))
        NewParaFile.WriteLine("HIC36\" & Me.InjuryScaleLst.get_TextMatrix(3, 1))
        NewParaFile.WriteLine("NTE\" & Me.InjuryScaleLst.get_TextMatrix(4, 1))
        NewParaFile.WriteLine("NTF\" & Me.InjuryScaleLst.get_TextMatrix(5, 1))
        NewParaFile.WriteLine("NCE\" & Me.InjuryScaleLst.get_TextMatrix(6, 1))
        NewParaFile.WriteLine("NCF\" & Me.InjuryScaleLst.get_TextMatrix(7, 1))
        NewParaFile.WriteLine("Head_Peak_G\" & Me.InjuryScaleLst.get_TextMatrix(8, 1))
        NewParaFile.WriteLine("Chest_D\" & Me.InjuryScaleLst.get_TextMatrix(9, 1))
        NewParaFile.WriteLine("KneeS_L\" & Me.InjuryScaleLst.get_TextMatrix(10, 1))
        NewParaFile.WriteLine("KneeS_R\" & Me.InjuryScaleLst.get_TextMatrix(11, 1))
        NewParaFile.WriteLine("Tibia_Comp_L\" & Me.InjuryScaleLst.get_TextMatrix(12, 1))
        NewParaFile.WriteLine("Tibia_Comp_R\" & Me.InjuryScaleLst.get_TextMatrix(13, 1))
        NewParaFile.WriteLine("TI_upr_L\" & Me.InjuryScaleLst.get_TextMatrix(14, 1))
        NewParaFile.WriteLine("TI_lwr_L\" & Me.InjuryScaleLst.get_TextMatrix(15, 1))
        NewParaFile.WriteLine("TI_upr_R\" & Me.InjuryScaleLst.get_TextMatrix(16, 1))
        NewParaFile.WriteLine("TI_lwr_R\" & Me.InjuryScaleLst.get_TextMatrix(17, 1))
        NewParaFile.WriteLine("Chest_VC\" & Me.InjuryScaleLst.get_TextMatrix(18, 1))
        NewParaFile.WriteLine("FemurL\" & Me.InjuryScaleLst.get_TextMatrix(19, 1))
        NewParaFile.WriteLine("FemurR\" & Me.InjuryScaleLst.get_TextMatrix(20, 1))
        NewParaFile.WriteLine("Neck_Comp\" & Me.InjuryScaleLst.get_TextMatrix(21, 1))
        NewParaFile.WriteLine("Neck_Tens\" & Me.InjuryScaleLst.get_TextMatrix(22, 1))
        NewParaFile.WriteLine("Neck_Shear\" & Me.InjuryScaleLst.get_TextMatrix(23, 1))
        NewParaFile.WriteLine("Neck_Exten\" & Me.InjuryScaleLst.get_TextMatrix(24, 1))
        NewParaFile.WriteLine("ThxIrUpL\" & Me.InjuryScaleLst.get_TextMatrix(25, 1))
        NewParaFile.WriteLine("ThxIrUpR\" & Me.InjuryScaleLst.get_TextMatrix(26, 1))
        NewParaFile.WriteLine("ThxIrLowL\" & Me.InjuryScaleLst.get_TextMatrix(27, 1))
        NewParaFile.WriteLine("ThxIrLowR\" & Me.InjuryScaleLst.get_TextMatrix(28, 1))
        For i = 1 To 32 Step 1
            NewParaFile.WriteLine("Graph #" & i & "\" & Me.GraphScaleLst.get_TextMatrix(i - 1, 1))
        Next
        NewParaFile.Close()
    End Sub

    Private Sub GraphScaleVal_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles GraphScaleVal.KeyPress
        If e.KeyChar = Chr(13) And IsNumeric(Me.GraphScaleVal.Text) = True Then
            Me.GraphScaleLst.set_TextMatrix(Me.GraphScaleLst.Row, 1, Me.GraphScaleVal.Text)
        ElseIf IsNumeric(Me.GraphScaleVal.Text) = False And e.KeyChar <> Chr(8) Then
            MsgBox("Numeric Value Only!!", , "Error")
        End If
    End Sub

    Private Sub InjuryScaleVal_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles InjuryScaleVal.KeyPress
        If e.KeyChar = Chr(13) And IsNumeric(Me.InjuryScaleVal.Text) = True Then
            Me.InjuryScaleLst.set_TextMatrix(Me.InjuryScaleLst.Row, 1, Me.InjuryScaleVal.Text)
        ElseIf IsNumeric(Me.InjuryScaleVal.Text) = False And e.KeyChar <> Chr(8) Then
            MsgBox("Numeric Value Only!!", , "Error")
        End If
    End Sub

    Private Sub BtnFindFolder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnFindFolder.Click
        With Me.FindFolderPath
            .Description = "Select Workspace Install Path" & vbCrLf & "ex> ....\....\Workspace_000"
            .ShowNewFolderButton = False
            .RootFolder = Environment.SpecialFolder.Desktop
            .ShowDialog()
        End With

        Dim Tmp_Str() As String

        Try
            Tmp_Str = Me.FindFolderPath.SelectedPath.Split("\")

            If Mid(Tmp_Str(UBound(Tmp_Str)), 1, 9) = "Workspace" Then
            Else
                Me.TxtPath.Text = "다시 선택하세요"
                Exit Sub
            End If

            Me.TxtPath.Text = Me.FindFolderPath.SelectedPath
            '파일 생성
            Dim NewPathFile As StreamWriter
            NewPathFile = New StreamWriter(Application.StartupPath & "\DATA\EnvironPATH.dat")
            NewPathFile.WriteLine("PATH=" & Me.FindFolderPath.SelectedPath)
            NewPathFile.Close()

        Catch ex As Exception
            MainMDI.Statuslbl.Text = ex.Message
        End Try

    End Sub

    Private Sub BtnFindFolderHW_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnFindFolderHW.Click
        With Me.FindFolderPath
            .Description = "Select HyperView Path" & vbCrLf & "ex> C:\Program Files\Altair\12.0\hw\bin\win64\hw.exe"
            .ShowNewFolderButton = False
            .RootFolder = Environment.SpecialFolder.Desktop
            .ShowDialog()
        End With

        'Dim Tmp_Str() As String

        Try
            'Tmp_Str = Me.FindFolderPath.SelectedPath.Split("\")

            'If Mid(Tmp_Str(UBound(Tmp_Str)), 1, 9) = "Workspace" Then
            'Else
            '    Me.TxtPath.Text = "다시 선택하세요"
            '    Exit Sub
            'End If

            Me.TxtPathHW.Text = Me.FindFolderPath.SelectedPath
            '파일 생성
            Dim NewPathFile As StreamWriter
            NewPathFile = New StreamWriter(Application.StartupPath & "\DATA\EnvironPATH_HW.dat")
            NewPathFile.WriteLine("PATH=" & Me.FindFolderPath.SelectedPath)
            NewPathFile.Close()

        Catch ex As Exception
            MainMDI.Statuslbl.Text = ex.Message
        End Try
    End Sub

    Private Sub BtnGbasedOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnGbasedOpen.Click

        '파일 확인
        If Not System.IO.File.Exists(Application.StartupPath & "\Profile\GraphSetting.Gcorrel") Then
            Dim NewParaFile As StreamWriter
            NewParaFile = New StreamWriter(Application.StartupPath & "\Profile\GraphSetting.Gcorrel")
            '파일 쓰기
            NewParaFile.WriteLine("HIC\0")
            NewParaFile.WriteLine("NeckFx\0")
            NewParaFile.WriteLine("NeckFz\0")
            NewParaFile.WriteLine("NeckMy\0")
            NewParaFile.WriteLine("NeckMocy\0")
            NewParaFile.WriteLine("NTE\0")
            NewParaFile.WriteLine("NTF\0")
            NewParaFile.WriteLine("NCE\0")
            NewParaFile.WriteLine("NCF\0")
            NewParaFile.Close()

        End If

        'With Me.GraphBasedDlg
        '    .Title = "Select Graph Index File"
        '    .Multiselect = False
        '    .FileName = ""                   '초기에 표시되는 파일 이름
        '    .InitialDirectory = Application.StartupPath & "\Profile"  '초기에 표시되는 폴더 위치 (예약어)
        '    .Filter = "Graph Index 파일|*.Gcorrel|모든 파일|*.*"
        '    .ShowDialog()
        'End With

        'If Me.GraphBasedDlg.FileNames(0) = "" Then
        '    MainMDI.Statuslbl.Text = "Cancel"
        '    Exit Sub
        'End If

        Dim InjuryFile As StreamReader
        'InjuryFile = New StreamReader(Me.GraphBasedDlg.FileName)
        InjuryFile = New StreamReader(Application.StartupPath & "\Profile\GraphSetting.Gcorrel")
        Dim Tmp_Str() As String

        Tmp_Str = InjuryFile.ReadLine.Split("\")
        Me.CmbHICgraph.SelectedIndex = CInt(Tmp_Str(1))
        Tmp_Str = InjuryFile.ReadLine.Split("\")
        Me.CmbFxGraph.SelectedIndex = CInt(Tmp_Str(1))
        Tmp_Str = InjuryFile.ReadLine.Split("\")
        Me.CmbFzGraph.SelectedIndex = CInt(Tmp_Str(1))
        Tmp_Str = InjuryFile.ReadLine.Split("\")
        Me.CmbMyGraph.SelectedIndex = CInt(Tmp_Str(1))
        Tmp_Str = InjuryFile.ReadLine.Split("\")
        Me.CmbMocyGraph.SelectedIndex = CInt(Tmp_Str(1))
        Tmp_Str = InjuryFile.ReadLine.Split("\")
        Me.CmbNTE.SelectedIndex = CInt(Tmp_Str(1))
        Tmp_Str = InjuryFile.ReadLine.Split("\")
        Me.CmbNTF.SelectedIndex = CInt(Tmp_Str(1))
        Tmp_Str = InjuryFile.ReadLine.Split("\")
        Me.CmbNCE.SelectedIndex = CInt(Tmp_Str(1))
        Tmp_Str = InjuryFile.ReadLine.Split("\")
        Me.CmbNCF.SelectedIndex = CInt(Tmp_Str(1))

        InjuryFile.Close()

    End Sub

    Private Sub BtnApplyNSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnApplyNSave.Click
        'With Me.GraphBasedSave
        '    .Title = "Select Graph Index File"
        '    .FileName = "Correlation_Factor"                   '초기에 표시되는 파일 이름
        '    .InitialDirectory = Application.StartupPath & "\Profile"  '초기에 표시되는 폴더 위치 (예약어)
        '    .Filter = "Graph Index 파일|*.Gcorrel|모든 파일|*.*"
        '    .ShowDialog()
        'End With

        Dim NewParaFile As StreamWriter
        'NewParaFile = New StreamWriter(Me.CorrelSaveFile.FileName)
        NewParaFile = New StreamWriter(Application.StartupPath & "\Profile\GraphSetting.Gcorrel")

        '파일 쓰기
        NewParaFile.WriteLine("HIC\" & Me.CmbHICgraph.SelectedIndex)
        NewParaFile.WriteLine("NeckFx\" & Me.CmbFxGraph.SelectedIndex)
        NewParaFile.WriteLine("NeckFz\" & Me.CmbFzGraph.SelectedIndex)
        NewParaFile.WriteLine("NeckMy\" & Me.CmbMyGraph.SelectedIndex)
        NewParaFile.WriteLine("NeckMocy\" & Me.CmbMocyGraph.SelectedIndex)
        NewParaFile.WriteLine("NTE\" & Me.CmbNTE.SelectedIndex)
        NewParaFile.WriteLine("NTF\" & Me.CmbNTF.SelectedIndex)
        NewParaFile.WriteLine("NCE\" & Me.CmbNCE.SelectedIndex)
        NewParaFile.WriteLine("NCF\" & Me.CmbNCF.SelectedIndex)

        NewParaFile.Close()

        MainMDI.Statuslbl.Text = "Saved"
    End Sub

    Private Sub GraphScaleLst_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GraphScaleLst.Enter

    End Sub
End Class

