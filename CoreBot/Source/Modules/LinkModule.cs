﻿using System.Threading.Tasks;
using CoreBot.Settings;
using Discord.Commands;

namespace CoreBot.Modules
{
    /// <summary>
    /// Allows lazy users to link specific links (e.g. Twitch channels) fast, without having to write
    /// or copy/paste the whole URL.
    /// </summary>
    public class LinkModule : ModuleBase
    {
        [Command("twitch"), Summary("Allows users to quickly link Twitch channels.")]
        [Alias("stream")]
        public async Task GetTwitchLinkAsync(string channel)
        {
            await ReplyAsync(string.Format(DefaultValues.TWITCH_URL, channel));
        }

        [Command("twitter"), Summary("Allows users to quickly link Twitter user profiles.")]
        public async Task GetTwitterLinkAsync(string user)
        {
            await ReplyAsync(string.Format(DefaultValues.TWITTER_URL, user));
        }
    }
}
