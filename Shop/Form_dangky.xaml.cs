using Shop.Cls_thuvien;
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
using System.Windows.Shapes;

namespace Shop
{
    /// <summary>
    /// Interaction logic for Form_dangky.xaml
    /// </summary>
    public partial class Form_dangky : Window
    {

        string ConnectionString = "";
        //Biến Connection để kết nối CSDL
        SqlConnection Conn = new SqlConnection();

        public Form_dangky()
        {
            InitializeComponent();
        }
        // Drag form
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }    
        }
        // button minisize
        private void btn_Minisize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        // button đóng
        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            Form_dangnhap dn = new Form_dangnhap();
            dn.Show();
            this.Close();
        }
        // button đăng ký
        private void btn_dangky_Click(object sender, RoutedEventArgs e)
        {
            check_login.Visibility = Visibility.Hidden;
            if(!check_MK(label_MK.Password,label_reMK.Password) || !check_TK(label_TK.Text) || !check_TK(label_Name.Text))
            {
                check_login.Text = "Kiểm tra lại thông tin đang ký !";
                check_login.Visibility = Visibility.Visible;
            } else
            {
                 if(Check_exists())
                 {
                    check_login.Visibility = Visibility.Hidden;
                    // thêm thông tin vào bảng Account
                    String sqlStr = "Insert Into tblUsers(Account,Pass,UserName)values(" +
                       "N'" + label_TK.Text + "'," +
                       "N'" + Hash_MD5.EncodeMD5(label_MK.Password) + "'," +
                       "N'" + label_Name.Text + "')";
                    SqlCommand cmd = new SqlCommand(sqlStr, Conn);
                    cmd.ExecuteNonQuery();
                    // đưa ra thông báo
                    check_login.Text = "*Đăng ký thành công , vui lòng trở lại trang đăng nhập";
                    check_login.Foreground = Brushes.SkyBlue;
                    check_login.Visibility = Visibility.Visible;
                 } else
                 {
                    check_login.Text = "Tài khoản đã tồn tại!";
                    check_login.Visibility= Visibility.Visible;
                 }
            }
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

        // kiem tra thong tin tai khoan

        public static bool check_TK( String input)
        {
            String check = @"!@#$%^&*()_-+=/?.>,<;:' ";
            foreach(var i in input)
            {
                foreach (var j in check)
                {
                    if (j.Equals(i)) return false;
                }
            }
            return true;
        }

        // kiem tra mat khau giong nhau?

        public static bool check_MK(String input1, String input2)
        {
            if (!input1.Equals("") && !input2.Equals(""))
            {
                if (input1.Equals(input2)) return true;
            }
            return false;
        }
        // kiem tra tai khoan da ton tai tren csdl chua
        public bool Check_exists()
        {
            string sql = "Select * from tblUsers Where (Account='" +
                label_TK.Text + "')";
            SqlDataAdapter adapter = new SqlDataAdapter(sql, Conn);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet);

            if (dataSet.Tables[0].Rows.Count == 0)
            {
                return true;
            }
            return false;
        }
    }
}
