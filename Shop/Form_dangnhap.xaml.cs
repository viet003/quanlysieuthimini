using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Shop.Cls_thuvien;


namespace Shop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Form_dangnhap : Window
    {
        // thuoc tinh
        private String MaNV;
        private String Loai;
        //
        string ConnectionString = "";
        //Biến Connection để kết nối CSDL
        SqlConnection Conn = new SqlConnection();
        public Form_dangnhap()
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
        //btn close
        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        // button dang nhap
        private void btn_dangnhap_Click(object sender, RoutedEventArgs e)
        {
            check_login.Visibility = Visibility.Hidden;
            if (check_DN() && _TK.Text != "" && _MK.Password != "")
            {
                try
                {
                    MessageBox.Show("Đănh nhập thành công!", "Thông báo!", MessageBoxButton.OK, MessageBoxImage.Information);
                    Form_Menu m = new Form_Menu();
                    m.MaNV = this.MaNV;
                    switch(Loai)
                    {
                        case "Thu ngân":
                            m.Nhanvien.Visibility = Visibility.Hidden;
                            m.Khohang.Visibility = Visibility.Hidden;
                            m.Nhaphang.Visibility = Visibility.Hidden;
                            m.Thongke.Visibility = Visibility.Hidden;
                            break;

                        case "Quản lý kho":
                            m.Nhanvien.Visibility = Visibility.Collapsed;
                            m.Banhang.Visibility = Visibility.Collapsed;
                            m.Donhang.Visibility = Visibility.Collapsed;
                            m.Thongke.Visibility = Visibility.Collapsed;
                            break;
                    }
                    /* if (Loai.Contains("Nhân viên"))
                    {
                        m.Nhanvien.Visibility = Visibility.Hidden;
                        m.Khohang.Visibility = Visibility.Hidden;
                        m.Nhaphang.Visibility = Visibility.Hidden;
                        m.Thongke.Visibility = Visibility.Hidden;
                    }*/
                    m.Show();
                    this.Close();
                } catch (Exception ex)
                {
                    MessageBox.Show("Loi : " + ex.Message);
                }
            }
            else
            {
                check_login.Visibility = Visibility.Visible;
            }
        }
        // button dang ky
        private void btn_dangnky_Click(object sender, RoutedEventArgs e)
        {
            Form_dangky dk = new Form_dangky();
            this.Hide();
            dk.Show();
        }
        // event loaded
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
        }

        // kiem tra thong tin dang nhap
        public bool check_DN()
        {
            try
            {
                string sql = "Select * from tbl_Taikhoan Where Account = @s1 and Pass = @s2";
                SqlCommand cmd = new SqlCommand(sql, Conn);
                cmd.Parameters.AddWithValue("@s1", _TK.Text);
                cmd.Parameters.AddWithValue("@s2", SHA256Example.SHA256Hash(_MK.Password));
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);
                if (dataSet.Tables[0].Rows.Count > 0)
                {
                    MaNV = dataSet.Tables[0].Rows[0][3].ToString().Trim();
                    Loai = dataSet.Tables[0].Rows[0][4].ToString().Trim();
                    return true;
                }
            } catch(Exception ex)
            {
                MessageBox.Show("Loi : " + ex.Message);
            }
            return false;
        }

    }
}

