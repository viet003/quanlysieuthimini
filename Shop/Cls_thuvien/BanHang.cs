using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Cls_thuvien
{
    internal class BanHang
    {
        String _MaHD;
        String _MaHang;
        String _Soluong;
        String _Dongia;
        String _Thanhtien;

        public BanHang() { }
        public BanHang(string maHD, string maHang, string soluong, string dongia, string thanhtien)
        {
            this._MaHD = maHD;
            this._MaHang = maHang;
            this._Soluong = soluong;
            this._Dongia = dongia;
            this._Thanhtien = thanhtien;
        }

        public String MaHD
        {
            get { return _MaHD; }
            set { _MaHD = value; }
        }
        public String MaHang
        {
            get { return _MaHang; }
            set { _MaHang = value; }
        }
        public String Soluong
        {
            get { return _Soluong; }
            set { _Soluong = value; }
        }
        public String Dongia
        {
            get { return _Dongia; }
            set
            {
                _Dongia = value;
            }
        }
        public String Thanhtien
        {
            get { return _Thanhtien; }
            set { _Thanhtien = value; }
        }
    }
}
