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
using Shop.Cls_thuvien;

namespace Shop.Child_view
{
    /// <summary>
    /// Interaction logic for Form_Taikhoan.xaml
    /// </summary>
    public partial class Form_Taikhoan : Window
    {
        string ConnectionString = @"Data Source=Lenovo\SQLEXPRESS;Initial Catalog=QL_Shop;Integrated Security=True;";
        //Biến Connection để kết nối CSDL
        SqlConnection Conn = new SqlConnection();
        DataTable DataSource = null;
        private SqlDataAdapter adapter;
        private int ID;
        public Form_Taikhoan()
        {
            InitializeComponent();
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
        // button close
        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {

            this.Close();
        }
        // drag form
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
        // load
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            /*try
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
            // cbbox serch
            cbbox_Search.Items.Add("Mã nhân viên");
            // thuc hien nap
            NapDuLieuTuMayChu();
            // load cbbox Manv
            Load_NV();
            Load_BP();
        }
        // load dgview
        private void NapDuLieuTuMayChu()
        {
            try
            {
                dgView.ItemsSource = null;
                Conn.ConnectionString = ConnectionString;
                Conn.Open();
                string SqlStr = "Select t.Account , t.Pass , t.MaNV , n.Hoten , t.Loai from tbl_Taikhoan as t inner join tbl_Nhanvien as n on t.MaNV = n.MaNV";
                SqlDataAdapter adapter = new SqlDataAdapter(SqlStr, Conn);
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);
                DataSource = dataSet.Tables[0];
                //Thiết lập cho hiển thị lên DataGrid
                foreach (DataRow row in DataSource.Rows)
                {
                    // Ví dụ: thay đổi tên nhân viên
                    row[3] = RSA.Decryption(row[3].ToString().Trim());
                }
                dgView.ItemsSource = DataSource.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Loi : " + ex.Message);
            }
            finally
            {
                Conn.Close();
            }
        }
        // button tim kiem
        private void btn_Search_Click(object sender, RoutedEventArgs e)
        {
            String sql = "";
            if (cbbox_Search.SelectedIndex == 0)
            {
                dgView.ItemsSource = null;
                Conn.ConnectionString = ConnectionString;
                Conn.Open();
                sql = "Select Account , Pass , MaNV , Hoten , Loai from tbl_Taikhoan inner join tbl_Nhanvien on tbl_Taikhoan.MaNV = tbl_Nhanvien.MaNV where MaNV = '" + _Search.Text + "'";
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
        //button them moi , tạo tài khoản
        private void btn_themoi_Click(object sender, RoutedEventArgs e)
        {
            if (!check_TT())
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (cbbox_LoaiTK.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);

                return;
            }
            if (check_AC())
            {
                MessageBox.Show("Tài khoản đã tồn tại", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (check_NV())
            {
                MessageBox.Show("Nhân viên đã có tài khoản!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if(Check_Mk(_Pass.Text))
            {
                MessageBox.Show("Độ dài tối thiểu của mật khẩu là 6 ký tự", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            String sql = "insert into tbl_Taikhoan(Account,Pass,MaNV,Loai) values ('"
                + _User.Text + "','"
                + SHA256Example.SHA256Hash(_Pass.Text) + "','"
                + _MaNV.Text + "',N'"
                + cbbox_LoaiTK.SelectedItem.ToString() + "')";
            try
            {
                Conn.ConnectionString = ConnectionString;
                Conn.Open();
                SqlCommand cmd = new SqlCommand(sql, Conn);
                if (MessageBox.Show("Bạn có muốn thêm thông tin?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Thêm mới thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Loi : " + ex.Message);
            }
            finally
            {
                Conn.Close();
            }
            reText();
            NapDuLieuTuMayChu();
            Load_NV();
        }
        // button Xoa
        private void btn_Xoa_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                String sql = "delete from tbl_Taikhoan Where MaNV = '" + _MaNV.Text + "';";
                Conn.ConnectionString = ConnectionString;
                Conn.Open();
                SqlCommand cmd = new SqlCommand(sql, Conn);
                if (MessageBox.Show("Bạn có muốn xóa thông tin không?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Thông tin đã được xóa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    return;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Loi : " + ex.Message);
            }
            finally
            {
                Conn.Close();
            }
            reText();
            Load_NV();
            NapDuLieuTuMayChu();
        }
        // button Sua
        private void btn_Sua_Click(object sender, RoutedEventArgs e)
        {
            SetButtonsState(true);
        }
        // button cap nhat
        private void btn_Capnhat_Click(object sender, RoutedEventArgs e)
        {
            NapDuLieuTuMayChu();
        }
        // selection
        private void dgView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (dgView.CurrentItem == null) return;
                DataRowView row = (DataRowView)dgView.CurrentItem;
                _User.Text = row[0].ToString().Trim();
                _MaNV.Text = row[2].ToString().Trim();
                _Hoten.Text = row[3].ToString().Trim();
                cbbox_LoaiTK.SelectedItem = row[4].ToString().Trim();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Loi : " + ex.Message);
            }
        }
        // sua 1
        private void btn_Sua1_Click(object sender, RoutedEventArgs e)
        {
            if (!check_TT())
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (Check_Mk(_Pass.Text))
            {
                MessageBox.Show("Độ dài tối thiểu của mật khẩu là 6 ký tự", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            String sql = "update tbl_Taikhoan set Account = '" + _User.Text + "',"
                + " Pass = '" + SHA256Example.SHA256Hash(_Pass.Text) + "',"
                + " Loai = N'" + cbbox_LoaiTK.SelectedItem.ToString() + "'"
                + " where MaNV = '" + _MaNV.Text + "';";
            try
            {
                Conn.ConnectionString = ConnectionString;
                Conn.Open();
                SqlCommand cmd = new SqlCommand(sql, Conn);
                if (MessageBox.Show("Bạn có muốn sửa thông tin không?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Sửa thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Loi : " + ex.Message);
            }
            finally
            {
                Conn.Close();
            }
            reText();
            SetButtonsState(false);
            NapDuLieuTuMayChu();
        }
        // button thoat
        private void btn_Thoat_Click(object sender, RoutedEventArgs e)
        {
            SetButtonsState(false);
        }
        // search 
        private void _Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            String sql = "";
            if (cbbox_Search.SelectedIndex == 0)
            {
                if (_Search.Text != "")
                {
                    Conn.ConnectionString = ConnectionString;
                    Conn.Open();
                    dgView.ItemsSource = null;
                    if (Conn.State != ConnectionState.Open) return;
                    sql = "Select t.Account , t.Pass , t.MaNV , n.Hoten , t.Loai from tbl_Taikhoan as t inner join tbl_Nhanvien as n on t.MaNV = n.MaNV where t.MaNV = '" + _Search.Text + "'";
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
            else
            {
                NapDuLieuTuMayChu();
            }
        }

        // set trang thai nut
        private void SetButtonsState(bool Editing)
        {
            //True -> Dang o che do soan thao
            btn_Sua1.Visibility = Editing ? Visibility.Visible : Visibility.Hidden;
            btn_Thoat.Visibility = Editing ? Visibility.Visible : Visibility.Hidden;

            Editing = !Editing;
            btn_themoi.Visibility = Editing ? Visibility.Visible : Visibility.Hidden;
            btn_Sua.Visibility = Editing ? Visibility.Visible : Visibility.Hidden;
            btn_Xoa.Visibility = Editing ? Visibility.Visible : Visibility.Hidden;
            btn_Capnhat.Visibility = Editing ? Visibility.Visible : Visibility.Hidden;
            dgView.IsEnabled = Editing;
            _MaNV.IsEnabled = Editing;
            cbbox_MaNV.IsEnabled = Editing;
        }
        // retext
        private void reText()
        {
            _User.Text = "";
            _Pass.Text = "";
            _MaNV.Text = "";
            cbbox_MaNV.SelectedIndex = -1;
            _Hoten.Text = "";
            cbbox_LoaiTK.SelectedIndex = -1;
        }
        // load cbbox Manv ( nhan vien chua co tai khoan )
        private void Load_NV()
        {
            cbbox_MaNV.SelectedIndex = -1;
            cbbox_MaNV.Items.Clear();
            String sql = "select MaNV from tbl_Nhanvien where not exists (select * from tbl_Taikhoan where tbl_Taikhoan.MaNV = tbl_Nhanvien.MaNV);";
            SqlDataReader reader = null;
            try
            {
                Conn.ConnectionString = ConnectionString;
                Conn.Open();
                SqlCommand cmd = new SqlCommand(sql, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cbbox_MaNV.Items.Add(reader["MaNV"].ToString().Trim());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Loi : " + ex.Message);
            }
            finally
            {
                reader.Close();
                Conn.Close();
            }
        }
        // selection change
        private void cbbox_MaNV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {

                if (cbbox_MaNV.SelectedIndex == -1)
                {
                    _MaNV.Text = String.Empty;
                    _Hoten.Text = String.Empty;
                    cbbox_LoaiTK.SelectedIndex = -1;
                }
                else
                {
                    _MaNV.Text = cbbox_MaNV.SelectedItem.ToString();
                    take_BP();
                    String sql = "select Hoten from tbl_Nhanvien Where MaNV = '" + cbbox_MaNV.SelectedItem.ToString() + "'";
                    SqlDataReader reader = null;
                    try
                    {
                        Conn.ConnectionString = ConnectionString;
                        Conn.Open();
                        SqlCommand cmd = new SqlCommand(sql, Conn);
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            _Hoten.Text = RSA.Decryption(reader["Hoten"].ToString().Trim());
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Loi : " + ex.Message);
                    }
                    finally
                    {
                        reader.Close();
                        Conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Loi : " + ex.Message);
            }
        }
        //check tt
        private bool check_TT()
        {
            if (_MaNV.Text != "" || _Hoten.Text != "" || _MaNV.Text != "")
            {
                return true;
            }
            return false;
        }
        // check account
        private bool check_AC()
        {
            String sql = "select Account from tbl_taikhoan Where Account = '" + _User.Text + "'";
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
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Loi : " + ex.Message);
            }
            finally
            {
                reader.Close();
                Conn.Close();
            }
            return false;
        }
        // check tk nv
        private bool check_NV()
        {
            String sql = "select Account from tbl_taikhoan Where MaNV = '" + _MaNV.Text + "'";
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
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Loi : " + ex.Message);
            }
            finally
            {
                reader.Close();
                Conn.Close();
            }
            return false;
        }
        // load item cbb bộ phận
        private void Load_BP()
        {
            cbbox_LoaiTK.SelectedIndex = -1;
            cbbox_LoaiTK.Items.Clear();
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
                    cbbox_LoaiTK.Items.Add(reader[0].ToString().Trim());
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
        // lấy thông tin từ bp
        // lay MBP
        private void take_BP()
        {
            SqlDataReader reader = null;
            try
            {
                Conn.ConnectionString = ConnectionString;
                Conn.Open();
                String sql = @"SELECT TenBophan FROM tbl_Bophan as b inner join tbl_Nhanvien as n on b.MaBP = n.MaBP where n.MaNV = N'" + _MaNV.Text + "'";
                SqlCommand cmd = new SqlCommand(sql, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cbbox_LoaiTK.SelectedItem = reader[0].ToString().Trim();
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
        // kiểm tra độ dài của mật khẩu
        private bool Check_Mk(String input)
        {
            if(input.Length < 6)
            {
                return true;
            }
            return false;
        }
    }
}
