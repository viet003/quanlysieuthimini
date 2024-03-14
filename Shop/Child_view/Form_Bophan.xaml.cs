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
    /// Interaction logic for Form_Bophan.xaml
    /// </summary>
    public partial class Form_Bophan : Window
    {
        string ConnectionString = @"Data Source=Lenovo\SQLEXPRESS;Initial Catalog=QL_Shop;Integrated Security=True;";
        //Biến Connection để kết nối CSDL
        SqlConnection Conn = new SqlConnection();
        DataTable DataSource = null;
        private SqlDataAdapter adapter;

        public Form_Bophan()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            NapDuLieuTuMayChu();
        }

        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

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

        private void btn_Minisize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void btn_Them_Click(object sender, RoutedEventArgs e)
        {
            if (_TenBP.Text == "")
            {
                MessageBox.Show("Vui lòng điền thông tin : Tên loại", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if(Check_MaBP())
            {
                MessageBox.Show("Mã bộ phận đã tồn tại", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (Check_TenBP())
            {
                MessageBox.Show("Tên bộ phận đã tồn tại", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            String sql = "insert into tbl_Bophan(MaBP,TenBophan) values (N'" + _MaBP.Text + "',N'" + _TenBP.Text + "');";
            try
            {
                Conn.ConnectionString = ConnectionString;
                Conn.Open();
                SqlCommand cmd = new SqlCommand(sql, Conn);
                if (MessageBox.Show("Bạn có muốn thêm thông tin?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Thêm thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Loi insert  : " + ex.Message);
            }
            finally
            {
                Conn.Close();
            }
            NapDuLieuTuMayChu();
            dgView_Hang.ItemsSource = null;
            reText();
        }

        private void btn_Sua_Click(object sender, RoutedEventArgs e)
        {
            SetButtonsState(true);
        }

        private void btn_Xoa_Click(object sender, RoutedEventArgs e)
        {
            if (Check_TT())
            {
                MessageBox.Show("Vui lòng điền thông tin : Mã loại \nhoặc chọn trường thông tin cần xóa.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            String sql = "delete from tbl_Bophan where MaBP = '" + _MaBP.Text + "';";
            try
            {
                Conn.ConnectionString = ConnectionString;
                Conn.Open();
                SqlCommand cmd = new SqlCommand(sql, Conn);
                if (MessageBox.Show("Bạn có muốn xóa thông tin?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Xóa thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Loi delete : " + ex.Message);
            }
            finally
            {
                Conn.Close();
            }
            NapDuLieuTuMayChu();
            dgView_Hang.ItemsSource = null;
            reText();
        }

        private void btn_Capnhat_Click(object sender, RoutedEventArgs e)
        {
            if (Check_TT())
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (Check_TenBP())
            {
                MessageBox.Show("Tên bộ phận  đã tồn tại", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            String sql = "update tbl_Bophan set TenBophan = N'" + _TenBP.Text + "' where MaBP = '" + _MaBP.Text + "';";
            try
            {
                Conn.ConnectionString = ConnectionString;
                Conn.Open();
                SqlCommand cmd = new SqlCommand(sql, Conn);
                if (MessageBox.Show("Bạn có muốn sửa thông tin?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Sửa thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Loi update : " + ex.Message);
            }
            finally
            {
                Conn.Close();
            }
            NapDuLieuTuMayChu();
            dgView_Hang.ItemsSource = null;
            reText();
        }

        private void btn_Thoat_Click(object sender, RoutedEventArgs e)
        {
            SetButtonsState(false);
        }

        private void dgView_Loai_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (dgView_Loai.CurrentItem == null) return;
                DataRowView row = (DataRowView)dgView_Loai.CurrentItem;
                _MaBP.Text = row[0].ToString().Trim();
                _TenBP.Text = row[1].ToString().Trim();

                //
                Conn.ConnectionString = ConnectionString;
                Conn.Open();
                String sql = "select * from tbl_Nhanvien where MaBP = '" + row[0].ToString().Trim() + "';";
                dgView_Hang.ItemsSource = null;
                if (Conn.State != ConnectionState.Open) return;

                SqlDataAdapter adapter = new SqlDataAdapter(sql, Conn);
                DataSet dataSet = new DataSet(); // lưu trữ dữ liệu từ cơ sổ dữ liệu 
                adapter.Fill(dataSet); // đổ dữ liệu và dataset
                DataSource = dataSet.Tables[0]; // lấy dữ liệu từ bảng table 0 gán cho bảng dl
                                                //Thiết lập cho hiển thị lên DataGrid

                // Thay đổi dữ liệu trong DataTable theo nhu cầu của bạn
                foreach (DataRow rows in DataSource.Rows)
                {
                    // Ví dụ: thay đổi tên nhân viên
                    rows[1] = RSA.Decryption(rows[1].ToString().Trim());
                    rows[2] = RSA.Decryption(rows[2].ToString().Trim());
                    rows[3] = RSA.Decryption(rows[3].ToString().Trim());
                    rows[4] = RSA.Decryption(rows[4].ToString().Trim());
                    rows[6] = RSA.Decryption(rows[6].ToString().Trim());
                }

                dgView_Hang.ItemsSource = DataSource.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Loi selectionchange dgView_Loai : " + ex.Message);
            }
            finally
            {
                Conn.Close();
            }
        }

        private void NapDuLieuTuMayChu()
        {
            dgView_Loai.ItemsSource = null;
            try
            {
                Conn.ConnectionString = ConnectionString;
                Conn.Open();
                string SqlStr = "select * from tbl_Bophan";
                SqlDataAdapter adapter = new SqlDataAdapter(SqlStr, Conn);
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);
                DataSource = dataSet.Tables[0];
                //Thiết lập cho hiển thị lên DataGrid

                dgView_Loai.ItemsSource = DataSource.DefaultView;
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

        private bool Check_TT()
        {
            if (_MaBP.Text == "" || _TenBP.Text == "")
            {
                return true;
            }
            return false;
        }
        // set trang thai nut
        private void SetButtonsState(bool Editing)
        {
            //True -> Dang o che do soan thao
            btn_Capnhat.Visibility = Editing ? Visibility.Visible : Visibility.Hidden;
            btn_Thoat.Visibility = Editing ? Visibility.Visible : Visibility.Hidden;

            Editing = !Editing;
            btn_Them.Visibility = Editing ? Visibility.Visible : Visibility.Hidden;
            btn_Sua.Visibility = Editing ? Visibility.Visible : Visibility.Hidden;
            btn_Xoa.Visibility = Editing ? Visibility.Visible : Visibility.Hidden;
            _MaBP.IsEnabled = Editing;
        }
        // retext
        private void reText()
        {
            _MaBP.Text = "";
            _TenBP.Text = "";
        }
        // Check tên bộ phận
        private bool Check_TenBP()
        {
            String sql = "select * from tbl_Bophan where TenBophan = N'" + _TenBP.Text.Trim() + "';";
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
                MessageBox.Show("Loi Check_tenBophan : " + ex.Message);
            }
            finally
            {
                reader.Close();
                Conn.Close();
            }
            return false;
        }
        // check mã bp
        private bool Check_MaBP()
        {
            String sql = "select * from tbl_Bophan where MaBP = N'" + _MaBP.Text.Trim() + "';";
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
                MessageBox.Show("Loi Check_tenBophan : " + ex.Message);
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

