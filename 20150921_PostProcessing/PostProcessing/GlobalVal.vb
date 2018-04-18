Imports System.IO
Imports System.Windows.Forms.DataVisualization.Charting
Imports ReadingR64
Imports ReadingAnalysisDATA
Imports WinSCP

Module GlobalVal

    Public TESTopen As New ReadingR64Binary
    Public Analopen As New ReadingAnalysisDATA.ReadingAnalysisDATA

    ''' <summary>
    ''' 임시로 원격 로그 기록 남기는 기능을 비활성화 함
    ''' True : 비활성화
    ''' False : 활성화
    ''' </summary>
    ''' <remarks></remarks>
    Public TempLicExpire As Boolean = True

    '파일을 드래그/드랍 한 경우
    Public FileDrop As Boolean = False
    Public DragFiles As String()

    '글로벌 변수 선언 및 필수 공용 함수 선언부
    Public Const Pi = 3.141592

    '그래프 변수 위치 (Row,column) - 갯수는 이후에 결정하자
    Public Graph_01() As String
    Public Graph_02() As String
    Public Graph_03() As String
    Public Graph_04() As String
    Public Graph_05() As String
    Public Graph_06() As String
    Public Graph_07() As String
    Public Graph_08() As String
    Public Graph_09() As String
    Public Graph_10() As String
    Public Graph_11() As String
    Public Graph_12() As String
    Public Graph_13() As String
    Public Graph_14() As String
    Public Graph_15() As String
    Public Graph_16() As String
    Public Graph_17() As String
    Public Graph_18() As String
    Public Graph_19() As String
    Public Graph_20() As String
    Public Graph_21() As String
    Public Graph_22() As String
    Public Graph_23() As String
    Public Graph_24() As String
    Public Graph_25() As String
    Public Graph_26() As String
    Public Graph_27() As String
    Public Graph_28() As String
    Public Graph_29() As String
    Public Graph_30() As String
    Public Graph_31() As String
    Public Graph_32() As String
    Public Graph_01_THOR() As String
    Public Graph_02_THOR() As String
    Public Graph_03_THOR() As String
    Public Graph_04_THOR() As String
    Public Graph_05_THOR() As String
    Public Graph_06_THOR() As String
    Public Graph_07_THOR() As String
    Public Graph_08_THOR() As String
    Public Graph_09_THOR() As String
    Public Graph_10_THOR() As String
    Public Graph_11_THOR() As String
    Public Graph_12_THOR() As String
    Public Graph_13_THOR() As String
    Public Graph_14_THOR() As String
    Public Graph_15_THOR() As String
    Public Graph_16_THOR() As String
    Public Graph_17_THOR() As String
    Public Graph_18_THOR() As String
    Public Graph_19_THOR() As String
    Public Graph_20_THOR() As String
    Public Graph_21_THOR() As String
    Public Graph_22_THOR() As String
    Public Graph_23_THOR() As String
    Public Graph_24_THOR() As String
    Public Graph_25_THOR() As String
    Public Graph_26_THOR() As String
    Public Graph_27_THOR() As String
    Public Graph_28_THOR() As String
    Public Graph_29_THOR() As String
    Public Graph_30_THOR() As String
    Public Graph_31_THOR() As String
    Public Graph_32_THOR() As String
    Public Graph_33_THOR() As String
    Public Graph_34_THOR() As String
    Public Graph_35_THOR() As String
    Public Graph_36_THOR() As String
    Public Graph_37_THOR() As String
    Public Graph_38_THOR() As String
    Public Graph_39_THOR() As String
    Public Graph_40_THOR() As String
    Public Graph_41_THOR() As String
    Public Graph_42_THOR() As String
    Public Graph_43_THOR() As String
    Public Graph_44_THOR() As String
    Public Graph_45_THOR() As String
    Public Graph_46_THOR() As String
    Public Graph_47_THOR() As String
    Public Graph_48_THOR() As String

    '그래프 파라미터 로드 변수 (해석용) - 확장자 및 키워드 포함
    Public Graph_Ext() As String
    Public Graph_Ext_THOR() As String

    '그래프 제목
    Public GraphTitle() As String
    Public GraphTitle_THOR() As String

    '시험 데이터 파라미터 로드 변수 (시험 DAT-R64) - X,Y키워드 포함
    Public TEST_Para(,) As String
    Public TEST_DRV_Para() As String '데이터 채널 식별자
    Public TEST_PAS_Para() As String '데이터 채널 식별자
    Public TEST_Etc_Para() As String '데이터 채널 식별자
    '====================  For THOR  ===========================
    Public TEST_Para_THOR(,) As String
    Public TEST_DRV_Para_THOR() As String '데이터 채널 식별자
    Public TEST_PAS_Para_THOR() As String '데이터 채널 식별자
    Public TEST_Etc_Para_THOR() As String '데이터 채널 식별자

    'FrmSelTDMCH에서 선택한 채널 그룹의 번호를 반환하기 위한 변수
    Public TDM_Sel_CHGroup_Num As Integer

    '후석 서브마린 여부 판단 시 선택한 그래프 번호 및 Drop/Rise, 타임윈도우, Different값을 반환하기 위한 변수
    Public SelGraphDetail(3) As Double

    'General Setting 관련 ====================================================================
    Public StartUpTab_NCAP As Integer
    Public StartUpTab_THOR As Integer

    '팝업그래프의 크기 저장 변수 (복사할때 편하게)
    Public PopUpWidth As Integer
    Public PopUpHeight As Integer
    '=========================================================================================

    Public RemoteValFolder As String = "\\HMC-NAM-W017538\Certification\" '내 PC 루트

    Public mySession As New Session '로그인 세션
    Public myTransferOptions As New TransferOptions
    Public mySessionOptions As New SessionOptions
    Public SessionInfor(3) As String
    Public Connected As Boolean = False
    Public IsRemote As Boolean = False '슈퍼컴 폴더
    Public RemoteFilesNames() As String
    Public RemoteFolderName As String 'Reloading시에 필요

    'For File Comment Browser to Run Remote MADYMO Run
    Public MAD_Run_Folder As String = ""
    Public Last_Opened_Folder As String = ""

    Public Function FileNameGet(ByVal Tmp_Str() As String) As String()
        '파일명.확장자 에서 파일명만 빼오는 글로벌 함수 (배열을 받아 배열을 반환한다)
        Dim i As Integer

        For i = 0 To UBound(Tmp_Str)
            Tmp_Str(i) = StrReverse(Tmp_Str(i))
            Tmp_Str(i) = Mid(Tmp_Str(i), InStr(Tmp_Str(i), ".") + 1, Len(Tmp_Str(i))).ToString
            Tmp_Str(i) = StrReverse(Tmp_Str(i))
        Next

        FileNameGet = Tmp_Str
    End Function

    Public Function FileNameGet2(ByVal Tmp_Str As String) As String
        '파일명.확장자 에서 파일명만 빼오는 글로벌 함수 (배열을 받아 배열을 반환한다)
        
        Tmp_Str = StrReverse(Tmp_Str)
        Tmp_Str = Mid(Tmp_Str, InStr(Tmp_Str, ".") + 1, Len(Tmp_Str)).ToString
        Tmp_Str = StrReverse(Tmp_Str)

        FileNameGet2 = Tmp_Str
    End Function

    Public Function FilePathGet(ByVal Tmp_Str() As String) As String
        '파일의 경로만 가져오는 글로벌 함수.
        Tmp_Str(0) = StrReverse(Tmp_Str(0))
        Tmp_Str(0) = Mid(Tmp_Str(0), InStr(Tmp_Str(0), "\")).ToString
        Tmp_Str(0) = StrReverse(Tmp_Str(0))
        Return Tmp_Str(0)
    End Function

    Public Function FilePathGet2(ByVal Tmp_Str As String) As String
        '파일의 경로만 가져오는 글로벌 함수.
        Tmp_Str = StrReverse(Tmp_Str)
        Tmp_Str = Mid(Tmp_Str, InStr(Tmp_Str, "\")).ToString
        Tmp_Str = StrReverse(Tmp_Str)
        Return Tmp_Str
    End Function

    '표준 정규분포함수 수치화
    Public Function CND(ByVal X_mu As Double) As Double

        'The cumulative normal distribution function
        '누적 표준 정규분포를 계산한다.
        '엑셀의 NORMSDIST()와 같은 역할을 할 수 있다.
        'X_mu 값은 표준화된 값을 넣어야 한다.

        Const PI = 3.14159265358979
        Dim L As Double
        Dim K_Const As Double

        Const a1 = 0.31938153
        Const a2 = -0.356563782
        Const a3 = 1.781477937
        Const a4 = -1.821255978
        Const a5 = 1.330274429

        L = Math.Abs(X_mu)
        K_Const = 1 / (1 + 0.2316419 * L)
        CND = 1 - 1 / Math.Sqrt(2 * PI) * Math.Exp(-L ^ 2 / 2) _
                * (a1 * K_Const + a2 * K_Const ^ 2 + a3 * K_Const ^ 3 + a4 * K_Const ^ 4 + a5 * K_Const ^ 5)

        If X_mu < 0 Then
            CND = 1 - CND
        End If

    End Function

    Public Function P_Neck_Max_Judg(ByVal p1 As Double, ByVal p2 As Double, ByVal p3 As Double, ByVal p4 As Double, ByVal p5 As Double, ByVal p6 As Double) As Double

        ''6개 중에서 maxium을 구하는 함수 (향후 더 정교한 알고리즘 교환 필요)
        'Dim max1 As Double, max2 As Double

        'If p1 > p2 And p1 > p3 Then
        '    max1 = p1
        'ElseIf p2 > p1 And p2 > p3 Then
        '    max1 = p2
        'ElseIf p3 > p1 And p3 > p2 Then
        '    max1 = p3
        'End If

        'If p4 > p5 And p4 > p6 Then
        '    max2 = p4
        'ElseIf p5 > p4 And p2 > p6 Then
        '    max2 = p5
        'ElseIf p6 > p4 And p6 > p5 Then
        '    max2 = p6
        'End If

        'If max1 >= max2 Then
        '    P_Neck_Max_Judg = max1
        'Else
        '    P_Neck_Max_Judg = max2
        'End If

        Dim P_array(5) As Double

        P_array(0) = p1
        P_array(1) = p2
        P_array(2) = p3
        P_array(3) = p4
        P_array(4) = p5
        P_array(5) = p6

        Array.Sort(P_array)

        P_Neck_Max_Judg = P_array(5)

    End Function

    '5-Star Rating
    Public Function StarRating(ByVal Rvalue As Double)

        Dim rate As Double

        If Rvalue <= 0.66 Then
            rate = 5 + (0.66 - Rvalue) / 0.66
        ElseIf Rvalue <= 0.99 Then
            rate = 4 + (0.99 - Rvalue) / 0.33
        ElseIf Rvalue <= 1.32 Then
            rate = 3 + (1.32 - Rvalue) / 0.33
        ElseIf Rvalue <= 2.66 Then
            rate = 2 + (2.66 - Rvalue) / 1.34
        ElseIf Rvalue <= 6.67 Then
            rate = 1 + (6.67 - Rvalue) / 4.01
        End If

        StarRating = rate

    End Function

    '내수 점수계산용
    Public Function DOM_Score(ByVal U_bound As Single, ByVal L_bound As Single, ByVal Injury_Val As Double, ByVal Max_Score As Integer) As Single
        '내수 각 상해 점수를 구한다.
        If Injury_Val >= U_bound Then
            DOM_Score = 0
        ElseIf Injury_Val <= L_bound Then
            DOM_Score = Max_Score
        Else
            DOM_Score = ((U_bound - Injury_Val) / (U_bound - L_bound)) * Max_Score
        End If
    End Function

    '중국 후석 계산용 (총점이 소수점이다.)
    Public Function China_Rear_Score(ByVal U_bound As Single, ByVal L_bound As Single, ByVal Injury_Val As Double, ByVal Max_Score As Single) As Single
        '내수 각 상해 점수를 구한다.
        If Injury_Val >= U_bound Then
            China_Rear_Score = 0
        ElseIf Injury_Val <= L_bound Then
            China_Rear_Score = Max_Score
        Else
            China_Rear_Score = ((U_bound - Injury_Val) / (U_bound - L_bound)) * Max_Score
        End If
    End Function

    '내수 점수계산용
    Public Function DOM_min(ByVal v_1() As Single, ByVal case_number As Integer) As Single
        '상해중 가장 낮은 점수를 구한다.
        'Bubble Sorting 을 이용함
        Dim i As Integer
        Dim j As Integer
        Dim temp As Single

        For i = 1 To case_number - 1
            For j = 1 To case_number - 1
                If v_1(j) > v_1(j + 1) Then
                    temp = v_1(j)
                    v_1(j) = v_1(j + 1)
                    v_1(j + 1) = temp
                End If
            Next j
        Next i

        DOM_min = v_1(1)

    End Function

    '중국 상해의 점수를 계산함 (피호출함수)
    Public Function Chi_Score(ByVal U_bound As Single, ByVal L_bound As Single, ByVal Injury_Val As Double, ByVal Max_Score As Single) As Single

        If Injury_Val >= U_bound Then
            Chi_Score = 0
        ElseIf Injury_Val <= L_bound Then
            Chi_Score = Max_Score
        Else
            Chi_Score = ((U_bound - Injury_Val) / (U_bound - L_bound)) * Max_Score
        End If
    End Function

    '중국 상해의 점수를 계산함 (피호출함수)
    Public Function Chi_min(ByVal v_1() As Single, ByVal case_number As Integer) As Single
        '상해중 가장 낮은 점수를 구한다.
        'Bubble Sorting 을 이용함
        Dim i As Integer
        Dim j As Integer
        Dim temp As Single

        For i = 1 To case_number - 1
            For j = 1 To case_number - 1
                If v_1(j) > v_1(j + 1) Then
                    temp = v_1(j)
                    v_1(j) = v_1(j + 1)
                    v_1(j + 1) = temp
                End If
            Next j
        Next i

        Chi_min = v_1(1)

    End Function

    'Correlation Factor를 읽어들이는 프로시저
    Public Sub CorrelFactorReading(ByVal CorrelFileName As String, ByRef Injury_Factor() As Single, ByRef Graph_Factor() As Single)
        Dim i As Integer

        '상관성 계수 로드
        '상해 파라미터를 로드한다.
        Dim InjuryFile As StreamReader
        InjuryFile = New StreamReader(CorrelFileName)
        Dim Tmp_Str() As String

        For i = 1 To 29 Step 1
            Tmp_Str = InjuryFile.ReadLine.Split("\")
            Injury_Factor(i - 1) = Tmp_Str(1)
        Next

        For i = 1 To 32 Step 1
            Tmp_Str = InjuryFile.ReadLine.Split("\")
            Graph_Factor(i - 1) = Tmp_Str(1)
        Next

        InjuryFile.Close()
    End Sub

    'Correlation Factor를 읽어들이는 프로시저
    Public Sub CorrelFactorReading_THOR(ByVal CorrelFileName As String, ByRef Injury_Factor() As Single, ByRef Graph_Factor() As Single)
        Dim i As Integer

        '상관성 계수 로드
        '상해 파라미터를 로드한다.
        Dim InjuryFile As StreamReader
        InjuryFile = New StreamReader(CorrelFileName)
        Dim Tmp_Str() As String

        For i = 1 To 30 Step 1
            Tmp_Str = InjuryFile.ReadLine.Split("\")
            Injury_Factor(i - 1) = Tmp_Str(1)
        Next

        For i = 1 To 48 Step 1
            Tmp_Str = InjuryFile.ReadLine.Split("\")
            Graph_Factor(i - 1) = Tmp_Str(1)
        Next

        InjuryFile.Close()

    End Sub

    '그래프 파라미터를 읽어들이는 프로시저 (프로그램의 시작 시 로드한다)
    Public Sub ParaReading()

        '파일 확인 
        'System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) -내문서 폴더
        If Not System.IO.File.Exists(Application.StartupPath & "\Profile\ParameterSetting.Set") Then
            '파일 생성
            Dim NewParaFile As StreamWriter
            NewParaFile = New StreamWriter(Application.StartupPath & "\Profile\ParameterSetting.Set")
            '파일 쓰기
            NewParaFile.WriteLine("lac=HeadCG_acc,HeadCG_CFC1000_acc\Res. acceleration (m/s**2)\0.101972")
            NewParaFile.WriteLine("lac=HeadCG_acc,HeadCG_CFC1000_acc\X-comp. acceleration (m/s**2)\0.101972")
            NewParaFile.WriteLine("lac=HeadCG_acc,HeadCG_CFC1000_acc\Y-comp. acceleration (m/s**2)\0.101972")
            NewParaFile.WriteLine("lac=HeadCG_acc,HeadCG_CFC1000_acc\Z-comp. acceleration (m/s**2)\0.101972")
            NewParaFile.WriteLine("lac=Thorax_acc,Thorax_CFC180_acc\Res. acceleration (m/s**2)\0.101972")
            NewParaFile.WriteLine("lac=Thorax_acc,Thorax_CFC180_acc\X-comp. acceleration (m/s**2)\0.101972")
            NewParaFile.WriteLine("lac=Thorax_acc,Thorax_CFC180_acc\Y-comp. acceleration (m/s**2)\0.101972")
            NewParaFile.WriteLine("lac=Thorax_acc,Thorax_CFC180_acc\Z-comp. acceleration (m/s**2)\0.101972")
            NewParaFile.WriteLine("lac=Pelvis_acc,Pelvis_CFC1000_acc\Res. acceleration (m/s**2)\0.101972")
            NewParaFile.WriteLine("lac=Pelvis_acc,Pelvis_CFC1000_acc\X-comp. acceleration (m/s**2)\0.101972")
            NewParaFile.WriteLine("lac=Pelvis_acc,Pelvis_CFC1000_acc\Y-comp. acceleration (m/s**2)\0.101972")
            NewParaFile.WriteLine("lac=Pelvis_acc,Pelvis_CFC1000_acc\Z-comp. acceleration (m/s**2)\0.101972")
            NewParaFile.WriteLine("frc=OUT_BELT-SH_BELT-FORCE\Resultant Force (N)\1.0")
            NewParaFile.WriteLine("frc=OUT_BELT-LAP_BELT-FORCE-FIXED,OUT_BELT-LAP_BELT-FORCE\Resultant Force (N)\1.0")
            NewParaFile.WriteLine("rds=ChestDefl_dis_CFC180\X-comp. displacement (m)\-1000")
            NewParaFile.WriteLine("injury=VC_inj_CFC180\-\1")
            NewParaFile.WriteLine("injury=NeckUp_Fx_lce,NeckUp_Fx_CFC1000_lce\-\1")
            NewParaFile.WriteLine("injury=NeckUp_Fz_lce,NeckUp_Fz_CFC1000_lce\-\1")
            NewParaFile.WriteLine("injury=NeckUp_My_lce,NeckUp_My_CFC600_lce\-\1")
            NewParaFile.WriteLine("injury=MOCy_inj\-\1")
            NewParaFile.WriteLine("injury=NTE_inj\-\1")
            NewParaFile.WriteLine("injury=NTF_inj\-\1")
            NewParaFile.WriteLine("injury=NCE_inj\-\1")
            NewParaFile.WriteLine("injury=NCF_inj\-\1")
            NewParaFile.WriteLine("injury=TIUpL_inj\-\1")
            NewParaFile.WriteLine("injury=TIUpR_inj\-\1")
            NewParaFile.WriteLine("injury=TILowL_inj\-\1")
            NewParaFile.WriteLine("injury=TILowR_inj\-\1")
            NewParaFile.WriteLine("injury=FemurL_Fz_lce,FemurL_Fz_CFC600_lce\-\1.0")
            NewParaFile.WriteLine("injury=FemurR_Fz_lce,FemurR_Fz_CFC600_lce\-\1.0")
            NewParaFile.WriteLine("injury=None\None\1")
            NewParaFile.WriteLine("injury=None\None\1")
            NewParaFile.Close()
        End If

        '그래프 파라미터를 로드한다. (프로그램 시작 시 로드한다.)
        Dim GraphFile As StreamReader
        Dim Tmp_Str() As String
        ReDim Graph_Ext(31)
        ' Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
        GraphFile = New StreamReader(Application.StartupPath & "\Profile\ParameterSetting.Set")

        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext(0) = "." & Tmp_Str(0)
        Graph_01 = Tmp_Str(1).Split("\")
        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext(1) = "." & Tmp_Str(0)
        Graph_02 = Tmp_Str(1).Split("\")
        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext(2) = "." & Tmp_Str(0)
        Graph_03 = Tmp_Str(1).Split("\")
        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext(3) = "." & Tmp_Str(0)
        Graph_04 = Tmp_Str(1).Split("\")
        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext(4) = "." & Tmp_Str(0)
        Graph_05 = Tmp_Str(1).Split("\")
        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext(5) = "." & Tmp_Str(0)
        Graph_06 = Tmp_Str(1).Split("\")
        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext(6) = "." & Tmp_Str(0)
        Graph_07 = Tmp_Str(1).Split("\")
        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext(7) = "." & Tmp_Str(0)
        Graph_08 = Tmp_Str(1).Split("\")
        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext(8) = "." & Tmp_Str(0)
        Graph_09 = Tmp_Str(1).Split("\")
        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext(9) = "." & Tmp_Str(0)
        Graph_10 = Tmp_Str(1).Split("\")
        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext(10) = "." & Tmp_Str(0)
        Graph_11 = Tmp_Str(1).Split("\")
        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext(11) = "." & Tmp_Str(0)
        Graph_12 = Tmp_Str(1).Split("\")
        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext(12) = "." & Tmp_Str(0)
        Graph_13 = Tmp_Str(1).Split("\")
        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext(13) = "." & Tmp_Str(0)
        Graph_14 = Tmp_Str(1).Split("\")
        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext(14) = "." & Tmp_Str(0)
        Graph_15 = Tmp_Str(1).Split("\")
        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext(15) = "." & Tmp_Str(0)
        Graph_16 = Tmp_Str(1).Split("\")
        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext(16) = "." & Tmp_Str(0)
        Graph_17 = Tmp_Str(1).Split("\")
        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext(17) = "." & Tmp_Str(0)
        Graph_18 = Tmp_Str(1).Split("\")
        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext(18) = "." & Tmp_Str(0)
        Graph_19 = Tmp_Str(1).Split("\")
        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext(19) = "." & Tmp_Str(0)
        Graph_20 = Tmp_Str(1).Split("\")
        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext(20) = "." & Tmp_Str(0)
        Graph_21 = Tmp_Str(1).Split("\")
        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext(21) = "." & Tmp_Str(0)
        Graph_22 = Tmp_Str(1).Split("\")
        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext(22) = "." & Tmp_Str(0)
        Graph_23 = Tmp_Str(1).Split("\")
        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext(23) = "." & Tmp_Str(0)
        Graph_24 = Tmp_Str(1).Split("\")
        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext(24) = "." & Tmp_Str(0)
        Graph_25 = Tmp_Str(1).Split("\")
        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext(25) = "." & Tmp_Str(0)
        Graph_26 = Tmp_Str(1).Split("\")
        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext(26) = "." & Tmp_Str(0)
        Graph_27 = Tmp_Str(1).Split("\")
        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext(27) = "." & Tmp_Str(0)
        Graph_28 = Tmp_Str(1).Split("\")
        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext(28) = "." & Tmp_Str(0)
        Graph_29 = Tmp_Str(1).Split("\")
        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext(29) = "." & Tmp_Str(0)
        Graph_30 = Tmp_Str(1).Split("\")
        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext(30) = "." & Tmp_Str(0)
        Graph_31 = Tmp_Str(1).Split("\")
        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext(31) = "." & Tmp_Str(0)
        Graph_32 = Tmp_Str(1).Split("\")

        GraphFile.Close()
    End Sub

    '그래프 파라미터를 읽어들이는 프로시저 (프로그램의 시작 시 로드한다)
    Public Sub ParaReading_THOR()

        '그래프 파라미터를 로드한다. (프로그램 시작 시 로드한다.)
        Dim GraphFile As StreamReader
        Dim Tmp_Str() As String
        ReDim Graph_Ext_THOR(47)
        ' Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
        GraphFile = New StreamReader(Application.StartupPath & "\Profile\ParameterSetting_THOR.Set")

        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext_THOR(0) = "." & Tmp_Str(0)
        Graph_01_THOR = Tmp_Str(1).Split("\")
        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext_THOR(1) = "." & Tmp_Str(0)
        Graph_02_THOR = Tmp_Str(1).Split("\")
        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext_THOR(2) = "." & Tmp_Str(0)
        Graph_03_THOR = Tmp_Str(1).Split("\")
        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext_THOR(3) = "." & Tmp_Str(0)
        Graph_04_THOR = Tmp_Str(1).Split("\")
        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext_THOR(4) = "." & Tmp_Str(0)
        Graph_05_THOR = Tmp_Str(1).Split("\")
        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext_THOR(5) = "." & Tmp_Str(0)
        Graph_06_THOR = Tmp_Str(1).Split("\")
        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext_THOR(6) = "." & Tmp_Str(0)
        Graph_07_THOR = Tmp_Str(1).Split("\")
        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext_THOR(7) = "." & Tmp_Str(0)
        Graph_08_THOR = Tmp_Str(1).Split("\")
        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext_THOR(8) = "." & Tmp_Str(0)
        Graph_09_THOR = Tmp_Str(1).Split("\")
        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext_THOR(9) = "." & Tmp_Str(0)
        Graph_10_THOR = Tmp_Str(1).Split("\")
        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext_THOR(10) = "." & Tmp_Str(0)
        Graph_11_THOR = Tmp_Str(1).Split("\")
        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext_THOR(11) = "." & Tmp_Str(0)
        Graph_12_THOR = Tmp_Str(1).Split("\")
        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext_THOR(12) = "." & Tmp_Str(0)
        Graph_13_THOR = Tmp_Str(1).Split("\")
        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext_THOR(13) = "." & Tmp_Str(0)
        Graph_14_THOR = Tmp_Str(1).Split("\")
        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext_THOR(14) = "." & Tmp_Str(0)
        Graph_15_THOR = Tmp_Str(1).Split("\")
        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext_THOR(15) = "." & Tmp_Str(0)
        Graph_16_THOR = Tmp_Str(1).Split("\")
        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext_THOR(16) = "." & Tmp_Str(0)
        Graph_17_THOR = Tmp_Str(1).Split("\")
        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext_THOR(17) = "." & Tmp_Str(0)
        Graph_18_THOR = Tmp_Str(1).Split("\")
        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext_THOR(18) = "." & Tmp_Str(0)
        Graph_19_THOR = Tmp_Str(1).Split("\")
        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext_THOR(19) = "." & Tmp_Str(0)
        Graph_20_THOR = Tmp_Str(1).Split("\")
        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext_THOR(20) = "." & Tmp_Str(0)
        Graph_21_THOR = Tmp_Str(1).Split("\")
        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext_THOR(21) = "." & Tmp_Str(0)
        Graph_22_THOR = Tmp_Str(1).Split("\")
        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext_THOR(22) = "." & Tmp_Str(0)
        Graph_23_THOR = Tmp_Str(1).Split("\")
        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext_THOR(23) = "." & Tmp_Str(0)
        Graph_24_THOR = Tmp_Str(1).Split("\")
        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext_THOR(24) = "." & Tmp_Str(0)
        Graph_25_THOR = Tmp_Str(1).Split("\")
        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext_THOR(25) = "." & Tmp_Str(0)
        Graph_26_THOR = Tmp_Str(1).Split("\")
        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext_THOR(26) = "." & Tmp_Str(0)
        Graph_27_THOR = Tmp_Str(1).Split("\")
        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext_THOR(27) = "." & Tmp_Str(0)
        Graph_28_THOR = Tmp_Str(1).Split("\")
        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext_THOR(28) = "." & Tmp_Str(0)
        Graph_29_THOR = Tmp_Str(1).Split("\")
        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext_THOR(29) = "." & Tmp_Str(0)
        Graph_30_THOR = Tmp_Str(1).Split("\")
        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext_THOR(30) = "." & Tmp_Str(0)
        Graph_31_THOR = Tmp_Str(1).Split("\")
        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext_THOR(31) = "." & Tmp_Str(0)
        Graph_32_THOR = Tmp_Str(1).Split("\")
        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext_THOR(32) = "." & Tmp_Str(0)
        Graph_33_THOR = Tmp_Str(1).Split("\")
        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext_THOR(33) = "." & Tmp_Str(0)
        Graph_34_THOR = Tmp_Str(1).Split("\")
        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext_THOR(34) = "." & Tmp_Str(0)
        Graph_35_THOR = Tmp_Str(1).Split("\")
        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext_THOR(35) = "." & Tmp_Str(0)
        Graph_36_THOR = Tmp_Str(1).Split("\")
        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext_THOR(36) = "." & Tmp_Str(0)
        Graph_37_THOR = Tmp_Str(1).Split("\")
        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext_THOR(37) = "." & Tmp_Str(0)
        Graph_38_THOR = Tmp_Str(1).Split("\")
        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext_THOR(38) = "." & Tmp_Str(0)
        Graph_39_THOR = Tmp_Str(1).Split("\")
        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext_THOR(39) = "." & Tmp_Str(0)
        Graph_40_THOR = Tmp_Str(1).Split("\")
        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext_THOR(40) = "." & Tmp_Str(0)
        Graph_41_THOR = Tmp_Str(1).Split("\")
        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext_THOR(41) = "." & Tmp_Str(0)
        Graph_42_THOR = Tmp_Str(1).Split("\")
        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext_THOR(42) = "." & Tmp_Str(0)
        Graph_43_THOR = Tmp_Str(1).Split("\")
        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext_THOR(43) = "." & Tmp_Str(0)
        Graph_44_THOR = Tmp_Str(1).Split("\")
        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext_THOR(44) = "." & Tmp_Str(0)
        Graph_45_THOR = Tmp_Str(1).Split("\")
        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext_THOR(45) = "." & Tmp_Str(0)
        Graph_46_THOR = Tmp_Str(1).Split("\")
        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext_THOR(46) = "." & Tmp_Str(0)
        Graph_47_THOR = Tmp_Str(1).Split("\")
        Tmp_Str = GraphFile.ReadLine.Split("=")
        Graph_Ext_THOR(47) = "." & Tmp_Str(0)
        Graph_48_THOR = Tmp_Str(1).Split("\")
        GraphFile.Close()
    End Sub

    '그래프 타이틀을 읽어들이는 프로시저 (프로그램의 시작 시 로드한다)
    Public Sub ReadingTitles()
        '그래프 타이틀을 읽어들이는 프로시저 (프로그램의 시작 시 로드한다)
        '파일 확인 
        If Not System.IO.File.Exists(Application.StartupPath & "\Profile\Titles.set") Then
            '파일 생성
            Dim NewTitleFile As StreamWriter
            NewTitleFile = New StreamWriter(Application.StartupPath & "\Profile\Titles.set")
            NewTitleFile.WriteLine("Head Resultant Acc.")
            NewTitleFile.WriteLine("Head X Acc.")
            NewTitleFile.WriteLine("Head Y Acc.")
            NewTitleFile.WriteLine("Head Z Acc.")
            NewTitleFile.WriteLine("Thorax Resultant Acc.")
            NewTitleFile.WriteLine("Thorax X Acc.")
            NewTitleFile.WriteLine("Thorax Y Acc.")
            NewTitleFile.WriteLine("Thorax Z Acc.")
            NewTitleFile.WriteLine("Pelvis Resultant Acc.")
            NewTitleFile.WriteLine("Pelvis X Acc.")
            NewTitleFile.WriteLine("Pelvis Y Acc.")
            NewTitleFile.WriteLine("Pelvis Z Acc.")
            NewTitleFile.WriteLine("Shoulder Belt Force")
            NewTitleFile.WriteLine("Lap Belt Force")
            NewTitleFile.WriteLine("Chest Deflection")
            NewTitleFile.WriteLine("Viscous Criterion")
            NewTitleFile.WriteLine("Neck Upper Fx")
            NewTitleFile.WriteLine("Neck Upper Fz")
            NewTitleFile.WriteLine("Neck Upper My")
            NewTitleFile.WriteLine("Neck Upper MOCy")
            NewTitleFile.WriteLine("NTE")
            NewTitleFile.WriteLine("NTF")
            NewTitleFile.WriteLine("NCE")
            NewTitleFile.WriteLine("NCF")
            NewTitleFile.WriteLine("Tibia Index Upper Left")
            NewTitleFile.WriteLine("Tibia Index Upper Right")
            NewTitleFile.WriteLine("Tibia Index Lower Left")
            NewTitleFile.WriteLine("Tibia Index Lower Right")
            NewTitleFile.WriteLine("Femur Load Left")
            NewTitleFile.WriteLine("Femur Load Right")
            NewTitleFile.WriteLine("None")
            NewTitleFile.WriteLine("None")
            NewTitleFile.Close()
        End If

        Dim TitleFile As StreamReader
        Dim Tmp_Str As String
        Dim i As Integer
        ReDim GraphTitle(32)
        ' Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
        TitleFile = New StreamReader(Application.StartupPath & "\Profile\Titles.Set")

        For i = 0 To 31
            Tmp_Str = TitleFile.ReadLine
            GraphTitle(i) = Tmp_Str
        Next
        GraphTitle(32) = "Additional Graph"

        TitleFile.Close()

    End Sub

    '그래프 타이틀을 읽어들이는 프로시저 (프로그램의 시작 시 로드한다)
    Public Sub ReadingTitles_THOR()
        '그래프 타이틀을 읽어들이는 프로시저 (프로그램의 시작 시 로드한다)

        Dim TitleFile As StreamReader
        Dim Tmp_Str As String
        Dim i As Integer
        ReDim GraphTitle_THOR(48)
        ' Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
        TitleFile = New StreamReader(Application.StartupPath & "\Profile\Titles_THOR.Set")

        For i = 0 To 47
            Tmp_Str = TitleFile.ReadLine
            GraphTitle_THOR(i) = Tmp_Str
        Next
        GraphTitle_THOR(48) = "Additional Graph"

        TitleFile.Close()

    End Sub

    '시험데이터 파라미터를 읽어들이는 프로시저 (프로그램의 시작 시 로드한다)
    Public Sub ReadingTESTPara()

        If Not System.IO.File.Exists(Application.StartupPath & "\Profile\ParameterTESTSetting.Set") Then
            Dim NewTestFile As StreamWriter
            NewTestFile = New StreamWriter(Application.StartupPath & "\Profile\ParameterTESTSetting.set")
            NewTestFile.WriteLine("Time/TIME/time\HEADCG,H3ACRA/HEADCG,HFACRA")
            NewTestFile.WriteLine("Time/TIME/time\HEADCG,H3ACXA/HEADCG,HFACXA/HEADCG,H3ACXP/HDCG Ax")
            NewTestFile.WriteLine("Time/TIME/time\HEADCG,H3ACYA/HEADCG,HFACYA/HEADCG,H3ACYP/HDCG Ay")
            NewTestFile.WriteLine("Time/TIME/time\HEADCG,H3ACZA/HEADCG,HFACZA/HEADCG,H3ACZP/HDCG Az")
            NewTestFile.WriteLine("Time/TIME/time\CHST,H3ACRC/CHST,HFACRC/CHSTCG,H3ACRC")
            NewTestFile.WriteLine("Time/TIME/time\CHST,H3ACXC/CHST,HFACXC/CHST,H3ACXP/SPNM Ax/11CHSTH3ACRC")
            NewTestFile.WriteLine("Time/TIME/time\CHST,H3ACYC/CHST,HFACYC/CHST,H3ACYP/SPNM Ay")
            NewTestFile.WriteLine("Time/TIME/time\CHST,H3ACZC/CHST,HFACZC/CHST,H3ACZP/SPNM Az")
            NewTestFile.WriteLine("Time/TIME/time\PELV,H3ACRA/PELV,HFACRA")
            NewTestFile.WriteLine("Time/TIME/time\PELV,H3ACXA/PELV,HFACXA/PELV,H3ACXP/PVCG Ax")
            NewTestFile.WriteLine("Time/TIME/time\PELV,H3ACYA/PELV,HFACYA/PELV,H3ACYP/PVCG Ay")
            NewTestFile.WriteLine("Time/TIME/time\PELV,H3ACZA/PELV,HFACZA/PELV,H3ACZP/PVCG Az")
            NewTestFile.WriteLine("Time/TIME/time\SEBEUP,FO0B/SEBEUP,FO0D/SHBT F")
            NewTestFile.WriteLine("Time/TIME/time\SEBELO,FO0B/SEBELO,FO0D/LPBO F")
            NewTestFile.WriteLine("Time/TIME/time\CHST,H3DSXB/CHST,HFDSXB/CHST Dx/CHST,H3DSXP")
            NewTestFile.WriteLine("Time/TIME/time\VC")
            NewTestFile.WriteLine("Time/TIME/time\NECKUP,H3FOXA/NECKUP,HFFOXA/NECKUP,H3FOXP/NEKU Fx")
            NewTestFile.WriteLine("Time/TIME/time\NECKUP,H3FOZA/NECKUP,HFFOZA/NECKUP,H3FOZP/NEKU Fz")
            NewTestFile.WriteLine("Time/TIME/time\NECKUP,H3MOYB/NECKUP,HFMOYB/NECKUP,H3MOYP/NEKU My")
            NewTestFile.WriteLine("Time/TIME/time\Mocy")
            NewTestFile.WriteLine("Time/TIME/time\Nte")
            NewTestFile.WriteLine("Time/TIME/time\Ntf")
            NewTestFile.WriteLine("Time/TIME/time\Nce")
            NewTestFile.WriteLine("Time/TIME/time\Ncf")
            NewTestFile.WriteLine("Time/TIME/time\TI,LH,UPR")
            NewTestFile.WriteLine("Time/TIME/time\TI,RH,UPR")
            NewTestFile.WriteLine("Time/TIME/time\TI,LH,LWR")
            NewTestFile.WriteLine("Time/TIME/time\TI,RH,LWR")
            NewTestFile.WriteLine("Time/TIME/time\FEMR,LE,H3,FOZB/FEMR,LE,HF,FOZB/FEMR,LE,H3,FOZP/FMRL Fz")
            NewTestFile.WriteLine("Time/TIME/time\FEMR,RI,H3,FOZB/FEMR,RI,HF,FOZB/FEMR,RI,H3,FOZP/FMRR Fz")
            NewTestFile.WriteLine("Time/TIME/time\STCO")
            NewTestFile.WriteLine("Time/TIME/time\BPIL,ACX")
            NewTestFile.WriteLine("11/DR/DRV/Driver/V2P1\13/PA/PAS/Passenger/V2P2\14/16")
            NewTestFile.Close()
        End If

        Dim TestFile As StreamReader
        Dim Tmp_Str1 As String
        Dim Tmp_Str2() As String
        Dim i As Integer
        ReDim TEST_Para(31, 1)

        TestFile = New StreamReader(Application.StartupPath & "\Profile\ParameterTESTSetting.Set")

        For i = 0 To 31
            Tmp_Str1 = TestFile.ReadLine
            Tmp_Str2 = Tmp_Str1.Split("\")
            TEST_Para(i, 0) = Tmp_Str2(0)
            TEST_Para(i, 1) = Tmp_Str2(1)
        Next

        '테스트 더미 파라미터
        Tmp_Str1 = TestFile.ReadLine
        Tmp_Str2 = Tmp_Str1.Split("\")
        TEST_DRV_Para = Tmp_Str2(0).Split("/")
        TEST_PAS_Para = Tmp_Str2(1).Split("/")
        TEST_Etc_Para = Tmp_Str2(2).Split("/")

        TestFile.Close()
    End Sub

    '시험데이터 파라미터를 읽어들이는 프로시저 (프로그램의 시작 시 로드한다)
    Public Sub ReadingTESTPara_THOR()

        Dim TestFile As StreamReader
        Dim Tmp_Str1 As String
        Dim Tmp_Str2() As String
        Dim i As Integer
        ReDim TEST_Para_THOR(47, 1)

        TestFile = New StreamReader(Application.StartupPath & "\Profile\ParameterTESTSetting_THOR.Set")

        For i = 0 To 47
            Tmp_Str1 = TestFile.ReadLine
            Tmp_Str2 = Tmp_Str1.Split("\")
            TEST_Para_THOR(i, 0) = Tmp_Str2(0)
            TEST_Para_THOR(i, 1) = Tmp_Str2(1)
        Next

        '테스트 더미 파라미터
        Tmp_Str1 = TestFile.ReadLine
        Tmp_Str2 = Tmp_Str1.Split("\")
        TEST_DRV_Para_THOR = Tmp_Str2(0).Split("/")
        TEST_PAS_Para_THOR = Tmp_Str2(1).Split("/")
        TEST_Etc_Para_THOR = Tmp_Str2(2).Split("/")

        TestFile.Close()
    End Sub

    '일반 셋업항목을 읽어온다. General SetUp
    Public Sub ReadingGeneral_Set()
        '파일 확인
        If Not System.IO.File.Exists(Application.StartupPath & "\DATA\General.setup") Then
            Dim NewParaFile As StreamWriter
            NewParaFile = New StreamWriter(Application.StartupPath & "\DATA\General.setup")
            '파일 쓰기
            NewParaFile.WriteLine("H3\0")
            NewParaFile.WriteLine("THOR\0")
            NewParaFile.WriteLine("POPUP_WIDTH\490")
            NewParaFile.WriteLine("POPUP_HEIGHT\320")
            NewParaFile.Close()
        End If

        '파일 읽기
        Dim SetupFile As StreamReader
        'InjuryFile = New StreamReader(Me.GraphBasedDlg.FileName)
        SetupFile = New StreamReader(Application.StartupPath & "\DATA\General.setup")
        Dim Tmp_Str() As String

        Tmp_Str = SetupFile.ReadLine.Split("\")
        StartUpTab_NCAP = CInt(Tmp_Str(1))
        Tmp_Str = SetupFile.ReadLine.Split("\")
        StartUpTab_THOR = CInt(Tmp_Str(1))
        Tmp_Str = SetupFile.ReadLine.Split("\")
        PopUpWidth = CInt(Tmp_Str(1))
        Tmp_Str = SetupFile.ReadLine.Split("\")
        PopUpHeight = CInt(Tmp_Str(1))
        SetupFile.Close()

    End Sub

    ''데이터의 행/열/총 행수/총 열수 위치를 반환한다.
    'Public Function FileRowCol(ByVal Fpath As String, ByVal Fnames As String, ByVal Ext As String, ByVal Para() As String) As Integer()
    '    'Para() 변수는 여러개 파라미터가 String으로 합쳐져있다. 
    '    ' 0 : 행식별자 이름  /  1 : 열식별자 이름  /  3  : Conversion Factor ..............식별자 (",")
    '    '각 인덱스 안에는 여러 항목이 식별자 (",") 로 합쳐져있다.
    
    ''해석 데이터 읽어오는 함수
    'Public Sub DataReading(ByVal FilePath As String, ByVal FileNames As String, ByVal Exts As String _
    '                            , ByVal Rows As Integer, ByVal Cols As Integer, ByVal TotRows As Integer, ByVal TotCols As Integer _
    '                            , ByRef Time() As Decimal, ByRef Vals() As Double, _
    '                            Optional ByVal C_Factor As Decimal = 1.0, _
    '                            Optional ByVal X_Scale As Single = 1.0, _
    '                            Optional ByVal X_Offset As Single = 0.0, _
    '                            Optional ByVal Y_Offset As Single = 0.0)
    '    '(Y Scale / X Scale / X Offset / Y Offset)  ==>> 옵션 파라미터 순서 중요!!!!!
    

    '차트를 클립보드에 올린다. (BMP Image File)
    Public Sub CopyChartImage(ByRef CopyCharts As Chart)
        ' Create a memory stream to save the chart image    
        Dim stream As New System.IO.MemoryStream()

        ' Save the chart image to the stream    
        'CopyCharts.SaveImage(stream, System.Drawing.Imaging.ImageFormat.Bmp)
        CopyCharts.SaveImage(stream, System.Drawing.Imaging.ImageFormat.Bmp)

        ' Create a bitmap using the stream    
        Dim BmpImage As New Bitmap(stream)

        ' Save the bitmap to the clipboard    
        Clipboard.SetDataObject(BmpImage)
    End Sub

    'Reading DAT Header File of TEST DATA
    Public Function OepnDATfileHeader(ByVal FileNamePath As String) As String(,)

        Dim i As Integer

        Dim HeaderFile As StreamReader
        Dim DAT_CH(,) As String = Nothing
        Dim DAT_CH_names() As String = Nothing
        Dim DAT_CH_len() As String = Nothing
        Dim DAT_CH_locate() As String = Nothing
        Dim DAT_CH_Bit() As String = Nothing
        Dim StartOffset() As String = Nothing
        Dim IcrementFactor() As String = Nothing
        Dim IsImplicit() As String = Nothing
        Dim DAT_CH_Unit() As String = Nothing
        Dim DAT_Source_File() As String = Nothing
        'Dim Tmp_Strs() As String
        Dim Tmp_read As String

        HeaderFile = New StreamReader(FileNamePath)  '파일을 연다
        i = 0

        Do While Not HeaderFile.EndOfStream
            Tmp_read = HeaderFile.ReadLine

            '헤더의 시작
            If InStr(Tmp_read, "#BEGINCHANNELHEADER") >= 1 Then
                ReDim Preserve DAT_CH_names(0 To i)
                ReDim Preserve DAT_CH_locate(0 To i)
                ReDim Preserve DAT_CH_len(0 To i)
                ReDim Preserve DAT_CH_Bit(0 To i)
                ReDim Preserve StartOffset(0 To i)
                ReDim Preserve IcrementFactor(0 To i)
                ReDim Preserve IsImplicit(0 To i)
                ReDim Preserve DAT_CH_Unit(0 To i)
                ReDim Preserve DAT_Source_File(0 To i)

                Do While (InStr(Tmp_read, "#ENDCHANNELHEADER") = 0)
                    Tmp_read = HeaderFile.ReadLine
                    Select Case Left(Tmp_read, 3)
                        Case "200"
                            DAT_CH_names(i) = Right(Tmp_read, Len(Tmp_read) - 4)
                        Case "202"
                            DAT_CH_Unit(i) = Right(Tmp_read, Len(Tmp_read) - 4)
                        Case "210"
                            If Right(Tmp_read, Len(Tmp_read) - 4) = "IMPLICIT" Then
                                IsImplicit(i) = 1
                            Else
                                IsImplicit(i) = 0
                            End If
                        Case "211"
                            DAT_Source_File(i) = Right(Tmp_read, Len(Tmp_read) - 4)
                        Case "220"
                            DAT_CH_len(i) = Right(Tmp_read, Len(Tmp_read) - 4)  '이게 0 이면 데이터가 없는 것이다.
                        Case "221"
                            DAT_CH_locate(i) = CStr((CLng(Right(Tmp_read, Len(Tmp_read) - 4)) - 1) * 8) '이게 "" 이면 데이터가 없는 것이다. point를 구별한다.
                        Case "240"
                            StartOffset(i) = Right(Tmp_read, Len(Tmp_read) - 4)
                        Case "241"
                            IcrementFactor(i) = Right(Tmp_read, Len(Tmp_read) - 4)
                        Case Else

                    End Select
                    DAT_CH_Bit(i) = 8 ' R64파일은 DOUBLE이다.
                Loop

                i = i + 1

            End If
        Loop

        HeaderFile.Close() '파일을 닫는다.

        '채널 해더 배열의 정의
        ReDim DAT_CH(0 To UBound(DAT_CH_names), 0 To 8)
        'CH_names(채널명,0-데이터이름/1-시작위치/2-길이(갯수)/3-시작옵셋값/4-증분/5-Implicit여부/6-단위/7-소스파일/8-32bit및64bit구분)

        For i = 0 To UBound(DAT_CH_names)
            DAT_CH(i, 0) = DAT_CH_names(i)
            DAT_CH(i, 1) = DAT_CH_locate(i)
            DAT_CH(i, 2) = DAT_CH_len(i)
            DAT_CH(i, 3) = StartOffset(i)
            DAT_CH(i, 4) = IcrementFactor(i)
            DAT_CH(i, 5) = IsImplicit(i)
            DAT_CH(i, 6) = DAT_CH_Unit(i)
            DAT_CH(i, 7) = DAT_Source_File(i)
            DAT_CH(i, 8) = DAT_CH_Bit(i)
        Next

        OepnDATfileHeader = DAT_CH

    End Function

    'Reading TDM Header File of TEST DATA
    Public Function OepnTDMfileHeader(ByVal FileNamePath As String, ByVal CH_Num As Integer) As String(,)

        Dim i As Integer

        Dim Tmp_read As String
        Dim Locate(1) As String

        Dim IsOneLineTDM As Boolean = False

        Dim DAT_CH(,) As String = Nothing
        Dim DAT_CH_names() As String = Nothing '<tdm_channel id=
        Dim DAT_CH_len() As String = Nothing   '<submatrix id= <number_of_rows>
        Dim DAT_CH_locate() As String = Nothing
        Dim DAT_CH_Bit() As Integer 'eFloat64Usi 및 eFloat32Usi 구분
        Dim StartOffset() As String = Nothing
        Dim IcrementFactor() As String = Nothing
        Dim IsImplicit() As String = Nothing   '<localcolumn id=  ...<sequence_representation>
        Dim DAT_CH_Unit() As String = Nothing
        Dim DAT_Source_File() As String = Nothing
        Dim DAT_CH_Min() As String = Nothing
        Dim DAT_CH_Max() As String = Nothing

        Dim TDM_Lines() As String '한 줄짜리 TDM 일 때

        Dim HeaderFile As StreamReader

        i = 0

        '채널 이름을 식별한다.==================================================================================
        HeaderFile = New StreamReader(FileNamePath)

        Tmp_read = HeaderFile.ReadLine

        If InStr(Tmp_read, "><") > 0 Then

            IsOneLineTDM = True

            Dim Tmp_read_2 As String
            Tmp_read_2 = Tmp_read.Replace("><", ">" & vbCrLf & "<")
            TDM_Lines = Tmp_read_2.Split(vbCrLf)

            Dim kk As Integer = 0

            Do While kk <= UBound(TDM_Lines)

                Tmp_read = TDM_Lines(kk)

                If InStr(Tmp_read, "<tdm_channel id=""") > 0 Then
                    kk = kk + 1
                    Tmp_read = TDM_Lines(kk)
                    i = i + 1
                    ReDim Preserve DAT_CH_names(i)
                    ReDim Preserve DAT_CH_Unit(i)
                    ReDim Preserve StartOffset(i)
                    ReDim Preserve IcrementFactor(i)

                    DAT_CH_names(i) = BetweenEle(Tmp_read)

                    Do While Not InStr(Tmp_read, "</tdm_channel>") > 1
                        kk = kk + 1
                        Tmp_read = TDM_Lines(kk)
                        '채널의 단위를 식별=========================================================================
                        If InStr(Tmp_read, "<unit_string>") > 0 Then
                            DAT_CH_Unit(i) = BetweenEle(Tmp_read)
                        End If
                        '채널의 시작 옵셋값을 식별==================================================================
                        If InStr(Tmp_read, """wf_start_offset""") > 0 Then
                            StartOffset(i) = BetweenEle(Tmp_read)
                        End If
                        '채널의 증분을 식별========================================================================
                        If InStr(Tmp_read, """wf_increment""") > 0 Then
                            IcrementFactor(i) = CStr(Math.Round(CDbl(BetweenEle(Tmp_read)) * 1000, 3) / 1000)
                        End If
                    Loop
                End If

                kk = kk + 1
            Loop

        Else

            Do While Not HeaderFile.EndOfStream
                Tmp_read = HeaderFile.ReadLine

                If InStr(Tmp_read, "<tdm_channel id=""") > 0 Then
                    Tmp_read = HeaderFile.ReadLine
                    i = i + 1
                    ReDim Preserve DAT_CH_names(i)
                    ReDim Preserve DAT_CH_Unit(i)
                    ReDim Preserve StartOffset(i)
                    ReDim Preserve IcrementFactor(i)

                    DAT_CH_names(i) = BetweenEle(Tmp_read)

                    Do While Not InStr(Tmp_read, "</tdm_channel>") > 1
                        Tmp_read = HeaderFile.ReadLine
                        '채널의 단위를 식별=========================================================================
                        If InStr(Tmp_read, "<unit_string>") > 0 Then
                            DAT_CH_Unit(i) = BetweenEle(Tmp_read)
                        End If
                        '채널의 시작 옵셋값을 식별==================================================================
                        If InStr(Tmp_read, """wf_start_offset""") > 0 Then
                            StartOffset(i) = BetweenEle(Tmp_read)
                        End If
                        '채널의 증분을 식별========================================================================
                        If InStr(Tmp_read, """wf_increment""") > 0 Then
                            IcrementFactor(i) = CStr(Math.Round(CDbl(BetweenEle(Tmp_read)) * 1000, 3) / 1000)
                        End If
                    Loop

                End If
            Loop

        End If


        HeaderFile.Close()



        Dim TotChs As Integer = i

        ReDim Preserve IsImplicit(TotChs)
        ReDim Preserve DAT_CH_len(TotChs)
        ReDim Preserve DAT_Source_File(TotChs)

        For i = 0 To TotChs
            DAT_Source_File(i) = Mid(FileNamePath, 1, Len(FileNamePath) - 3) & "tdx"
        Next

        i = 0

        If IsOneLineTDM = False Then

            '채널 형식을 식별한다.==================================================================================
            HeaderFile = New StreamReader(FileNamePath)
            Do While Not HeaderFile.EndOfStream
                Tmp_read = HeaderFile.ReadLine

                If InStr(Tmp_read, "<sequence_representation>") > 0 Then
                    i = i + 1
                    ReDim Preserve IsImplicit(i)
                    Select Case BetweenEle(Tmp_read)
                        Case "implicit_linear"
                            IsImplicit(i) = 1
                        Case Else
                            IsImplicit(i) = 0
                    End Select
                End If
            Loop
            HeaderFile.Close()

            i = 0

            '채널의 길이를 식별한다.==================================================================================
            HeaderFile = New StreamReader(FileNamePath)
            Do While Not HeaderFile.EndOfStream
                Tmp_read = HeaderFile.ReadLine

                If InStr(Tmp_read, "<number_of_rows>") > 0 Then
                    i = i + 1
                    ReDim Preserve DAT_CH_len(i)
                    DAT_CH_len(i) = BetweenEle(Tmp_read)
                End If
            Loop
            HeaderFile.Close()

            ' Time 채널이 데이터가 없는 경우가 있어서 보완 ==========================================================
            i = 0
            HeaderFile = New StreamReader(FileNamePath)
            Do While Not HeaderFile.EndOfStream
                Tmp_read = HeaderFile.ReadLine

                If InStr(Tmp_read, "<tdm_channel id=""") > 0 Then
                    Tmp_read = HeaderFile.ReadLine
                    i = i + 1
                    ReDim Preserve DAT_CH_Min(i)
                    ReDim Preserve DAT_CH_Max(i)

                    If (DAT_CH_names(i) = "time") Or (DAT_CH_names(i) = "TIME") Or (DAT_CH_names(i) = "Time") Then
                        Do While Not InStr(Tmp_read, "</tdm_channel>") > 1
                            Tmp_read = HeaderFile.ReadLine
                            '채널의 단위를 식별=========================================================================
                            If InStr(Tmp_read, "<minimum>") > 0 Then
                                DAT_CH_Min(i) = BetweenEle(Tmp_read)
                            End If
                            If InStr(Tmp_read, "<maximum>") > 0 Then
                                DAT_CH_Max(i) = BetweenEle(Tmp_read)
                            End If
                        Loop

                        StartOffset(i) = DAT_CH_Min(i)
                        IcrementFactor(i) = (DAT_CH_Max(i) - DAT_CH_Min(i)) / (DAT_CH_len(i) - 1)
                    End If
                End If
            Loop
            HeaderFile.Close()
            '======================================================================================================

            i = 0

            Dim k, m As Integer
            k = 0
            m = 0

            '각 채널의 시작 위치를 식별한다.========================================================================= DAT_CH_locate
            ReDim Preserve DAT_CH_locate(TotChs)
            ReDim Preserve DAT_CH_Bit(TotChs)
            HeaderFile = New StreamReader(FileNamePath)
            Do While Not HeaderFile.EndOfStream
                Tmp_read = HeaderFile.ReadLine

                If InStr(Tmp_read, "<block byteOffset") > 0 Then

                    i = i + 1

                    '인덱스가 왜 넘어가는지는 모르겠는데 여튼 추가 필요=======================================
                    If i > UBound(DAT_CH_names) Then
                        Exit Do
                    End If
                    '======================================================================================

                    If InStr(Tmp_read, "eFloat64Usi") > 1 Then
                        DAT_CH_Bit(i) = 8
                    ElseIf InStr(Tmp_read, "eFloat32Usi") > 1 Then
                        DAT_CH_Bit(i) = 4
                    End If

                    If (DAT_CH_len(i) <> 0) And (IsImplicit(i) = 0) Then
                        DAT_CH_locate(i) = CStr(CInt(AttriGet(Tmp_read, 1)(1)))
                    ElseIf DAT_CH_len(i) = 0 Or (IsImplicit(i) = 1) Then
                        For m = 1 To UBound(DAT_CH_len)
                            If (i + m) <= UBound(DAT_CH_len) Then
                                If (DAT_CH_len(i + m) <> 0) And (IsImplicit(i + m) = 0) Then 'OrElse (IsImplicit(i + m) <> 1) Then
                                    DAT_CH_locate(i + m) = AttriGet(Tmp_read, 1)(1)
                                    Exit For
                                End If
                            End If
                        Next

                        i = i + m

                    End If
                End If
            Loop
            HeaderFile.Close()

            i = 0
            '채널 그룹을 식별한다.==================================================================================
            Dim CH_Group_Name() As String = Nothing  'tdm_channelgroup
            Dim CH_Group_Cnt() As String = Nothing '각 그룹마다 있는 채널 수를 판독
            Dim Tmp_CH_cnt() As String = Nothing
            HeaderFile = New StreamReader(FileNamePath)
            Do While Not HeaderFile.EndOfStream
                Tmp_read = HeaderFile.ReadLine

                If InStr(Tmp_read, "<tdm_channelgroup id=""") > 0 Then
                    Tmp_read = HeaderFile.ReadLine
                    i = i + 1
                    ReDim Preserve CH_Group_Name(i)
                    ReDim Preserve CH_Group_Cnt(i)
                    CH_Group_Name(i) = BetweenEle(Tmp_read)

                    Do While Not InStr(Tmp_read, "</tdm_channelgroup>") > 1
                        Tmp_read = HeaderFile.ReadLine
                        '채널그룹의 이름식별=========================================================================
                        If InStr(Tmp_read, "<name>") > 0 Then
                            CH_Group_Name(i) = BetweenEle(Tmp_read)
                        End If
                        '채널그룹내 채널 개수 식별====================================================================
                        '단일 채널인 경우에는 생기지 않는 Element임
                        If InStr(Tmp_read, "<channels>") > 0 Then
                            Tmp_CH_cnt = RTrim(LTrim(Tmp_read)).Split(" ")
                            CH_Group_Cnt(i) = Tmp_CH_cnt.Count
                        End If
                    Loop

                End If
            Loop


            If UBound(CH_Group_Name) = 1 Then CH_Group_Cnt(1) = TotChs

            CH_Group_Cnt(0) = 0

            HeaderFile.Close()

            '채널 해더 배열의 정의####################################################################################
            ReDim DAT_CH(0 To CH_Group_Cnt(CH_Num) - 1, 0 To 8)
            'CH_names(채널명,0-데이터이름/1-시작위치/2-길이(갯수)/3-시작옵셋값/4-증분/5-Implicit여부/6-단위/7-소스파일/8-32및64구분)

            Dim Ch_Cnt_Tot As Integer = 0
            If UBound(CH_Group_Name) > 1 Then
                For i = 1 To CH_Num - 1
                    Ch_Cnt_Tot = Ch_Cnt_Tot + CH_Group_Cnt(i)
                Next
            Else
                Ch_Cnt_Tot = 0
            End If

            For i = 1 To CH_Group_Cnt(CH_Num) 'UBound(DAT_CH_names)
                DAT_CH(i - 1, 0) = DAT_CH_names(Ch_Cnt_Tot + i)
                DAT_CH(i - 1, 1) = DAT_CH_locate(Ch_Cnt_Tot + i)
                DAT_CH(i - 1, 2) = DAT_CH_len(Ch_Cnt_Tot + i)
                DAT_CH(i - 1, 3) = StartOffset(Ch_Cnt_Tot + i)
                DAT_CH(i - 1, 4) = IcrementFactor(Ch_Cnt_Tot + i)
                DAT_CH(i - 1, 5) = IsImplicit(Ch_Cnt_Tot + i)
                DAT_CH(i - 1, 6) = DAT_CH_Unit(Ch_Cnt_Tot + i)
                DAT_CH(i - 1, 7) = DAT_Source_File(Ch_Cnt_Tot + i)
                DAT_CH(i - 1, 8) = DAT_CH_Bit(Ch_Cnt_Tot + i)
            Next

            'For i = 1 To CH_Group_Cnt(CH_Num) 'UBound(DAT_CH_names)
            '    DAT_CH(i - 1, 0) = DAT_CH_names(CH_Group_Cnt(CH_Num - 1) + i)
            '    DAT_CH(i - 1, 1) = DAT_CH_locate(CH_Group_Cnt(CH_Num - 1) + i)
            '    DAT_CH(i - 1, 2) = DAT_CH_len(CH_Group_Cnt(CH_Num - 1) + i)
            '    DAT_CH(i - 1, 3) = StartOffset(CH_Group_Cnt(CH_Num - 1) + i)
            '    DAT_CH(i - 1, 4) = IcrementFactor(CH_Group_Cnt(CH_Num - 1) + i)
            '    DAT_CH(i - 1, 5) = IsImplicit(CH_Group_Cnt(CH_Num - 1) + i)
            '    DAT_CH(i - 1, 6) = DAT_CH_Unit(CH_Group_Cnt(CH_Num - 1) + i)
            '    DAT_CH(i - 1, 7) = DAT_Source_File(CH_Group_Cnt(CH_Num - 1) + i)
            'Next

            OepnTDMfileHeader = DAT_CH



        Else
            'TDM이 한 줄일때

            Dim kk As Integer = 0

            '채널 형식을 식별한다.==================================================================================

            Do While kk < UBound(TDM_Lines)
                Tmp_read = TDM_Lines(kk + 1)

                If InStr(Tmp_read, "<sequence_representation>") > 0 Then
                    i = i + 1
                    ReDim Preserve IsImplicit(i)
                    Select Case BetweenEle(Tmp_read)
                        Case "implicit_linear"
                            IsImplicit(i) = 1
                        Case Else
                            IsImplicit(i) = 0
                    End Select
                End If
                kk = kk + 1
            Loop

            i = 0
            kk = 0

            '채널의 길이를 식별한다.==================================================================================
            Do While kk < UBound(TDM_Lines)
                Tmp_read = TDM_Lines(kk + 1)

                If InStr(Tmp_read, "<number_of_rows>") > 0 Then
                    i = i + 1
                    ReDim Preserve DAT_CH_len(i)
                    DAT_CH_len(i) = BetweenEle(Tmp_read)
                End If
                kk = kk + 1
            Loop

            ' Time 채널이 데이터가 없는 경우가 있어서 보완 ==========================================================
            i = 0
            kk = 0
            Do While kk < UBound(TDM_Lines)
                Tmp_read = TDM_Lines(kk + 1)

                If InStr(Tmp_read, "<tdm_channel id=""") > 0 Then
                    kk = kk + 1
                    Tmp_read = TDM_Lines(kk)
                    i = i + 1
                    ReDim Preserve DAT_CH_Min(i)
                    ReDim Preserve DAT_CH_Max(i)

                    If (DAT_CH_names(i) = "time") Or (DAT_CH_names(i) = "TIME") Or (DAT_CH_names(i) = "Time") Then
                        Do While Not InStr(Tmp_read, "</tdm_channel>") > 1
                            kk = kk + 1
                            Tmp_read = TDM_Lines(kk)
                            '채널의 단위를 식별=========================================================================
                            If InStr(Tmp_read, "<minimum>") > 0 Then
                                DAT_CH_Min(i) = BetweenEle(Tmp_read)
                            End If
                            If InStr(Tmp_read, "<maximum>") > 0 Then
                                DAT_CH_Max(i) = BetweenEle(Tmp_read)
                            End If
                        Loop

                        StartOffset(i) = DAT_CH_Min(i)
                        IcrementFactor(i) = (DAT_CH_Max(i) - DAT_CH_Min(i)) / (DAT_CH_len(i) - 1)
                    End If
                End If
                kk = kk + 1
            Loop
            '======================================================================================================

            i = 0
            kk = 0

            Dim k, m As Integer
            k = 0
            m = 0

            '각 채널의 시작 위치를 식별한다.========================================================================= DAT_CH_locate
            ReDim Preserve DAT_CH_locate(TotChs)
            ReDim Preserve DAT_CH_Bit(TotChs)

            Do While kk < UBound(TDM_Lines)

                Tmp_read = TDM_Lines(kk + 1)

                If InStr(Tmp_read, "<block byteOffset") > 0 Then

                    i = i + 1

                    '인덱스가 왜 넘어가는지는 모르겠는데 여튼 추가 필요=======================================
                    If i > UBound(DAT_CH_names) Then
                        Exit Do
                    End If
                    '======================================================================================

                    If InStr(Tmp_read, "eFloat64Usi") > 1 Then
                        DAT_CH_Bit(i) = 8
                    ElseIf InStr(Tmp_read, "eFloat32Usi") > 1 Then
                        DAT_CH_Bit(i) = 4
                    End If

                    If (DAT_CH_len(i) <> 0) And (IsImplicit(i) = 0) Then
                        DAT_CH_locate(i) = CStr(CInt(AttriGet(Tmp_read, 1)(1)))
                    ElseIf DAT_CH_len(i) = 0 Or (IsImplicit(i) = 1) Then
                        For m = 1 To UBound(DAT_CH_len)
                            If (i + m) <= UBound(DAT_CH_len) Then
                                If (DAT_CH_len(i + m) <> 0) And (IsImplicit(i + m) = 0) Then 'OrElse (IsImplicit(i + m) <> 1) Then
                                    DAT_CH_locate(i + m) = AttriGet(Tmp_read, 1)(1)
                                    Exit For
                                End If
                            End If
                        Next

                        i = i + m

                    End If
                End If

                kk = kk + 1
            Loop

            i = 0
            kk = 0

            '채널 그룹을 식별한다.==================================================================================
            Dim CH_Group_Name() As String = Nothing  'tdm_channelgroup
            Dim CH_Group_Cnt() As String = Nothing '각 그룹마다 있는 채널 수를 판독
            Dim Tmp_CH_cnt() As String = Nothing

            Do While kk < UBound(TDM_Lines)

                kk = kk + 1
                Tmp_read = TDM_Lines(kk)

                If InStr(Tmp_read, "<tdm_channelgroup id=""") > 0 Then
                    kk = kk + 1
                    Tmp_read = TDM_Lines(kk)
                    i = i + 1
                    ReDim Preserve CH_Group_Name(i)
                    ReDim Preserve CH_Group_Cnt(i)
                    CH_Group_Name(i) = BetweenEle(Tmp_read)

                    Do While Not InStr(Tmp_read, "</tdm_channelgroup>") > 1
                        kk = kk + 1
                        Tmp_read = TDM_Lines(kk)
                        '채널그룹의 이름식별=========================================================================
                        If InStr(Tmp_read, "<name>") > 0 Then
                            CH_Group_Name(i) = BetweenEle(Tmp_read)
                        End If
                        '채널그룹내 채널 개수 식별====================================================================
                        '단일 채널인 경우에는 생기지 않는 Element임
                        If InStr(Tmp_read, "<channels>") > 0 Then
                            Tmp_CH_cnt = RTrim(LTrim(Tmp_read)).Split(" ")
                            CH_Group_Cnt(i) = Tmp_CH_cnt.Count
                        End If
                    Loop

                End If

                'kk = kk + 1
            Loop


            If UBound(CH_Group_Name) = 1 Then CH_Group_Cnt(1) = TotChs

            CH_Group_Cnt(0) = 0

            HeaderFile.Close()

            '채널 해더 배열의 정의####################################################################################
            ReDim DAT_CH(0 To CH_Group_Cnt(CH_Num) - 1, 0 To 8)
            'CH_names(채널명,0-데이터이름/1-시작위치/2-길이(갯수)/3-시작옵셋값/4-증분/5-Implicit여부/6-단위/7-소스파일/8-32및64구분)

            Dim Ch_Cnt_Tot As Integer = 0
            If UBound(CH_Group_Name) > 1 Then
                For i = 1 To CH_Num - 1
                    Ch_Cnt_Tot = Ch_Cnt_Tot + CH_Group_Cnt(i)
                Next
            Else
                Ch_Cnt_Tot = 0
            End If

            For i = 1 To CH_Group_Cnt(CH_Num) 'UBound(DAT_CH_names)
                DAT_CH(i - 1, 0) = DAT_CH_names(Ch_Cnt_Tot + i)
                DAT_CH(i - 1, 1) = DAT_CH_locate(Ch_Cnt_Tot + i)
                DAT_CH(i - 1, 2) = DAT_CH_len(Ch_Cnt_Tot + i)
                DAT_CH(i - 1, 3) = StartOffset(Ch_Cnt_Tot + i)
                DAT_CH(i - 1, 4) = IcrementFactor(Ch_Cnt_Tot + i)
                DAT_CH(i - 1, 5) = IsImplicit(Ch_Cnt_Tot + i)
                DAT_CH(i - 1, 6) = DAT_CH_Unit(Ch_Cnt_Tot + i)
                DAT_CH(i - 1, 7) = DAT_Source_File(Ch_Cnt_Tot + i)
                DAT_CH(i - 1, 8) = DAT_CH_Bit(Ch_Cnt_Tot + i)
            Next

            'For i = 1 To CH_Group_Cnt(CH_Num) 'UBound(DAT_CH_names)
            '    DAT_CH(i - 1, 0) = DAT_CH_names(CH_Group_Cnt(CH_Num - 1) + i)
            '    DAT_CH(i - 1, 1) = DAT_CH_locate(CH_Group_Cnt(CH_Num - 1) + i)
            '    DAT_CH(i - 1, 2) = DAT_CH_len(CH_Group_Cnt(CH_Num - 1) + i)
            '    DAT_CH(i - 1, 3) = StartOffset(CH_Group_Cnt(CH_Num - 1) + i)
            '    DAT_CH(i - 1, 4) = IcrementFactor(CH_Group_Cnt(CH_Num - 1) + i)
            '    DAT_CH(i - 1, 5) = IsImplicit(CH_Group_Cnt(CH_Num - 1) + i)
            '    DAT_CH(i - 1, 6) = DAT_CH_Unit(CH_Group_Cnt(CH_Num - 1) + i)
            '    DAT_CH(i - 1, 7) = DAT_Source_File(CH_Group_Cnt(CH_Num - 1) + i)
            'Next

            OepnTDMfileHeader = DAT_CH

        End If

    End Function



    'Reading R64 Binary File DATA
    'Public Function OpenBinaryR64(ByVal Values As Byte(), ByVal Start_Pos As Long, ByVal Val_Count As Long, _
    '                              Optional ByVal Factor As Single = 1.0, Optional ByVal Offset As Single = 0.0) As Double()

    'TDM Header Reading Related Functions
    Public Function FindXMLattributeByName(ByVal AttName As String, ByVal TargetText As String) As String
        '속성 이름으로 속성 값을 반환한다.
        Dim i As Integer
        Dim Tmp_Str() As String = TargetText.Split("=")
        Dim Tot_att As Integer = UBound(Tmp_Str) ' +1
        Dim AttriVal As String = ""

        For i = 1 To Tot_att
            If AttriGet(TargetText, i)(0) = AttName Then
                AttriVal = AttriGet(TargetText, i)(1)
                Exit For
            End If
        Next

        FindXMLattributeByName = AttriVal

    End Function

    Public Function AttriGet(ByVal StrLine As String, ByVal Num As Integer) As String()
        '속성값을 찾는다.
        Dim tmpstr1() As String
        Dim tmpstr2() As String
        Dim AttributeName As String
        Dim AttributeVal As String

        tmpstr1 = StrLine.Split("=")

        tmpstr2 = tmpstr1(Num - 1).Split(" ")

        AttributeName = tmpstr2(UBound(tmpstr2))

        Try
            tmpstr2 = Mid(tmpstr1(Num), 2, Len(tmpstr1(Num))).Split("""")
            AttributeVal = tmpstr2(LBound(tmpstr2))
        Catch ex As Exception
            AttributeVal = ""
        End Try

        AttriGet = {AttributeName, AttributeVal}

    End Function

    'TDM Header Reading Related Functions
    Public Function BetweenEle(ByVal StrLine As String) As String
        '<>....<> 사이를 읽어온다.

        Dim tmpstr1() As String
        Dim tmpstr2() As String

        tmpstr1 = StrLine.Split(">")
        tmpstr2 = tmpstr1(LBound(tmpstr1) + 1).Split("<")

        BetweenEle = tmpstr2(LBound(tmpstr2))

    End Function

    'XML의 테이블에서 숫자 추출
    Public Function SplitTableLine(ByVal TmpText As String) As String()

        Dim ReturnVal(0) As String
        Dim i, k As Integer

        If TmpText = "" Then
            ReDim Preserve ReturnVal(10)
            SplitTableLine = ReturnVal
            Exit Function
        End If

        TmpText = TmpText.Replace(Chr(9), " ")
        Dim tmp_str() As String = TmpText.Split(" ")

        k = 0

        For i = 0 To UBound(tmp_str)
            If IsNumeric(tmp_str(i)) Then
                ReDim Preserve ReturnVal(k)
                ReturnVal(k) = Trim(tmp_str(i))
                k = k + 1
            End If
        Next

        SplitTableLine = ReturnVal

    End Function

    Public Function Add_10_Letters(ByVal IDNumber As String) As String

        Dim SpaceNum As Integer
        Dim i As Integer

        '10글자가 넘어가면 앞에서 10개만 잘라버림
        If Len(IDNumber) > 10 Then
            Add_10_Letters = Mid(IDNumber, 1, 10)
            Exit Function
        End If

        SpaceNum = 10 - Len(IDNumber)
        For i = 1 To SpaceNum
            IDNumber = " " & IDNumber
        Next

        Add_10_Letters = IDNumber

    End Function

    Public Function Add_Dyna_Letters(ByVal IDNumber As String, ByVal Num As Integer) As String

        Dim SpaceNum As Integer
        Dim i As Integer

        Dim NumtoStr As Double = Format(CDbl(IDNumber), "####0.000000000000")
        IDNumber = CStr(NumtoStr)

        'Num글자가 넘어가면 앞에서 Num개만 잘라버림
        If Len(IDNumber) > Num Then
            Add_Dyna_Letters = Mid(IDNumber, 1, Num)
            Exit Function
        End If

        SpaceNum = Num - Len(IDNumber)
        For i = 1 To SpaceNum
            IDNumber = " " & IDNumber
        Next

        Add_Dyna_Letters = IDNumber

    End Function


    '임의의 축 A에 대한 P의 회전행렬 - 뭔가 이상함..점검 필요
    Public Function OrientationA(ByVal A_pt As Double(), ByVal P_pt As Double(), ByVal theta As Double) As Double()

        Dim Matrix_Ori(3, 3) As Double
        Dim Result(3) As Double

        theta = theta / (180.0 / Math.PI)

        Matrix_Ori(1, 1) = Math.Cos(theta) + (1 - Math.Cos(theta)) * (A_pt(1) ^ 2)
        Matrix_Ori(1, 2) = (1 - Math.Cos(theta)) * A_pt(1) * A_pt(2) - Math.Sin(theta) * A_pt(3)
        Matrix_Ori(1, 3) = (1 - Math.Cos(theta)) * A_pt(1) * A_pt(3) + Math.Sin(theta) * A_pt(2)

        Matrix_Ori(2, 1) = (1 - Math.Cos(theta)) * A_pt(1) * A_pt(2) + Math.Sin(theta) * A_pt(3)
        Matrix_Ori(2, 2) = Math.Cos(theta) + (1 - Math.Cos(theta)) * (A_pt(2) ^ 2)
        Matrix_Ori(2, 3) = (1 - Math.Cos(theta)) * A_pt(2) * A_pt(3) - Math.Sin(theta) * A_pt(1)

        Matrix_Ori(3, 1) = (1 - Math.Cos(theta)) * A_pt(1) * A_pt(3) - Math.Sin(theta) * A_pt(2)
        Matrix_Ori(3, 2) = (1 - Math.Cos(theta)) * A_pt(2) * A_pt(3) + Math.Sin(theta) * A_pt(1)
        Matrix_Ori(3, 3) = Math.Cos(theta) + (1 - Math.Cos(theta)) * (A_pt(3) ^ 2)

        Result(1) = P_pt(1) * Matrix_Ori(1, 1) + P_pt(2) * Matrix_Ori(1, 2) + P_pt(3) * Matrix_Ori(1, 3)
        Result(2) = P_pt(1) * Matrix_Ori(2, 1) + P_pt(2) * Matrix_Ori(2, 2) + P_pt(3) * Matrix_Ori(2, 3)
        Result(3) = P_pt(1) * Matrix_Ori(3, 1) + P_pt(2) * Matrix_Ori(3, 2) + P_pt(3) * Matrix_Ori(3, 3)

        Result(0) = 0

        OrientationA = Result

    End Function

    Public Function OrientationZ(ByVal P_pt As Double(), ByVal theta As Double) As Double()

        Dim Matrix_Ori(3, 3) As Double
        Dim Result(3) As Double

        theta = theta / (180.0 / Math.PI)

        Matrix_Ori(1, 1) = Math.Cos(theta)
        Matrix_Ori(1, 2) = Math.Sin(theta)
        Matrix_Ori(1, 3) = 0

        Matrix_Ori(2, 1) = -Math.Sin(theta)
        Matrix_Ori(2, 2) = Math.Cos(theta)
        Matrix_Ori(2, 3) = 0

        Matrix_Ori(3, 1) = 0
        Matrix_Ori(3, 2) = 0
        Matrix_Ori(3, 3) = 1

        Result(1) = P_pt(1) * Matrix_Ori(1, 1) + P_pt(2) * Matrix_Ori(1, 2) + P_pt(3) * Matrix_Ori(1, 3)
        Result(2) = P_pt(1) * Matrix_Ori(2, 1) + P_pt(2) * Matrix_Ori(2, 2) + P_pt(3) * Matrix_Ori(2, 3)
        Result(3) = P_pt(1) * Matrix_Ori(3, 1) + P_pt(2) * Matrix_Ori(3, 2) + P_pt(3) * Matrix_Ori(3, 3)

        Result(0) = 0

        OrientationZ = Result

    End Function

    Public Function OrientationY(ByVal P_pt As Double(), ByVal theta As Double) As Double()

        Dim Matrix_Ori(3, 3) As Double
        Dim Result(3) As Double

        theta = theta / (180.0 / Math.PI)

        Matrix_Ori(1, 1) = Math.Cos(theta)
        Matrix_Ori(1, 2) = 0
        Matrix_Ori(1, 3) = Math.Sin(theta)

        Matrix_Ori(2, 1) = 0
        Matrix_Ori(2, 2) = 1
        Matrix_Ori(2, 3) = 0

        Matrix_Ori(3, 1) = -Math.Sin(theta)
        Matrix_Ori(3, 2) = 0
        Matrix_Ori(3, 3) = Math.Cos(theta)

        Result(1) = P_pt(1) * Matrix_Ori(1, 1) + P_pt(2) * Matrix_Ori(1, 2) + P_pt(3) * Matrix_Ori(1, 3)
        Result(2) = P_pt(1) * Matrix_Ori(2, 1) + P_pt(2) * Matrix_Ori(2, 2) + P_pt(3) * Matrix_Ori(2, 3)
        Result(3) = P_pt(1) * Matrix_Ori(3, 1) + P_pt(2) * Matrix_Ori(3, 2) + P_pt(3) * Matrix_Ori(3, 3)

        Result(0) = 0

        OrientationY = Result

    End Function

    Public Function HICfromDATA(ByVal Time() As Decimal, ByVal Val() As Double) As Double()

        Dim HIC15, HIC36 As Double

        'Time 은 msec 단위, Val 은 G 단위로 들어오는 것을 가정한다.
        Dim i, k As Integer
        Dim DataPt As Integer = CInt(15 / (Time(1) - Time(0))) ' + 1

        Dim TmpLocalSum As Double = 0.0
        Dim MaximaValue As Double = -1.0E+15
        ' Dim MaximaStartIndex As Integer = 0
        'Dim MaximaEndIndex As Integer = 0
        Dim MinimaValue As Double = 1.0E+15

        'HIC15
        For i = 0 To UBound(Val) + 1 - DataPt - 1
            For k = 0 To DataPt - 1
                'Summation
                TmpLocalSum = TmpLocalSum + (Val(i + k) + Val(i + k + 1)) / 2
            Next
            If MaximaValue < TmpLocalSum Then
                MaximaValue = TmpLocalSum
                'MaximaStartIndex = i
                'MaximaEndIndex = i + k
            End If
            TmpLocalSum = 0.0
        Next

        'MaximaValue = (MaximaValue / 10000)
        MaximaValue = (MaximaValue / (1000 / (Time(1) - Time(0))))

        HIC15 = ((MaximaValue / 0.015) ^ (2.5)) * 0.015


        'HIC36
        DataPt = CInt(36 / (Time(1) - Time(0))) '+ 1
        For i = 0 To UBound(Val) + 1 - DataPt - 1
            For k = 0 To DataPt - 1
                'Summation
                TmpLocalSum = TmpLocalSum + (Val(i + k) + Val(i + k + 1)) / 2
            Next
            If MaximaValue < TmpLocalSum Then
                MaximaValue = TmpLocalSum
                'MaximaStartIndex = i
                'MaximaEndIndex = i + k - 1
            End If
            TmpLocalSum = 0.0
        Next

        'MaximaValue = MaximaValue / 10000
        MaximaValue = (MaximaValue / (1000 / (Time(1) - Time(0))))

        HIC36 = ((MaximaValue / 0.036) ^ (2.5)) * 0.036

        HICfromDATA = {HIC15, HIC36}

    End Function

    Public Function Acc3MSfromDATA(ByVal Time() As Decimal, ByVal Val() As Double) As Double

        Dim Acc3MS As Double

        'Time 은 msec 단위, Val 은 G 단위로 들어오는 것을 가정한다.
        Dim i, k As Integer
        Dim DataPt As Integer = CInt(3 / (Time(1) - Time(0))) ' + 1

        Dim TmpLocalSum As Double = 0.0
        Dim MaximaValue As Double = -1.0E+15
        Dim MinimaValue As Double = 1.0E+15

        '3MS
        For i = 0 To UBound(Val) - DataPt - 1
            For k = 1 To DataPt
                'Summation
                TmpLocalSum = TmpLocalSum + Val(i + k - 1)
            Next
            TmpLocalSum = TmpLocalSum + Val(i + k)
            If MaximaValue < TmpLocalSum Then
                MaximaValue = TmpLocalSum
            End If
            TmpLocalSum = 0.0
        Next

        Acc3MS = (MaximaValue / (DataPt + 1))

        Acc3MSfromDATA = Acc3MS

    End Function

    Public Function CalNTEDATA(ByVal Neck_Fz() As Double, ByVal Neck_Mocy() As Double, ByVal DummyType As Integer) As Double()
        '0=50% H-3 / 1=5% H-3 / 2=50%THOR
        Dim i As Integer
        Dim Temp_Nij(UBound(Neck_Fz)) As Double
        If DummyType = 0 Or DummyType = 2 Then
            For i = 0 To UBound(Neck_Fz)
                If Neck_Fz(i) >= 0 And Neck_Mocy(i) <= 0 Then
                    Temp_Nij(i) = Neck_Fz(i) / 6806.0 + Neck_Mocy(i) / (-135.0)
                Else
                    Temp_Nij(i) = 0
                End If
            Next
        ElseIf DummyType = 1 Then
            For i = 0 To UBound(Neck_Fz)
                If Neck_Fz(i) >= 0 And Neck_Mocy(i) <= 0 Then
                    Temp_Nij(i) = Neck_Fz(i) / 4287.0 + Neck_Mocy(i) / (-67.0)
                Else
                    Temp_Nij(i) = 0
                End If
            Next
        End If

        CalNTEDATA = Temp_Nij
    End Function
    Public Function CalNTFDATA(ByVal Neck_Fz() As Double, ByVal Neck_Mocy() As Double, ByVal DummyType As Integer) As Double()
        '0=50% H-3 / 1=5% H-3 / 2=50%THOR
        Dim i As Integer
        Dim Temp_Nij(UBound(Neck_Fz)) As Double
        If DummyType = 0 Or DummyType = 2 Then
            For i = 0 To UBound(Neck_Fz)
                If Neck_Fz(i) >= 0 And Neck_Mocy(i) >= 0 Then
                    Temp_Nij(i) = Neck_Fz(i) / 6806.0 + Neck_Mocy(i) / (310.0)
                Else
                    Temp_Nij(i) = 0
                End If
            Next
        ElseIf DummyType = 1 Then
            For i = 0 To UBound(Neck_Fz)
                If Neck_Fz(i) >= 0 And Neck_Mocy(i) >= 0 Then
                    Temp_Nij(i) = Neck_Fz(i) / 4287.0 + Neck_Mocy(i) / (155.0)
                Else
                    Temp_Nij(i) = 0
                End If
            Next
        End If

        CalNTFDATA = Temp_Nij
    End Function
    Public Function CalNCEDATA(ByVal Neck_Fz() As Double, ByVal Neck_Mocy() As Double, ByVal DummyType As Integer) As Double()
        '0=50% H-3 / 1=5% H-3 / 2=50%THOR
        Dim i As Integer
        Dim Temp_Nij(UBound(Neck_Fz)) As Double
        If DummyType = 0 Or DummyType = 2 Then
            For i = 0 To UBound(Neck_Fz)
                If Neck_Fz(i) <= 0 And Neck_Mocy(i) <= 0 Then
                    Temp_Nij(i) = Neck_Fz(i) / (-6160.0) + Neck_Mocy(i) / (-135.0)
                Else
                    Temp_Nij(i) = 0
                End If
            Next
        ElseIf DummyType = 1 Then
            For i = 0 To UBound(Neck_Fz)
                If Neck_Fz(i) <= 0 And Neck_Mocy(i) <= 0 Then
                    Temp_Nij(i) = Neck_Fz(i) / (-3880.0) + Neck_Mocy(i) / (-67.0)
                Else
                    Temp_Nij(i) = 0
                End If
            Next
        End If

        CalNCEDATA = Temp_Nij
    End Function
    Public Function CalNCFDATA(ByVal Neck_Fz() As Double, ByVal Neck_Mocy() As Double, ByVal DummyType As Integer) As Double()
        '0=50% H-3 / 1=5% H-3 / 2=50%THOR
        Dim i As Integer
        Dim Temp_Nij(UBound(Neck_Fz)) As Double
        If DummyType = 0 Or DummyType = 2 Then
            For i = 0 To UBound(Neck_Fz)
                If Neck_Fz(i) <= 0 And Neck_Mocy(i) >= 0 Then
                    Temp_Nij(i) = Neck_Fz(i) / (-6160.0) + Neck_Mocy(i) / (310.0)
                Else
                    Temp_Nij(i) = 0
                End If
            Next
        ElseIf DummyType = 1 Then
            For i = 0 To UBound(Neck_Fz)
                If Neck_Fz(i) <= 0 And Neck_Mocy(i) >= 0 Then
                    Temp_Nij(i) = Neck_Fz(i) / (-3880.0) + Neck_Mocy(i) / (155.0)
                Else
                    Temp_Nij(i) = 0
                End If
            Next
        End If

        CalNCFDATA = Temp_Nij
    End Function

    Public Function CalMocyDATA(ByVal Neck_Fx() As Double, ByVal Neck_My() As Double, ByVal DummyType As Integer) As Double()
        '0=50% H-3 / 1=5% H-3 / 2=50%THOR
        Dim i As Integer
        Dim Mocy As Double()
        Dim d As Double

        If DummyType = 0 Or DummyType = 1 Then
            d = 0.01778
        ElseIf DummyType = 2 Then
            d = 0.01778
        End If

        ReDim Mocy(UBound(Neck_My))

        For i = 0 To UBound(Neck_My)
            Mocy(i) = Neck_My(i) - d * Neck_Fx(i)
        Next

        CalMocyDATA = Mocy
    End Function

    Public Function ChartDataSamplingIntervals(ByRef Chrt As Chart) As Double()
        Dim i As Integer
        Dim ConvertingFactor(Chrt.Series.Count - 1) As Double

        ConvertingFactor(0) = 1

        Dim Firstinterval As Double = Math.Abs((Chrt.Series(0).Points(1).XValue - Chrt.Series(0).Points(0).XValue))

        With Chrt
            For i = 1 To .Series.Count - 1
                Dim OtherIntervals As Double = Math.Abs((.Series(i).Points(1).XValue - .Series(i).Points(0).XValue))
                If Math.Abs(Firstinterval - OtherIntervals) > (Firstinterval * 0.1) Then
                    If Firstinterval > OtherIntervals Then
                        ConvertingFactor(i) = Firstinterval / OtherIntervals
                    Else
                        ConvertingFactor(i) = -1 * (Firstinterval / OtherIntervals)
                    End If
                Else
                    ConvertingFactor(i) = 1
                End If
            Next
        End With

        ChartDataSamplingIntervals = ConvertingFactor
    End Function

    Public Function ExtractingChrtDataY(ByRef Chrt As Chart, ByVal SeriesNum As Integer, ByVal Factor As Integer) As Double()
        Dim i, k As Integer
        Dim DataPt() As Double = Nothing

        For i = 0 To Chrt.Series(SeriesNum).Points.Count - 1 Step Factor
            ReDim Preserve DataPt(k)
            DataPt(k) = Chrt.Series(SeriesNum).Points(i).YValues(0)
            k = k + 1
        Next

        ExtractingChrtDataY = DataPt
    End Function

    Public Function ExtractingChrtDataX(ByRef Chrt As Chart, ByVal SeriesNum As Integer, ByVal Factor As Integer) As Double()
        Dim i, k As Integer
        Dim DataPt() As Double = Nothing

        For i = 0 To Chrt.Series(SeriesNum).Points.Count - 1 Step Factor
            ReDim Preserve DataPt(k)
            DataPt(k) = Chrt.Series(SeriesNum).Points(i).XValue
            k = k + 1
        Next

        ExtractingChrtDataX = DataPt
    End Function

    Public Function LinearInterpolation(ByRef Chrt As Chart, ByVal Series As Integer, _
                                       ByVal StartVal As Double, ByVal EndVal As Double, ByVal Interval As Double) As Double()

        Dim ResultArr_Y() As Double
        Dim ResultArr_X() As Double
        Dim i As Single
        Dim k As Integer = 0
        Dim StartIndex As Integer
        Dim TmpSeriesPoints As DataPointCollection

        TmpSeriesPoints = Chrt.Series(Series).Points

        'Find 0.0 index for start index 
        Do While (0.0 - TmpSeriesPoints(k).XValue) > 0.0
            k = k + 1
        Loop
        StartIndex = k

        Dim ForeXIndex As Integer = StartIndex
        Dim RearXIndex As Integer = StartIndex
        Dim m As Integer = StartIndex
        Dim n As Integer = StartIndex

        ReDim ResultArr_X(CInt((EndVal - StartVal) / Interval))
        ReDim ResultArr_Y(CInt((EndVal - StartVal) / Interval))
        Dim t As Integer = 0

        'Chrt.Series.Add("dd")

        If StartVal < 0.0 Then StartVal = 0.0
        For i = StartVal To EndVal - Interval Step Interval
            'For i = 0.0 To EndVal - Interval Step Interval

            '앞쪽 인덱스를 구함 
            Do While TmpSeriesPoints(m).XValue <= i
                ForeXIndex = m
                m = m + 1
            Loop

            '뒷쪽 인덱스를 구함
            Do While TmpSeriesPoints(n).XValue <= i
                n = n + 1
            Loop
            RearXIndex = n

            '계산 - Calculate the Y value by Linear Interpolation at X=i
            ResultArr_X(t) = i
            ResultArr_Y(t) = TmpSeriesPoints(ForeXIndex).YValues(0) + _
                                         ((TmpSeriesPoints(RearXIndex).YValues(0) - TmpSeriesPoints(ForeXIndex).YValues(0)) / _
                                         (TmpSeriesPoints(RearXIndex).XValue - TmpSeriesPoints(ForeXIndex).XValue)) * _
                                         (i - TmpSeriesPoints(ForeXIndex).XValue)
            t = t + 1

            'Chrt.Series("dd").Points.AddXY(ResultArr_X(t - 1), ResultArr_Y(t - 1))
        Next

        'Chrt.Series("dd").ChartType = SeriesChartType.Line
        LinearInterpolation = ResultArr_Y

    End Function

    Public Function FindDUC(ByVal Ref_Arr() As Double, ByVal Target_Arr() As Double, ByVal X_Interval As Double) As Double()

        Dim i As Integer
        Dim DUC(1) As Double

        '각 그래프 적분
        Dim Target_Area As Double
        Dim Ref_Area As Double
        Dim Diff_Area As Double

        For i = 1 To UBound(Ref_Arr) Step 1
            Target_Area = Target_Area + ((Math.Abs(Target_Arr(i - 1)) + Math.Abs(Target_Arr(i))) / 2) * X_Interval
            Ref_Area = Ref_Area + ((Math.Abs(Ref_Arr(i - 1)) + Math.Abs(Ref_Arr(i))) / 2) * X_Interval
            Diff_Area = Diff_Area + ((Math.Abs(Math.Abs(Target_Arr(i - 1)) - Math.Abs(Ref_Arr(i - 1))) _
                                  + Math.Abs(Math.Abs(Target_Arr(i)) - Math.Abs(Ref_Arr(i)))) / 2) * _
                                  X_Interval
        Next

        ' ''**DUC계산 (the yellow area - the green area) / (yellow+orange area)
        Dim Cm_Area As Double = 0.0
        Dim tmp_green As Double = 0.0
        Dim tmp_yell As Double = 0.0
        Cm_Area = ((Target_Area + Ref_Area) - Math.Abs(Diff_Area)) / 2
        Dim Tmp_Max As Double
        If Target_Area > Ref_Area Then
            Tmp_Max = Target_Area
        Else
            Tmp_Max = Ref_Area
        End If
        '분모가 해석(Target) 기준일때 (Factor Method) **************************
        tmp_yell = Target_Area - Cm_Area
        tmp_green = Ref_Area - Cm_Area
        DUC(0) = 1 - Math.Abs((tmp_yell - tmp_green) / Tmp_Max)
        If DUC(0) <= 0.00001 Then DUC(0) = 0.0
        '분모가 시험(Reference) 기준일때 (Relative Method) ************************
        tmp_yell = Ref_Area - Cm_Area
        tmp_green = Target_Area - Cm_Area
        DUC(1) = 1 - Math.Abs((tmp_yell - tmp_green) / Ref_Area)
        If DUC(1) <= 0.00001 Then DUC(1) = 0.0
        '********************************************************************

        FindDUC = DUC

    End Function

    Public Function FindWIFac(ByVal Ref_Arr() As Double, ByVal Target_Arr() As Double, ByVal X_Interval As Double) As Double

        Dim i As Integer = 0
        Dim term_1 As Double = 0.0
        Dim term_2 As Double = 0.0
        Dim term_3 As Double = 0.0
        Dim numerator_1 As Double = 0.0
        Dim denominator_1 As Double = 0.0

        For i = 0 To UBound(Ref_Arr) Step 1
            term_1 = Math.Max(0.0, Target_Arr(i) * Ref_Arr(i))
            term_2 = Math.Max(0.000001, Math.Max(Target_Arr(i) ^ 2, Ref_Arr(i) ^ 2))
            term_3 = Math.Max(Target_Arr(i) ^ 2, Ref_Arr(i) ^ 2) * (1 - term_1 / term_2) ^ 2
            numerator_1 = numerator_1 + term_3     '분자
            denominator_1 = denominator_1 + term_2 '분모
        Next

        FindWIFac = 1 - Math.Sqrt(numerator_1 / denominator_1)

    End Function

    'Public Function FindWIRel(ByVal Ref_Arr() As Double, ByVal Target_Arr() As Double, ByVal X_Interval As Double) As Double

    '    '안맞음;;;;;
    '    Dim i As Integer = 0
    '    Dim term_1 As Double = 0.0
    '    Dim term_2 As Double = 0.0
    '    Dim term_3 As Double = 0.0
    '    Dim numerator_1 As Double = 0.0
    '    Dim denominator_1 As Double = 0.0

    '    For i = 0 To UBound(Ref_Arr) Step 1
    '        term_1 = 1 - (Math.Abs(Ref_Arr(i) - Target_Arr(i)) _
    '                    / Math.Max(0.000001, Math.Abs(Ref_Arr(i))))
    '        term_2 = (Ref_Arr(i) ^ 2) * (1 - Math.Max(0.0, term_1) ^ 2)
    '        term_3 = Ref_Arr(i) ^ 2
    '        numerator_1 = numerator_1 + term_2     '분자
    '        denominator_1 = denominator_1 + term_3 '분모
    '    Next

    '    FindWIRel = 1 - Math.Sqrt(numerator_1 / Math.Max(0.00001, denominator_1))

    'End Function

    Public Function FindSnG(ByVal Ref_Arr() As Double, ByVal Target_Arr() As Double, ByVal X_Interval As Double) As Double()

        Dim i As Integer
        Dim Irr As Double = 0.0
        Dim Iss As Double = 0.0
        Dim Irs As Double = 0.0
        Dim Sng As Double = 0.0
        Dim SnG_P As Double = 0.0
        Dim SnG_M As Double = 0.0
        Dim SnG_Arr(2) As Double

        For i = 1 To UBound(Ref_Arr) Step 1
            Iss = Iss + ((Target_Arr(i - 1) ^ 2 + Target_Arr(i) ^ 2) / 2) * X_Interval 'ss
            Irr = Irr + ((Ref_Arr(i - 1) ^ 2 + Ref_Arr(i) ^ 2) / 2) * X_Interval  'rr
            Irs = Irs + ((Target_Arr(i - 1) * Ref_Arr(i - 1) + Target_Arr(i) * Ref_Arr(i)) / 2) * X_Interval
        Next

        If Iss = 0 Then
            Iss = 0.0000000001
        End If
        If Irs = 0 Then
            Irs = 0.0000000001
        End If
        If Irr = 0 Then
            Irr = 0.0000000001
        End If

        SnG_P = (1 / 3.141592) * Math.Acos(Irs / Math.Sqrt(Irr * Iss))
        SnG_M = Math.Sqrt(Iss / Irr) - 1
        Sng = Math.Sqrt(SnG_P ^ 2 + SnG_M ^ 2)

        SnG_Arr(0) = 1 - Math.Abs(Sng)
        SnG_Arr(1) = 1 - Math.Abs(SnG_M)
        SnG_Arr(2) = 1 - Math.Abs(SnG_P)

        For i = 0 To 2
            If SnG_Arr(i) < 0 Then
                SnG_Arr(i) = 0
            End If
        Next

        FindSnG = SnG_Arr

    End Function

    Public Function GPTV(ByVal Ref_Arr() As Double, ByVal Target_Arr() As Double, ByVal X_Interval As Double) As Double()
        'Factor Method와 Relative의 차이가 없다.

        Dim i As Integer
        Dim Tmp_GPV As Double
        Dim Tmp_GPT As Double
        Dim Tmp_GPT_Ref As Integer
        Dim Tmp_GPT_Target As Integer

        If (Math.Abs(Ref_Arr.Max) - Math.Abs(Ref_Arr.Min)) < 0 Then
            Tmp_GPV = (Math.Abs(Ref_Arr.Min) * Math.Abs(Target_Arr.Min)) / Math.Max(Math.Abs(Ref_Arr.Min) ^ 2, Math.Abs(Target_Arr.Min) ^ 2)
            For i = 0 To UBound(Ref_Arr)
                If Ref_Arr(i) = Ref_Arr.Min Then
                    Tmp_GPT_Ref = i
                    Exit For
                End If
            Next
            For i = 0 To UBound(Target_Arr)
                If Target_Arr(i) = Target_Arr.Min Then
                    Tmp_GPT_Target = i
                    Exit For
                End If
            Next
        Else
            Tmp_GPV = (Math.Abs(Ref_Arr.Max) * Math.Abs(Target_Arr.Max)) / Math.Max(Math.Abs(Ref_Arr.Max) ^ 2, Math.Abs(Target_Arr.Max) ^ 2)
            For i = 0 To UBound(Ref_Arr)
                If Ref_Arr(i) = Ref_Arr.Max Then
                    Tmp_GPT_Ref = i
                    Exit For
                End If
            Next
            For i = 0 To UBound(Target_Arr)
                If Target_Arr(i) = Target_Arr.Max Then
                    Tmp_GPT_Target = i
                    Exit For
                End If
            Next
        End If

        Tmp_GPT = Tmp_GPT_Ref * Tmp_GPT_Target / Math.Max(Tmp_GPT_Ref ^ 2, Tmp_GPT_Target ^ 2)

        GPTV = {Tmp_GPV, Tmp_GPT}
    End Function
End Module
