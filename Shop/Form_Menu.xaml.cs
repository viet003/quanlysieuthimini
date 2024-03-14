using System;
using System.Collections.Generic;
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
using System.Runtime.InteropServices;
using System.Runtime;
using System.Windows.Interop;
using System.Windows.Threading;
using Shop.Child_view;
using System.Data.SqlClient;
using Shop.Cls_thuvien;

namespace Shop
{
    /// <summary>
    /// Interaction logic for Form_Menu.xaml
    /// </summary>
    public partial class Form_Menu : Window
    {
        public String MaNV;
        private string MaBP;
        string ConnectionString = "";
        //Biến Connection để kết nối CSDL
        SqlConnection Conn = new SqlConnection();
        public Form_Menu()
        {
            InitializeComponent();
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;

            DispatcherTimer timer = new DispatcherTimer();
            //timer.Tick += new EventHandler(UpdateTimer_Tick);
            timer.Start();
        }

        /*private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            Text_time.Text = DateTime.Now.ToString();
        }*/

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

        // Drag form
        private void panel_control_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
        // button close
        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
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
        // Drag form
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
        // button bán hàng
        private void Banhang_Click(object sender, RoutedEventArgs e)
        {
            Form_Banhang form_Banhang = new Form_Banhang();
            form_Banhang.MaNV = this.MaNV;
            this.Hide();
            form_Banhang.ShowDialog();
            this.Show();
        }
        // button Đơn hàng
        private void Donhang_Click(object sender, RoutedEventArgs e)
        {
            Form_Donhang dh = new Form_Donhang();
            this.Hide();
            dh.ShowDialog();
            this.Show();
        }
        // button Nhân viên
        private void Nhanvien_Click(object sender, RoutedEventArgs e)
        {
            Form_Nhanvien nv = new Form_Nhanvien();
            this.Hide();
            nv.ShowDialog();
            this.Show();
        }
        // button Thống kê
        private void Thongke_Click(object sender, RoutedEventArgs e)
        {
            Form_Thongke tk = new Form_Thongke();
            this.Hide();
            tk.ShowDialog();
            this.Show();
        }
        // Button nhập hàng
        private void Nhaphang_Click(object sender, RoutedEventArgs e)
        {
            Form_Nhaphang nhaphang = new Form_Nhaphang();
            this.Hide();
            nhaphang.ShowDialog();
            this.Show();
        }
        // button kho hàng
        private void Khohang_Click(object sender, RoutedEventArgs e)
        {
            Form_Quanlykho qlk = new Form_Quanlykho();
            this.Hide();
            qlk.ShowDialog();
            this.Show();
        }
        // button dang xuất
        private void Dangxuat_Click(object sender, RoutedEventArgs e)
        {
            Form_dangnhap dn = new Form_dangnhap();
            dn.Show();
            this.Close();
        }
        // win load
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //Mở kết nối
                ConnectionString = @"Data Source=Lenovo\SQLEXPRESS;Initial Catalog=QL_Shop;Integrated Security=True;";
                Conn.ConnectionString = ConnectionString;
                Conn.Open();
                //if (Conn.State == ConnectionState.Open)
                //    MessageBox.Show("Mở kết nối thành công.");
                //else
                //    MessageBox.Show("Lỗi khi mở kết nối");

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi mở kết nối");
            }
            Laythongtin();
            Layvitri();
        }
        // lay thong tin
        private void Laythongtin()
        {
            String sql = "select * from tbl_Nhanvien where MaNV = '" +MaNV+ "';";
            SqlDataReader reader = null;
            try
            {
                if (Conn.State != System.Data.ConnectionState.Open) return;
                SqlCommand cmd = new SqlCommand(sql, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    _MaNV.Text = "Mã nhân viên : " + reader["MaNV"].ToString().Trim();
                    _Hoten.Text = "Họ tên : " + RSA.Decryption(reader["Hoten"].ToString().Trim());
                    _Ngaysinh.Text = "Ngày sinh : " + RSA.Decryption(reader["Ngaysinh"].ToString().Trim());
                    _Diachi.Text = "Địa chỉ : " + RSA.Decryption(reader["Diachi"].ToString().Trim());
                    _SDT.Text = "Số điện thoại : " + RSA.Decryption(reader["SDT"].ToString().Trim());
                    _Trangthai.Text = "Trang thái : " + RSA.Decryption(reader["Trangthai"].ToString().Trim());
                    _Ghichu.Text = "Ghi chú : Nhìn đẹp trai ";
                    MaBP = reader["MaBP"].ToString().Trim();
                }
            } catch (Exception ex)
            {
                MessageBox.Show("Loi : " +  ex.Message);
            } finally
            {
                reader.Close();
            }
        }

        // lấy tên bộ phận - vị trí
        private void Layvitri()
        {
            String sql = "select TenBophan from tbl_Bophan where MaBP = '" + MaBP + "';";
            SqlDataReader reader = null;
            try
            {
                if (Conn.State != System.Data.ConnectionState.Open) return;
                SqlCommand cmd = new SqlCommand(sql, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    _Chucvu.Text = "Chức vụ: " + reader[0].ToString().Trim();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Loi : " + ex.Message);
            }
            finally
            {
                reader.Close();
            }
        }
        private void CartesianChart_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
