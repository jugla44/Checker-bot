﻿// <copyright file="DataAccessLayer.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Checkers.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Checkers.Data.Context;
    using Checkers.Data.Models;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// The DataAccessLayer for the Checkers client.
    /// </summary>
    public class DataAccessLayer
    {
        private readonly IDbContextFactory<CheckersDbContext> contextFactory;

        public DataAccessLayer(IDbContextFactory<CheckersDbContext> contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        #region Guild Commands
        public async Task CreateGuild(ulong id)
        {
            using var context = this.contextFactory.CreateDbContext();
            if (context.Guilds.Any(x => x.Id == id))
            {
                return;
            }

            context.Add(new Guild { Id = id });
            await context.SaveChangesAsync();
        }

        public string GetPrefix(ulong id)
        {
            using var context = this.contextFactory.CreateDbContext();
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
            using var context = this.contextFactory.CreateDbContext();
            var guild = await context.Guilds
                .FindAsync(id);

            if (guild != null)
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
            using var context = this.contextFactory.CreateDbContext();
            var guild = await context.Guilds.FindAsync(id);

            if (guild == null)
            {
                return;
            }

            context.Remove(guild);
            await context.SaveChangesAsync();
        }
        #endregion

        /// <summary>
        /// This is test Summary
        /// </summary>
        /// <param name="username" > The username of the player. </param>
        /// <param name="id" > The id of the player. </param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        public async Task RegisterPlayer(string username, ulong id)
        {
            using var context = this.contextFactory.CreateDbContext();

            context.Add(new Player(id, username));
            await context.SaveChangesAsync();
        }

        public async Task AttemptLogIn(ulong id)
        {
            using var context = this.contextFactory.CreateDbContext();

            var player = await context.Players.FindAsync(id);

            if (player != null)
            {
                player.IsActive = true;
            }
        }

        public async Task LogOut(ulong id)
        {

        }

        /// <summary>
        /// Searches for the Player.
        /// </summary>
        /// <param name="id"> The ID of the Player being searched for. </param>
        /// <returns> True if the Player is found within the database. </returns>
        public Player? HasPlayer(ulong id)
        {
            using var context = this.contextFactory.CreateDbContext();

            var player = context.Players.Find(id);
            return player;
        }

        public async Task UpdatePlayerName(Player player, string name)
        {
            using var context = this.contextFactory.CreateDbContext();

            var playerEntry = await context.Players.FindAsync(player.Id);

            if (playerEntry != null)
            {
                playerEntry.Username = name;
                player.Username = name;
            }
            await context.SaveChangesAsync();
        }
    }
}
