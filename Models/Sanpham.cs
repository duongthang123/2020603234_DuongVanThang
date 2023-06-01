﻿namespace _2020603234.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Sanpham")]
    public partial class Sanpham
    {
        [Key]
        public int Mavd { get; set; }

        [DisplayName("Tên sản phẩm")]
        [Required(ErrorMessage = "Không để trống tên")]
        [StringLength(100)]
        public string Tenvd { get; set; }

        [Required]
        [StringLength(250)]
        public string TenAnh { get; set; }

        [StringLength(250)]
        public string Mota { get; set; }

        public decimal Giatien { get; set; }

        public int? Soluong { get; set; }

        public int MaDanhmuc { get; set; }

        public virtual Danhmuc Danhmuc { get; set; }
    }
}
