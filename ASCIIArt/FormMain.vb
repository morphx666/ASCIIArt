Public Class FormMain
    Private Sub FormMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AsciiArtCtrl.Charset = ASCIIArtCtrl.Charsets.Standard
        AsciiArtCtrl.ColorMode = ASCIIArtCtrl.ColorModes.GrayScale
        AsciiArtCtrl.ScanMode = ASCIIArtCtrl.ScanModes.Fast
        AsciiArtCtrl.GrayScaleMode = ASCIIArtCtrl.GrayscaleModes.Average
        AsciiArtCtrl.BackColor = Color.Black

        SetupControls()
    End Sub

    Private Sub SetupControls()
        Dim AddItems = Sub(c As ComboBox, values As Type, selected As Object)
                           Dim v = values.GetEnumValues()
                           Dim n = values.GetEnumNames()

                           For i As Integer = 0 To v.Length - 1
                               c.Items.Add(n(i))
                               If v(i) = selected Then c.SelectedItem = n(i)
                           Next
                           c.Tag = values
                       End Sub

        AddItems(ComboBoxCharset, GetType(ASCIIArtCtrl.Charsets), AsciiArtCtrl.Charset)
        AddItems(ComboBoxColorMode, GetType(ASCIIArtCtrl.ColorModes), AsciiArtCtrl.ColorMode)
        AddItems(ComboBoxScanMode, GetType(ASCIIArtCtrl.ScanModes), AsciiArtCtrl.ScanMode)
        AddItems(ComboBoxGrayScaleMode, GetType(ASCIIArtCtrl.GrayscaleModes), AsciiArtCtrl.GrayScaleMode)

        AddHandler ComboBoxCharset.SelectedIndexChanged, Sub() [Enum].TryParse(ComboBoxCharset.SelectedItem.ToString(), AsciiArtCtrl.Charset)
        AddHandler ComboBoxColorMode.SelectedIndexChanged, Sub() [Enum].TryParse(ComboBoxColorMode.SelectedItem.ToString(), AsciiArtCtrl.ColorMode)
        AddHandler ComboBoxScanMode.SelectedIndexChanged, Sub() [Enum].TryParse(ComboBoxScanMode.SelectedItem.ToString(), AsciiArtCtrl.ScanMode)
        AddHandler ComboBoxGrayScaleMode.SelectedIndexChanged, Sub() [Enum].TryParse(ComboBoxGrayScaleMode.SelectedItem.ToString(), AsciiArtCtrl.GrayScaleMode)
    End Sub

    Private Sub AsciiArtCtrl_DragOver(sender As Object, e As DragEventArgs) Handles AsciiArtCtrl.DragOver
        e.Effect = DragDropEffects.None

        If e.Data.GetFormats().Contains("FileDrop") Then
            Dim files() As String = CType(e.Data.GetData("FileDrop"), String())
            If files.Count = 1 AndAlso files(0).Contains(".") Then
                Dim ext As String = files(0).ToLower().Split("."c)(1).Replace("jpg", "jpeg")
                If (From p In GetType(Imaging.ImageFormat).GetProperties() Select p.Name.ToLower()).Contains(ext) Then
                    e.Effect = DragDropEffects.Copy
                End If
            End If
        End If
    End Sub

    Private Sub AsciiArtCtrl_DragDrop(sender As Object, e As DragEventArgs) Handles AsciiArtCtrl.DragDrop
        If e.Effect = DragDropEffects.Copy Then
            Dim bmp As Bitmap = Bitmap.FromFile(CType(e.Data.GetData("FileDrop"), String())(0))
            AsciiArtCtrl.CanvasSize = New Size(bmp.Width / 5, bmp.Height / 10)
            AsciiArtCtrl.Bitmap = bmp
        End If
    End Sub

    Private Sub ButtonExport_Click(sender As Object, e As EventArgs) Handles ButtonExport.Click
        If AsciiArtCtrl.Canvas Is Nothing Then
            MsgBox($"Nothing to export.{Environment.NewLine}Drop an image first!", MsgBoxStyle.Information)
            Exit Sub
        End If

        Using dlg As New SaveFileDialog()
            dlg.Title = "Save ASCII Art File"
            dlg.Filter = "Text File|*.txt|VB.NET Code|*.vb"
            If dlg.ShowDialog(Me) = DialogResult.OK Then
                Select Case dlg.FilterIndex
                    Case 1 : SaveTextFile(dlg.FileName)
                    Case 2 : SaveAsVBFile(dlg.FileName)
                End Select
            End If
        End Using
    End Sub

    Private Sub SaveAsVBFile(fileName As String)
        Dim str As String = ""

        Dim lastColor As String = ""
        Dim curCharColor As String = ""

        Dim lastChar As String = AsciiArtCtrl.Canvas(0)(0).Character
        Dim curChar As String = ""
        Dim charDupCount As Integer = 0

        Dim ProcessChar = Sub(x As Integer, force As Boolean)
                              If lastChar <> curChar OrElse force OrElse x = AsciiArtCtrl.CanvasSize.Width - 1 Then
                                  str += $"Console.Write(""{StrDup(charDupCount, lastChar)}"")"
                                  str += Environment.NewLine
                                  charDupCount = 1
                                  lastChar = curChar
                              Else
                                  charDupCount += 1
                              End If
                          End Sub

        Dim HexColorToArray = Function(hexColor As String) As Integer()
                                  Return {Integer.Parse(hexColor.Substring(0, 2), Globalization.NumberStyles.HexNumber),
                                          Integer.Parse(hexColor.Substring(2, 2), Globalization.NumberStyles.HexNumber),
                                          Integer.Parse(hexColor.Substring(4, 2), Globalization.NumberStyles.HexNumber)}
                              End Function

        Dim ToConsoleColor = Function(c As Color) As ConsoleColor
                                 Dim d As Double
                                 Dim minD As Double = Double.MaxValue
                                 Dim bestResult As ConsoleColor
                                 Dim ccRgb() As Integer = Nothing

                                 For Each cc As ConsoleColor In [Enum].GetValues(GetType(ConsoleColor))
                                     Select Case cc
                                         Case ConsoleColor.Black : ccRgb = HexColorToArray("000000")
                                         Case ConsoleColor.DarkBlue : ccRgb = HexColorToArray("000080")
                                         Case ConsoleColor.DarkGreen : ccRgb = HexColorToArray("008000")
                                         Case ConsoleColor.DarkCyan : ccRgb = HexColorToArray("008080")
                                         Case ConsoleColor.DarkRed : ccRgb = HexColorToArray("800000")
                                         Case ConsoleColor.DarkMagenta : ccRgb = HexColorToArray("800080")
                                         Case ConsoleColor.DarkYellow : ccRgb = HexColorToArray("808000")
                                         Case ConsoleColor.Gray : ccRgb = HexColorToArray("C0C0C0")
                                         Case ConsoleColor.DarkGray : ccRgb = HexColorToArray("808080")
                                         Case ConsoleColor.Blue : ccRgb = HexColorToArray("0000FF")
                                         Case ConsoleColor.Green : ccRgb = HexColorToArray("00FF00")
                                         Case ConsoleColor.Cyan : ccRgb = HexColorToArray("00FFFF")
                                         Case ConsoleColor.Red : ccRgb = HexColorToArray("FF0000")
                                         Case ConsoleColor.Magenta : ccRgb = HexColorToArray("FF00FF")
                                         Case ConsoleColor.Yellow : ccRgb = HexColorToArray("FFFF00")
                                         Case ConsoleColor.White : ccRgb = HexColorToArray("FFFFFF")
                                     End Select

                                     d = Math.Sqrt((c.R - ccRgb(0)) ^ 2 + (c.G - ccRgb(1)) ^ 2 + (c.B - ccRgb(2)) ^ 2)
                                     If d < minD Then
                                         minD = d
                                         bestResult = cc
                                     End If
                                 Next

                                 Return bestResult
                             End Function

        ' EGA Palette
        ' http://stackoverflow.com/questions/1988833/converting-color-to-consolecolor
        Dim ToConsoleColor2 = Function(c As Color) As ConsoleColor
                                  Dim index As Integer = If(c.R > 128 Or c.G > 128 Or c.B > 128, 8, 0) ' Bright bit
                                  index = index Or If(c.R > 64, 4, 0) ' Red bit
                                  index = index Or If(c.G > 64, 2, 0) ' Green bit
                                  index = index Or If(c.B > 64, 1, 0) ' Blue bit
                                  Return CType(index, ConsoleColor)
                              End Function

        str += "Dim x As Integer = Console.CursorLeft"
        str += Environment.NewLine
        str += "Dim y As Integer = Console.CursorTop"
        str += Environment.NewLine

        For y As Integer = 0 To AsciiArtCtrl.CanvasSize.Height - 1
            For x = 0 To AsciiArtCtrl.CanvasSize.Width - 1
                curChar = AsciiArtCtrl.Canvas(x)(y).Character
                curCharColor = ToConsoleColor2(AsciiArtCtrl.Canvas(x)(y).Color).ToString()
                If lastColor <> curCharColor Then
                    str += $"Console.ForegroundColor = ConsoleColor.{curCharColor}"
                    str += Environment.NewLine
                    lastColor = curCharColor
                    ProcessChar(x, charDupCount > 0)
                Else
                    ProcessChar(x, False)
                End If
            Next
            If y < AsciiArtCtrl.CanvasSize.Height - 1 Then
                str += "Console.SetCursorPosition(x, Console.CursorTop + 1)"
                str += Environment.NewLine
            End If
        Next
        IO.File.WriteAllText(fileName, str)
    End Sub

    Private Sub SaveTextFile(fileName As String)
        Dim str As String = ""
        For y As Integer = 0 To AsciiArtCtrl.CanvasSize.Height - 1
            For x = 0 To AsciiArtCtrl.CanvasSize.Width - 1
                str += AsciiArtCtrl.Canvas(x)(y).Character
            Next
            str += Environment.NewLine
        Next

        IO.File.WriteAllText(fileName, str)
    End Sub
End Class
