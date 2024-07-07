using Cyanen;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        string[] args = [];
        public static bool ConsoleEnable;
        public Form1()
        {
            InitializeComponent();
            Thread Background = new Thread(Cyan.Init)
            {
                IsBackground = true,
                Name = "Background"
            };
            Background.Start();

        }

        public Form1(string[] args)
        {
            InitializeComponent();
            this.args = args;
            Commonds LoadRules = new Commonds();
            foreach (string arg in args)
            {
                if (arg == "-Console")
                {
                    ConsoleEnable = true;
                }
            }
            Thread Background = new Thread(Cyan.Init)
            {
                IsBackground = true,
                Name = "Background"
            };
            Background.Start();

        }

        private void sampleControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                System.Environment.Exit(0);
            }
            if (e.KeyCode == Keys.W)
            {
                DataBase.IsKeyDown[e.KeyCode] = true;
            }
            if (e.KeyCode == Keys.S)
            {
                DataBase.IsKeyDown[e.KeyCode] = true;
            }
            if (e.KeyCode == Keys.A)
            {
                DataBase.IsKeyDown[e.KeyCode] = true;
            }
            if (e.KeyCode == Keys.D)
            {
                DataBase.IsKeyDown[e.KeyCode] = true;
            }
            if (e.KeyCode == Keys.Escape)
            {
                DataBase.IsKeyDown[e.KeyCode] = true;
            }
        }

        private void sampleControl_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W)
            {
                DataBase.IsKeyDown[e.KeyCode] = false;
            }
            if (e.KeyCode == Keys.S)
            {
                DataBase.IsKeyDown[e.KeyCode] = false;
            }
            if (e.KeyCode == Keys.A)
            {
                DataBase.IsKeyDown[e.KeyCode] = false;
            }
            if (e.KeyCode == Keys.D)
            {
                DataBase.IsKeyDown[e.KeyCode] = false;
            }
            if (e.KeyCode == Keys.Escape)
            {
                DataBase.IsKeyDown[e.KeyCode] = false;
            }
        }

        private void sampleControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                DataBase.mouseData.IsLeftPress = true;
            }
            if (e.Button == MouseButtons.Right)
            {
                DataBase.mouseData.IsRightPress = true;
            }
        }

        private void sampleControl_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                DataBase.mouseData.IsLeftPress = false;
            }
            if (e.Button == MouseButtons.Right)
            {
                DataBase.mouseData.IsRightPress = false;
            }
        }

        private void sampleControl_MouseMove(object sender, MouseEventArgs e)
        {
            DataBase.mouseData.Offset = new Microsoft.Xna.Framework.Vector2(e.X, e.Y) - DataBase.mouseData.Position;
            DataBase.mouseData.Position = new Microsoft.Xna.Framework.Vector2(e.X, e.Y);
        }
    }

    class Commonds
    {
        struct StartUpCmd
        {

        }
    }
}