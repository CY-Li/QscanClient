using System.Windows;
using System.Windows.Controls;

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

        public string Size
        {
            get { return (string)GetValue(SizeProperty); }
            set { SetValue(SizeProperty, value); }
        }

        public static readonly DependencyProperty SizeProperty =
            DependencyProperty.Register("Size", typeof(string), typeof(DocCard), new PropertyMetadata("0 KB"));

        public string PreviewType
        {
            get { return (string)GetValue(PreviewTypeProperty); }
            set { SetValue(PreviewTypeProperty, value); }
        }

        public static readonly DependencyProperty PreviewTypeProperty =
            DependencyProperty.Register("PreviewType", typeof(string), typeof(DocCard), new PropertyMetadata("Text"));
    }
}
