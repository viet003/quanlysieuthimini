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
    /// Interaction logic for Form_Loaihanghoa.xaml
    /// </summary>
    public partial class Form_Loaihanghoa : Window
    {
        string ConnectionString = @"Data Source=Lenovo\SQLEXPRESS;Initial Catalog=QL_Shop;Integrated Security=True;";
        //Biến Connection để kết nối CSDL
        SqlConnection Conn = new SqlConnection();
        DataTable DataSource = null;
        private SqlDataAdapter adapter;
        public Form_Loaihanghoa()
        {
            InitializeComponent();
        }
        // button minisize
        private void btn_Minisize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        // button max size
        private void btn_Maxsize_Click(object sender, RoutedEventArgs e)
        {
            if(WindowState == WindowState.Normal)
            {
                WindowState = WindowState.Maximized;
            } else
            {
                WindowState = WindowState.Normal;
            }
        }
        // button close
        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        // drag from
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
        // load form
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            /*try
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
            }*/
            NapDuLieuTuMayChu();
        }
        // button them
        private void btn_Them_Click(object sender, RoutedEventArgs e)
        {
            if(_Tenloai.Text == "")
            {
                MessageBox.Show("Vui lòng điền thông tin : Tên loại", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (Check_Tenloai())
            {
                MessageBox.Show("Loại mặt hàng đã tồn tại", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            String sql = "insert into tbl_Loai(Tenloai) values (N'" + _Tenloai.Text + "');";
            try
            {
                Conn.ConnectionString = ConnectionString;
                Conn.Open();
                SqlCommand cmd = new SqlCommand(sql, Conn);
                if(MessageBox.Show("Bạn có muốn thêm thông tin?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Thêm thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            } catch (Exception ex)
            {
                MessageBox.Show("Loi insert loai : " + ex.Message);
            } finally
            {
                Conn.Close();
            }
            NapDuLieuTuMayChu();
            dgView_Hang.ItemsSource = null;
            reText();
        }
        // btn Sua
        private void btn_Sua_Click(object sender, RoutedEventArgs e)
        {
            SetButtonsState(true);
        }
        // button xoa
        private void btn_Xoa_Click(object sender, RoutedEventArgs e)
        {
            if (Check_TT())
            {
                MessageBox.Show("Vui lòng điền thông tin : Mã loại \nhoặc chọn trường thông tin cần xóa.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            String sql = "delete from tbl_Loai where Maloai = '" +_Maloai.Text+ "';";
            try
            {
                Conn.ConnectionString = ConnectionString;
                Conn.Open();
                SqlCommand cmd = new SqlCommand(sql, Conn);
                if(MessageBox.Show("Bạn có muốn xóa thông tin?" , "Thông báo" , MessageBoxButton.YesNo , MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Xóa thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            } catch(Exception ex)
            {
                MessageBox.Show("Loi delete : " + ex.Message);
            } finally
            {
                Conn.Close();
            }
            NapDuLieuTuMayChu();
            dgView_Hang.ItemsSource = null;
            reText();
        }
        // selection
        private void dgView_Loai_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (dgView_Loai.CurrentItem == null) return;
                DataRowView row = (DataRowView)dgView_Loai.CurrentItem;
                _Maloai.Text = row[0].ToString().Trim();
                _Tenloai.Text = row[1].ToString().Trim();

                Conn.ConnectionString = ConnectionString;
                Conn.Open();
                String sql = "select * from tbl_Hang where Maloai = '" + row[0].ToString().Trim() + "';";
                dgView_Hang.ItemsSource = null;
                if (Conn.State != ConnectionState.Open) return;
                SqlDataAdapter adapter = new SqlDataAdapter(sql, Conn);
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);
                DataSource = dataSet.Tables[0];
                //Thiết lập cho hiển thị lên DataGrid

                dgView_Hang.ItemsSource = DataSource.DefaultView;
            } catch (Exception ex)
            {
                MessageBox.Show("Loi selectionchange dgView_Loai : " + ex.Message);
            } finally
            {
                Conn.Close();
            }
        }
        // cap nhat
        private void btn_Capnhat_Click(object sender, RoutedEventArgs e)
        {
            if (Check_TT())
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if(Check_Tenloai())
            {
                MessageBox.Show("Loại mặt hàng đã tồn tại", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            String sql = "update tbl_Loai set Tenloai = N'" + _Tenloai.Text +"' where Maloai = '" +_Maloai.Text+ "';";
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
            } catch(Exception ex)
            {
                MessageBox.Show("Loi update : " + ex.Message);
            } finally
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
            _Maloai.IsEnabled = Editing;
        }
        // load dgview_Loai
        private void NapDuLieuTuMayChu()
        {
            dgView_Loai.ItemsSource = null;
            try
            {
                Conn.ConnectionString = ConnectionString;
                Conn.Open();
                string SqlStr = "select * from tbl_Loai";
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
            } finally
            {
                Conn.Close();
            }
        }
        // check tt
        private bool Check_TT()
        {
            if(_Maloai.Text == "" || _Tenloai.Text == "")
            {
                return true;
            }
            return false;
        }
        // retext
        private void reText()
        {
            _Maloai.Text = "";
            _Tenloai.Text = "";
        }
        // Check_Loai
        private bool Check_Tenloai()
        {
            String sql = "select * from tbl_Loai where Tenloai = N'" +_Tenloai.Text.Trim()+ "';";
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
            } catch (Exception ex)
            {
                MessageBox.Show("Loi Check_tenloai : " + ex.Message);
            } finally
            {
                reader.Close();
                Conn.Close();
            }
            return false;
        }
    }
}
