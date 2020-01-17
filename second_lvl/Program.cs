using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
namespace second_lvl
{
    class Program
    {
        static void Main(string[] args)
        {
            Form form = new Form();
            form.Width = 800;
            form.Height = 600;
            Game.Init(form);
            form.Show();
            Game.Draw();
            Application.Run(form);

        }

        static class Game
        {
            private static BufferedGraphicsContext _context;
            public static BufferedGraphics Buffer;
            // Свойства
            // Ширина и высота игрового поля
            public static int Width { get; set; }
            public static int Height { get; set; }
            static Game()
            {
            }


            public static BaseObject[] _objs;
         

            public static void Load()
            {
                _objs = new BaseObject[30];
                for (int i = 0; i < _objs.Length / 2; i++)
                    _objs[i] = new BaseObject(new Point(600, i * 20), new Point(-i, -i), new Size(10, 10));
                for (int i = _objs.Length / 2; i < _objs.Length; i++)
                    _objs[i] = new Star(new Point(600, i * 20), new Point(-i, 0), new Size(5, 5));
            }


            public static void Draw()
            {
              

                Buffer.Graphics.Clear(Color.Black);
                foreach (BaseObject obj in _objs)
                    obj.Draw();
                Buffer.Render();
            }

            public static void Update()
            {
                foreach (BaseObject obj in _objs)
                    obj.Update();
            }


            public static void Init(Form form)
            {
                // Графическое устройство для вывода графики            
                Graphics g;
                // Предоставляет доступ к главному буферу графического контекста для текущего приложения
                _context = BufferedGraphicsManager.Current;
                g = form.CreateGraphics();
                // Создаем объект (поверхность рисования) и связываем его с формой
                // Запоминаем размеры формы
                Width = form.ClientSize.Width;
                Height = form.ClientSize.Height;
                // Связываем буфер в памяти с графическим объектом, чтобы рисовать в буфере
                Buffer = _context.Allocate(g, new Rectangle(0, 0, Width, Height));


                Timer timer = new Timer { Interval = 100 };
                timer.Start();
                timer.Tick += Timer_Tick;


                Load();
            }

            private static void Timer_Tick(object sender, EventArgs e)
            {
                Draw();
                Update();
            }
        


        }

        class BaseObject
        {
            protected Point Pos;
            protected Point Dir;
            protected Size Size;
            public BaseObject(Point pos, Point dir, Size size)
            {
                Pos = pos;
                Dir = dir;
                Size = size;
            }


            public virtual void  Draw()
            {
                Game.Buffer.Graphics.DrawEllipse(Pens.White, Pos.X, Pos.Y, Size.Width, Size.Height);
            }
            public virtual void Update()
            {
                Pos.X = Pos.X + Dir.X;
                Pos.Y = Pos.Y + Dir.Y;
                if (Pos.X < 0) Dir.X = -Dir.X;
                if (Pos.X > Game.Width) Dir.X = -Dir.X;
                if (Pos.Y < 0) Dir.Y = -Dir.Y;
                if (Pos.Y > Game.Height) Dir.Y = -Dir.Y;
            }
        }


        class Star : BaseObject
        {
            public Star(Point pos, Point dir, Size size) : base(pos, dir, size)
            {
            }

            public override void Draw()
            {
                Game.Buffer.Graphics.DrawLine(Pens.White, Pos.X, Pos.Y, Pos.X + Size.Width, Pos.Y + Size.Height);
                Game.Buffer.Graphics.DrawLine(Pens.White, Pos.X + Size.Width, Pos.Y, Pos.X, Pos.Y + Size.Height);
            }


            public override void Update()
            {
                Pos.X = Pos.X - Dir.X;
                if (Pos.X> Game.Width) Pos.X = 0;
            }

        }



    }
}
