﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;
using Humanizer;

namespace CoreBot.Modules
{
    public class SeenModule : ModuleBase
    {
        [Command("seen"), Summary("Shows latest activity of the specified user.")]
        public async Task Seen(string userName)
        {
            userName = userName.ToLower();
            var found = await Context.Channel.GetMessagesAsync(2000)
                .FirstOrDefault(batch => batch
                .Any(message => message.Author.Username.ToLower() == userName));
            if (found != null)
            {
                var msg = found.First(m => m.Author.Username.ToLower() == userName);
                await ReplyAsync($"{msg.Author.Username} was last seen {DateTime.Now.Subtract(msg.Timestamp.DateTime).Humanize(3)} ago saying: `{msg.Content}`");
            }
            else
            {
                await ReplyAsync($"No messages from \"{userName}\"");
            }
        }
    }
}