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
    /// Interaction logic for Form_Thongke.xaml
    /// </summary>
    public partial class Form_Thongke : Window
    {
        string ConnectionString = @"Data Source=Lenovo\SQLEXPRESS;Initial Catalog=QL_Shop;Integrated Security=True;";
        public Form_Thongke()
        {
            InitializeComponent();
        }
        //drag form
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
        // button minosize
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

        private void btn_Search_Click(object sender, RoutedEventArgs e)
        {
            string sql = "SELECT * FROM tbl_BanHang where MaHD = '" + label_Search.Text + "'";
            SqlConnection connection = new SqlConnection(ConnectionString);
            SqlDataAdapter dataadapter = new SqlDataAdapter(sql, connection);
            DataTable ds = new DataTable();
            connection.Open();
            dataadapter.Fill(ds);
            dgView.ItemsSource = ds.DefaultView;
            connection.Close();
        }
        public void loadComboThang()
        {
            string[] list = { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12" };
            DataTable ds = new DataTable();
            foreach (string film in list)
            {
                Console.WriteLine(film);
                comboThang.Items.Add(film);
            }
        }

        public void load()
        {
            try
            {
                string sql = "select * from tbl_Hang";
                SqlConnection connection = new SqlConnection(ConnectionString);
                SqlDataAdapter dataadapter = new SqlDataAdapter(sql, connection);
                DataTable ds = new DataTable();
                connection.Open();
                dataadapter.Fill(ds);
                dgView.ItemsSource = ds.DefaultView;
                connection.Close();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Loi load table : " + ex.Message);
            }
        }

        public void loadComboNam()
        {
            string sql = "SELECT DISTINCT YEAR(Ngaylap) as 'Year' FROM tbl_Hoadon inner join tbl_Banhang on tbl_Banhang.MaHD = tbl_Hoadon.MaHD";
            SqlConnection con = new SqlConnection(ConnectionString);
            DataSet ds = new DataSet();
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(ds);
                comboNam.DisplayMemberPath = "Year"; //DisplayMember
                comboNam.SelectedValuePath = "Year";
                comboNam.ItemsSource = ds.Tables[0].DefaultView;   //DataSource
            }
            catch (Exception e)
            {
            }
        }

        public void loadConboMahang()
        {
            String sql = "select MaHang from tbl_Hang";
            SqlConnection conn = new SqlConnection(ConnectionString);
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    comboMahang.Items.Add(reader["MaHang"].ToString().Trim());
                }
            } catch(Exception ex)
            {
                MessageBox.Show("Loi : " + ex.Message);
            } finally
            {
                reader.Close();
            }
        }

        public void load_dgview_nhap()
        {
            try
            {
                string sql = "select * from tbl_Nhaphang where MaHang = '" +_Mahang.Text+ "'";
                SqlConnection connection = new SqlConnection(ConnectionString);
                SqlDataAdapter dataadapter = new SqlDataAdapter(sql, connection);
                DataTable ds = new DataTable();
                connection.Open();
                dataadapter.Fill(ds);
                dgView_Donnhap.ItemsSource = ds.DefaultView;
                connection.Close();
            } catch(Exception ex)
            {
                MessageBox.Show("Loi load dg_view_nhap :" + ex.Message);
            }
        }
        public void load_dgview_ban()
        {
            try
            {
                string sql = "SELECT bh.MaHD , bh.MaHang, bh.Soluong , bh.Dongia , bh.Thanhtien , hd.Ngaylap from tbl_Banhang as bh inner join tbl_Hoadon as hd on bh.MaHD = hd.MaHD  where bh.MaHang = '" +_Mahang.Text+ "';";
                SqlConnection connection = new SqlConnection(ConnectionString);
                SqlDataAdapter dataadapter = new SqlDataAdapter(sql, connection);
                DataTable ds = new DataTable();
                connection.Open();
                dataadapter.Fill(ds);
                dgView_Donban.ItemsSource = ds.DefaultView;
                connection.Close();
            } catch (Exception ex)
            {
                MessageBox.Show("LOi load_dgview_ban :" + ex.Message);
            }
        }
        List<BanHang> getData_Banhang()
        {
            List<BanHang> list = new List<BanHang>();

            using (SqlConnection cn = new SqlConnection(ConnectionString))
            {
                cn.Open();
                SqlCommand sqlCommand = new SqlCommand("SELECT * FROM tbl_Banhang inner join tbl_HoaDon on tbl_Banhang.MaHD = tbl_Hoadon.MaHD where Month(Ngaylap) ='" + comboThang.SelectedValue + "' and year(Ngaylap) = '" + comboNam.SelectedValue + "'", cn);
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    var banHang = new BanHang();
                    banHang.MaHD = (string)reader["MaHD"];
                    banHang.MaHang = (string)reader["MaHang"];
                    banHang.Soluong = (string)reader["SoLuong"];
                    banHang.Dongia = (string)reader["DonGia"];
                    banHang.Thanhtien = (string)reader["ThanhTien"];
                    list.Add(banHang);
                }
                cn.Close();
            }

            return list;
        }
        private long tongnhap()
        {
            String sql = "select Giatri from tbl_Nhaphang where MaHang = '" +_Mahang.Text+ "' and Month(NgayNhap) = '" +comboThang.SelectedValue+ "' and year(NgayNhap) = '" +comboNam.SelectedValue+ "'";
            SqlConnection Conn = new SqlConnection(ConnectionString);
            SqlDataReader reader = null;
            long tongnhap = 0;
            try
            {
                Conn.Open();
                SqlCommand cmd = new SqlCommand(sql, Conn);
                reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    tongnhap += Convert.ToInt32(reader["Giatri"].ToString().Trim());
                }
            } catch (Exception ex)
            {
                MessageBox.Show("Loi tinh thanhtoannhap : " + ex.Message);
            } finally
            {
                reader.Close();
            }
            return tongnhap;
        }
        private long tongban()
        {
            long tongban = 0;
            String sql = "SELECT * FROM tbl_Banhang inner join tbl_HoaDon on tbl_Banhang.MaHD = tbl_Hoadon.MaHD where Month(Ngaylap) ='" + comboThang.SelectedValue + "' and year(Ngaylap) = '" + comboNam.SelectedValue + "' and  tbl_Banhang.MaHang = '" + _Mahang.Text+ "'";
            SqlConnection Conn = new SqlConnection(ConnectionString);
            SqlDataReader reader = null;
            try
            {
                Conn.Open();
                SqlCommand cmd = new SqlCommand(sql, Conn);
                reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    tongban += Convert.ToInt32(reader["Thanhtien"].ToString().Trim());
                }
            } catch (Exception ex )
            {
                MessageBox.Show("Loi tinh tongban : " + ex.Message);
            } finally
            {
                reader.Close();
            }
            return tongban;
        }
        private void btn_thongke_Click(object sender, RoutedEventArgs e)
        {
            if (check_Date())
            {
                MessageBox.Show("Vui lòng không bỏ trống tháng và năm ", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (_Mahang.Text == "")
            {
                var list = getData_Banhang();

                int tong = 0;
                foreach (BanHang b in list)
                {
                    tong += Convert.ToInt32(b.Thanhtien);
                }
                _Tongnhap.Text = ("Tổng số tiền nhập hàng ở" + comboThang.SelectedValue + "/" + comboNam.SelectedValue + " của mọi mặt hàng là: " + getData_Nhaphang().ToString() + "VND");
                _Tongban.Text = ("Tổng tiền thu nhập được ở " + comboThang.SelectedValue + "/" + comboNam.SelectedValue + " của mọi mặt hàng là: " + tong + " VND");
            } else
            {
                _Tongban.Text = tongban().ToString() + "     VND";
                _Tongnhap.Text = tongnhap().ToString() + "     VND";
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            load();
            loadComboThang();
            loadComboNam();
            loadConboMahang();
            //
            cbbox_Search.Items.Add("Mã hàng");
        }
        //


        private void _Mahang_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (_Mahang.Text == "")
                {
                    dgView_Donban.ItemsSource = null;
                    dgView_Donnhap.ItemsSource = null;
                }
                else
                {
                    load_dgview_ban();
                    load_dgview_nhap();
                }
            } catch(Exception ex)
            {

            }
        }

        private void comboMahang_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (comboMahang.SelectedIndex == -1)
            {
                _Mahang.Text = String.Empty;
            }
            else
            {
                _Mahang.Text = comboMahang.SelectedItem.ToString();
            }
        }

        private bool check_Date()
        {
            if(comboNam.Text == "" || comboThang.Text == "")
            {
                return true;
            }
            return false;
        }

        private long getData_Nhaphang()
        {
            long tongnhap = 0;
            String sql = "SELECT * FROM tbl_Nhaphang where Month(NgayNhap) ='" + comboThang.SelectedValue + "' and year(NgayNhap) = '" + comboNam.SelectedValue + "'";
            SqlConnection Conn = new SqlConnection(ConnectionString);
            SqlDataReader reader = null;
            try
            {
                Conn.Open();
                SqlCommand cmd = new SqlCommand(sql, Conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    tongnhap += Convert.ToInt32(reader["Giatri"].ToString().Trim());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Loi tinh tổng nhập : " + ex.Message);
            }
            finally
            {
                reader.Close();
            }
            return tongnhap;
        }

        private void dgView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (dgView.CurrentItem == null) return;
                DataRowView row = (DataRowView)dgView.CurrentItem;
                _Mahang.Text = row[0].ToString().Trim();
            } catch(Exception ex)
            {
                MessageBox.Show("LOi : " + ex.Message);
            }
        }
    }
}
