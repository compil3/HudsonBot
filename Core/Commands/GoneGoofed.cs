using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;


namespace Hudson.Core.Commands
{
    public class GoneGoofed : ModuleBase<SocketCommandContext>
    {
        [Command(".g"), Alias(".gio"), Summary("Gio commands")]
        public async Task initialCommand()
        {
            await Context.Channel.SendMessageAsync("Did he fuck up again?");
        }

        //displays embed stats.
        [Command(".stats"), Summary("Running count of fuck ups")]
        public async Task Embeded()
        {
            EmbedBuilder builder = new EmbedBuilder();
            builder.WithAuthor("Gio Stats");
            builder.WithColor(253,185,19);
            builder.WithFooter("GioBot by SpillShot", Context.Guild.Owner.GetAvatarUrl());
            builder.WithDescription("Current stats for Gio");

           await Context.Channel.SendMessageAsync("", false, builder.Build());
        }
    }
}
