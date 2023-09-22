using System;
using DbModels;
using System.ComponentModel.DataAnnotations;

namespace Models.DTO
{
	public class csMusicGroupCUdto
    {
        public Guid MusicGroupId { get; set; }
        public bool Seeded { get; set; } = true;

        public string Name { get; set; }
        public int EstablishedYear { get; set; }

        public enMusicGenre Genre { get; set; }

        public List<Guid> AlbumsId { get; set; } = new List<Guid>();

        public csMusicGroupCUdto()
        {

        }
        public csMusicGroupCUdto(csMusicGroup model)
		{
            this.MusicGroupId = model.MusicGroupId;
            this.Seeded = model.Seeded;
            this.Name = model.Name;
            this.EstablishedYear = model.EstablishedYear;
            this.Genre = model.Genre;

            this.AlbumsId = model?.Albums.Select(a => a.AlbumId).ToList();
		}
	}
}

