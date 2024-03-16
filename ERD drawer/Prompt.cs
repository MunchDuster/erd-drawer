using System.Diagnostics;

namespace ERD_drawer
{
    public static class Prompt
    {
        private const int totalWidth = 500;
        private const int padding = 50;
        private const int itemSpacing = 50;
        private const int itemHeight = 40;

        private const int itemPadding = (itemSpacing - itemHeight) / 2;

        public static string[]? ShowDialog(string[] inputs, string caption, Color borderColor)
        {
            if (inputs == null)
            {
                Debug.WriteLine("Prompt.ShowDialog ERROR: input text array is null");
                return null;
            }
            if (inputs.Length == 0)
            {
                Debug.WriteLine("Prompt.ShowDialog ERROR: input text array is empty");
                return null;
            }

            int totalHeight = padding + (inputs.Length + 1) * itemSpacing;

            Form prompt = new Form()
            {
                Width = totalWidth,
                Height = totalHeight,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen,
            };
            totalHeight -= 50;

            // confirm button
            int buttonTop = totalHeight - itemSpacing;
            Button confirmation = new Button() { Text = "Ok", Left = totalWidth / 2 - 50, Width = 100, Height = itemHeight, Top = buttonTop, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(confirmation);
            prompt.AcceptButton = confirmation;

            // allow closing of prompt if escape pressed when in any text box
            void ExitIfEscapePressed(object? sender, KeyEventArgs e)
            {
                if (e.KeyCode == Keys.Escape)
                    prompt.Close();
            }

            // inputs
            TextBox[] textBoxes = new TextBox[inputs.Length];
            for (int i = 0; i < inputs.Length; i++)
            {
                int top = i * itemSpacing + (i + 1) * itemPadding;
                
                textBoxes[i] = new TextBox() { Left = 100, Top = top, Width = 350 };
                textBoxes[i].KeyDown += ExitIfEscapePressed;
                prompt.Controls.Add(textBoxes[i]);
                
                Label textLabel = new Label() { Left = 20, Top = top, Width = 70, Text = inputs[i] };
                prompt.Controls.Add(textLabel);
            }

            textBoxes[0].Select();

            if (prompt.ShowDialog() != DialogResult.OK)
                return null;

            return textBoxes.Select(textBox => textBox.Text).ToArray();
        }
    }
}
