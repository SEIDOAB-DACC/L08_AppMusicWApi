using System;
using DbModels;
using Models.DTO;

namespace Services
{
	public interface IMusicService
	{
        public Task<int> Seed(int NrOfitems);
        public Task<int> RemoveSeed();

        public Task<List<csMusicGroup>> Read(bool flat);
        public Task<csMusicGroup> ReadItem(Guid id, bool flat);

        public Task<csMusicGroup> UpdateItem(csMusicGroupCUdto _src);
    }
}

