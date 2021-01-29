using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using KMCCC.Authentication;
using KMCCC.Launcher;
using System.Collections;

namespace mcqidognqi
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public static LauncherCore Core = LauncherCore.Create();
        ArrayList java_path = new ArrayList();

        public MainWindow()
        {

            
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
            java_path.Add(Environment.GetEnvironmentVariable("JAVA_HOME")+@"\bin\javaw.exe");
            javapath.Text = java_path[0].ToString();
            var versions = Core.GetVersions().ToArray();//定义变量获取版本列表
            comboBox1.ItemsSource = versions;//绑定数据源
            comboBox1.DisplayMemberPath = "Id";//设置comboBox显示的为版本Id
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var ver = (KMCCC.Launcher.Version)comboBox1.SelectedItem;
            LaunchOptions options = new LaunchOptions
            {
                JavaPath = javapath.Text,
                Version = ver, //Ver为Versions里你要启动的版本名字
                MaxMemory = 1024, //最大内存，int类型
                //Authenticator = new OfflineAuthenticator("Xiao_yejun"), //离线启动
                Mode = LaunchMode.MCLauncher, //启动模式，这个我会在后面解释有哪几种
                Size = new WindowSize { Height = 768, Width = 1280 } //设置窗口大小，可以不要
            };
            if ((bool)zb.IsChecked)
            {
                options.Authenticator = new YggdrasilLogin(youxiang.Text, mima.Text, false);
            }
            else
            {
                options.Authenticator = new OfflineAuthenticator("Xiao_yejun"); //离线启动
            }

            var result = Core.Launch(options);
            


            //Authenticator = new YggdrasilLogin("邮箱", "密码", true),
            //case ErrorType.AuthenticationFailed:
            //MessageBox.Show(this, "正版验证失败！请检查你的账号密码", "账号错误\n详细信息：" + result.ErrorMessage,MessageBoxButtons.OK,MessageBoxIcon.Error);
            if (!result.Success)
            {
                switch (result.ErrorType)
                {
                    case ErrorType.AuthenticationFailed:
                        MessageBox.Show(this, "正版验证失败！请检查你的账号密码", "账号错误\n详细信息：" + result.ErrorMessage);
                        break;
                    case ErrorType.NoJAVA:
                        MessageBox.Show(result.ErrorMessage + "你没有java哦");
                        break;
                    case ErrorType.UncompressingFailed:
                        MessageBox.Show(result.ErrorMessage + "文件损坏了呢");
                        break;
                    default:
                        MessageBox.Show(result.ErrorMessage+"启动错误");
                        break;
                }
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
