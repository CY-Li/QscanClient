using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QscanClient
{
    public partial class DocCard : UserControl
    {
        public DocCard()
        {
            InitializeComponent();
        }

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(DocCard), new PropertyMetadata("Document"));

        public string Date
        {
            get { return (string)GetValue(DateProperty); }
            set { SetValue(DateProperty, value); }
        }

        public static readonly DependencyProperty DateProperty =
            DependencyProperty.Register("Date", typeof(string), typeof(DocCard), new PropertyMetadata("12/10/2021"));

        public string Subtitle
        {
            get { return (string)GetValue(SubtitleProperty); }
            set { SetValue(SubtitleProperty, value); }
        }

        public static readonly DependencyProperty SubtitleProperty =
            DependencyProperty.Register("Subtitle", typeof(string), typeof(DocCard), new PropertyMetadata("0 pages"));

        public string PreviewType
        {
            get { return (string)GetValue(PreviewTypeProperty); }
            set { SetValue(PreviewTypeProperty, value); }
        }

        public static readonly DependencyProperty PreviewTypeProperty =
            DependencyProperty.Register("PreviewType", typeof(string), typeof(DocCard), new PropertyMetadata("Text"));

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(DocCard));

        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(object), typeof(DocCard));
    }
}
