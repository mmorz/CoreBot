﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreBot.Collections;
using CoreBot.Managers;
using CoreBot.Models;
using Discord.Commands;
using Humanizer;

namespace CoreBot.Modules
{
    [Group("event")]
    [Alias("events")]
    public class EventModule : ModuleBase
    {
        private readonly EventManager _eventManager;

        public EventModule(EventManager eventManager)
        {
            _eventManager = eventManager;
        }

        [Command("add")]
        public async Task AddEvent(string date, string time, [Remainder] string description)
        {
            DateTime eventDate;
            DateTime.TryParse(string.Format("{0} {1}", date, time), out eventDate);
            var remainder = eventDate.Subtract(DateTime.Now);
            if (remainder.TotalSeconds > 0)
            {
                await _eventManager.AddEventAsync(new Event(description, eventDate));
                await ReplyAsync($"Event **{description}** added, **time left:** {remainder.Humanize(2)}");
            }
            else await ReplyAsync("Invalid date.");
        }

        [Command("list")]
        public async Task ListEvents()
        {
            var list = new List<string>();
            Events.Instance.EventsList.Sort((a, b) => a.DateTime.CompareTo(b.DateTime));
            foreach (var eve in Events.Instance.EventsList)
            {
                var remainder = eve.DateTime.Subtract(DateTime.Now);
                list.Add($"{eve.Description} (id: {eve.Id}), **time left:** {remainder.Humanize(2)}");
            }
            await ReplyAsync($"{string.Join(Environment.NewLine, list)}");
        }

        [Command("find")]
        public async Task FindEvents([Remainder] string searchTerm)
        {
            var list = new List<string>();
            Events.Instance.EventsList.Sort((a, b) => a.DateTime.CompareTo(b.DateTime));
            foreach (var eve in Events.Instance.EventsList)
            {
                if (eve.Description.ToLower().Contains(searchTerm.ToLower()))
                {
                    var remainder = eve.DateTime.Subtract(DateTime.Now);
                    list.Add($"{eve.Description} (id: {eve.Id}), **time left:** {remainder.Humanize(2)}");
                }
            }
            if (list.Count > 0)
            {
                await ReplyAsync($"{string.Join(Environment.NewLine, list)}");
            }
            else await ReplyAsync("No events found.");
        }

        [Command("today")]
        public async Task GetEventsToday()
        {
            var list = new List<string>();
            Events.Instance.EventsList.Sort((a, b) => a.DateTime.CompareTo(b.DateTime));
            foreach (var eve in Events.Instance.EventsList)
            {
                if (eve.DateTime.Date == DateTime.Today.Date)
                {
                    var remainder = eve.DateTime.Subtract(DateTime.Now);
                    list.Add($"{eve.Description} (id: {eve.Id}), **time left:** {remainder.Humanize(2)}");
                }
            }
            if (list.Count > 0)
            {
                await ReplyAsync($"{string.Join(Environment.NewLine, list)}");
            }
            else await ReplyAsync("No events today.");
        }

        [Command("tomorrow")]
        public async Task GetEventsTomorrow()
        {
            var list = new List<string>();
            Events.Instance.EventsList.Sort((a, b) => a.DateTime.CompareTo(b.DateTime));
            foreach (var eve in Events.Instance.EventsList)
            {
                if (eve.DateTime.Date == DateTime.Now.AddDays(1).Date)
                {
                    var remainder = eve.DateTime.Subtract(DateTime.Now);
                    list.Add($"{eve.Description} (id: {eve.Id}), **time left:** {remainder.Humanize(2)}");
                }
            }
            if (list.Count > 0)
            {
                await ReplyAsync($"{string.Join(Environment.NewLine, list)}");
            }
            else await ReplyAsync("No events tomorrow.");
        }

        [Command("delete")]
        [Alias("del")]
        public async Task DeleteEvent(int id)
        {
            var eve = Events.Instance.EventsList.Find(x => x.Id == id);
            if (eve != null)
            {
                await _eventManager.DeleteEventAsync(eve);
                await ReplyAsync($"Event **{eve.Description}** deleted.");
            }
            else await ReplyAsync("Event not found.");
        }
    }
}