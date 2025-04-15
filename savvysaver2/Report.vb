Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports Microsoft.Data.SqlClient
Imports System.IO
Imports Windows.Win32.System

Public Class Report
    Dim con As New SqlConnection("Data Source=CHIDAMBARAM-LAP\SQLEXPRESS01;Initial Catalog=Comparison;Integrated Security=True;TrustServerCertificate=True")

    Private Sub VendorToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles VendorToolStripMenuItem.Click
        Vendor.Hide()
        Me.Hide()
    End Sub

    Private Sub ProductToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ProductToolStripMenuItem.Click
        Product.Show()
        Me.Hide()

    End Sub

    Private Sub DashboardToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DashboardToolStripMenuItem.Click
        dashboard.Show()
        Me.Hide()
    End Sub

    Private Sub LogoutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LogoutToolStripMenuItem.Click
        Form1.Show()
        Me.Hide()
    End Sub

    Private Sub CloseToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CloseToolStripMenuItem.Click
        Application.Exit()
    End Sub


    Private Sub Report_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Optional: You can pre-load data here if needed
    End Sub

    ' 🔹 Show selected customer details + most searched product
    Private Sub Guna2GradientButton1_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton1.Click
        Try
            con.Open()
            Dim query As String =
            "SELECT C.Cust_ID, C.Username, C.Email, COUNT(C1.Product_Name) AS SearchCount, 
                    (SELECT TOP 1 Product_Name 
                     FROM Comparison1 WHERE Username = C.Username 
                     GROUP BY Product_Name 
                     ORDER BY COUNT(*) DESC) AS MostSearchedProduct 
             FROM Customer1 C 
             LEFT JOIN Comparison1 C1 ON C.Username = C1.Username 
             GROUP BY C.Cust_ID, C.Username, C.Email"
            Dim cmd As New SqlCommand(query, con)
            Dim reader As SqlDataReader = cmd.ExecuteReader()
            Dim dt As New DataTable()
            dt.Load(reader)
            Guna2DataGridView1.DataSource = dt
            con.Close()

            If Guna2DataGridView1.SelectedRows.Count = 0 Then
                MessageBox.Show("Please select a customer row.")
                Return
            End If

            If MessageBox.Show("Do you want to print the selected customer's data to PDF?", "Print PDF", MessageBoxButtons.YesNo) = DialogResult.Yes Then
                Dim sfd As New SaveFileDialog()
                sfd.Filter = "PDF Files|*.pdf"
                sfd.Title = "Save PDF"
                sfd.FileName = "SelectedCustomerReport.pdf"

                If sfd.ShowDialog() = DialogResult.OK Then
                    Dim row As DataGridViewRow = Guna2DataGridView1.SelectedRows(0)
                    Dim pdfDoc As New iTextSharp.text.Document()
                    PdfWriter.GetInstance(pdfDoc, New FileStream(sfd.FileName, FileMode.Create))
                    pdfDoc.Open()

                    pdfDoc.Add(New Paragraph("Customer Report"))
                    pdfDoc.Add(New Paragraph("Cust ID: " & row.Cells("Cust_ID").Value.ToString()))
                    pdfDoc.Add(New Paragraph("Username: " & row.Cells("Username").Value.ToString()))
                    pdfDoc.Add(New Paragraph("Email: " & row.Cells("Email").Value.ToString()))
                    pdfDoc.Add(New Paragraph("Most Searched Product: " & row.Cells("MostSearchedProduct").Value.ToString()))
                    pdfDoc.Close()

                    MessageBox.Show("PDF saved at: " & sfd.FileName)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        Finally
            con.Close()
        End Try
    End Sub

    ' 🔹 Print all customer data in DataGridView to PDF
    Private Sub Guna2GradientButton2_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton2.Click
        Try
            If Guna2DataGridView1.Rows.Count = 0 Then
                MessageBox.Show("No data to export.")
                Return
            End If

            Dim sfd As New SaveFileDialog()
            sfd.Filter = "PDF Files|*.pdf"
            sfd.Title = "Save PDF"
            sfd.FileName = "AllCustomersReport.pdf"

            If sfd.ShowDialog() = DialogResult.OK Then
                Dim pdfDoc As New Document()
                PdfWriter.GetInstance(pdfDoc, New FileStream(sfd.FileName, FileMode.Create))
                pdfDoc.Open()

                Dim pdfTable As New PdfPTable(Guna2DataGridView1.ColumnCount)

                ' Header
                For Each col As DataGridViewColumn In Guna2DataGridView1.Columns
                    pdfTable.AddCell(New Phrase(col.HeaderText))
                Next

                ' Rows
                For Each row As DataGridViewRow In Guna2DataGridView1.Rows
                    If Not row.IsNewRow Then
                        For Each cell As DataGridViewCell In row.Cells
                            pdfTable.AddCell(If(cell.Value IsNot Nothing, cell.Value.ToString(), ""))
                        Next
                    End If
                Next

                pdfDoc.Add(pdfTable)
                pdfDoc.Close()

                MessageBox.Show("PDF saved at: " & sfd.FileName)
            End If
        Catch ex As Exception
            MessageBox.Show("Error exporting PDF: " & ex.Message)
        End Try
    End Sub
End Class