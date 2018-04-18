Module Globalvb

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
End Module
