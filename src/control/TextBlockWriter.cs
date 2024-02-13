using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace PWC.src.control {
    public class TextBlockWriter : TextWriter {
        private readonly TextBlock _textBlock;

        public TextBlockWriter(TextBlock textBlock) {
            _textBlock = textBlock;
        }

        public override void Write(char value) {
            Application.Current.Dispatcher.Invoke(() => {
                _textBlock.Text += value;
            });
        }

        public override void Write(string? value) {
            Application.Current.Dispatcher.Invoke(() => {
                _textBlock.Text += value;
            });
        }

        public override System.Text.Encoding Encoding => System.Text.Encoding.UTF8;
    }
}