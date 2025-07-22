using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using OGCCinema.Models;

namespace OGCCinema.Data
{
    public partial class OgccinemaContext : DbContext
    {
        public OgccinemaContext(DbContextOptions<OgccinemaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Chucvu> Chucvus { get; set; }
        public virtual DbSet<CthdMonan> CthdMonans { get; set; }
        public virtual DbSet<Dinhdangphim> Dinhdangphims { get; set; }
        public virtual DbSet<Dotuoi> Dotuois { get; set; }
        public virtual DbSet<Ghe> Ghes { get; set; }
        public virtual DbSet<HdMonan> HdMonans { get; set; }
        public virtual DbSet<HdVe> HdVes { get; set; }
        public virtual DbSet<Khachhang> Khachhangs { get; set; }
        public virtual DbSet<Kho> Khos { get; set; }
        public virtual DbSet<Khuyenmai> Khuyenmais { get; set; }
        public virtual DbSet<Lichchieu> Lichchieus { get; set; }
        public virtual DbSet<Loaimonan> Loaimonans { get; set; }
        public virtual DbSet<Loaiphong> Loaiphongs { get; set; }
        public virtual DbSet<LogNhanvien> LogNhanviens { get; set; }
        public virtual DbSet<LuuTruTamTknhanVienDangDangNhap> LuuTruTamTknhanVienDangDangNhaps { get; set; }
        public virtual DbSet<Monan> Monans { get; set; }
        public virtual DbSet<Nhanvien> Nhanviens { get; set; }
        public virtual DbSet<OtpXacnhan> OtpXacnhans { get; set; }
        public virtual DbSet<Phim> Phims { get; set; }
        public virtual DbSet<Phongchieu> Phongchieus { get; set; }
        public virtual DbSet<Theloaiphim> Theloaiphims { get; set; }
        public virtual DbSet<Tkkhachhang> Tkkhachhangs { get; set; }
        public virtual DbSet<Tknhanvien> Tknhanviens { get; set; }
        public virtual DbSet<Trangthaighe> Trangthaighes { get; set; }
        public virtual DbSet<Ve> Ves { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Chucvu>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__CHUCVU__3214EC27E03D7E1F");
                entity.ToTable("CHUCVU", tb => tb.HasTrigger("trg_Log_CHUCVU_InsertUpdateDelete"));
                entity.HasIndex(e => e.TenChucVu, "UQ__CHUCVU__A7E2123E7EC849AE").IsUnique();
                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.TenChucVu).HasMaxLength(100);
            });

            modelBuilder.Entity<CthdMonan>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__CTHD_MON__3214EC27130BDAD7");
                entity.ToTable("CTHD_MONAN", tb =>
                {
                    tb.HasTrigger("trg_RestoreStock_AfterDelete");
                    tb.HasTrigger("trg_TruKhoSauKhiBan");
                });
                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.Gia).HasColumnType("decimal(8, 0)");
                entity.Property(e => e.IdhoaDon).HasColumnName("IDHoaDon");
                entity.Property(e => e.IdmonAn).HasColumnName("IDMonAn");
                entity.Property(e => e.NgayMua).HasDefaultValueSql("(getdate())").HasColumnType("datetime");
                entity.Property(e => e.TenMonAn).HasMaxLength(100);
                entity.Property(e => e.TrangThai).HasMaxLength(20);
                entity.HasOne(d => d.IdhoaDonNavigation).WithMany(p => p.CthdMonans)
                    .HasForeignKey(d => d.IdhoaDon)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CTHD_MONA__IDHoa__17F790F9");
                entity.HasOne(d => d.IdmonAnNavigation).WithMany(p => p.CthdMonans)
                    .HasForeignKey(d => d.IdmonAn)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_CTHD_MONAN_MONAN");
            });

            modelBuilder.Entity<Dinhdangphim>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__DINHDANG__3214EC279663462B");
                entity.ToTable("DINHDANGPHIM", tb => tb.HasTrigger("trg_Log_DINHDANGPHIM_InsertUpdateDelete"));
                entity.HasIndex(e => e.TenDinhDang, "UQ__DINHDANG__74BC616FCB01CB29").IsUnique();
                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.TenDinhDang).HasMaxLength(20);
            });

            modelBuilder.Entity<Dotuoi>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__DOTUOI__3214EC27E701717B");
                entity.ToTable("DOTUOI");
                entity.Property(e => e.Id).HasMaxLength(10).HasColumnName("ID");
                entity.Property(e => e.TenDoTuoi).HasMaxLength(100);
            });

            modelBuilder.Entity<Ghe>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__GHE__3214EC2747E5EBA4");
                entity.ToTable("GHE");
                entity.HasIndex(e => new { e.MaGhe, e.Idphong }, "UQ__GHE__04CF776FCCBDFDAC").IsUnique();
                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.Idphong).HasColumnName("IDPhong");
                entity.Property(e => e.MaGhe).HasMaxLength(10);
                entity.Property(e => e.TrangThai).HasDefaultValue(0);
                entity.HasOne(d => d.IdphongNavigation).WithMany(p => p.Ghes)
                    .HasForeignKey(d => d.Idphong)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__GHE__IDPhong__6C190EBB");
            });

            modelBuilder.Entity<HdMonan>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__HD_MONAN__3214EC2763739CA6");
                entity.ToTable("HD_MONAN");
                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.IdkhachHang).HasColumnName("IDKhachHang");
                entity.Property(e => e.IdnhanVien).HasColumnName("IDNhanVien");
                entity.Property(e => e.MaKm).HasMaxLength(20).HasColumnName("MaKM");
                entity.Property(e => e.NgayMua).HasDefaultValueSql("(getdate())").HasColumnType("datetime");
                entity.Property(e => e.SdtKh).HasMaxLength(15).HasColumnName("SdtKH");
                entity.Property(e => e.SdtNv).HasMaxLength(15).HasColumnName("SdtNV");
                entity.Property(e => e.TenKh).HasMaxLength(50).HasDefaultValue("Đang cập nhật").HasColumnName("TenKH");
                entity.Property(e => e.TenNv).HasMaxLength(50).HasDefaultValue("Đang cập nhật").HasColumnName("TenNV");
                entity.Property(e => e.TongTien).HasColumnType("decimal(10, 0)");
                entity.HasOne(d => d.IdkhachHangNavigation).WithMany(p => p.HdMonans)
                    .HasForeignKey(d => d.IdkhachHang)
                    .HasConstraintName("FK__HD_MONAN__IDKhac__123EB7A3");
                entity.HasOne(d => d.IdnhanVienNavigation).WithMany(p => p.HdMonans)
                    .HasForeignKey(d => d.IdnhanVien)
                    .HasConstraintName("FK__HD_MONAN__IDNhan__1332DBDC");
                entity.HasOne(d => d.MaKmNavigation).WithMany(p => p.HdMonans)
                    .HasForeignKey(d => d.MaKm)
                    .HasConstraintName("FK__HD_MONAN__MaKM__114A936A");
            });

            modelBuilder.Entity<HdVe>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__HD_VE__3214EC27B0C7AF31");
                entity.ToTable("HD_VE");
                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.IdkhachHang).HasColumnName("IDKhachHang");
                entity.Property(e => e.IdnhanVien).HasColumnName("IDNhanVien");
                entity.Property(e => e.MaKm).HasMaxLength(20).HasColumnName("MaKM");
                entity.Property(e => e.NgayDat).HasDefaultValueSql("(getdate())").HasColumnType("datetime");
                entity.Property(e => e.SdtKh).HasMaxLength(15).HasColumnName("SdtKH");
                entity.Property(e => e.SdtNv).HasMaxLength(15).HasColumnName("SdtNV");
                entity.Property(e => e.TenKh).HasMaxLength(50).HasDefaultValue("Đang cập nhật").HasColumnName("TenKH");
                entity.Property(e => e.TenNv).HasMaxLength(50).HasDefaultValue("Đang cập nhật").HasColumnName("TenNV");
                entity.Property(e => e.TongTien).HasColumnType("decimal(10, 2)");
                entity.HasOne(d => d.IdkhachHangNavigation).WithMany(p => p.HdVes)
                    .HasForeignKey(d => d.IdkhachHang)
                    .HasConstraintName("FK__HD_VE__IDKhachHa__7A672E12");
                entity.HasOne(d => d.IdnhanVienNavigation).WithMany(p => p.HdVes)
                    .HasForeignKey(d => d.IdnhanVien)
                    .HasConstraintName("FK__HD_VE__IDNhanVie__7B5B524B");
                entity.HasOne(d => d.MaKmNavigation).WithMany(p => p.HdVes)
                    .HasForeignKey(d => d.MaKm)
                    .HasConstraintName("FK__HD_VE__MaKM__797309D9");
            });

            modelBuilder.Entity<Khachhang>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__KHACHHAN__3214EC278CB0F25C");
                entity.ToTable("KHACHHANG", tb => tb.HasTrigger("trg_BeforeDelete_KhachHang"));
                entity.HasIndex(e => e.Username, "UQ_UsernameKH").IsUnique();
                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.DiaChi).HasMaxLength(255);
                entity.Property(e => e.DiemThuong).HasDefaultValue(0);
                entity.Property(e => e.Email).HasMaxLength(100);
                entity.Property(e => e.HoTen).HasMaxLength(100);
                entity.Property(e => e.Sdt).HasMaxLength(15).HasColumnName("SDT");
                entity.Property(e => e.Username).HasMaxLength(50);
                entity.HasOne(d => d.UsernameNavigation).WithOne(p => p.Khachhang)
                    .HasForeignKey<Khachhang>(d => d.Username)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__KHACHHANG__Usern__3E52440B");
            });

            modelBuilder.Entity<Kho>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__KHO__3214EC2793A0B079");
                entity.ToTable("KHO", tb => tb.HasTrigger("trg_Log_KHO_InsertUpdateDelete"));
                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.IdmonAn).HasColumnName("IDMonAn");
                entity.Property(e => e.NgayCapNhat).HasDefaultValueSql("(getdate())").HasColumnType("datetime");
                entity.Property(e => e.TenMonAn).HasMaxLength(50);
                entity.HasOne(d => d.IdmonAnNavigation).WithMany(p => p.Khos)
                    .HasForeignKey(d => d.IdmonAn)
                    .HasConstraintName("FK__KHO__IDMonAn__7E02B4CC");
            });

            modelBuilder.Entity<Khuyenmai>(entity =>
            {
                entity.HasKey(e => e.MaKm).HasName("PK__KHUYENMA__2725CF1503206C58");
                entity.ToTable("KHUYENMAI", tb => tb.HasTrigger("trg_Log_KHUYENMAI_InsertUpdateDelete"));
                entity.Property(e => e.MaKm).HasMaxLength(20).HasColumnName("MaKM");
                entity.Property(e => e.LoaiApDung).HasMaxLength(50);
                entity.Property(e => e.MoTa).HasMaxLength(255);
                entity.Property(e => e.TenKm).HasMaxLength(100).HasColumnName("TenKM");
            });

            modelBuilder.Entity<Lichchieu>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__LICHCHIE__3214EC2786B3608C");
                entity.ToTable("LICHCHIEU", tb =>
                {
                    tb.HasTrigger("trg_BeforeDelete_LichChieu_VE");
                    tb.HasTrigger("trg_Log_LICHCHIEU_InsertUpdateDelete");
                });
                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.DiaDiem).HasMaxLength(255);
                entity.Property(e => e.GiaVe).HasColumnType("decimal(6, 0)");
                entity.Property(e => e.Idphim).HasColumnName("IDPhim");
                entity.Property(e => e.Idphong).HasColumnName("IDPhong");
                entity.Property(e => e.NgayGio).HasColumnType("datetime");
                entity.Property(e => e.TenPhim).HasMaxLength(100);
                entity.Property(e => e.TenPhong).HasMaxLength(50);
                entity.HasOne(d => d.IdphimNavigation).WithMany(p => p.Lichchieus)
                    .HasForeignKey(d => d.Idphim)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__LICHCHIEU__IDPhi__6FE99F9F");
                entity.HasOne(d => d.IdphongNavigation).WithMany(p => p.Lichchieus)
                    .HasForeignKey(d => d.Idphong)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__LICHCHIEU__IDPho__70DDC3D8");
            });

            modelBuilder.Entity<Loaimonan>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__LOAIMONA__3214EC27AB9E7145");
                entity.ToTable("LOAIMONAN", tb => tb.HasTrigger("trg_Log_LOAIMONAN_InsertUpdateDelete"));
                entity.HasIndex(e => e.TenLoai, "UQ__LOAIMONA__E29B1042F7E58E05").IsUnique();
                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.TenLoai).HasMaxLength(100);
            });

            modelBuilder.Entity<Loaiphong>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__LOAIPHON__3214EC2712BBD233");
                entity.ToTable("LOAIPHONG", tb => tb.HasTrigger("trg_Log_LOAIPHONG_InsertUpdateDelete"));
                entity.HasIndex(e => e.TenLoaiPhong, "UQ__LOAIPHON__2508179CFF275F7D").IsUnique();
                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.SoCot).HasDefaultValue(10);
                entity.Property(e => e.SoHang).HasDefaultValue(5);
                entity.Property(e => e.TenLoaiPhong).HasMaxLength(20);
            });

            modelBuilder.Entity<LogNhanvien>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__LOG_NHAN__3214EC27853D662E");
                entity.ToTable("LOG_NHANVIEN");
                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.ChucNang).HasMaxLength(100);
                entity.Property(e => e.IdchucVu).HasColumnName("IDChucVu");
                entity.Property(e => e.ThaoTac).HasMaxLength(200);
                entity.Property(e => e.ThoiGian).HasDefaultValueSql("(getdate())").HasColumnType("datetime");
                entity.Property(e => e.Username).HasMaxLength(50);
                entity.HasOne(d => d.IdchucVuNavigation).WithMany(p => p.LogNhanviens)
                    .HasForeignKey(d => d.IdchucVu)
                    .HasConstraintName("FK__LOG_NHANV__IDChu__4924D839");
                entity.HasOne(d => d.UsernameNavigation).WithMany(p => p.LogNhanviens)
                    .HasForeignKey(d => d.Username)
                    .HasConstraintName("FK__LOG_NHANV__Usern__4830B400");
            });

            modelBuilder.Entity<LuuTruTamTknhanVienDangDangNhap>(entity =>
            {
                entity.HasKey(e => e.Username).HasName("PK__LuuTruTa__536C85E5822ED1B6");
                entity.ToTable("LuuTruTam_TKNhanVien_DangDangNhap");
                entity.Property(e => e.Username).HasMaxLength(50);
            });

            modelBuilder.Entity<Monan>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__MONAN__3214EC2762DE966C");
                entity.ToTable("MONAN", tb =>
                {
                    tb.HasTrigger("trg_BeforeDelete_MonAn_CTHD_MONAN");
                    tb.HasTrigger("trg_InsertMonAn_ToKho");
                    tb.HasTrigger("trg_Log_MONAN_InsertUpdateDelete");
                    tb.HasTrigger("trg_UpdateMonAn_ToKho");
                });
                entity.HasIndex(e => e.TenMonAn, "UQ__MONAN__987DD97AAC87C7C9").IsUnique();
                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.Gia).HasColumnType("decimal(8, 0)");
                entity.Property(e => e.IdloaiMonAn).HasColumnName("IDLoaiMonAn");
                entity.Property(e => e.MoTa).HasMaxLength(255);
                entity.Property(e => e.TenMonAn).HasMaxLength(100);
                entity.HasOne(d => d.IdloaiMonAnNavigation).WithMany(p => p.Monans)
                    .HasForeignKey(d => d.IdloaiMonAn)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MONAN__IDLoaiMon__0A9D95DB");
            });

            modelBuilder.Entity<Nhanvien>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__NHANVIEN__3214EC279B137850");
                entity.ToTable("NHANVIEN", tb =>
                {
                    tb.HasTrigger("trg_BeforeDelete_NhanVien");
                    tb.HasTrigger("trg_Log_NHANVIEN_InsertUpdateDelete");
                });
                entity.HasIndex(e => e.Username, "UQ_Username").IsUnique();
                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.AnhNv).HasMaxLength(200).HasColumnName("AnhNV");
                entity.Property(e => e.DiaChi).HasMaxLength(200);
                entity.Property(e => e.Email).HasMaxLength(100);
                entity.Property(e => e.GioiTinh).HasMaxLength(10);
                entity.Property(e => e.HoTen).HasMaxLength(100);
                entity.Property(e => e.IdchucVu).HasColumnName("IDChucVu");
                entity.Property(e => e.NgayVao).HasDefaultValue(new DateOnly(2025, 5, 1));
                entity.Property(e => e.Sdt).HasMaxLength(15).HasColumnName("SDT");
                entity.Property(e => e.Username).HasMaxLength(50);
                entity.HasOne(d => d.IdchucVuNavigation).WithMany(p => p.Nhanviens)
                    .HasForeignKey(d => d.IdchucVu)
                    .HasConstraintName("FK__NHANVIEN__IDChuc__48CFD27E");
                entity.HasOne(d => d.UsernameNavigation).WithOne(p => p.Nhanvien)
                    .HasForeignKey<Nhanvien>(d => d.Username)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__NHANVIEN__Userna__47DBAE45");
            });

            modelBuilder.Entity<OtpXacnhan>(entity =>
            {
                entity.HasNoKey().ToTable("OTP_XACNHAN", tb => tb.HasTrigger("trg_Delete_OTP_HetHan"));
                entity.Property(e => e.Email).HasMaxLength(100);
                entity.Property(e => e.MaOtp).HasMaxLength(6).HasColumnName("MaOTP");
                entity.Property(e => e.ThoiGianTao).HasDefaultValueSql("(getdate())").HasColumnType("datetime");
                entity.Property(e => e.TrangThai).HasMaxLength(20).HasDefaultValue("Chưa dùng");
                entity.Property(e => e.Username).HasMaxLength(50);
            });

            modelBuilder.Entity<Phim>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__PHIM__3214EC2757964081");
                entity.ToTable("PHIM", tb => tb.HasTrigger("trg_Log_PHIM_InsertUpdateDelete"));
                entity.HasIndex(e => e.TenPhim, "UQ__PHIM__FA38361C47141D2A").IsUnique();
                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.Anh).HasMaxLength(255);
                entity.Property(e => e.DaoDien).HasMaxLength(100);
                entity.Property(e => e.DienVien).HasMaxLength(255);
                entity.Property(e => e.IdtheLoaiPhim).HasColumnName("IDTheLoaiPhim");
                entity.Property(e => e.IddinhDang).HasColumnName("IDDinhDang");
                entity.Property(e => e.IddoTuoi).HasMaxLength(10).HasColumnName("IDDoTuoi");
                entity.Property(e => e.MoTa).HasMaxLength(500);
                entity.Property(e => e.NgayKhoiChieu).HasColumnType("datetime");
                entity.Property(e => e.PosterUrl).HasMaxLength(255).HasColumnName("Poster_Url");
                entity.Property(e => e.TenPhim).HasMaxLength(100);
                entity.Property(e => e.TrailerUrl).HasMaxLength(255).HasColumnName("Trailer_Url");
                entity.Property(e => e.TrangThai).HasDefaultValue(1);
                entity.HasOne(p => p.TheLoaiPhim).WithMany(t => t.Phims)
                    .HasForeignKey(p => p.IdtheLoaiPhim)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PHIM__IDTheLoaiP__59063A47");
                entity.HasOne(p => p.DinhDangPhim).WithMany(d => d.Phims)
                    .HasForeignKey(p => p.IddinhDang)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PHIM__IDDinhDang__59FA5E80");
                entity.HasOne(p => p.DoTuoi).WithMany(d => d.Phims)
                    .HasForeignKey(p => p.IddoTuoi)
                    .HasConstraintName("FK__PHIM__IDDoTuoi__5AEE82B9");
            });

            modelBuilder.Entity<Phongchieu>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__PHONGCHI__3214EC27113D601B");
                entity.ToTable("PHONGCHIEU", tb => tb.HasTrigger("trg_Log_PHONGCHIEU_InsertUpdateDelete"));
                entity.HasIndex(e => e.TenPhong, "UQ__PHONGCHI__AE382B297A982F1A").IsUnique();
                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.AnhPhong).HasMaxLength(255);
                entity.Property(e => e.TenPhong).HasMaxLength(50);
                entity.Property(e => e.TrangThai).HasDefaultValue(1);
                entity.HasOne(d => d.MaLoaiPhongNavigation).WithMany(p => p.Phongchieus)
                    .HasForeignKey(d => d.MaLoaiPhong)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PHONGCHIE__MaLoa__66603565");
            });

            modelBuilder.Entity<Theloaiphim>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__THELOAIP__3214EC2769F97B4B");
                entity.ToTable("THELOAIPHIM", tb => tb.HasTrigger("trg_Log_THELOAIPHIM_InsertUpdateDelete"));
                entity.HasIndex(e => e.TenTheLoai, "UQ__THELOAIP__327F958FD6E2DB7B").IsUnique();
                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.TenTheLoai).HasMaxLength(50);
            });

            modelBuilder.Entity<Tkkhachhang>(entity =>
            {
                entity.HasKey(e => e.Username).HasName("PK__TKKHACHH__536C85E52618B7E8");
                entity.ToTable("TKKHACHHANG");
                entity.Property(e => e.Username).HasMaxLength(50);
                entity.Property(e => e.NgayTao).HasDefaultValueSql("(getdate())").HasColumnType("datetime");
            });

            modelBuilder.Entity<Tknhanvien>(entity =>
            {
                entity.HasKey(e => e.Username).HasName("PK__TKNHANVI__536C85E5242A5323");
                entity.ToTable("TKNHANVIEN", tb => tb.HasTrigger("trg_Log_TKNHANVIEN_InsertUpdateDelete"));
                entity.Property(e => e.Username).HasMaxLength(50);
                entity.Property(e => e.TrangThai).HasMaxLength(20);
            });

            modelBuilder.Entity<Trangthaighe>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__TRANGTHA__3214EC27B626F996");
                entity.ToTable("TRANGTHAIGHE");
                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.Idghe).HasColumnName("IDGhe");
                entity.Property(e => e.TrangThai).HasDefaultValue(0);
                entity.HasOne(d => d.IdgheNavigation).WithMany(p => p.Trangthaighes)
                    .HasForeignKey(d => d.Idghe)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TRANGTHAI__IDGhe__22FF2F51");
            });

            modelBuilder.Entity<Ve>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__VE__3214EC2723F75705");
                entity.ToTable("VE");
                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.GiaVe).HasColumnType("decimal(6, 0)");
                entity.Property(e => e.GioChieu).HasColumnType("datetime");
                entity.Property(e => e.Idghe).HasColumnName("IDGhe");
                entity.Property(e => e.IdhoaDonVe).HasColumnName("IDHoaDonVe");
                entity.Property(e => e.IdlichChieu).HasColumnName("IDLichChieu");
                entity.Property(e => e.NgayMua).HasDefaultValueSql("(getdate())").HasColumnType("datetime");
                entity.HasOne(d => d.IdgheNavigation).WithMany(p => p.Ves)
                    .HasForeignKey(d => d.Idghe)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__VE__IDGhe__01142BA1");
                entity.HasOne(d => d.IdhoaDonVeNavigation).WithMany(p => p.Ves)
                    .HasForeignKey(d => d.IdhoaDonVe)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__VE__IDHoaDonVe__02084FDA");
                entity.HasOne(d => d.IdlichChieuNavigation).WithMany(p => p.Ves)
                    .HasForeignKey(d => d.IdlichChieu)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__VE__IDLichChieu__00200768");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}