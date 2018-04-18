Imports System.IO

Public Class ReadingAnalysisDATA
    '데이터의 행/열/총 행수/총 열수 위치를 반환한다.
    Public Function FileRowCol(ByVal Fpath As String, ByVal Fnames As String, ByVal Ext As String, ByVal Para() As String) As Integer()
        'Para() 변수는 여러개 파라미터가 String으로 합쳐져있다. 
        ' 0 : 행식별자 이름  /  1 : 열식별자 이름  /  3  : Conversion Factor ..............식별자 (",")
        '각 인덱스 안에는 여러 항목이 식별자 (",") 로 합쳐져있다.
        Dim FileReading As StreamReader
        Dim Read_Str As String
        Dim Tmp_Str() As String
        Dim Tot_Row As Integer
        Dim Tot_Col As Integer
        Dim Tmp_Para() As String
        Dim k, j, m As Integer
        Dim Row As Integer = -1
        Dim Col As Integer = -1
        Dim IsRow As Integer = -1
        Dim IsColumn As Integer = -1

        ReDim FileRowCol(1)

        Try

            Dim ReadFiles As New FileStream(Fpath & Fnames & Ext, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
            '맨 마지막 상수를 FileShare.ReadWrite 로 지정해야 해석 도중 접근이 가능함

            FileReading = New StreamReader(ReadFiles)
            Read_Str = FileReading.ReadLine
            Read_Str = FileReading.ReadLine
            Tmp_Str = RTrim(LTrim(FileReading.ReadLine)).Split(" ")

            Tot_Row = CInt(Tmp_Str(LBound(Tmp_Str)))
            Tot_Col = CInt(Tmp_Str(UBound(Tmp_Str)))

            Do While Not FileReading.EndOfStream

                Tmp_Para = Para(0).Split(",")

                For k = 1 To Tot_Row
                    Read_Str = FileReading.ReadLine
                    For j = 0 To UBound(Tmp_Para)
                        If InStr(Read_Str, Tmp_Para(j)) >= 1 Then   '행 식별자가 있으면 저장
                            Row = k
                            IsRow = IsRow + 1
                        End If
                    Next
                    If (Row <> -1) And (IsRow = UBound(Tmp_Para)) Then
                        For m = k + 1 To Tot_Row
                            Read_Str = FileReading.ReadLine
                        Next
                        Exit For
                    End If
                Next

                Tmp_Para = Para(1).Split(",")

                For k = 1 To Tot_Col
                    Read_Str = FileReading.ReadLine
                    For j = 0 To UBound(Tmp_Para)
                        If InStr(Read_Str, Tmp_Para(j)) >= 1 Then  '열 식별자가 있으면 저장
                            Col = k
                            IsColumn = IsColumn + 1
                        End If
                    Next
                    If (Col <> -1) And (IsColumn = UBound(Tmp_Para)) Then
                        For m = k + 1 To Tot_Col
                            Read_Str = FileReading.ReadLine
                        Next
                        Exit For
                    End If
                Next

                '데이터를 읽는다.
                Read_Str = FileReading.ReadLine
            Loop

            FileReading.Close()
            ReadFiles.Close()
        Catch

            If Ext = "None" Then
                Row = 0
                Col = 0
                Tot_Row = 0
                Tot_Col = 0
            End If

        End Try

        If Ext = "None" Then
            Row = 0
            Col = 0
            Tot_Row = 0
            Tot_Col = 0
        End If

        If (Row = -1) Or (Col = -1) Then
            Row = 0
            Col = 0
            Tot_Row = 0
            Tot_Col = 0
        End If

        FileRowCol = {Row, Col, Tot_Row, Tot_Col}
    End Function

    '해석 데이터 읽어오는 함수
    Public Sub DataReading(ByVal FilePath As String, ByVal FileNames As String, ByVal Exts As String _
                                , ByVal Rows As Integer, ByVal Cols As Integer, ByVal TotRows As Integer, ByVal TotCols As Integer _
                                , ByRef Time() As Decimal, ByRef Vals() As Double, _
                                Optional ByVal C_Factor As Decimal = 1.0, _
                                Optional ByVal X_Scale As Single = 1.0, _
                                Optional ByVal X_Offset As Single = 0.0, _
                                Optional ByVal Y_Offset As Single = 0.0)
        '(Y Scale / X Scale / X Offset / Y Offset)  ==>> 옵션 파라미터 순서 중요!!!!!
        Dim i As Integer
        Dim Tmp_Str As String
        Dim Block_Cnt As Integer
        Dim Global_Cnt As Integer
        Dim File_num As StreamReader


        Block_Cnt = 0
        Global_Cnt = -1

        If Exts = ".None" OrElse Cols = 0 OrElse Rows = 0 Then

            ReDim Time(0)
            ReDim Vals(0)

        Else

            Dim ReadFiles As New FileStream(FilePath & FileNames & Exts, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
            File_num = New StreamReader(ReadFiles)

            For i = 1 To TotRows + TotCols + 3
                Tmp_Str = File_num.ReadLine
            Next

            Do While Not File_num.EndOfStream

                Tmp_Str = File_num.ReadLine
                Global_Cnt = Global_Cnt + 1

                Select Case Global_Cnt Mod (TotRows + 1)
                    Case 0
                        ReDim Preserve Time(Block_Cnt)
                        ReDim Preserve Vals(Block_Cnt)
                        Time(Block_Cnt) = CSng(Tmp_Str) * X_Scale + X_Offset
                        Block_Cnt = Block_Cnt + 1
                    Case Rows
                        Try
                            Vals(Block_Cnt - 1) = CDbl(Mid$(Tmp_Str, 15 * (Cols - 1) + 1, 15)) * C_Factor + Y_Offset
                            '                                                               단위변환 Factor
                        Catch ex As Exception
                            '없으면 그냥 넘긴다. (끝내버린다. 해석 도중 읽는 것을 처리함)
                        End Try
                    Case Else

                End Select
            Loop

            File_num.Close()
            ReadFiles.Close()

        End If

    End Sub

End Class
