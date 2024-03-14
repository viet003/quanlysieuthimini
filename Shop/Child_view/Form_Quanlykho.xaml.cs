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

namespace Shop.Child_view
{
    /// <summary>
    /// Interaction logic for Form_Quanlykho.xaml
    /// </summary>
    public partial class Form_Quanlykho : Window
    {
        string ConnectionString = @"Data Source=Lenovo\SQLEXPRESS;Initial Catalog=QL_Shop;Integrated Security=True;";
        //Biến Connection để kết nối CSDL
        SqlConnection Conn = new SqlConnection();
        DataTable DataSource = null;
        private SqlDataAdapter adapter;
        private int Maloai;
        public Form_Quanlykho()
        {
            InitializeComponent();
        }
        // Drag form
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
        // button close
        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        // load form
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // add item cbb_search
            cbbox_Search.Items.Add("Mã hàng");
            cbbox_Search.Items.Add("Tên hàng");
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
            Load_DVT();
        }
        // them moi
        private void btn_themoi_Click(object sender, RoutedEventArgs e)
        {

            if (_Mahang.Text == "" || _Tenhang.Text == "" || _Dongia.Text == "" || _Dvt.Text == "" || _NhaSX.Text == "" || _Soluong.Text == "" || _Dvt.Text == "")
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (MessageBox.Show("Bạn có muốn thêm bản ghi?", "Thông báo", MessageBoxButton.YesNo,MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        Conn.ConnectionString = ConnectionString;
                        Conn.Open();
                        String sql = "select * from tbl_Hang Where MaHang = '" + _Mahang.Text + "';";
                        SqlDataAdapter adapter = new SqlDataAdapter(sql, Conn);
                        DataSet dataSet = new DataSet();
                        adapter.Fill(dataSet);
                        // kiem tra mnv ton tai hay chua
                        if (dataSet.Tables[0].Rows.Count > 0)
                        {
                            MessageBox.Show("Mã hàng hóa đã tồn tại!","Thông báo", MessageBoxButton.OK,MessageBoxImage.Error);
                            Conn.Close();
                            return;
                        }
                        Conn.Close();
                        // kiem tra dinh dang Đơn giá
                        if (!check(_Dongia.Text))
                        {
                            MessageBox.Show("Đơn giá không bao gồm chữ!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        // kiem tra dinh dang Số lượng
                        if (!check(_Soluong.Text))
                        {
                            MessageBox.Show("Số lượng không bao gồm chữ!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        if(Check_L())
                        {
                            Conn.ConnectionString = ConnectionString;
                            Conn.Open();
                            String sqlStr = "Insert Into tbl_Hang (MaHang,Tenhang,Dvt,Gia,NhaCC,Soluong,Maloai,NgayCN) values(" +
                                "'" + _Mahang.Text + "'," +
                                "N'" + _Tenhang.Text + "'," +
                                "N'" + _Dvt.Text + "'," +
                                "'" + _Dongia.Text + "'," +
                                "N'" + _NhaSX.Text + "    '," +
                                "'" + _Soluong.Text + "'," +
                                "N'" + Maloai + "'," +
                                "'" + DP_Ngaycapnhat.Text + "')";
                            SqlCommand cmd = new SqlCommand(sqlStr, Conn);
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Thêm mới thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                            Conn.Close();
                        } else
                        {
                            Insert_Loai();
                            Check_L();
                            Conn.ConnectionString = ConnectionString;
                            Conn.Open();
                            String sqlStr = "Insert Into tbl_Hang (MaHang,Tenhang,Dvt,Gia,NhaCC,Soluong,Maloai,NgayCN) values(" +
                                "'" + _Mahang.Text + "'," +
                                "N'" + _Tenhang.Text + "'," +
                                "N'" + _Dvt.Text + "'," +
                                "'" + _Dongia.Text + "'," +
                                "N'" + _NhaSX.Text + "    '," +
                                "'" + _Soluong.Text + "'," +
                                "N'" + Maloai + "'," +
                                "'" + DP_Ngaycapnhat.Text + "')";
                            SqlCommand cmd = new SqlCommand(sqlStr, Conn);
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Thêm mới thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                            Conn.Close();                        
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    reText();
                    Load_L();
                    Load_DVT();
                    NapDuLieuTuMayChu();
                }
                else
                {
                    return;
                }
            }
        }
        // nhan button sua
        private void btn_sua_Click(object sender, RoutedEventArgs e)
        {
            SetButtonsState(true);
        }
        // button loai
        private void btn_Loai_Click(object sender, RoutedEventArgs e)
        {
            Form_Loaihanghoa lh = new Form_Loaihanghoa();
            this.Hide();
            lh.ShowDialog();
            this.ShowDialog();
            NapDuLieuTuMayChu();
            Load_L();
        }
        // xoa
        private void btn_xoa_Click(object sender, RoutedEventArgs e)
        {
            String sql = "Delete from tbl_Hang Where MaHang = '" + _Mahang.Text + "';";
            try
            {
                Conn.ConnectionString = ConnectionString;
                Conn.Open();
                if (MessageBox.Show("Bạn chắc chắn muốn xóa bản ghi?", "Thông báo", MessageBoxButton.YesNo , MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    SqlCommand cmd = new SqlCommand(sql, Conn);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Bản ghi đã được xóa." , "Thông báo" , MessageBoxButton.OK , MessageBoxImage.Question);

                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi phát sinh khi xóa bản ghi.");
            } finally
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
        // button tim kiem
        private void btn_Search_Click(object sender, RoutedEventArgs e)
        {
            String sql = "";
            if (cbbox_Search.SelectedIndex == 0)
            {
                try
                {
                    if(_Search.Text != "")
                    {
                        Conn.ConnectionString = ConnectionString;
                        Conn.Open();
                        dgView.ItemsSource = null;
                        if (Conn.State != ConnectionState.Open) return;
                        sql = "Select h.MaHang , h.Tenhang , h.Dvt , h.Gia , h.NhaCC , h.Soluong , l.Tenloai , h.NgayCN from tbl_Hang as h inner join tbl_Loai as l on h.Maloai = l.Maloai  Where MaHang ='" + _Search.Text + "' order by h.MaHang ;";
                        SqlDataAdapter adapter = new SqlDataAdapter(sql, Conn);
                        DataSet dataSet = new DataSet();
                        adapter.Fill(dataSet);
                        DataSource = dataSet.Tables[0];
                        //Thiết lập cho hiển thị lên DataGrid
                        dgView.ItemsSource = DataSource.DefaultView;
                        Conn.Close();
                    } else
                    {
                        NapDuLieuTuMayChu();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Loi :" + ex.Message);
                }
            }
            else if (cbbox_Search.SelectedIndex == 1)
            {
                // xóa nguồn của lưới dữ liệu
                try
                {
                    if(_Search.Text != "")
                    {
                        dgView.ItemsSource = null;
                        Conn.ConnectionString = ConnectionString;
                        Conn.Open();
                        //đảm bảo kết nối cơ sở dữ liệu được mở trước khi thực hiện truy vấny
                        if (Conn.State != ConnectionState.Open)
                        {
                            Conn.Open();
                        }
                        // xây dựng truy vấn SQL bằng cách sử dụng các truy vấn được tham số hóa để ngăn SQL injection
                        string _sql = "Select h.MaHang , h.Tenhang , h.Dvt , h.Gia , h.NhaCC , h.Soluong , l.Tenloai , h.NgayCN from tbl_Hang as h inner join tbl_Loai as l on h.Maloai = l.Maloai  WHERE TenHang LIKE @s0 or TenHang like @s1 or TenHang like @s2 order by h.MaHang";
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
                    } else
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
                NapDuLieuTuMayChu();
            }
        }
        // button thoat
        private void btn_thoat_Click(object sender, RoutedEventArgs e)
        {
            SetButtonsState(false);
        }
        // sua 1
        private void btn_Sua1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // kiem tra dinh dang đơn giá
                if (!check(_Dongia.Text))
                {
                    MessageBox.Show("Đơn giá  không bao gồm chữ!", "Thông báo" , MessageBoxButton.OK , MessageBoxImage.Error);
                    return;
                }
                // kiem tra dinh dang số lượng
                if (!check(_Soluong.Text))
                {
                    MessageBox.Show("Số lượng không bao gồm chữ!","Thông báo", MessageBoxButton.OK , MessageBoxImage.Error);
                    return;
                }
                if (MessageBox.Show("Bạn có chắc chắn muốn sửa?", "Thông báo", MessageBoxButton.YesNo,MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                   if(Check_L())
                    {
                        try
                        {
                            Conn.ConnectionString = ConnectionString;
                            Conn.Open();
                            String sqlStr = "Update tbl_Hang Set " +
                             "TenHang = N'" + _Tenhang.Text + "'," +
                             "Dvt='" + _Dvt.Text + "'," +
                             "Gia='" + _Dongia.Text + "'," +
                             "NhaCC=N'" + _NhaSX.Text + "'," +
                             "Soluong='" + _Soluong.Text + "'," +
                             "Maloai=N'" + Maloai + "'," +
                             "NgayCN=N'" + DP_Ngaycapnhat.Text + "'" +
                             " Where MaHang = '" + _Mahang.Text + "';";
                            SqlCommand cmd = new SqlCommand(sqlStr, Conn);
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Dữ liệu đã được sửa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                            reText();
                            Conn.Close();
                            Load_L();
                            Load_DVT();
                            NapDuLieuTuMayChu();
                            SetButtonsState(false);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Lỗi : " + ex.Message);
                        }
                    } else
                    {
                        Insert_Loai();
                        Check_L();
                        try
                        {
                            Conn.ConnectionString = ConnectionString;
                            Conn.Open();
                            String sqlStr = "Update tbl_Hang Set " +
                             "TenHang = N'" + _Tenhang.Text + "'," +
                             "Dvt='" + _Dvt.Text + "'," +
                             "Gia='" + _Dongia.Text + "'," +
                             "NhaCC=N'" + _NhaSX.Text + "'," +
                             "Soluong='" + _Soluong.Text + "'," +
                             "Maloai=N'" + Maloai + "'," +
                             "NgayCN=N'" + DP_Ngaycapnhat.Text + "'" +
                             " Where MaHang = '" + _Mahang.Text + "';";
                            SqlCommand cmd = new SqlCommand(sqlStr, Conn);
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Dữ liệu đã được sửa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                            reText();
                            Conn.Close();
                            Load_L();
                            Load_DVT();
                            NapDuLieuTuMayChu();
                            SetButtonsState(false);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Lỗi : " + ex.Message);
                        }
                    }
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
        }
        // tim kiem khi thay doi gia tri tai textbox
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
                        sql = "Select h.MaHang , h.Tenhang , h.Dvt , h.Gia , h.NhaCC , h.Soluong , l.Tenloai , h.NgayCN from tbl_Hang as h inner join tbl_Loai as l on h.Maloai = l.Maloai  Where MaHang ='" + _Search.Text + "' order by h.MaHang ;";
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
                    MessageBox.Show("Loi :" + ex.Message);
                }
            }
            else if (cbbox_Search.SelectedIndex == 1)
            {
                // xóa nguồn của lưới dữ liệu
                try
                {
                    if (_Search.Text != "")
                    {
                        dgView.ItemsSource = null;
                        Conn.ConnectionString = ConnectionString;
                        Conn.Open();
                        //đảm bảo kết nối cơ sở dữ liệu được mở trước khi thực hiện truy vấny
                        if (Conn.State != ConnectionState.Open)
                        {
                            Conn.Open();
                        }
                        // xây dựng truy vấn SQL bằng cách sử dụng các truy vấn được tham số hóa để ngăn SQL injection
                        string _sql = "Select h.MaHang , h.Tenhang , h.Dvt , h.Gia , h.NhaCC , h.Soluong , l.Tenloai , h.NgayCN from tbl_Hang as h inner join tbl_Loai as l on h.Maloai = l.Maloai  WHERE TenHang LIKE @s0 or TenHang like @s1 or TenHang like @s2 order by h.MaHang";
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
                    MessageBox.Show("Loi : " + ex.Message);
                }

            }
            else
            {
                NapDuLieuTuMayChu();
            }
        }
        // selection changed dgview
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
                    _Mahang.Text = (row[0].ToString()).Trim();
                    _Tenhang.Text = (row[1].ToString()).Trim();
                    cbbox_dvt.Text = (row[2].ToString()).Trim();
                    _Dongia.Text = row[3].ToString().Trim();
                    _NhaSX.Text = row[4].ToString().Trim();
                    _Soluong.Text = row[5].ToString().Trim();
                    cbbox_Loai.SelectedItem = row[6].ToString().Trim();
                    DP_Ngaycapnhat.Text = row[7].ToString().Trim();
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
            dgView.ItemsSource = null;
            try
            {
                Conn.ConnectionString = ConnectionString;
                Conn.Open();
                string SqlStr = "Select h.MaHang , h.Tenhang , h.Dvt , h.Gia , h.NhaCC , h.Soluong , l.Tenloai , h.NgayCN from tbl_Hang as h inner join tbl_Loai as l on h.Maloai = l.Maloai ORDER BY h.MaHang";
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
            } finally
            {
                Conn.Close();
            }
        }
        // set trang thai nut
        private void SetButtonsState(bool Editing)
        {
            //True -> Dang o che do soan thao
            btn_Sua1.Visibility = Editing ? Visibility.Visible : Visibility.Hidden;
            btn_thoat.Visibility = Editing ? Visibility.Visible : Visibility.Hidden;

            Editing = !Editing;
            btn_themoi.Visibility = Editing ? Visibility.Visible : Visibility.Hidden;
            btn_sua.Visibility = Editing ? Visibility.Visible : Visibility.Hidden;
            btn_xoa.Visibility = Editing ? Visibility.Visible : Visibility.Hidden;
            btn_capnhat.Visibility = Editing ? Visibility.Visible : Visibility.Hidden;
            dgView.IsEnabled = Editing;
            _Mahang.IsEnabled = Editing;
        }
        // check dongia
        public static bool check(String input)
        {
            long result;
            bool _check = long.TryParse(input, out result);
            return _check;
        }
        // load dvt
        private void Load_DVT()
        {
            cbbox_dvt.SelectedIndex = -1;
            cbbox_dvt.Items.Clear();
            String sql = "select distinct Dvt from tbl_Hang";
            SqlDataReader reader = null;
            try
            {
                Conn.ConnectionString = ConnectionString;
                Conn.Open();
                SqlCommand cmd = new SqlCommand(sql, Conn);
                reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    cbbox_dvt.Items.Add(reader["Dvt"].ToString().Trim());   
                }
            } catch (Exception ex)
            {
                MessageBox.Show("Loi load dvt : " + ex.Message);
            } finally
            {
                reader.Close();
                Conn.Close();
            }
        }
        // load loai
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
                SqlCommand cmd = new SqlCommand(sql, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cbbox_Loai.Items.Add(reader[0].ToString().Trim());
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
                    Maloai = int.Parse(reader[0].ToString().Trim());
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
            } finally
            {
                Conn.Close();
            }
        }
        // chon dvt
        private void cbbox_dvt_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if ( cbbox_dvt.SelectedIndex == -1)
                {
                    _Dvt.Text = String.Empty;
                } else
                {
                    _Dvt.Text = cbbox_dvt.SelectedItem.ToString();
                }
            } catch(Exception ex)
            {
                MessageBox.Show("Loi : " + ex.Message);
            }
        }
        // Retext
        private void reText()
        {
            try
            {
                _Mahang.Text = "";
                _Tenhang.Text = "";
                cbbox_dvt.SelectedIndex = -1;
                _Dongia.Text = "";
                _NhaSX.Text = "";
                _Soluong.Text = "";
                cbbox_Loai.SelectedIndex = -1;
                DP_Ngaycapnhat.Text = "";
            } catch(Exception ex)
            {
                MessageBox.Show("Ngu" + ex.Message);
            }
        }
        // lay ID
        private void cbbox_Loai_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(cbbox_Loai.SelectedIndex == -1)
            {
                _Loai.Text = String.Empty;
            } else
            {
                _Loai.Text = cbbox_Loai.SelectedItem.ToString();
            }
        }
    }
}
