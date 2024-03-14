using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Shop.Child_view
{
    /// <summary>
    /// Interaction logic for Form_Donhang.xaml
    /// </summary>
    public partial class Form_Donhang : Window
    {
        SqlConnection Conn = new SqlConnection();
        DataTable DataSource = null;
        // bien connection
        String ConnectionString = @"Data Source=Lenovo\SQLEXPRESS;Initial Catalog=QL_Shop;Integrated Security=True;";
        private SqlDataAdapter adapter;
        public Form_Donhang()
        {
            InitializeComponent();
        }
        // drag form
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
        // button close
        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        // button maxsize
        private void btn_Maxsize_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
            {
                WindowState = WindowState.Maximized;
            }
            else
            {
                WindowState = WindowState.Normal;
            }
        }
        // button minizise
        private void btn_Minisize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        // button search
        private void btn_Search_Click(object sender, RoutedEventArgs e)
        {
            String sql = "";
            if (cbbox_Search.SelectedIndex == 0)
            {
                if (_Search.Text != "")
                {
                    try
                    {
                        dgView_Hoadon.ItemsSource = null;
                        Conn.ConnectionString = ConnectionString;
                        Conn.Open();
                        string _sql = "SELECT * FROM tbl_Hoadon WHERE MaNV LIKE @s0 or MaNV like @s1 or MaNV like @s2";
                        SqlCommand cmd = new SqlCommand(_sql, Conn);
                        cmd.Parameters.AddWithValue("@s0", "%" + _Search.Text + "%");
                        cmd.Parameters.AddWithValue("@s1", _Search.Text + "%");
                        cmd.Parameters.AddWithValue("@s2", "%" + _Search.Text);
                        // tạo bộ adapter dữ liệu và điền vào tập dữ liệu các kết quả của truy vấn
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataSet dataSet = new DataSet();
                        adapter.Fill(dataSet);
                        DataSource = dataSet.Tables[0];
                        //Thiết lập cho hiển thị lên DataGrid
                        dgView_Hoadon.ItemsSource = DataSource.DefaultView;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Loi :" + ex.Message);
                    }
                    finally
                    {
                        Conn.Close();
                    }
                }
                else
                {
                    NapDuLieuTuMayChu();
                }
            }
            else if (cbbox_Search.SelectedIndex == 1)
            {
                // xóa nguồn của lưới dữ liệu
                try
                {
                    if (_Search.Text != "")
                    {
                        dgView_Hoadon.ItemsSource = null;
                        Conn.ConnectionString = ConnectionString;
                        Conn.Open();
                        //đảm bảo kết nối cơ sở dữ liệu được mở trước khi thực hiện truy vấny
                        if (Conn.State != ConnectionState.Open)
                        {
                            Conn.Open();
                        }
                        // xây dựng truy vấn SQL bằng cách sử dụng các truy vấn được tham số hóa để ngăn SQL injection
                        string _sql = "SELECT * FROM tbl_Hoadon WHERE MaHD LIKE @s0 or MaHD like @s1 or MaHD like @s2";
                        SqlCommand cmd = new SqlCommand(_sql, Conn);
                        cmd.Parameters.AddWithValue("@s0", "%" + _Search.Text + "%");
                        cmd.Parameters.AddWithValue("@s1", _Search.Text + "%");
                        cmd.Parameters.AddWithValue("@s2", "%" + _Search.Text);
                        // tạo bộ adapter dữ liệu và điền vào tập dữ liệu các kết quả của truy vấn
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataSet dataSet = new DataSet();
                        adapter.Fill(dataSet);

                        //đặt nguồn dữ liệu cho lưới dữ liệu
                        dgView_Hoadon.ItemsSource = dataSet.Tables[0].DefaultView;
                        Conn.Close();
                    }
                    else
                    {
                        NapDuLieuTuMayChu();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Loi : " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng lựa chọn danh mục!");
                NapDuLieuTuMayChu();
            }
        }
        // button cap nhat
        private void btn_Capnhat_Click(object sender, RoutedEventArgs e)
        {
            NapDuLieuTuMayChu();
        }
        // button sua
        private void btn_Sua_Click(object sender, RoutedEventArgs e)
        {

        }
        //buttun xoa
        private void btn_Xoa_Click(object sender, RoutedEventArgs e)
        {
            if(dgView_Hoadon.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn trường thông tin cần xóa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            String sql = "delete from tbl_Hoadon where MaHD = '" +_Mahoadon.Text+ "';";
            try
            {
                Conn.ConnectionString = ConnectionString;
                Conn.Open();
                SqlCommand cmd = new SqlCommand(sql,Conn);
                if (MessageBox.Show("Bạn có muốn xóa thông tin không?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Xóa thông tin thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            } catch (Exception ex)
            {
                MessageBox.Show("Loi delete hoadon : " + ex.Message);
            } finally
            {
                Conn.Close();
            }
            NapDuLieuTuMayChu();
            dgView_Thongtinhoadon.ItemsSource = null;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // cbbox search
            cbbox_Search.Items.Add("Mã nhân viên");
            cbbox_Search.Items.Add("Mã hóa đơn");
            NapDuLieuTuMayChu();
        }
        // nạp dữ liệu vào table
        private void NapDuLieuTuMayChu()
        {
            dgView_Hoadon.ItemsSource = null;
            try
            {
                Conn.ConnectionString = ConnectionString;
                Conn.Open();
                string SqlStr = "Select * from tbl_Hoadon";
                SqlDataAdapter adapter = new SqlDataAdapter(SqlStr, Conn);
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);
                DataSource = dataSet.Tables[0];
                //Thiết lập cho hiển thị lên DataGrid

                dgView_Hoadon.ItemsSource = DataSource.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi : " + ex.Message, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Conn.Close();
            }
        }
        // tìm kiếm
        private void label_Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            String sql = "";
            if (cbbox_Search.SelectedIndex == 0)
            {
                if (_Search.Text != "")
                {
                    try
                    {
                        dgView_Hoadon.ItemsSource = null;
                        Conn.ConnectionString = ConnectionString;
                        Conn.Open();
                        string _sql = "SELECT * FROM tbl_Hoadon WHERE MaNV LIKE @s0 or MaNV like @s1 or MaNV like @s2";
                        SqlCommand cmd = new SqlCommand(_sql, Conn);
                        cmd.Parameters.AddWithValue("@s0", "%" + _Search.Text + "%");
                        cmd.Parameters.AddWithValue("@s1", _Search.Text + "%");
                        cmd.Parameters.AddWithValue("@s2", "%" + _Search.Text);
                        // tạo bộ adapter dữ liệu và điền vào tập dữ liệu các kết quả của truy vấn
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataSet dataSet = new DataSet();
                        adapter.Fill(dataSet);
                        DataSource = dataSet.Tables[0];
                        //Thiết lập cho hiển thị lên DataGrid
                        dgView_Hoadon.ItemsSource = DataSource.DefaultView;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Loi :" + ex.Message);
                    }
                    finally
                    {
                        Conn.Close();
                    }
                }
                else
                {
                    NapDuLieuTuMayChu();
                }
            }
            else if (cbbox_Search.SelectedIndex == 1)
            {
                // xóa nguồn của lưới dữ liệu
                try
                {
                    if (_Search.Text != "")
                    {
                        dgView_Hoadon.ItemsSource = null;
                        Conn.ConnectionString = ConnectionString;
                        Conn.Open();
                        //đảm bảo kết nối cơ sở dữ liệu được mở trước khi thực hiện truy vấny
                        if (Conn.State != ConnectionState.Open)
                        {
                            Conn.Open();
                        }
                        // xây dựng truy vấn SQL bằng cách sử dụng các truy vấn được tham số hóa để ngăn SQL injection
                        string _sql = "SELECT * FROM tbl_Hoadon WHERE MaHD LIKE @s0 or MaHD like @s1 or MaHD like @s2";
                        SqlCommand cmd = new SqlCommand(_sql, Conn);
                        cmd.Parameters.AddWithValue("@s0", "%" + _Search.Text + "%");
                        cmd.Parameters.AddWithValue("@s1", _Search.Text + "%");
                        cmd.Parameters.AddWithValue("@s2", "%" + _Search.Text);
                        // tạo bộ adapter dữ liệu và điền vào tập dữ liệu các kết quả của truy vấn
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataSet dataSet = new DataSet();
                        adapter.Fill(dataSet);

                        //đặt nguồn dữ liệu cho lưới dữ liệu
                        dgView_Hoadon.ItemsSource = dataSet.Tables[0].DefaultView;
                        Conn.Close();
                    }
                    else
                    {
                        NapDuLieuTuMayChu();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Loi : " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng lựa chọn danh mục!");
                NapDuLieuTuMayChu();
            }
        }
        // selection
        private void dgView_Hoadon_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (dgView_Hoadon.CurrentItem == null) return;
                DataRowView row = (DataRowView)dgView_Hoadon.CurrentItem;

                _Mahoadon.Text = row[0].ToString().Trim();
                _MaNV.Text = row[1].ToString().Trim();
                _Dongia.Text = row[2].ToString().Trim();
                DP_Ngaylap.Text = row[3].ToString().Trim();

                LayThongTinHoaDon(_Mahoadon.Text);
            } catch (Exception ex)
            {
                MessageBox.Show("Loi select : " + ex.Message);
            }
        }
        // lay thong tin chi tiet hoa don
        private void LayThongTinHoaDon(String input)
        {
            try
            {
                Conn.ConnectionString = ConnectionString;
                Conn.Open();
                string SqlStr = "Select * from tbl_Banhang where MaHD = '" + input+ "'";
                SqlDataAdapter adapter = new SqlDataAdapter(SqlStr, Conn);
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);
                DataSource = dataSet.Tables[0];
                //Thiết lập cho hiển thị lên DataGrid

                dgView_Thongtinhoadon.ItemsSource = DataSource.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi : " + ex.Message, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Conn.Close();
            }
        }
    }
}
