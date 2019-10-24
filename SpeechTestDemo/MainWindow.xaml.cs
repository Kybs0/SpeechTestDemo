using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Speech.Synthesis;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp15
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            TextBox1.Text = "You have to take capital appreciation of the property into account.你必须将该处房产的资本增值考虑在内。\r\n" +
                            "The direction of the prevailing winds should be taken into account.应该将盛行风的方向考虑在内。\r\n" +
                            "The Villad'Este was a short distance northeast of Rome, nestled high in the Sabine Hills.埃斯泰别墅在罗马东北部不远的地方, 偎倚在萨空山的高处.\r\n";

            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var voices = speechSyn.GetInstalledVoices().ToList();
            VoiceComboBox.ItemsSource = voices;
        }

        private void TestSpeechButton_OnClick(object sender, RoutedEventArgs e)
        {
            PlayAsync();
        }

        private SpeechSynthesizer speechSyn = new SpeechSynthesizer();
        /// <summary>
        /// 异步播放
        /// </summary>
        private void PlayAsync()
        {
            speechSyn.SpeakAsyncCancelAll();
            speechSyn.Volume = 50;
            speechSyn.Rate = 0;
            var selectedValue = VoiceComboBox.SelectionBoxItem;
            if (selectedValue is InstalledVoice voiceInfo)
            {
                speechSyn.SelectVoice(voiceInfo.VoiceInfo.Name);
            }
            speechSyn.SpeakAsync(TextBox1.Text);
        }
        /// <summary>
        /// 同步播放
        /// 注：卡UI
        /// </summary>
        private void Play()
        {
            using (SpeechSynthesizer speechSyn = new SpeechSynthesizer())
            {
                speechSyn.Speak(TextBox1.Text);
            }
        }
        /// <summary>
        /// 导出音频文件
        /// </summary>
        private void ExportAudioButton_OnClick(object sender, RoutedEventArgs e)
        {
            speechSyn.SpeakAsyncCancelAll();
            speechSyn.Volume = 50;
            speechSyn.Rate = 0;
            var selectedValue = VoiceComboBox.SelectedValue;
            if (selectedValue is InstalledVoice voiceInfo)
            {
                speechSyn.SelectVoice(voiceInfo.VoiceInfo.Name);
            }

            var filePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\{Guid.NewGuid()}.mp3";
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            speechSyn.SetOutputToWaveFile(filePath);
            speechSyn.Speak(TextBox1.Text);
            speechSyn.SetOutputToDefaultAudioDevice();

            MessageBox.Show($"保存录音文件成功，保存路径：{filePath}");
        }
    }
}
