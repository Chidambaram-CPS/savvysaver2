Imports Guna.UI2.WinForms
Imports Microsoft.Data.SqlClient

Public Class COMPARE
    Dim con As New SqlConnection("Data Source=CHIDAMBARAM-LAP\SQLEXPRESS01;Initial Catalog=Comparison;Integrated Security=True;TrustServerCertificate=True")

    Dim dtProducts As New DataTable()

    Private Sub Compare_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadProductVendorData()
        LoadComboBoxData()
    End Sub

    ' 🔁 Load all product/vendor data
    Private Sub LoadProductVendorData()
        dtProducts.Clear()
        Dim query As String = "SELECT P.Product_Name, V.Vendor_Name, P.Price, V.Address 
                               FROM Product P 
                               JOIN Vendor V ON P.Vendor_ID = V.Vendor_ID"
        Dim adapter As New SqlDataAdapter(query, con)
        adapter.Fill(dtProducts)
        Guna2DataGridView1.DataSource = dtProducts
    End Sub

    ' 🔁 Load product names into ComboBox for searching
    Private Sub LoadComboBoxData()
        Guna2ComboBox1.Items.Clear()

        Dim cmd As New SqlCommand("SELECT DISTINCT Product_Name FROM Product", con)
        con.Open()
        Dim reader As SqlDataReader = cmd.ExecuteReader()
        While reader.Read()
            Guna2ComboBox1.Items.Add(reader("Product_Name").ToString())
        End While
        con.Close()

        Guna2ComboBox2.Items.Clear()
        Guna2ComboBox2.Items.Add("Low to High")
        Guna2ComboBox2.Items.Add("High to Low")
    End Sub

    ' 🔘 Show All Data Button
    Private Sub Guna2GradientButton1_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton1.Click
        LoadProductVendorData()
    End Sub

    ' 🔍 Search by Product Name Button
    Private Sub Guna2GradientButton2_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton2.Click
        If Guna2ComboBox1.SelectedIndex <> -1 Then
            Dim filtered As DataView = dtProducts.DefaultView
            filtered.RowFilter = $"[Product_Name] = '{Guna2ComboBox1.SelectedItem.ToString()}'"
            Guna2DataGridView1.DataSource = filtered
        Else
            MessageBox.Show("Please select a product name to search.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub Guna2GradientButton3_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton3.Click
        If Guna2DataGridView1.Rows.Count = 0 Then
            MessageBox.Show("No data to sort.")
            Return
        End If

        ' 👇 Handle both DataTable and DataView
        Dim dtView As DataTable = Nothing

        If TypeOf Guna2DataGridView1.DataSource Is DataView Then
            dtView = CType(Guna2DataGridView1.DataSource, DataView).ToTable()
        ElseIf TypeOf Guna2DataGridView1.DataSource Is DataTable Then
            dtView = CType(Guna2DataGridView1.DataSource, DataTable)
        End If

        If dtView Is Nothing OrElse dtView.Rows.Count = 0 Then
            MessageBox.Show("No search data to sort.")
            Return
        End If

        ' 🧪 Create copy of current search data
        Dim sortedDT As DataTable = dtView.Copy()
        Dim productList As List(Of DataRow) = sortedDT.AsEnumerable().ToList()

        ' 🔁 Perform QuickSort
        QuickSort(productList, 0, productList.Count - 1)

        ' ⬇ Check sort direction
        If Guna2ComboBox2.SelectedItem IsNot Nothing AndAlso Guna2ComboBox2.SelectedItem.ToString() = "High to Low" Then
            productList.Reverse()
        End If

        ' 📌 Rebind the sorted data
        Guna2DataGridView1.DataSource = productList.CopyToDataTable()
    End Sub
    ' 🔁 Quick Sort Function
    Private Sub QuickSort(ByRef list As List(Of DataRow), low As Integer, high As Integer)
        If low < high Then
            Dim pivotIndex As Integer = Partition(list, low, high)
            QuickSort(list, low, pivotIndex - 1)
            QuickSort(list, pivotIndex + 1, high)
        End If
    End Sub

    Private Function Partition(ByRef list As List(Of DataRow), low As Integer, high As Integer) As Integer
        Dim pivot As Decimal = Convert.ToDecimal(list(high)("Price"))
        Dim i As Integer = low - 1

        For j As Integer = low To high - 1
            If Convert.ToDecimal(list(j)("Price")) <= pivot Then
                i += 1
                Dim temp = list(i)
                list(i) = list(j)
                list(j) = temp
            End If
        Next

        Dim tmp = list(i + 1)
        list(i + 1) = list(high)
        list(high) = tmp

        Return i + 1
    End Function
    Private Sub LogoutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LogoutToolStripMenuItem.Click
        Form1.Show()
        Me.Hide()
    End Sub

    Private Sub CloseToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CloseToolStripMenuItem.Click
        Application.Exit()
    End Sub
End Class