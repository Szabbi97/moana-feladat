﻿namespace moana_feladat.Models
{
    public class Card
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public byte Status { get; set; }
        public uint Position { get; set; }
    }
}
