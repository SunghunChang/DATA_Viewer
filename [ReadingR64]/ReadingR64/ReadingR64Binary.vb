Public Class ReadingR64Binary

    ''' <summary>
    ''' Reading R64 Binary File DATA
    ''' </summary>
    ''' <param name="Values">Binary Value</param>
    ''' <param name="Start_Pos">Start Position</param>
    ''' <param name="Val_Count">Count</param>
    ''' <param name="Factor">Scale Factor</param>
    ''' <param name="Offset">Offset Factor</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function OpenBinaryR64(ByVal Values As Byte(), ByVal Start_Pos As Long, ByVal Val_Count As Long, _
                                  Optional ByVal Factor As Single = 1.0, Optional ByVal Offset As Single = 0.0) As Double()
        Dim j, k, ByteNum(8), NumSign, Byte6Hex, Fraction, Exponent, Ticks52
        Ticks52 = 2 ^ 52                           ' the stored Fraction is 52 bits wide

        Dim ReData() As Double 'Return Data
        ReDim ReData(0 To Val_Count - 1)

        For j = 0 To Val_Count - 1
            'If Endian = "LV" Then Values(j) = StrReverse(Values(j)) ' Big -> Little Endian
            For k = 0 To 7
                'ByteNum(k) = Asc(Mid(Values(j), k + 1, 1))
                ByteNum(k) = Values(Start_Pos + j * 8 + k)
            Next ' k
            NumSign = 1
            If ByteNum(7) > 127 Then
                NumSign = -1
                ByteNum(7) = ByteNum(7) - 128        ' set the sign (most significant) bit = 0
            End If ' the current (j) R64Val is negative
            Byte6Hex = Right("0" & Hex(ByteNum(6)), 2)
            Exponent = CLng("&H" & Hex(ByteNum(7)) & Left(Byte6Hex, 1)) ' Byte6 high nibble
            If Exponent = 2047 Then
                Values(j) = vbNull                      ' NoValue (+INF, -INF, NaN)
            Else ' Value(j) is a viable number
                ByteNum(6) = CLng("&H" & Right(Byte6Hex, 1))              ' Byte6 low  nibble
                Fraction = 0
                For k = 0 To 6
                    Fraction = Fraction + ByteNum(k) * (256 ^ k)
                Next ' k
                If Exponent = 0 Then
                    If Fraction = 0 Then
                        Fraction = 0                     ' Zero
                    Else
                        Fraction = Fraction / Ticks52    ' subnormal number
                        Exponent = 1                     ' subnormal number
                    End If ' Fraction = 0 (all the Fraction bits were 0)
                Else
                    Fraction = 1 + Fraction / Ticks52  ' the stored Fraction is 52 bits wide
                End If ' Exponent = 0 (all Exponent bits were 0)

                'Values(j) = NumSign * Fraction * (2 ^ (Exponent - 1023))

                ReData(j) = CDbl(NumSign * Fraction * (2 ^ (Exponent - 1023))) * Factor + Offset

            End If ' the current (j) R64Val is a NoValue (+INF, -INF, NaN)
        Next ' j

        OpenBinaryR64 = ReData

    End Function

    'Sub FromR32BinStr(Endian, Values)
    ''' <summary>
    ''' Reading R32 Binary File DATA
    ''' </summary>
    ''' <param name="Values"></param>
    ''' <param name="Start_Pos"></param>
    ''' <param name="Val_Count"></param>
    ''' <param name="Factor"></param>
    ''' <param name="Offset"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function OpenBinaryR32(ByVal Values As Byte(), ByVal Start_Pos As Long, ByVal Val_Count As Long, _
                                      Optional ByVal Factor As Single = 1.0, Optional ByVal Offset As Single = 0.0) As Double()
        Dim j, k, ByteNum(4), NumSign, Fraction, Exponent, Ticks23, ByteMult(3)
        Ticks23 = 2 ^ 23

        Dim ReData() As Double 'Return Data
        ReDim ReData(0 To Val_Count - 1)

        For k = 0 To 2
            ByteMult(k) = 256 ^ k
        Next ' k

        For j = 0 To Val_Count - 1
            'IF Len(Values(j)) < 4 THEN
            '  Values(j) = Null
            'ELSE
            'IF Endian = "LV" THEN Values(j) = StrReverse(Values(j)) ' Big -> Little Endian
            For k = 0 To 3
                'ByteNum(k) = Asc(Mid(Values(j), k+1, 1))
                ByteNum(k) = Values(Start_Pos + j * 4 + k)
            Next ' k
            NumSign = 1
            If ByteNum(3) > 127 Then
                NumSign = -1
                ByteNum(3) = ByteNum(3) - 128          ' set the sign (most sign.) bit = 0
            End If ' the current (j) R32Val is negative
            Exponent = 2 * ByteNum(3) + ByteNum(2) \ 128 ' left-shift(Byte3) + high-bit(Byte2)
            If Exponent = 255 Then
                Values(j) = vbNull                       ' NoValue (+INF, -INF, NaN)
            Else ' NOT NoValue
                If ByteNum(2) > 127 Then ByteNum(2) = ByteNum(2) - 128 ' ignore highest-bit
                Fraction = 0
                For k = 0 To 2
                    Fraction = Fraction + ByteNum(k) * ByteMult(k)
                Next ' k
                If Exponent = 0 Then
                    If Fraction = 0 Then
                        Fraction = 0
                    Else
                        Fraction = 0.5                     ' subnormal numbers
                    End If ' Fraction = 0 (all the Fraction bits were 0)
                Else
                    Fraction = 1 + Fraction / Ticks23      ' the stored Fraction is 23 bits wide
                End If ' Exponent = 0 (all Exponent bits were 0)
                'Values(j) = NumSign*Fraction*(2^(Exponent-127))
                ReData(j) = CDbl(NumSign * Fraction * (2 ^ (Exponent - 127))) * Factor + Offset
            End If ' the current (j) R32Val is a NoValue (+INF, -INF, NaN)
            'END IF ' the current (j) Value is at least 4 Bytes long
        Next ' j

        OpenBinaryR32 = ReData

    End Function ' FromR32BinStr()

End Class
