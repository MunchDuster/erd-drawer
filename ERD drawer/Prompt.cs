using System.Diagnostics;

namespace ERD_drawer
{
    public static class Prompt
    {
        private const int totalWidth = 800;
        private const int padding = 50;
        private const int itemSpacing = 50;
        private const int itemHeight = 40;
        private const int labelPadding = 20;

        private const int itemPadding = (itemSpacing - itemHeight) / 2;

        public static string[]? ShowDialog(Dictionary<string, string> inputs, string caption, Color borderColor)
        {
            if (inputs == null)
            {
                Debug.WriteLine("Prompt.ShowDialog ERROR: input text array is null");
                return null;
            }
            if (inputs.Count == 0)
            {
                Debug.WriteLine("Prompt.ShowDialog ERROR: input text array is empty");
                return null;
            }
            int totalHeight = padding + (inputs.Count + 1) * itemSpacing;

            Form prompt = new() {
                Font = Displayable.font,
                Width = totalWidth,
                Height = totalHeight,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen,
            };
            totalHeight -= 50;

            // confirm button
            Button confirmation = new() { 
                Text = "Ok", 
                Left = totalWidth / 2 - 50, 
                Width = 100, 
                Height = itemHeight, 
                Top = totalHeight - itemSpacing, 
                DialogResult = DialogResult.OK 
            };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(confirmation);
            prompt.AcceptButton = confirmation;

            // allow closing of prompt if escape pressed when in any text box
            void ExitIfEscapePressed(object? sender, KeyEventArgs e)
            {
                if (e.KeyCode == Keys.Escape)
                    prompt.Close();
            }

            int CalcTop(int i) => i * itemSpacing + (i + 1) * itemPadding;

            int[] labelWidths = new int[inputs.Count];
            string[] labels = inputs.Keys.ToArray();
            for (int i =0; i < inputs.Count; i++)
            {
                labelWidths[i] = Displayable.GetStringWidth(labels[i]) + labelPadding;
                Label textLabel = new Label() { 
                    Left = 20, 
                    Top = CalcTop(i), 
                    Width = labelWidths[i], 
                    Text = labels[i]
                };
                prompt.Controls.Add(textLabel);
            }

            // inputs
            TextBox[] textBoxes = new TextBox[inputs.Count];
            string[] placeholders = inputs.Values.ToArray();
            for (int i = 0; i < inputs.Count; i++)
            {
                int left = labelWidths[i] + 30;
                textBoxes[i] = new TextBox() {
                    Left = left,
                    Top = CalcTop(i),
                    Width = totalWidth - (left + 60),
                    Text = placeholders[i]
                };
                textBoxes[i].KeyDown += ExitIfEscapePressed;
                prompt.Controls.Add(textBoxes[i]);
            }

            textBoxes[0].Select();

            if (prompt.ShowDialog() != DialogResult.OK)
                return null;

            return textBoxes.Select(textBox => textBox.Text).ToArray();
        }
    }
}
