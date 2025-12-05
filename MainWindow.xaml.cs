using MahApps.Metro.Controls;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;  // For plist XML parsing

namespace PresetMagician.Reborn
{
    public class PluginInfo
    {
        public string Name { get; set; } = "";
        public string Path { get; set; } = "";
        public string Type => "VST3";
    }

    public partial class MainWindow : MetroWindow
    {
        public ObservableCollection<PluginInfo> Plugins { get; } = new();
        public ICommand ScanCommand { get; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            ScanCommand = new RelayCommand(_ => ScanVsts());
        }

        private void ScanVsts()
        {
            StatusText.Text = "Scanning VST3 plugins...";
            Plugins.Clear();

            var vst3Paths = new[]
            {
                @"C:\Program Files\Common Files\VST3",
                @"C:\Program Files (x86)\Common Files\VST3"
            };

            int count = 0;

            foreach (var basePath in vst3Paths)
            {
                if (!Directory.Exists(basePath)) continue;

                foreach (var vst3Dir in Directory.GetDirectories(basePath, "*.vst3"))
                {
                    try
                    {
                        var name = GetVst3Name(vst3Dir);
                        Plugins.Add(new PluginInfo
                        {
                            Name = name ?? Path.GetFileNameWithoutExtension(vst3Dir),
                            Path = vst3Dir
                        });
                        count++;
                    }
                    catch { }  // Ignore broken .vst3 bundles
                }
            }

            StatusText.Text = count > 0
                ? $"SUCCESS → Found {count} VST3 plugin{(count == 1 ? "" : "s")}!"
                : "No VST3 plugins found";
        }

        // Parse VST3 plist for name
        private string GetVst3Name(string vst3Dir)
		{
			var plistPath = Path.Combine(vst3Dir, "Contents", "Info.plist"); // Note: capital I
			if (!File.Exists(plistPath)) return null;

			try
			{
				var doc = XDocument.Load(plistPath);
				var dict = doc.Root?.Element("dict");
				if (dict == null) return null;

				var key = dict.Elements("key")
							  .FirstOrDefault(k => k.Value == "Name");

				if (key != null)
				{
					var next = key.NextNode as XElement;
					if (next?.Name == "string")
						return next.Value;
				}
			}
			catch { }

			return null;
		}
    }

    public class RelayCommand : ICommand
		{
			private readonly Action<object?> _execute;

			public RelayCommand(Action<object?> execute)
			{
				_execute = execute;
			}

			public bool CanExecute(object? parameter) => true;

			public void Execute(object? parameter) => _execute(parameter);

			// This line is REQUIRED by ICommand — never remove it
			public event EventHandler? CanExecuteChanged;
		}
}