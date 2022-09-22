using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Advanced_Combat_Tracker;
using FFXIV_ACT_Plugin.Common;
using FFXIV_ACT_Plugin;
using RetainerNotice.Network.Parser;

namespace RetainerNotice
{
    public class Retainer : IActPluginV1
    {
        private static readonly int[] WaitTime = {5, 5, 10, 15, 25};
        private Label _pluginLabel;
        private FFXIV_ACT_Plugin.FFXIV_ACT_Plugin _ffxiv;
        private NetworkParser _parser = new NetworkParser();

        private string pattern1 =
            @"向雇员下达了“[\u4e00-\u9fa5]{6}”的探险委托";

        private string pattern2 =
            @"雇员[\u4e00-\u9fa5]{1,}成功完成了探险！";

        public async void InitPlugin(TabPage page, Label pluginLabel)
        {
            _pluginLabel = pluginLabel;
            
            _ffxiv = await FindFFxivACTPlugin();
            
            if(_ffxiv == null)
            {
                pluginLabel.Text = "你猜我为啥叫FF14插件";
                throw new NullReferenceException("找不到FF14解析插件");
            }

            if (!_ffxiv.PluginStarted)
            {
                pluginLabel.Text = "你先别急，先把解析插件加载了再打开我";
                throw new NoFFXIVPluginLoadedFirstException("FF14解析插件未首先加载");
            }

            page.Text = "资本家模拟器.jpg";
            page.Controls.Add(new UserControl());
            pluginLabel.Text = "啊哈哈哈，路灯来咯";
            await Toasty.Toast("蒙多想去哪就去哪");
            await StartPlugin();
        }

        private async Task StartPlugin()
        {
            ActGlobals.oFormActMain.OnLogLineRead +=
                async (import, info) =>
                {
                    string log = info.logLine;
                    if (Regex.IsMatch(log,pattern1))
                    {
                        string type = Regex.Match(log, @"“[\u4e00-\u9fa5]{6}”").Value;

                        await Toasty.Toast(type+"出发！");
                    }

                    if (Regex.IsMatch(log, pattern2))
                    {
                        string retainerName = Regex.Match(log, pattern2)
                            .Value
                            .Replace("雇员", String.Empty)
                            .Replace("成功完成了探险！", String.Empty);
                        await Toasty.Toast(retainerName+"成功归来！");
                    }
                };

        }

        public async void DeInitPlugin()
        {
            _pluginLabel.Text = "你雇员爷爷来了";
            await Toasty.Toast("下班下班");
        }

        private async Task<FFXIV_ACT_Plugin.FFXIV_ACT_Plugin> FindFFxivACTPlugin()
        {
            var plugins = ActGlobals.oFormActMain.ActPlugins;
            for (int i = 0; i < WaitTime.Length; ++i)
            {
                var plugin = plugins
                    .Find(p => p.lblPluginTitle.Text.Contains("FFXIV_ACT_Plugin"));
                if (plugin != null)
                {
                    return plugin.pluginObj as FFXIV_ACT_Plugin.FFXIV_ACT_Plugin;
                }

                await Task.Delay(i * 1000);
            }

            return null;
        }

    }

    class NoFFXIVPluginLoadedFirstException : Exception
    {
        public NoFFXIVPluginLoadedFirstException(string message):base(message)
        {
            
        }

        public NoFFXIVPluginLoadedFirstException():base()
        {
            
        }
    }

}
