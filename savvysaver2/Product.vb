Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports Microsoft.Data.SqlClient

Public Class Product

    ' Connection String
    Dim con As New SqlConnection("Data Source=CHIDAMBARAM-LAP\SQLEXPRESS01;Initial Catalog=Comparison;Integrated Security=True;TrustServerCertificate=True")
    Dim da As SqlDataAdapter
    Dim dt As DataTable
    Dim cmd As SqlCommand
    Dim builder As SqlCommandBuilder

    Private Sub Product_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadPlatforms()
        LoadCategories()
        LoadVendors()
    End Sub

    ' 🔹 Load Platforms into ComboBox1 (if needed)
    Private Sub LoadPlatforms()
        ' Already added in designer as you said, no code needed unless dynamic fetch
    End Sub

    ' 🔹 Load Categories into ComboBox2
    Private Sub LoadCategories()
        ' Already added in designer as you said, no code needed unless dynamic fetch
    End Sub

    ' 🔹 Load Vendor IDs into Guna2TextBox2 tooltip suggestion
    ' 🔹 Load Vendor IDs into Guna2TextBox2 tooltip suggestion
    Private Sub LoadVendors()
        Try
            con.Open()
            Dim cmd As New SqlCommand("SELECT Vendor_ID, Vendor_Name FROM Vendor", con)
            Dim reader As SqlDataReader = cmd.ExecuteReader()
            Dim vendorList As String = ""

            While reader.Read()
                vendorList &= $"{reader("Vendor_ID")} - {reader("Vendor_Name")}" & Environment.NewLine
            End While

            ToolTip1.SetToolTip(Guna2TextBox2, "Available Vendors:" & Environment.NewLine & vendorList)

        Catch ex As Exception
            MessageBox.Show("Error loading vendors: " & ex.Message)
        Finally
            con.Close()
        End Try
    End Sub


    ' 🔹 Register Product (Guna2GradientButton1)
    Private Sub Guna2GradientButton1_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton1.Click
        Try
            Dim query As String = "INSERT INTO Product (Product_Name, Category, Price, Vendor_ID, Stock_Quantity, Description, Added_Date) 
                                   VALUES (@Product_Name, @Category, @Price, @Vendor_ID, @Stock_Quantity, @Description, @Added_Date)"

            cmd = New SqlCommand(query, con)
            cmd.Parameters.AddWithValue("@Product_Name", Guna2TextBox3.Text)
            cmd.Parameters.AddWithValue("@Category", Guna2ComboBox2.SelectedItem.ToString())
            cmd.Parameters.AddWithValue("@Price", Decimal.Parse(Guna2TextBox1.Text))
            cmd.Parameters.AddWithValue("@Vendor_ID", Integer.Parse(Guna2TextBox2.Text))
            cmd.Parameters.AddWithValue("@Stock_Quantity", Integer.Parse(Guna2TextBox4.Text))
            cmd.Parameters.AddWithValue("@Description", RichTextBox1.Text)
            cmd.Parameters.AddWithValue("@Added_Date", DateTime.Now)

            con.Open()
            cmd.ExecuteNonQuery()
            con.Close()

            MessageBox.Show("Product added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            ShowData()

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        Finally
            con.Close()
        End Try
    End Sub

    ' 🔹 Show Data (Guna2GradientButton2)
    Private Sub Guna2GradientButton2_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton2.Click
        ShowData()
    End Sub

    Private Sub ShowData()
        Try
            con.Open()
            da = New SqlDataAdapter("SELECT * FROM Product", con)
            dt = New DataTable()
            da.Fill(dt)
            Guna2DataGridView1.DataSource = dt
            con.Close()
        Catch ex As Exception
            MessageBox.Show("Error loading data: " & ex.Message)
        Finally
            con.Close()
        End Try
    End Sub

    ' 🔹 Save Changes from DataGridView (Guna2GradientButton3)
    Private Sub Guna2GradientButton3_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton3.Click
        Try
            builder = New SqlCommandBuilder(da)
            da.Update(dt)
            MessageBox.Show("Changes saved successfully!", "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show("Error saving changes: " & ex.Message)
        End Try
    End Sub

    Private Sub VendorToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles VendorToolStripMenuItem.Click
        Vendor.Show()
        Me.Hide()

    End Sub

    Private Sub ReportToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ReportToolStripMenuItem.Click
        Report.Show()
        Me.Hide()
    End Sub

    Private Sub LogOutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LogOutToolStripMenuItem.Click
        Form1.Show()
        Me.Hide()

    End Sub

    Private Sub CloseToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CloseToolStripMenuItem.Click
        Application.Exit()
    End Sub
End Class