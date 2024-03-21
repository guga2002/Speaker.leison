﻿namespace BuisnessLayer.Models
{
    public class ChanellModel
    {
        public int Id { get; set; }
        public string ChanellFormat { get; set; } //MPG4 vs MPG2
        public int PortIn250 { get; set; }
        public bool FromOptic { get; set; }
        public string Name { get; set; }
    }
}
