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
using System.Security.Policy;

namespace Shop.Child_view
{
    /// <summary>
    /// Interaction logic for Form_Nhaphang.xaml
    /// </summary>
    public partial class Form_Nhaphang : Window
    {
        string ConnectionString = @"Data Source=Lenovo\SQLEXPRESS;Initial Catalog=QL_Shop;Integrated Security=True;";
        //Biến Connection để kết nối CSDL
        SqlConnection Conn = new SqlConnection();
        DataTable DataSource = null;
        private int ID = -1;
        private String Soluong;
        private SqlDataAdapter adapter;
        public Form_Nhaphang()
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
                try
                {
                    if (_Search.Text != "")
                    {
                        Conn.ConnectionString = ConnectionString;
                        Conn.Open();
                        dgView.ItemsSource = null;
                        if (Conn.State != ConnectionState.Open) return;
                        sql = "select n.MaNhap , n.MaHang , n.Tenhang ,  n.Soluong , l.Tenloai , n.NhaCC , n.NgayNhap , n.Giatri , n .Trangthai from tbl_Nhaphang as n inner join tbl_Loai as l on n.Maloai = l.Maloai Where MaNhap ='" + _Search.Text + "';";
                        SqlDataAdapter adapter = new SqlDataAdapter(sql, Conn);
                        DataSet dataSet = new DataSet();
                        adapter.Fill(dataSet);
                        DataSource = dataSet.Tables[0];
                        //Thiết lập cho hiển thị lên DataGrid
                        dgView.ItemsSource = DataSource.DefaultView;
                        Conn.Close();
                    }
                    else
                    {
                        NapDuLieuTuMayChu();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi : " + ex.Message);
                }
            }
            else if (cbbox_Search.SelectedIndex == 1)
            {
                try
                {
                    if (_Search.Text != "")
                    {
                        // xóa nguồn của lưới dữ liệu
                        dgView.ItemsSource = null;
                        Conn.ConnectionString = ConnectionString;
                        Conn.Open();
                        //đảm bảo kết nối cơ sở dữ liệu được mở trước khi thực hiện truy vấny
                        if (Conn.State != ConnectionState.Open)
                        {
                            Conn.Open();
                        }
                        // xây dựng truy vấn SQL bằng cách sử dụng các truy vấn được tham số hóa để ngăn SQL injection
                        string _sql = "select n.MaNhap , n.MaHang , n.Tenhang ,  n.Soluong , l.Tenloai , n.NhaCC , n.NgayNhap , n.Giatri , n .Trangthai from tbl_Nhaphang as n inner join tbl_Loai as l on n.Maloai = l.Maloai WHERE n.MaHang LIKE @s0 or n.MaHang like @s1 or n.MaHang like @s2";
                        SqlCommand cmd = new SqlCommand(_sql, Conn);
                        cmd.Parameters.AddWithValue("@s0", "%" + _Search.Text + "%");
                        cmd.Parameters.AddWithValue("@s1", _Search.Text + "%");
                        cmd.Parameters.AddWithValue("@s2", "%" + _Search.Text);
                        // tạo bộ adapter dữ liệu và điền vào tập dữ liệu các kết quả của truy vấn
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataSet dataSet = new DataSet();
                        adapter.Fill(dataSet);

                        //đặt nguồn dữ liệu cho lưới dữ liệu
                        dgView.ItemsSource = dataSet.Tables[0].DefaultView;
                        Conn.Close();
                    }
                    else
                    {
                        NapDuLieuTuMayChu();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi : " + ex.Message, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
            else
            {
                NapDuLieuTuMayChu();
            }
        }
        // text change
        private void label_Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            String sql = "";
            if (cbbox_Search.SelectedIndex == 0)
            {
                try
                {
                    if (_Search.Text != "")
                    {
                        Conn.ConnectionString = ConnectionString;
                        Conn.Open();
                        dgView.ItemsSource = null;
                        if (Conn.State != ConnectionState.Open) return;
                        sql = "select n.MaNhap , n.MaHang , n.Tenhang ,  n.Soluong , l.Tenloai , n.NhaCC , n.NgayNhap , n.Giatri , n .Trangthai from tbl_Nhaphang as n inner join tbl_Loai as l on n.Maloai = l.Maloai Where MaNhap ='" + _Search.Text + "';";
                        SqlDataAdapter adapter = new SqlDataAdapter(sql, Conn);
                        DataSet dataSet = new DataSet();
                        adapter.Fill(dataSet);
                        DataSource = dataSet.Tables[0];
                        //Thiết lập cho hiển thị lên DataGrid
                        dgView.ItemsSource = DataSource.DefaultView;
                        Conn.Close();
                    }
                    else
                    {
                        NapDuLieuTuMayChu();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi : " + ex.Message);
                }
            }
            else if (cbbox_Search.SelectedIndex == 1)
            {
                try
                {
                    if (_Search.Text != "")
                    {
                        // xóa nguồn của lưới dữ liệu
                        dgView.ItemsSource = null;
                        Conn.ConnectionString = ConnectionString;
                        Conn.Open();
                        //đảm bảo kết nối cơ sở dữ liệu được mở trước khi thực hiện truy vấny
                        if (Conn.State != ConnectionState.Open)
                        {
                            Conn.Open();
                        }
                        // xây dựng truy vấn SQL bằng cách sử dụng các truy vấn được tham số hóa để ngăn SQL injection
                        string _sql = "select n.MaNhap , n.MaHang , n.Tenhang ,  n.Soluong , l.Tenloai , n.NhaCC , n.NgayNhap , n.Giatri , n .Trangthai from tbl_Nhaphang as n inner join tbl_Loai as l on n.Maloai = l.Maloai WHERE n.MaHang LIKE @s0 or n.MaHang like @s1 or n.MaHang like @s2";
                        SqlCommand cmd = new SqlCommand(_sql, Conn);
                        cmd.Parameters.AddWithValue("@s0", "%" + _Search.Text + "%");
                        cmd.Parameters.AddWithValue("@s1", _Search.Text + "%");
                        cmd.Parameters.AddWithValue("@s2", "%" + _Search.Text);
                        // tạo bộ adapter dữ liệu và điền vào tập dữ liệu các kết quả của truy vấn
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataSet dataSet = new DataSet();
                        adapter.Fill(dataSet);

                        //đặt nguồn dữ liệu cho lưới dữ liệu
                        dgView.ItemsSource = dataSet.Tables[0].DefaultView;
                        Conn.Close();
                    }
                    else
                    {
                        NapDuLieuTuMayChu();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi : " + ex.Message, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
            else
            {
                NapDuLieuTuMayChu();
            }


        }
        // load dgview
        private void NapDuLieuTuMayChu()
        {
            dgView.ItemsSource = null;
            Conn.ConnectionString = ConnectionString;
            Conn.Open();
            try
            {
                string SqlStr = "select n.MaNhap , n.MaHang , n.Tenhang ,  n.Soluong , l.Tenloai , n.NhaCC , n.NgayNhap , n.Giatri , n .Trangthai from tbl_Nhaphang as n inner join tbl_Loai as l on n.Maloai = l.Maloai";
                SqlDataAdapter adapter = new SqlDataAdapter(SqlStr, Conn);
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);
                DataSource = dataSet.Tables[0];
                //Thiết lập cho hiển thị lên DataGrid

                dgView.ItemsSource = DataSource.DefaultView;
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
        // kiem tra ky tu
        public static bool check(String input)
        {
            long result;
            bool _check = long.TryParse(input, out result);
            return _check;
        }
        // load form      
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // cbb trang thai
            cbbox_Trangthai.Items.Add("Đã thanh toán");
            cbbox_Trangthai.Items.Add("Chưa thanh toán");
            // cbb tim kiem
            cbbox_Search.Items.Add("Mã nhập");
            cbbox_Search.Items.Add("Mã hàng");
            /* try
             {
                 //Mở kết nối
                 ConnectionString = @"Data Source=Lenovo\SQLEXPRESS;Initial Catalog=QL_Shop;Integrated Security=True;";
                 Conn.ConnectionString = ConnectionString;
                 Conn.Open();
             }
             catch (Exception ex)
             {
                 MessageBox.Show("Lỗi khi mở kết nối");
             }*/
            // thuc hien nap
            NapDuLieuTuMayChu();
            Load_MH();
            Load_L();
        }
        // load mã hàng
        private void Load_MH()
        {
            cbbox_Mahang.SelectedIndex = -1;
            cbbox_Mahang.Items.Clear();
            SqlDataReader read = null;
            try
            {
                Conn.ConnectionString = ConnectionString;
                Conn.Open();
                String sql = @"SELECT MaHang FROM tbl_Hang";
                SqlCommand smd = new SqlCommand(sql, Conn);
                read = smd.ExecuteReader();
                while (read.Read())
                {
                    cbbox_Mahang.Items.Add(read["MaHang"].ToString().Trim());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi (Load_MH)" + ex.Message);
            }
            finally
            {
                read.Close();
                Conn.Close();
            }
        }
        // check Mh
        private bool check_MH()
        {
            String sql = "select Soluong from tbl_Hang where MaHang = '" + _Mahang.Text + "';";
            SqlDataReader reader = null;
            try
            {
                Conn.ConnectionString = ConnectionString;
                Conn.Open();
                if (Conn.State == ConnectionState.Open)
                {
                    SqlCommand cmd = new SqlCommand(sql, Conn);
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Soluong = reader["Soluong"].ToString().Trim();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("LOi : " + ex.Message);
            }
            finally
            {
                reader.Close();
                Conn.Close();
            }
            return false;
        }
        //insert Hang
        private void Insert_Hang()
        {
            try
            {
                Conn.ConnectionString = ConnectionString;
                Conn.Open();
                String sqlStr = "Insert Into tbl_Hang (MaHang,Tenhang,NhaCC,Soluong,Maloai,NgayCN) values(" +
                      "'" + _Mahang.Text + "'," +
                      "N'" + _Tenhang.Text + "'," +
                      "N'" + _NhaCC.Text + "    '," +
                      "'" + _Soluong.Text + "'," +
                      "'" + ID + "'," +
                      "'" + DP_Ngaynhap.Text + "')";
                SqlCommand cmd = new SqlCommand(sqlStr, Conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Loi insert hang : " + ex.Message);
            }
            finally
            {
                Conn.Close();
            }
        }
        // update mat hang khi nhap
        private void Update_Hang_Nhap()
        {
            String sql = "update tbl_Hang set Soluong = '" + Cong_SL() + "', NgayCN = '" + DP_Ngaynhap.Text + "' where MaHang = '" + _Mahang.Text + "';";
            try
            {
                Conn.ConnectionString = ConnectionString;
                Conn.Open();
                SqlCommand cmd = new SqlCommand(sql, Conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Loi update hang : " + ex.Message);
            }
            finally
            {
                Conn.Close();
            }
        }
        // update măt hang khi huy don
        private void Update_Hang_HuyNhap()
        {
            String sql = "update tbl_Hang set Soluong = '" + Tru_SL() + "' where MaHang = '" + _Mahang.Text + "';";
            try
            {
                Conn.ConnectionString = ConnectionString;
                Conn.Open();
                SqlCommand cmd = new SqlCommand(sql, Conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Loi update hang : " + ex.Message);
            }
            finally
            {
                Conn.Close();
            }
        }
        // load cbb loai
        private void Load_L()
        {
            cbbox_Loai.SelectedIndex = -1;
            cbbox_Loai.Items.Clear();
            SqlDataReader reader = null;
            try
            {
                Conn.ConnectionString = ConnectionString;
                Conn.Open();
                String sql = @"SELECT Tenloai FROM tbl_Loai";
                SqlCommand smd = new SqlCommand(sql, Conn);
                reader = smd.ExecuteReader();
                while (reader.Read())
                {
                    cbbox_Loai.Items.Add(reader["Tenloai"].ToString().Trim());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load cbbox loai : " + ex.Message);
            }
            finally
            {
                reader.Close();
                Conn.Close();
            }
        }
        // kiem tra loai
        private bool Check_L()
        {
            SqlDataReader reader = null;
            try
            {
                Conn.ConnectionString = ConnectionString;
                Conn.Open();
                String sql = "SELECT Maloai FROM tbl_Loai WHERE Tenloai = N'" + _Loai.Text.Trim() + "';";
                SqlCommand cmd = new SqlCommand(sql, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ID = int.Parse(reader[0].ToString().Trim());
                    return true;
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi check Loai : " + ex.Message);
            }
            finally
            {
                reader.Close();
                Conn.Close();
            }

            return false;
        }
        // insert loai
        private void Insert_Loai()
        {
            try
            {
                Conn.ConnectionString = ConnectionString;
                Conn.Open();
                String sql = "insert into tbl_Loai(Tenloai) values(N'" + _Loai.Text + "');";
                SqlCommand cmd = new SqlCommand(sql, Conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Loi insert loai : " + ex.Message);
            }
            finally
            {
                Conn.Close();
            }
        }
        // insert nhap hang
        private void Insert_Nhaphang()
        {
            String sql = "insert into tbl_Nhaphang(MaNhap,MaHang,Tenhang,Soluong,Maloai,NhaCC,NgayNhap,Giatri,Trangthai) values('"
                + _MaNhap.Text + "','"
                + _Mahang.Text + "',N'"
                + _Tenhang.Text + "','"
                + _Soluong.Text + "',N'"
                + ID + "',N'"
                + _NhaCC.Text + "','"
                + DP_Ngaynhap.Text + "','"
                + _Giatri.Text + "','"
                + cbbox_Trangthai.Text + "');";
            try
            {
                Conn.ConnectionString = ConnectionString;
                Conn.Open();
                SqlCommand cmd = new SqlCommand(sql, Conn);
                if (MessageBox.Show(" Bạn có muốn nhập mặt hàng này không?\n Thông tin của mặt hàng sẽ tự động cập nhật vào kho hàng.", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Nhập thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Loi insert nhap hang : " + ex.Message);
            }
            finally
            {
                Conn.Close();
            }
            reText();
            Load_L();
            Load_MH();
            NapDuLieuTuMayChu();
        }
        // xóa đơn nhập hàng
        private void Delete_Nhaphang()
        {
            String sql = "delete from tbl_Nhaphang where MaNhap = '" + _MaNhap.Text + "';";
            try
            {
                Conn.ConnectionString = ConnectionString;
                Conn.Open();
                SqlCommand cmd = new SqlCommand(sql, Conn);
                if (MessageBox.Show("Bạn có muốn xóa đơn nhập hàng không?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    cmd.ExecuteNonQuery();
                    MessageBox.Show(" Hủy đơn thành công !\n Thông tin sẽ được cập nhật tự động vào kho hàng.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Loi xoa don hang : " + ex.Message);
            }
            finally
            {
                Conn.Close();
            }
            reText();
            NapDuLieuTuMayChu();
        }
        // select 
        private void dgView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (dgView.CurrentItem == null)
                {
                    return;
                }
                else
                {
                    DataRowView row = (DataRowView)dgView.CurrentItem;
                    _MaNhap.Text = row[0].ToString().Trim();
                    cbbox_Mahang.SelectedItem = row[1].ToString().Trim();
                    _Tenhang.Text = row[2].ToString().Trim();
                    _Soluong.Text = row[3].ToString().Trim();
                    cbbox_Loai.SelectedItem = row[4].ToString().Trim();
                    _NhaCC.Text = row[5].ToString().Trim();
                    DP_Ngaynhap.Text = row[6].ToString().Trim();
                    _Giatri.Text = row[7].ToString().Trim();
                    if (row[8].ToString().Trim().Contains("Đã thanh toán"))
                    {
                        cbbox_Trangthai.SelectedIndex = 0;
                    }
                    else
                    {
                        cbbox_Trangthai.SelectedIndex = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        // select change
        private void cbbox_Mahang_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            String m = null;
            try
            {
                if (cbbox_Mahang.SelectedIndex == -1)
                {
                    _Mahang.Text = String.Empty;

                }
                else
                {
                    String sql = "select Tenhang , Maloai from tbl_Hang where Mahang = '" + cbbox_Mahang.SelectedItem.ToString().Trim() + "'";
                    SqlDataReader reader = null;
                    try
                    {
                        Conn.ConnectionString = ConnectionString;
                        Conn.Open();
                        _Mahang.Text = cbbox_Mahang.SelectedItem.ToString();
                        if (Conn.State != ConnectionState.Open) return;
                        SqlCommand cmd = new SqlCommand(sql, Conn);
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            _Tenhang.Text = reader["Tenhang"].ToString().Trim();
                            m = reader["Maloai"].ToString().Trim();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("loi selection cbbox_loai : " + ex.Message);
                    }
                    finally
                    {
                        reader.Close();
                        Conn.Close();
                    }
                }
                Get_L(m);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi (cbbox_Mahang_sl) : " + ex.Message);
            }
        }
        // lay loai
        private void Get_L(String input)
        {
            String sql = "select Tenloai from tbl_Loai where Maloai = '" + input + "'";
            SqlDataReader reader = null;
            try
            {
                Conn.ConnectionString = ConnectionString;
                Conn.Open();
                SqlCommand cmd = new SqlCommand(sql, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cbbox_Loai.SelectedItem = reader["Tenloai"].ToString().Trim();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Loi Get_L : " + ex.Message);
            }
            finally
            {
                reader.Close();
                Conn.Close();
            }
        }
        // cbb loai
        private void cbbox_Loai_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cbbox_Loai.SelectedIndex == -1)
                {
                    _Loai.Text = String.Empty;
                }
                else
                {
                    _Loai.Text = cbbox_Loai.SelectedItem.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Loi : " + ex.Message);
            }
        }
        // nhập
        private void btn_Nhap_Click(object sender, RoutedEventArgs e)
        {
            if (Check_TT())
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!Check_So(_Soluong.Text))
            {
                MessageBox.Show("Số lượng không bao gồm ký tự chữ !", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!Check_So(_Giatri.Text))
            {
                MessageBox.Show("Giá trị không bao gồm ký tự chữ !", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if(check_MDN())
            {
                MessageBox.Show("Mã nhập hàng đã tồn tại!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (Check_L())
            {
                if (check_MH())
                {
                    Update_Hang_Nhap();
                    Insert_Nhaphang();
                }
                else
                {
                    Insert_Hang();
                    Insert_Nhaphang();
                }
            }
            else
            {
                Insert_Loai();
                Check_L();
                if (check_MH())
                {
                    Update_Hang_Nhap();
                    Insert_Nhaphang();
                }
                else
                {
                    Insert_Hang();
                    Insert_Nhaphang();
                }
            }
        }
        //  cập nhật
        private void btn_Capnhat_Click(object sender, RoutedEventArgs e)
        {
            NapDuLieuTuMayChu();
        }
        //
        private void btn_Thoat_Click(object sender, RoutedEventArgs e)
        {
            SetButtonsState(false);
            SetIsEnable(true);
        }
        // Hủy
        private void btn_Huy__Click(object sender, RoutedEventArgs e)
        {
            if (Check_TT())
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin hoặc lựa chọn trong bảng bên", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            check_MH();
            Update_Hang_HuyNhap();
            Delete_Nhaphang();
        }
        // đưa vào giao diện hủy
        private void btn_Huy_Click(object sender, RoutedEventArgs e)
        {
            if (dgView.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn trường thông tin cần xóa", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            SetButtonsState(true);
            SetIsEnable(false);
        }
        // set trang thai nut
        private void SetButtonsState(bool Editing)
        {
            //True -> Dang o che do soan thao
            btn_Huy_.Visibility = Editing ? Visibility.Visible : Visibility.Hidden;
            btn_Thoat.Visibility = Editing ? Visibility.Visible : Visibility.Hidden;
            lb_thongbao.Visibility = Editing ? Visibility.Visible : Visibility.Hidden;

            Editing = !Editing;
            btn_Nhap.Visibility = Editing ? Visibility.Visible : Visibility.Hidden;
            btn_Capnhat.Visibility = Editing ? Visibility.Visible : Visibility.Hidden;
            btn_Huy.Visibility = Editing ? Visibility.Visible : Visibility.Hidden;
        }
        // 
        private void SetIsEnable(bool Editing)
        {
            _MaNhap.IsEnabled = Editing;
            _Mahang.IsEnabled = Editing;
            cbbox_Mahang.IsEnabled = Editing;
            _Tenhang.IsEnabled = Editing;
            _Soluong.IsEnabled = Editing;
            _Loai.IsEnabled = Editing;
            cbbox_Loai.IsEnabled = Editing;
            _NhaCC.IsEnabled = Editing;
            _Giatri.IsEnabled = Editing;
            DP_Ngaynhap.IsEnabled = Editing;
            cbbox_Trangthai.IsEnabled = Editing;
        }
        // kiem tra tt
        private bool Check_TT()
        {
            if (_MaNhap.Text == "" || _Mahang.Text == "" || _Tenhang.Text == "" || _Soluong.Text == "" || _Loai.Text == "" || _NhaCC.Text == "" || DP_Ngaynhap.Text == "" || _Giatri.Text == "" || cbbox_Trangthai.Text == "")
            {
                return true;
            }

            return false;
        }
        // kiem tra int
        private bool Check_So(String input)
        {
            int result;
            bool check = int.TryParse(input, out result);
            return check;
        }
        // reText
        private void reText()
        {
            _MaNhap.Text = "";
            cbbox_Mahang.SelectedIndex = -1;
            _Tenhang.Text = "";
            _Soluong.Text = "";
            cbbox_Loai.SelectedIndex = -1;
            _NhaCC.Text = "";
            DP_Ngaynhap.Text = "";
            _Giatri.Text = "";
            cbbox_Trangthai.SelectedIndex = -1;
        }
        // Cong so luong
        private String Cong_SL()
        {
            int a = int.Parse(_Soluong.Text);
            int b = int.Parse(Soluong);
            return (a + b).ToString();
        }
        // Tru so luong
        private String Tru_SL()
        {
            String output;
            int b = int.Parse(Soluong);
            int a = int.Parse(_Soluong.Text);
            int c = b - a;
            if (c >= 0)
            {
                output = Convert.ToString(c);
            }
            else
            {
                c = 0;
                output = Convert.ToString(0);
            }
            return output;
        }


        // check mdn
        private bool check_MDN()
        {
            String sql = "select MaNhap from tbl_Nhaphang where MaNhap = '" + _MaNhap.Text + "'";
            SqlDataReader reader = null;
            try
            {
                Conn.ConnectionString = ConnectionString;
                Conn.Open();
                SqlCommand cmd = new SqlCommand(sql, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Loi Get_L : " + ex.Message);
            }
            finally
            {
                reader.Close();
                Conn.Close();
            }
            return false;
        }
    }
}

