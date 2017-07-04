Public Class FormMain
    Private Sub FormMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AsciiArtCtrl.I2A.Charset = Image2Ascii.Charsets.Standard
        AsciiArtCtrl.I2A.ColorMode = Image2Ascii.ColorModes.GrayScale
        AsciiArtCtrl.I2A.ScanMode = Image2Ascii.ScanModes.Fast
        AsciiArtCtrl.I2A.GrayScaleMode = Image2Ascii.GrayscaleModes.Average
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

        AddItems(ComboBoxCharset, GetType(Image2Ascii.Charsets), AsciiArtCtrl.I2A.Charset)
        AddItems(ComboBoxColorMode, GetType(Image2Ascii.ColorModes), AsciiArtCtrl.I2A.ColorMode)
        AddItems(ComboBoxScanMode, GetType(Image2Ascii.ScanModes), AsciiArtCtrl.I2A.ScanMode)
        AddItems(ComboBoxGrayScaleMode, GetType(Image2Ascii.GrayscaleModes), AsciiArtCtrl.I2A.GrayScaleMode)

        AddHandler ComboBoxCharset.SelectedIndexChanged, Sub() [Enum].TryParse(ComboBoxCharset.SelectedItem.ToString(), AsciiArtCtrl.I2A.Charset)
        AddHandler ComboBoxColorMode.SelectedIndexChanged, Sub() [Enum].TryParse(ComboBoxColorMode.SelectedItem.ToString(), AsciiArtCtrl.I2A.ColorMode)
        AddHandler ComboBoxScanMode.SelectedIndexChanged, Sub() [Enum].TryParse(ComboBoxScanMode.SelectedItem.ToString(), AsciiArtCtrl.I2A.ScanMode)
        AddHandler ComboBoxGrayScaleMode.SelectedIndexChanged, Sub() [Enum].TryParse(ComboBoxGrayScaleMode.SelectedItem.ToString(), AsciiArtCtrl.I2A.GrayScaleMode)
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
            AsciiArtCtrl.I2A.CanvasSize = New Size(AsciiArtCtrl.Width / AsciiArtCtrl.I2A.CharSize.Width, AsciiArtCtrl.Height / AsciiArtCtrl.I2A.CharSize.Height)
            AsciiArtCtrl.I2A.Bitmap = bmp
        End If
    End Sub

    Private Sub ButtonExport_Click(sender As Object, e As EventArgs) Handles ButtonExport.Click
        If AsciiArtCtrl.I2A.Canvas Is Nothing Then
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

        Dim lastChar As String = AsciiArtCtrl.I2A.Canvas(0)(0).Character
        Dim curChar As String = ""
        Dim charDupCount As Integer = 0

        Dim ProcessChar = Sub(x As Integer, force As Boolean)
                              If lastChar <> curChar OrElse force OrElse x = AsciiArtCtrl.I2A.CanvasSize.Width - 1 Then
                                  str += $"Console.Write(""{StrDup(charDupCount, lastChar)}"")"
                                  str += Environment.NewLine
                                  charDupCount = 1
                                  lastChar = curChar
                              Else
                                  charDupCount += 1
                              End If
                          End Sub

        str += "Dim x As Integer = Console.CursorLeft"
        str += Environment.NewLine
        str += "Dim y As Integer = Console.CursorTop"
        str += Environment.NewLine

        For y As Integer = 0 To AsciiArtCtrl.I2A.CanvasSize.Height - 1
            For x = 0 To AsciiArtCtrl.I2A.CanvasSize.Width - 1
                curChar = AsciiArtCtrl.I2A.Canvas(x)(y).Character
                curCharColor = Image2Ascii.ToConsoleColorEGA(AsciiArtCtrl.I2A.Canvas(x)(y).Color).ToString()
                If lastColor <> curCharColor Then
                    str += $"Console.ForegroundColor = ConsoleColor.{curCharColor}"
                    str += Environment.NewLine
                    lastColor = curCharColor
                    ProcessChar(x, charDupCount > 0)
                Else
                    ProcessChar(x, False)
                End If
            Next
            If y < AsciiArtCtrl.I2A.CanvasSize.Height - 1 Then
                str += "Console.SetCursorPosition(x, Console.CursorTop + 1)"
                str += Environment.NewLine
            End If
        Next
        IO.File.WriteAllText(fileName, str)
    End Sub

    Private Sub SaveTextFile(fileName As String)
        Dim str As String = ""
        For y As Integer = 0 To AsciiArtCtrl.I2A.CanvasSize.Height - 1
            For x = 0 To AsciiArtCtrl.I2A.CanvasSize.Width - 1
                str += AsciiArtCtrl.I2A.Canvas(x)(y).Character
            Next
            str += Environment.NewLine
        Next

        IO.File.WriteAllText(fileName, str)
    End Sub
End Class
