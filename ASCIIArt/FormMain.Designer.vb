<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.ComboBoxCharset = New System.Windows.Forms.ComboBox()
        Me.ComboBoxColorMode = New System.Windows.Forms.ComboBox()
        Me.ComboBoxScanMode = New System.Windows.Forms.ComboBox()
        Me.ComboBoxGrayScaleMode = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.AsciiArtCtrl = New ASCIIArt.ASCIIArtCtrl()
        Me.ButtonExport = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'ComboBoxCharset
        '
        Me.ComboBoxCharset.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxCharset.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ComboBoxCharset.FormattingEnabled = True
        Me.ComboBoxCharset.Location = New System.Drawing.Point(139, 25)
        Me.ComboBoxCharset.Name = "ComboBoxCharset"
        Me.ComboBoxCharset.Size = New System.Drawing.Size(121, 21)
        Me.ComboBoxCharset.TabIndex = 1
        '
        'ComboBoxColorMode
        '
        Me.ComboBoxColorMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxColorMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ComboBoxColorMode.FormattingEnabled = True
        Me.ComboBoxColorMode.Location = New System.Drawing.Point(266, 25)
        Me.ComboBoxColorMode.Name = "ComboBoxColorMode"
        Me.ComboBoxColorMode.Size = New System.Drawing.Size(121, 21)
        Me.ComboBoxColorMode.TabIndex = 1
        '
        'ComboBoxScanMode
        '
        Me.ComboBoxScanMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxScanMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ComboBoxScanMode.FormattingEnabled = True
        Me.ComboBoxScanMode.Location = New System.Drawing.Point(12, 25)
        Me.ComboBoxScanMode.Name = "ComboBoxScanMode"
        Me.ComboBoxScanMode.Size = New System.Drawing.Size(121, 21)
        Me.ComboBoxScanMode.TabIndex = 1
        '
        'ComboBoxGrayScaleMode
        '
        Me.ComboBoxGrayScaleMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxGrayScaleMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ComboBoxGrayScaleMode.FormattingEnabled = True
        Me.ComboBoxGrayScaleMode.Location = New System.Drawing.Point(393, 25)
        Me.ComboBoxGrayScaleMode.Name = "ComboBoxGrayScaleMode"
        Me.ComboBoxGrayScaleMode.Size = New System.Drawing.Size(121, 21)
        Me.ComboBoxGrayScaleMode.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(64, 13)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Scan Mode"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(136, 9)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(46, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Charset"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(263, 9)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(68, 13)
        Me.Label3.TabIndex = 3
        Me.Label3.Text = "Color Mode"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(390, 9)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(89, 13)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "GrayScale Mode"
        '
        'AsciiArtCtrl
        '
        Me.AsciiArtCtrl.AllowDrop = True
        Me.AsciiArtCtrl.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AsciiArtCtrl.BackColor = System.Drawing.Color.Black
        Me.AsciiArtCtrl.Bitmap = Nothing
        Me.AsciiArtCtrl.CanvasSize = New System.Drawing.Size(80, 25)
        Me.AsciiArtCtrl.Charset = ASCIIArt.ASCIIArtCtrl.Charsets.Standard
        Me.AsciiArtCtrl.ColorMode = ASCIIArt.ASCIIArtCtrl.ColorModes.GrayScale
        Me.AsciiArtCtrl.Font = New System.Drawing.Font("Lucida Console", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.AsciiArtCtrl.GrayScaleMode = ASCIIArt.ASCIIArtCtrl.GrayscaleModes.Accuarte
        Me.AsciiArtCtrl.Location = New System.Drawing.Point(12, 52)
        Me.AsciiArtCtrl.Name = "AsciiArtCtrl"
        Me.AsciiArtCtrl.ScanMode = ASCIIArt.ASCIIArtCtrl.ScanModes.Fast
        Me.AsciiArtCtrl.Size = New System.Drawing.Size(868, 529)
        Me.AsciiArtCtrl.TabIndex = 0
        '
        'ButtonExport
        '
        Me.ButtonExport.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonExport.Location = New System.Drawing.Point(805, 23)
        Me.ButtonExport.Name = "ButtonExport"
        Me.ButtonExport.Size = New System.Drawing.Size(75, 23)
        Me.ButtonExport.TabIndex = 4
        Me.ButtonExport.Text = "Export"
        Me.ButtonExport.UseVisualStyleBackColor = True
        '
        'FormMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(892, 593)
        Me.Controls.Add(Me.ButtonExport)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ComboBoxScanMode)
        Me.Controls.Add(Me.ComboBoxGrayScaleMode)
        Me.Controls.Add(Me.ComboBoxColorMode)
        Me.Controls.Add(Me.ComboBoxCharset)
        Me.Controls.Add(Me.AsciiArtCtrl)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "FormMain"
        Me.Text = "ASCII Art DEMO"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents AsciiArtCtrl As ASCIIArtCtrl
    Friend WithEvents ComboBoxCharset As System.Windows.Forms.ComboBox
    Friend WithEvents ComboBoxColorMode As System.Windows.Forms.ComboBox
    Friend WithEvents ComboBoxScanMode As System.Windows.Forms.ComboBox
    Friend WithEvents ComboBoxGrayScaleMode As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents ButtonExport As Button
End Class
