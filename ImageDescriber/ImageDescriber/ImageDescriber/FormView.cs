using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

using ImageDescriber.ViewModel;

namespace ImageDescriber
{
    public partial class FormView : Form
    {
        private ImageDescriberViewModel ViewModel { get; set; }
        public OpenFileDialog FileDialog { get { return openFileDialog1; } }
        public PictureBox PictureBox { get { return pictureBox1; } }
        
        public FormView()
        {
            ViewModel = new ImageDescriberViewModel(this);
            InitializeComponent();
        }
    }
}