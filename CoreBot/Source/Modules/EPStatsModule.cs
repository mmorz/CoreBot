﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreBot.Settings;
using Discord;
using Discord.Commands;
using epnetcore;

namespace CoreBot.Modules
{
    public class EPStatsModule : ModuleBase
    {
        private readonly EPClient _client;

        public EPStatsModule(EPClient client)
        {
            _client = client;
        }

        [Command("contract"), Summary("Displays contract summary of the given player.")]
        public async Task GetPlayerContractInfoAsync([Remainder] string playerName)
        {
            int id;

            try
            {
                id = await _client.GetPlayerIdAsync(playerName);
            }
            catch (NullReferenceException)
            {
                await ReplyAsync($"No contract found for {playerName}.");
                return;
            }

            var stats = await _client.GetPlayerStatsAsync(id);
            var data = stats.Data.Find(x => x.Season.EndYear == (DateTime.Now.Year - 1) || x.Season.EndYear == DateTime.Now.Year);

            if (data != null)
            {
                if (data.Player.Caphit == null || string.IsNullOrEmpty(data.Player.Caphit)) await ReplyAsync($"**Contract:** {data.Player.Contract} **Team:** {data.Team.Name}");
                else await ReplyAsync($"**Contract:** {data.Player.Contract} **Cap hit:** {data.Player.Caphit} **Team:** {data.Team.Name}");
            }
            else await ReplyAsync("No contract found for current season.");
        }

        [Command("scoring"), Summary("Displays current top 5 scoring.")]
        public async Task GetPlayerStatsAsync()
        {
            var scoring = await _client.GetTopScorers(7);
            var scoringList = new List<string>();
            foreach (var scorer in scoring.Data)
            {
                scoringList.Add($"**{scorer.Player.FirstName} {scorer.Player.LastName}** ({scorer.Team.Name}) **GP:** {scorer.GP} **G:** {scorer.G} **A:** {scorer.A} **TP:** {scorer.TP} **PPG:** {scorer.PPG} **+/-:** {scorer.PM} **PIM:** {scorer.PIM}");
            }
            await ReplyAsync(string.Join(Environment.NewLine, scoringList));
        }

        [Command("stats"), Summary("Displays stats of the given player.")]
        public async Task GetPlayerStatsAsync([Remainder] string playerName)
        {
            int id;

            try
            {
                id = await _client.GetPlayerIdAsync(playerName);
            }
            catch (NullReferenceException)
            {
                await ReplyAsync($"No stats found for {playerName}.");
                return;
            }

            var stats = await _client.GetPlayerStatsAsync(id);
            stats.Data.Sort((a, b) => b.Season.EndYear.CompareTo(a.Season.EndYear));
            var data = stats.Data.FindAll(x => x.Season.EndYear == stats.Data.FirstOrDefault().Season.EndYear);
            if (data.Count < 1)
            {
                await ReplyAsync($"No stats found for {playerName}.");
                return;
            }

            var embed = new EmbedBuilder();
            var season = data.FirstOrDefault();
            var player = season.Player;
            embed.WithTitle($"{player.FirstName} {player.LastName}");
            embed.WithDescription($"**DoB:** {player.DateOfBirth} **Country:** {player.Country.Name} **Height:** {player.Height} cm **Weight:** {player.Weight} kg");
            embed.AddField("Latest season", $"{season.Season.StartYear}-{season.Season.EndYear}");
            foreach (var team in data)
            {
                string gameType = string.Empty;
                switch (team.GameType)
                {
                    case "REGULAR_SEASON": gameType = "Regular season"; break;
                    case "PLAYOFFS": gameType = "Playoffs"; break;
                    default: gameType = team.GameType; break;
                }
                if (player.PlayerPosition == "GOALIE")
                {
                    embed.AddField($"{team.Team.Name} ({gameType})", $"**GP:** {team.GP} **SVP:** {team.SVP} **GAA:** {team.GAA}");
                }
                else
                {
                    embed.AddField($"{team.Team.Name} ({gameType})", $"**GP:** {team.GP} **G:** {team.G} **A:** {team.A} **TP:** {team.TP} **PPG:** {team.PPG} **+/-:** {team.PM} **PIM:** {team.PIM}");
                }
                embed.WithTimestamp(DateTime.Parse(season.Updated));
                embed.WithUrl(string.Format(DefaultValues.EP_PLAYERSTATS_URL, player.Id));
                if (!string.IsNullOrEmpty(player.ImageUrl)) embed.WithThumbnailUrl(string.Format(DefaultValues.EP_PLAYERIMAGE_URL, player.ImageUrl));
            }
            await ReplyAsync(string.Empty, embed: embed);
        }
    }
}
