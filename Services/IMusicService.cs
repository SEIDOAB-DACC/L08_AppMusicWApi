using System;
using DbModels;

namespace Services
{
	public interface IMusicService
	{
        public Task<int> Seed(int NrOfitems);
        public Task<int> RemoveSeed();

        public Task<List<csMusicGroup>> Read(bool flat);
        public Task<csMusicGroup> ReadItem(Guid id, bool flat);
    }
}

