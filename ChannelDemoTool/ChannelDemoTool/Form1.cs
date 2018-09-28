using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChannelDemoTool
{

//服务器资源目录(https://git.oschina.net/joymeng/channelDemo/raw/master/)
//本地资源目录(E:\GitDir\channelDemo\)

//待更新文件(

//README.md(142054a0528ec0327915c9a69bcd5092)
//demo_自测用例/0000066/v7.1.3/GameDemo-release.apk(38f18e9185ead8eba5857f51560e39f6)
//demo_自测用例/0000066/v7.1.3/ltsdk_23_v7.1.3_UC.apk(f8e01fb149d9b48b08e75d70f78287e8)
//demo_自测用例/0000843_奇虎360/V1.8.0_546/360SDK_Android_OnlineGame.apk(549e8caffbf8062912bd28b36deb0af7)
    
//)待更新文件

    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            loadChannelDemoInfos();
        }

        //string configUrl = "https://git.oschina.net/joymeng/ChannelConfig/raw/master/MD5.txt";    // 文件列表信息
        //string configUrl = "https://raw.githubusercontent.com/joymeng/ChannelConfig/master/MD5.txt";    // 文件列表信息
        string configUrl = "";

        List<string> channels = new List<string>();                                             // 渠道信息, 0000843_奇虎360
        Dictionary<string, string> channelDirs = new Dictionary<string, string>();               // 渠道目录信息, 配置文件/0000843_奇虎360/V1.8.0_546/

        /// <summary>
        /// 获取“待更新资源目录”
        /// </summary>
        private string getAttach_ConfigUrl()
        {
            string url = "";

            string configInfo = WebSettings.getWebData(DependentFiles.ToolUrl);
            if (!configInfo.Equals("")) url = WebSettings.getNodeData(configInfo, "待更新资源目录", true).Trim();
            if (url.Equals("")) MessageBox.Show("配置文件" + DependentFiles.ToolUrl + "，中的“待更新资源目录”为空。\r\n请先配置，再使用此工具");

            return url;
        }

        /// <summary>
        /// 载入渠道demo、测试用例文件信息
        /// </summary>
        private void loadChannelDemoInfos()
        {
            configUrl = getAttach_ConfigUrl();

            // 获取渠道文件信息
            string configInfo = WebSettings.getWebData(configUrl);
            if (configInfo.Equals("")) return;

            string serverDir = WebSettings.getNodeData(configInfo, "服务器资源目录", true).Trim();
            string filesInfo = WebSettings.getNodeData(configInfo, "待更新文件", false).Trim();

            string[] files = filesInfo.Replace("\r\n", "\n").Split('\n');
            List<string> updateFiles = new List<string>();                      // 用于记录更新的文件完整路径信息
            foreach (string file0 in files)
            {
                string file = file0.Trim().TrimEnd(')');
                if (file.Equals("")) continue;

                if (file.StartsWith("demo_自测用例/"))
                {
                    string channelDir = file.Substring(0, file.LastIndexOf("/"));

                    string[] A = file.Split('/');
                    if (A.Length > 2)
                    {
                        if (!channels.Contains(A[1]))
                        {
                            channels.Add(A[1]);
                            channelDirs.Add(A[1], channelDir);
                        }
                        else channelDirs[A[1]] = channelDir;
                    }
                }
            }

            // 按渠道显示至list列表
            listBoxChannel.Items.Clear();
            listBoxChannel.Items.AddRange(channels.ToArray());
        }

        // 检索
        private void textBox_search_TextChanged(object sender, EventArgs e)
        {
            String text = textBox_search.Text;
            if (text.Equals("")) return;

            for (int i = channels.Count - 1; i >= 0; i--)
            {
                //bool march = (list[i].StartsWith(text) || list[i].Contains(" " + text));
                bool march = (channels[i].Contains(text));
                listBoxChannel.SetSelected(i, march);
            }
        }

        /// <summary>
        /// 获取对应渠道的测试用例和demo，并打开
        /// </summary>
        private void listBoxChannel_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxChannel.SelectedItem == null || configUrl.Equals("")) return;

            string iteam = listBoxChannel.SelectedItem.ToString();
            //MessageBox.Show("当前选中项为：" + iteam);
            DependentFiles.updateFiles(configUrl, "demo_自测用例/" + iteam + "/");  // 更新选中渠道文件
            DependentFiles.openChannelDir(channelDirs[iteam]);                      // 打开文件所在目录
        }

    }
}
