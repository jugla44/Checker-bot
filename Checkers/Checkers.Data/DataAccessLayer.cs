﻿using Checkers.Data.Context;
using Checkers.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers.Data
{
    public class DataAccessLayer
    {
        private readonly IDbContextFactory<CheckersDbContext> _contextFactory;
        public DataAccessLayer(IDbContextFactory<CheckersDbContext> contextFactory)
        {
            
        }

        public async Task CreateGuild(ulong id)
        {
            using var context = _contextFactory.CreateDbContext();
            if (context.Guilds.Any(x => x.Id == id)) 
                return;

            context.Add(new Guild { Id = id });
            await context.SaveChangesAsync();
        }

        public string GetPrefix(ulong id)
        {
            using var context = _contextFactory.CreateDbContext();
            var guild = context.Guilds
                .Find(id);

            if (guild == null)
            {
                guild = context.Add(new Guild { Id = id }).Entity;
                context.SaveChanges();
            }

            return guild.Prefix;
        }

        public async Task SetPrefix(ulong id, string prefix)
        {
            using var context = _contextFactory.CreateDbContext();
            var guild = await context.Guilds
                .FindAsync(id);

            if(guild != null)
            {
                guild.Prefix = prefix;
            }
            else
            {
                context.Add(new Guild { Id = id, Prefix = prefix });
            }

            await context.SaveChangesAsync();
        }

        public async Task DeleteGuild(ulong id)
        {
            using var context = _contextFactory.CreateDbContext();   
            var guild = await context.Guilds.FindAsync(id);

            if (guild == null)
                return;

            context.Remove(guild);
            await context.SaveChangesAsync();
        }
        public async Task RegisterPlayer(long id)
        {
            using var context = _contextFactory.CreateDbContext();
            if (context.Guilds.Any(x => x.Id == id))
                return;

            context.Add(new Guild { Id = id });
            await context.SaveChangesAsync();
        }
    }
}
