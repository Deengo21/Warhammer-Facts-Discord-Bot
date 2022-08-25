using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using PuppeteerSharp;

namespace TutorialBot.Modules
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
        [Command("random")]
        public async Task random()
        {
           var msg = await ReplyAsync("Pobieram losowy artykuł!");
            
            string fullUrl = "https://warhammerfantasy.fandom.com/wiki/Special:Random";

            List<string> programmerLinks = new List<string>();

            var options = new LaunchOptions()
            {
                Headless = true,
                ExecutablePath = "C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe"
            };

            var browser = await Puppeteer.LaunchAsync(options);
            var page = await browser.NewPageAsync();
            await page.GoToAsync(fullUrl);
            var headingElements = await page.XPathAsync("//*[@id=\"firstHeading\"]");
            var header = await page.EvaluateFunctionAsync<string>("e => e.textContent", headingElements);
            var firstParagraph = await page.XPathAsync("//*[@id=\"mw-content-text\"]/div/p[1]");
            var paraContent = await page.EvaluateFunctionAsync<string>("e => e.textContent", firstParagraph);
            var linkToPage = page.Url;
            
            await browser.CloseAsync();

            await msg.ModifyAsync(msg => msg.Content = $"{header.Trim()} - {paraContent} \n [{header.Trim()}]({linkToPage}) ");
        }

        }
    }
