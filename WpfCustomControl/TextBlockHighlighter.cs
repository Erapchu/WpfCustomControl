using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace WpfCustomControl
{
    /// <summary>
    /// Using: 
    /// TextBlock 
    /// local:TextBlockHighlighter.Selection="{Binding DataContext.Filter,
    /// local: TextBlockHighlighter.HighlightColor = "LightGreen"
    /// local: TextBlockHighlighter.Forecolor = "Teal"
    /// </summary>
    public static class TextBlockHighlighter
    {
        public static readonly DependencyProperty SelectionProperty = DependencyProperty.RegisterAttached(
            "Selection",
            typeof(string),
            typeof(TextBlockHighlighter),
            new PropertyMetadata(new PropertyChangedCallback(SelectText)));

        public static string GetSelection(DependencyObject obj)
        {
            return (string)obj.GetValue(SelectionProperty);
        }

        public static void SetSelection(DependencyObject obj, string value)
        {
            obj.SetValue(SelectionProperty, value);
        }

        private static void SelectText(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is null)
                return;

            if (!(d is TextBlock textBlock))
                throw new InvalidOperationException("Only valid for TextBlock");

            string text = textBlock.Text;
            if (string.IsNullOrEmpty(text))
                return;

            string highlightText = GetSelection(textBlock);
            if (string.IsNullOrEmpty(highlightText))
                return;

            int index = text.IndexOf(highlightText, StringComparison.CurrentCultureIgnoreCase);
            if (index < 0)
                return;

            Brush selectionColor = GetHighlightColor(d);
            Brush forecolor = GetForecolor(d);

            textBlock.Inlines.Clear();
            while (true)
            {
                textBlock.Inlines.AddRange(new Inline[]
                {
                    new Run(text.Substring(0, index)),
                    new Run(text.Substring(index, highlightText.Length))
                    {
                        Background = selectionColor,
                        Foreground = forecolor
                    }
                });

                text = text.Substring(index + highlightText.Length);
                index = text.IndexOf(highlightText, StringComparison.CurrentCultureIgnoreCase);

                if (index < 0)
                {
                    textBlock.Inlines.Add(new Run(text));
                    break;
                }
            }
        }

        public static readonly DependencyProperty HighlightColorProperty = DependencyProperty.RegisterAttached(
            "HighlightColor",
            typeof(Brush),
            typeof(TextBlockHighlighter),
            new PropertyMetadata(Brushes.Yellow, new PropertyChangedCallback(SelectText)));

        public static Brush GetHighlightColor(DependencyObject obj)
        {
            return (Brush)obj.GetValue(HighlightColorProperty);
        }

        public static void SetHighlightColor(DependencyObject obj, Brush value)
        {
            obj.SetValue(HighlightColorProperty, value);
        }

        public static readonly DependencyProperty ForecolorProperty = DependencyProperty.RegisterAttached(
            "Forecolor",
            typeof(Brush),
            typeof(TextBlockHighlighter),
            new PropertyMetadata(Brushes.Black, new PropertyChangedCallback(SelectText)));

        public static Brush GetForecolor(DependencyObject obj)
        {
            return (Brush)obj.GetValue(ForecolorProperty);
        }

        public static void SetForecolor(DependencyObject obj, Brush value)
        {
            obj.SetValue(ForecolorProperty, value);
        }
    }
}
