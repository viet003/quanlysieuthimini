using Shop.Cls_thuvien;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Shop.Child_view
{
    /// <summary>
    /// Interaction logic for Form_Nhanvien.xaml
    /// </summary> 
    public partial class Form_Nhanvien : Window
    {
        string ConnectionString = @"Data Source=Lenovo\SQLEXPRESS;Initial Catalog=QL_Shop;Integrated Security=True;";
        //Biến Connection để kết nối CSDL
        SqlConnection Conn = new SqlConnection();
        DataTable DataSource = null;
        private SqlDataAdapter adapter;
        string MBP;
        public Form_Nhanvien()
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
        // button minisize
        private void btn_Minisize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
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
        // btn_CLose
        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {
                Application.Current.Shutdown();
            }
            else
            {
                this.Close();
            }
        }
        // load
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            // add item cbb_trangthai
            cbbox_Trangthai.Items.Add("Hoạt động");
            cbbox_Trangthai.Items.Add("Đã nghỉ");

            // add item cbb_search
            cbbox_Search.Items.Add("Mã nhân viên");
            //cbbox_Search.Items.Add("Họ và tên");
            /*
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
            Load_BP();
        }
        // them moi
        private void btn_themoi_Click(object sender, RoutedEventArgs e)
        {
            if (_MaNV.Text == "" || _Hoten.Text == "" || _Diachi.Text == "" || DP_Ngaysinh.Text == "" || _SDT.Text == "" || cbbox_Bophan.Text == "" || cbbox_Trangthai.Text == "")
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                take_MBP();
                Conn.ConnectionString = ConnectionString;
                Conn.Open();
                String sql = "select * from tbl_Nhanvien Where MaNV = '" + _MaNV.Text + "';";
                SqlDataAdapter adapter = new SqlDataAdapter(sql, Conn);
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);
                // kiem tra mnv ton tai hay chua
                if (dataSet.Tables[0].Rows.Count > 0)
                {
                    MessageBox.Show("Mã nhân viên đã tồn tại!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    Conn.Close();
                    return;
                }
                Conn.Close();
                // kiem tra dinh dang SDt
                if (!check_SDT(_SDT.Text))
                {
                    MessageBox.Show("Số điện thoại không bao gồm chữ!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (MessageBox.Show("Bạn chắc chắn muốn thêm bản ghi?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    
                    try
                    {
                        Conn.ConnectionString = ConnectionString;
                        Conn.Open();
                        String sqlStr = "Insert Into tbl_Nhanvien (MaNV,Hoten,Ngaysinh,Diachi,SDT,MaBP,Trangthai)values(" +
                        "'" + _MaNV.Text + "'," +
                        "N'" + RSA.Encryption(_Hoten.Text) + "'," +
                        "'" + RSA.Encryption(DP_Ngaysinh.Text) + "'," +
                        "N'" + RSA.Encryption(_Diachi.Text) + "'," +
                        "'" + RSA.Encryption(_SDT.Text) + "'," +
                        "'" + MBP + "'," +
                        "N'" + RSA.Encryption(cbbox_Trangthai.Text) + "')";
                        SqlCommand cmd = new SqlCommand(sqlStr, Conn);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Thêm mới thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Loi insert nv : " + ex.Message);
                    }
                    finally
                    {
                        Conn.Close();
                    }
                }
                else
                {
                    return;
                }
                NapDuLieuTuMayChu();
                reText();
            }
        }
        // sua
        private void btn_sua_Click(object sender, RoutedEventArgs e)
        {
            SetButtonsState(true);
        }
        // xoa
        private void btn_xoa_Click(object sender, RoutedEventArgs e)
        {
            string sql = "Delete from tbl_Nhanvien Where MaNV = '" + _MaNV.Text + "';";

            try
            {
                Conn.ConnectionString = ConnectionString;
                Conn.Open();
                if (MessageBox.Show("Bạn có chắc chắn muốn xóa bản ghi?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    SqlCommand cmd = new SqlCommand(sql, Conn);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Xóa thông tin thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    return;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi phát sinh khi xóa bản ghi.");
            }
            finally
            {
                Conn.Close();
            }
            reText();
            NapDuLieuTuMayChu();
        }
        // cap nhat
        private void btn_capnhat_Click(object sender, RoutedEventArgs e)
        {
            NapDuLieuTuMayChu();
        }
        // button search
        private void btn_Search_Click(object sender, RoutedEventArgs e)
        {
            String sql = "";
            if (cbbox_Search.SelectedIndex == 0)
            {
                if (_Search.Text != "")
                {
                    Conn.ConnectionString = ConnectionString;
                    Conn.Open();
                    dgView.ItemsSource = null;
                    if (Conn.State != ConnectionState.Open)
                    {
                        Conn.Open();
                    }
                    sql = "Select n.MaNV , n.Hoten , n.Ngaysinh , n.Diachi , n.SDT , b.TenBophan , n.Trangthai from tbl_Nhanvien as n inner join tbl_Bophan as b on n.MaBP = b.MaBP Where n.MaNV ='" + _Search.Text + "';";
                    SqlDataAdapter adapter = new SqlDataAdapter(sql, Conn);
                    DataSet dataSet = new DataSet();
                    adapter.Fill(dataSet);
                    DataSource = dataSet.Tables[0];

                    foreach (DataRow row in DataSource.Rows)
                    {
                        // Ví dụ: thay đổi tên nhân viên
                        row[1] = RSA.Decryption(row[1].ToString().Trim());
                        row[2] = RSA.Decryption(row[2].ToString().Trim());
                        row[3] = RSA.Decryption(row[3].ToString().Trim());
                        row[4] = RSA.Decryption(row[4].ToString().Trim());
                        row[6] = RSA.Decryption(row[6].ToString().Trim());
                    }

                    //Thiết lập cho hiển thị lên DataGrid
                    dgView.ItemsSource = DataSource.DefaultView;
                    Conn.Close();
                }
                else
                {
                    NapDuLieuTuMayChu();
                }
            }
            else if (cbbox_Search.SelectedIndex == 1)
            {
                if (_Search.Text != "")
                {
                    // xóa nguồn của lưới dữ liệu
                    dgView.ItemsSource = null;

                    //đảm bảo kết nối cơ sở dữ liệu được mở trước khi thực hiện truy vấny
                    if (Conn.State != ConnectionState.Open)
                    {
                        Conn.Open();
                    }
                    // xây dựng truy vấn SQL bằng cách sử dụng các truy vấn được tham số hóa để ngăn SQL injection
                    string _sql = "Select n.MaNV , n.Hoten , n.Ngaysinh , n.Diachi , n.SDT , b.TenBophan , n.Trangthai from tbl_Nhanvien as n inner join tbl_Bophan as b on n.MaBP = b.MaBP WHERE n.Hoten LIKE @searchTerm";
                    SqlCommand cmd = new SqlCommand(_sql, Conn);
                    cmd.Parameters.AddWithValue("@searchTerm", "%" + _Search.Text + "%");

                    // tạo bộ adapter dữ liệu và điền vào tập dữ liệu các kết quả của truy vấn
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataSet dataSet = new DataSet();
                    adapter.Fill(dataSet);
                    DataSource = dataSet.Tables[0];

                    foreach (DataRow row in DataSource.Rows)
                    {
                        // Ví dụ: thay đổi tên nhân viên
                        row[1] = RSA.Decryption(row[1].ToString().Trim());
                        row[2] = RSA.Decryption(row[2].ToString().Trim());
                        row[3] = RSA.Decryption(row[3].ToString().Trim());
                        row[4] = RSA.Decryption(row[4].ToString().Trim());
                        row[6] = RSA.Decryption(row[6].ToString().Trim());
                    }

                    //Thiết lập cho hiển thị lên DataGrid
                    dgView.ItemsSource = DataSource.DefaultView;
                    Conn.Close();
                }
                else
                {
                    NapDuLieuTuMayChu();
                }
            }
            else
            {
                NapDuLieuTuMayChu();
            }
        }
        // dgView seclected
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
                    _MaNV.Text = row[0].ToString().Trim();
                    _Hoten.Text = row[1].ToString().Trim();
                    DP_Ngaysinh.Text = row[2].ToString().Trim();
                    _Diachi.Text = row[3].ToString().Trim();
                    _SDT.Text = row[4].ToString().Trim();
                    cbbox_Bophan.SelectedItem = row[5].ToString().Trim();
                    cbbox_Trangthai.SelectedItem = row[6].ToString().Trim();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        // load dgview
        private void NapDuLieuTuMayChu()
        {
            Conn.ConnectionString = ConnectionString;
            Conn.Open();
            dgView.ItemsSource = null;
            string SqlStr = "Select n.MaNV , n.Hoten , n.Ngaysinh , n.Diachi , n.SDT , b.TenBophan , n.Trangthai from tbl_Nhanvien as n inner join tbl_Bophan as b on n.MaBP = b.MaBP";
            SqlDataAdapter adapter = new SqlDataAdapter(SqlStr, Conn);
            DataSet dataSet = new DataSet(); // lưu trữ dữ liệu từ cơ sổ dữ liệu 
            adapter.Fill(dataSet); // đổ dữ liệu và dataset
            DataSource = dataSet.Tables[0]; // lấy dữ liệu từ bảng table 0 gán cho bảng dl
                                            //Thiết lập cho hiển thị lên DataGrid

            // Thay đổi dữ liệu trong DataTable theo nhu cầu của bạn
            foreach (DataRow row in DataSource.Rows)
            {
                // Ví dụ: thay đổi tên nhân viên
                row[1] = RSA.Decryption(row[1].ToString().Trim());
                row[2] = RSA.Decryption(row[2].ToString().Trim());
                row[3] = RSA.Decryption(row[3].ToString().Trim());
                row[4] = RSA.Decryption(row[4].ToString().Trim());
                row[6] = RSA.Decryption(row[6].ToString().Trim());
            }

            dgView.ItemsSource = DataSource.DefaultView;
            Conn.Close();
        }
        // tim khi thay doi noi dung text_Search
        private void label_Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            String sql = "";
            if (cbbox_Search.SelectedIndex == 0)
            {
                if (_Search.Text != "")
                {
                    Conn.ConnectionString = ConnectionString;
                    Conn.Open();
                    dgView.ItemsSource = null;
                    if (Conn.State != ConnectionState.Open)
                    {
                        Conn.Open();
                    }
                    sql = "Select n.MaNV , n.Hoten , n.Ngaysinh , n.Diachi , n.SDT , b.TenBophan , n.Trangthai from tbl_Nhanvien as n inner join tbl_Bophan as b on n.MaBP = b.MaBP Where n.MaNV ='" + _Search.Text + "';";
                    SqlDataAdapter adapter = new SqlDataAdapter(sql, Conn);
                    DataSet dataSet = new DataSet();
                    adapter.Fill(dataSet);
                    DataSource = dataSet.Tables[0];

                    foreach (DataRow row in DataSource.Rows)
                    {
                        // Ví dụ: thay đổi tên nhân viên
                        row[1] = RSA.Decryption(row[1].ToString().Trim());
                        row[2] = RSA.Decryption(row[2].ToString().Trim());
                        row[3] = RSA.Decryption(row[3].ToString().Trim());
                        row[4] = RSA.Decryption(row[4].ToString().Trim());
                        row[6] = RSA.Decryption(row[6].ToString().Trim());
                    }

                    //Thiết lập cho hiển thị lên DataGrid
                    dgView.ItemsSource = DataSource.DefaultView;
                    Conn.Close();
                }
                else
                {
                    NapDuLieuTuMayChu();
                }
            }
            else if (cbbox_Search.SelectedIndex == 1)
            {
                if (_Search.Text != "")
                {
                    // xóa nguồn của lưới dữ liệu
                    dgView.ItemsSource = null;

                    //đảm bảo kết nối cơ sở dữ liệu được mở trước khi thực hiện truy vấny
                    if (Conn.State != ConnectionState.Open)
                    {
                        Conn.Open();
                    }
                    // xây dựng truy vấn SQL bằng cách sử dụng các truy vấn được tham số hóa để ngăn SQL injection
                    string _sql = "Select n.MaNV , n.Hoten , n.Ngaysinh , n.Diachi , n.SDT , b.TenBophan , n.Trangthai from tbl_Nhanvien as n inner join tbl_Bophan as b on n.MaBP = b.MaBP WHERE n.Hoten LIKE @searchTerm";
                    SqlCommand cmd = new SqlCommand(_sql, Conn);
                    cmd.Parameters.AddWithValue("@searchTerm", "%" + _Search.Text + "%");

                    // tạo bộ adapter dữ liệu và điền vào tập dữ liệu các kết quả của truy vấn
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataSet dataSet = new DataSet();
                    adapter.Fill(dataSet);
                    DataSource = dataSet.Tables[0];

                    foreach (DataRow row in DataSource.Rows)
                    {
                        // Ví dụ: thay đổi tên nhân viên
                        row[1] = RSA.Decryption(row[1].ToString().Trim());
                        row[2] = RSA.Decryption(row[2].ToString().Trim());
                        row[3] = RSA.Decryption(row[3].ToString().Trim());
                        row[4] = RSA.Decryption(row[4].ToString().Trim());
                        row[6] = RSA.Decryption(row[6].ToString().Trim());
                    }

                    //Thiết lập cho hiển thị lên DataGrid
                    dgView.ItemsSource = DataSource.DefaultView;
                    Conn.Close();
                }
                else
                {
                    NapDuLieuTuMayChu();
                }

            }
            else
            {
                NapDuLieuTuMayChu();
            }
            Conn.Close();
        }
        // set trang thai nut
        private void SetButtonsState(bool Editing)
        {
            //True -> Dang o che do soan thao
            btn_Sua_.Visibility = Editing ? Visibility.Visible : Visibility.Hidden;
            btn_Thoat_.Visibility = Editing ? Visibility.Visible : Visibility.Hidden;

            Editing = !Editing;
            btn_themoi.Visibility = Editing ? Visibility.Visible : Visibility.Hidden;
            btn_sua.Visibility = Editing ? Visibility.Visible : Visibility.Hidden;
            btn_xoa.Visibility = Editing ? Visibility.Visible : Visibility.Hidden;
            btn_capnhat.Visibility = Editing ? Visibility.Visible : Visibility.Hidden;
            dgView.IsEnabled = Editing;
            _MaNV.IsEnabled = Editing;
        }
        // button thoat
        private void btn_Thoat__Click(object sender, RoutedEventArgs e)
        {
            SetButtonsState(false);
        }
        // button sua 1
        private void btn_Sua__Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // kiem tra dinh dang SDt
                if (!check_SDT(_SDT.Text))
                {
                    MessageBox.Show("Số điện thoại không bao gồm chữ!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (MessageBox.Show("Bạn có chắc chắn muốn sửa?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    take_MBP();
                    Conn.ConnectionString = ConnectionString;
                    Conn.Open();
                    String sqlStr = "Update tbl_Nhanvien Set " +
                     "Hoten = N'" + RSA.Encryption(_Hoten.Text) + "'," +
                     "Ngaysinh='" + RSA.Encryption(DP_Ngaysinh.Text) + "'," +
                     "Diachi=N'" + RSA.Encryption(_Diachi.Text) + "'," +
                     "SDT='" + RSA.Encryption(_SDT.Text) + "'," +
                     "MaBP ='" + MBP + "'," +
                     "Trangthai=N'" + RSA.Encryption(cbbox_Trangthai.Text) + "'" +
                     " Where MaNV = '" + _MaNV.Text + "';";
                    SqlCommand cmd = new SqlCommand(sqlStr, Conn);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Dữ liệu đã được sửa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Conn.Close();
            }
            reText();
            NapDuLieuTuMayChu();
            SetButtonsState(false);
        }
        // load item cbb bộ phận
        private void Load_BP()
        {
            cbbox_Bophan.SelectedIndex = -1;
            cbbox_Bophan.Items.Clear();
            SqlDataReader reader = null;
            try
            {
                Conn.ConnectionString = ConnectionString;
                Conn.Open();
                String sql = @"SELECT TenBophan FROM tbl_Bophan";
                SqlCommand cmd = new SqlCommand(sql, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cbbox_Bophan.Items.Add(reader[0].ToString().Trim());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                reader.Close();
                Conn.Close();
            }
        }
        // kiem tra dinh dang sdt
        public bool check_SDT(String sdt)
        {
            int result;
            bool _check = int.TryParse(sdt, out result);
            return _check;
        }
        // btn Tài khoản
        private void btn_Taikhoan_Click(object sender, RoutedEventArgs e)
        {
            Form_Taikhoan tk = new Form_Taikhoan();
            this.Hide();
            tk.ShowDialog();
            this.ShowDialog();
        }
        // btn Bo phan
        private void btn_Bophan_Click(object sender, RoutedEventArgs e)
        {
            Form_Bophan tk = new Form_Bophan();
            this.Hide();
            tk.ShowDialog();
            this.ShowDialog();
        }
        // retext
        private void reText()
        {
            _MaNV.Text = "";
            _Hoten.Text = "";
            DP_Ngaysinh.Text = "";
            _Diachi.Text = "";
            _SDT.Text = "";
            cbbox_Trangthai.SelectedIndex = -1;
        }
        // lấy ra MaBP
        private void cbbox_Bophan_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
        // lay MBP
        private void take_MBP()
        {
            SqlDataReader reader = null;
            try
            {
                Conn.ConnectionString = ConnectionString;
                Conn.Open();
                String sql = @"SELECT MaBP FROM tbl_Bophan where TenBophan = N'" + cbbox_Bophan.Text + "'";
                SqlCommand cmd = new SqlCommand(sql, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    MBP = reader[0].ToString().Trim();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                reader.Close();
                Conn.Close();
            }
        }
    }
}
