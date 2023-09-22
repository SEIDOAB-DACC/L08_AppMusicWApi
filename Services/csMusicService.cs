using System.Runtime.InteropServices;
using System.Security.Cryptography;
using DbContext;
using DbModels;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Services;

public class csMusicService : IMusicService
{
    public async Task<int> Seed(int NrOfitems)
    {
        var sg = new csSeedGenerator();

        var _musicgroups = new List<csMusicGroup>();
        for (int i = 0; i < NrOfitems; i++)
        {
            var mg = new csMusicGroup().Seed(sg);

            var _nrAlbums = sg.Next(0, 11);
            for (int a = 0; a < _nrAlbums; a++)
            {
                var al = new csAlbum().Seed(sg);
                mg.Albums.Add(al);
            }

            _musicgroups.Add(mg);
        }

        using (var db = csMainDbContext.DbContext("sysadmin"))
        {
            foreach (var item in _musicgroups)
            {
                db.MusicGroups.Add(item);
            }

            await db.SaveChangesAsync();

            int cnt = await db.MusicGroups.CountAsync();
            return cnt;
        }
    }

    public async Task<int> RemoveSeed()
    {
        using (var db = csMainDbContext.DbContext("sysadmin"))
        {
            //db.Albums.RemoveRange(db.Albums.Where(mg => mg.Seeded));
            db.MusicGroups.RemoveRange(db.MusicGroups.Where(mg => mg.Seeded));
            await db.SaveChangesAsync();

            int _count = await db.MusicGroups.CountAsync();
            return _count;
        }
    }

    public async Task<List<csMusicGroup>> Read(bool flat)
    {
        using (var db = csMainDbContext.DbContext("sysadmin"))
        {
            if (flat)
            {
                var _list = await db.MusicGroups.ToListAsync();
                return _list;
            }
            else
            {
                var _list = await db.MusicGroups
                    .Include(mg => mg.Albums).ToListAsync();
                return _list;
            }
        }
    }

    public async Task<csMusicGroup> ReadItem (Guid id, bool flat)
    {
        using (var db = csMainDbContext.DbContext("sysadmin"))
        {
            if (flat)
            {
                var mg = await db.MusicGroups
                    .FirstOrDefaultAsync(mg => mg.MusicGroupId == id);

                return mg;
            }
            else
            {
                var mg = await db.MusicGroups.Include(mg => mg.Albums)
                    .FirstOrDefaultAsync(mg => mg.MusicGroupId == id);

                return mg;
            }
        }
    }
}

