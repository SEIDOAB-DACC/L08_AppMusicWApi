using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using System.Linq;

using Configuration;
using Models;
using Models.DTO;


namespace DbModels
{
    public enum enMusicGenre {Rock, Blues, Jazz, Metall }

    public class csMusicGroup //: ISeed<csMusicGroup>
    {
        [Key]       
        public Guid MusicGroupId { get; set; }
        public bool Seeded { get; set; } = true;

        public string Name { get; set; }
        public int EstablishedYear { get; set; }

        [Required]
        public enMusicGenre Genre { get; set; }

        [Required]
        public string strGenre
        {
            get => Genre.ToString();
            set { }
        }

        public List<csAlbum> Albums { get; set; } = new List<csAlbum>();

        public csMusicGroup()
        {
        }
        public csMusicGroup(csMusicGroupCUdto _dto)
        {
            MusicGroupId = Guid.NewGuid();
            Name = _dto.Name;
            EstablishedYear = _dto.EstablishedYear;
            Genre = _dto.Genre;
            Seeded = false;
        }

        public csMusicGroup Seed(csSeedGenerator sgen)
        {
            var mg = new csMusicGroup
            {
                MusicGroupId = Guid.NewGuid(),
                Name = sgen.MusicBand,
                EstablishedYear = sgen.Next(1970, 2023),
                Genre = sgen.FromEnum<enMusicGenre>(),
                Seeded = true
            };
            return mg;
        }
    }

    public class csAlbum : ISeed<csAlbum>
    {
        [Key]       
        public Guid AlbumId { get; set; }
        public bool Seeded { get; set; } = true;

        public string Name { get; set; }
        public int ReleaseYear { get; set; }
        public long CopiesSold { get; set; }

        public csMusicGroup MusicGroup { get; set; }

        public csAlbum()
        {
        }

        public csAlbum Seed(csSeedGenerator sgen)
        {
            var al = new csAlbum
            {
                AlbumId = Guid.NewGuid(),
                Name = sgen.MusicAlbum,
                CopiesSold = sgen.Next(1000, 1000000),
                ReleaseYear = sgen.Next(1970, 2023),
                Seeded = true
            };
            return al;
        }
    }
}

