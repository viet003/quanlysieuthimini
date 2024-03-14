
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
using System.Linq.Expressions;
using Shop.Cls_thuvien;
using System.Collections;
using System.IO;

namespace Shop.Child_view
{
    /// <summary>
    /// Interaction logic for Form_Banhang.xaml
    /// </summary>
    public partial class Form_Banhang : Window
    {
        public String MaNV;
        private int index;
        private int soluong;
        //Biến Connection để kết nối CSDL
        SqlConnection Conn = new SqlConnection();
        DataTable DataSource = null;
        // bien connection
        String ConnectionString = @"Data Source=Lenovo\SQLEXPRESS;Initial Catalog=QL_Shop;Integrated Security=True;";
        private SqlDataAdapter adapter;
        ArrayList List_loai = new ArrayList();
        List<BanHang> list_BH = new List<BanHang>();
        public Form_Banhang()
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
        // button minisize
        private void btn_Minisize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        // button sua
        private void btn_Sua_Click(object sender, RoutedEventArgs e)
        {
            if (Check_TT())
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!Check_H())
            {
                MessageBox.Show("Mã hàng không tồn tại!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Sua();
        }
        // button them moi
        private void btn_themoi_Click(object sender, RoutedEventArgs e)
        {
            if (Check_TT())
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!Check_H())
            {
                MessageBox.Show("Mã hàng không tồn tại!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (check_Hoadon())
            {
                MessageBox.Show("Hóa đơn đã tồn tại !", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            long a = LaySoluong(_MaHang.Text) - long.Parse(_Soluong.Text);
            if (a < 0)
            {
                MessageBox.Show("Số lượng còn lại trong kho không đủ", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if(check_list_H())
            {
                if(MessageBox.Show("Mặt hàng này đã có trong danh sách?\nNếu tiếp tục thêm mới , số lượng hàng hóa sẽ cộng thêm số lượng thêm mới.", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                {
                    ADD_H();
                    reText();
                }
                return;
            }
            Them();
        }
        // button xoa
        private void btn_Xoa_Click(object sender, RoutedEventArgs e)
        {
            Xoa();
        }
        // button thanh toan
        private void btn_Thanhtoan_Click(object sender, RoutedEventArgs e)
        {
            Thanhtoan();
        }
        //button Lam moi
        private void btn_Lammoi_Click(object sender, RoutedEventArgs e)
        {
            Lammoi();
        }
        // load loai hang
        private void Load_L()
        {
            SqlDataReader reader = null;
            try
            {
                Conn.ConnectionString = ConnectionString;
                Conn.Open();
                if (Conn.State != ConnectionState.Open) return;
                String sql = @"SELECT * FROM tbl_Loai";
                SqlCommand smd = new SqlCommand(sql, Conn);
                reader = smd.ExecuteReader();
                while (reader.Read())
                {
                    List_loai.Add(int.Parse(reader["Maloai"].ToString()));
                    cbbox_Loai.Items.Add(reader["Tenloai"].ToString().Trim());
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
        // lay thong tin tu don
        private void LayThongTin(String input)
        {
            String sql = "select Tenhang, Dvt , Gia from tbl_Hang where MaHang = '" + input + "'";
            SqlDataReader reader = null;
            try
            {
                Conn.ConnectionString = ConnectionString;
                Conn.Open();
                SqlCommand cmd = new SqlCommand(sql, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    _Tenhang.Text = reader["Tenhang"].ToString().Trim();
                    _Dvt.Text = reader["Dvt"].ToString().Trim();
                    _Gia.Text = reader["Gia"].ToString().Trim();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Loi lay thong tin : " + ex.Message);
            }
            finally
            {
                reader.Close();
                Conn.Close();
            }
        }
        // lua chon cbbox ma hang
        private void cbbox_MaHang_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbbox_MaHang.SelectedIndex == -1)
            {
                _MaHang.Text = String.Empty;
                _Tenhang.Text = String.Empty;
                _Dvt.Text = String.Empty;
            }
            else
            {
                _MaHang.Text = cbbox_MaHang.SelectedItem.ToString();
                LayThongTin(cbbox_MaHang.SelectedItem.ToString());
            }
        }
        // load dvt
        private void Load_DVT()
        {
            String sql = "select distinct Dvt from tbl_Hang";
            SqlDataReader reader = null;
            try
            {
                Conn.ConnectionString = ConnectionString;
                Conn.Open();
                SqlCommand cmd = new SqlCommand(sql, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cbbox_Dvt.Items.Add(reader["Dvt"].ToString().Trim());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Loi load dvt : " + ex.Message);
            }
            finally
            {
                reader.Close();
                Conn.Close();
            }
        }
        // load hang
        private void Load_H()
        {
            String sql = "select Mahang from tbl_Hang";
            SqlDataReader reader = null;
            try
            {
                Conn.ConnectionString = ConnectionString;
                Conn.Open();
                SqlCommand cmd = new SqlCommand(sql, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cbbox_MaHang.Items.Add(reader["MaHang"].ToString().Trim());
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Loi load Hang : " + ex.Message);
            }
            finally
            {
                reader.Close();
                Conn.Close();
            }
        }
        // nạp dữ liệu vào table
        private void NapDuLieuTuMayChu()
        {
            dgView_Hanghoa.ItemsSource = null;
            try
            {
                Conn.ConnectionString = ConnectionString;
                Conn.Open();
                string SqlStr = "Select * from tbl_Hang";
                SqlDataAdapter adapter = new SqlDataAdapter(SqlStr, Conn);
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);
                DataSource = dataSet.Tables[0];
                //Thiết lập cho hiển thị lên DataGrid

                dgView_Hanghoa.ItemsSource = DataSource.DefaultView;
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
        // load form
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cbbox_Search.Items.Add("Mã hàng");
            cbbox_Search.Items.Add("Tên hàng");
            cbbox_Search.Items.Add("Loại");
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
            // thuc hien nap
            NapDuLieuTuMayChu();
            Load_L();
            Load_H();
            Load_DVT();
            dgView_Hoadon.ItemsSource = list_BH;
            _Gia.Text = Convert.ToString(0);
            DP_Ngayban.Text = DateTime.Now.ToString();
            DP_Ngayban.IsEnabled = false;
        }
        // lựa chọn mục tìm kếm tự động cập nhật bảng
        private void cbbox_Loai_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List_loai.ToArray();
            try
            {
                Conn.ConnectionString = ConnectionString;
                Conn.Open();
                String sql = "SELECT * FROM tbl_Hang WHERE Maloai = '" + List_loai[cbbox_Loai.SelectedIndex] + "';";
                adapter = new SqlDataAdapter(sql, Conn);
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);
                DataSource = dataSet.Tables[0];
                //Thiết lập cho hiển thị lên DataGrid

                dgView_Hanghoa.ItemsSource = DataSource.DefaultView;
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
        // lựa chọn mục tìm kiếm
        private void cbbox_Search_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbbox_Search.SelectedItem.Equals("Loại"))
            {
                cbbox_Loai.Visibility = Visibility.Visible;
            }
            else
            {
                cbbox_Loai.Visibility = Visibility.Hidden;
            }
        }
        // tự động cập nhật thành tiền khi thay đổi số lượng
        private void _Soluong_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (_Soluong.Text.Equals(""))
                {
                    _Soluong.Text = string.Empty;
                    _Thanhtien.Text = Convert.ToString(0);
                }
                else
                {
                    long a = long.Parse(_Soluong.Text);
                    long b = long.Parse(_Gia.Text);

                    _Thanhtien.Text = Convert.ToString(a * b);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Loi :" + ex.Message);
            }
        }
        // data grid selection
        private void dgView_Hanghoa_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (dgView_Hanghoa.CurrentItem == null)
                {
                    return;
                }
                else
                {
                    DataRowView row = (DataRowView)dgView_Hanghoa.CurrentItem;
                    _MaHang.Text = row[0].ToString().Trim();
                    _Tenhang.Text = row[1].ToString().Trim();
                    cbbox_Dvt.Text = row[2].ToString().Trim();
                    _Gia.Text = row[3].ToString().Trim();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Loi : " + ex.Message);
            }
        }
        // tim kiem khi thay doi text search
        private void _Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            String sql = "";
            if (cbbox_Search.SelectedIndex == 0)
            {
                if (_Search.Text != "")
                {
                    try
                    {
                        dgView_Hanghoa.ItemsSource = null;
                        Conn.ConnectionString = ConnectionString;
                        Conn.Open();
                        sql = "select * from tbl_Hang Where MaHang ='" + _Search.Text + "';";
                        SqlDataAdapter adapter = new SqlDataAdapter(sql, Conn);
                        DataSet dataSet = new DataSet();
                        adapter.Fill(dataSet);
                        DataSource = dataSet.Tables[0];
                        //Thiết lập cho hiển thị lên DataGrid
                        dgView_Hanghoa.ItemsSource = DataSource.DefaultView;
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
                        dgView_Hanghoa.ItemsSource = null;
                        Conn.ConnectionString = ConnectionString;
                        Conn.Open();
                        //đảm bảo kết nối cơ sở dữ liệu được mở trước khi thực hiện truy vấny
                        if (Conn.State != ConnectionState.Open)
                        {
                            Conn.Open();
                        }
                        // xây dựng truy vấn SQL bằng cách sử dụng các truy vấn được tham số hóa để ngăn SQL injection
                        string _sql = "SELECT * FROM tbl_Hang WHERE TenHang LIKE @s0 or TenHang like @s1 or TenHang like @s2";
                        SqlCommand cmd = new SqlCommand(_sql, Conn);
                        cmd.Parameters.AddWithValue("@s0", "%" + _Search.Text + "%");
                        cmd.Parameters.AddWithValue("@s1", _Search.Text + "%");
                        cmd.Parameters.AddWithValue("@s2", "%" + _Search.Text);
                        // tạo bộ adapter dữ liệu và điền vào tập dữ liệu các kết quả của truy vấn
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataSet dataSet = new DataSet();
                        adapter.Fill(dataSet);

                        //đặt nguồn dữ liệu cho lưới dữ liệu
                        dgView_Hanghoa.ItemsSource = dataSet.Tables[0].DefaultView;
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
        // check TT
        private bool Check_TT()
        {
            if (_MaHang.Text == "" || _Tenhang.Text == "" || _Dvt.Text == "" || _Madonhang.Text == "" || _Soluong.Text == "" || _Gia.Text == "" || _Thanhtien.Text == "" || DP_Ngayban.Text == "")
            {
                return true;
            }
            return false;
        }
        // retext
        private void reText()
        {
            cbbox_MaHang.SelectedIndex = -1;
            _Tenhang.Text = "";
            _Dvt.Text = "";
            _Soluong.Text = "";
            _Gia.Text = String.Empty;
            _Thanhtien.Text = String.Empty;
        }
        // lay so luong
        private long LaySoluong(String input)
        {
            long a = 0;
            String sql = "Select Soluong from tbl_Hang where MaHang = '" +input+ "'";
            SqlDataReader reader = null;
            try
            {
                Conn.ConnectionString = ConnectionString;
                Conn.Open();
                SqlCommand cmd = new SqlCommand(sql, Conn);
                reader = cmd.ExecuteReader(); 
                while(reader.Read())
                {
                    a = Convert.ToInt32(reader["Soluong"].ToString().Trim());
                }

            } catch(Exception ex)
            {
                MessageBox.Show("Loi lay so luong hang hoa! : " + ex.Message);
            } finally
            {
                reader.Close();
                Conn.Close();
            }
            return a;
        }
        // Them moi
        private void Them()
        {
            if (MessageBox.Show("Bạn có muốn thêm không?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                BanHang bh = new BanHang(_Madonhang.Text, _MaHang.Text, _Soluong.Text, _Gia.Text, _Thanhtien.Text);
                list_BH.Add(bh);
                dgView_Hoadon.SelectedIndex = -1;
                dgView_Hoadon.ItemsSource = null;
                dgView_Hoadon.ItemsSource = list_BH;
                _Madonhang.IsEnabled = false;
                reText();
            }
        }
        //Sua
        private void Sua()
        {
            if (dgView_Hoadon.SelectedIndex != -1)
            {
                if (MessageBox.Show("Bạn có muốn sửa thông tin không?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    _Madonhang.IsEnabled = true;
                    list_BH[index].MaHD = _Madonhang.Text;
                    list_BH[index].MaHang = _MaHang.Text;
                    list_BH[index].Dongia = _Gia.Text;
                    list_BH[index].Soluong = _Soluong.Text;
                    list_BH[index].Thanhtien = _Thanhtien.Text;
                    dgView_Hoadon.SelectedIndex = -1;
                    dgView_Hoadon.ItemsSource = null;
                    dgView_Hoadon.ItemsSource = list_BH;
                }
            }
            else
            {
                MessageBox.Show("Vui lòng lựa chọn hàng cần sửa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        // Xoa
        private void Xoa()
        {
            if (dgView_Hoadon.SelectedIndex != -1)
            {
                if (MessageBox.Show("Bạn có muốn xóa thông tin không?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    list_BH.RemoveAt(index);
                    MessageBox.Show("Xóa thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    dgView_Hoadon.SelectedIndex = -1;
                    dgView_Hoadon.ItemsSource = null;
                    dgView_Hoadon.ItemsSource = list_BH;
                }
            }
            else
            {
                MessageBox.Show("Vui lòng lựa chọn hàng cần xóa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        // Thanh toan
        private void Thanhtoan()
        {
            if (MessageBox.Show("Bạn có muốn thanh toán đơn hàng không?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Insert_TTHoadon();
                Insert_TTDonhang();
                Update_Kho();
                NapDuLieuTuMayChu();
                _Madonhang.Text = "";
                MessageBox.Show("Thanh toán thành công!\nTổng tiền : " + TongHoaDon().ToString() + "đ", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                Lammoi();
            }
        }
        // insert tthoa don
        private void Insert_TTHoadon()
        {
            String sql = @"insert into tbl_Hoadon(MaHD,MaNV,Ngaylap,Tongtien) values(@s1,@s2,@s3,@s4);";
            try
            {
                Conn.ConnectionString = ConnectionString;
                Conn.Open();
                SqlCommand cmd = new SqlCommand(sql, Conn);
                cmd.Parameters.AddWithValue("@s1", _Madonhang.Text.Trim());
                cmd.Parameters.AddWithValue("@s2", MaNV.Trim());
                cmd.Parameters.AddWithValue("@s3", DP_Ngayban.Text.Trim());
                cmd.Parameters.AddWithValue("@s4", Convert.ToString(TongHoaDon()));
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Loi insert hoadon : " + ex.Message);
                return;
            }
            finally
            {
                Conn.Close();
            }
        }
        // insert thong tin don hang
        private void Insert_TTDonhang()
        {
            String sql = "insert into tbl_Banhang(MaHD,MaHang,Soluong,Dongia,Thanhtien) values(@s0,@s1,@s2,@s3,@s4);";
            Conn.ConnectionString = ConnectionString;
            Conn.Open();
            try
            {
                foreach (var i in list_BH)
                {
                    SqlCommand cmd = new SqlCommand(sql, Conn);
                    cmd.Parameters.AddWithValue("@s0", i.MaHD);
                    cmd.Parameters.AddWithValue("@s1", i.MaHang);
                    cmd.Parameters.AddWithValue("@s2", i.Soluong);
                    cmd.Parameters.AddWithValue("@s3", i.Dongia);
                    cmd.Parameters.AddWithValue("@s4", i.Thanhtien);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Loi insert thong tin hoa don : " + ex.Message);
                return;
            }
            finally
            {
                Conn.Close();
            }
        }
        // update kho
        private void Update_Kho()
        {
            String sql = "update tbl_Hang set Soluong = @s1 where MaHang = @s2";
            try
            {
                foreach (var i in list_BH)
                {
                    long a = LaySoluong(i.MaHang) - long.Parse(i.Soluong);
                    Conn.ConnectionString = ConnectionString;
                    Conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, Conn);
                    cmd.Parameters.AddWithValue("@s1", a);
                    cmd.Parameters.AddWithValue("@s2", i.MaHang);
                    cmd.ExecuteNonQuery();
                    Conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Loi insert thong tin hoa don : " + ex.Message);
                return;
            }

        }
        // select
        private void cbbox_Dvt_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbbox_Dvt.SelectedIndex == -1)
            {
                _Dvt.Text = String.Empty;
            }
            else
            {
                _Dvt.Text = cbbox_Dvt.SelectedItem.ToString();
            }
        }
        // select
        private void dgView_Hoadon_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (dgView_Hoadon.SelectedIndex != -1)
                {
                    index = dgView_Hoadon.SelectedIndex;
                    cbbox_MaHang.SelectedItem = list_BH[index].MaHang.ToString();
                    _Madonhang.Text = list_BH[index].MaHD.ToString();
                    _Gia.Text = list_BH[index].Dongia.ToString();
                    _Soluong.Text = list_BH[index].Soluong.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Loi selection : " + ex.Message);
            }
        }
        // tinh tong gia tri hoa don
        private long TongHoaDon()
        {
            long a = 0;
            try
            {
                foreach (var i in list_BH)
                {
                    a += Convert.ToInt32(i.Thanhtien);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Loi : " + ex.Message);
            }
            return a;
        }
        // lam moi
        private void Lammoi()
        {
            reText();         
            _Madonhang.IsEnabled = true;
            _Madonhang.Text = "";
            list_BH.Clear();
            dgView_Hoadon.ItemsSource = null;
            dgView_Hoadon.ItemsSource = list_BH;
        }
        // check_Mh
        private bool Check_H()
        {
            String sql = "select * from tbl_Hang where MaHang = '" + _MaHang.Text + "'";
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

            }
            finally
            {
                reader.Close();
                Conn.Close();
            }
            return false;
        }
        
        // check hoa don
        private bool check_Hoadon()
        {
            String sql = "select * from tbl_Hoadon where MaHD = '" +_Madonhang.Text+ "'";
            SqlDataReader reader = null;
            try
            {
                Conn.ConnectionString = ConnectionString;
                Conn.Open();
                SqlCommand cmd = new SqlCommand(sql, Conn);
                reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    return true;
                }

            } catch (Exception ex)
            {
                MessageBox.Show("LOi check tt hoadon : " + ex.Message);
            } finally
            {
                reader.Close();
                Conn.Close();
            }
            return false;
        }

        // kiểm tra xem trong list đã tồn tại mã hàng này chưa
        private bool check_list_H()
        {
            foreach(var i in list_BH)
            {
                if(i.MaHang == _MaHang.Text)
                {
                    soluong = int.Parse(i.Soluong);
                    return true;
                }
            }
            return false;
        }

        // cập nhật lại số lượng hàng trong list khi thêm mới hàng đã có trước đó
        private void ADD_H()
        {
            foreach( var i in list_BH)
            {
                if(i.MaHang == _MaHang.Text)
                {
                    i.Soluong = Convert.ToString(soluong + int.Parse(_Soluong.Text));
                }
            }

            dgView_Hoadon.SelectedIndex = -1;
            dgView_Hoadon.ItemsSource = null;
            dgView_Hoadon.ItemsSource = list_BH;
        }
    }

}
